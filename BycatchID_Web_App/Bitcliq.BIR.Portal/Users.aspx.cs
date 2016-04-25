using Bitcliq.BIR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bitcliq.BIR.Portal
{
    public partial class Users : System.Web.UI.Page
    {
        Bitcliq.BIR.Platform.User user;

        protected void Page_Load(object sender, EventArgs e)
        {




            if (Session["user"] != null)
            {
                string text = UtilMethods.ReadTextFile(StaticKeys.BackofficeTemplatesPath + @"usersscript.html");

                user = (Bitcliq.BIR.Platform.User)Session["user"];
                if (StaticKeys.OnlyAdminCanSee.ToLower() == "true")
                {
                    if (!user.ISAdmin)
                    {
                        Response.Redirect("LoginForm.aspx");
                    }
                }

                //user = (Bitcliq.BIR.Platform.User)Session["user"];
                //List<Organization> list = new List<Organization>();
                //if (user.SuperUser)
                //{
                //    list = Organization.List();

                //    //
                //    string super = UtilMethods.ReadTextFile(StaticKeys.PortalPath + @"Templates\superUsersScript.html");
                //    text = UtilMethods.ReplaceTag(text, "SuperUser", super);
                //    text = UtilMethods.ReplaceTag(text, "Vis", "true");

                //}
                //else
                //{
                //    list = Organization.List(user.ID);
                //    text = UtilMethods.ReplaceTag(text, "SuperUser", "");
                //    text = UtilMethods.ReplaceTag(text, "Vis", "false");

                //}

                //string typeoptions = "";

                //foreach (Organization tp in list)
                //{
                //    string val = tp.Name;
                //    val = val.Replace("\r\n", " ");
                //    val = val.Replace("\n", " ");
                //    val = val.Replace("\r", " ");
                //    val = val.Replace("\t", " ");
                //    val = val.Replace("\"", "'");


                //    if (typeoptions == "")
                //    {
                //        typeoptions = "{ 'label': '" + tp.Name + "', 'value': '" + tp.ID + "' }";

                //    }
                //    else
                //        typeoptions += ",{ 'label': '" + tp.Name + "', 'value': '" + tp.ID + "' }";
                //}




                //text = UtilMethods.ReplaceTag(text, "TypeOptions", "'ipOpts': [" + typeoptions + "]");


                litScript.Text = text;
            }
            else
            {
                Response.Redirect(StaticKeys.LoginUrl);
            }
        }
    }
}