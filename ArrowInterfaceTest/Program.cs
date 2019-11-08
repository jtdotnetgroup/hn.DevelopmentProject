using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using hn.ArrowInterface;
using hn.ArrowInterface.Entities;
using hn.ArrowInterface.Jobs;
using hn.ArrowInterface.Schedule;
using hn.Common;

namespace ArrowInterfaceTest
{
    class Program
    {
        static ArrowInterface iInterface = new ArrowInterface();
        static void Main(string[] args)
        {


            LogHelper.Init(Console.Out);

            ///*TestSyncQueryObOrderJob*/();
            TestProductJob();
            //TestPolicyJob();
            //TestQueryObPageJob();

            //TestSaleOrderJob();

            //TestSaleOrderUploadJob();

            //TestAcctOAStatusJob();

            //TestSyncInventoryDayJob();

            //TestSyncHnObOrderDay();

            //TestQueryProdPage();
            Console.WriteLine("OK");
            Console.ReadKey();
        }

        static void TestLogin()
        {
            iInterface.GetToken();
        }

        static void TestQueryLHInventoryPage()
        {
            var token = iInterface.GetToken();
            //var result = iInterface.QueryLHInventoryPage(token.Token); 
        }

        static void TestQueryProdPage()
        {
            var token = iInterface.GetToken();
            //var result = iInterface.QueryProdPage(token.Token); 
        }

        static void TestQueryPolicy()
        {
            //var token = iInterface.GetToken();
            //var result = iInterface.QueryPolicy(token.Token); 
        }

        static void TestDelete()
        {
            OracleDBHelper helper = new OracleDBHelper("");
            AuthorizationToken token = new AuthorizationToken();

            helper.Delete<AuthorizationToken>("");

        }
        static void TestInventoryJob()
        {
            ISyncJob job = new SyncInventoryJob();
            job.Sync(); 
        }

        static void TestProductJob()
        {
            ISyncJob job = new SyncProductJob();
            job.Sync(); 
        }

        static void TestPolicyJob()
        {
            ISyncJob job = new SyncPolicyJob();
            job.Sync(); 
        }
        /// <summary>
        /// 7  销售订单上传
        /// </summary>
        static void TestSaleOrderUploadJob()
        {
            ISyncJob job = new SyncSaleOrderUploadResultJob();
            job.Sync(); 
        }
        /// <summary>
        /// 8  审核状态回传
        /// </summary>
        static void TestAcctOAStatusJob()
        {
            //ISyncJob job = new SyncAcctOAStatusJob();
            //job.Sync(); 
        }
        /// <summary>
        /// 9  定制订单下载
        /// </summary>
        static void TestSaleOrderJob()
        {
            ISyncJob job = new SyncSaleOrderJob();
            job.Sync(); 
        }
        /// <summary>
        /// 10  物流部开单记录下载
        /// </summary>
        static void TestQueryObPageJob()
        {
            ISyncJob job = new SyncQueryObPageJob();
            job.Sync();
        }
        static void TestSyncHnObOrderDay()
        {
            ISyncJob job = new SyncHnObOrderDay();
            job.Sync();
            Console.WriteLine("OK");
        }

        static void TestSyncInventoryDayJob()
        {
            ISyncJob job = new SyncInventoryDayJob();
            job.Sync();
            Console.WriteLine("OK");
        }

        static void TestSyncQueryObOrderJob()
        {
            ISyncJob job   =new SyncqueryObOrderPageJob();
            job.Sync();
            
        }
    }
}
