using Quartz.Spi;
using Quartz;
using IntegrationService.Configuration;
using System.Reflection;

namespace IntegrationService.Quartz
{
    public class QuartzHostedService : IHostedService
    {
        private readonly IScheduler _schedulerFactory;
        private readonly IJobFactory _jobFactory;
        private readonly Configurations _configuration;
        private readonly ILogger<QuartzHostedService> _logger;

        public QuartzHostedService(
            IScheduler schedulerFactory,
            IJobFactory jobFactory,
            Configurations configuration,
            ILogger<QuartzHostedService> logger)
        {
            _schedulerFactory = schedulerFactory;
            _jobFactory = jobFactory;
            _configuration = configuration;
            _logger = logger;   
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (_configuration.EnabledQuartz.Equals(false))
                {
                    await StopAsync(cancellationToken);
                }
                _schedulerFactory.JobFactory = _jobFactory;
                var jobs = _configuration.Jobs;
                List<Type> types = Assembly.GetExecutingAssembly().GetTypes()
                    .Where(t => jobs.Any(x => x.Name == t.Name
                                && (x.Expression != null && x.Expression != string.Empty)))
                                    .ToList();

                var jobsTypes = types.Join(jobs,
                    types => types.Name,
                    jobs => jobs.Name,
                    (type, job) => new
                    {
                        Job = job,
                        Type = type
                    });

                foreach (var jobSchedule in jobsTypes)
                {
                    var type = Type.GetType(jobSchedule.Type.FullName ?? throw new ArgumentNullException(nameof(QuartzHostedService)));
                    var schedule = new JobSchedule(type ?? throw new ArgumentNullException(nameof(QuartzHostedService)), jobSchedule.Job.Expression ?? throw new ArgumentNullException(nameof(QuartzHostedService)));
                    var job = CreateJob(schedule);
                    var trigger = CreateTrigger(schedule);
                    if (!await _schedulerFactory.CheckExists(job.Key))
                    {
                        await _schedulerFactory.ScheduleJob(job, trigger);
                    }
                    else if ((!(await _schedulerFactory.GetTrigger(trigger.Key))?.Key.Equals(trigger.Key) ?? true
                        && await _schedulerFactory.CheckExists(job.Key)
                        ))
                    {
                        await _schedulerFactory.DeleteJob(job.Key);
                        await _schedulerFactory.ScheduleJob(job, trigger);
                    }
                }

                await _schedulerFactory.Start(cancellationToken);
            }
            catch (Exception ex)
            {
                if (_configuration.EnabledQuartz.Equals(false))
                {
                    _logger.LogWarning(ex, "Quartz is disabled");
                }
                else
                {
                    _logger.LogError(ex, "QuartzHostedService");
                }
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_schedulerFactory != null) await _schedulerFactory.Shutdown(cancellationToken);
        }

        private static IJobDetail CreateJob(JobSchedule schedule)
        {
            return JobBuilder
                .Create(schedule.JobType)
                .WithIdentity(schedule.JobType.FullName ?? throw new ArgumentNullException(nameof(QuartzHostedService)))
                .WithDescription(schedule.JobType.Name)
                .Build();
        }

        private static ITrigger CreateTrigger(JobSchedule schedule)
        {
            return TriggerBuilder
                .Create()
                .WithIdentity($"{schedule.JobType.FullName}.trigger")
                .WithCronSchedule(schedule.CronExpression)
                .WithDescription(schedule.CronExpression)
                .Build();
        }
    }
    
}
