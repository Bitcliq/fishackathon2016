using Bitcliq.BIR.Logs;
using Bitcliq.BIR.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Bitcliq.BIR.Portal
{
    /// <summary>
    /// Summary description for PropertiesHandler
    /// </summary>
    public class PropertiesHandler : IHttpHandler, IRequiresSessionState
    {


        private static string tbName = "Propriedade";
        private string sucessResponse = "{\"id\": -1,\"fieldErrors\": [],\"sError\": \"\",\"aaData\": []}";

        private string errorResponse = "{\"id\": \"-1\",\"fieldErrors\": [],\"error\": \"Erro a <!--@action--> " + tbName + "\",\"aaData\": []}";




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

                #region ADD
                if (context.Request["action"] + "" == "create")
                {


                    Bitcliq.BIR.Platform.Property tt = new Bitcliq.BIR.Platform.Property();



                    ArrayList listDynamicColmns = new ArrayList();

                    foreach (string key in forms.AllKeys)
                    {
                        if (key != "action" && key != "table" && key != "id")
                        {

                            string cellName = key.Replace("data[", "");
                            cellName = cellName.Substring(0, cellName.Length - 1);





                            System.Reflection.PropertyInfo prop = typeof(Bitcliq.BIR.Platform.Property).GetProperty(cellName);
                            if (prop != null)
                            {
                                try
                                {
                                    try
                                    {
                                        object value = null;
                                        try
                                        {
                                            value = System.Convert.ChangeType(forms[key], Nullable.GetUnderlyingType(prop.PropertyType));

                                            prop.SetValue(tt, value, null);

                                        }
                                        catch (InvalidCastException)
                                        {

                                        }
                                    }
                                    catch (ArgumentNullException)
                                    {

                                        prop.SetValue(tt, forms[key]);
                                    }
                                }
                                catch (Exception)
                                {
                                    // not a key
                                }
                            }

                        }

                    }

                    #region INSERT

                    if (tt.Name.Trim() == "")
                    {
                        context.Response.Write("{\"fieldErrors\": [{\"name\": \"Name\",\"status\": \"O Nome é obrigatório!\"}]}");



                        return;
                    }


                    else
                    {

                        int cont = 0;



                        cont = Bitcliq.BIR.Platform.Property.CountByName(tt.Name.Trim(), null, u.AccountID);
                        if (cont > 0)
                        {
                            context.Response.Write("{\"fieldErrors\": [{\"name\": \"Name\",\"status\": \"A propriedade já existe!\"}]}");

                            //context.Response.Write("{\"id\": -1,\"fieldErrors\": [],\"sError\": \"O Nome já está registado\",\"aaData\": []}");

                            return;



                        }
                        else
                        {

                            tt.AccountID = u.AccountID;
                            tt.CreatedBy_User = u.UserID;
                            if (tt.Save())
                            {


                                UserLogger.PrepareLog(u.UserID.ToString(), u.Email);
                                UserLogger.LOGGER.Info("Insert " + tbName + " : id=" + tt.ID + ", code= " + tt.Name);
                                context.Response.Write(sucessResponse);

                            }
                            else
                                context.Response.Write(UtilMethods.ReplaceTag(errorResponse, "action", "inserir"));
                        }



                    }
                    #endregion



                }
                #endregion

                #region DELETE
                else if (context.Request["action"] + "" == "remove")
                {

                    Bitcliq.BIR.Platform.Property tt = new Bitcliq.BIR.Platform.Property((int)UtilMethods.toInt(context.Request["data[]"], 0));
                    tt.ModifiedBy = u.UserID;

                    if (tt.Delete())
                    {

                        UserLogger.PrepareLog(u.UserID.ToString(), u.Email);
                        UserLogger.LOGGER.Info("Delete " + tbName + " : id=" + tt.ID + ", name= " + tt.Name);

                        context.Response.Write(sucessResponse);
                    }
                    else
                    {
                        context.Response.Write(UtilMethods.ReplaceTag(errorResponse, "action", "eliminar"));
                    }

                }
                #endregion
                #region UPDATE
                else if (context.Request["action"] + "" == "edit")
                {
                    Bitcliq.BIR.Platform.Property tt = new Bitcliq.BIR.Platform.Property(Convert.ToInt32(forms["id"]));

                    ArrayList listDynamicColmns = new ArrayList();

                    foreach (string key in forms.AllKeys)
                    {
                        if (key != "action" && key != "table" && key != "id")
                        {

                            string cellName = key.Replace("data[", "");
                            cellName = cellName.Substring(0, cellName.Length - 1);

                       

                            System.Reflection.PropertyInfo prop = typeof(Bitcliq.BIR.Platform.Property).GetProperty(cellName);
                            if (prop != null)
                            {
                                try
                                {
                                    try
                                    {
                                        object value = null;
                                        try
                                        {
                                            value = System.Convert.ChangeType(forms[key], Nullable.GetUnderlyingType(prop.PropertyType));

                                            prop.SetValue(tt, value, null);

                                        }
                                        catch (InvalidCastException)
                                        {

                                        }
                                    }
                                    catch (ArgumentNullException)
                                    {
                                        prop.SetValue(tt, forms[key]);
                                    }
                                }
                                catch (Exception)
                                {
                                    // not a key
                                }
                            }



                        }

                    }

                    #region UPDATE
                    if (tt.Name.Trim() == "")
                    {
                        context.Response.Write("{\"fieldErrors\": [{\"name\": \"Name\",\"status\": \"O Nome é obrigatório!\"}]}");



                        return;
                    }

                    else
                    {

                        int cont = 0;



                        cont = Platform.Property.CountByName(tt.Name.Trim(), tt.ID, tt.AccountID);
                        if (cont > 0)
                        {
                            context.Response.Write("{\"fieldErrors\": [{\"name\": \"Name\",\"status\": \"A propriedade já existe\"}]}");


                            return;
                        }

                        tt.ModifiedBy = u.UserID;
                        if (tt.Save())
                        {
                            UserLogger.PrepareLog(u.UserID.ToString(), u.Email);
                            UserLogger.LOGGER.Info("Update " + tbName + " : id=" + tt.ID + ", name= " + tt.Name);

                            context.Response.Write(sucessResponse);

                        }
                        else
                        {
                            context.Response.Write(UtilMethods.ReplaceTag(errorResponse, "action", "alterar"));
                        }
                    }
                    #endregion



                }
                #endregion




                #region LIST
                else
                {

                    int iColumns = 0, iDisplayStart = 0, iDisplayLength = 0;


                    string echo = context.Request["sEcho"] + "";

                    iColumns = Convert.ToInt32(context.Request["iColumns"]);
                    iDisplayStart = Convert.ToInt32(context.Request["iDisplayStart"]);
                    iDisplayLength = Convert.ToInt32(context.Request["iDisplayLength"]);
                    string search = context.Request["sSearch"];


                    int SortingCols = 0;
                    string sort = "id ";



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
                        sort = " name ";

                 



                    if (context.Request["sSortDir_0"] + "" != "")
                        sortingDir = context.Request["sSortDir_0"];


                    context.Response.ContentType = "application/json";

                    Bitcliq.BIR.Platform.Property tt = new Bitcliq.BIR.Platform.Property();

                    tt.AccountID = u.AccountID;
                    DataSet ds = tt.ListToDataSet(tt.AccountID, search, iDisplayStart, iDisplayLength, sort + sortingDir);


                    context.Response.Write(UtilMethods.JsonForDataTable(echo, ds, iDisplayLength, iDisplayStart, false));

                }
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
