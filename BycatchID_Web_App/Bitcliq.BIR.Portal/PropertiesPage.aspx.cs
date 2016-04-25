using Bitcliq.BIR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bitcliq.BIR.Portal
{
    public partial class PropertiesPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null)
            {
                Response.Redirect(StaticKeys.LoginUrl);
            }
            else
            {
                Bitcliq.BIR.Platform.User user = (Bitcliq.BIR.Platform.User)Session["user"];
                if (StaticKeys.OnlyAdminCanSee.ToLower() == "true")
                {
                    if (!user.ISAdmin)
                    {
                        Response.Redirect("LoginForm.aspx");
                    }
                }
            }
        }
    }
}