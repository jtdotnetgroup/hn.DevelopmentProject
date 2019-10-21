using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using hn.AutoSyncLib.Jobs;
using hn.AutoSyncLib.Model;
using log4net;
using log4net.Core;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;

namespace hn.AutoSyncLib.Schedule
{
    public class MC_PickUpGoods_Schedule
    {
        public static async  Task Start()
        {
            ISchedulerFactory factory=new StdSchedulerFactory();
            var scheduler = factory.GetScheduler().Result;

           await scheduler.Start();

            var job = JobBuilder.Create<MC_PickUpGoods_SyncJob>()
                .WithIdentity("identtiy","group2")
                .Build();

            var trigger = TriggerBuilder.Create()
                .StartNow()
                .WithIdentity("trigg","group2")
                .WithSimpleSchedule(b => b.WithIntervalInHours(1).RepeatForever())
                .Build();



            await scheduler.ScheduleJob(job, trigger);

        }

        public static async Task SyncData<T>(string rq1, string rq2,string identity="identity",string group="PickUpGoods")
        where T:IJob
        {
            ISchedulerFactory factory=new StdSchedulerFactory();
            var schheduler = factory.GetScheduler().Result;

            await schheduler.Start();

            var job = JobBuilder.Create<T>()
                .WithIdentity(identity, group)
                .UsingJobData("rq1",rq1)
                .UsingJobData("rq2",rq2)
                .Build();

            var trigger = TriggerBuilder.Create()
                .StartNow()
                .WithIdentity("PickUpGoodstrigger", group)
                .Build();

           await schheduler.ScheduleJob(job, trigger);

        }

        
    }
}