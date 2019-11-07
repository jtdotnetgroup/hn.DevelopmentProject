using hn.AutoSyncLib.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using SHA256 = System.Security.Cryptography.SHA256;

namespace hn.ArrowInterface.Entities
{
    [Table("LH_INVENTORY")]
    public class QuerylHInventoryPageResult :IComputeFID
    {
        public string LHProdId { get; set; }
        public string LHProdCode { get; set; }
        public string LHProdName { get; set; }
        public string LHProdUnit { get; set; }
        public decimal LHProdVolume { get; set; }
        public decimal LHInventoryQty { get; set; }
        public string LHproductionBase { get; set; }
        public string LHOrgName { get; set; }
        public string LHProdLine { get; set; }
        public string LHProdType { get; set; }
        public string LHProdModel { get; set; }
        public string lHProdColor { get; set; }

        [Key]
        public string FID { get; set; }

        public void ComputeFID()
        {
            var h256 = SHA256.Create();
            string value = LHProdId + LHProdCode + LHProdName + LHProdUnit + LHProdVolume + LHInventoryQty + LHproductionBase + LHOrgName + LHProdLine + LHProdType+ LHProdModel;
            var hasData = Encoding.UTF8.GetBytes(value);
            var has = h256.ComputeHash(hasData);
            string fid = BitConverter.ToString(has).Replace("-", "");
            FID = fid;
        }
    }

     
}