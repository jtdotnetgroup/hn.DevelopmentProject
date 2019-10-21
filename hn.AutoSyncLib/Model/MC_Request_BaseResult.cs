using System.Collections.Generic;

namespace hn.AutoSyncLib.Model
{
    public class MC_Request_BaseResult<T>
    {
        public int Status { get; set; }
        public string Msg { get; set; }
        public string Comid { get; set; }
        public int TotalCount { get; set; }

        public List<T> resultInfo { get; set; }
    }
}