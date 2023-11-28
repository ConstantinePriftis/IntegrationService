using Quartz;
using System;

namespace IntegrationService.Quartz.Jobs
{
    [DisallowConcurrentExecution]
    public class Job : IJob
    {
        private readonly ILogger<Job> _logger;
        public Job(
           ILogger<Job> logger)
        {
            _logger = logger;
        }
        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation($"Job | ScheduleId: Test, Date: {DateTime.UtcNow}");
            return Task.CompletedTask;
        }
    }
}
