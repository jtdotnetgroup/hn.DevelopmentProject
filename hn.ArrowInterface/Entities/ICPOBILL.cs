using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hn.ArrowInterface.Entities
{
    public class ICPOBILL
    {
        public string FID { get; set; }
        public string FBRANDID { get; set; }
        public string FTRANSTYPE { get; set; }
        public string FCLIENTID { get; set; }
        public string FBILLNO { get; set; }
        public DateTime FDATE { get; set; }
        public string FBILLER { get; set; }
        public string FTELEPHONE { get; set; }
        public DateTime FBILLDATE { get; set; }
        public decimal FSTATUS { get; set; }
        public DateTime FCHECKDATE { get; set; }
        public string FREMARK { get; set; }
        public decimal FSTATE { get; set; }
        public decimal FSYNCSTATUS { get; set; }
        public string FICSEBILLID { get; set; }
        public string FDESBILLNO { get; set; }
        public string FCOMPANY { get; set; }
        public string FPROJECTNO { get; set; }
        public string FACCOUNT { get; set; }
        public string FPOTYPE { get; set; }
        public string FPRICEPOLICY { get; set; }
        public string FNOTE { get; set; }
        public DateTime FTIMESTAMP { get; set; }

        //COMMENT ON COLUMN "PURCHASE"."ICPOBILL"."FSTATUS" IS '1:草稿  2:待审核  3:审核通过  4:审核不通过  5:关闭  6:完成  7:部分审核';
        //COMMENT ON COLUMN "PURCHASE"."ICPOBILL"."FSTATE" IS '0：禁用  1：正常    默认：1';
        //COMMENT ON COLUMN "PURCHASE"."ICPOBILL"."FSYNCSTATUS" IS '1：未同步  2：已同步  3：同步失败';
    }
}
