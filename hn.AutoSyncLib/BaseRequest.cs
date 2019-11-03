using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using hn.AutoSyncLib.Common;
using hn.AutoSyncLib.Model;
using Newtonsoft.Json;
using log4net;


namespace hn.AutoSyncLib
{

    public abstract class BaseRequest<T, TP> where T : new() where TP : MC_Request_BaseParams, ICloneable, IPageInterface
    {
        protected static T Instance { get; set; }

        protected static object lockobj = new object();

        protected string conStr = ConfigurationManager.ConnectionStrings["DBConnectionTest"].ConnectionString;

        protected OracleDBHelper helper { get; set; }

        public int pageSize { get; set; }

        public BaseRequest(int pageSize = 1000)
        {
            this.pageSize = 1000;
            this.helper = new OracleDBHelper(conStr);
        }

        public static T GetInstance()
        {
            lock (lockobj)
            {
                if (Instance == null)
                {
                    lock (lockobj)
                    {
                        Instance = new T();
                    }
                }

                return Instance;
            }
        }

        public void WriteDataToDB<T1>(List<T1> list) where T1 : IComputeFID, IFID, new()
        {

            if (list.Count > 0)
            {
                //并发写入
                foreach (var row in list.AsParallel())
                {
                    try
                    {
                        row.ComputeFID();
                        var item = helper.GetWhere(new T1() { FID = row.FID }).SingleOrDefault();

                        if (item != null)
                        {
                            helper.Update(row);
                            continue;
                        }

                        helper.Insert(row);
                    }
                    catch (Exception e)
                    {
                       LogHelper.Info("数据更新或插入失败：" + JsonConvert.SerializeObject(row));
                       LogHelper.Error(e);
                    }

                }

                //LogHelper.LogInfo("数据写入完毕");
            }
        }

        protected abstract List<TP> ComputeParams(string token, string rq1, string rq2, int pageSize, int pageIndex);

        public async Task<bool> RequestAndWriteData_V2<T1, T3>(MC_getToken_Result token, string rq1, string rq2) where T1 : MC_Request_BaseResult<T3> where T3 : IComputeFID, IFID, new()
        {
            string msg = "";

            var pageSize = 1000;
            var pageIndex = 1;
            //计算请求参数
            var pars = ComputeParams(token.token, rq1, rq2, pageSize, pageIndex);

            string parJson = "";
            const string logstr = "参数：{0}\r\n返回结果：共【{1}】条记录\r\n异常：{2}";

            List<TP> exceptionList = null;

            var action = new Func<TP, Task<List<T3>>>(async (param) =>
           {
               var json = JsonConvert.SerializeObject(param);
               List<T3> result = null;
               const string log = "参数：{0}\r\n返回结果：共【{1}】条记录\r\n异常：{2}";
               try
               {
                   result = await CommonRequest<T1, T3>(param);

                   var formatTxt = string.Format(log, json,
                       result.Count, "");

                   WriteDataToDB(result);
                   result.Clear();
                   LogHelper.Info(formatTxt);

               }
               catch (Exception e)
               {
                   //请求异常时，请加请求参数到异常列表，待列表循环结束后，再重新请求异常列表
                   if (exceptionList == null)
                   {
                       exceptionList = new List<TP>();
                   }

                   var formatTxt = string.Format(log, json, result?.Count ?? 0,
                         e.Message);

                  LogHelper.Info(formatTxt);
                  LogHelper.Error(e);

               }
               return null;
           });

            foreach (var par in pars.AsParallel())
            {
              await  action.Invoke(par);
            }

            int fixCount = 0;
            //处理异常列表
            if (exceptionList != null && exceptionList.Count > 0)
            {
                LogHelper.Info("重新请求异常列表数据");
                foreach (var par in exceptionList.AsParallel())
                {
                    try
                    {
                        var result = await CommonRequest<T1, T3>(par);

                        WriteDataToDB(result);
                        fixCount++;
                    }
                    catch (Exception e)
                    {
                        msg = string.Format(logstr, parJson, 0,
                            0, e.Message);
                       LogHelper.Info(msg);
                       LogHelper.Error(e);
                    }

                }
            }

            return exceptionList == null || fixCount == exceptionList.Count;
        }

        protected virtual async Task<List<T3>> CommonRequest<T1, T3>(TP par) where T1 : MC_Request_BaseResult<T3> where T3 : IComputeFID, IFID, new()
        {
            string parJson = "";
            //const string logstr = "参数：{0}\r\n返回结果：总条数【{1}】，当前页共【{2}】条记录\r\n异常：{3}";
            var pagecount = 0;
            var total = 0;
            var pageindex = 1;
            List<T3> resultList = new List<T3>();

            parJson = JsonConvert.SerializeObject(par);
            do
            {
                par.pageIndex = pageindex;
                var requestTask = await Request<T1, T3>(par);

                total = requestTask.TotalCount;

                if (pagecount == 0)
                {
                    pagecount = total / par.pageSize;

                    if (total % par.pageSize > 0)
                    {
                        pagecount++;
                    }
                }

                resultList.AddRange(requestTask.resultInfo);

                pageindex++;
            } while (pageindex <= pagecount);

            return resultList;
        }

        /// <summary>
        /// 发送POST请求
        /// </summary>
        /// <typeparam name="T1">返回结果的类型</typeparam>
        /// <typeparam name="T2">请求参数类型</typeparam>
        /// <param name="pars">请求参数</param>
        /// <param name="url">请求地址</param>
        /// <returns></returns>
        public virtual async Task<T1> Request<T1, T3>(TP pars, string url = "https://tms.monalisagroup.com.cn/mapi/doAction") where T1 : MC_Request_BaseResult<T3>
        {
            HttpClient client = new HttpClient();
            client.Timeout = new TimeSpan(0, 0, 10, 0);
            HttpContent content = new FormUrlEncodedContent(pars.ModelToDic<TP>());
            try
            {
                var data = await client.PostAsync(url, content);
                string jsonStr = await data.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<T1>(jsonStr);

                return result;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int DeleteAllData<T1>()
        {
            return helper.Delete<T1>("");
        }




    }
}