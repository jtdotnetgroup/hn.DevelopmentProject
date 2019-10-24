using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using hn.ArrowInterface;
using hn.ArrowInterface.Entities;
using hn.ArrowInterface.Jobs;
using hn.AutoSyncLib.Common;

namespace ArrowInterfaceTest
{
    class Program
    {
        static ArrowInterface iInterface = new ArrowInterface();
        static void Main(string[] args)
        {
            TestPolicyJob();
            
        }

        static void TestLogin()
        {
            iInterface.GetToken();
        }

        static void TestQueryLHInventoryPage()
        {
            var token = iInterface.GetToken();
            var result= iInterface.QueryLHInventoryPage(token.Token);
            Console.WriteLine("ok");
        }

        static void TestQueryProdPage()
        {
            var token = iInterface.GetToken();
            var result = iInterface.QueryProdPage(token.Token);
            Console.WriteLine("ok");          
        }

        static void TestQueryPolicy()
        {
            var token = iInterface.GetToken();
            var result = iInterface.QueryPolicy(token.Token);

            Console.WriteLine("OK");
        }

        static void TestDelete()
        {
            OracleDBHelper helper = new OracleDBHelper("");
            AuthorizationToken token=new AuthorizationToken();

            helper.Delete<AuthorizationToken>("");

        }

        static void TestInventoryJob()
        {
            ISyncJob job=new SyncInventoryJob();
            job.Sync();
             Console.WriteLine("OK");
        }

        static void TestProductJob()
        {
            ISyncJob job=new SyncProductJob();
            job.Sync();
            Console.WriteLine("OK");
            Console.ReadKey();
        }

        static void TestPolicyJob()
        {
            ISyncJob job=new SyncPolicyJob();
            job.Sync();
            Console.WriteLine("OK");
            Console.ReadKey();
        }
        /// <summary>
        /// 9  定制订单下载
        /// </summary>
        static void TestSaleOrderJob()
        {
            ISyncJob job = new SyncSaleOrderJob();
            job.Sync();
            Console.WriteLine("OK");
            Console.ReadKey();
        }
        /// <summary>
        /// 10  物流部开单记录下载
        /// </summary>
        static void TestQueryObPageJob()
        {
            ISyncJob job = new SyncQueryObPageJob();
            job.Sync();
            Console.WriteLine("OK");
            Console.ReadKey();
        }
    }
}
