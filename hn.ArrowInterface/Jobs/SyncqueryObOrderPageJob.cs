using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
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

                var conn = Helper.GetNewConnection();
                conn.Open();
                var tran = conn.BeginTransaction();

                //批量插入数据，单次插入量，此值不宜过大，太大反而会降低效率，最佳值在100-500
                int size = 100;
                //插入次数，用数据总条数除以单次插入量得出
                var insertTime = result.Rows.Count / size;
                //如数据总条数对单次插入量求余结果大于0，则插入次数+1
                insertTime += result.Rows.Count % size > 0 ? 1 : 0;

                for (int i = 0; i < insertTime; i++)
                {
                    try
                    {
                        //取第 i 次要插入的数据，跳过第 i*size 条数据，取size条插入
                        var insertData = result.Rows.Skip((i) * size).Take(size).ToList();
                        string where = "AND lhodoID in (";
                        StringBuilder builder = new StringBuilder(where);
                        insertData.ForEach(d =>
                        {
                            builder.Append($"'{d.lhodoID}'");
                            builder.Append(insertData.Last() == d ? ")" : ",");
                        });

                        where = builder.ToString();

                        Helper.DeleteWithTran<OutOrder>(where, tran);
                        Helper.DeleteWithTran<OutOrderDetailed>(where, tran);
                        //批量插入表头
                        Helper.BatchInsert(insertData, tran);

                        var details = new List<OutOrderDetailed>();
                        //拿所所有明细数据
                        var lists = insertData.Select(header => header.items).ToList();
                        //由于每一个OutOrder对象的OutOrderDetailed是一个数组，需要转换成一个List集合进行批量插入
                        lists.ForEach(l =>
                        {
                            var tmp = details.Union(l).ToList();
                            details = tmp;
                        });

                        Helper.BatchInsert(details, tran);
                    }
                    catch (Exception e)
                    {
                        tran.Rollback();
                        tran.Connection.Close();
                        string message = "出库单插入失败";
                        LogHelper.Error(message);
                        LogHelper.Error(e);
                    }
                }
                tran.Commit();
                
                UpdateSyncRecord(pars);

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
                pars.attr2 = "2019-10-26 10:28:54";

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
    }
}
