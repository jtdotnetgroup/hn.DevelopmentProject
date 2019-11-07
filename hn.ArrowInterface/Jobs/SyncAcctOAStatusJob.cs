using System;
using hn.Common;
using Newtonsoft.Json;

namespace hn.ArrowInterface.Jobs
{
    public class SyncAcctOAStatusJob : AbsJob
    {
        public override bool Sync()
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
                        foreach (var item in row.saleOrderItemList) {
                            item.orderNo = row.orderNo;
                            Helper.Insert(item);
                        }
                    }
                    catch (Exception e)
                    {
                        string message = string.Format("OA审核状态回传：{0}", JsonConvert.SerializeObject(row));
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
