using Microsoft.EntityFrameworkCore;
using shift_dashboard.Model;

namespace shift_dashboard.Data
{
    public class DashboardContext : DbContext
    {
        public DbSet<Delegate> Delegates { get; set; }
        //public DbSet<Account> Accounts { get; set; }
        public DbSet<DelegateStat> DelegateStats { get; set; }

        public DashboardContext(DbContextOptions<DashboardContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}