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
    public class SyncSaleOrderJob : AbsJob
    {
        protected override AbstractRequestParams GetParams()
        {

            //查历史同步记录
            var jobRecord = Helper.GetWhere<SyncJob_Definition>(new SyncJob_Definition() { JobClassName = this.JobName }).FirstOrDefault();

            var pars = new LH_SaleOrderParam();
            pars.attr1 = DealerCode;
            if (jobRecord == null)
            {
                pars.attr2 = "2019-10-12 10:28:54";

                jobRecord = new SyncJob_Definition();
                jobRecord.JobClassName = this.JobName;
                jobRecord.LastExecute = DateTime.Now;
            }
            else
            {
                var attrs = JsonConvert.DeserializeAnonymousType(jobRecord.ParsJson,
                    new { attr1 = "", attr2 = "", attr3 = "" });
                //如果已存在同步历史，取上一次同步参数的结束时间再往前5分钟作为本次同步的开始时间
                pars.attr2 = DateTime.Parse(attrs.attr3).AddMinutes(-5).ToString(DateTimeFormat);
            }

            pars.attr3 = DateTime.Now.ToString(DateTimeFormat); 

            return pars;
        }

        public override bool Sync()
        {
            var token = GetToken();

            var pars = GetParams() as LH_SaleOrderParam;

            var result = Interface.SaleOrder(token.Token,pars);

            if (result.Success)
            {
                foreach (var row in result.Rows)
                {
                    try
                    {
                        Helper.Delete<SaleOrder>(row.KeyId());
                        Helper.Delete<SaleOrderDetailed>(row.KeyId());
                        Helper.Insert(row);
                        foreach (var item in row.saleOrderItemList) {
                            item.orderId = row.Id;
                            Helper.Insert(item);
                        }
                    }
                    catch (Exception e)
                    {
                        string message = $"定制订单&常规工程订单&计划工程订单下载失败：{JsonConvert.SerializeObject(row)}";
                        LogHelper.Info(message);
                        LogHelper.Error(e);
                    }
                }

                UpdateSyncRecord(pars);

                return true;
            }

            return false;
        }
    }
}
