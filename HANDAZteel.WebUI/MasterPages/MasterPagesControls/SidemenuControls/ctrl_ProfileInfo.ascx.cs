using HANDAZ.PEB.WebUI.EFW;
using HANDAZ.PEB.WebUI.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HANDAZ.PEB.WebUI.UserControls.Designer
{
    public partial class ctrl_ProfileInfo : System.Web.UI.UserControl
    {
        MembershipUser activeMember;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public string GetImageURL()
        {
            try
            {
                activeMember = Membership.GetUser(); //If the user hasn't got SQL
            }
            catch (Exception)
            {
            }
            if (activeMember != null)
            {
                Member myMember = MemberBLL.GetbyMembershipId((Guid)activeMember.ProviderUserKey);
                if (myMember.Image != null)
                {
                    return "data:image/jpeg;base64," + Convert.ToBase64String(myMember.Image);
                }
                return "/images/Profile Pictures/TempPicture.jpg";
            }
            /// TODO : n5tar Picture mo5tlfa for Anonymous user
            else return "/images/Profile Pictures/TempPicture.jpg";
        }

        public string GetUserFullName()
        {
            try
            {
                activeMember = Membership.GetUser(); //If the user hasn't got SQL
            }
            catch (Exception)
            {
            }
            if (activeMember != null)
            {
                Member myMember = MemberBLL.GetbyMembershipId((Guid)activeMember.ProviderUserKey);
                if (myMember.FullName != "")
                {
                    return myMember.FullName;
                }
                else
                {
                    // law lsa md5lsh el first wel las name bta3o
                    return activeMember.UserName;
                }
            }
            /// TODO : n5tar Picture mo5tlfa for Anonymous user
            else return "Anonymous User";

        }
    }
}