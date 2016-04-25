using log4net;

namespace Bitcliq.BIR.Logs
{
    public class ErrorLogger : BaseLogger
    {
        public static ILog LOGGER = log4net.LogManager.GetLogger("ErrorLog");
        
    }
}
