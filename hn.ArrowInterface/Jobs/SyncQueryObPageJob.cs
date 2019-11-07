using System;
using hn.ArrowInterface.Entities;
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
                        Helper.Delete<QueryObPage>(row.KeyId());
                        Helper.Delete<QueryObPageDetailed>(row.KeyId());
                        Helper.Insert(row);
                        foreach (var item in row.items) {
                            Helper.Insert(item);
                        }
                    }
                    catch (Exception e)
                    {
                        string message = string.Format("物流部开单记录下载：{0}", JsonConvert.SerializeObject(row));
                        LogHelper.Info(message);
                        LogHelper.Error(e);
                    }
                }

                return true;
            }

            return false;
        }
    }
}
