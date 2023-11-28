using IntegrationService.Quartz.ProcessMethods;
using Microsoft.DotNet.Scaffolding.Shared.ProjectModel;
using Quartz;
using Quartz.Impl.AdoJobStore.Common;

namespace IntegrationService.Quartz.Jobs
{
    public class ImportJob : IJob
    {
        private readonly ILogger<Job> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ImportJob(
           ILogger<Job> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public Task Execute(IJobExecutionContext context)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedDependency = scope.ServiceProvider.GetRequiredService<ScheduleProcess>();

                scopedDependency.Execute();

                // Use the scoped dependency
                // ...
            }

            _logger.LogInformation($"IMPORT | ScheduleId: Test, Date: {DateTime.UtcNow}");
            return Task.CompletedTask;
        }
    }
}
