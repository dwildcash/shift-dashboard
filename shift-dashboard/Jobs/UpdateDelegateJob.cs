using Microsoft.Extensions.Logging;
using Quartz;
using shift_dashboard.Services;
using System;
using System.Threading.Tasks;

namespace shift_dashboard.Jobs
{
    [DisallowConcurrentExecution]
    public class UpdateDelegateJob : IJob
    {
        private readonly ILogger<UpdateDelegateJob> _logger;
        private IApiService _apiService;

        public UpdateDelegateJob(ILogger<UpdateDelegateJob> logger, IApiService apiService)
        {
            _logger = logger;
            _apiService = apiService;
        }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                Task e = _apiService.UpdateDelegateDb();
                e.Wait();

                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException.ToString());
                return null;
            }
        }
    }
}