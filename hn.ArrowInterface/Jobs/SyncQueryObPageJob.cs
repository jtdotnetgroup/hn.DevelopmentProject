using System;
using hn.Common;
using Newtonsoft.Json;

namespace hn.ArrowInterface.Jobs
{
    public class SyncQueryObPageJob : AbsJob
    {
        public override bool Sync()
        {
            var token = GetToken();

            var result = Interface.QueryObPage(token.Token);

            if (result.Success)
            {
                foreach (var row in result.Rows)
                {
                    try
                    { 
                        Helper.Insert(row);
                        foreach (var item in row.saleOrderItemList) {
                            Helper.Insert(item);
                        }
                    }
                    catch (Exception e)
                    {
                        string message = string.Format("物流部开单记录下载：{0}", JsonConvert.SerializeObject(row));
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
