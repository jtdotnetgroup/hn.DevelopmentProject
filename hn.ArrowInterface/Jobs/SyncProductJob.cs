using System;
using System.Configuration;
using System.Linq;
using hn.ArrowInterface.Entities;
using hn.ArrowInterface.RequestParams;
using hn.ArrowInterface.WebCommon;
using hn.Common;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hn.ArrowInterface.Jobs
{
    public class SyncProductJob : AbsJob
    {


        //实现获取请求参数方法
        protected override AbstractRequestParams GetParams()
        {
            //查历史同步记录
            var jobRecord = Helper.GetWhere(new SyncJob_Definition() { JobClassName = this.JobName }).FirstOrDefault();

            var pars = new LH_ProductParam();
            pars.attr1 = DealerCode;
            if (jobRecord == null)
            {
                pars.attr2 = "2019-01-02 10:28:54";

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

            //拿请求参数
            var pars = GetParams() as LH_ProductParam;

            var result = Interface.QueryProdPage(token.Token, pars);

            if (result.Success)
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
                    Helper.BatchInsert(insertData);
                }

                //同步完成，更新请求记录
                UpdateSyncRecord(pars);

                return true;
            }

            return false;
        }

    }
}