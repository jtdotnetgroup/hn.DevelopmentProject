using System.Threading.Tasks;
using hn.AutoSyncLib.Jobs;
using hn.AutoSyncLib.Model;
using log4net;
using log4net.Core;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;

namespace hn.AutoSyncLib.Schedule
{
    public class MC_OutOfStore_Schedule
    {

        public static async  Task Start()
        {
            ISchedulerFactory factory=new StdSchedulerFactory();
            var scheduler = factory.GetScheduler().Result;

           await scheduler.Start();

            var job1 = JobBuilder.Create<MC_OutOfStore_SyncJob>()
                .WithIdentity("outofStore","group1")
                .Build();



            var trigger = TriggerBuilder.Create()
                .StartNow()
                .WithIdentity("trigg","group1")
                .WithSimpleSchedule(b => b.WithIntervalInHours(1).RepeatForever())
                .Build();

            await scheduler.ScheduleJob(job1, trigger);

            

        }
    }
}