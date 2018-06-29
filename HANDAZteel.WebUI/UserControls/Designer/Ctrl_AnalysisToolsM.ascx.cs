using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HANDAZ.PEB.WebUI.UserControls.Designer
{
    public partial class Ctrl_DesignerChoice : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Btn_Sap2000_Click(object sender, EventArgs e)
        {
            
        }

        protected void Btn_Staad_Click(object sender, EventArgs e)
        {

        }

        protected void Btn_robot_Click(object sender, EventArgs e)
        {

        }

        protected void Btn_design_Click(object sender, EventArgs e)
        {
            Response.Redirect("DesignCode.aspx");
        }
    }
}