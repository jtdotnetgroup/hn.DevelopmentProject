using System;
using System.Configuration;
using System.Linq;
using hn.ArrowInterface.Entities;
using hn.ArrowInterface.RequestParams;
using hn.ArrowInterface.WebCommon;
using hn.Common;
using Newtonsoft.Json;

namespace hn.ArrowInterface.Jobs
{
    public class SyncPolicyJob:AbsJob
    {
        public override bool Sync()
        {
            var token = GetToken();
            //拿请求参数
            var pars = GetParams() as QueryPolicyParam;
            var result = Interface.QueryPolicy(token.Token, pars);
            if (result.Success)
            {
                Helper.Delete<QueryPolicy>("");

                foreach (var row in result.Rows.AsParallel())
                {
                    try
                    {
                        Helper.Insert(row);
                    }
                    catch (Exception e)
                    {
                        string msg = string.Format("插入政策记录失败：{0}", JsonConvert.SerializeObject(row));
                        LogHelper.Info(msg);
                        LogHelper.Error(e);
                    }
                }
                //同步完成，更新请求记录
                UpdateSyncRecord(pars);
            }
           
            return result.Success;
        }

        protected override AbstractRequestParams GetParams()
        {
            //查历史同步记录
            var jobRecord = Helper.GetWhere<SyncJob_Definition>(new SyncJob_Definition() { JobClassName = this.JobName }).FirstOrDefault();

            var pars = new QueryPolicyParam();
            pars.acctKey = ConfigurationManager.AppSettings["dealerCode"];
            if (jobRecord == null)
            { 

                jobRecord = new SyncJob_Definition();
                jobRecord.JobClassName = this.JobName;
                jobRecord.LastExecute = DateTime.Now;
            } 
             

            return pars;
        }
    }
}