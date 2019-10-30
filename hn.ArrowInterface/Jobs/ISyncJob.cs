using Quartz;

namespace hn.ArrowInterface.Jobs
{
    public interface ISyncJob
    {
        bool Sync();
    }
}