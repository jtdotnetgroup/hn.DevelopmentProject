using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hn.ArrowInterface.Entities
{
    /// <summary>
    /// 销售订单上传
    /// </summary>
    public class SaleOrderUploadResult
    {
        public string lHOutSystemID { get; set; }
        public string orderNo { get; set; }
        public string lHreviweStatus { get; set; }
        public string lHOutSystemLineID { get; set; }
        public decimal lHdealerPrice { get; set; }
        public string lHprodUnit { get; set; }
        public decimal lHFactoryDiscount { get; set; }
        public decimal lHoriginalDiscount { get; set; }
        public decimal lHapprovalDiscount { get; set; }
        public decimal lHfinalDiscount { get; set; }
        public decimal lHDiscountPrice { get; set; }
    } 
}
