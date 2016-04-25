using Bitcliq.BIR.Logs;
using Bitcliq.BIR.Platform;
using Bitcliq.BIR.Utils;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using PdfSharp;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Script.Services;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace Bitcliq.BIR.Portal
{
    public partial class Paginated : System.Web.UI.Page
    {
        private int AccountID = 2;
        private Bitcliq.BIR.Platform.User user;

        #region CREATE METADATA
        static SqlMetaData[] ids_tbltype;
        static List<SqlDataRecord> idsList;

        private static void CreateSqlMetaData()
        {
            // And this is the Tracks table type.
            ids_tbltype = new SqlMetaData[]
          { // In this type, there is a two-column key.
            new SqlMetaData("id", SqlDbType.Int, false,
                            true,  System.Data.SqlClient.SortOrder.Ascending, 0)
          };

            idsList = new List<SqlDataRecord>();
        }



        // This procedure reads the files and stores the contents in a List.
        private static void AddID(int val)
        {
            SqlDataRecord rec = new SqlDataRecord(ids_tbltype);
            rec.SetInt32(0, val);
            idsList.Add(rec);
        }


        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            CreateSqlMetaData();

            hdProfileAdmin.Value = StaticKeys.AdminProfileID;

            if (Session["user"] != null)
            {
                user = (Bitcliq.BIR.Platform.User)Session["user"];

                //if (!user.ISAdmin)
                //{
                //    listFType.Style["display"] = "none";

                //}


                DataSet ds = user.ListTypesByUserID();

                if (UtilMethods.DataSetHasData(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        AddID(Convert.ToInt32(dr["TypeID"]));
                    }
                }

                AccountID = user.AccountID;
                hdAccountID.Value = user.AccountID + "";

                hdProfile.Value = user.ProfileID + "";


                if (!IsPostBack)
                {
                    BindAreasList();
                    BindAllAreasToChangeInIssue();
                    BindPropertiesList();
                    BindTypesList();
                    BindStatesList();
                    BindUsersList();
                }
            }
            else
            {
                Response.Redirect(StaticKeys.LoginUrl);
            }
        }

        #region BIND DROPDOWNLISTS


        private void BindAreasList()
        {
            List<Bitcliq.BIR.Platform.Type> list = Bitcliq.BIR.Platform.Type.ListByUserIDWithoutParent(idsList, null, AccountID, user.UserID, user.ISAdmin);

          
            listFArea.Items.Add(new ListItem("All", "0"));
            foreach (Bitcliq.BIR.Platform.Type t in list)
            {
                listFArea.Items.Add(new ListItem(t.Name, t.ID + ""));
               
            }

        }


        private void BindAllAreasToChangeInIssue()

        {

            List<Bitcliq.BIR.Platform.Type> list = Bitcliq.BIR.Platform.Type.List(null, AccountID);

            listArea.Items.Add(new ListItem("Select", "0"));
        
            foreach (Bitcliq.BIR.Platform.Type t in list)
            {
                if(t.ParentID == null)
                    listArea.Items.Add(new ListItem(t.Name, t.ID + ""));
            }
        
        }



        private void BindPropertiesList()
        {
            List<Bitcliq.BIR.Platform.Property> list = Bitcliq.BIR.Platform.Property.List( AccountID);

            listProperties.Items.Add(new ListItem("Select", "0"));


            foreach (Bitcliq.BIR.Platform.Property t in list)
            {
                listProperties.Items.Add(new ListItem(t.Name, t.ID + ""));
                
            }

        }
        private void BindTypesList()
        {
            List<Bitcliq.BIR.Platform.Type> list = Bitcliq.BIR.Platform.Type.ListByUserID(idsList, null, AccountID, user.UserID, user.ISAdmin);

            //listType.Items.Add(new ListItem("Selecione", "0"));
            listFType.Items.Add(new ListItem("All", "0"));
            foreach (Bitcliq.BIR.Platform.Type t in list)
            {
                //listType.Items.Add(new ListItem(t.Name, t.ID + ""));
                listFType.Items.Add(new ListItem(t.Name, t.ID + ""));
            }

        }

        private void BindStatesList()
        {
            List<Bitcliq.BIR.Platform.State> list = Bitcliq.BIR.Platform.State.List();
            listFState.Items.Add(new ListItem("All", "0"));
            foreach (Bitcliq.BIR.Platform.State t in list)
            {
                listState.Items.Add(new ListItem(t.Name, t.ID + ""));

                // n se resolve
                if (t.ID + "" != StaticKeys.StateResolvedID)
                {

                    listFState.Items.Add(new ListItem(t.Name, t.ID + ""));
                }
            }

        }


        private void BindUsersList()
        {
            List<Bitcliq.BIR.Platform.User> list = Bitcliq.BIR.Platform.Type.ListUsersByTypes(idsList);
            usersList.Items.Add(new ListItem("Select", "0"));
            foreach (Bitcliq.BIR.Platform.User t in list)
            {
                usersList.Items.Add(new ListItem(t.Name, t.UserID + ""));

            }

        }
        #endregion

        #region WEB METHODS


        #region GET REPORTER DETAIL
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string GetReporter(int reporterID, int isReporter)
        {
            if (isReporter == 1)
            {
                Reporter rep = new Reporter(Convert.ToInt32(reporterID));

                if (rep.ID > 0)
                {
                    ReporterJson ij = new ReporterJson(rep);
                    string json = JsonConvert.SerializeObject(ij);

                    return json;

                }
                else
                {

                    return "{\"DataItem\": {}}";
                }
            }
            else if(isReporter == 0)
            {
                User rep = new User(Convert.ToInt32(reporterID));

                if (rep.UserID > 0)
                {
                    ReporterJson ij = new ReporterJson(rep);
                    string json = JsonConvert.SerializeObject(ij);

                    return json;

                }
                else
                {

                    return "{\"DataItem\": {}}";
                }
            }
             else
                return "{\"DataItem\": {}}";
        }
        #endregion

        #region GET DETAILS
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string GetIssues(string accountID, string typeID, string priority, string state, string si, string numRec, string parent)
        {
            Account account = new Account(Convert.ToInt32(accountID));

            if (parent != "0" && typeID == "0")
            {
                idsList.Clear();
                List<Bitcliq.BIR.Platform.Type> listCategories = Bitcliq.BIR.Platform.Type.ListByParentID2(null, account.ID, Convert.ToInt32(parent));
                AddID(Convert.ToInt32(parent));
                foreach (Bitcliq.BIR.Platform.Type tp in listCategories)
                {
                    AddID(tp.ID);
                }
            }
            else
            {
                if (typeID != "0")
                {

                    if (Convert.ToInt32(typeID) > 0)
                    {
                        idsList.Clear();


                        AddID(Convert.ToInt32(typeID));
                    }
                }
                else
                {

                    idsList.Clear();
                    User u = (User)HttpContext.Current.Session["user"];
                    DataSet dst = u.ListTypesByUserID();

                    if (UtilMethods.DataSetHasData(dst))
                    {
                        foreach (DataRow drt in dst.Tables[0].Rows)
                        {
                            AddID(Convert.ToInt32(drt["TypeID"]));
                        }
                    }
                }
            }

            DataSet ds = account.GetIssues(idsList, Convert.ToInt32(priority), Convert.ToInt32(state), Convert.ToInt32(si), Convert.ToInt32(numRec));


            string items = "";
            if (UtilMethods.DataSetHasData(ds))
            {


                int i = 1;
                string content = UtilMethods.ReadTextFile(StaticKeys.BackofficeTemplatesPath + "ItemReorderable.html");
                string contentIssueStatesList = UtilMethods.ReadTextFile(StaticKeys.BackofficeTemplatesPath + "IssueStateList.html");
                string contentIssueState = UtilMethods.ReadTextFile(StaticKeys.BackofficeTemplatesPath + "IssueState.html");

                string states = "";

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string aux = content;

                    // for sortIndex
                    aux = UtilMethods.ReplaceTag(aux, "I", i + "");

                    aux = UtilMethods.ReplaceTag(aux, "ID", dr["ID"] + "");
                    aux = UtilMethods.ReplaceTag(aux, "Subject", dr["Subject"] + "");
                    aux = UtilMethods.ReplaceTag(aux, "Priority", dr["Priority"] + "");



                    DateTime dt = Convert.ToDateTime(dr["DateReported"]);
                    aux = UtilMethods.ReplaceTag(aux, "DateReported", dt.ToString("dd-MM-yyyy") + "");
                    aux = UtilMethods.ReplaceTag(aux, "HoursReported", dt.ToString("HH:mm") + "");

                    if (dr["Type"] + "" != "")
                        aux = UtilMethods.ReplaceTag(aux, "Type", "(" + dr["Type"] + ")");
                    else
                        aux = UtilMethods.ReplaceTag(aux, "Type", "");

                    //aux = UtilMethods.ReplaceTag(aux, "HoursReported", dt.ToString("HH:mm") + "");

                    if (dr["State"] + "" != "")
                    {
                        State s = new State(Convert.ToInt32(dr["State"]));

                        aux = UtilMethods.ReplaceTag(aux, "State", s.Name + "");
                    }
                    else
                        aux = UtilMethods.ReplaceTag(aux, "State", "");


                    if (dr["ReportedBy"] + "" != "")
                    {
                        aux = UtilMethods.ReplaceTag(aux, "IDReporter", dr["ReportedBy"] + "");
                        aux = UtilMethods.ReplaceTag(aux, "Reporter", dr["Reporter"] + "");
                        aux = UtilMethods.ReplaceTag(aux, "IsReporter", "1");
                    }
                    else
                    {
                        aux = UtilMethods.ReplaceTag(aux, "IDReporter", dr["ReportedBy_UserID"] + "");
                        aux = UtilMethods.ReplaceTag(aux, "Reporter", dr["UserName"] + "");
                        aux = UtilMethods.ReplaceTag(aux, "IsReporter", "0");
                    }

                    // lista de estados do issue

                    string auxState = "";
                    states = "";

                    List<IssueState> list = IssueState.GetIssueHistory(Convert.ToInt32(dr["ID"]));

                    foreach (IssueState issueState in list)
                    {
                        auxState = contentIssueState;
                        // <!--@State-->  // <!--@Date--> // <!--@Internal Notes--> // <!--@By-->
                        auxState = UtilMethods.ReplaceTag(auxState, "State", issueState.StateName);
                        auxState = UtilMethods.ReplaceTag(auxState, "Date", issueState.StateDate + "");
                        auxState = UtilMethods.ReplaceTag(auxState, "Internal Notes", issueState.InternalNotes);
                        auxState = UtilMethods.ReplaceTag(auxState, "By", issueState.By + "");

                        states += auxState;

                    }


                    string auxStateList = contentIssueStatesList;

                    auxStateList = UtilMethods.ReplaceTag(auxStateList, "States", states);


                    aux = UtilMethods.ReplaceTag(aux, "History", auxStateList);


                    items += aux;
                    i++;
                }

                //string contentList = UtilMethods.ReadTextFile(StaticKeys.BackofficeTemplatesPath + "ListReorderable.html");
                //contentList = UtilMethods.ReplaceTag(contentList, "Issues", items);
                //return contentList;


                //ErrorLogger.LOGGER.Error(items, new Exception("lalalala"));

                return items;
            }
            return "";
        }




        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string GetIssueDetail(string id)
        {

            Issue issue = new Issue(Convert.ToInt32(id));

            if (issue.ID > 0)
            {
                IssueJson ij = new IssueJson(issue, false);
                string json = JsonConvert.SerializeObject(ij);

                return json;
                //return "{\"DataItem\": " + json + "}";
            }
            else
            {
                return "{\"DataItem\": {}}";
            }

        }


        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string GetUsersForIssue(string id)
        {

            Issue issue = new Issue(Convert.ToInt32(id));

            if (issue.ID > 0)
            {
                List<User> list = new List<User>();
                if (issue.TypeID > 0)
                {
                    SqlMetaData[] ids_tbltype2;
                    List<SqlDataRecord> idsList2;

                    // And this is the Tracks table type.
                    ids_tbltype2 = new SqlMetaData[]
                      { // In this type, there is a two-column key.
                        new SqlMetaData("id", SqlDbType.Int, false,
                                        true,  System.Data.SqlClient.SortOrder.Ascending, 0)
                      };

                    idsList2 = new List<SqlDataRecord>();

                    SqlDataRecord rec = new SqlDataRecord(ids_tbltype2);
                    rec.SetInt32(0, (int)issue.TypeID);
                    idsList2.Add(rec);

                    list = Bitcliq.BIR.Platform.Type.ListUsersByTypes(idsList2);

                }
                else
                {
                    list = Bitcliq.BIR.Platform.Type.ListUsersByTypes(idsList);
                }


                string json = JsonConvert.SerializeObject(list);

                return json;
                //return "{\"DataItem\": " + json + "}";
            }
            else
            {
                return "{\"DataItem\": {}}";
            }

        }


        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string GetUsersForType(string id)
        {

            if (id + "" != "")
            {
                try
                {
                    List<User> list = new List<User>();
                    int tp = Convert.ToInt32(id);
                    if (tp > 0)
                    {
                        SqlMetaData[] ids_tbltype2;
                        List<SqlDataRecord> idsList2;

                        // And this is the Tracks table type.
                        ids_tbltype2 = new SqlMetaData[]
                      { // In this type, there is a two-column key.
                        new SqlMetaData("id", SqlDbType.Int, false,
                                        true,  System.Data.SqlClient.SortOrder.Ascending, 0)
                      };

                        idsList2 = new List<SqlDataRecord>();

                        SqlDataRecord rec = new SqlDataRecord(ids_tbltype2);
                        rec.SetInt32(0, (int)tp);
                        idsList2.Add(rec);

                        list = Bitcliq.BIR.Platform.Type.ListUsersByTypes(idsList2);

                    }
                    else
                    {
                        list = Bitcliq.BIR.Platform.Type.ListUsersByTypes(idsList);
                    }


                    string json = JsonConvert.SerializeObject(list);

                    return json;
                    //return "{\"DataItem\": " + json + "}";
                }
                catch (Exception)
                {
                    return "";
                }


            }
            else
                return "";
        }

        #endregion



        #region SAVE PRIORITY AND TYPE
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string SaveIssue(string id, string typeID, string priority, string state, string sendMail,
            string message, string typeFID, string priorityF, string stateF, string notes,
            string users, string address, string imgSrc, string parent, string property)
        {
            User u = (User)HttpContext.Current.Session["user"];


            Issue issue = new Issue(Convert.ToInt32(id));

            int? propertyID = null;

            if(property + "" != "")
            {
                try{
                    propertyID = Convert.ToInt32(property);
                }
                catch{}
            }
            int? oldPriority = issue.Priority;
            int? oldState = issue.State;
            int? oldType = issue.TypeID;


            int? oldUser = issue.AssignedTo;




            if (typeID == "null")
                typeID = "0";

            if (priority == "null")
                priority = "0";

            if (issue.ID > 0)
            {
                issue.Priority = Convert.ToInt32(priority);

                if (typeID + "" != "")
                    issue.TypeID = Convert.ToInt32(typeID);

                if (state + "" != "")
                    issue.State = Convert.ToInt32(state);

                //if (userID + "" != "")
                //{
                //    if(Convert.ToInt32(userID) > 0)
                //        issue.AssignedTo = Convert.ToInt32(userID);
                //}

                bool saved = false;
                if (users + "" != "")
                {
                    if (Issue_AssignedUsers.Save(issue.ID, users))
                        saved = true;
                    else
                        return "ERROR";
                }
                else
                {
                    if (Issue_AssignedUsers.DeleteAssignedUsers(issue.ID))
                        saved = true;
                    else
                        return "ERROR";
                }
                issue.PropertyID = propertyID;

                if (issue.UpdatePriorityAndType(u.UserID, message, notes))
                {

                    /// LOG de Actividade -----------------------------------------------------------------------------------
                    ///

                    UserLogger.PrepareLog(u.UserID.ToString(), u.Email);
                    UserLogger.LOGGER.Info("Altera issue (id= " + issue.ID + ", subject=" + issue.Subject + ", state=" + issue.State + ", assignedTo= " + issue.AssignedTo + ")");

                    ///------------------------------------------------------------------------------------------------------
                    ///

                    #region SEND MAIL TO REPORTER
                    //Send Email to Issue Reporter? 0-No; 1 - Yes 

                    if (sendMail == "1")
                    {
                        string reporterEmail = "";
                        string reporterName = "";

                        if (issue.ReportedBy > 0)
                        {
                            Reporter reporter = new Reporter((int)issue.ReportedBy);
                            reporterEmail = reporter.Email;
                            reporterName = reporter.Name;

                        }
                        else if (issue.ReportedBy_UserID > 0)
                        {
                            User userReporter = new User((int)issue.ReportedBy);
                            reporterEmail = userReporter.Email;
                            reporterName = userReporter.Name;
                        }

                        if (reporterEmail + "" != "")
                        {

                            string content = UtilMethods.ReadTextFile(StaticKeys.BackofficePath + @"Templates\MailTemplate.html");


                            content = UtilMethods.ReplaceTag(content, "Reporter", reporterName + "");
                            content = UtilMethods.ReplaceTag(content, "Subject", issue.Subject);

                            content = UtilMethods.ReplaceTag(content, "Message", message);


                            string body = "A sua questão teve as seguintes alterações";
                            if (oldPriority != issue.Priority)
                            {
                                body += "A prioridade é agora" + issue.Priority + "<br/>";
                            }

                            if (oldState != issue.State)
                            {
                                State st = new State(issue.State);
                                body += "O estado é agora" + issue.State + "<br/>";
                            }

                            content = UtilMethods.ReplaceTag(content, "Body", body);
                            try
                            {
                                SendMail sm = new SendMail(StaticKeys.MailFrom, reporterEmail, StaticKeys.MailSubject, content, null);
                            }
                            catch (Exception ex)
                            {
                                ErrorLogger.LOGGER.Error(ex.Message, ex);
                            }


                            /// LOG de Actividade -----------------------------------------------------------------------------------
                            ///

                            UserLogger.PrepareLog(u.UserID.ToString(), u.Email);
                            UserLogger.LOGGER.Info("Envia  Email(email " + reporterEmail + ", id= " + issue.ID + ", subject=" + issue.Subject + ")");

                            ///------------------------------------------------------------------------------------------------------
                            ///

                        }
                    }
                    #endregion

                    if (saved)
                    {
                        if (users + "" != "")
                        {
                            string[] array = users.Split(';');

                            foreach (string usrID in array)
                            {

                                if (usrID != "")
                                {
                                    int assignedTo = Convert.ToInt32(usrID);

                                    #region SEND EMAIL TO ASSIGNED
                                    //envia email se o user a quem foi atribuido o issue é diferente do anterior
                                    if (assignedTo > 0)
                                    {


                                        string pdf = GeneratePDF(issue.ID, imgSrc, address);
                                        Uri ur = new Uri(pdf);
                                        string filename = "";
                                        filename = Path.GetFileName(ur.Segments[ur.Segments.Length - 1]);


                                        if (assignedTo > 0)
                                        {
                                            User ass = new User(assignedTo);
                                            if (ass.UserID > 0)
                                            {
                                                string content = UtilMethods.ReadTextFile(StaticKeys.BackofficeTemplatesPath + "AssignIssue.html");
                                                content = UtilMethods.ReplaceTag(content, "EntityName", StaticKeys.EntityName);


                                                content = UtilMethods.ReplaceTag(content, "User", ass.Name);

                                                content = UtilMethods.ReplaceTag(content, "Notes", notes);
                                                content = UtilMethods.ReplaceTag(content, "Url", StaticKeys.BackofficeUrl);

                                                try
                                                {
                                                    SendMail sm = new SendMail(StaticKeys.MailFrom, ass.Email, StaticKeys.AssignIssueSubject, content, StaticKeys.BackofficeTempPath + filename);
                                                }
                                                catch (Exception ex)
                                                {
                                                    ErrorLogger.LOGGER.Error(ex.Message, ex);
                                                }

                                            }
                                        }

                                        //if (oldUser > 0)
                                        //{
                                        //    User old = new User((int)oldUser);


                                        //    filename = Path.GetFileName(ur.Segments[ur.Segments.Length - 1]);


                                        //    string content = UtilMethods.ReadTextFile(StaticKeys.BackofficeTemplatesPath + "RemoveAssignIssue.html");
                                        //    content = UtilMethods.ReplaceTag(content, "EntityName", StaticKeys.EntityName);


                                        //    content = UtilMethods.ReplaceTag(content, "User", old.Name);
                                        //    content = UtilMethods.ReplaceTag(content, "Subject", issue.Subject);

                                        //    content = UtilMethods.ReplaceTag(content, "Notes", notes);
                                        //    content = UtilMethods.ReplaceTag(content, "Url", StaticKeys.BackofficeUrl);

                                        //    try
                                        //    {
                                        //        SendMail sm = new SendMail(StaticKeys.MailFrom, old.Email, StaticKeys.AssignNewIssueSubject, content, StaticKeys.BackofficeTempPath + filename);
                                        //    }
                                        //    catch (Exception ex)
                                        //    {
                                        //        ErrorLogger.LOGGER.Error(ex.Message, ex);
                                        //    }

                                        //}

                                    }
                                    #endregion
                                }
                            }
                        }

                    }

                    //#region SEND EMAIL TO ASSIGNED
                    ////envia email se o user a quem foi atribuido o issue é diferente do anterior
                    //if (oldUser != issue.AssignedTo)
                    //{


                    //    string pdf = GeneratePDF(issue.ID, imgSrc, address);
                    //    Uri ur = new Uri(pdf);
                    //    string filename = "";
                    //    filename = Path.GetFileName(ur.Segments[ur.Segments.Length - 1]);


                    //    if (issue.AssignedTo > 0)
                    //    {
                    //        User ass = new User((int)issue.AssignedTo);
                    //        if (ass.UserID > 0)
                    //        {
                    //            string content = UtilMethods.ReadTextFile(StaticKeys.BackofficeTemplatesPath + "AssignIssue.html");
                    //            content = UtilMethods.ReplaceTag(content, "EntityName", StaticKeys.EntityName);


                    //            content = UtilMethods.ReplaceTag(content, "User", ass.Name);

                    //            content = UtilMethods.ReplaceTag(content, "Notes", notes);
                    //            content = UtilMethods.ReplaceTag(content, "Url", StaticKeys.BackofficeUrl);

                    //            try
                    //            {
                    //                SendMail sm = new SendMail(StaticKeys.MailFrom, ass.Email, StaticKeys.AssignIssueSubject, content, StaticKeys.BackofficeTempPath + filename);
                    //            }
                    //            catch (Exception ex)
                    //            {
                    //                ErrorLogger.LOGGER.Error(ex.Message, ex);
                    //            }

                    //        }
                    //    }

                    //    if (oldUser > 0)
                    //    {
                    //        User old = new User((int)oldUser);


                    //        filename = Path.GetFileName(ur.Segments[ur.Segments.Length - 1]);


                    //        string content = UtilMethods.ReadTextFile(StaticKeys.BackofficeTemplatesPath + "RemoveAssignIssue.html");
                    //        content = UtilMethods.ReplaceTag(content, "EntityName", StaticKeys.EntityName);


                    //        content = UtilMethods.ReplaceTag(content, "User", old.Name);
                    //        content = UtilMethods.ReplaceTag(content, "Subject", issue.Subject);

                    //        content = UtilMethods.ReplaceTag(content, "Notes", notes);
                    //        content = UtilMethods.ReplaceTag(content, "Url", StaticKeys.BackofficeUrl);

                    //        try
                    //        {
                    //            SendMail sm = new SendMail(StaticKeys.MailFrom, old.Email, StaticKeys.AssignNewIssueSubject, content, StaticKeys.BackofficeTempPath + filename);
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            ErrorLogger.LOGGER.Error(ex.Message, ex);
                    //        }

                    //    }

                    //}
                    //#endregion

                    //return GetIssues(issue.AccountID + "", typeFID, priorityF, stateF, si, numRec, parent);

                    return "OK";
                }
                else
                    return "ERROR";
            }
            else
                return "ERROR";
        }

        #endregion

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string GetIssue(string id)
        {
            Issue i = new Issue(Convert.ToInt32(id));


            string content = UtilMethods.ReadTextFile(StaticKeys.BackofficeTemplatesPath + "ItemReorderableAfterSave.html");
            string contentIssueStatesList = UtilMethods.ReadTextFile(StaticKeys.BackofficeTemplatesPath + "IssueStateList.html");
            string contentIssueState = UtilMethods.ReadTextFile(StaticKeys.BackofficeTemplatesPath + "IssueState.html");

            string states = "";

            string reportedBy = "";
            if (i.ReportedBy > 0)
            {
                Reporter rep = new Reporter(Convert.ToInt32(i.ReportedBy));
                reportedBy = rep.Name;
            }
            else if (i.ReportedBy_UserID > 0)
            {
                User u = new User((int)i.ReportedBy_UserID);
                reportedBy = u.Name;
            }

            string aux = content;

            // for sortIndex
            aux = UtilMethods.ReplaceTag(aux, "I", i.ID + "");

            aux = UtilMethods.ReplaceTag(aux, "ID", i.ID + "");
            aux = UtilMethods.ReplaceTag(aux, "Subject", i.Subject + "");
            aux = UtilMethods.ReplaceTag(aux, "Priority", i.Priority + "");
            aux = UtilMethods.ReplaceTag(aux, "Reporter", reportedBy + "");


            DateTime dt = Convert.ToDateTime(i.DateReported);
            aux = UtilMethods.ReplaceTag(aux, "DateReported", dt.ToString("dd-MM-yyyy") + "");
            aux = UtilMethods.ReplaceTag(aux, "HoursReported", dt.ToString("HH:mm") + "");

            if (i.TypeName + "" != "")
                aux = UtilMethods.ReplaceTag(aux, "Type", "(" + i.TypeName + ")");
            else
                aux = UtilMethods.ReplaceTag(aux, "Type", "");

            //aux = UtilMethods.ReplaceTag(aux, "HoursReported", dt.ToString("HH:mm") + "");

            if (i.State + "" != "")
            {
                State s = new State(Convert.ToInt32(i.State));

                aux = UtilMethods.ReplaceTag(aux, "State", s.Name + "");
            }
            else
                aux = UtilMethods.ReplaceTag(aux, "State", "");


            // lista de estados do issue

            string auxState = "";
            states = "";

            List<IssueState> list = IssueState.GetIssueHistory(i.ID);

            foreach (IssueState issueState in list)
            {
                auxState = contentIssueState;
                // <!--@State-->  // <!--@Date--> // <!--@Internal Notes--> // <!--@By-->
                auxState = UtilMethods.ReplaceTag(auxState, "State", issueState.StateName);
                auxState = UtilMethods.ReplaceTag(auxState, "Date", issueState.StateDate + "");
                auxState = UtilMethods.ReplaceTag(auxState, "Internal Notes", issueState.InternalNotes);
                auxState = UtilMethods.ReplaceTag(auxState, "By", issueState.By + "");

                states += auxState;

            }


            string auxStateList = contentIssueStatesList;

            auxStateList = UtilMethods.ReplaceTag(auxStateList, "States", states);


            aux = UtilMethods.ReplaceTag(aux, "History", auxStateList);

            return aux;



        }
        #region MAP
        [System.Web.Services.WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<MarkerHelperResult> LoadOneMarker(string id)
        {

            List<MarkerHelperResult> tagsList = new List<MarkerHelperResult>();

            User u = (User)HttpContext.Current.Session["user"];

            Issue issue = new Issue(Convert.ToInt32(id));

            if (issue.ID > 0)
            {
                if (issue.Longitude + "" != "" && issue.Longitude + "" != "")
                {
                    tagsList.Add(new MarkerHelperResult
                    {
                        id = issue.ID + "",

                        label = "", //"E mesmo o nome", //o.Name.Replace("\"", "").Replace("'", "").Replace(@"\", "").Replace("/", "") + "",
                        lat = issue.Latitude.ToString().Replace(",", "."),
                        lng = issue.Longitude.ToString().Replace(",", "."),
                        icon = ""
                        //icon = "_img/markers/markers-base.png"
                    });
                }
            }




            return tagsList;


        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<MarkerHelperResult> LoadMarkers(string accountID, string typeID, string priority, string state)
        {
            User u = (User)HttpContext.Current.Session["user"];
            if (typeID != "")
            {
                if (Convert.ToInt32(typeID) > 0)
                {
                    idsList.Clear();
                    //idsList.Add(Convert.ToInt32(typeID));
                    AddID(Convert.ToInt32(typeID));
                }
            }
            else
            {
                idsList.Clear();


                DataSet dst = u.ListTypesByUserID();

                if (UtilMethods.DataSetHasData(dst))
                {
                    foreach (DataRow drt in dst.Tables[0].Rows)
                    {
                        AddID(Convert.ToInt32(drt["TypeID"]));
                    }
                }
            }


            Account account = new Account(Convert.ToInt32(accountID));

            List<MarkerHelperResult> tagsList = new List<MarkerHelperResult>();

            DataSet ds = account.GetIssues(idsList, Convert.ToInt32(priority), Convert.ToInt32(state));


            //DataSet ds = account.GetIssues(Convert.ToInt32(typeID), Convert.ToInt32(priority), Convert.ToInt32(state));


            if (UtilMethods.DataSetHasData(ds))
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Issue issue = new Issue(Convert.ToInt32(dr["ID"]));

                    if (issue.ID > 0)
                    {
                        if (issue.Longitude + "" != "" && issue.Longitude + "" != "")
                        {
                            tagsList.Add(new MarkerHelperResult
                            {
                                id = issue.ID + "",

                                label = "", //"E mesmo o nome", //o.Name.Replace("\"", "").Replace("'", "").Replace(@"\", "").Replace("/", "") + "",
                                lat = issue.Latitude.ToString().Replace(",", "."),
                                lng = issue.Longitude.ToString().Replace(",", "."),
                                icon = ""
                                //icon = "_img/markers/markers-base.png"
                            });
                        }
                    }

                }
            }

            return tagsList;


        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string GetDetails(int id)
        {
            Issue m = new Issue(id);

            string content = UtilMethods.ReadTextFile(StaticKeys.BackofficeTemplatesPath + "InfoWindow.htm");
            content = UtilMethods.ReplaceTag(content, "ID", m.ID + "");
            content = UtilMethods.ReplaceTag(content, "Subject", m.Subject);
            content = UtilMethods.ReplaceTag(content, "Message", m.Message);

            return content;
        }


        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string ReorderIssues(string idList)
        {
            if (!Issue.Reorder(idList))
            {
                return "ERROR";
            }
            else
            {
                User u = (User)HttpContext.Current.Session["user"];


                /// LOG de Actividade -----------------------------------------------------------------------------------
                ///

                UserLogger.PrepareLog(u.UserID.ToString(), u.Email);
                UserLogger.LOGGER.Info("Ordena issues (idList=" + idList + ")");

                ///------------------------------------------------------------------------------------------------------
                ///

            }

            return "";

        }


        #endregion



        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string GeneratePDF(int id, string imgSrc, string address)
        {
            Issue i = new Issue(Convert.ToInt32(id));

            if (i.ID > 0)
            {
                string content = UtilMethods.ReadTextFile(StaticKeys.BackofficeTemplatesPath + "PdfTemplate.html");

                IssueJson ij = new IssueJson(i, true);


                content = UtilMethods.ReplaceTag(content, "ImgSrc", imgSrc);
                //content = UtilMethods.ReplaceTag(content, "ImgSrc", ij.PhotoUrl);

                content = UtilMethods.ReplaceTag(content, "Subject", ij.Subject);
                content = UtilMethods.ReplaceTag(content, "Priority", ij.Priority + "");
                content = UtilMethods.ReplaceTag(content, "Reporter", ij.ReporterName);
                content = UtilMethods.ReplaceTag(content, "ReporterEmail", ij.ReporterEmail);
                content = UtilMethods.ReplaceTag(content, "Message", ij.Message);

                DateTime dt = Convert.ToDateTime(ij.DateReported);
                content = UtilMethods.ReplaceTag(content, "Date", dt.ToString("dd-MM-yyyy") + "");
                content = UtilMethods.ReplaceTag(content, "Type", ij.TypeName);
                content = UtilMethods.ReplaceTag(content, "State", ij.StateName);
                content = UtilMethods.ReplaceTag(content, "Hours", dt.ToString("HH:mm") + "");



                // google maps


                //http://maps.googleapis.com/maps/api/staticmap?zoom=17&size=400x400&markers=color:green|label:G|39.406938,-9.135937


                if (ij.Latitude + "" != "" && ij.Longitude + "" != "")
                {
                    content = UtilMethods.ReplaceTag(content, "GoogleSrc", StaticKeys.GoogleMapsUrl + ij.Latitude + "," + ij.Longitude);
                    content = UtilMethods.ReplaceTag(content, "Display", "");
                }
                else
                {
                    content = UtilMethods.ReplaceTag(content, "GoogleSrc", "");
                    content = UtilMethods.ReplaceTag(content, "Display", "none");
                }

                content = UtilMethods.ReplaceTag(content, "Address", address);



                //aux = UtilMethods.ReplaceTag(aux, "HoursReported", dt.ToString("HH:mm") + "");

                //if (dr["Type"] + "" != "")
                //    aux = UtilMethods.ReplaceTag(aux, "Type",i.TypeID + "");
                //else
                //    aux = UtilMethods.ReplaceTag(aux, "Type", "");

                ////aux = UtilMethods.ReplaceTag(aux, "HoursReported", dt.ToString("HH:mm") + "");

                //if (dr["State"] + "" != "")
                //{
                //    State s = new State(Convert.ToInt32(dr["State"]));

                //    aux = UtilMethods.ReplaceTag(aux, "State", s.Name + "");
                //}
                //else
                //    aux = UtilMethods.ReplaceTag(aux, "State", "");



                DateTime dt1 = DateTime.Now;

                //https://htmlrenderer.codeplex.com/
                //www.pdfsharp.net/


                PdfDocument pdf = PdfGenerator.GeneratePdf(content, PageSize.A4);
                pdf.Save(StaticKeys.BackofficeTempPath + "pdf_" + dt1.Ticks + ".pdf");
                pdf.Close();
                pdf.Dispose();



                return StaticKeys.BackofficeTempUrl + "pdf_" + dt1.Ticks + ".pdf";

            }
            else
                return "ERROR";

        }


        #endregion



        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string RotateImg(string fileName)
        {
            return UtilMethods.RotateImage(fileName);
        }




        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string GetCategories(int id)
        {
            User u = (User)HttpContext.Current.Session["user"];
            List<Bitcliq.BIR.Platform.Type> list = Bitcliq.BIR.Platform.Type.ListByParentID2(null, u.AccountID, id);

            string json = JsonConvert.SerializeObject(list);

            return json;


        }



    }
}