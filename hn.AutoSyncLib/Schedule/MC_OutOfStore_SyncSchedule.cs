using System.Threading.Tasks;
using hn.AutoSyncLib.Jobs;
using Quartz;
using Quartz.Impl;

namespace hn.AutoSyncLib.Schedule
{
    public class MC_OutOfStore_SyncSchedule
    {
        private  IScheduler scheduler;
        private IJobDetail job;

        public  void Start(int  hour,int min)
        {
            
            scheduler = new StdSchedulerFactory().GetScheduler().GetAwaiter().GetResult();
            scheduler.Start().GetAwaiter().GetResult();

            job = JobBuilder.Create<MC_OutOfStoreWithClearTable_SyncJob>()
                .WithIdentity("MC_OutOfStore_everyDayJob", "MC_OutOfStore_group1")
                .Build();

            var trigger = TriggerBuilder.Create()
                .StartNow()
                .WithIdentity("trigg", "MC_OutOfStore_group1")
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(hour, min))
                .Build();

             scheduler.ScheduleJob(job, trigger).GetAwaiter().GetResult();

             
        }

        public  async void Stop()
        {
            
           await  scheduler.Shutdown();
        }
    }
}