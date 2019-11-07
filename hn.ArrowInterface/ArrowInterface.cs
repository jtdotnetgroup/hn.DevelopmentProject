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
        ///     1、登录接口
        /// </summary>
        /// <returns></returns>
        public AuthorizationToken GetToken()
        { 
            var username = ConfigurationManager.AppSettings["username"];
            var password = ConfigurationManager.AppSettings["password"];

            var pars = new Dictionary<string, object>();
            pars.Add("username", username);
            pars.Add("password", password);

            return BaseRequest<AuthorizationToken>(GlobParams.ApiLogin, null, pars);
        }
        /// <summary>
        ///     2、库存下载接口
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public AbsRequestResult<QuerylHInventoryPageResult> QueryLHInventoryPage(string token, QueryLHInventoryPageParam pars)
        {
            return BaseRequest<AbsRequestResult<QuerylHInventoryPageResult>, QuerylHInventoryPageResult>(GlobParams.QueryLHInventoryPageURL, token, pars.ToDictionary());
        }
        /// <summary>
        ///     3、华耐日销出库下载
        /// </summary>
        /// <param name="token"></param>
        /// <param name="data">上传的日销出库数据</param>
        /// <returns></returns>
        public AbsRequestResult HnInventoryBatchInsert(string token, List<HnInventoryBatchInsertEntity> data)
        {
            var json = JsonConvert.SerializeObject(data);
            return BaseRequest<AbsRequestResult>(GlobParams.Inventory_BatchInsertURL, token, json);
        }
        /// <summary>
        ///     4、	库存结存数据下载
        /// </summary>
        /// <param name="token"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public AbsRequestResult HnObOrderDayBatchInsert(string token, List<HnObOrderBatchInsertEntityDto> data)
        {
            var json = JsonConvert.SerializeObject(data);

            return BaseRequest<AbsRequestResult>(GlobParams.ObOrderDay_BatchInsertURL, token, json);
        }
        /// <summary>
        ///     5、物料下载接口
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public AbsRequestResult<LH_Product> QueryProdPage(string token,LH_ProductParam pars)
        { 
            return BaseRequest<AbsRequestResult<LH_Product>, LH_Product>(GlobParams.Item_QueryProdPage, token, pars.ToDictionary());
        }
        /// <summary>
        ///     6、折扣政策下载
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public AbsRequestResult<QueryPolicy> QueryPolicy(string token, QueryPolicyParam queryPolicyParam)
        {
            return BaseRequest<AbsRequestResult<QueryPolicy>, QueryPolicy>(GlobParams.QueryPolicyList, token, queryPolicyParam.ToDictionary());
        }
        /// <summary>
        ///     7、销售订单上传
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public AbsRequestResult<Order> SaleOrderUpload(string token, SaleOrderUploadParam pars)
        { 
            return BaseRequest<AbsRequestResult<Order>>(GlobParams.SaleSaleUpload, token, pars.ToDictionary());
        }
        /// <summary>
        ///     8、审核状态回传
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
        ///     11、发货车牌号下载
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
    }
}