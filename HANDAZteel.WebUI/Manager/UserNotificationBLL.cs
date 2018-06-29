using HANDAZ.PEB.WebUI.EFW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HANDAZ.PEB.WebUI.Manager
{
    public class UserNotificationBLL
    {
        public static List<UserNotification> GetAll(Member member)
        {
            HANDZ_PEB_DBEntities all = new HANDZ_PEB_DBEntities();
            List<UserNotification> notifications = null;
            foreach (UserNotification item in all.UserNotifications)
            {
                if (item.MemberID == member.Id)
                {
                    notifications.Add(item);
                }
            }
            return notifications;
        }

        public static int Add(Member member, string content)
        {
            UserNotification notification = new UserNotification();
            HANDZ_PEB_DBEntities all = new HANDZ_PEB_DBEntities();
            all.UserNotifications.Add(notification);

            notification.MemberID = member.Id;
            notification.Message = content;
            notification.SentTime = DateTime.Now;

            return all.SaveChanges();
        }
    }
}