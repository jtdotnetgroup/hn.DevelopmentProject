using System; 
using System.ComponentModel.DataAnnotations.Schema; 

namespace hn.ArrowInterface.Entities
{
    /// <summary>
    /// 出库单头
    /// </summary>
    [Table("LH_OUTBOUNDORDER")]
    public class QueryObPage
    {
        public string lhodoID { get; set; }
        [NotMapped]
        public string lhodoId { get; set; }
        public string lhodoNo { get; set; }
        public decimal lhtotalNumber { get; set; }
        public string lhodoType { get; set; }
        public string acctCode { get; set; }
        public string acctName { get; set; }
        public decimal lhtotalAmount { get; set; }
        public decimal lhtotalVolume { get; set; }
        public string lhdeliveryBase { get; set; }
        public string lhdeliveryMan { get; set; }
        public string lhcontractNo { get; set; }
        public string billAcctName { get; set; }
        public string billAcctCode { get; set; }
        public decimal lhadvertisingPoint { get; set; }
        public DateTime lhdeliveryTime { get; set; }
        public string lhplateNo { get; set; }
        public decimal lhrebatesPoint { get; set; }
        public string lHbuType { get; set; }
        public string lhmark { get; set; }
        public string lhorgCode { get; set; }
        public DateTime attr2 { get; set; }
        public DateTime attr3 { get; set; }
        [NotMapped]
        public QueryObPageDetailed[] items { get; set; }
        public string KeyId()
        {
            return " AND lhodoID = '" + lhodoID + "'";
        }
    }
    /// <summary>
    /// 出库单明细
    /// </summary>
    [Table("LH_OUTBOUNDORDERDETAILED")]
    public class QueryObPageDetailed {
        public string lhodoID { get; set; }
        public string lhodoRowID { get; set; }
        public string lhprodCode { get; set; }
        public string lhprodStandard { get; set; }
        public string lHOrderCode { get; set; }
        public string custSelfNum { get; set; }
        public string numOfPackage { get; set; } 
        public string lhprodColor { get; set; }
        public string colorNum { get; set; }
        public decimal lhsentedQty { get; set; }
        public string lhprodUnit { get; set; }
        public decimal lhoriginalPrice { get; set; }
        public decimal lhdiscountRate { get; set; }
        public decimal lhdiscountPrice { get; set; }
        public decimal lhamount { get; set; }
        public decimal lhvolume { get; set; }
        public string KeyId()
        {
            return " AND lhodoID = '" + lhodoID + "'";
        }
    }

}
