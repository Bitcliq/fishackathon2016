using Bitcliq.BIR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bitcliq.BIR.Portal
{
    public partial class MyProfile : System.Web.UI.Page
    {
        private Bitcliq.BIR.Platform.User user;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["user"] != null)
            {
                user = (Bitcliq.BIR.Platform.User)Session["user"];

                if (!IsPostBack)
                {
                    txtEmail.Value = user.Email;
                    txtName.Value = user.Name;
                    chkNot.Checked = !user.RecieveNotifications;

                    if (user.ProfileID + "" != StaticKeys.AdminProfileID)
                    {
                        dvEmail.Style["display"] = "none";
                    }
                }
            }
        }



        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (txtEmail.Value.Trim() != "" && txtName.Value.Trim() + "" != "")
            {
                user.Name = txtName.Value.Trim();
                user.Email = txtEmail.Value.Trim();

                user.RecieveNotifications = !chkNot.Checked;

                if (user.UpdateMyProfile())
                {
                    // ok
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid() + "", "showSuccess();", true);
                }

                else
                {
                    // erro
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid() + "", "showError('Não foi possível gravar o perfil');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid() + "", "showError('Introduza nome / Email');", true);
            }

        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string SavePassword(string password, string confirm)
        {

            Bitcliq.BIR.Platform.User user = (Bitcliq.BIR.Platform.User)HttpContext.Current.Session["User"];


            if (password != confirm)
            {

                return "Erro: As senhas não coincidem";
            }

            if (user.ChangeMyPassword(password))
                return "OK";
            else
                return "Não foi possível guardar a senha";
        }


    }
}