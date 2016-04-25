using Bitcliq.BIR.Platform;
using Bitcliq.BIR.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Bitcliq.BIR.Portal
{
    /// <summary>
    /// Summary description for IssuesHandler
    /// </summary>
    public class IssuesHandler : IHttpHandler, IRequiresSessionState
    {

        private static string tbName = "User";
        private string sucessResponse = "{\"id\": -1,\"fieldErrors\": [],\"sError\": \"\",\"aaData\": []}";

        private string errorResponse = "{\"id\": \"-1\",\"fieldErrors\": [],\"error\": \"Error <!--@action--> " + tbName + "\",\"aaData\": []}";




        private Bitcliq.BIR.Platform.User u;

        private int? typeID = null;

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            if (context.Session["user"] != null)
            {
                u = (Bitcliq.BIR.Platform.User)context.Session["user"];
            }
            else
            {
                context.Response.Write("");
                return;
            }

            if (u != null)
            {
                System.Collections.Specialized.NameValueCollection forms = context.Request.Form;



                if (context.Request["t"] + "" != "")
                {
                    typeID = (int)UtilMethods.toInt(context.Request["t"] + "", -1);
                }


                if (typeID >= 0)
                {
                    context.Response.Write("");
                    return;
                }

                #region USER

                string oper = context.Request["oper"] + "";

                //#region ADD
                //if (context.Request["action"] + "" == "create")
                //{


                //    Reporter tt = new Reporter();



                //    ArrayList listDynamicColmns = new ArrayList();

                //    foreach (string key in forms.AllKeys)
                //    {
                //        if (key != "action" && key != "table" && key != "id")
                //        {

                //            string cellName = key.Replace("data[", "");
                //            cellName = cellName.Substring(0, cellName.Length - 1);




                //            else
                //            {
                //                System.Reflection.PropertyInfo prop = typeof(User).GetProperty(cellName);
                //                if (prop != null)
                //                {
                //                    try
                //                    {
                //                        try
                //                        {
                //                            object value = null;
                //                            try
                //                            {
                //                                value = System.Convert.ChangeType(forms[key], Nullable.GetUnderlyingType(prop.PropertyType));

                //                                prop.SetValue(tt, value, null);

                //                            }
                //                            catch (InvalidCastException)
                //                            {

                //                            }
                //                        }
                //                        catch (ArgumentNullException)
                //                        {

                //                            prop.SetValue(tt, forms[key]);
                //                        }
                //                    }
                //                    catch (Exception)
                //                    {
                //                        // not a key
                //                    }
                //                }
                //            }



                //        }

                //    }

                //    #region INSERT

                //    if (tt.Name.Trim() == "")
                //    {
                //        context.Response.Write("{\"fieldErrors\": [{\"name\": \"Name\",\"status\": \"O Nome é obrigatório\"}]}");



                //        return;
                //    }
                //    else if (tt.Email.Trim() == "")
                //    {

                //        context.Response.Write("{\"fieldErrors\": [{\"name\": \"Email\",\"status\": \"O email é obrigatório\"}]}");



                //        return;




                //    }
                //    else if (UtilMethods.ValidateEmail(tt.Email.Trim()) == "")
                //    {
                //        context.Response.Write("{\"fieldErrors\": [{\"name\": \"Email\",\"status\": \"O email é inválido\"}]}");

                //        return;



                //    }

                //    else
                //    {

                //        int cont = 0;



                //        cont = Platform.User.CountLogin(null, tt.Email.Trim());
                //        if (cont > 0)
                //        {
                //            context.Response.Write("{\"fieldErrors\": [{\"name\": \"Email\",\"status\": \"O Email já está registado\"}]}");


                //            return;
                //        }
                //        else
                //        {
                //            // we shoud do this in a different way
                //            tt.AccountID = u.AccountID; // user account has to be th same as user that is inserting
                //            // INSERT AND SEND EMAIL
                //            if (tt.Save(u.UserID))
                //            {


                //                UserLogger.PrepareLog(u.UserID.ToString(), u.Email);
                //                UserLogger.LOGGER.Info("Insert " + tbName + " : id=" + tt.UserID + ", code= " + tt.Name);




                //                //sucessResponse = "{\"id\": " + tt.ID +",\"fieldErrors\": [],\"sError\": \"\",\"aaData\": [\"id\":\"" + tt.Name + "\"]}";
                //                context.Response.Write(sucessResponse);

                //            }
                //            else
                //            {

                //                context.Response.Write(UtilMethods.ReplaceTag(errorResponse, "action", "inserting"));
                //            }
                //        }



                //    }
                //    #endregion



                //}
                //#endregion

                //#region DELETE
                //else if (context.Request["action"] + "" == "remove")
                //{

                //    Reporter tt = new Reporter((int)UtilMethods.toInt(context.Request["data[]"], 0));

                //    if (tt.UserID == u.UserID)
                //    {

                //        throw new Exception("Não pode alterar eliminar-se a si próprio!");
                //        return;

                //    }


                //    if (tt.Delete(u.UserID))
                //    {

                //        UserLogger.PrepareLog(u.UserID.ToString(), u.Email);
                //        UserLogger.LOGGER.Info("Delete " + tbName + " : id=" + tt.UserID + ", name= " + tt.Name);

                //        context.Response.Write(sucessResponse);
                //    }
                //    else
                //    {
                //        context.Response.Write(UtilMethods.ReplaceTag(errorResponse, "action", "deleting"));
                //    }

                //}
                //#endregion
                //#region UPDATE
                //else if (context.Request["action"] + "" == "edit")
                //{
                //    User tt = new User(Convert.ToInt32(forms["id"]));

                //    ArrayList listDynamicColmns = new ArrayList();

                //    foreach (string key in forms.AllKeys)
                //    {
                //        if (key != "action" && key != "table" && key != "id")
                //        {

                //            string cellName = key.Replace("data[", "");
                //            cellName = cellName.Substring(0, cellName.Length - 1);



                //            if (cellName == "AccountID")
                //            {
                //                tt.AccountID = Convert.ToInt32(forms[key]);


                //            }
                //            else if (cellName == "ProfileID")
                //            {
                //                tt.ProfileID = Convert.ToInt32(forms[key]);


                //            }
                //            else
                //            {
                //                System.Reflection.PropertyInfo prop = typeof(Bitcliq.BIR.Platform.User).GetProperty(cellName);
                //                if (prop != null)
                //                {
                //                    try
                //                    {
                //                        try
                //                        {
                //                            object value = null;
                //                            try
                //                            {
                //                                value = System.Convert.ChangeType(forms[key], Nullable.GetUnderlyingType(prop.PropertyType));

                //                                prop.SetValue(tt, value, null);

                //                            }
                //                            catch (InvalidCastException)
                //                            {

                //                            }
                //                        }
                //                        catch (ArgumentNullException)
                //                        {
                //                            prop.SetValue(tt, forms[key]);
                //                        }
                //                    }
                //                    catch (Exception)
                //                    {
                //                        // not a key
                //                    }
                //                }
                //            }



                //        }

                //    }

                //    #region UPDATE
                //    if (tt.Name.Trim() == "")
                //    {
                //        context.Response.Write("{\"fieldErrors\": [{\"name\": \"Name\",\"status\": \"O Nome é obrigatório\"}]}");



                //        return;
                //    }
                //    else if (tt.Email.Trim() == "")
                //    {

                //        context.Response.Write("{\"fieldErrors\": [{\"name\": \"Email\",\"status\": \"O email é obrigatório\"}]}");



                //        return;




                //    }
                //    else if (UtilMethods.ValidateEmail(tt.Email.Trim()) == "")
                //    {
                //        context.Response.Write("{\"fieldErrors\": [{\"name\": \"Email\",\"status\": \"O email é inválido\"}]}");

                //        return;



                //    }

                //    else
                //    {

                //        int cont = 0;



                //        cont = Platform.User.CountLogin(tt.UserID, tt.Email.Trim());
                //        if (cont > 0)
                //        {
                //            context.Response.Write("{\"fieldErrors\": [{\"name\": \"Email\",\"status\": \"O Email já está registado\"}]}");


                //            return;
                //        }
                //        if (tt.Save(u.UserID))
                //        {
                //            UserLogger.PrepareLog(u.UserID.ToString(), u.Email);
                //            UserLogger.LOGGER.Info("Update " + tbName + " : id=" + tt.UserID + ", name= " + tt.Name);

                //            context.Response.Write(sucessResponse);

                //        }
                //        else
                //        {
                //            context.Response.Write(UtilMethods.ReplaceTag(errorResponse, "action", "updating"));
                //        }
                //    }
                //    #endregion



                //}
                //#endregion
                //#region CHANGE STATUS
                //else if (context.Request["action"] + "" == "changeStatus")
                //{
                //    User tt = new User(Convert.ToInt32(context.Request["id"] + ""));


                //    if (tt.UserID == u.UserID)
                //    {
                //        context.Response.Write("{\"data\": \"Não pode alterar o seu próprio estado\"}");
                //        return;
                //    }

                //    if (tt.Active)
                //        tt.Active = false;
                //    else
                //        tt.Active = true;


                //    #region UPDATE
                //    if (tt.ChangeStatus(u.UserID))
                //    {
                //        UserLogger.PrepareLog(u.UserID.ToString(), u.Email);
                //        UserLogger.LOGGER.Info("Change Status " + tbName + " : id=" + tt.UserID + ", name= " + tt.Name);

                //        context.Response.Write("{\"data\": \"OK\"}");




                //    }
                //    else
                //    {
                //        context.Response.Write(UtilMethods.ReplaceTag(errorResponse, "action", "updating"));
                //    }
                //    #endregion



                //}
                //#endregion



                #region LIST
                //else
                //{

                int iColumns = 0, iDisplayStart = 0, iDisplayLength = 0;


                string echo = context.Request["sEcho"] + "";

                iColumns = Convert.ToInt32(context.Request["iColumns"]);
                iDisplayStart = Convert.ToInt32(context.Request["iDisplayStart"]);
                iDisplayLength = Convert.ToInt32(context.Request["iDisplayLength"]);
                string search = context.Request["sSearch"];


                int SortingCols = 0;
                string sort = "id ";
                int rid = 0;

                if (context.Request["r"] + "" != "")
                {
                    try
                    {
                        rid = Convert.ToInt32(context.Request["r"]);
                    }
                    catch (Exception)
                    {
                    }
                }

                if (context.Request["iSortCol_0"] + "" != "")
                {
                    try
                    {
                        SortingCols = Convert.ToInt32(context.Request["iSortCol_0"]);
                    }
                    catch (Exception)
                    {
                    }
                }

                string sortingDir = " desc";

                if (SortingCols == 1)
                    sort = " code ";

                if (SortingCols == 2)
                    sort = " year ";
                if (SortingCols == 4)
                    sort = " compartment ";


                if (context.Request["sSortDir_0"] + "" != "")
                    sortingDir = context.Request["sSortDir_0"];


                context.Response.ContentType = "application/json";

                Reporter tt = new Reporter(rid);


                DataSet ds = tt.ListIssuesToDataSet(u.AccountID, search, iDisplayStart, iDisplayLength, sort + sortingDir);


                context.Response.Write(UtilMethods.JsonForDataTable(echo, ds, iDisplayLength, iDisplayStart, true));

                // context.Response.Write(Trial.ListToJsonTable(echo, iDisplayLength, iDisplayStart, sort + sortingDir, false, search, true));
                //}
                #endregion


                #endregion


            }
            else
                context.Response.Write("");

        }



        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
