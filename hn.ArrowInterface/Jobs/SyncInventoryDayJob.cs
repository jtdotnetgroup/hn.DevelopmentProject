using hn.ArrowInterface.Entities;
using hn.ArrowInterface.WebCommon;

namespace hn.ArrowInterface.Jobs
{
    public class SyncInventoryDayJob:AbsJob
    {
        public override bool Sync()
        {
            var token = GetToken();

            var where=new HnInventoryBatchInsertEntity();

            var data = Helper.GetWhere<HnInventoryBatchInsertEntity>(where);

            var result = Interface.HnInventoryBatchInsert(token.Token, data);

            return result.Success;
        }

        protected override AbstractRequestParams GetParams()
        {
            throw new System.NotImplementedException();
        }
    }
}