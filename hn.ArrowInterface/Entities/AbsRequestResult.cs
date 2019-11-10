using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace hn.ArrowInterface.Entities
{
    public  class AbsRequestResult<T>:AbsRequestResult
    {
        public AbsRequestResult() {
            Rows = new List<T>(); 
        }
        public List<T> Rows { get; set; }
         
        public T Order { get; set; } 
        //
        public List<T> item { get; set; }
}

    /// <summary>
    /// 通用字段，所有请求的返回结果均包含以下两个字段
    /// </summary>
    public  class AbsRequestResult
    {
        [NotMapped]
        public string Message { get; set; }
        [NotMapped]
        public int Total { get; set; }
        [NotMapped]
        public bool Success { get; set; }
    } 
    /// <summary>
    /// 通用字段，所有返回结果中为多条记录的都包含以下字段
    /// </summary>
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