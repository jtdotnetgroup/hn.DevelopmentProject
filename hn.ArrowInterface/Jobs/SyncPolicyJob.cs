﻿using System;
using hn.AutoSyncLib.Common;
using Newtonsoft.Json;

namespace hn.ArrowInterface.Jobs
{
    public class SyncPolicyJob:AbsJob,ISyncJob
    {
        public bool Sync()
        {
            var token = GetToken();
            var result = Interface.QueryPolicy(token.Token);

            foreach (var row in result.Rows)
            {
                try
                {
                    Helper.Insert(row);
                }
                catch (Exception e)
                {
                    string msg = string.Format("插入政策记录失败：{0}", JsonConvert.SerializeObject(row));
                    LogHelper.LogInfo(msg);
                    LogHelper.LogErr(e);
                }
            }

            return true;
        }
    }
}