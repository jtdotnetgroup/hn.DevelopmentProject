using System.ComponentModel.DataAnnotations.Schema;

namespace hn.ArrowInterface.Entities
{
    [Table("LH_SR_REPORT")]
    public class HnInventoryBatchInsertEntity
    {
        [Column("FID")]
        public  string DataKey { get; set; }
        [Column("PART_NUMBER")]
        public string LHProdCode { get; set; }
        [Column("PART_NAME")]
        public string LHProdDesc { get; set; }
        [Column("SALES_AREA")]
        public string Region { get; set; }
        [Column("COLOR_NO")]
        public string LHProdColor { get; set; }
        [Column("FUNIT")]
        public string LHProdUnit { get; set; }
        [Column("FOBQTY")]
        public decimal PeForBalance { get; set; }
        [Column("FINQTY")]
        public decimal PeWarehouse { get; set; }
        [Column("FOUTQTY")]
        public decimal PeExWarehouse { get; set; }
        [Column("FCBQTY")]
        public decimal PeBalance { get; set; }
    }
}