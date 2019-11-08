using System;
using System.Linq;
using hn.ArrowInterface.RequestParams;
using hn.ArrowInterface.WebCommon;
using hn.Common;
using Newtonsoft.Json;

namespace hn.ArrowInterface.Jobs
{
    public class SyncInventoryJob:AbsJob
    {
        protected override AbstractRequestParams GetParams()
        {
            QueryLHInventoryPageParam pars=new QueryLHInventoryPageParam();
            pars.dealerCode = DealerCode;
            return pars;
        }

        public override bool Sync()
        {
            var token = GetToken();

            var pars = GetParams() as QueryLHInventoryPageParam;

            var result = Interface.QueryLHInventoryPage(token.Token,pars);

            if (result.Success)
            {
                foreach (var row in result.Rows.AsParallel())
                {
                    try
                    {
                        row.ComputeFID();
                        Helper.Insert(row);
                    }
                    catch (Exception e)
                    {
                        string message = string.Format("库存记录插入失败：{0}", JsonConvert.SerializeObject(row));
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