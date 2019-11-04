using System;
using System.Threading.Tasks;
using hn.AutoSyncLib.Common;
using hn.AutoSyncLib.Model;
using Quartz;

namespace hn.AutoSyncLib.Jobs
{
    public class MC_OutOfStoreWithClearTable_SyncJob:IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var mc_outofstore = MC_OutOfStore.GetInstance();

            mc_outofstore.DeleteAllData<MC_OutofStore_ResultInfo>();

            var token = MC_GetToken.GetInstance().GetToken().GetAwaiter().GetResult();
            await mc_outofstore.SyncData_EveryDate(token);
        }

        
    }
}