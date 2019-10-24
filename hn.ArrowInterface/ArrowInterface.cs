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
        /// <summary>
        /// 登录接口
        /// </summary>
        /// <returns></returns>
        public AuthorizationToken GetToken()
        {
            string url = GlobParams.ApiLogin;
            string username = ConfigurationManager.AppSettings["username"];
            string password = ConfigurationManager.AppSettings["password"];

            Dictionary<string,object> pars=new Dictionary<string, object>();
            pars.Add("username",username);
            pars.Add("password",password);

            return BaseRequest<AuthorizationToken>(url, null, pars);
        }

        /// <summary>
        /// 库存下载接口
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public AbsRequestResult<QuerylHInventoryPageResult> QueryLHInventoryPage(string token)
        {
            string url = GlobParams.QueryLHInventoryPageURL;
            string dealerCode = ConfigurationManager.AppSettings["dealerCode"];

            Dictionary<string,object> pars=new Dictionary<string, object>();
            pars.Add("dealerCode", dealerCode);

            return BaseRequest<AbsRequestResult<QuerylHInventoryPageResult>>(url, token, pars);
        }

        /// <summary>
        /// 物料下载接口
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 政策下载
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public AbsRequestResult<QueryPolicy> QueryPolicy(string token)
        {
            string url = GlobParams.QueryPolicyList;
            string dealerCode = ConfigurationManager.AppSettings["dealerCode"];

            Dictionary<string, object> pars = new Dictionary<string, object>();
            pars.Add("acctKey", dealerCode);

            return BaseRequest<AbsRequestResult<QueryPolicy>>(url, token, pars);
        }

        /// <summary>
        /// 华耐日销出库下载
        /// </summary>
        /// <param name="token"></param>
        /// <param name="data">上传的日销出库数据</param>
        /// <returns></returns>
        public AbsRequestResult HnInventoryBatchInsert(string token, List<HnInventoryBatchInsertEntity> data)
        {
            string url = GlobParams.Inventory_BatchInsertURL;
            Dictionary<string, object> pars = new Dictionary<string, object>();
            pars.Add("",data);

            return BaseRequest<AbsRequestResult>(url, token, pars);
        }


    }
}