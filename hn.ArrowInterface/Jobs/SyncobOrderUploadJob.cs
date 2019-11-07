 using System;
using hn.Common;
using Newtonsoft.Json;

namespace hn.ArrowInterface.Jobs
{
    public class SyncobOrderUploadJob : AbsJob
    {
        public override bool Sync()
        {
            var token = GetToken();

            var result = Interface.obOrderUpload(token.Token);

            if (result.Success)
            {
                foreach (var row in result.Rows)
                {
                    try
                    {
                         
                    }
                    catch (Exception e)
                    {
                        string message = string.Format("发货车牌号下载失败：{0}", JsonConvert.SerializeObject(row));
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
