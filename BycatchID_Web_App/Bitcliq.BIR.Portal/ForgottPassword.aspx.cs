using Bitcliq.BIR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bitcliq.BIR.Portal
{
    public partial class ForgottPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            errorDiv.Style["display"] = "none";
            successDiv.Style["display"] = "none";
        }

        protected void loginButton_Click(object sender, EventArgs e)
        {
            //string pwd = "";

            ////if (passwordTextBox.Text == " - - - - " && hdPwdSave.Value != "")
            ////{
            ////    pwd = System.Web.Security.Decrypt(hdPwdSave.Value, "mkwsaeie_" + WebKeys.Skin, WebKeys.Skin + "_mwp4", "SHA1", 2, "h7d6hf3eye73he62", 256);
            ////}
            ////else
            ////{
            ////    pwd = passwordTextBox.Text.Trim();
            ////}

            errorDiv.Style["display"] = "none";
            successDiv.Style["display"] = "none";

            Bitcliq.BIR.Platform.User u = Bitcliq.BIR.Platform.User.ListByEmail(email.Value.Trim());

            if (u == null)
            {
                errorDiv.Style["display"] = "block";

            }
            else
            {

                Guid g = Guid.NewGuid();

                Bitcliq.BIR.Platform.User.ResetPassword(u.UserID, g);

                string message = UtilMethods.ReadTextFile(StaticKeys.BackofficeTemplatesPath + @"RecPassword.html");



                message = UtilMethods.ReplaceTag(message, "ImgUrl", StaticKeys.BackofficeUrl);
                message = UtilMethods.ReplaceTag(message, "Url", StaticKeys.BackofficeUrl + "ChangePassword.aspx?r=" + g);
                message = UtilMethods.ReplaceTag(message, "EntityName", StaticKeys.EntityName);

                if (u.Email != "")
                {

                    //SendMail m = new SendMail("noreply@makewise.pt", u.Email, "Recuperação de senha", message);

                    //bool mailSend = SendMail.Send(new Guid(StaticKeys.EmailsContextKey), u.Email, "Alteração de senha", message);

                    SendMail sm = new SendMail(StaticKeys.MailFrom, u.Email, "Alteração de senha", message, null);


                    errorDiv.Style["display"] = "none";
                    successDiv.Style["display"] = "block";



                }
                else
                {
                    errorDiv.Style["display"] = "block";
                    successDiv.Style["display"] = "none";
                }



            }


        }

    }
}