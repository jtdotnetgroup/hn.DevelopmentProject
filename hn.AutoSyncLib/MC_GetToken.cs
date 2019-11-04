using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;
using hn.AutoSyncLib.Model;
using Newtonsoft.Json;

namespace hn.AutoSyncLib
{
    public class MC_GetToken:BaseRequest<MC_GetToken,MC_getToken_Params>
    {
        private  MC_getToken_Result token { get; set; }

        public  MC_getToken_Result Token
        {
             get
            {
                CheckToken().GetAwaiter();
                return token;
            }

            set { token = value; }
        }

        public async Task<MC_getToken_Result> GetToken()
        {
            await CheckToken();

            return token;
        }

        private async Task  Request(string url = "https://tms.monalisagroup.com.cn/mapi/doAction")
        {
           token =await Request<MC_getToken_Result,TokenInfo>(new MC_getToken_Params(), url);
        }

        private async Task CheckToken()
        {
            if (token == null)
            {
                await Console.Out.WriteLineAsync("token过期，更新token");
                await Request();
                return;
            }

            var endDate = DateTime.Parse(token.tokenInfo.endDate);
            if (endDate <= DateTime.Now)
            {
                 await Request();
            }
        }

        protected override List<MC_getToken_Params> ComputeParams(string token, string rq1, string rq2, int pageSize, int pageIndex)
        {
            throw new NotImplementedException();
        }
    }
}