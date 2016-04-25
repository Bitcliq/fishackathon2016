using Bitcliq.BIR.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bitcliq.BIR.Portal
{
    public partial class LoginForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void logintBtn_Click(object sender, EventArgs e)
        {
            errorDiv.Style["display"] = "none";


            Bitcliq.BIR.Platform.User user = Bitcliq.BIR.Platform.User.Login(userTxt.Value.Trim(), passTxt.Value.Trim());

            if (user == null)
                errorDiv.Style["display"] = "block";
            else
            {
                Session.Add("user", user);


                Bitcliq.BIR.Platform.User.SetLastLogin(user.UserID);
                if (user.ChangedPassword)
                {
                    /// LOG de Actividade -----------------------------------------------------------------------------------
                    ///

                    UserLogger.PrepareLog(user.UserID.ToString(), user.Email);
                    UserLogger.LOGGER.Info("Login ()");

                    ///------------------------------------------------------------------------------------------------------
                    ///

                    Response.Redirect("Paginated.aspx");

                }
                else
                {
                    Response.Redirect("ChangePassword.aspx");
                }
            }
        }

    }
}
