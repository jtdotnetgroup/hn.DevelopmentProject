using System.Collections.Generic;
using hn.ArrowInterface.WebCommon;

namespace hn.ArrowInterface.RequestParams
{
    public class ObOrderUploadParam: AbstractRequestParams
{
        public string lhodoID { get; set; }
        public string lhplateNo { get; set; }
    }

    public class ObOrderUploadPrarams 
    {
        public List<ObOrderUploadParam> Rows { get; set; }
    }
}