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
            Start();
        }
        static void Start()
        {
            Console.WriteLine(System.Environment.NewLine + "请输入测试方法,如需要退出方法请输入0");
            int num = Convert.ToInt32(Console.ReadLine());
            if (num == 0) { Console.WriteLine("程序已退出"); }
            else
            { 
                Test(num); 
            }
        }
        /// <summary>
        /// 需要测试的方法
        /// </summary>
        static void Test(int num)
        {
            DateTime d1 = DateTime.Now;
            switch (num)
            {
                case 1:
                    {
                        TestLogin();
                        break;
                    }
                case 2:
                    {
                        TestInventoryJob();
                        break;
                    }
                case 3:
                    {
                        TestInventoryDayJob();
                        break;
                    }
                case 4:
                    {
                        TestHnObOrderDay();
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
                case 11:
                    {
                        TestQueryObPageJob();
                        break;
                    }
                case 12:
                    {
                        TestQueryObOrderJob();
                        break;
                    }
            }
            TimeSpan d3 = DateTime.Now.Subtract(d1);
            string msg = "用时:" + d3.Days.ToString() + "天" + d3.Hours.ToString() + "小时" + d3.Minutes.ToString() + "分钟" + d3.Seconds.ToString() + "秒" + d3.Milliseconds.ToString() + "毫秒";
            Console.WriteLine(msg);
            Start();
        }

        #region 接口的方法
        /// <summary>
        /// 1、Token获取接口
        /// </summary>  
        static void TestLogin()
        {
            iInterface.GetToken();
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
        /// 3、华耐日销出库下载
        /// </summary>
        static void TestInventoryDayJob()
        {
            ISyncJob job = new SyncInventoryDayJob();
            job.Sync();
        }
        /// <summary>
        /// 4、库存结存数据下载
        /// </summary>
        static void TestHnObOrderDay()
        {
            ISyncJob job = new SyncHnObOrderDay();
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
        /// <summary>
        /// 11  物流部开单记录下载
        /// </summary>
        static void TestobOrderUploadJob()
        {
            ISyncJob job = new SyncobOrderUploadJob();
            job.Sync();
        }
        /// <summary>
        /// 12、出库单下载
        /// </summary> 
        static void TestQueryObOrderJob()
        {
            ISyncJob job   =new SyncqueryObOrderPageJob();
            job.Sync();
        }
        #endregion
        
        static void TestDelete()
        {
            OracleDBHelper helper = new OracleDBHelper("");
            AuthorizationToken token = new AuthorizationToken();

            helper.Delete<AuthorizationToken>("");

        }
    }
}
