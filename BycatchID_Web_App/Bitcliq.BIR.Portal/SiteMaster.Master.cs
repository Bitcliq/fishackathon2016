using Bitcliq.BIR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bitcliq.BIR.Portal
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            linkLogout.ServerClick += linkLogout_ServerClick;

          

             if (Session["user"] != null)
             {
                 Bitcliq.BIR.Platform.User user = (Bitcliq.BIR.Platform.User)Session["user"];

                 h5Howdy.InnerHtml = "Olá " + user.Name;
                 spanUserName.InnerHtml = user.Name;




                 if (user.ProfileID + "" != StaticKeys.AdminProfileID)
                 {
                     linkTypes.Style["display"] = "none";
                     linkUsers.Style["display"] = "none";
                     //linkCategories.Style["display"] = "none";
                 }

                 if(StaticKeys.OnlyAdminCanSee.ToLower() == "true")
                 {
                     if(!user.ISAdmin)
                     { 
                         linkTypes.Style["display"] = "none";
                         linkUsers.Style["display"] = "none";
                         //linkCategories.Style["display"] = "none";
                     }
                 }
             }
        }

        void linkLogout_ServerClick(object sender, EventArgs e)
        {
            Session["user"] = null;

            Session.Clear();
            Response.Redirect(StaticKeys.LoginUrl);

        }

    }
}