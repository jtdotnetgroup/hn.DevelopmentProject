using System;
using hn.AutoSyncLib;
using hn.AutoSyncLib.Jobs;
using hn.AutoSyncLib.Model;
using hn.AutoSyncLib.Schedule;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace hn.AutoSyncLibTests
{
    [TestClass]
    public class MC_PickUpGoodsTests
    {
        [TestMethod]
        public void TestMethod1()
        {

            var rq1 = DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm:ss");
            var rq2 = DateTime.Now.Date.AddDays(1).AddMilliseconds(-1000).ToString("yyyy-MM-dd HH:mm:ss");


        }
    }
}
