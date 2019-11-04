using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace hn.ArrowInterface.Entities
{

    /// <summary>
    /// OA审核状态
    /// </summary>
    [Table("LH_ACCTOASTATUS")]
    public class AcctOAStatus
    {
        public string orderNo { get; set; }
        public string lHreviweStatus { get; set; }
        [NotMapped]
        public List<AcctOAStatusDetailed> saleOrderItemList { get; set; }
    }
    [Table("LH_ACCTOASTATUSDETAILED")]
    public class AcctOAStatusDetailed {
        public string lHOutSystemLineID { get; set; }
        public string receiptReturn { get; set; }
        public string receiptNoReturn { get; set; }
        public string orderNo { get; set; }
    }
}
