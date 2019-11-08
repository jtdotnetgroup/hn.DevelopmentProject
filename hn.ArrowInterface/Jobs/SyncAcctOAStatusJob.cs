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
    [Obsolete("不用定时执行")]
    public class SyncAcctOAStatusJob : AbsJob
    {
        protected override AbstractRequestParams GetParams()
        {
            //查历史同步记录
            var jobRecord = Helper.GetWhere<SyncJob_Definition>(new SyncJob_Definition() { JobClassName = this.JobName }).FirstOrDefault();

            var pars = new AcctOAStatusParam();
            pars.acctCode = ConfigurationManager.AppSettings["dealerCode"];
            if (jobRecord == null)
            { 

                jobRecord = new SyncJob_Definition();
                jobRecord.JobClassName = this.JobName;
                jobRecord.LastExecute = DateTime.Now;
            }  
            return pars;
        }

        public override bool Sync()
        { 

            var token = GetToken();
            //请求参数
            var pars = GetParams() as AcctOAStatusParam; 

            var result = Interface.AcctOaStatus(token.Token,pars);

            if (result.Success)
            {
                foreach (var row in result.Rows)
                {
                    try
                    { 
                        Helper.Insert(row);
                        foreach (var item in row.saleOrderItemList) {
                            item.orderNo = row.orderNo;
                            Helper.Insert(item);
                        }
                    }
                    catch (Exception e)
                    {
                        string message = string.Format("OA审核状态回传：{0}", JsonConvert.SerializeObject(row));
                        LogHelper.Info(message);
                        LogHelper.Error(e);
                    }
                }

                return true;
            }

            return false;
        }
    } 
}
