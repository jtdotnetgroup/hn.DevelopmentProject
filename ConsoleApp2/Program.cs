using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hn.AutoSyncLib;
using hn.AutoSyncLib.Common;
using hn.AutoSyncLib.Jobs;
using hn.AutoSyncLib.Schedule;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            //初始化日志文件 
            var path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase +
                       ConfigurationManager.AppSettings["log4net"];
            var fi = new System.IO.FileInfo(path);
            log4net.Config.XmlConfigurator.Configure(fi);

            MC_OutOfStore_Schedule.StartEveryDayTask().GetAwaiter().GetResult();
            MC_PickUpGoods_Schedule.StartEveryDayTask().GetAwaiter().GetResult();
            //MC_PickUpGoods_Schedule.StartTodaySync().GetAwaiter().GetResult();
            //MC_OutOfStore_Schedule.StartTodaySync().GetAwaiter().GetResult();


            while (true)
            {
                var key = Console.ReadKey();
                if (key.KeyChar == 'Q')
                {
                    Console.WriteLine("确定要退出吗(y/n)？");
                    key = Console.ReadKey();
                    if (key.KeyChar=='y')
                    {
                        break;
                    }
                }
            }
        }
    }
}
