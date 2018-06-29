using HANDAZ.PEB.WebUI.EFW;
using HANDAZ.PEB.WebUI.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HANDAZ.PEB.WebUI.UserControls.Anonymous
{
    public partial class Ctrl_SignUp : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    MembershipUser activeMember = Membership.GetUser(); //If the user hasn't got SQL
                    if (activeMember != null)
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
     
        protected void CreateUserWizard2_CreatedUser(object sender, EventArgs e)
        {
            MembershipUser NewUser = Membership.GetUser(CreateUserWizard1.UserName);
            MemberBLL.Add(NewUser);
            Member activeMember = MemberBLL.GetbyMembershipId((Guid)NewUser.ProviderUserKey);
            UserNotificationBLL.Add(activeMember, "Welcome to HANDAZ PEB Designer , Thank you for creating your account in HANDAZ");
        }

        protected void ContinueButton_Click(object sender, EventArgs e)
        {
        }

 

        protected void FinishPreviousButton_Click(object sender, EventArgs e)
        {
            
            Response.Redirect("/Pages/Anonymous/SignUp.aspx");
        }

        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
           
            Response.Redirect("/Pages/Anonymous/Index.aspx");
        }

       

       
    }
}