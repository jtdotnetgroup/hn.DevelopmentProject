using System;
using System.Collections.Generic;
using AutoMapper;
using AutoMapper.Configuration;
using hn.ArrowInterface.Entities;
using hn.ArrowInterface.WebCommon;

namespace hn.ArrowInterface.Jobs
{
    public class SyncInventoryDayJob:AbsJob
    {
        protected override AbstractRequestParams GetParams()
        {
            throw new System.NotImplementedException();
        }

        public override bool Sync()
        {
            var token = GetToken();

            var where=new HnInventoryBatchInsertEntity(){FTime = DateTime.Parse("2019-11-02")};

            var data = Helper.GetWhere(where);

            var mapper = Mapper.Instance;

            var pars = mapper.Map<List<HnlnventoryBatchInsertEntityDto>>(data);

            var result = Interface.HnInventoryBatchInsert(token.Token, pars);

            if (result.Success)
                UpdateSyncRecord(null);

            return result.Success;
        }

    }
}