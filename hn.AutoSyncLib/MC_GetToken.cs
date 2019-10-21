using System;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;
using hn.AutoSyncLib.Model;
using Newtonsoft.Json;

namespace hn.AutoSyncLib
{
    public class MC_GetToken:BaseRequest<MC_GetToken>
    {
        private  MC_getToken_Result token { get; set; }

        public  MC_getToken_Result Token
        {
            get
            {
                CheckToken();
                return token;
            }

            set { token = value; }
        }

        private void   Request(string url = "https://tms.monalisagroup.com.cn/mapi/doAction")
        {
           token = Request<MC_getToken_Result, MC_getToken_Params,TokenInfo>(new MC_getToken_Params(), url).Result;
        }

        private void CheckToken()
        {
            if (token == null)
            {
                 Console.Out.WriteLineAsync("token过期，更新token");
                 Request();
                return;
            }

            var endDate = DateTime.Parse(token.tokenInfo.endDate);
            if (endDate <= DateTime.Now)
            {
                 Request();
            }
        }
    }
}