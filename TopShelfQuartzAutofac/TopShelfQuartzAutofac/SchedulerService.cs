using Quartz;

namespace TopShelfQuartzAutofac
{
    public class SchedulerService
    {
        private readonly IScheduler _scheduler;

        public SchedulerService(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public void Start()
        {
            ScheduleJobs();
            _scheduler.Start().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private void ScheduleJobs()
        {
            ScheduleJobWithCronSchedule<MySyncJob>("");
            ScheduleJobWithCronSchedule<MyOtherSyncJob>("");
        }

        private void ScheduleJobWithCronSchedule<T>(string cronShedule) where T : IJob
        {
            var jobName = typeof(T).Name;
            var job = JobBuilder
                .Create<T>()
                .WithIdentity(jobName, $"{jobName}-Group")
                .Build();

            var cronTrigger = TriggerBuilder
                .Create()
                .WithIdentity($"{jobName}-Trigger")
                .StartNow()
                .WithCronSchedule(cronShedule)
                .ForJob(job)
                .Build();

            _scheduler.ScheduleJob(cronTrigger);
        }

        public void Stop()
        {
            _scheduler.Shutdown().ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
