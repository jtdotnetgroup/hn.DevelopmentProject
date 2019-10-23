using System.Collections.Generic;

namespace hn.ArrowInterface.Entities
{
    public  class AbsRequestResult<T>:AbsRequestResult
    {
        public List<T> Rows { get; set; }
    }

    public  class AbsRequestResult
    {
        public int Total { get; set; }
        public bool Success { get; set; }
    }

    public class CommomPropertyObject
    {
        public string CreateBy { get; set; }
        public string Created { get; set; }
        public string LastUpdated { get; set; }
        public string LastUpdateBy { get; set; }
        public string Id { get; set; }
        public string Corpid { get; set; }
        public string CreateId { get; set; }
        public string UserCorpid { get; set; }
        public int Page { get; set; }
        public int Row { get; set; }
        public int Total { get; set; }
        public int TotalPage { get; set; }
        public int StartRow { get; set; }
        public int EndRow { get; set; }
        public bool TotalFlag { get; set; }
        public bool OnlyCountFlag { get; set; }
        public string OrderByClause { get; set; }
        public string PrivateAddr { get; set; }
        public bool HaveOrderClause { get; set; }
        public bool TransFrontConditionFlag { get; set; }
        public bool TransOrderConditionFlag { get; set; }
    }
}