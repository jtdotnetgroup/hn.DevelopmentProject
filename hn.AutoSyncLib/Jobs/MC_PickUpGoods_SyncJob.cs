using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using hn.AutoSyncLib.Common;
using hn.AutoSyncLib.Model;
using Newtonsoft.Json;
using Quartz;

namespace hn.AutoSyncLib.Jobs
{
    public class MC_PickUpGoods_SyncJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {

            var mc_outofstore = MC_PickUpGoods.GetInstance();

            LogHelper.Info("开始同步");
            await Console.Out.WriteLineAsync("提货单开始同步");

            var token = MC_GetToken.GetInstance().Token;
            await mc_outofstore.SyncData_Today(token);



        }

    }
}