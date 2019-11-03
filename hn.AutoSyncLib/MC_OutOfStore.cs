using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using hn.AutoSyncLib.Common;
using hn.AutoSyncLib.Model;
using log4net;
using Newtonsoft.Json;

namespace hn.AutoSyncLib
{
    public class MC_OutOfStore : BaseRequest<MC_OutOfStore,MC_OutOfStore_Params>
    {
        public async Task<bool> SyncData_EveryDate(MC_getToken_Result token)
        {
            LogHelper.Info("开始MN_CKD同步作业");
            string sql = "SELECT MAX(RQ) FROM MN_CKD";

            var dbdate = helper.ExecuteScalar(sql, new Dictionary<string, object>()).ToString();

            var startDate = string.IsNullOrEmpty(dbdate) ?
                DateTime.Parse("2019/08/01").ToString("yyyy/MM/dd")
                : DateTime.Parse(dbdate).Date.ToString("yyyy/MM/dd");

            var endDate = DateTime.Now.Date.ToString("yyyy/MM/dd");

            var result= await RequestAndWriteData_V2<MC_OutOfStore_Result,MC_OutofStore_ResultInfo>(token, startDate, endDate);
            LogHelper.Info("本次MN_CKD同步作业结束");
            return result;
        }

        public async Task<bool> SyncData_Today(MC_getToken_Result token)
        {
            var startDate = DateTime.Now.Date.ToString("yyyy/MM/dd");
            return await RequestAndWriteData_V2<MC_OutOfStore_Result,MC_OutofStore_ResultInfo>(token, startDate, startDate);
        }

        protected override List<MC_OutOfStore_Params> ComputeParams(string token, string rq1, string rq2, int pageSize, int pageIndex)
        {
            var startDate = DateTime.Parse(rq1);
            var endDate = DateTime.Parse(rq2);

            List<MC_OutOfStore_Params> result = new List<MC_OutOfStore_Params>();

            do
            {
                var start = startDate.ToString("yyyy/MM/dd");
                var end = startDate.AddDays(1) <= endDate ? startDate.AddDays(1).ToString("yyyy/MM/dd") : endDate.ToString("yyyy/MM/dd");

                result.Add(new MC_OutOfStore_Params(token, start, end, pageSize, pageIndex));

                startDate = startDate.AddDays(2);

            } while (startDate <= endDate);

            return result;
        }
    }
}
