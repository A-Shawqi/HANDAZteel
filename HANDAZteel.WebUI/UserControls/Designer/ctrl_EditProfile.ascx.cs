using HANDAZ.PEB.WebUI.EFW;
using HANDAZ.PEB.WebUI.Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HANDAZ.PEB.WebUI.UserControls.Designer
{
    public partial class ctrl_EditProfile : System.Web.UI.UserControl
    {
        MembershipUser activeMember;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                activeMember = Membership.GetUser();
                if (activeMember != null)
                {
                    Member myMember = MemberBLL.GetbyMembershipId((Guid)activeMember.ProviderUserKey);
                    
                    img_ProfilePic.ImageUrl = GetImageURL();
                    txt_FName.Text = myMember.FullName;
                    txt_Jop.Text = myMember.Jop;
                    txt_Company.Text = myMember.Company;
                    txt_Address.Text = myMember.Address;
                    txt_Phone.Text = myMember.Phone;
                    ViewState["PPStream"] = myMember.Image;
                }
                else
                {
                    btn_SaveInfo.Visible = false;
                    btn_SavePic.Visible = false;
                }
            }
        }

        /// <summary>
        /// Return Logged In User Image as URL ,if not assigned any Picture returns the temp Picture path
        /// </summary>
        /// <returns></returns>
        public string GetImageURL()
        {
            activeMember = Membership.GetUser();
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
            activeMember = Membership.GetUser();
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

        protected void btn_upload_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                try
                {
                    string filename = Path.GetFileName(FileUpload1.PostedFile.FileName);
                    string ext = Path.GetExtension(filename);
                    if (ext == ".png" || ext == ".jpg" || ext == ".jpeg" || ext == ".PNG" || ext == ".JPG" || ext == ".JPEG" || ext == ".gif" || ext == ".GIF")
                    {

                        Stream fs = FileUpload1.PostedFile.InputStream;
                        BinaryReader br = new BinaryReader(fs);
                        Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                        string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);

                        img_ProfilePic.ImageUrl = "data:image/jpeg;base64," + base64String;
                        ViewState["PPStream"] = bytes;
                    }
                    else
                    {
                        Response.Write("<script>alert('unsupported format of photo file');</script>");
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
            }
        }

        protected void btn_SavePic_Click(object sender, EventArgs e)
        {
            activeMember = Membership.GetUser();
            if (activeMember != null)
            {


                Member myMember = MemberBLL.GetbyMembershipId((Guid)activeMember.ProviderUserKey);

                byte[] PP = (byte[])ViewState["PPStream"];
                MemberBLL.UpdatePicture(myMember, PP);

                //Response.Redirect("~/Pages/Designer/EditProfile");
            }
            else
            {
                Response.Write("<script>alert('sorry,you must log in first to save your picture');</script>");
                //Response.Redirect("/Pages/Anonymous/Login.aspx");
            }
        }

        protected void btn_SaveInfo_Click(object sender, EventArgs e)
        {
            activeMember = Membership.GetUser();
            if (activeMember != null)
            {


                Member myMember = MemberBLL.GetbyMembershipId((Guid)activeMember.ProviderUserKey);
                string fullName = string.Format("{0}", txt_FName.Text);
                MemberBLL.UpdateInfo(myMember, fullName, txt_Jop.Text, txt_Company.Text, txt_Phone.Text, txt_Address.Text);
                //Response.Redirect("/Pages/Designer/EditProfile");
            }
            else
            {
           
                Response.Write("<script>alert('sorry,you must log in first to save your info');</script>");
                //Response.Redirect("/Pages/Anonymous/Login.aspx");
            }
        }

        protected void btn_Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/Designer/Profile.aspx");
        }
        public string LoggedIn()
        {
            activeMember = Membership.GetUser();
            if (activeMember == null)
            {
                return "visible";
            }
            return "hidden";
        }
    }
}