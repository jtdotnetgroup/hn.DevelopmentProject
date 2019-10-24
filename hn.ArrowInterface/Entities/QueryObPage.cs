using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace hn.ArrowInterface.Entities
{
    /// <summary>
    /// 出库单头
    /// </summary>
    [Table("LH_OutboundOrder")]
    public class QueryObPage
    {
        public string lhodoID { get; set; }
        public string lhodoNo { get; set; }
        public string lhtotalNumber { get; set; }
        public string lhodoType { get; set; }
        public string acctCode { get; set; }
        public string acctName { get; set; }
        public string lhtotalAmount { get; set; }
        public string lhtotalVolume { get; set; }
        public string lhdeliveryBase { get; set; }
        public string lhdeliveryMan { get; set; }
        public string lhcontractNo { get; set; }
        public string billAcctName { get; set; }
        public string billAcctCode { get; set; }
        public string lhadvertisingPoint { get; set; }
        public string lhdeliveryTime { get; set; }
        public string lhplateNo { get; set; }
        public string lhrebatesPoint { get; set; }
        public string lHbuType { get; set; }
        public string lhmark { get; set; }
        public string lhorgCode { get; set; }
        public string attr2 { get; set; }
        public string attr3 { get; set; }
        
    }
    /// <summary>
    /// 出库单明细
    /// </summary>
    [Table("LH_OutboundOrderDetailed")]
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
        public string lhsentedQty { get; set; }
        public string lhprodUnit { get; set; }
        public string lhoriginalPrice { get; set; }
        public string lhdiscountRate { get; set; }
        public string lhdiscountPrice { get; set; }
        public string lhamount { get; set; }
        public string lhvolume { get; set; }
    }
}
