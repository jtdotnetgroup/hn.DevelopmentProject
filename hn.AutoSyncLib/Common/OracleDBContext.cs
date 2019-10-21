using System.Data.Entity;
using hn.AutoSyncLib.Model;
using Oracle.ManagedDataAccess.Client;

namespace hn.AutoSyncLib.Common
{
    public class OracleDBContext:DbContext
    {
        public OracleDBContext(string conStr) : base(conStr)
        {
            
        }

        public DbSet<MC_OutofStore_ResultInfo> OutOfStore { get; set; }
        public DbSet<MC_PickUpGoods_ResultInfo> PickUpGoods { get; set; }
    }
}