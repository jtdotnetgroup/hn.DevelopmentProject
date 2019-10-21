using System.Collections.Generic;
using System.Linq;

namespace hn.AutoSyncLib.Model
{
    public class MC_Request_BaseParams:CommonParams
    {
        public string action { get; set; }
        public string comid { get; set; }
        public string khhm { get; set; }
        public string token { get; set; }

        public MC_Request_BaseParams(string action, string comid = "100",string khhm="300384")
        {
            this.action = action;
            this.comid = comid;
            this.khhm = khhm;
        }

        public Dictionary<string, string> ModelToDic<T>() where T:MC_Request_BaseParams
        {
            var t = typeof(T);
            var pis = t.GetProperties().ToList();

            Dictionary<string,string> result=new Dictionary<string, string>();

            pis.ForEach(p =>
            {
                var value = p.GetValue(this, null);
                if(value!=null)
                    result.Add(p.Name,value.ToString());
            });

            return result;
        }
    }
}