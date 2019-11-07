using System;
using hn.Common;
using Newtonsoft.Json;
using hn.ArrowInterface.Entities;
using System.Collections.Generic;
using System.Linq;
using hn.ArrowInterface.WebCommon;
using hn.ArrowInterface.RequestParams;
using System.Configuration;

namespace hn.ArrowInterface.Jobs
{
    public class SyncSaleOrderUploadResultJob : AbsJob
    {
        public override bool Sync()
        {
            var token = GetToken(); 
            var pars = new SaleOrderUploadParam(); 
            var result = Interface.SaleOrderUpload(token.Token, pars);

            if (result.Success)
            {
                var tmp = result.item;
                if (tmp != null)
                {
                    try
                    {
                        foreach (var row in tmp.AsParallel())
                        {
                            Helper.Delete<Order>(row.KeyId());
                            Helper.Insert(row);
                        }
                    }
                    catch (Exception e)
                    {
                        string message = string.Format("销售订单上传结果：{0}", JsonConvert.SerializeObject(tmp));
                        LogHelper.Info(message);
                        LogHelper.Error(e);
                        return false;
                    }
                }
                //同步完成，更新请求记录
                UpdateSyncRecord(pars);
                return true;
            }

            return false;
        }

        protected override AbstractRequestParams GetParams()
        {
            var jobRecord = Helper.GetWhere<SyncJob_Definition>(new SyncJob_Definition() { JobClassName = this.JobName }).FirstOrDefault();
             
            var pars = new SaleOrderUploadParam
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
                saleOrderItemList = new[]
                {
                    new SaleOrderUploadDetailedParam
                    {
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

            return pars;
        }
    }
}
