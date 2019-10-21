using System;
using hn.AutoSyncLib.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace hn.AutoSyncLib.Tests
{
    [TestClass()]
    public class MC_OutStoreSyncTest
    {
        [TestMethod()]
        public void SyncTest()
        {
            var outofstore = MC_OutOfStore.GetInstance();
            var gettoken = MC_GetToken.GetInstance();

            var token = gettoken.Request<MC_getToken_Result, MC_getToken_Params>(new MC_getToken_Params()).Result.token;

            int total = 0;
            int pagesize = 50000;
            int pageindex = 1;
            int pagecount = 0;

            do
            {
                var result =
                    outofstore.Request<MC_OutOfStore_Result, MC_OutOfStore_Params>(
                        new MC_OutOfStore_Params(token, "20190501", "20190821", pagesize, pageindex));

                total = result.Result.TotalCount;

                if (pagecount == 0)
                {
                    pagecount = total / pagesize;

                    if (total % pagesize > 0)
                    {
                        pagecount++;
                    }
                }

                Console.WriteLine(pageindex);

                pageindex++;

            } while (pageindex<=pagecount);
           
                
            
            

        }
    }
}