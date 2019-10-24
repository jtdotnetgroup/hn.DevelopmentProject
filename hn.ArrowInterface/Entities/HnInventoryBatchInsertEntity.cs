namespace hn.ArrowInterface.Entities
{
    public class HnInventoryBatchInsertEntity
    {
        public  string DataKey { get; set; }
        public string LHProdCode { get; set; }
        public string LHProdDesc { get; set; }
        public string Region { get; set; }
        public string LHProdColor { get; set; }
        public string LHProdUnit { get; set; }
        public decimal PeForBalance { get; set; }
        public decimal PeWarehouse { get; set; }
        public decimal PeExWarehouse { get; set; }
        public decimal PeBalance { get; set; }
    }
}