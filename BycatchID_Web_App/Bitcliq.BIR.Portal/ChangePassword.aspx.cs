using Bitcliq.BIR.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bitcliq.BIR.Portal
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            errorDiv.Style["display"] = "none";

        }

        protected void changeButton_Click(object sender, EventArgs e)
        {

            errorDiv.Style["display"] = "none";

            if (Request["g"] + "" != "")
            {
                try
                {
                    //Activa logo o utilizador 
                    Bitcliq.BIR.Platform.User user = Bitcliq.BIR.Platform.User.ListByActivationKey(new Guid(Request["g"] + ""));


                    if (user != null)
                    {

                        if (user.UserID > 0)
                        {

                            user.ChangedPassword = true;
                            user.Pwd = password.Text;
                            user.ChangePasswordAndActivate(password.Text);

                            UserLogger.PrepareLog(user.UserID.ToString(), user.Email);
                            UserLogger.LOGGER.Info("Change Pass And Activate()");


                            Session.Add("user", user);
                            Response.Redirect("Paginated.aspx");

                        }
                        else
                        {

                            errorDiv.Style["display"] = "block";
                        }
                    }
                    else
                    {


                        errorDiv.Style["display"] = "block";
                    }
                }
                catch (Exception ex)
                {


                    ErrorLogger.LOGGER.Error("------------- Error Log Begin ------------------");
                    ErrorLogger.LOGGER.Error(ex.Message, ex);


                    //errorDiv.Style["display"] = "block";
                    //return;
                }



            }
            else if (Request["r"] + "" != "")
            {
                try
                {
                    //Activa logo o utilizador 
                    Bitcliq.BIR.Platform.User user = Bitcliq.BIR.Platform.User.ListByRecoverGuid(new Guid(Request["r"] + ""));


                    if (user != null)
                    {

                        if (user.UserID > 0)
                        {

                            user.ChangedPassword = true;
                            user.Pwd = password.Text;
                            user.ChangePasswordAndActivate(password.Text);

                            UserLogger.PrepareLog(user.UserID.ToString(), user.Email);
                            UserLogger.LOGGER.Info("Change Pass And Activate()");


                            Session.Add("user", user);
                            Response.Redirect("Paginated.aspx");

                        }
                        else
                        {

                            errorDiv.Style["display"] = "block";
                        }
                    }
                    else
                    {


                        errorDiv.Style["display"] = "block";
                    }
                }
                catch (Exception ex)
                {


                    ErrorLogger.LOGGER.Error("------------- Error Log Begin ------------------");
                    ErrorLogger.LOGGER.Error(ex.Message, ex);


                    //errorDiv.Style["display"] = "block";
                    //return;
                }



            }
            else if (Session["user"] != null)
            {
                Bitcliq.BIR.Platform.User user = (Bitcliq.BIR.Platform.User)Session["user"];
                user.ChangedPassword = true;
                user.ChangePassword(password.Text.Trim());
                Session.Add("user", user);
                Response.Redirect("Paginated.aspx");
            }
            else
            {
                errorDiv.InnerHtml = "Invalid data!";
                errorDiv.Style["display"] = "block";
                return;
            }
        }

    }
}