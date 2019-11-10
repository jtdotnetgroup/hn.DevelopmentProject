using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;
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
            string conStr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            this.Helper = new OracleDBHelper(conStr);
            Mapper.Initialize(new MapperConfigurationExpression());
        }

        public  void DoWork()
        {
            //反射当前程序集信息
            var assembly = Assembly.GetExecutingAssembly();
            //反射获取当前程序集中，所有实现了ISyncJob接口,并且没有标注Obsolete特性的类
            var jobsTypes = assembly.GetTypes().Where(p => typeof(AbsJob).IsAssignableFrom(p)&&p.Name!=typeof(AbsJob).Name&&p.GetCustomAttributes(true).Count(c=>c is ObsoleteAttribute)==0);
            //获取所有接口的同步记录
            ISchedulerFactory factory = new StdSchedulerFactory();

            foreach (var t in jobsTypes)
            {
                string jobName = t.Name;

                IScheduler scheduler = factory.GetScheduler();
                scheduler.Start();

                var job = JobBuilder.Create(t)
                    .WithIdentity(jobName, jobName)
                    .Build();

                var trigger = TriggerBuilder.Create()
                    .StartNow()
                    .WithIdentity("trigg_"+jobName, jobName)
                    .WithSimpleSchedule(b => b.WithIntervalInMinutes(10).RepeatForever())
                    .Build();

                scheduler.ScheduleJob(job, trigger);

            }

        }

        
    }
}