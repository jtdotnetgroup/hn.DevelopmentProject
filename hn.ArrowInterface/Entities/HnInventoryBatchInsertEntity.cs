using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace hn.ArrowInterface.Entities
{
    [Table("LH_SR_REPORT")]
    public class HnInventoryBatchInsertEntity
    {
        [Column("FID")] public decimal? dataKey { get; set; }

        [Column("PART_NUMBER")] public string lHProdCode { get; set; }

        [Column("PART_NAME")] public string lHProdDesc { get; set; }

        [Column("SALES_AREA")] public string region { get; set; }

        [Column("COLOR_NO")] public string lHProdColor { get; set; }

        [Column("FUNIT")] public string lHProdUnit { get; set; }

        [Column("FOBQTY")] public decimal? peForBalance { get; set; }

        [Column("FINQTY")] public decimal? peWarehouse { get; set; }

        [Column("FOUTQTY")] public decimal? peExWarehouse { get; set; }

        [Column("FCBQTY")] public decimal? peBalance { get; set; }

        public DateTime? FTime { get; set; }
    }

    public class HnlnventoryBatchInsertEntityDto
    {
        public decimal? dataKey { get; set; }
        public string lHProdCode { get; set; }
        public string lHProdDesc { get; set; }
        public string region { get; set; }
        public string lHProdColor { get; set; }
        public string lHProdUnit { get; set; }
        public decimal? peForBalance { get; set; }
        public decimal? peWarehouse { get; set; }
        public decimal? peExWarehouse { get; set; }
        public decimal? peBalance { get; set; }
    }
}