using System;
using System.Configuration;
using System.Data;
using System.Linq;
using hn.ArrowInterface.Entities;
using hn.ArrowInterface.RequestParams;
using hn.ArrowInterface.WebCommon;
using hn.Common;
using Newtonsoft.Json;

namespace hn.ArrowInterface.Jobs
{
    public class SyncQueryObPageJob : AbsJob
    {
        public override bool Sync()
        {
            var token = GetToken();
            //拿请求参数
            var pars = GetParams() as QueryObPageParam;
            var result = Interface.QueryObPage(token.Token,pars);

            if (result!=null&&result.Success)
            {
                foreach (var row in result.Rows)
                {
                    //事务不允许并发，锁定数据库连接
                    lock (Helper)
                    {
                        var conn = Helper.GetNewConnection();
                        conn.Open();
                        var tran = conn.BeginTransaction();
                        try
                        {
                            Helper.DeleteWithTran<LH_OUTBOUNDORDER>(row.KeyId(), tran);
                            Helper.DeleteWithTran<LH_OUTBOUNDORDERDETAILED>(row.KeyId(), tran);
                            Helper.InsertWithTransation(row, tran);
                            foreach (var item in row.items)
                            {
                                Helper.InsertWithTransation(item, tran);
                            }

                            //主从表均插入结束，提交事务
                            tran.Commit();
                            //关闭连接，下一条主表记录开启新的事务
                        }
                        catch (Exception e)
                        {
                            //发生异常，回滚事务
                            tran.Rollback();
                            tran.Connection.Close();
                            string message = string.Format("物流部开单记录下载：{0}", JsonConvert.SerializeObject(row));
                            LogHelper.Info(message);
                            LogHelper.Error(e);
                        }
                    }
                }

                UpdateSyncRecord(pars);

                return true;
            }

            return false;
        }

        protected override AbstractRequestParams GetParams()
        {
            //查历史同步记录
            var jobRecord = Helper.GetWhere<SyncJob_Definition>(new SyncJob_Definition() { JobClassName = this.JobName }).FirstOrDefault();

            var pars = new QueryObPageParam();
            pars.dealerCode = ConfigurationManager.AppSettings["dealerCode"];
            if (jobRecord == null)
            {
                pars.attr2 = "2019-09-02 10:28:54";

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
