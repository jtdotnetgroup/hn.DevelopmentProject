using System;
using hn.AutoSyncLib.Common;
using Newtonsoft.Json;

namespace hn.ArrowInterface.Jobs
{
    public class SyncAcctOAStatusJob : AbsJob, ISyncJob
    {
        public bool Sync()
        {
            var token = GetToken();

            var result = Interface.AcctOAStatus(token.Token);

            if (result.Success)
            {
                foreach (var row in result.Rows)
                {
                    try
                    {
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
