using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bitcliq.BIR.Portal
{
    public partial class Issues : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["r"] + "" != "")
            {
                HDReporter.Value = Request["r"];
            }
        }
    }
}