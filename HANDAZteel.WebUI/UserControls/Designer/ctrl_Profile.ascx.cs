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
    public partial class ctrl_Profile : System.Web.UI.UserControl
    {
        MembershipUser activeMember;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                try
                {
                    activeMember = Membership.GetUser();
                }
                catch
                {
                    activeMember = null;
                }
            }
        }

        /// <summary>
        /// Return Logged In User Image as URL ,if not assigned any Picture returns the temp Picture path
        /// </summary>
        /// <returns></returns>
        public string GetImageURL()
        {
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
            if (activeMember != null)
            {
                Member myMember = MemberBLL.GetbyMembershipId((Guid)activeMember.ProviderUserKey);
                if (myMember.FullName != "")
                {
                    return myMember.FullName;
                }
                // law lsa md5lsh el first wel las name bta3o
                return activeMember.UserName;
            }
            /// TODO : n5tar Picture mo5tlfa for Anonymous user
            else return "Anonymous User";

        }
        public string GetUserJop()
        {
            if (activeMember != null)
            {
                Member myMember = MemberBLL.GetbyMembershipId((Guid)activeMember.ProviderUserKey);
                if (myMember.Jop != "")
                {
                    return "\t" + myMember.Jop;
                }
            }
            return "\t hasn't assigned yet";

        }
        public string GetUserCompany()
        {
            if (activeMember != null)
            {
                Member myMember = MemberBLL.GetbyMembershipId((Guid)activeMember.ProviderUserKey);
                if (myMember.Company != "")
                {
                    return "\t" + myMember.Company;
                }
            }
            return "\t hasn't assigned yet";

        }
        public string GetUserPhone()
        {
            if (activeMember != null)
            {
                Member myMember = MemberBLL.GetbyMembershipId((Guid)activeMember.ProviderUserKey);
                if (myMember.Phone != "")
                {
                    return "\t"+myMember.Phone;
                }
            }
            return "\t hasn't assigned yet";

        }
        public string GetUserAddress()
        {
            if (activeMember != null)
            {
                Member myMember = MemberBLL.GetbyMembershipId((Guid)activeMember.ProviderUserKey);
                if (myMember.Address != "")
                {
                    return "\t"+ myMember.Address;
                }
            }
            return "\t hasn't assigned yet";

        }
        public string GetUserEmail()
        {
            if (activeMember != null)
            {
                if (activeMember.Email != "")
                {
                    return "\t" + activeMember.Email;
                }
            }
            return "\t hasn't assigned yet";

        }

    }


}