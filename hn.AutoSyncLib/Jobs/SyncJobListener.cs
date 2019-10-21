using System.Threading;
using System.Threading.Tasks;
using Quartz;

namespace hn.AutoSyncLib.Jobs
{
    public class SyncJobListener:IJobListener
    {

        public SyncJobListener(string name = "syncJobListener")
        {
            this.Name = name;
            JobResult = false;
        }

        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return new Task(() =>
            {
                JobResult = context.Result;
            });
        }

        public object JobResult { get; set; }

        public string Name { get; set; }
    }
}