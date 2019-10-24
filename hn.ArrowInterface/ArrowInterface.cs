using System;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.InteropServices;
using hn.ArrowInterface.Entities;
using hn.ArrowInterface.WebCommon;
using hn.ArrowInterface.Helper;

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
        /// 9、定制订单下载
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public AbsRequestResult<SaleOrder> SaleOrder(string token)
        {
            string url = GlobParams.QueryCustomOrderPage;
            string dealerCode = ConfigurationManager.AppSettings["dealerCode"];

            Dictionary<string, object> pars = new Dictionary<string, object>(); 
            pars.Add("attr1", dealerCode);

            DateTime attr2 = DateTime.Parse("2019-03-27 10:30:28");
            var attr3 = attr2.AddDays(30);

            pars.Add("attr2", attr2.ToString("yyyy-MM-dd HH:mm:ss"));
            pars.Add("attr3", attr3.ToString("yyyy-MM-dd HH:mm:ss"));

            return BaseRequest<AbsRequestResult<SaleOrder>>(url, token, pars);
        }
        /// <summary>
        /// 10、物流部开单记录下载
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public AbsRequestResult<QueryObPage> QueryObPage(string token)
        {
            string url = GlobParams.QueryCustomOrderPage;
            string dealerCode = ConfigurationManager.AppSettings["dealerCode"];

            Dictionary<string, object> pars = new Dictionary<string, object>();
            pars.Add("attr1", dealerCode);

            DateTime attr2 = DateTime.Parse("2019-03-27 10:30:28");
            var attr3 = attr2.AddDays(30);

            pars.Add("attr2", attr2.ToString("yyyy-MM-dd HH:mm:ss"));
            pars.Add("attr3", attr3.ToString("yyyy-MM-dd HH:mm:ss"));

            return BaseRequest<AbsRequestResult<QueryObPage>>(url, token, pars);
        }
        /// <summary>
        /// 7、销售订单上传
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public AbsRequestResult<SaleOrderUploadResult> SaleOrderUpload(string token)
        {

            Dictionary<string, object> pars = new Dictionary<string, object>();

            SaleOrderUpload saleOrderUpload = new SaleOrderUpload()
            {
                orderType = "",
                acctCode = "",
                tradeCompanyName = "",
                billIdName = "",
                salesChannel = "",
                lHbuType = "",
                contractWay = "",
                orderProdLine = "",
                balanceName = "",
                lHexpectedArrivedDate = DateTime.Now,
                lHdepositOrNot = "",
                lHdiscountType = "",
                lHorgName = "",
                submissionDate = DateTime.Now,
                source = "",
                lHOutSystemID = "",
                lHOutSystemOd = "",
                lHpromotionPolicyID = "",
                remarks = "",
                consignee = "",
                lHoutboundOrder = "",
                lHAdvertingMoneyType = "",
                saleOrderItemList = new SaleOrderUploadDetailed[] {
                    new SaleOrderUploadDetailed {
                        prodCode = "",
                        qTY = 0,
                        lHrowSource = "",
                        lHOutSystemID = "",
                        lHOutSystemLineID = "",
                        lHcomments = "",
                        lHDctpolicyItemId = ""
                    }
                }
            };
            pars = saleOrderUpload.ObjectToMap();

            return BaseRequest<AbsRequestResult<SaleOrderUploadResult>>(GlobParams.SaleSaleUpload, token, pars);
        }
        /// <summary>
        /// 8、审核状态回传
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public AbsRequestResult<AcctOAStatus> AcctOAStatus(string token)
        {
            string dealerCode = ConfigurationManager.AppSettings["dealerCode"];
            Dictionary<string, object> pars = new Dictionary<string, object>(); 
            pars.Add("acctCode", dealerCode); 
            pars.Add("idStrings", ""); 

            return BaseRequest<AbsRequestResult<AcctOAStatus>>(GlobParams.QueryAcctOAStatus, token, pars);
        }
        
    }
}