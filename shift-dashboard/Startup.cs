using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using Quartz;
using shift_dashboard.Data;
using shift_dashboard.Jobs;
using shift_dashboard.Models;
using shift_dashboard.Services;
using System;

namespace shift_dashboard
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Bind the Appsettings.json to shiftDashboardConfig
            DashboardConfig shiftDashboardConfig = new DashboardConfig();
            Configuration.GetSection(shiftDashboardConfig.Position).Bind(shiftDashboardConfig);
            services.AddSingleton<DashboardConfig>(shiftDashboardConfig);


            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });


            // Initialize DB Context
            services.AddDbContext<DashboardContext>(options =>
            {
                var builder = new NpgsqlDbContextOptionsBuilder(options);
                builder.SetPostgresVersion(new Version(9, 6));
                options.UseNpgsql(shiftDashboardConfig.ConnectionString);
            }
            );

            // Shift Api Service (Need a DB Context
            services.AddTransient<IApiService, ApiService>();

            // Schedule Tasks.
            services.AddQuartz(q =>
            {
                // Create a "key" for the job
                var updateDelegateJobKey = new JobKey("UpdateDelegateJob");

                // Register the job with the DI container
                q.AddJob<UpdateDelegateJob>(opts => opts.WithIdentity(updateDelegateJobKey));

                // Create a trigger for the job
                //
                q.AddTrigger(opts => opts
                .ForJob(updateDelegateJobKey)
                .WithIdentity("UpdateDelegateJob-trigger") // give the trigger a unique name
                .WithCronSchedule("0 */45 * * * ?")); ; // run every 45 minutes

                // Use a Scoped container to create jobs. I'll touch on this later
                q.UseMicrosoftDependencyInjectionScopedJobFactory();
            });

            // Add the Quartz.NET hosted service
            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
            services.AddControllersWithViews();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            
            // Use Forwarded Header to keep track of client info.
            app.UseForwardedHeaders();


            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}