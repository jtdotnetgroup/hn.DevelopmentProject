using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace hn.ArrowInterface.Entities
{
    [Table("LH_INVENTORYREPORT")]
    public class HnObOrderBatchInsertEntity
    {
        [Column("FID")]
        public  decimal? DataKey { get; set; }
        [Column("PART_NUMBER")]
        public string HnProdCode { get; set; }
        [Column("PART_NAME")]
        public string HnProdDesc { get; set; }
        [Column("SALES_AREA")]
        public string Region { get; set; }
        [Column("COLOR_NO")]
        public string HnProdColor { get; set; }
        [Column("FUNIT")]
        public  string MainUnit { get; set; }
        [Column("FQTY")]
        public decimal? HnNumber { get; set; }

        public DateTime FTime { get; set; }


    }

    
    public class HnObOrderBatchInsertEntityDto
    {
        public decimal? dataKey { get; set; }
        public string hnProdCode { get; set; }
        public string hnProdDesc { get; set; }
        public string region { get; set; }
        public string hnProdColor { get; set; }
        public string mainUnit { get; set; }
        public decimal? hnNumber { get; set; }

        public HnObOrderBatchInsertEntityDto(HnObOrderBatchInsertEntity data)
        {
            this.dataKey = data.DataKey;
            this.hnProdCode = data.HnProdCode;
            this.hnProdDesc = data.HnProdDesc;
            this.region = data.Region;
            this.hnProdColor = data.HnProdColor;
            this.mainUnit = data.MainUnit;
            this.hnNumber = data.HnNumber;
        }
    }
}