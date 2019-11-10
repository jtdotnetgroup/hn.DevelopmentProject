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
    public class SyncInventoryJob : AbsJob
    {
        protected override AbstractRequestParams GetParams()
        {
            QueryLHInventoryPageParam pars = new QueryLHInventoryPageParam();
            pars.dealerCode = DealerCode;
            return pars;
        }

        public override bool Sync()
        {
            var token = GetToken();

            var pars = GetParams() as QueryLHInventoryPageParam;

            var result = Interface.QueryLHInventoryPage(token.Token, pars);

            Helper.Delete<QuerylHInventoryPageResult>("");

            if (result.Success)
            {
                result.Rows.ForEach(p=>p.ComputeFID());

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


                UpdateSyncRecord(pars);

                return true;
            }

            return false;
        }
    }
}