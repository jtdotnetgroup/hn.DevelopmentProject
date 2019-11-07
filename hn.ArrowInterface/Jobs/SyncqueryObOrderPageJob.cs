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
    public class SyncqueryObOrderPageJob : AbsJob
    {
        public override bool Sync()
        {
            var token = GetToken();
            //拿请求参数
            var pars = GetParams() as QueryObOrderPageParam;
            var result = Interface.queryObOrderPage(token.Token, pars);

            if (result.Success)
            {
                foreach (var row in result.Rows)
                {
                    try
                    {
                        Helper.Delete<OutOrder>(row.KeyId());
                        Helper.Delete<OutOrderDetailed>(row.KeyId());
                        Helper.Insert(row);
                        if (row.items != null) { 
                            foreach (var item in row.items)
                            {
                                Helper.Insert(item);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        string message = string.Format("出库单下载失败：{0}", JsonConvert.SerializeObject(row));
                        LogHelper.Info(message);
                        LogHelper.Error(e);
                    }
                }

                return true;
            }

            return false;
        }

        protected override AbstractRequestParams GetParams()
        {
            //查历史同步记录
            var jobRecord = Helper.GetWhere<SyncJob_Definition>(new SyncJob_Definition() { JobClassName = this.JobName }).FirstOrDefault();

            var pars = new QueryObOrderPageParam();
            pars.dealerCode = ConfigurationManager.AppSettings["dealerCode"];
            if (jobRecord == null)
            {
                pars.attr2 = "2019-01-02 10:28:54";

                jobRecord = new SyncJob_Definition();
                jobRecord.JobClassName = this.JobName;
                jobRecord.LastExecute = DateTime.Now;
            }
            else
            {
                var attrs = JsonConvert.DeserializeAnonymousType(jobRecord.ParasJSON,
                    new { attr1 = "", attr2 = "", attr3 = "" });
                //如果已存在同步历史，取上一次同步参数的结束时间再往前5分钟作为本次同步的开始时间
                pars.attr2 = DateTime.Parse(attrs.attr3).AddMinutes(-5).ToString(DateTimeFormat);
            }

            pars.attr3 = DateTime.Now.ToString(DateTimeFormat);

            return pars;
        }
    }
}
