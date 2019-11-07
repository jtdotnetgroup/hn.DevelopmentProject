﻿using System;
using hn.ArrowInterface.Entities;
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
                        Helper.Delete<OutOrder>(row.KeyId());
                        Helper.Delete<OutOrderDetailed>(row.KeyId());
                        Helper.Insert(row);
                        if (row.items != null) { 
                            foreach (var item in row.items)
                            {
                                Helper.Insert(item);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        string message = string.Format("出库单下载失败：{0}", JsonConvert.SerializeObject(row));
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
