namespace hn.ArrowInterface.Entities
{
    public class QueryProdPageResult:CommomPropertyObject
    {
        private string prodCode;
        private string prodName;
        private string prodMatSeries;
        private decimal prodVolume;
        private string comments;
        private string deptId;
        private string deptName;
        private string lHprodLine;
        private string lHprodStatus;
        private string lHprodModel;
        private string lHprodType;
        private string lHprodType1;
        private string lHprodType2;
        private string lHprodSign;

        public string ProdCode
        {
            get => prodCode;
            set => prodCode = value;
        }

        public string ProdName
        {
            get => prodName;
            set => prodName = value;
        }

        public string ProdMatSeries
        {
            get => prodMatSeries;
            set => prodMatSeries = value;
        }

        public decimal ProdVolume
        {
            get => prodVolume;
            set => prodVolume = value;
        }

        public string Comments
        {
            get => comments;
            set => comments = value;
        }

        public string DeptId
        {
            get => deptId;
            set => deptId = value;
        }

        public string DeptName
        {
            get => deptName;
            set => deptName = value;
        }

        public string LHprodLine
        {
            get => lHprodLine;
            set => lHprodLine = value;
        }

        public string LHprodStatus
        {
            get => lHprodStatus;
            set => lHprodStatus = value;
        }

        public string LHprodModel
        {
            get => lHprodModel;
            set => lHprodModel = value;
        }

        public string LHprodType
        {
            get => lHprodType;
            set => lHprodType = value;
        }

        public string LHprodType1
        {
            get => lHprodType1;
            set => lHprodType1 = value;
        }

        public string LHprodType2
        {
            get => lHprodType2;
            set => lHprodType2 = value;
        }

        public string LHprodSign
        {
            get => lHprodSign;
            set => lHprodSign = value;
        }
    }
}