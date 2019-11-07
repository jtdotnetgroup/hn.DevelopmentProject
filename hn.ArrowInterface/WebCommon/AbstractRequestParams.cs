using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hn.ArrowInterface.WebCommon
{
    public abstract class AbstractRequestParams
    {
        public Dictionary<string, object> ToDictionary()
        {
            var result=new Dictionary<string,object>();

            var pis = this.GetType().GetProperties();

            foreach (var pi in pis)
            {
                var value = pi.GetValue(this, null);
                result.Add(pi.Name, value);
            }

            return result;
        }
    }
}
