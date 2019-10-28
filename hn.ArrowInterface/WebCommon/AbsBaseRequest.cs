using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
            client=new HttpClient();
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
        public  T BaseRequest<T>(string url, string token,Dictionary<string, object> pars)
        {
            HttpContent content;
            //当token为空时，设置请求的ContentType为 w-xxx-form-urlencoded
            if (string.IsNullOrEmpty(token))
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                foreach (var k in pars.Keys)
                {
                    dic.Add(k,pars[k].ToString());
                }
                content=new FormUrlEncodedContent(dic);
            }
            else
            {
                var json = JsonConvert.SerializeObject(pars);
                content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization =AuthenticationHeaderValue.Parse( "bearer " + token);
            }

            var res = client.PostAsync(url, content).Result;
            var result = JsonConvert.DeserializeObject<T>(res.Content.ReadAsStringAsync().Result);

            return result;

        }

        public T BaseRequest<T>(string url, string token, string json)
        {
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse("bearer " + token);

            var res = client.PostAsync(url, content).Result;
            var result = JsonConvert.DeserializeObject<T>(res.Content.ReadAsStringAsync().Result);

            return result;
        }
    }
}