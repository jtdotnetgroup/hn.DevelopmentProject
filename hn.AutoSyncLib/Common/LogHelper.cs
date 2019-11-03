using System;
using System.IO;
using log4net;
using log4net.Core;

namespace hn.AutoSyncLib.Common
{
    public class LogHelper
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(LogHelper));

        private static TextWriter textWriter;

        public static void Info(string msg)
        {
            logger.Info(msg);
            Console.WriteLine(msg);
        }

        public static void Error(Exception ex)
        {
            logger.Error(ex);
            Console.WriteLine(ex.Message);
        }

        public static void Init(TextWriter writer)
        {
            textWriter = writer;
            Console.SetOut(textWriter);
        }
    }
}