using log4net;

namespace Bitcliq.BIR.Logs
{
   	public class UserLogger : BaseLogger
	{
		public static ILog LOGGER = log4net.LogManager.GetLogger("UserLog");
        public static ILog LOGINLOGGER = log4net.LogManager.GetLogger("LoginLog");

		public static void PrepareLog(string userID, string userName)
		{
			log4net.GlobalContext.Properties["userID"] = userID;
			log4net.GlobalContext.Properties["userName"] = userName;	
		}

			
	}
}
