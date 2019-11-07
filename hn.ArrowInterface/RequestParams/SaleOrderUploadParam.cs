using hn.ArrowInterface.WebCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hn.ArrowInterface.RequestParams
{
    /// <summary>
    /// 销售订单上传
    /// </summary> 
    [Table("ICPOBILL")]
    public class SaleOrderUploadParam : AbstractRequestParams
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
        public string lHexpectedArrivedDate { get; set; }
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
        public SaleOrderUploadDetailedParam[] saleOrderItemList { get; set; }
    }
    /// <summary>
    /// 明细
    /// </summary>
    [Table("ICPOBILLENTRY")]
    public class SaleOrderUploadDetailedParam
    {
        public string prodCode { get; set; }
        public decimal qTY { get; set; }
        public string lHrowSource { get; set; }
        public string lHOutSystemID { get; set; }
        public string lHOutSystemLineID { get; set; }
        public string lHcomments { get; set; }
        public string lHDctpolicyItemId { get; set; }
    }
}
