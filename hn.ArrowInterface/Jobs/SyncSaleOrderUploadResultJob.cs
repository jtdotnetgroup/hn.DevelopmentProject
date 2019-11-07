using System;
using hn.Common;
using Newtonsoft.Json;
using hn.ArrowInterface.Entities;
using System.Collections.Generic;
using System.Linq;

namespace hn.ArrowInterface.Jobs
{
    public class SyncSaleOrderUploadResultJob : AbsJob
    {
        public override bool Sync()
        {
            var token = GetToken();
            //ICPOBILL m = Helper.Get<ICPOBILL>("SELECT * FROM ICPOBILL LIMIT 1");
            //List<ICPOBILLENTRY> mEntity = Helper.Select<ICPOBILLENTRY>("SELECT * FROM ICPOBILL ");
            SaleOrderUpload SelResult = new SaleOrderUpload();



            var result = Interface.SaleOrderUpload(token.Token, SelResult);

            if (result.Success)
            {
                var tmp = result.item;
                if (tmp != null)
                {
                    try
                    {
                        foreach (var row in tmp.AsParallel())
                        {
                            Helper.Delete<Order>(row.KeyId());
                            Helper.Insert(row);
                        }
                    }
                    catch (Exception e)
                    {
                        string message = string.Format("销售订单上传结果：{0}", JsonConvert.SerializeObject(tmp));
                        LogHelper.Info(message);
                        LogHelper.Error(e);
                        return false;
                    }
                }

                return true;
            }

            return false;
        }
    }
}
