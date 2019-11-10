using System;
using System.IO;
using log4net;
using log4net.Core;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace hn.Common
{
    public class LogHelper
    {
        private static readonly ILog InfoLogger = LogManager.GetLogger("INFO");
        private static readonly ILog ErrorLogger = LogManager.GetLogger("ERROR");
        static object lockobj=new object();
        private static TextWriter _textWriter;

        public static void Info(string msg)
        {
            lock (lockobj)
            {
                InfoLogger.Info(msg);
            }
        }

        public static void Error(Exception ex)
        {
            lock (lockobj)
            {
                ErrorLogger.Error(ex);
            }
        }

        public static void Error(string ex)
        {
            lock (lockobj)
            {
                ErrorLogger.Error(ex);
            }
        }

        public static void Init(TextWriter writer)
        {
            _textWriter = writer;
            Console.SetOut(_textWriter);
        }
    }
}