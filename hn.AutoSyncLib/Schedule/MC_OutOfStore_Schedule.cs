using System;
using System.Configuration;
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
       static ISchedulerFactory factory = new StdSchedulerFactory();

       /// <summary>
       /// 此方法为每天12：30运行一次，清空全表
       /// </summary>
       /// <returns></returns>
        public static async  Task StartEveryDayTask()
       {

           var actionTime = ConfigurationManager.AppSettings["MC_OutOfStore"];
           var hour =Convert.ToInt32( actionTime.Split(':')[0]);
           var min =Convert.ToInt32( actionTime.Split(':')[1]);

            var scheduler = factory.GetScheduler().Result;

           await scheduler.Start();

            var job1 = JobBuilder.Create<MC_OutOfStoreWithClearTable_SyncJob>()
                .WithIdentity("MC_OutOfStore_everyDayJob", "MC_OutOfStore_group1")
                .Build();


            var trigger = TriggerBuilder.Create()
                .StartNow()
                .WithIdentity("trigg", "MC_OutOfStore_group1")
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(hour, min))
                .Build();

            await scheduler.ScheduleJob(job1, trigger);
        }

        /// <summary>
        /// 此方法为每小时同步一次，不清全表
        /// </summary>
        /// <returns></returns>
        public static async Task StartTodaySync()
        {
            IScheduler scheduler = factory.GetScheduler().Result;

            await scheduler.Start();

            var job = JobBuilder.Create<MC_OutOfStore_SyncJob>()
                .WithIdentity("outofStore_todayJob", "MC_OutOfStore_group1")
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("OutOfStore_trigger", "MC_OutOfStore_group1")
                .WithSimpleSchedule(b => b.WithIntervalInHours(1))
                .Build();

            await scheduler.ScheduleJob(job, trigger);

        }
    }
}