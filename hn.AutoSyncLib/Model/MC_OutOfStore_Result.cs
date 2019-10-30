using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace hn.AutoSyncLib.Model
{
    public class MC_OutOfStore_Result:MC_Request_BaseResult<MC_OutofStore_ResultInfo>
    {
       
    }

    [Table("MN_CKD")]
    public class MC_OutofStore_ResultInfo: IComputeFID, IFID
    {
        public string pzhm { get; set; }
        public string pjhm { get; set; }
        public DateTime rq { get; set; }
        public string khhm { get; set; }
        public string khmc { get; set; }
        public string cppz { get; set; }
        public string cpgg { get; set; }
        public string cpxh { get; set; }
        public string cpdj { get; set; }
        public string dw { get; set; }
        public decimal ks { get; set; }
        public decimal sl { get; set; }
        public string DB { get; set; }
        public string cpsh { get; set; }
        public string cpcm { get; set; }
        public string carno { get; set; }

        [Column("TPackage")]
        public string package { get; set; }

        [Key]
        public string FID { get; set; }

        public void ComputeFID()
        {
            var h256= SHA256.Create();
            string value = pzhm + rq + pjhm + khhm + cpgg + cpxh + cpsh + cpcm+package+carno;

            var hasData = Encoding.UTF8.GetBytes(value);

            var has= h256.ComputeHash(hasData);

            string fid = BitConverter.ToString(has).Replace("-", "");
            FID = fid;
        }
    }


}