using System;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.InteropServices;
using hn.ArrowInterface.Entities;
using hn.ArrowInterface.WebCommon;
using hn.ArrowInterface.Helper;
using Newtonsoft.Json;

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
        /// 7、销售订单上传
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public AbsRequestResult<Order> SaleOrderUpload(string token, SaleOrderUpload saleOrderUpload)
        {
            saleOrderUpload = new SaleOrderUpload()
            {
                orderType = "常规订单",
                acctCode = "AW04298",
                tradeCompanyName = "广东乐华智能卫浴有限公司",
                billIdName = "测试法人",
                salesChannel = "零售",
                lHbuType = "常规",
                contractWay = "经销",
                orderProdLine = "卫浴",
                balanceName = "测试-箭牌卫浴事业部-卫浴-零售",
                lHexpectedArrivedDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"),
                lHdepositOrNot = "N",
                lHdiscountType = "底价不变",
                lHorgName = "箭牌卫浴事业部",
                submissionDate = DateTime.Now,
                source = "华耐系统",
                lHOutSystemID = "testorder001",
                lHOutSystemOd = "dsdd-9999901",
                lHpromotionPolicyID = "",
                consignee = "1",
                lHoutboundOrder = "",
                lHAdvertingMoneyType = "PayForGoods",
                remarks = "",
                saleOrderItemList = new SaleOrderUploadDetailed[] {
                    new SaleOrderUploadDetailed {
                        prodCode = "17103103036416",
                        qTY = 10,
                        lHrowSource = "华耐系统",
                        lHOutSystemID = "testorder001",
                        lHOutSystemLineID = "testorderitem001",
                        lHcomments = "",
                        lHDctpolicyItemId = "W-57ASBU2SD"
                    }
                }
            };


            return BaseRequest<AbsRequestResult<Order>>(GlobParams.SaleSaleUpload, token, saleOrderUpload);
        }
        /// <summary>
        /// 8、审核状态回传3
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public AbsRequestResult<AcctOAStatus> AcctOAStatus(string token)
        {
            string dealerCode = ConfigurationManager.AppSettings["dealerCode"];
            Dictionary<string, object> pars = new Dictionary<string, object>();
            string[] arr =new string[] { "ASC-1907000091", "ASC-1906000119" };
            pars.Add("idStrings", arr); 
            pars.Add("acctCode", "123"); 

            return BaseRequest<AbsRequestResult<AcctOAStatus>>(GlobParams.QueryAcctOAStatus, token, pars);
        }
        /// <summary>
        /// 9、定制订单下载
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public AbsRequestResult<SaleOrder> SaleOrder(string token)
        { 
            string dealerCode = ConfigurationManager.AppSettings["dealerCode"];

            Dictionary<string, object> pars = new Dictionary<string, object>();
            pars.Add("attr1", dealerCode);

            DateTime attr2 = DateTime.Parse("2019-09-10 15:01:26");
            var attr3 = attr2.AddDays(10);

            pars.Add("attr2", attr2.ToString("yyyy-MM-dd HH:mm:ss"));
            pars.Add("attr3", attr3.ToString("yyyy-MM-dd HH:mm:ss"));

            return BaseRequest<AbsRequestResult<SaleOrder>>(GlobParams.QueryCustomOrderPage, token, pars);
        }
        /// <summary>
        /// 10、物流部开单记录下载
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public AbsRequestResult<QueryObPage> QueryObPage(string token)
        { 
            string dealerCode = ConfigurationManager.AppSettings["dealerCode"];

            Dictionary<string, object> pars = new Dictionary<string, object>();
            pars.Add("dealerCode", dealerCode);

            DateTime attr2 = DateTime.Parse("2019-10-12 15:01:26");
            var attr3 = attr2.AddDays(10);

            pars.Add("attr2", attr2.ToString("yyyy-MM-dd HH:mm:ss"));
            pars.Add("attr3", attr3.ToString("yyyy-MM-dd HH:mm:ss"));

            return BaseRequest<AbsRequestResult<QueryObPage>>(GlobParams.QueryObPage, token, pars);
        }
        /// <summary>
        /// 11、发货车牌号下载
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public AbsRequestResult<bool> obOrderUpload(string token)
        {
            Dictionary<string, object> pars = new Dictionary<string, object>();
            pars.Add("lhodoID", "12493");
            pars.Add("lhplateNo", "赣AJ3622");
            return BaseRequest<AbsRequestResult<bool>>(GlobParams.GoodsCarNoDown, token, pars);
        }

        /// <summary>
        /// 12、出库单下载
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public AbsRequestResult<OutOrder> queryObOrderPage(string token)
        {
            Dictionary<string, object> pars = new Dictionary<string, object>();
            pars.Add("attr2", DateTime.Parse("2019-09-10 15:01:26").ToString("yyyy-MM-dd HH:mm:ss"));
            pars.Add("attr3", DateTime.Parse("2019-09-10 15:01:26").AddDays(10).ToString("yyyy-MM-dd HH:mm:ss"));
            pars.Add("dealerCode", 123);
            return BaseRequest<AbsRequestResult<OutOrder>>(GlobParams.OutOrderDown, token, pars);
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
            string json = JsonConvert.SerializeObject(data); 
            return BaseRequest<AbsRequestResult>(url, token, json);
        }

        /// <summary>
        /// 四、	库存结存数据下载
        /// </summary>
        /// <param name="token"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public AbsRequestResult HnObOrderDayBatchInsert(string token, List<HnObOrderBatchInsertEntityDto> data)
        {
            string url = GlobParams.ObOrderDay_BatchInsertURL;

            string json = JsonConvert.SerializeObject(data);

            return BaseRequest<AbsRequestResult>(url, token, json);
        } 
    }
}