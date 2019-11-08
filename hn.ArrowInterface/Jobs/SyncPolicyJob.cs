﻿using System;
using System.Linq;
using hn.ArrowInterface.WebCommon;
using hn.Common;
using Newtonsoft.Json;

namespace hn.ArrowInterface.Jobs
{
    public class SyncPolicyJob:AbsJob
    {
        protected override AbstractRequestParams GetParams()
        {
            throw new NotImplementedException();
        }

        public override bool Sync()
        {
            var token = GetToken();
            var result = Interface.QueryPolicy(token.Token);
            foreach (var row in result.Rows.AsParallel())
            {
                try
                {
                    Helper.Insert(row);
                }
                catch (Exception e)
                {
                    string msg = string.Format("插入政策记录失败：{0}", JsonConvert.SerializeObject(row));
                    LogHelper.Info(msg);
                    LogHelper.Error(e);
                }
            }

            return true;
        }
    }
}