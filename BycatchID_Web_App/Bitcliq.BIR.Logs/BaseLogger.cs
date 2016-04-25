
namespace Bitcliq.BIR.Logs
{
    public abstract class BaseLogger
    {

        public static bool isInit = false;



        /// <summary>
        /// This Method should be invoked in the start of the application
        /// </summary>
        /// <param name="LogConfFile"></param>
        public static void Init(string LogConfFile)
        {
            if (!isInit)
            {
                log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(LogConfFile));
                isInit = true;
            }
        }


    }

}
