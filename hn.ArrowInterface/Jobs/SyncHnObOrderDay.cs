using System;
using System.Collections.Generic;
using System.Linq;
using hn.ArrowInterface.Entities;
using hn.ArrowInterface.WebCommon;
using Quartz;

namespace hn.ArrowInterface.Jobs
{
    public class SyncHnObOrderDay:AbsJob 
    {
        protected override AbstractRequestParams GetParams()
        {
            throw new NotImplementedException();
        }

        public override bool Sync()
        {
            var token = GetToken();

            var where = new HnObOrderBatchInsertEntity();
            where.FTime = DateTime.Now.Date;

            var data = Helper.GetWhere(where);

            List<HnObOrderBatchInsertEntityDto> postData = new List<HnObOrderBatchInsertEntityDto>();

            for (int i = 0; i < data.Count; i++)
            {
                var item = data[i];
                item.DataKey = ++i;

                postData.Add( new HnObOrderBatchInsertEntityDto(item));
            }

            var result = Interface.HnObOrderDayBatchInsert(token.Token, postData);

            return result.Success;

        }

        protected override AbstractRequestParams GetParams()
        {
            throw new NotImplementedException();
        }
    }
}