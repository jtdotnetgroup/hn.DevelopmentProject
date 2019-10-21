using System;
using System.Threading.Tasks;
using Quartz;

namespace hn.AutoSyncLib.Jobs
{
    public class MC_PickUpGoods_SyncJob_WithDataMap:IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var rq1 = context.JobDetail.JobDataMap.GetString("rq1");
            var rq2 = context.JobDetail.JobDataMap.GetString("rq2");


            if (string.IsNullOrEmpty(rq1) || string.IsNullOrEmpty(rq2))
            {
                throw  new ArgumentException("参数错误，开始日期和结束日期不能为空");
            }

            var mc_pickupgoods = MC_PickUpGoods.GetInstance();

            var token = MC_GetToken.GetInstance().Token;
           var result= await mc_pickupgoods.RequestAndWriteData(token, rq1, rq2);

            mc_pickupgoods.Call_MN_THD_Update();

            await Console.Out.WriteLineAsync("同步任务完成");

           context.Result = result;

        }
    }
}