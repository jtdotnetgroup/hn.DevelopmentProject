using System.Threading.Tasks;
using hn.AutoSyncLib.Model;

namespace hn.AutoSyncLib
{
    public interface ISync
    {
        Task<bool> RequestAndWriteData(MC_getToken_Result token, string rq1, string rq2, int pagesize, int pageindex = 1);

        Task<bool> SyncData_EveryDate(MC_getToken_Result token);
    }
}