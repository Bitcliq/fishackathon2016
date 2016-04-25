using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net.Config;
using log4net;
using Bitcliq.BIR.Utils;



namespace Bitcliq.BIR.Logs
{
    public sealed class ExLogger
    {
        private static readonly ILog log = LogManager.GetLogger("InfoLog");
        private static readonly ILog errorlog = LogManager.GetLogger("ErrorLog");
        private static readonly ILog criticalErrorlog = LogManager.GetLogger("CriticalErrorLog");

        static ExLogger()
        {
            
            XmlConfigurator.Configure(new System.IO.FileInfo(StaticKeys.LogConfigFilePath));
        }

        public static void LogCriticalError(string message)
        {
         
            criticalErrorlog.Error(message);
        }

        public static void LogError(string message)
        {
            errorlog.Error(message);
        }

        public static void LogInfo(string message)
        {
            log.Info(message);
        }
        public static void LogWarn(string message)
        {
            log.Warn(message);
        }
        public static void LogDebug(string message)
        {
            log.Debug(message);
        }
        public static void LogFatal(string message)
        {
            errorlog.Fatal(message);
        }

    }
}
