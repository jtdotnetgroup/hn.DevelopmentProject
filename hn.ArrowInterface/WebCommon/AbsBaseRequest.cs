using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace hn.ArrowInterface.WebCommon
{
    public abstract class AbsBaseRequest
    {
        private HttpClient client;
        public AbsBaseRequest()
        {
            client=new HttpClient();
            client.Timeout = new TimeSpan(0, 0, 10, 0);
        }

        public  T BaseRequest<T>(string url, string token,Dictionary<string, object> pars)
        {
            HttpContent content;
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
    }
}