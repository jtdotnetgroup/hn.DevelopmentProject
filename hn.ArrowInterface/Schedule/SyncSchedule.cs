using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using hn.ArrowInterface.Entities;
using hn.ArrowInterface.Jobs;
using hn.Common;
using Quartz;
using Quartz.Impl;

namespace hn.ArrowInterface.Schedule
{
    public  class SyncSchedule
    {
        private OracleDBHelper Helper { get; set; }

        public SyncSchedule()
        {
            string conStr = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            this.Helper = new OracleDBHelper(conStr);
        }

        public  void DoWork()
        {
            //反射当前程序集信息
            var assembly = Assembly.GetExecutingAssembly();
            //反射获取当前程序信中，所有实现了ISyncJob接口的类
            var jobsTypes = assembly.GetTypes().Where(p => typeof(ISyncJob).IsAssignableFrom(p));
            //获取所有接口的同步记录
            List<SyncJob_Definition> job_definitions = Helper.GetAll<SyncJob_Definition>();
            ISchedulerFactory factory = new StdSchedulerFactory();

            foreach (var t in jobsTypes)
            {
                string jobName = t.Name;
                //接口间隔定义值，于配置文件中定义
                var interval = Convert.ToInt32(ConfigurationManager.AppSettings.Get(jobName));

                IScheduler scheduler = factory.GetScheduler();
                scheduler.Start();

                var job = JobBuilder.Create(t)
                    .WithIdentity(jobName, jobName)
                    .Build();

                var trigger = TriggerBuilder.Create()
                    .StartNow()
                    .WithIdentity("trigg_"+jobName, jobName)
                    .WithSimpleSchedule(b => b.WithIntervalInMinutes(5).RepeatForever())
                    .Build();

                scheduler.ScheduleJob(job, trigger);

            }

        }

        
    }
}