using System.Collections.Generic;

namespace hn.AutoSyncLib
{
    public interface IComputeParams<T>
    {
        List<T> ComputeParams(string token,string rq1,string rq2,int pageSize,int pageIndex);
    }
}