using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hn.ArrowInterface.Entities
{

    /// <summary>
    /// OA审核状态
    /// </summary>
    public class AcctOAStatus
    {
        public string orderNo { get; set; }
        public string lHreviweStatus { get; set; }
        public string lHOutSystemLineID { get; set; }
        public string receiptReturn { get; set; }
        public string receiptNoReturn { get; set; }
    }
}
