using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Hangout.Services.Windows.Notification
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "INotificationService" in both code and config file together.
    [ServiceContract]
    public interface INotificationService
    {
       
        [OperationContract]
        Web.Services.Objects.Service.Result MarkAllAsRead(Guid userId, string zat);


        [OperationContract]
        List<Web.Services.Objects.Notification.Notification> GetNotifications(Guid userId, int take, List<Guid> skip, string zat);

        [OperationContract]
        List<Web.Services.Objects.Notification.Notification> GetNewNotifications(Guid userId, List<Guid> skipList, string zat);


        [OperationContract]
        Web.Services.Objects.Notification.Notification GetNotification(Guid notificationId, Guid userId, string zat);


        [OperationContract]
        Web.Services.Objects.Notification.NotificationData GetLastUnReadNotification(Guid userId, string zat);


        [OperationContract]
        int GetUnreadNotificationCount(Guid userId, string zat);
    }
}
