using System;
using System.Collections.Generic;
using System.Configuration;
using hn.ArrowInterface.Entities;
using hn.ArrowInterface.RequestParams;
using hn.ArrowInterface.WebCommon;
using Newtonsoft.Json;

namespace hn.ArrowInterface
{
    public class ArrowInterface : AbsBaseRequest
    {
        /// <summary>
        ///     登录接口
        /// </summary>
        /// <returns></returns>
        public AuthorizationToken GetToken()
        {
            var url = GlobParams.ApiLogin;
            var username = ConfigurationManager.AppSettings["username"];
            var password = ConfigurationManager.AppSettings["password"];

            var pars = new Dictionary<string, object>();
            pars.Add("username", username);
            pars.Add("password", password);

            return BaseRequest<AuthorizationToken>(url, null, pars);
        }

        /// <summary>
        ///     库存下载接口
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public AbsRequestResult<QuerylHInventoryPageResult> QueryLHInventoryPage(string token)
        {
            var url = GlobParams.QueryLHInventoryPageURL;
            var dealerCode = ConfigurationManager.AppSettings["dealerCode"];

            var pars = new Dictionary<string, object>();
            pars.Add("dealerCode", dealerCode);

            return BaseRequest<AbsRequestResult<QuerylHInventoryPageResult>, QuerylHInventoryPageResult>(url, token,
                pars);
        }

        /// <summary>
        ///     物料下载接口
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public AbsRequestResult<LH_Product> QueryProdPage(string token,LH_ProductParam pars)
        {
            var url = GlobParams.Item_QueryProdPage;

            return BaseRequest<AbsRequestResult<LH_Product>, LH_Product>(url, token, pars.ToDictionary());
        }

        /// <summary>
        ///     政策下载
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public AbsRequestResult<QueryPolicy> QueryPolicy(string token)
        {
            var url = GlobParams.QueryPolicyList;
            var dealerCode = ConfigurationManager.AppSettings["dealerCode"];

            var pars = new Dictionary<string, object>();
            pars.Add("acctKey", dealerCode);

            return BaseRequest<AbsRequestResult<QueryPolicy>, QueryPolicy>(url, token, pars);
        }

        /// <summary>
        ///     7、销售订单上传
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public AbsRequestResult<Order> SaleOrderUpload(string token, SaleOrderUpload saleOrderUpload)
        {
            //saleOrderUpload = new SaleOrderUpload
            //{
            //    orderType = "常规订单",
            //    acctCode = "AW04298",
            //    tradeCompanyName = "广东乐华智能卫浴有限公司",
            //    billIdName = "测试法人",
            //    salesChannel = "零售",
            //    lHbuType = "常规",
            //    contractWay = "经销",
            //    orderProdLine = "卫浴",
            //    balanceName = "测试-箭牌卫浴事业部-卫浴-零售",
            //    lHexpectedArrivedDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"),
            //    lHdepositOrNot = "N",
            //    lHdiscountType = "底价不变",
            //    lHorgName = "箭牌卫浴事业部",
            //    submissionDate = DateTime.Now,
            //    source = "华耐系统",
            //    lHOutSystemID = "testorder001",
            //    lHOutSystemOd = "dsdd-9999901",
            //    lHpromotionPolicyID = "",
            //    consignee = "1",
            //    lHoutboundOrder = "",
            //    lHAdvertingMoneyType = "PayForGoods",
            //    remarks = "",
            //    saleOrderItemList = new[]
            //    {
            //        new SaleOrderUploadDetailed
            //        {
            //            prodCode = "17103103036416",
            //            qTY = 10,
            //            lHrowSource = "华耐系统",
            //            lHOutSystemID = "testorder001",
            //            lHOutSystemLineID = "testorderitem001",
            //            lHcomments = "",
            //            lHDctpolicyItemId = "W-57ASBU2SD"
            //        }
            //    }
            //};


            return BaseRequest<AbsRequestResult<Order>>(GlobParams.SaleSaleUpload, token, saleOrderUpload);
        }

        /// <summary>
        ///     8、审核状态回传3
        /// </summary>
        /// <param name="token"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public AbsRequestResult<AcctOAStatus> AcctOaStatus(string token,AcctOAStatusParam pars)
        {
            return BaseRequest<AbsRequestResult<AcctOAStatus>>(GlobParams.QueryAcctOAStatus, token, pars.ToDictionary());
        }

        /// <summary>
        ///     9、定制订单下载
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public AbsRequestResult<SaleOrder> SaleOrder(string token,LH_SaleOrderParam pars)
        {
            return BaseRequest<AbsRequestResult<SaleOrder>>(GlobParams.QueryCustomOrderPage, token, pars.ToDictionary());
        }

        /// <summary>
        ///     10、物流部开单记录下载
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public AbsRequestResult<QueryObPage> QueryObPage(string token,QueryObPageParam pars)
        {
            return BaseRequest<AbsRequestResult<QueryObPage>>(GlobParams.QueryObPage, token, pars.ToDictionary());
        }

        /// <summary>
        /// 发货车牌号下载
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public AbsRequestResult<bool> obOrderUpload(string token, ObOrderUploadParam pars)
        {
            return BaseRequest<AbsRequestResult<bool>>(GlobParams.GoodsCarNoDown, token, pars.ToDictionary());
        }

        /// <summary>
        ///     12、出库单下载
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public AbsRequestResult<OutOrder> queryObOrderPage(string token,QueryObOrderPageParam pars)
        {
            return BaseRequest<AbsRequestResult<OutOrder>>(GlobParams.OutOrderDown, token, pars.ToDictionary());
        }

        /// <summary>
        ///     华耐日销出库下载
        /// </summary>
        /// <param name="token"></param>
        /// <param name="data">上传的日销出库数据</param>
        /// <returns></returns>
        public AbsRequestResult HnInventoryBatchInsert(string token, List<HnInventoryBatchInsertEntity> data)
        {
            var url = GlobParams.Inventory_BatchInsertURL;
            var json = JsonConvert.SerializeObject(data);
            return BaseRequest<AbsRequestResult>(url, token, json);
        }

        /// <summary>
        ///     四、	库存结存数据下载
        /// </summary>
        /// <param name="token"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public AbsRequestResult HnObOrderDayBatchInsert(string token, List<HnObOrderBatchInsertEntityDto> data)
        {
            var url = GlobParams.ObOrderDay_BatchInsertURL;

            var json = JsonConvert.SerializeObject(data);

            return BaseRequest<AbsRequestResult>(url, token, json);
        }
    }
}