using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using hn.AutoSyncLib.Common;
using hn.AutoSyncLib.Model;
using Newtonsoft.Json;
using Quartz;

namespace hn.AutoSyncLib.Jobs
{
    public class MC_OutOfStore_SyncJob:IJob
    {

        public async Task Execute(IJobExecutionContext context)
        {

            var mc_outofstore = MC_OutOfStore.GetInstance();
            var token = MC_GetToken.GetInstance().Token;

            LogHelper.LogInfo("出仓单开始同步");

            await Console.Out.WriteLineAsync("出仓单开始同步");

            await mc_outofstore.ClearTable();

            await mc_outofstore.SyncData_EveryDate(token);
        }

       
    }
}