using System;

namespace hn.AutoSyncLib.Model
{
    public interface CommonRequestParams:CommonParams
    {
        string rq1 { get; set; }
        string rq2 { get; set; }
        int pageSize { get; set; }
        int pageIndex { get; set; }
    }
}