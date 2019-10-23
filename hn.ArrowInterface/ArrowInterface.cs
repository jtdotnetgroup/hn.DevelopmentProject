using System;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.InteropServices;
using hn.ArrowInterface.Entities;
using hn.ArrowInterface.WebCommon;

namespace hn.ArrowInterface
{
    public class ArrowInterface:AbsBaseRequest
    {
        public AutorizationTokenDTO GetToken()
        {
            string url = GlobParams.ApiLogin;
            string username = ConfigurationManager.AppSettings["username"];
            string password = ConfigurationManager.AppSettings["password"];

            Dictionary<string,object> pars=new Dictionary<string, object>();
            pars.Add("username",username);
            pars.Add("password",password);

            return BaseRequest<AutorizationTokenDTO>(url, null, pars);
        }

        public AbsRequestResult<QuerylHInventoryPageResult> QueryLHInventoryPage(string token)
        {
            string url = GlobParams.QueryLHInventoryPageURL;
            string dealerCode = ConfigurationManager.AppSettings["dealerCode"];

            Dictionary<string,object> pars=new Dictionary<string, object>();
            pars.Add("dealerCode", dealerCode);

            return BaseRequest<AbsRequestResult<QuerylHInventoryPageResult>>(url, token, pars);
        }

        public AbsRequestResult<QueryProdPageResult> QueryProdPage(string token)
        {
            string url = GlobParams.Item_QueryProdPage;

            Dictionary<string, object> pars = new Dictionary<string, object>();
            string dealerCode = ConfigurationManager.AppSettings["dealerCode"];
            pars.Add("attr1",dealerCode);

            DateTime attr2 = DateTime.Parse("2019-03-27 10:30:28");
            var attr3 = attr2.AddDays(30);

            pars.Add("attr2", attr2.ToString("yyyy-MM-dd HH:mm:ss"));
            pars.Add("attr3",attr3.ToString("yyyy-MM-dd HH:mm:ss"));

            return BaseRequest<AbsRequestResult<QueryProdPageResult>>(url, token, pars);

        }
        
    }
}