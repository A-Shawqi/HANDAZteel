using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HANDAZ.PEB.WebUI.UserControls.Anonymous
{
    public partial class Ctrl_LogIn : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    MembershipUser activeMember = Membership.GetUser(); //If the user hasn't got SQL
                if (activeMember!=null)
                {
                    FormsAuthentication.SignOut();
                        activeMember = null;
                }
                }
                catch (Exception)
                {
                }
            }
        }

        protected void Login1_LoggedIn(object sender, EventArgs e)
        {
           
            if (Membership.ValidateUser(Login1.UserName, Login1.Password) && Login1.RememberMeSet)
            {
                FormsAuthentication.SetAuthCookie(Login1.UserName, true);
            }
            Response.Redirect("/Pages/Designer/Profile.aspx");
        }

        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/Anonymous/Index.aspx");
        }
    }
}