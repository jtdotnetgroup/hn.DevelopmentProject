using System;
using hn.Common;
using Newtonsoft.Json;

namespace hn.ArrowInterface.Jobs
{
    public class SyncqueryObOrderPageJob : AbsJob
    {
        public override bool Sync()
        {
            var token = GetToken();

            var result = Interface.queryObOrderPage(token.Token);

            if (result.Success)
            {
                foreach (var row in result.Rows)
                {
                    try
                    {
                        Helper.Insert(row);
                        foreach (var item in row.saleOrderItemList)
                        {
                            Helper.Insert(item);
                        }
                    }
                    catch (Exception e)
                    {
                        string message = string.Format("出库单下载失败：{0}", JsonConvert.SerializeObject(row));
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
