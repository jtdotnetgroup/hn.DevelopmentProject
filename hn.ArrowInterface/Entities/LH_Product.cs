using System;
using System.ComponentModel.DataAnnotations.Schema;
using hn.AutoSyncLib.Model;

namespace hn.ArrowInterface.Entities
{
    [Table("LH_PRODUCT")]
    public class LH_Product : IComputeFID
    {
        public DateTime LastUpdate { get; set; }
        public string lHprodChannel { get; set; }
        public string prodStandard { get; set; }

        public string FID { get; set; }

        public string ProdCode { get; set; }

        public string ProdName { get; set; }

        public decimal ProdVolume { get; set; }

        public string LHprodLine { get; set; }

        [Column("ProdStatus")] public string ProdStatus { get; set; }

        [Column("prodModel")] public string LHprodModel { get; set; }

        public string LHprodType { get; set; }

        public string LHprodType1 { get; set; }

        public string LHprodType2 { get; set; }

        public string LHprodSign { get; set; }

        public void ComputeFID()
        {
            throw new NotImplementedException();
        }
    }
}