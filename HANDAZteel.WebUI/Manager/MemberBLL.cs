using HANDAZ.PEB.WebUI.EFW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace HANDAZ.PEB.WebUI.Manager
{
    class MemberBLL
    {

        public static int Add(MembershipUser validUser, string fullName, /*byte[] image,*/ int cityId, string zip, string gender, DateTime bdate, int depId, int roleId)
        {

            Member newMember = new Member();
            newMember.FullName = fullName;
            newMember.Image = null;//edit
            newMember.Zip = zip;
            newMember.Gender = gender;
            newMember.BirthDate = bdate;
            //newMember.RoleID = roleId;
            newMember.IsDeleted = false;
            newMember.ASPuserID = (Guid)validUser.ProviderUserKey;
            //Roles.AddUserToRole(validUser.UserName, "");

            HANDZ_PEB_DBEntities newone = new HANDZ_PEB_DBEntities();
            newone.Members.Add(newMember);
            return newone.SaveChanges();
        }

        //constructor 3al sa5an mo2ktn
        public static int Add(MembershipUser validUser)
        {
            Member newMember = new Member();

            newMember.FullName = "";
            /* newMember.Image = System.IO.File.ReadAllBytes("~/layout/styles/images/TempPicture.jpg");*///edit
                                                                                                         //newMember.CityID = 1;
            newMember.Zip = null;
            newMember.Gender = "";
            //newMember.BirthDate = new DateTime(1990,1,1);
            //newMember.RoleID = ;
            newMember.ASPuserID = (Guid)validUser.ProviderUserKey;

            HANDZ_PEB_DBEntities newone = new HANDZ_PEB_DBEntities();
            newone.Members.Add(newMember);
            return newone.SaveChanges();
        }

        public static List<Member> GetAll()
        {
            HANDZ_PEB_DBEntities all = new HANDZ_PEB_DBEntities();
            return all.Members.ToList();
        }
        public static Member GetbyMembershipId(Guid userID)
        {
            return MemberBLL.GetAll().FirstOrDefault(m => m.ASPuserID == userID);
        }

        public static int UpdateInfo(Member _member, string fullName, string jop,string company,string phone,string address)
        {
            HANDZ_PEB_DBEntities all = new HANDZ_PEB_DBEntities();
            Member member = all.Members.FirstOrDefault(memberDB => memberDB.Id == _member.Id);
            if (member != null)
            {
                member.FullName = fullName;
                member.Jop = jop;
                member.Company = company;
                member.Address = address;
                member.Phone = phone;
            }
            return all.SaveChanges();
        }
        public static int UpdatePicture(Member _member, byte[] image)
        {
            HANDZ_PEB_DBEntities all = new HANDZ_PEB_DBEntities();
            Member member = all.Members.FirstOrDefault(memberDB => memberDB.Id == _member.Id);
            if (member != null)
            {
                member.Image = image;

            }
            return all.SaveChanges();
        }
        public static string GetImageURL(Member member)
        {
            return "data:image/jpeg;base64," + Convert.ToBase64String(member.Image);
        }
    }
}
