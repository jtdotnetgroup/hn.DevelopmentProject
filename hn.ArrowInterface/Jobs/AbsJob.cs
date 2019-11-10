using System;
using System.Configuration;
using System.Linq; 
using hn.ArrowInterface.Entities;
using hn.ArrowInterface.WebCommon;
using hn.Common;
using Newtonsoft.Json;
using Quartz;

namespace hn.ArrowInterface.Jobs
{
    /// <summary>
    /// 通用同步作业类
    /// </summary>
    public abstract class AbsJob:ISyncJob,IJob
    {
        protected ArrowInterface Interface { get; set; }
        protected  OracleDBHelper Helper { get; set; }
        protected int Interval { get; set; }
        protected string JobName { get; set; }
        protected string DealerCode = ConfigurationManager.AppSettings["dealerCode"];
        protected string DateTimeFormat { get; set; }

        /// <summary>
        /// 获取请求参数的抽象方法，由子类实现
        /// </summary>
        /// <returns></returns>
        protected abstract AbstractRequestParams GetParams();

        protected  bool UpdateSyncRecord(AbstractRequestParams pars)
        {
            //查历史同步记录
            var jobRecord = Helper.GetWhere<SyncJob_Definition>(new SyncJob_Definition() { JobClassName = this.JobName }).FirstOrDefault();
            //同步记录更新标记
            bool isUpdate = true;
            if (jobRecord == null)
            {
                //同步记录不存在，新建记录
                isUpdate = false;
                jobRecord = new SyncJob_Definition();
                jobRecord.JobClassName = this.JobName;
            }
            jobRecord.LastExecute = DateTime.Now;
            jobRecord.ParsJson = JsonConvert.SerializeObject(pars);
            //更新标记为true时更新同步记录，为false插入新的同步记录
            var result = isUpdate ? this.Helper.Update(jobRecord) : this.Helper.Insert(jobRecord);

            return result;
        }

        public AbsJob()
        {
            string conStr = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
            Helper=new OracleDBHelper(conStr);

            Interface=new ArrowInterface();

            this.JobName = this.GetType().Name;

            //接口间隔定义值，于配置文件中定义
            this.Interval = Convert.ToInt32(ConfigurationManager.AppSettings.Get(JobName));
            DateTimeFormat = ConfigurationManager.AppSettings["DateTimeFormat"];
        }

        public AuthorizationToken GetToken()
        {
            //请求Token
            return CommonToken.GetToken();
        }

        public abstract bool Sync();

        public void Execute(IJobExecutionContext context)
        {
            var where=new SyncJob_Definition();
            where.JobClassName = this.JobName;
            var jobRecord = Helper.GetWhere(where).SingleOrDefault();

            if (jobRecord!=null&&jobRecord.LastExecute != null && (jobRecord != null&&( DateTime.Now-jobRecord.LastExecute.Value).TotalMinutes<Interval))
            {
                //未达到设定同步间隔
                LogHelper.Info($"【{this.JobName}】同步时间未到");
                return;
            }

            LogHelper.Info($"作业开始：【{this.JobName}】");
            var start = DateTime.Now;
            Sync();
            var end = DateTime.Now - start;
            LogHelper.Info($"作业结束：【{this.JobName}】，耗时【{end.TotalMilliseconds}】毫秒");
        }


    }
}