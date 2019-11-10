using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
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
            pars.attr1 = ConfigurationManager.AppSettings["dealerCode"];
            if (jobRecord == null)
            {
                pars.attr2 = "2019-06-02 10:28:54";

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
                var conn = Helper.GetNewConnection();
                conn.Open();
                var tran = conn.BeginTransaction();
                try
                {
                   
                    //批量插入数据，单次插入量，此值不宜过大，太大反而会降低效率，最佳值在100-500
                    int size = 100;
                    //插入次数，用数据总条数除以单次插入量得出
                    var insertTime = result.Rows.Count / size;
                    //如数据总条数对单次插入量求余结果大于0，则插入次数+1
                    insertTime += result.Rows.Count % size > 0 ? 1 : 0;

                    for (int i = 0; i < insertTime; i++)
                    {
                        //取第 i 次要插入的数据，跳过第 i*size 条数据，取size条插入
                        var insertData = result.Rows.Skip((i) * size).Take(size).ToList();
                        string where = "AND orderId in (";
                        StringBuilder builder = new StringBuilder(where);
                        insertData.ForEach(d =>
                        {
                            builder.Append($"'{d.orderId}'");
                            builder.Append(insertData.Last() == d ? ")" : ",");
                        });

                        where = builder.ToString();
                        Helper.DeleteWithTran<SaleOrder>(where, tran);
                        Helper.DeleteWithTran<SaleOrderDetailed>(where, tran);
                        //批量插入表头
                        Helper.BatchInsert(insertData, tran);

                        var details = new List<SaleOrderDetailed>();
                        //拿所所有明细数据
                        var lists = insertData.Select(header => header.saleOrderItemList).ToList();
                        //由于每一个SaleOrder对象的saleOrderItemList是一个数组，需要转换成一个List集合进行批量插入
                        lists.ForEach(l =>
                        {
                            var li = l.ToList();
                            var tmp= details.Union(li).ToList();
                            details = tmp;
                        });

                        Helper.BatchInsert(details, tran);
                    }
                    tran.Commit();
                    
                    UpdateSyncRecord(pars);
                }
                catch (Exception e)
                {
                    tran.Rollback();
                    tran.Connection.Close();
                    string message = $"定制订单&常规工程订单&计划工程订单插入失败：{e.Message}";
                    LogHelper.Error(message);
                    LogHelper.Error(e);
                }

                return true;
            }

            return false;
        }
    }
}
