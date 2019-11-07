using System;
using hn.ArrowInterface.Entities;
using hn.Common;
using Newtonsoft.Json;

namespace hn.ArrowInterface.Jobs
{
    public class SyncSaleOrderJob : AbsJob
    {
        public override bool Sync()
        {
            var token = GetToken();

            var result = Interface.SaleOrder(token.Token);

            if (result.Success)
            {
                foreach (var row in result.Rows)
                {
                    try
                    {
                        Helper.Delete<SaleOrder>(row.KeyId());
                        Helper.Delete<SaleOrderDetailed>(row.KeyId());
                        Helper.Insert(row);
                        foreach (var item in row.saleOrderItemList) {
                            item.orderId = row.Id;
                            Helper.Insert(item);
                        }
                    }
                    catch (Exception e)
                    {
                        string message = string.Format("定制订单&常规工程订单&计划工程订单下载失败：{0}", JsonConvert.SerializeObject(row));
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
