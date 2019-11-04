using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace hn.ArrowInterface.Entities
{
    /// <summary>
    /// 销售订单上传返回结果
    /// </summary>
    [Table("LH_SALEORDERRESULT")]
    public class Order
    {  
        public string lHOutSystemID { get; set; }
        public string orderNo { get; set; }
        public string lHreviweStatus { get; set; } 
        [NotMapped]
        public List<SaleOrderItemList> saleOrderItemList { get; set; }
        public string KeyId()
        {
            return " AND lHOutSystemID = '" + lHOutSystemID + "'";
        }
    }
    [Table("LH_SALEORDERRESULTITEMLIST")]
    public class SaleOrderItemList
    { 
        public string lHOutSystemID { get; set; }
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
