using Microsoft.VisualStudio.TestTools.UnitTesting;
using hn.AutoSyncLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hn.AutoSyncLib.Model;

namespace hn.AutoSyncLib.Tests
{
    [TestClass()]
    public class MC_GetTokenTests
    {
        [TestMethod()]
        public void GetTokenTest()
        {
            var par = new MC_getToken_Params("AF7E9E381F3D7A7F9AD11E0186306031");

            string url = "https://tms.monalisagroup.com.cn/mapi/doAction";

            MC_GetToken request = MC_GetToken.GetInstance();
            var data = request.Request<MC_getToken_Result, MC_getToken_Params,TokenInfo>(par);

            Console.WriteLine(data.Result.token);
        }

        
    }
}