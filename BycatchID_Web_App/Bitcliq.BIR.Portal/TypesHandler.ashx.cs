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
    /// Summary description for TypesHandler
    /// </summary>
    public class TypesHandler : IHttpHandler, IRequiresSessionState
    {

    

        private static string tbName = "Tipo";
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


                    Bitcliq.BIR.Platform.Type tt = new Bitcliq.BIR.Platform.Type();



                    ArrayList listDynamicColmns = new ArrayList();

                    foreach (string key in forms.AllKeys)
                    {
                        if (key != "action" && key != "table" && key != "id")
                        {

                            string cellName = key.Replace("data[", "");
                            cellName = cellName.Substring(0, cellName.Length - 1);


                           

                            
                                System.Reflection.PropertyInfo prop = typeof(Bitcliq.BIR.Platform.Type).GetProperty(cellName);
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
                        context.Response.Write("{\"fieldErrors\": [{\"name\": \"Name\",\"status\": \"Name is Required!\"}]}");



                        return;
                    }
                   

                    else
                    {

                        int cont = 0;



                        cont = Bitcliq.BIR.Platform.Type.CountByName(null, tt.Name.Trim(), tt.ParentID);
                        if (cont > 0)
                        {
                            context.Response.Write("{\"fieldErrors\": [{\"name\": \"Name\",\"status\": \"Fish Specie already exists!\"}]}");

                            //context.Response.Write("{\"id\": -1,\"fieldErrors\": [],\"sError\": \"O Nome já está registado\",\"aaData\": []}");

                            return;


                         
                        }
                        else
                        {

                            tt.AccountID = u.AccountID;
                            if (tt.Save(u.UserID))
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

                    Bitcliq.BIR.Platform.Type tt = new Bitcliq.BIR.Platform.Type((int)UtilMethods.toInt(context.Request["data[]"], 0));


                    int cont = tt.CountInType();
                    if (cont > 0)
                    {
                        context.Response.Write(UtilMethods.ReplaceTag(errorResponse, "action", "eliminar"));
                    }
                    else
                    {

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

                }
                #endregion
                #region UPDATE
                else if (context.Request["action"] + "" == "edit")
                {
                    Bitcliq.BIR.Platform.Type tt = new Bitcliq.BIR.Platform.Type(Convert.ToInt32(forms["id"]));

                    ArrayList listDynamicColmns = new ArrayList();

                    foreach (string key in forms.AllKeys)
                    {
                        if (key != "action" && key != "table" && key != "id")
                        {

                            string cellName = key.Replace("data[", "");
                            cellName = cellName.Substring(0, cellName.Length - 1);

                            //int id = 0;

                            //int col = Convert.ToInt32(context.Request["column"]);
                            //try
                            //{
                            //    id = Convert.ToInt32(context.Request["row_id"]);
                            //}
                            //catch (Exception)
                            //{
                            //    context.Response.Write("Error");
                            //    return;
                            //}

                            //string cellValue = context.Request["value"];

                            //string cellName1 = context.Request["columnName"] + "";

                           
                            System.Reflection.PropertyInfo prop = typeof(Bitcliq.BIR.Platform.Type).GetProperty(cellName);
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
                        context.Response.Write("{\"fieldErrors\": [{\"name\": \"Name\",\"status\": \"Name is Required!\"}]}");



                        return;
                    }

                    else
                    {

                        int cont = 0;



                        cont = Platform.Type.CountByName(tt.ID, tt.Name.Trim(), tt.ParentID);
                        if (cont > 0)
                        {
                            context.Response.Write("{\"fieldErrors\": [{\"name\": \"Name\",\"status\": \"Fish Specie already exists\"}]}");


                            return;
                        }
                        if (tt.Save(u.UserID))
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

                    if (SortingCols == 0)
                        sort = " o.id ";

                    else if (SortingCols == 1)
                        sort = " o.name ";
                   


                    if (context.Request["sSortDir_0"] + "" != "")
                        sortingDir = context.Request["sSortDir_0"];


                    context.Response.ContentType = "application/json";

                    Bitcliq.BIR.Platform.Type tt = new Bitcliq.BIR.Platform.Type();


                    DataSet ds = tt.ListToDataSetWithoutParent(u.UserID, search, iDisplayStart, iDisplayLength, sort + sortingDir);


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
