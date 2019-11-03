using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using hn.AutoSyncLib.Common;
using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace hn.MN_SyncUI
{

    static class Program
    {

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
        }
    }
}
