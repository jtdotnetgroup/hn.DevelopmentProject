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
            DateTime d1 = DateTime.Now;
            Test();
            DateTime d2 = DateTime.Now;
            TimeSpan d3 = d2.Subtract(d1);
            string msg = "相差:" + d3.Days.ToString() + "天" + d3.Hours.ToString() + "小时" + d3.Minutes.ToString() + "分钟" + d3.Seconds.ToString() + "秒";
            Console.WriteLine(msg); 
            LogHelper.Init(Console.Out); 
            Console.ReadKey();
        }
        /// <summary>
        /// 需要测试的方法
        /// </summary>
        static void Test()
        {
            int num = 2;
            switch (num)
            {
                case 2:
                    {
                        TestInventoryJob();
                        break;
                    }
                case 5:
                    {
                        TestProductJob();
                        break;
                    }
                case 6:
                    {
                        TestPolicyJob();
                        break;
                    }
                case 7:
                    {
                        TestSaleOrderUploadJob();
                        break;
                    }
                case 8:
                    {
                        TestAcctOAStatusJob();
                        break;
                    }
                case 9:
                    {
                        TestSaleOrderJob();
                        break;
                    }
                case 10:
                    {
                        TestQueryObPageJob();
                        break;
                    }
                case 12:
                    {
                        TestSyncQueryObOrderJob();
                        break;
                    }
            }
        }
        static void TestLogin()
        {
            iInterface.GetToken();
        } 
        static void TestDelete()
        {
            OracleDBHelper helper = new OracleDBHelper("");
            AuthorizationToken token = new AuthorizationToken();

            helper.Delete<AuthorizationToken>("");

        }
        /// <summary>
        /// 2、库存下载接口
        /// </summary>
        static void TestInventoryJob()
        {
            ISyncJob job = new SyncInventoryJob();
            job.Sync(); 
        }
        /// <summary>
        /// 5、物料下载接口
        /// </summary>
        static void TestProductJob()
        {
            ISyncJob job = new SyncProductJob();
            job.Sync(); 
        }
        /// <summary>
        /// 6、折扣政策下载
        /// </summary>
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
            ISyncJob job = new SyncAcctOAStatusJob();
            job.Sync(); 
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
        /// <summary>
        /// 12、出库单下载
        /// </summary>
        static void TestSyncQueryObOrderJob()
        {
            ISyncJob job   =new SyncqueryObOrderPageJob();
            job.Sync();
            
        }
    }
}
