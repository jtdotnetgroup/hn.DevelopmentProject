using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hn.ArrowInterface.Entities
{
    /// <summary>
    /// 销售订单上传
    /// </summary>
    public class SaleOrderUpload
    {
        public string orderType { get; set; }
        public string acctCode { get; set; }
        public string tradeCompanyName { get; set; }
        public string billIdName { get; set; }
        public string salesChannel { get; set; }
        public string lHbuType { get; set; }
        public string contractWay { get; set; }
        public string orderProdLine { get; set; }
        public string balanceName { get; set; }
        public DateTime lHexpectedArrivedDate { get; set; }
        public string lHdepositOrNot { get; set; }
        public string lHdiscountType { get; set; }
        public string lHorgName { get; set; }
        public DateTime submissionDate { get; set; }
        public string source { get; set; }
        public string lHOutSystemID { get; set; }
        public string lHOutSystemOd { get; set; }
        public string lHpromotionPolicyID { get; set; }
        public string remarks { get; set; }
        public string consignee { get; set; }
        public string lHoutboundOrder { get; set; }
        public string lHAdvertingMoneyType { get; set; } 
        public SaleOrderUploadDetailed[] saleOrderItemList { get; set; }
    }
    /// <summary>
    /// 明细
    /// </summary>
    public class SaleOrderUploadDetailed {
        public string prodCode { get; set; }
        public decimal qTY { get; set; }
        public string lHrowSource { get; set; }
        public string lHOutSystemID { get; set; }
        public string lHOutSystemLineID { get; set; }
        public string lHcomments { get; set; }
        public string lHDctpolicyItemId { get; set; }
    }
}
