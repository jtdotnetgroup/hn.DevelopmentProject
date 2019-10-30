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
    public class MC_PickUpGoods : BaseRequest<MC_PickUpGoods>, ISync
    {

        

        public async Task<bool> RequestAndWriteData(MC_getToken_Result token, string rq1, string rq2, int pagesize = 1000, int pageindex = 1)
        {

            var startDate = DateTime.Parse(rq1);
            var endDate = DateTime.Parse(rq2);

            string parJson = "";
            const string logstr = "参数：{0}\r\n返回结果：总条数【{1}】，当前页共【{2}】条记录\r\n异常：{3}";

            try
            {
                do
                {
                    rq1 = startDate.ToString("yyyy/MM/dd");
                    rq2 = startDate.AddDays(1) <= endDate ? startDate.AddDays(1).ToString("yyyy/MM/dd") : rq1;

                    var pagecount = 0;
                    var total = 0;
                    pageindex = 1;

                    do
                    {
                        var pars = new MC_PickUpGoods_Params(token.token, rq1, rq2, pagesize, pageindex);

                        parJson = JsonConvert.SerializeObject(pars);

                        var result =await 
                            Request<MC_PickUpGoods_Result, MC_PickUpGoods_Params, MC_PickUpGoods_ResultInfo>(
                                pars);

                        total = result.TotalCount;

                        var msg = string.Format(logstr, parJson, total,
                            result.resultInfo.Count, "");

                        LogHelper.LogInfo(msg);
                        await Console.Out.WriteLineAsync(msg);

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
                    } while (pageindex <= pagecount);

                    startDate = startDate.AddDays(2);

                } while (startDate <= endDate);

                return true;
            }
            catch (Exception e)
            {
                LogHelper.LogErr(e);
                return false;
            }

        }

        public async Task<bool> RequestDataWithMultiThreading(MC_getToken_Result token,
            string rq1, string rq2, int pagesize = 1000, int pageindex = 1)
        {
            var endDate = DateTime.Parse(rq2);

            var currentDate = DateTime.Parse(rq1);

            List<MC_PickUpGoods_Params> pars = new List<MC_PickUpGoods_Params>();

            while (currentDate <= endDate)
            {
                var d1 = currentDate.Date.ToString("yyyy/MM/dd");
                var d2 = currentDate.AddDays(1).Date <= endDate
                    ? currentDate.AddDays(1).Date.ToString("yyyy/MM/dd")
                    : endDate.ToString("yyyy/MM/dd");

                pars.Add(new MC_PickUpGoods_Params(token.token, d1, d2, pagesize, pageindex));

                currentDate = currentDate.AddDays(2);
            }

            List<MC_PickUpGoods_ResultInfo> requestData = await RequestWithThread<MC_PickUpGoods_Result, MC_PickUpGoods_Params, MC_PickUpGoods_ResultInfo>(pars);

            return SaveToDataBase(requestData);

        }

        private bool SaveToDataBase(List<MC_PickUpGoods_ResultInfo> data)
        {
            //并发写入
            var now = DateTime.Now;
            MC_PickUpGoods_ResultInfo datarow = null;
            string sql = "SELECT AUTOID FROM MN_THD";
            var table = helper.Select(sql);

            List<string> idList = new List<string>();

            for (int i = 0; i < table.Rows.Count; i++)
            {
                var tbrow = table.Rows[i];
                idList.Add(tbrow["AUTOID"].ToString());
            }

            List<MC_PickUpGoods_ResultInfo> inserdata = new List<MC_PickUpGoods_ResultInfo>();
            List<MC_PickUpGoods_ResultInfo> updatedata = new List<MC_PickUpGoods_ResultInfo>();

            List<List<MC_PickUpGoods_ResultInfo>> inserList = new List<List<MC_PickUpGoods_ResultInfo>>();

            data.ForEach(row =>
            {
                if (string.IsNullOrEmpty(row.autoId))
                {
                    row.ComputeFID();
                }

                if (idList.Contains(row.autoId))
                {
                    //已存在数据库，放入更新列表
                    updatedata.Add(row);
                }
                else
                {
                    //不存在数据库，放入插入列表
                    row.ComputeFID();
                    inserdata.Add(row);
                    //分批插入，每次批量插入1000条
                    if (inserdata.Count >= 1000)
                    {
                        inserList.Add(inserdata);
                        inserdata = new List<MC_PickUpGoods_ResultInfo>();
                    }
                }
            });
            //最后一批不足1000条待插入的数据
            inserList.Add(inserdata);


            inserList.ForEach(list =>
            {
                try
                {
                    helper.BatchInsert(list);
                }
                catch (Exception e)
                {
                    Console.Out.WriteLineAsync($"提货单写入数据库失败\n异常：{e.Message}");
                }
            });

            try
            {
                helper.BatchUpdate(updatedata, " AND AUTOID=:AUTOID");
            }
            catch (Exception ex)
            {
                Console.Out.WriteLineAsync($"提货单写入数据库失败\n异常：{ex.Message}");
            }

            var timespan = DateTime.Now - now;
            Console.Out.WriteLineAsync($"提货单写入数据库完毕耗时：{timespan.Hours}时{timespan.Minutes}分{timespan.Seconds}秒，共写入{data.Count}条数据");

            return true;
        }

        public async Task<bool> SyncData_EveryDate(MC_getToken_Result token)
        {
            string sql = "SELECT MAX(RQ) FROM MN_THD";

            var dbdate = helper.ExecuteScalar(sql, new Dictionary<string, object>()).ToString();

            var startDate = string.IsNullOrEmpty(dbdate) ?
                DateTime.Parse("2019/08/01").ToString("yyyy/MM/dd")
                : DateTime.Parse(dbdate).Date.ToString("yyyy/MM/dd");

            var endDate = DateTime.Now.Date.AddDays(-1).ToString("yyyy/MM/dd");

            int pageindex = 1;

            //var result = await RequestAndWriteData(token, startDate, endDate, pageSize);

            var result =await RequestAndWriteData(token, startDate, endDate);

            Call_MN_THD_Update();

            return result;
        }

        public async Task<bool> SyncData_Today(MC_getToken_Result token)
        {
            int pageindex = 1;
            var startDate = DateTime.Now.Date.ToString("yyyy/MM/dd");
            var result = await RequestAndWriteData(token, startDate, startDate);

            Call_MN_THD_Update();

            return result;

        }

        public void Call_MN_THD_Update()
        {
            helper.ExecuteNonQuery("call mn_thd_update()", new Dictionary<string, object>());
        }

        public async Task<int> ClearTable()
        {
            string sql = "SELECT MAX(RQ) FROM MN_THD";
            var dbdate = helper.ExecuteScalar(sql, new Dictionary<string, object>()).ToString();

            if (!string.IsNullOrEmpty(dbdate))
            {
                var maxdate = DateTime.Parse(dbdate).Date;
                if (DateTime.Today.Date.AddDays(-1) > maxdate)
                {
                    return helper.Delete<MC_PickUpGoods_ResultInfo>("");
                }

                return 0;
            }

            return helper.Delete<MC_PickUpGoods_ResultInfo>("");
        }
    }
}