using System;
using System.IO;
using log4net;
using log4net.Core;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace hn.AutoSyncLib.Common
{
    public class LogHelper
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(LogHelper));

        private static TextWriter _textWriter;

        public static void Info(string msg)
        {
            Logger.Info(msg);
        }

        public static void Error(Exception ex)
        {
            Logger.Error(ex);
        }

        public static void Init(TextWriter writer)
        {
            _textWriter = writer;
            Console.SetOut(_textWriter);
        }
    }
}