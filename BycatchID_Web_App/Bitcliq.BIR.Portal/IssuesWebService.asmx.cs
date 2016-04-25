using Bitcliq.BIR.Platform;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using Newtonsoft.Json;
using Microsoft.SqlServer.Types;
using Bitcliq.BIR.Utils;
using Bitcliq.BIR.Logs;
using System.Data;

namespace Bitcliq.BIR.Portal
{
    /// <summary>
    /// Summary description for IssuesWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class IssuesWebService : System.Web.Services.WebService
    {


        private static string DATA = "Data";
        private static string ERROR = "Error";

        private static string EMPTY = "Error";


        //[System.Web.Services.WebMethod(EnableSession = true)]
        //public static string GeneratePDF(int id, string imgSrc, string address)
        //{
        //    Issue i = new Issue(Convert.ToInt32(id));



        //    if (i.ID > 0)
        //    {
        //        string content = UtilMethods.ReadTextFile(StaticKeys.BackofficeTemplatesPath + "PdfTemplate.html");



        //        IssueJson ij = new IssueJson(i, true);


        //        content = UtilMethods.ReplaceTag(content, "ImgSrc", imgSrc);
        //        //content = UtilMethods.ReplaceTag(content, "ImgSrc", ij.PhotoUrl);

        //        content = UtilMethods.ReplaceTag(content, "Subject", ij.Subject);
        //        content = UtilMethods.ReplaceTag(content, "Priority", ij.Priority + "");
        //        content = UtilMethods.ReplaceTag(content, "Reporter", ij.ReporterName);
        //        content = UtilMethods.ReplaceTag(content, "ReporterEmail", ij.ReporterEmail);
        //        content = UtilMethods.ReplaceTag(content, "Message", ij.Message);

        //        DateTime dt = Convert.ToDateTime(ij.DateReported);
        //        content = UtilMethods.ReplaceTag(content, "Date", dt.ToString("dd-MM-yyyy") + "");
        //        content = UtilMethods.ReplaceTag(content, "Type", ij.TypeName);
        //        content = UtilMethods.ReplaceTag(content, "State", ij.StateName);
        //        content = UtilMethods.ReplaceTag(content, "Hours", dt.ToString("HH:mm") + "");



        //        // google maps


        //        //http://maps.googleapis.com/maps/api/staticmap?zoom=17&size=400x400&markers=color:green|label:G|39.406938,-9.135937


        //        if (ij.Latitude + "" != "" && ij.Longitude + "" != "")
        //        {
        //            content = UtilMethods.ReplaceTag(content, "GoogleSrc", "http://maps.googleapis.com/maps/api/staticmap?zoom=17&size=400x400&markers=" + ij.Latitude + "," + ij.Longitude);
        //            content = UtilMethods.ReplaceTag(content, "Display", "");
        //        }
        //        else
        //        {
        //            content = UtilMethods.ReplaceTag(content, "GoogleSrc", "");
        //            content = UtilMethods.ReplaceTag(content, "Display", "none");
        //        }

        //        content = UtilMethods.ReplaceTag(content, "Address", address);







        //        DateTime dt1 = DateTime.Now;

        //        //https://htmlrenderer.codeplex.com/
        //        //www.pdfsharp.net/


        //        PdfDocument pdf = PdfGenerator.GeneratePdf(content, PageSize.A4);
        //        pdf.Save(StaticKeys.BackofficeTempPath + "pdf_" + dt1.Ticks + ".pdf");
        //        pdf.Close();
        //        pdf.Dispose();



        //        return StaticKeys.BackofficeTempUrl + "pdf_" + dt1.Ticks + ".pdf";

        //    }
        //    else
        //        return "ERROR";
        //}





        [WebMethod]
        // regista utilizador
        public string RegisterReporter(string Name, string Email, string Password, string PhoneNumber, string IPAddress)
        {


            if (Email + "" == "" || Password + "" == "")
            {
                return "{\"" + ERROR + "\": \"Preencha o Email e Senha\"}";
            }

            int cont = Reporter.CountByEmail(Email);

            if (cont == 0)
            {
                int reporterID = Reporter.Register(Name, Email, Password, PhoneNumber, IPAddress);
                if (reporterID <= 0)
                {
                    return "{\"" + ERROR + "\": \"Ocorreu um erro a registar o utilizador\"}";



                    // return "Ocorreu um erro a registar!";
                }
                else
                {
                    // reporter ID
                    return "{\"" + DATA + "\": " + JsonConvert.SerializeObject(reporterID, Formatting.Indented) + "}";
                }

            }
            else
            {
                return "{\"" + ERROR + "\": \"O email já existe\"}";

            }

        }

        //[WebMethod]
        //public string LoginReporter(string Email, string Password)
        //{

        //    Reporter reporter = Reporter.Login(Email, Password);

        //    if (reporter != null)
        //    {

        //        /// LOG de Actividade -----------------------------------------------------------------------------------
        //        ///

        //        UserLogger.PrepareLog(reporter.ID.ToString(), reporter.Email);
        //        UserLogger.LOGGER.Info("Reporter Login through web service (reporter id= " + reporter.ID + ", email=" + reporter.Email + ")");

        //        ///------------------------------------------------------------------------------------------------------
        //        ///

        //        return "{\"" + DATA + "\": \"" + reporter.ID + "\"}";

        //        //int reporterID = Reporter.Register(AccountID, Name, Email, Password, IPAddress);
        //        //if (reporterID >= 0)
        //        //{
        //        //    return "{\"" + ERROR + "\": Ocorreu um erro a registar o utilizador}";



        //        //    // return "Ocorreu um erro a registar!";
        //        //}
        //        //else
        //        //{
        //        //    // reporter ID
        //        //    return "{\"" + DATA + "\": " + JsonConvert.SerializeObject(reporterID, Formatting.Indented) + "}";
        //        //}

        //    }
        //    else
        //    {
        //        return "{\"" + ERROR + "\": \"Dados inválidos\"}";

        //    }

        //}


        //[WebMethod]
        //public string LoginUser(string Email, string Password)
        //{

        //    BIR.Platform.User user = BIR.Platform.User.Login(Email, Password);

        //    if (user != null)
        //    {

        //        /// LOG de Actividade -----------------------------------------------------------------------------------
        //        ///

        //        UserLogger.PrepareLog(user.UserID.ToString(), user.Email);
        //        UserLogger.LOGGER.Info("User Login through web service (id= " + user.UserID + ", email=" + user.Email + ")");

        //        ///------------------------------------------------------------------------------------------------------
        //        ///



        //        return "{\"" + DATA + "\": \"" + user.UserID + "\"}";

        //        //int reporterID = Reporter.Register(AccountID, Name, Email, Password, IPAddress);
        //        //if (reporterID >= 0)
        //        //{
        //        //    return "{\"" + ERROR + "\": Ocorreu um erro a registar o utilizador}";



        //        //    // return "Ocorreu um erro a registar!";
        //        //}
        //        //else
        //        //{
        //        //    // reporter ID
        //        //    return "{\"" + DATA + "\": " + JsonConvert.SerializeObject(reporterID, Formatting.Indented) + "}";
        //        //}

        //    }
        //    else
        //    {
        //        return "{\"" + ERROR + "\": \"Dados inválidos\"}";

        //    }

        //}


        #region SEND MAILS

        private void SendEmails(int TypeID, string MailSubject, string ReporterName, string Subject, string Message, string TemplateName, int? UserID, int? ReporterID)
        {
            BIR.Platform.Type t = new Platform.Type((int)TypeID);

            string closedBy = "";

            if (UserID > 0)
            {
                User u = new Platform.User((int)UserID);
                closedBy = u.Name;
            }


            else if (ReporterID > 0)
            {
                Reporter r = new Platform.Reporter((int)ReporterID);
                closedBy = r.Name + " - (Reporter)";
            }
            List<BIR.Platform.User> list;

            if (StaticKeys.SendEmailToExecutors == "false")
            {
                list = BIR.Platform.Type.ListDecisorsByTypeID((int)TypeID);
            }
            else
                list = BIR.Platform.Type.ListUsersByTypeID((int)TypeID);

            if (list.Count > 0)
            {
                foreach (User usr in list)
                {
                    if (usr.RecieveNotifications)
                    {
                        string content = UtilMethods.ReadTextFile(StaticKeys.BackofficeTemplatesPath + TemplateName);
                        content = UtilMethods.ReplaceTag(content, "EntityName", StaticKeys.EntityName);
                        content = UtilMethods.ReplaceTag(content, "ClosedBy", closedBy);
                        content = UtilMethods.ReplaceTag(content, "Reporter", ReporterName);
                        content = UtilMethods.ReplaceTag(content, "User", usr.Name);
                        content = UtilMethods.ReplaceTag(content, "Type", t.Name);
                        content = UtilMethods.ReplaceTag(content, "Subject", Subject);
                        content = UtilMethods.ReplaceTag(content, "Message", Message);

                        content = UtilMethods.ReplaceTag(content, "Url", StaticKeys.BackofficeUrl);


                        SendMail sm = new SendMail(StaticKeys.MailFrom, usr.Email, MailSubject, content, null);


                        /// LOG de Actividade -----------------------------------------------------------------------------------
                        ///

                        UserLogger.PrepareLog(usr.UserID.ToString(), usr.Email);
                        UserLogger.LOGGER.Info("Send email to (email = " + usr.Email + ", Subject=" + Subject + ", Message=" + Message + ")");

                        ///------------------------------------------------------------------------------------------------------
                        ///


                    }
                }
            }

        }

        #endregion




        //[WebMethod]
        //public string ReportIssue(int? AccountID, int ReporterID, string Subject, string Message, string FileName, byte[] File, string FileType,
        //                           int FileLength, decimal? Latitude, decimal? Longitude, int? TypeID, int? ImageRotation, string Device)
        //{
        //    if (AccountID == null)
        //    {
        //        AccountID = 2;
        //        if (StaticKeys.AccountID + "" != "")
        //        {
        //            try
        //            {
        //                AccountID = Convert.ToInt32(StaticKeys.AccountID);
        //            }
        //            catch (Exception)
        //            {
        //            }
        //        }
        //    }

        //    Reporter reporter = new Reporter(ReporterID);


        //    if (reporter.ID > 0)
        //    {


        //        // save File 
        //        Guid g = Guid.NewGuid();

        //        UtilMethods.CreateFile(File, StaticKeys.WebserviceTempPath + g + "_" + FileName);


        //        System.Drawing.Image img = System.Drawing.Image.FromFile(StaticKeys.WebserviceTempPath + g + "_" + FileName);

        //        DateTime? dt = ImageExtensions.GetDateTaken(img);

        //        #region DIRECTLY FROM PHOTO
        //        float? lat = ImageExtensions.GetLatitude(img);
        //        float? lon = ImageExtensions.GetLongitude(img);

        //        //from photo

        //        if (lat != null && lon != null)
        //        {
        //            Latitude = Convert.ToDecimal(lat);
        //            Longitude = Convert.ToDecimal(lon);
        //        }
        //        img.Dispose();
        //        #endregion



        //        if (Latitude != null && Longitude != null)
        //        {

        //            try
        //            {
        //                SqlGeographyBuilder builder = new SqlGeographyBuilder();

        //                builder.SetSrid(4326);
        //                builder.BeginGeography(OpenGisGeographyType.Point);

        //                //double lat = double.Parse(UtilMethods.ReplaceDangerous(latTxt.Text.Trim()), CultureInfo.InvariantCulture);
        //                //double ln = double.Parse(UtilMethods.ReplaceDangerous(longTxt.Text.Trim()), CultureInfo.InvariantCulture);

        //                builder.BeginFigure((double)Latitude, (double)Longitude);


        //                builder.EndFigure();
        //                builder.EndGeography();



        //                #region FILL ISSUE DATA
        //                Issue i = new Issue();
        //                i.GPS = builder.ConstructedGeography.ToString();
        //                i.AccountID = (int)AccountID;
        //                i.ReportedBy = ReporterID;
        //                i.Blob = File;
        //                i.BlobName = FileName;
        //                i.BlobType = FileType;
        //                i.BlobLen = FileLength;
        //                i.Subject = Subject;
        //                i.Message = Message;
        //                i.DateTaken = dt;
        //                // i.Priority = Priority;
        //                i.TypeID = TypeID;
        //                i.ImageRotation = ImageRotation;
        //                i.Device = Device;

        //                if (i.Save())
        //                {
        //                    IssueJson ij = new IssueJson(i, true);


        //                    /// LOG de Actividade -----------------------------------------------------------------------------------
        //                    ///

        //                    UserLogger.PrepareLog(reporter.ID.ToString(), reporter.Email);
        //                    UserLogger.LOGGER.Info("Report through web service (issueid=" + i.ID + " , reporter id= " + reporter.ID + ", email=" + reporter.Email + ")");

        //                    ///------------------------------------------------------------------------------------------------------
        //                    ///



        //                    //send mails to users

        //                    if (TypeID > 0)
        //                    {
        //                        SendEmails((int)TypeID, StaticKeys.NewIssueSubject, reporter.Name, Subject, Message, "NewIssue.html", null, null);
        //                    }


        //                    return "{\"" + DATA + "\": [" + JsonConvert.SerializeObject(ij, Formatting.Indented) + "] }";
        //                }
        //                else
        //                {
        //                    return "{\"" + ERROR + "\": \"Não foi possível reportar o pedido\"}";
        //                }
        //                #endregion

        //            }
        //            catch (Exception ex)
        //            {
        //                ErrorLogger.LOGGER.Error(ex.Message, ex);
        //                return "{\"" + ERROR + "\": \"Coordenadas inválidas\"}";
        //            }
        //        }
        //        else
        //        {
        //            return "{\"" + ERROR + "\": \"A foto não tem coordenadas\"}";
        //        }


        //    }
        //    else
        //    {
        //        return "{\"" + ERROR + "\": \"O utilizador não existe\"}";

        //    }

        //}



        //[WebMethod]
        //public string ReportIssueAndRegisterUser(int? ReporterID, string Name, string Email, string Password, string PhoneNumber, int? AccountID, string Subject, string Message, string FileName, byte[] File, string FileType,
        //                           int FileLength, decimal? Latitude, decimal? Longitude, int? TypeID, int? ImageRotation, string Device)
        //{





        //    ErrorLogger.LOGGER.Error("Report and register", new Exception("sss"));



        //    if (AccountID == null)
        //    {
        //        AccountID = 2;
        //        if (StaticKeys.AccountID + "" != "")
        //        {
        //            try
        //            {
        //                AccountID = Convert.ToInt32(StaticKeys.AccountID);
        //            }
        //            catch (Exception)
        //            {
        //            }
        //        }
        //    }




        //    Reporter reporter;

        //    if (ReporterID > 0)
        //    {
        //        reporter = new Reporter((int)ReporterID);
        //    }
        //    else
        //    {

        //        if (Email + "" == "")//|| Password + "" == "")
        //        {
        //            return "{\"" + ERROR + "\": \"Preencha o Email\"}";
        //        }



        //        reporter = new Reporter();
        //        reporter.Email = Email;
        //        reporter.Name = Name;
        //        reporter.Password = Password;
        //        reporter.PhoneNumber = PhoneNumber;

        //        reporter.Save();

        //        ReporterID = reporter.ID;
        //    }



        //    if (reporter.ID > 0)
        //    {

        //        DateTime? dt = null;

        //        #region IMAGE
        //        if (File != null)
        //        {
        //            // save File 
        //            Guid g = Guid.NewGuid();

        //            UtilMethods.CreateFile(File, StaticKeys.WebserviceTempPath + g + "_" + FileName);


        //            System.Drawing.Image img = System.Drawing.Image.FromFile(StaticKeys.WebserviceTempPath + g + "_" + FileName);

        //            dt = ImageExtensions.GetDateTaken(img);

        //            #region DIRECTLY FROM PHOTO



        //            float? lat = ImageExtensions.GetLatitude(img);
        //            float? lon = ImageExtensions.GetLongitude(img);


        //            //from photo

        //            if (lat != null && lon != null)
        //            {
        //                Latitude = Convert.ToDecimal(lat);

        //                Longitude = Convert.ToDecimal(lon);
        //            }
        //            img.Dispose();
        //            #endregion
        //        }
        //        #endregion

        //        if (Latitude != null && Longitude != null)
        //        {

        //            try
        //            {
        //                SqlGeographyBuilder builder = new SqlGeographyBuilder();

        //                builder.SetSrid(4326);
        //                builder.BeginGeography(OpenGisGeographyType.Point);

        //                //double lat = double.Parse(UtilMethods.ReplaceDangerous(latTxt.Text.Trim()), CultureInfo.InvariantCulture);
        //                //double ln = double.Parse(UtilMethods.ReplaceDangerous(longTxt.Text.Trim()), CultureInfo.InvariantCulture);

        //                builder.BeginFigure((double)Latitude, (double)Longitude);


        //                builder.EndFigure();
        //                builder.EndGeography();



        //                #region FILL ISSUE DATA
        //                Issue i = new Issue();
        //                i.GPS = builder.ConstructedGeography.ToString();
        //                i.AccountID = (int)AccountID;
        //                i.ReportedBy = ReporterID;
        //                i.Blob = File;
        //                i.BlobName = FileName;
        //                i.BlobType = FileType;
        //                i.BlobLen = FileLength;
        //                i.Subject = Subject;
        //                i.Message = Message;
        //                i.DateTaken = dt;
        //                //i.Priority = Priority;
        //                i.TypeID = TypeID;
        //                i.ImageRotation = ImageRotation;
        //                i.Device = Device;

        //                if (i.Save())
        //                {
        //                    IssueJson ij = new IssueJson(i, true);

        //                    /// LOG de Actividade -----------------------------------------------------------------------------------
        //                    ///

        //                    UserLogger.PrepareLog(reporter.ID.ToString(), reporter.Email);
        //                    UserLogger.LOGGER.Info("Report through web service (issueid=" + i.ID + " ,reporter id= " + reporter.ID + ", email=" + reporter.Email + ")");

        //                    ///------------------------------------------------------------------------------------------------------
        //                    ///


        //                    //send mails to users

        //                    if (TypeID > 0)
        //                    {
        //                        SendEmails((int)TypeID, StaticKeys.NewIssueSubject, reporter.Name, Subject, Message, "NewIssue.html", null, null);
        //                    }

        //                    return "{\"" + DATA + "\": [" + JsonConvert.SerializeObject(ij, Formatting.Indented) + " ]}";
        //                }
        //                else
        //                {
        //                    return "{\"" + ERROR + "\": \"Não foi possível reportar o pedido\"}";
        //                }
        //                #endregion

        //            }
        //            catch (Exception ex)
        //            {
        //                ErrorLogger.LOGGER.Error(ex.Message, ex);
        //                return "{\"" + ERROR + "\": \"Coordenadas inválidas\"}";
        //            }
        //        }
        //        else
        //        {
        //            return "{\"" + ERROR + "\": \"A foto não tem coordenadas\"}";
        //        }


        //    }
        //    else
        //    {
        //        return "{\"" + ERROR + "\": \"O utilizador não existe\"}";

        //    }

        //}






        //[WebMethod]
        //public string ReportIssue(int? AccountID, int ReporterID, string Subject, string Message, string FileName, byte[] File, string FileType,
        //                           int FileLength, decimal? Latitude, decimal? Longitude, int? TypeID, int? ImageRotation, string Device)
        //{
        //    if (AccountID == null)
        //    {
        //        AccountID = 2;
        //        if (StaticKeys.AccountID + "" != "")
        //        {
        //            try
        //            {
        //                AccountID = Convert.ToInt32(StaticKeys.AccountID);
        //            }
        //            catch (Exception)
        //            {
        //            }
        //        }
        //    }

        //    Reporter reporter = new Reporter(ReporterID);


        //    if (reporter.ID > 0)
        //    {


        //        //// save File 
        //        //Guid g = Guid.NewGuid();

        //        //UtilMethods.CreateFile(File, StaticKeys.WebserviceTempPath + g + "_" + FileName);


        //        //System.Drawing.Image img = System.Drawing.Image.FromFile(StaticKeys.WebserviceTempPath + g + "_" + FileName);

        //        //DateTime? dt = ImageExtensions.GetDateTaken(img);

        //        //#region DIRECTLY FROM PHOTO
        //        //float? lat = ImageExtensions.GetLatitude(img);
        //        //float? lon = ImageExtensions.GetLongitude(img);

        //        ////from photo

        //        //if (lat != null && lon != null)
        //        //{
        //        //    Latitude = Convert.ToDecimal(lat);
        //        //    Longitude = Convert.ToDecimal(lon);
        //        //}
        //        //img.Dispose();
        //        //#endregion




        //        if (Latitude != null && Longitude != null)
        //        {

        //            try
        //            {
        //                SqlGeographyBuilder builder = new SqlGeographyBuilder();

        //                builder.SetSrid(4326);
        //                builder.BeginGeography(OpenGisGeographyType.Point);

        //                //double lat = double.Parse(UtilMethods.ReplaceDangerous(latTxt.Text.Trim()), CultureInfo.InvariantCulture);
        //                //double ln = double.Parse(UtilMethods.ReplaceDangerous(longTxt.Text.Trim()), CultureInfo.InvariantCulture);

        //                builder.BeginFigure((double)Latitude, (double)Longitude);


        //                builder.EndFigure();
        //                builder.EndGeography();



        //                #region FILL ISSUE DATA
        //                Issue i = new Issue();
        //                i.GPS = builder.ConstructedGeography.ToString();
        //                i.AccountID = (int)AccountID;
        //                i.ReportedBy = ReporterID;
        //                i.Blob = File;
        //                i.BlobName = FileName;
        //                i.BlobType = FileType;
        //                i.BlobLen = FileLength;
        //                i.Subject = Subject;
        //                i.Message = Message;
        //                //i.DateTaken = dt;
        //                // i.Priority = Priority;
        //                i.TypeID = TypeID;
        //                i.ImageRotation = ImageRotation;
        //                i.Device = Device;

        //                if (i.Save())
        //                {
        //                    IssueJson ij = new IssueJson(i, true);


        //                    /// LOG de Actividade -----------------------------------------------------------------------------------
        //                    ///

        //                    UserLogger.PrepareLog(reporter.ID.ToString(), reporter.Email);
        //                    UserLogger.LOGGER.Info("Report through web service (issueid=" + i.ID + " , reporter id= " + reporter.ID + ", email=" + reporter.Email + ")");

        //                    ///------------------------------------------------------------------------------------------------------
        //                    ///



        //                    //send mails to users

        //                    if (TypeID > 0)
        //                    {
        //                        SendEmails((int)TypeID, StaticKeys.NewIssueSubject, reporter.Name, Subject, Message, "NewIssue.html", null, null);
        //                    }


        //                    return "{\"" + DATA + "\": [" + JsonConvert.SerializeObject(ij, Formatting.Indented) + "] }";
        //                }
        //                else
        //                {
        //                    return "{\"" + ERROR + "\": \"Não foi possível reportar o pedido\"}";
        //                }
        //                #endregion

        //            }
        //            catch (Exception ex)
        //            {
        //                ErrorLogger.LOGGER.Error(ex.Message, ex);
        //                return "{\"" + ERROR + "\": \"Coordenadas inválidas\"}";
        //            }
        //        }
        //        else
        //        {
        //            return "{\"" + ERROR + "\": \"Coordenadas inválidas\"}";
        //        }


        //    }
        //    else
        //    {
        //        return "{\"" + ERROR + "\": \"O utilizador não existe\"}";

        //    }

        //}



        [WebMethod]
        public string ReportIssueAndRegisterUser(int? ReporterID, string Name, string Email, string Password, string PhoneNumber, int? AccountID, string Subject, string Message, string FileName, byte[] File, string FileType,
                                   int FileLength, decimal? Latitude, decimal? Longitude, int? TypeID, int? ImageRotation, string Device, int? PropertyID, string PropertyName)
        {





            ErrorLogger.LOGGER.Error("Report and register", new Exception("sss"));



            if (AccountID == null)
            {
                AccountID = 2;
                if (StaticKeys.AccountID + "" != "")
                {
                    try
                    {
                        AccountID = Convert.ToInt32(StaticKeys.AccountID);
                    }
                    catch (Exception)
                    {
                    }
                }
            }




            Reporter reporter;

            if (ReporterID > 0)
            {
                reporter = new Reporter((int)ReporterID);
            }
            else
            {

                if (Email + "" == "")//|| Password + "" == "")
                {
                    return "{\"" + ERROR + "\": \"Preencha o Email\"}";
                }



                reporter = new Reporter();
                reporter.Email = Email;
                reporter.Name = Name;
                reporter.Password = Password;
                reporter.PhoneNumber = PhoneNumber;

                reporter.Save();

                ReporterID = reporter.ID;
            }



            if (reporter.ID > 0)
            {

                DateTime? dt = null;

                //#region IMAGE
                //if (File != null)
                //{
                //    // save File 
                //    Guid g = Guid.NewGuid();

                //    UtilMethods.CreateFile(File, StaticKeys.WebserviceTempPath + g + "_" + FileName);


                //    System.Drawing.Image img = System.Drawing.Image.FromFile(StaticKeys.WebserviceTempPath + g + "_" + FileName);

                //    dt = ImageExtensions.GetDateTaken(img);

                //    #region DIRECTLY FROM PHOTO



                //    float? lat = ImageExtensions.GetLatitude(img);
                //    float? lon = ImageExtensions.GetLongitude(img);


                //    //from photo

                //    if (lat != null && lon != null)
                //    {
                //        Latitude = Convert.ToDecimal(lat);

                //        Longitude = Convert.ToDecimal(lon);
                //    }
                //    img.Dispose();
                //    #endregion
                //}
                //#endregion

                if (Latitude != null && Longitude != null)
                {

                    try
                    {
                        SqlGeographyBuilder builder = new SqlGeographyBuilder();

                        builder.SetSrid(4326);
                        builder.BeginGeography(OpenGisGeographyType.Point);

                        //double lat = double.Parse(UtilMethods.ReplaceDangerous(latTxt.Text.Trim()), CultureInfo.InvariantCulture);
                        //double ln = double.Parse(UtilMethods.ReplaceDangerous(longTxt.Text.Trim()), CultureInfo.InvariantCulture);

                        builder.BeginFigure((double)Latitude, (double)Longitude);


                        builder.EndFigure();
                        builder.EndGeography();



                        #region FILL ISSUE DATA
                        Issue i = new Issue();
                        i.GPS = builder.ConstructedGeography.ToString();
                        i.AccountID = (int)AccountID;
                        i.ReportedBy = ReporterID;
                        i.Blob = File;
                        i.BlobName = FileName;
                        i.BlobType = FileType;
                        i.BlobLen = FileLength;
                        i.Subject = Subject;
                        i.Message = Message;
                        i.DateTaken = dt;
                        //i.Priority = Priority;
                        i.TypeID = TypeID;
                        i.ImageRotation = ImageRotation;
                        i.Device = Device;
                        i.PropertyID = PropertyID;
                        i.PropertyName = PropertyName;

                        if (i.Save())
                        {
                            IssueJson ij = new IssueJson(i, true);

                            /// LOG de Actividade -----------------------------------------------------------------------------------
                            ///

                            UserLogger.PrepareLog(reporter.ID.ToString(), reporter.Email);
                            UserLogger.LOGGER.Info("Report through web service (issueid=" + i.ID + " ,reporter id= " + reporter.ID + ", email=" + reporter.Email + ")");

                            ///------------------------------------------------------------------------------------------------------
                            ///


                            //send mails to users

                            if (TypeID > 0)
                            {
                                SendEmails((int)TypeID, StaticKeys.NewIssueSubject, reporter.Name, Subject, Message, "NewIssue.html", null, null);
                            }

                            return "{\"" + DATA + "\": [" + JsonConvert.SerializeObject(ij, Formatting.Indented) + " ]}";
                        }
                        else
                        {
                            return "{\"" + ERROR + "\": \"Não foi possível reportar o pedido\"}";
                        }
                        #endregion

                    }
                    catch (Exception ex)
                    {
                        ErrorLogger.LOGGER.Error(ex.Message, ex);
                        return "{\"" + ERROR + "\": \"Coordenadas inválidas\"}";
                    }
                }
                else
                {
                    return "{\"" + ERROR + "\": \"Coordenadas inválidas\"}";
                }


            }
            else
            {
                return "{\"" + ERROR + "\": \"O utilizador não existe\"}";

            }

        }





        //[WebMethod]
        //public string CloseIssue(int? AccountID, int? UserID, int? ReporterID, int IssueID, string InternalNotes, string FileName, byte[] File, string FileType,
        //                           int FileLength, decimal? Latitude, decimal? Longitude, int? ImageRotation, int ? propertyID, string propertyName)
        //{


        //    if (AccountID == null)
        //    {
        //        AccountID = 2;
        //        if (StaticKeys.AccountID + "" != "")
        //        {
        //            try
        //            {
        //                AccountID = Convert.ToInt32(StaticKeys.AccountID);
        //            }
        //            catch (Exception)
        //            {
        //            }
        //        }
        //    }




        //    BIR.Platform.User user = new User();
        //    Reporter reporter = new Reporter();

        //    if (UserID > 0)
        //    {
        //        user = new BIR.Platform.User((int)UserID);
        //    }
        //    else if (ReporterID > 0)
        //    {
        //        reporter = new Reporter((int)ReporterID);
        //    }

        //    else
        //    {

        //        return "{\"" + ERROR + "\": \"O utilizador não existe\"}";
        //    }



        //    if (user.UserID > 0 || reporter.ID > 0)
        //    {


        //        // save File 
        //        Guid g = Guid.NewGuid();

        //        UtilMethods.CreateFile(File, StaticKeys.WebserviceTempPath + g + "_" + FileName);


        //        System.Drawing.Image img = System.Drawing.Image.FromFile(StaticKeys.WebserviceTempPath + g + "_" + FileName);

        //        DateTime? dt = ImageExtensions.GetDateTaken(img);

        //        #region DIRECTLY FROM PHOTO
        //        float? lat = ImageExtensions.GetLatitude(img);
        //        float? lon = ImageExtensions.GetLongitude(img);

        //        //from photo

        //        if (lat != null && lon != null)
        //        {
        //            Latitude = Convert.ToDecimal(lat);
        //            Longitude = Convert.ToDecimal(lon);
        //        }
        //        img.Dispose();
        //        #endregion



        //        if (Latitude != null && Longitude != null)
        //        {

        //            try
        //            {
        //                SqlGeographyBuilder builder = new SqlGeographyBuilder();

        //                builder.SetSrid(4326);
        //                builder.BeginGeography(OpenGisGeographyType.Point);

        //                //double lat = double.Parse(UtilMethods.ReplaceDangerous(latTxt.Text.Trim()), CultureInfo.InvariantCulture);
        //                //double ln = double.Parse(UtilMethods.ReplaceDangerous(longTxt.Text.Trim()), CultureInfo.InvariantCulture);

        //                builder.BeginFigure((double)Latitude, (double)Longitude);


        //                builder.EndFigure();
        //                builder.EndGeography();


        //                Issue issue = new Issue(IssueID);
        //                if (issue.ID <= 0)
        //                {
        //                    return "{\"" + ERROR + "\": \"A ocorrência não existe\"}";
        //                }
        //                else
        //                {
        //                    SqlGeography issuePOINT = SqlGeography.Point(Convert.ToDouble(issue.Latitude), Convert.ToDouble(issue.Longitude), 4326);

        //                    var distance = builder.ConstructedGeography.STDistance(issuePOINT);

        //                    if (distance >= Convert.ToDouble(StaticKeys.Radius))
        //                    {
        //                        return "{\"" + ERROR + "\": \"Não está perto do local onde foi reportada a ocorrência!\"}";
        //                    }

        //                }


        //                #region FILL ISSUE DATA
        //                IssueState i = new IssueState();
        //                i.GPS = builder.ConstructedGeography.ToString();
        //                i.UserID = UserID;
        //                i.Blob = File;
        //                i.BlobName = FileName;
        //                i.BlobType = FileType;
        //                i.BlobLen = FileLength;
        //                i.IssueID = IssueID;
        //                i.InternalNotes = InternalNotes;
        //                i.ReporterIDClosed = reporter.ID;


        //                if (i.Fix())
        //                {

        //                    //send mails to users

        //                    if (issue.TypeID > 0)
        //                    {

        //                        string reporterName = "";
        //                        if (issue.ReportedBy > 0)
        //                        {
        //                            Reporter r = new Reporter((int)issue.ReportedBy);
        //                            reporterName = r.Name;

        //                        }

        //                        SendEmails((int)issue.TypeID, StaticKeys.CloseIssueSubject, reporterName, issue.Subject, issue.Message, "ClosedIssue.html", UserID, ReporterID);
        //                    }


        //                    /// LOG de Actividade -----------------------------------------------------------------------------------
        //                    ///

        //                    UserLogger.PrepareLog(user.UserID.ToString(), user.Email);
        //                    UserLogger.LOGGER.Info("Fix Issue through web service (issueid=" + issue.ID + " ,reporter id= " + user.UserID + ", email=" + user.Email + ")");

        //                    ///------------------------------------------------------------------------------------------------------
        //                    ///

        //                    return "{\"" + DATA + "\": [" + JsonConvert.SerializeObject(issue, Formatting.Indented) + " ]}";
        //                }
        //                else
        //                {
        //                    return "{\"" + ERROR + "\": \"Não foi possível reportar o pedido\"}";
        //                }
        //                #endregion

        //            }
        //            catch (Exception ex)
        //            {
        //                ErrorLogger.LOGGER.Error(ex.Message, ex);
        //                return "{\"" + ERROR + "\": \"Coordenadas inválidas\"}";
        //            }
        //        }
        //        else
        //        {
        //            return "{\"" + ERROR + "\": \"A foto não tem coordenadas\"}";
        //        }


        //    }
        //    else
        //    {
        //        return "{\"" + ERROR + "\": \"O utilizador não existe\"}";

        //    }

        //}


        [WebMethod]
        public string GetMyIssues(int ReporterID, int? AccountID, int? TypeID)
        {

            if (AccountID == null)
            {
                AccountID = 2;
                if (StaticKeys.AccountID + "" != "")
                {
                    try
                    {
                        AccountID = Convert.ToInt32(StaticKeys.AccountID);
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            Reporter reporter = new Reporter(ReporterID);


            if (reporter.ID > 0)
            {


                DataSet ds = reporter.GetIssues(AccountID, TypeID, null);

                if (UtilMethods.DataSetHasData(ds))
                {
                    List<IssueJson> list = new List<IssueJson>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Issue issue = new Issue(dr);

                        if (issue.ID > 0)
                        {
                            IssueJson ij = new IssueJson(issue, true);

                            list.Add(ij);
                        }
                    }

                    //string json = JsonConvert.SerializeObject(list);

                    //return json;

                    return "{\"" + DATA + "\": " + JsonConvert.SerializeObject(list) + " }";

                    //return "{\"DataItem\": " + json + "}";

                }

                else
                {
                    //return "{\"DataItems\": {}}";
                    return "{}";
                }



            }
            else
            {
                return "{\"" + ERROR + "\": \"O utilizador não existe\"}";

            }

        }


        [WebMethod]
        public string GetTypes(int? AccountID)
        {
            if (AccountID == null)
            {
                AccountID = 2;
                if (StaticKeys.AccountID + "" != "")
                {
                    try
                    {
                        AccountID = Convert.ToInt32(StaticKeys.AccountID);
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            Account a = new Account((int)AccountID);

            if (a.ID > 0)
            {


                //List<Bitcliq.BIR.Platform.Type> list = Bitcliq.BIR.Platform.Type.List(null, (int)AccountID);

                List<Bitcliq.BIR.Platform.Type> list = Bitcliq.BIR.Platform.Type.ListWithParent(null, (int)AccountID);

                return "{\"" + DATA + "\": " + JsonConvert.SerializeObject(list) + " }";


            }
            else
            {
                //return "{\"DataItems\": {}}";
                return "{}";
            }
        }



        [WebMethod]
        public string GetTypesAndSubTypes(int? AccountID)
        {
            if (AccountID == null)
            {
                AccountID = 2;
                if (StaticKeys.AccountID + "" != "")
                {
                    try
                    {
                        AccountID = Convert.ToInt32(StaticKeys.AccountID);
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            Account a = new Account((int)AccountID);

            if (a.ID > 0)
            {


                //List<Bitcliq.BIR.Platform.Type> list = Bitcliq.BIR.Platform.Type.List(null, (int)AccountID);

                List<Bitcliq.BIR.Platform.Type> list = Bitcliq.BIR.Platform.Type.List(null, (int)AccountID);

                return "{\"" + DATA + "\": " + JsonConvert.SerializeObject(list) + " }";


            }
            else
            {
                //return "{\"DataItems\": {}}";
                return "{}";
            }
        }



        #region DELETE ISSUE

        [WebMethod]
        public string DeleteIssue(int ReporterID, int? AccountID, int IssueID)
        {




            if (AccountID == null)
            {
                AccountID = 2;
                if (StaticKeys.AccountID + "" != "")
                {
                    try
                    {
                        AccountID = Convert.ToInt32(StaticKeys.AccountID);
                    }
                    catch (Exception)
                    {
                    }
                }
            }




            Reporter reporter;

            if (ReporterID > 0)
            {
                reporter = new Reporter((int)ReporterID);


                Issue issue = new Issue(IssueID);

                if (issue.ID > 0)
                {
                    if (issue.Delete())
                    {
                        return "{\"" + DATA + "\": \"Ocorrência eliminada com sucesso\"}";
                    }
                    else
                    {
                        return "{\"" + ERROR + "\": \"Não foi possível eliminar a Ocorrência\"}";
                    }
                }
                else
                    return "{\"" + ERROR + "\": \"A Ocorrência não existe\"}";
            }
            else
            {
                return "{\"" + ERROR + "\": \"O utilizador não existe\"}";
            }




            if (reporter.ID > 0)
            {




            }
            else
            {
                return "{\"" + ERROR + "\": \"O utilizador não existe\"}";

            }

        }




        #endregion


        #region GET NEAR BY ISSUES

        [WebMethod]
        public string GetNearByIssues(int? AccountID, double Latitude, double Longitude, int? Radius)
        {




            if (AccountID == null)
            {
                AccountID = 2;
                if (StaticKeys.AccountID + "" != "")
                {
                    try
                    {
                        AccountID = Convert.ToInt32(StaticKeys.AccountID);
                    }
                    catch (Exception)
                    {
                    }
                }
            }




            if (Latitude != null && Longitude != null)
            {

                try
                {
                    SqlGeographyBuilder builder = new SqlGeographyBuilder();

                    builder.SetSrid(4326);
                    builder.BeginGeography(OpenGisGeographyType.Point);

                    //double lat = double.Parse(UtilMethods.ReplaceDangerous(latTxt.Text.Trim()), CultureInfo.InvariantCulture);
                    //double ln = double.Parse(UtilMethods.ReplaceDangerous(longTxt.Text.Trim()), CultureInfo.InvariantCulture);

                    builder.BeginFigure((double)Latitude, (double)Longitude);


                    builder.EndFigure();
                    builder.EndGeography();





                    DataSet ds = Issue.GetNearestIssues((int)AccountID, Radius, builder.ConstructedGeography.ToString());

                    if (UtilMethods.DataSetHasData(ds))
                    {
                        List<IssueJson> list = new List<IssueJson>();

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            Issue issue = new Issue(dr);

                            if (issue.ID > 0)
                            {
                                IssueJson ij = new IssueJson(issue, true);

                                list.Add(ij);
                            }
                        }


                        return "{\"" + DATA + "\": " + JsonConvert.SerializeObject(list) + " }";


                    }
                    else
                    {
                        return "{\"" + ERROR + "\": \"Não foi possível obter as Ocorrências\"}";
                    }


                }
                catch (Exception ex)
                {
                    ErrorLogger.LOGGER.Error(ex.Message, ex);
                    return "{\"" + ERROR + "\": \"Coordenadas inválidas\"}";
                }


            }
            else
            {
                return "{\"" + ERROR + "\": \"Coordenadas inválidas\"}";
            }


        }

        [WebMethod]
        public string CountNearByIssues(int? AccountID, double Latitude, double Longitude, int? Radius)
        {




            if (AccountID == null)
            {
                AccountID = 2;
                if (StaticKeys.AccountID + "" != "")
                {
                    try
                    {
                        AccountID = Convert.ToInt32(StaticKeys.AccountID);
                    }
                    catch (Exception)
                    {
                    }
                }
            }




            if (Latitude != null && Longitude != null)
            {

                try
                {
                    SqlGeographyBuilder builder = new SqlGeographyBuilder();

                    builder.SetSrid(4326);
                    builder.BeginGeography(OpenGisGeographyType.Point);

                    //double lat = double.Parse(UtilMethods.ReplaceDangerous(latTxt.Text.Trim()), CultureInfo.InvariantCulture);
                    //double ln = double.Parse(UtilMethods.ReplaceDangerous(longTxt.Text.Trim()), CultureInfo.InvariantCulture);

                    builder.BeginFigure((double)Latitude, (double)Longitude);


                    builder.EndFigure();
                    builder.EndGeography();





                    int cont = Issue.CountNearestIssues((int)AccountID, Radius, builder.ConstructedGeography.ToString());

                    return "{\"" + DATA + "\": " + cont + " }";


                }
                catch (Exception ex)
                {
                    ErrorLogger.LOGGER.Error(ex.Message, ex);
                    return "{\"" + ERROR + "\": \"Não foi possível obter o número de ocorrências\"}";
                }


            }
            else
            {
                return "{\"" + ERROR + "\": \"Não foi possível obter o número de ocorrências\"}";
            }


        }

        #endregion

        #region PROPERTIES

        [WebMethod]
        public string ListProperties(int? AccountID)
        {




            if (AccountID == null)
            {
                AccountID = 2;
                if (StaticKeys.AccountID + "" != "")
                {
                    try
                    {
                        AccountID = Convert.ToInt32(StaticKeys.AccountID);
                    }
                    catch (Exception)
                    {
                    }
                }
            }



            try
            {


                List<Property> list = Property.List((int)AccountID);


                List<PropertyJson> listJson = new List<PropertyJson>();

                foreach (Property p in list)
                {

                    PropertyJson pj = new PropertyJson(p);
                    listJson.Add(pj);

                }


                return "{\"" + DATA + "\": " + JsonConvert.SerializeObject(listJson) + " }";




            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return "{\"" + ERROR + "\": \"Não foi possível obter as Propriedades\"}";
            }





        }




        [WebMethod]
        public string InsertProperty(int? AccountID, int? ReporterID, string Name, string Email, string Password, string PhoneNumber, string PropertyName)
        {





            ErrorLogger.LOGGER.Error("Report and register", new Exception("sss"));



            if (AccountID == null)
            {
                AccountID = 2;
                if (StaticKeys.AccountID + "" != "")
                {
                    try
                    {
                        AccountID = Convert.ToInt32(StaticKeys.AccountID);
                    }
                    catch (Exception)
                    {
                    }
                }
            }




            Reporter reporter;

            if (ReporterID > 0)
            {
                reporter = new Reporter((int)ReporterID);
            }
            else
            {

                if (Email + "" == "")//|| Password + "" == "")
                {
                    return "{\"" + ERROR + "\": \"Preencha o Email\"}";
                }



                reporter = new Reporter();
                reporter.Email = Email;
                reporter.Name = Name;
                reporter.Password = Password;
                reporter.PhoneNumber = PhoneNumber;

                reporter.Save();

                ReporterID = reporter.ID;
            }



            if (reporter.ID > 0)
            {
                try
                {

                    #region FILL ISSUE DATA
                    Property p = new Property();

                    p.CreatedBy_Reporter = ReporterID;
                    p.Name = PropertyName;


                    if (p.Save())
                    {
                        PropertyJson ij = new PropertyJson(p);

                        /// LOG de Actividade -----------------------------------------------------------------------------------
                        ///

                        UserLogger.PrepareLog(reporter.ID.ToString(), reporter.Email);
                        UserLogger.LOGGER.Info("Create Property through web service (issueid=" + p.ID + " ,reporter id= " + reporter.ID + ", email=" + reporter.Email + ")");

                        ///------------------------------------------------------------------------------------------------------
                        ///




                        return "{\"" + DATA + "\": [" + JsonConvert.SerializeObject(ij, Formatting.Indented) + " ]}";
                    }
                    else
                    {
                        return "{\"" + ERROR + "\": \"Não foi possível inserir a propriedade\"}";
                    }
                    #endregion

                }
                catch (Exception ex)
                {
                    ErrorLogger.LOGGER.Error(ex.Message, ex);
                    return "{\"" + ERROR + "\": \"Não foi possível inserir a propriedade\"}";
                }



            }
            else
            {
                return "{\"" + ERROR + "\": \"O utilizador não existe\"}";

            }

        }


        #endregion
    }
}
