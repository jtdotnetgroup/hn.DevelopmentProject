using System;
using System.Configuration;
using System.Linq; 
using hn.ArrowInterface.Entities;
using hn.Common;
using Quartz;

namespace hn.ArrowInterface.Jobs
{
    public abstract class AbsJob:ISyncJob,IJob
    {
        protected ArrowInterface Interface { get; set; }
        protected  OracleDBHelper Helper { get; set; }
        protected int Interval { get; set; }
        protected string JobName { get; set; }

        public AbsJob()
        {
            string conStr = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
            Helper=new OracleDBHelper(conStr);

            Interface=new ArrowInterface();

            this.JobName = this.GetType().Name;

            //接口间隔定义值，于配置文件中定义
            this.Interval = Convert.ToInt32(ConfigurationManager.AppSettings.Get(JobName));

        }

        public AuthorizationToken GetToken()
        {
            //从数据库读取Token值
            var token=Helper.GetAll<AuthorizationToken>().FirstOrDefault();
            if (token != null)
            {
                //判断TOKEN是否过期
                if (token.ExpiredTime <= DateTime.Now)
                {
                    //过期重新获取Token并更新数据库值
                    var newtoken = Interface.GetToken();
                    newtoken.ExpiredTime=DateTime.Now.AddHours(2);
                    //token.Token = newtoken.Token;
                    Helper.Update(newtoken, string.Format(" AND TokenValue='{0}'",token.Token));
                }

                return token;
            }
            //数据库不存在Token，获取并插入
            token = Interface.GetToken();
            token.ExpiredTime = DateTime.Now.AddHours(2);

            Helper.Insert(token);

            return token;
        }

        public abstract bool Sync();

        public void Execute(IJobExecutionContext context)
        {
            var where=new SyncJob_Definition();
            where.JobClassName = this.JobName;
            var jobRecord = Helper.GetWhere(where).SingleOrDefault();

            if (jobRecord != null&&( DateTime.Now-jobRecord.LastExecute).TotalMinutes<Interval)
            {
                //未达到设定同步间隔
                return;
            }

            if (Sync())
            {
                if (jobRecord == null)
                {
                    jobRecord = new SyncJob_Definition() { JobClassName = JobName,LastExecute = DateTime.Now};

                    Helper.Insert(jobRecord);
                    return;
                }

                jobRecord.LastExecute=DateTime.Now;

                Helper.Update(jobRecord);
            }
        }


    }
}