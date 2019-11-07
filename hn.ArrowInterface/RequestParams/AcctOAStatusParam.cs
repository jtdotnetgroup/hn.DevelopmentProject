using System.Collections.Generic;
using hn.ArrowInterface.WebCommon;

namespace hn.ArrowInterface.RequestParams
{
    public class AcctOAStatusParam:AbstractRequestParams
    {
        public string acctCode { get; set; }
        public List<string> idStrings { get; set; }
    }
}