using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using hn.AutoSyncLib.Common;
using hn.AutoSyncLib.Model;
using Newtonsoft.Json;
using Quartz.Logging;

namespace hn.AutoSyncLib
{
    public class MC_PickUpGoods : BaseRequest<MC_PickUpGoods, MC_PickUpGoods_Params>, ISync
    {
        public async Task<bool> RequestAndWriteData(MC_getToken_Result token, string rq1, string rq2,
            int pagesize = 1000, int pageindex = 1)
        {

            var startDate = DateTime.Parse(rq1);
            var endDate = DateTime.Parse(rq2);

            string parJson = "";
            const string logstr = "参数：{0}\r\n返回结果：总条数【{1}】，当前页共【{2}】条记录\r\n异常：{3}";

            do
            {
                rq1 = startDate.ToString("yyyy/MM/dd");
                rq2 = startDate.AddDays(1) <= endDate ? startDate.AddDays(1).ToString("yyyy/MM/dd") : rq1;

                var pagecount = 0;
                var total = 0;
                pageindex = 1;

                do
                {
                    try
                    {
                        var pars = new MC_PickUpGoods_Params(token.token, rq1, rq2, pagesize, pageindex);

                        parJson = JsonConvert.SerializeObject(pars);

                        var result = await  Request<MC_PickUpGoods_Result,  MC_PickUpGoods_ResultInfo>( pars);

                        total = result.TotalCount;

                        var msg = string.Format(logstr, parJson, total,
                            result.resultInfo.Count, "");

                        LogHelper.Info(msg);

                        if (pagecount == 0)
                        {
                            pagecount = total / pagesize;

                            if (total % pagesize > 0)
                            {
                                pagecount++;
                            }
                        }

                        //写入数据库
                        WriteDataToDB(result.resultInfo);

                        pageindex++;
                    }
                    catch (Exception e)
                    {
                       LogHelper.Error(e);
                    }

                } while (pageindex <= pagecount);

                startDate = startDate.AddDays(2);

            } while (startDate <= endDate);

            return true;

        }

        public async Task<bool> SyncData_EveryDate(MC_getToken_Result token)
        {
            string sql = "SELECT MAX(RQ) FROM MN_THD";

            var dbdate = helper.ExecuteScalar(sql, new Dictionary<string, object>()).ToString();

            var startDate = string.IsNullOrEmpty(dbdate)
                ? DateTime.Parse("2019/08/01").ToString("yyyy/MM/dd")
                : DateTime.Parse(dbdate).Date.ToString("yyyy/MM/dd");

            var endDate = DateTime.Now.Date.ToString("yyyy/MM/dd");

            //var result = await RequestAndWriteData(token, startDate, endDate, pageSize);

            LogHelper.Info("开始MN_THD同步作业");

            var result = await 
                 RequestAndWriteData_V2<MC_PickUpGoods_Result, MC_PickUpGoods_ResultInfo>(token, startDate,
                    endDate);

            Call_MN_THD_Update();

            LogHelper.Info("本次MN_THD同步作业结束");

            return result;
        }

        public async Task<bool> SyncData_Today(MC_getToken_Result token)
        {
            var startDate = DateTime.Now.Date.ToString("yyyy/MM/dd");
            var result = await RequestAndWriteData(token, startDate, startDate);

            Call_MN_THD_Update();

            return result;

        }

        public void Call_MN_THD_Update()
        {
            helper.ExecuteNonQuery("call mn_thd_update()", new Dictionary<string, object>());
        }


        /// <summary>
        /// 计算从开始日期到结束日期所需要的请求次数，并返回请求参数列表
        /// </summary>
        /// <param name="token"></param>
        /// <param name="rq1"></param>
        /// <param name="rq2"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        protected override List<MC_PickUpGoods_Params> ComputeParams(string token, string rq1, string rq2, int pageSize,
            int pageIndex)
        {
            var startDate = DateTime.Parse(rq1);
            var endDate = DateTime.Parse(rq2);

            List<MC_PickUpGoods_Params> result = new List<MC_PickUpGoods_Params>();

            do
            {
                var start = startDate.ToString("yyyy/MM/dd");
                var end = startDate.AddDays(1) <= endDate ? startDate.AddDays(1).ToString("yyyy/MM/dd") : endDate.ToString("yyyy/MM/dd");

                result.Add(new MC_PickUpGoods_Params(token, start, end, pageSize, pageIndex));

                startDate = startDate.AddDays(2);

            } while (startDate <= endDate);

            return result;
        }
    }
}