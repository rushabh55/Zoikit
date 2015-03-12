using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Hangout.Services.Windows.Notification
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "NotificationService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select NotificationService.svc or NotificationService.svc.cs at the Solution Explorer and start debugging.
    public class NotificationService : INotificationService
    {
        Web.Services.Notification.Notification obj = new Web.Services.Notification.Notification();
        public Web.Services.Objects.Service.Result MarkAllAsRead(Guid userId, string zat)
        {
            return obj.MarkAllAsRead(userId, zat);
        }


        public List<Web.Services.Objects.Notification.Notification> GetNotifications(Guid userId, int take, List<Guid> skip, string zat)
        {
            return obj.GetNotifications(userId, take, skip, zat);
        }

        public Web.Services.Objects.Notification.Notification GetNotification(Guid notificationId, Guid userId, string zat)
        {
            return obj.GetNotification(notificationId, userId, zat);

        }

        public Hangout.Web.Services.Objects.Notification.NotificationData GetLastUnReadNotification(Guid userId, string zat)
        {
            return obj.GetLastUnreadNotifications(userId, zat);
        }

        public int GetUnreadNotificationCount(Guid userId, string zat)
        {
            return obj.GetUnreadNotificationCount(userId, zat);
        }


        public List<Web.Services.Objects.Notification.Notification> GetNewNotifications(Guid userId, List<Guid> skipList, string zat)
        {
            return obj.GetNewNotifications(userId, skipList, zat);
        }
    }
}
