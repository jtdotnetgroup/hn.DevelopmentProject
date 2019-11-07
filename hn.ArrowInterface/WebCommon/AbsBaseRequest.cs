using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using hn.ArrowInterface.Entities;
using hn.Common;
using Newtonsoft.Json;

namespace hn.ArrowInterface.WebCommon
{

    /// <summary>
    /// 基础网络请求类，
    /// </summary>
    public abstract class AbsBaseRequest
    {
        private HttpClient client;
        public AbsBaseRequest()
        {
            client = new HttpClient();
            //设置超时时长为10分钟
            client.Timeout = new TimeSpan(0, 0, 10, 0);
        }

        /// <summary>
        /// 基础POST请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="token">此值无时，传入空字符串</param>
        /// <param name="pars">请求参数</param>
        /// <returns></returns>
        public  T BaseRequest<T,TR>(string url, string token, Dictionary<string, object> pars,string Method= "POST") where T: AbsRequestResult<TR>
        {
            HttpContent content; 
            Dictionary<string, string> dic = new Dictionary<string, string>();
            var json = JsonConvert.SerializeObject(pars);
            //当token为空时，设置请求的ContentType为 w-xxx-form-urlencoded
            if (string.IsNullOrEmpty(token))
            {
                foreach (var k in pars.Keys)
                {
                    dic.Add(k, pars[k].ToString());
                }
                content = new FormUrlEncodedContent(dic);
            }
            else
            {
                content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse("bearer " + token);
            }

            HttpResponseMessage res;

            LogHelper.Info($@"开始请求：{url}\r\n参数：{json}");
            if (Method == "POST")
            {
                res = client.PostAsync(url, content).Result;
            }
            else
            {
                url += "?";
                foreach (var item in pars) {
                    url += item.Key + "=" + item.Value+"&";
                }
                url += "k=1";
                res = client.GetAsync(url).Result;
            }

            var resultStr = res.Content.ReadAsStringAsync().Result;

            var result =  JsonConvert.DeserializeObject<T>(resultStr);


            var count = result.Rows?.Count ?? (result.item?.Count ?? 0);

            string resultMessage = $"请求完成，共返回【{count}】条结果";

           LogHelper.Info(resultMessage);

            return result;

        }
        public  T BaseRequest<T>(string url, string token, object json)
        {
            return BaseRequest<T>(url, token, JsonConvert.SerializeObject(json));
        }
        public T BaseRequest<T>(string url, string token, string json)
        {
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse("bearer " + token);

            LogHelper.Info($@"开始请求：{url}\r\n参数：{json}");
            var res = client.PostAsync(url, content).Result;
            var result = JsonConvert.DeserializeObject<T>(res.Content.ReadAsStringAsync().Result);
            LogHelper.Info( $@"请求完成");
            return result;
        }
    }
}