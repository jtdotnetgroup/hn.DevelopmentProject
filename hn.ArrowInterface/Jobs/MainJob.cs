using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using hn.ArrowInterface.Entities;
using hn.AutoSyncLib.Common;
using Quartz;

namespace hn.ArrowInterface.Jobs
{
    /// <summary>
    /// 主同步作业，作为程序入口，调用其它同步接口
    /// </summary>
    public class MainJob : IJob
    {
        private OracleDBHelper Helper { get; set; }

        public MainJob()
        {
            string conStr = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
            this.Helper = new OracleDBHelper(conStr);
        }

        public void Execute(IJobExecutionContext context)
        {
            //反射当前程序集信息
            var assembly = Assembly.GetExecutingAssembly();
            //反射获取当前程序信中，所有实现了ISyncJob接口的类
            var jobsTypes = assembly.GetTypes().Where(p => typeof(ISyncJob).IsAssignableFrom(p));
            //获取所有接口的同步记录
            List<SyncJob_Definition> job_definitions = Helper.GetAll<SyncJob_Definition>();

            foreach (var t in jobsTypes.AsParallel())
            {
                string jobName = t.Name;
                //接口间隔定义值，于配置文件中定义
                var interval = Convert.ToInt32(ConfigurationManager.AppSettings.Get(jobName));

                var jobRecord = job_definitions.FirstOrDefault(p => p.JobClassName == jobName);

                var timespan = jobRecord == null ? 0 : (DateTime.Now - jobRecord.LastExecute).TotalMinutes;

                //如果不存在同步记录，说明还没有同步过
                //比较当前时间与最后同步时间是否达到同步间隔
                if (jobRecord != null && timespan < interval)
                {
                    //未达到同步间隔，执行同步操作
                    continue;
                }

                //从未同步过或已达到同步时间，执行同步
                if (SyncJob(t))
                {
                    if (jobRecord == null)
                    {
                        jobRecord = new SyncJob_Definition();
                        jobRecord.JobClassName = jobName;
                        jobRecord.LastExecute = DateTime.Now;

                        Helper.Insert(jobRecord);
                        continue;
                    }

                    jobRecord.LastExecute = DateTime.Now;
                    //更新同步记录
                    
                }
            }
        }

        private bool SyncJob(Type jobType)
        {
            var method = jobType.GetMethod("Sync");

            var instance = Activator.CreateInstance(jobType);

            return Convert.ToBoolean(method.Invoke(instance, null));
        }
    }
}