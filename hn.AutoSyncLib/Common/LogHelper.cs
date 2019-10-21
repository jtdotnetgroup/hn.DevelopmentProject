using System;
using log4net;
using log4net.Core;

namespace hn.AutoSyncLib.Common
{
    public class LogHelper
    {
        private static readonly ILog info = LogManager.GetLogger("EventLog");
        private static readonly ILog error = LogManager.GetLogger("ExceptionLog");

        public static void LogInfo(string msg)
        {
            info.Info(msg);
        }

        public static void LogErr(Exception ex)
        {
            error.Error(ex);
        }
    }
}