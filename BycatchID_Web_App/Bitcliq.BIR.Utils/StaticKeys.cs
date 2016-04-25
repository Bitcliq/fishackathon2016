using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Bitcliq.BIR.Utils
{
    public class StaticKeys
    {

        #region SMARTCITYHUB INTEGRATION

        public static string SCHAccessCode = ConfigurationManager.AppSettings["SCHAccessCode"] + "";
        #endregion


        #region MAIL

        public static string EntityName = ConfigurationManager.AppSettings["EntityName"] + "";

        public static string MailSubject = ConfigurationManager.AppSettings["MailSubject"] + "";
        public static string MailFrom = ConfigurationManager.AppSettings["MailFrom"] + "";
        public static string MailServer = ConfigurationManager.AppSettings["MailServer"] + "";
        public static string MailServerPassword = ConfigurationManager.AppSettings["MailServerPassword"] + "";

        
        public static string EmailsContextKey = ConfigurationManager.AppSettings["EmailsContextKey"] + "";

        public static string InviteSubject = ConfigurationManager.AppSettings["InviteSubject"] + "";
        //public static string InternalSubject = ConfigurationManager.AppSettings["InternalSubject"] + "";
        public static string UpdateIssueSubject = ConfigurationManager.AppSettings["UpdateIssueSubject"] + "";
        public static string NewIssueSubject = ConfigurationManager.AppSettings["NewIssueSubject"] + "";
        public static string AssignIssueSubject = ConfigurationManager.AppSettings["AssignIssueSubject"] + "";
        public static string CloseIssueSubject = ConfigurationManager.AppSettings["CloseIssueSubject"] + "";
        public static string AssignNewIssueSubject = ConfigurationManager.AppSettings["AssignNewIssueSubject"] + "";
        

        #endregion

        #region LOG4NET
        public static string LogConfigFilePath = ConfigurationManager.AppSettings["LogConfigFilePath"] + "";
        #endregion

        #region CONN STRS
        // Connection Strings
        public static string ConnStr = ConfigurationManager.ConnectionStrings["ConnStr"] + "";
        #endregion


        #region #PATHS
        public static string BackofficePath = ConfigurationManager.AppSettings["BackofficePath"] + "";
        public static string WebserviceTempPath = ConfigurationManager.AppSettings["WebserviceTempPath"] + "";
        public static string BackofficeTempPath = ConfigurationManager.AppSettings["BackofficeTempPath"] + "";
        public static string BackofficeTemplatesPath = ConfigurationManager.AppSettings["BackofficeTemplatesPath"] + "";
        #endregion

        #region URLS
        public static string BackofficeUrl = ConfigurationManager.AppSettings["BackofficeUrl"] + "";
        public static string BackofficeTempUrl = ConfigurationManager.AppSettings["BackofficeTempUrl"] + "";
        public static string LoginUrl = ConfigurationManager.AppSettings["LoginUrl"] + "";
        #endregion



        //ACCOUNT ID
        public static string AccountID = ConfigurationManager.AppSettings["AccountID"] + "";

        //PROFILE ADMIN
        public static string AdminProfileID = ConfigurationManager.AppSettings["AdminProfileID"] + "";

        // BACKOFFICE KEYS
        public static string ImageExtensionsSupported = ConfigurationManager.AppSettings["ImageExtensionsSupported"] + "";
      
        public static string MultimediaMimeTypes = ConfigurationManager.AppSettings["MultimediaMimeTypes"] + "";


       
        // STATE New
        public static string StateResolvedID = ConfigurationManager.AppSettings["StateResolvedID"] + "";
        public static string StateNewID = ConfigurationManager.AppSettings["StateNewID"] + "";


        // IMG RESIZE
        public static string Img_PreferredWidth = ConfigurationManager.AppSettings["Img_PreferredWidth"] + "";

        public static string Img_PreferredHeight = ConfigurationManager.AppSettings["Img_PreferredHeight"] + "";

        

        public static string Radius = ConfigurationManager.AppSettings["Radius"] + "";


        public static string SendEmailToExecutors = ConfigurationManager.AppSettings["SendEmailToExecutors"] + "";


        public static string OnlyAdminCanSee = ConfigurationManager.AppSettings["OnlyAdminCanSee"] + "";

        public static string GoogleMapsUrl = ConfigurationManager.AppSettings["GoogleMapsUrl"] + "";


        public static bool ShowAllGraphsInDashboard = (bool)UtilMethods.toBool(ConfigurationManager.AppSettings["ShowAllGraphsInDashboard"], true);
        public static bool ShowMineGraphsInDashboard = (bool)UtilMethods.toBool(ConfigurationManager.AppSettings["ShowMineGraphsInDashboard"], true);

        
    }
}
