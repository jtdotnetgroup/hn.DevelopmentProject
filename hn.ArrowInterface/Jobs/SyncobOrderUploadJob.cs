 using System;
using System.Linq;
using hn.ArrowInterface.Entities;
using hn.ArrowInterface.RequestParams;
 using hn.ArrowInterface.WebCommon;
 using hn.Common;
using Newtonsoft.Json;

namespace hn.ArrowInterface.Jobs
{
    [Obsolete("实时调用，不需要定时同步")]
    public class SyncobOrderUploadJob : AbsJob
    {
        protected override AbstractRequestParams GetParams()
        {
            //查历史同步记录
            var jobRecord = Helper.GetWhere<SyncJob_Definition>(new SyncJob_Definition() { JobClassName = this.JobName }).FirstOrDefault();

            var pars = new ObOrderUploadParam();
            pars.lhodoID = "";
            pars.lhplateNo = "";
            return pars;
        }

        public override bool Sync()
        {
            var token = GetToken();

            var pars=new ObOrderUploadParam();


            var result = Interface.obOrderUpload(token.Token,pars);

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
