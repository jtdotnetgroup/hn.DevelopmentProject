using System;
using hn.Common;
using Newtonsoft.Json;
using hn.ArrowInterface.Entities;

namespace hn.ArrowInterface.Jobs
{
   public class SyncSaleOrderUploadResultJob : AbsJob
    {
        public override bool Sync()
        {
            var token = GetToken();

            var result = Interface.SaleOrderUpload(token.Token);

            if (result.Success)
            { 
                var row = result.Order;
                if (row != null)
                {
                    try
                    {
                        Helper.Delete<Order>(row.KeyId());
                        Helper.Insert(row);
                        foreach (var item in row.saleOrderItemList) {
                            item.lHOutSystemID = row.lHOutSystemID;
                            Helper.Delete<SaleOrderItemList>(row.KeyId());
                            Helper.Insert(item);
                        }
                    }
                    catch (Exception e)
                    {
                        string message = string.Format("销售订单上传结果：{0}", JsonConvert.SerializeObject(row));
                        LogHelper.LogInfo(message);
                        LogHelper.LogErr(e);
                        return false;
                    }
                }

                return true;
            }

            return false;
        }
    }
}
