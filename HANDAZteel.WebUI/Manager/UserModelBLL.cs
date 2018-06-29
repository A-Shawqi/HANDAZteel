using HANDAZ.PEB.WebUI.EFW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HANDAZ.PEB.WebUI.Manager
{
    public class UserModelBLL
    {
        public static List<UserModel> GetAll(Member member)
        {
            HANDZ_PEB_DBEntities all = new HANDZ_PEB_DBEntities();
            List<UserModel> models = null;
            foreach (UserModel item in all.UserModels)
            {
                if (item.MemberID == member.Id)
                {
                    models.Add(item);
                }
            }
            return models;
        }

        public static int Add(Member member,string name,string path)
        {
            UserModel model = new UserModel();
            HANDZ_PEB_DBEntities all = new HANDZ_PEB_DBEntities();
            all.UserModels.Add(model);

            model.MemberID = member.Id;
            model.Name =name;
            model.Path =path;

            return all.SaveChanges();
        }
    }
}