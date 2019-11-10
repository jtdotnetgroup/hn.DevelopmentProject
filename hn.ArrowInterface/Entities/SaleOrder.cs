using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace hn.ArrowInterface.Entities
{
    /// <summary>
    /// 定制订单头
    /// </summary>
    [Table("LH_SALEORDER")]
    public class SaleOrder
    { 
        public string Id { get; set; }
        public string orderId
        {
            get => Id;
        }
        public string orderNo { get; set; }
        public string acctCode { get; set; }
        public string acctName { get; set; }
        public string tradeCompanyName { get; set; }
        public string billIdName { get; set; }
        public string orderType { get; set; }
        public string salesChannel { get; set; }
        public string balanceName { get; set; }
        public string status { get; set; }
        public string orderProdLine { get; set; }
        public DateTime? lHexpectedArrivedDate { get; set; }
        public string source { get; set; }
        public string lHbuType { get; set; }
        public DateTime? submissionDate { get; set; }
        public string lHcusSelfNo { get; set; }
        public string lHreviweStatus { get; set; }
        public string productPeopleName { get; set; }
        public string lHpromotionPolicy { get; set; }
        public DateTime? lHdiscountEndTime { get; set; }
        public string OBBalance { get; set; }
        public string lHcontractNo { get; set; }
        public string lHprojectName { get; set; }
        public string lHProjectEndTime { get; set; }
        public string consignee { get; set; }
        public string consigneePhone { get; set; }
        public string consigneeAddress { get; set; }
        public decimal lHtotalVolume { get; set; }
        public decimal lHtotalNum { get; set; }
        public decimal totalAmount { get; set; }
        public string lHrebatesMoney { get; set; }
        public string lHadvertisingMoney { get; set; }
        public string lHdepositOrNot { get; set; }
        public string lHdiscountType { get; set; }
        public decimal lHdiscountRate { get; set; }
        public string remarks { get; set; }
        public string lHDeliRequire { get; set; }
        public string lHRequestRequire { get; set; }
        public string lHManAdvice { get; set; } 
        public DateTime? attr2 { get; set; }
        public DateTime? attr3 { get; set; }
        [NotMapped]
        public SaleOrderDetailed[] saleOrderItemList { get; set; }
        public string KeyId()
        {
            return " AND orderId = '" + orderId + "'";
        }
    }
    /// <summary>
    /// 定制订单明细
    /// </summary>
    [Table("LH_SALEORDERDETAILED")]
    public class SaleOrderDetailed
    {
        public string Id { get; set; }
        public string orderId { get; set; }
        public string prodCode { get; set; }
        public string prodName { get; set; }
        public string lHcustomProdCode { get; set; }
        public string lHcomments { get; set; }
        public string prodStandard { get; set; }
        public string lHprodColor { get; set; }
        public decimal QTY { get; set; }
        public string createdBy { get; set; }
        public decimal lHcustomPercent { get; set; }
        public string lHprodLine { get; set; }
        public string lHmodel { get; set; }
        public string CRMcomments { get; set; }
        public DateTime? itemDeliveryDate { get; set; }
        public decimal lHdealerPrice { get; set; }
        public decimal lHfinalDiscount { get; set; }
        public decimal lHDiscountPrice { get; set; }
        public decimal lHdealerAmount { get; set; }
        public decimal lHPredictPrice { get; set; }
        public decimal lHprojectPrice { get; set; }
        public decimal lHbilleQty { get; set; }
        public decimal lHunbilleQty { get; set; }
        public decimal lHcancelQty { get; set; }
        public double lHvolume { get; set; }
        public double lHProdWight { get; set; }
        public double lHToVolume { get; set; }
        public double lHProdToWight { get; set; }
        public string lHprodUnit { get; set; }
        public DateTime? lHrowcreatedDate { get; set; }
        public string lHProductionNumber { get; set; }
        public string lHproductStatus { get; set; }
        public string lHvalidateorNot { get; set; }
        public string lHrowSource { get; set; }
        public string customStatus { get; set; }
    }
}
