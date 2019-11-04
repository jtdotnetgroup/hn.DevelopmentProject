using hn.AutoSyncLib.Jobs;
using Quartz;
using Quartz.Impl;

namespace hn.AutoSyncLib.Schedule
{
    public class MC_PickUpGoods_SyncSchedule
    {
        private IScheduler scheduler;

        public void Start(int hour, int min)
        {

            scheduler = new StdSchedulerFactory().GetScheduler().GetAwaiter().GetResult();
            scheduler.Start().GetAwaiter().GetResult();

            var job1 = JobBuilder.Create<MC_PickUpGoodsWithClearTable_SyncJob>()
                .WithIdentity("MC_PickUpGoods", "MC_PickUpGoods_group1")
                .Build();

            


            var trigger = TriggerBuilder.Create()
                .StartNow()
                .WithIdentity("trigg", "group1")
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(hour, min))
                .Build();

            scheduler.ScheduleJob(job1, trigger).GetAwaiter().GetResult();


        }

        public async void Stop()
        {
            await scheduler.Shutdown();
        }
    }
}