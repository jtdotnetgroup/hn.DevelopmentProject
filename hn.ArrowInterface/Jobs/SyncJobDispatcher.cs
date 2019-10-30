using System.Linq;
using System.Reflection;

namespace hn.ArrowInterface.Jobs
{
    public class SyncJobDispatcher:AbsJob
    {
        public override bool Sync()
        {
            var jobs= Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(ISyncJob)));

            return true;
        }
    }
}