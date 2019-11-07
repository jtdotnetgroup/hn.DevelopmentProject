using hn.ArrowInterface.WebCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hn.ArrowInterface.RequestParams
{
    public class QueryLHInventoryPageParam : AbstractRequestParams
    {
        public string dealerCode { get; set; }
    }
}
