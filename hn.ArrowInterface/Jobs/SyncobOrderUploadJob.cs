using System;
using System.Collections.Generic;
using System.Linq;
using hn.ArrowInterface.Entities;
using hn.ArrowInterface.RequestParams;
using hn.ArrowInterface.WebCommon;
using hn.Common;
using Newtonsoft.Json;

namespace hn.ArrowInterface.Jobs
{
    [Obsolete("实时调用，不需要定时同步")]
    public class SyncobOrderUploadJob : AbsJob
    {
        protected override AbstractRequestParams GetParams()
        {
            var where = new LH_OUTBOUNDORDER() { FStatus = 1 };
            var bills = Helper.GetWhere(where).FirstOrDefault();
            if (bills == null)
            {
                return null;
            }

            ObOrderUploadParam pars = new ObOrderUploadParam() { lhodoID = bills.lhodoID, lhplateNo = bills.FCarno };

            return pars;
        }


        public override bool Sync()
        {
            var token = GetToken();
            var where = new LH_OUTBOUNDORDER() { FStatus = 1 };
            var bills = Helper.GetWhere(where);

            foreach (var bill in bills)
            {
                ObOrderUploadParam pars = new ObOrderUploadParam() { lhodoID = bill.lhodoID, lhplateNo = $"{bill.FCarno}-{DateTime.Now.Millisecond}" };
                
                var result = Interface.obOrderUpload(token.Token, pars);

                if (result.Success)
                {
                    bill.FStatus = 2;
                    bill.FCarno = pars.lhplateNo;
                    Helper.Update(bill);
                }
            }
            return true;

        }
    }
}
