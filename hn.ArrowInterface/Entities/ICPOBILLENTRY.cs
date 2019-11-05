using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hn.ArrowInterface.Entities
{
    public class ICPOBILLENTRY
    {
        public string FID { get; set; }
        public string FICPOBILLID { get; set; }
        public string FPLANID { get; set; }
        public decimal FENTRYID { get; set; }
        public string FBATCHNO { get; set; }
        public string FCOLORNO { get; set; }
        public decimal FPRICE { get; set; }
        public decimal FADVQTY { get; set; }
        public decimal FSRCQTY { get; set; }
        public decimal FSRCCOST { get; set; }
        public int FSTATUS { get; set; }
        public DateTime FNEEDDATE { get; set; }
        public decimal FCOMMITQTY { get; set; }
        public string FREMARK { get; set; }
        public int FSTATE { get; set; }
        public string FICSEBILLID { get; set; }
        public decimal FENTRYTOTAL { get; set; }
        public decimal FDESBILLENTRY { get; set; }
        public string FSRCCODE { get; set; }
        public string FITEMID { get; set; }
        public string FSRCNAME { get; set; }
        public string FSRCMODEL { get; set; }
        public string FLEVEL { get; set; }
        public string FSTOCKNO { get; set; }
        public string FCONTRACTNO { get; set; }
        public string FUNIT { get; set; }
        public decimal FAUDQTY { get; set; }
        public decimal FAMOUNT { get; set; }
        public string FERR_MESSAGE { get; set; }
        public string ICPRBILLENTRYIDS { get; set; }
        public string THDBMDETAIL { get; set; }
        //COMMENT ON COLUMN "PURCHASE"."ICPOBILLENTRY"."FSTATUS" IS '1:草稿  2:待审核  3:审核通过  4:审核不通过  5:关闭  6:完成';
        //COMMENT ON COLUMN "PURCHASE"."ICPOBILLENTRY"."FSTATE" IS '0：禁用  1：正常    默认：1';
        //COMMENT ON COLUMN "PURCHASE"."ICPOBILLENTRY"."FICSEBILLID" IS '发货单编号'; 
    }
}
