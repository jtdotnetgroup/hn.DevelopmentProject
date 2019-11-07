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
    public class SyncProductJob: AbsJob
    {
        //实现获取请求参数方法
        protected override AbstractRequestParams GetParams()
        {
            //查历史同步记录
            var jobRecord = Helper.GetWhere<SyncJob_Definition>(new SyncJob_Definition() { JobClassName = this.JobName }).FirstOrDefault();

            var pars = new LH_ProductParam();
            pars.attr1 = ConfigurationManager.AppSettings["dealerCode"];
            if (jobRecord == null)
            {
                pars.attr1 = "2019-01-02 10:28:54";

                jobRecord = new SyncJob_Definition();
                jobRecord.JobClassName = this.JobName;
                jobRecord.LastExecute = DateTime.Now;
            }
            else
            {
                var attrs = JsonConvert.DeserializeAnonymousType(jobRecord.ParasJSON,
                    new { attr1 = "", attr2 = "", attr3 = "" });
                //如果已存在同步历史，取上一次同步参数的结束时间再往前5分钟作为本次同步的开始时间
                pars.attr1 = DateTime.Parse(attrs.attr3).AddMinutes(-5).ToString(DateTimeFormat);
            }

            pars.attr2 = DateTime.Now.ToString(DateTimeFormat);

            return pars;
        }

        

        public override bool Sync()
        {
            var token = GetToken();

            //拿请求参数
            var pars = GetParams() as LH_ProductParam;
           
            var result = Interface.QueryProdPage(token.Token,pars);

            if (result.Success)
            {

                foreach (var row in result.Rows.AsParallel())
                {
                    try
                    {
                        Helper.Insert(row);
                    }
                    catch (Exception e)
                    {
                        string message = string.Format("物料记录插入失败：{0}", JsonConvert.SerializeObject(row));
                        LogHelper.Info(message);
                        LogHelper.Error(e);
                    }
                }

                //同步完成，更新请求记录
                UpdateSyncRecord(pars);
               
                return true;
            }

            return false;
        }

    }
}