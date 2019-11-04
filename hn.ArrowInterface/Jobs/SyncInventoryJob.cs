using System;
using hn.Common;
using Newtonsoft.Json;

namespace hn.ArrowInterface.Jobs
{
    public class SyncInventoryJob:AbsJob
    {
        public override bool Sync()
        {
            var token = GetToken();

            var result = Interface.QueryLHInventoryPage(token.Token);

            if (result.Success)
            {
                foreach (var row in result.Rows)
                {
                    try
                    {
                        row.ComputeFID();
                        Helper.Insert(row);
                    }
                    catch (Exception e)
                    {
                        string message = string.Format("库存记录插入失败：{0}", JsonConvert.SerializeObject(row));
                        LogHelper.LogInfo(message);
                        LogHelper.LogErr(e);
                    }
                }

                return true;
            }

            return false;
        }
    }
}