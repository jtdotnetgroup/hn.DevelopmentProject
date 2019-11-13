using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using hn.ArrowInterface.Entities;
using hn.Common;
using Newtonsoft.Json;

namespace hn.ArrowInterface.WebCommon
{
    /// <summary>
    ///     基础网络请求类，
    /// </summary>
    public abstract class AbsBaseRequest:IDisposable
    {
        private readonly HttpClient client;

        public AbsBaseRequest()
        {
            client = new HttpClient();
            //设置超时时长为10分钟
            client.Timeout = new TimeSpan(0, 0, 10, 0);
        }

        /// <summary>
        ///     基础POST请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="token">此值无时，传入空字符串</param>
        /// <param name="pars">请求参数</param>
        /// <returns></returns>
        public T BaseRequest<T, TR>(string url, string token, Dictionary<string, object> pars, string Method = "POST")
            where T : AbsRequestResult<TR>, new()
        {
            HttpContent content;
            var dic = new Dictionary<string, string>();
            var json = JsonConvert.SerializeObject(pars);
            //当token为空时，设置请求的ContentType为 w-xxx-form-urlencoded
            if (string.IsNullOrEmpty(token))
            {
                foreach (var k in pars.Keys) dic.Add(k, pars[k].ToString());
                content = new FormUrlEncodedContent(dic);
            }
            else
            {
                content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse("bearer " + token);
            }

            HttpResponseMessage res;

            LogHelper.Info($"开始请求：{url}");
            if (Method == "POST")
            {
                res = client.PostAsync(url, content).GetAwaiter().GetResult();
            }
            else
            {
                url += "?";
                foreach (var item in pars) url += item.Key + "=" + item.Value + "&";
                url += "k=1";
                res = client.GetAsync(url).GetAwaiter().GetResult();
            }

            var resultStr = res.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            var result = JsonConvert.DeserializeObject<T>(resultStr);


            var count = result.Rows?.Count ?? (result.item?.Count ?? 0);

            var resultMessage = $"请求完成，共返回【{count}】条结果";

            LogHelper.Info(resultMessage);

            return result;
        }

        public T CommonBaseRequest<T>(string url, HttpContent content) where T : new()
        {
            LogHelper.Info($"开始请求：{url}");
            T result = new T();
            HttpResponseMessage res = null;

            try
            {
                res = client.PostAsync(url, content).GetAwaiter().GetResult();

                var resultStr = res.Content.ReadAsStringAsync();

                result = JsonConvert.DeserializeObject<T>(res.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                LogHelper.Info($"请求完成:{url}");
            }
            catch (AggregateException ex)
            {
                LogHelper.Error(ex);
                LogHelper.Error($"URL:{url},请求信息：{JsonConvert.SerializeObject(content)}\r\n,请求异常：{ex.Message}");
            }
            catch (JsonReaderException e)
            {
                LogHelper.Error($"URL:{url},返回格式异常：{e.Message}\r\n返回值：{res.Content.ReadAsStringAsync()}");
                LogHelper.Error(e);
            }

            return result;
        }

        public T BaseRequest<T>(string url, string token, string json) where T : new()
        {
            HttpContent content;
            content = new StringContent(json, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse("bearer " + token);

            return CommonBaseRequest<T>(url, content);
        }

        public T BaseRequest<T>(string url, string token, object obj) where T : new()
        {
            return BaseRequest<T>(url, token, JsonConvert.SerializeObject(obj));
        }

        public T BaseRequest<T>(string url, string token, Dictionary<string, object> pars) where T : AbsRequestResult, new()
        {
            HttpContent content;
            var dic = new Dictionary<string, string>();
            var json = JsonConvert.SerializeObject(pars);
            if (string.IsNullOrEmpty(token))
            {
                foreach (var k in pars.Keys) dic.Add(k, pars[k].ToString());
                content = new FormUrlEncodedContent(dic);
            }
            else
            {
                content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse("bearer " + token);

            return CommonBaseRequest<T>(url, content);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                client?.Dispose();
                // Dispose managed resources
            }
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);

        }
    }
}