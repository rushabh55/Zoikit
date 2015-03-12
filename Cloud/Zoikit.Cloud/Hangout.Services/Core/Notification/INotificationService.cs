using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Hangout.Services.Core.Notification
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "INotificationService" in both code and config file together.
    [ServiceContract]
    public interface INotificationService
    {

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "MarkAllAsRead?UserID={userId}&ZAT={zat}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        [OperationContract]
        Web.Services.Objects.Service.Result MarkAllAsRead(Guid userId,string zat);
        
        
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetNotifications?UserID={userId}&ZAT={zat}&Results={take}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<Web.Services.Objects.Notification.Notification> GetNotifications(Guid userId, int take, List<Guid> skip, string zat);
        
        
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetNotification?NotificationID={notificationId}&UserID={userId}&ZAT={zat}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        Web.Services.Objects.Notification.Notification GetNotification(Guid notificationId, Guid userId,  string zat);
        
        
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetLastUnreadNotification?UserID={userId}&ZAT={zat}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        Web.Services.Objects.Notification.NotificationData GetLastUnReadNotification(Guid userId, string zat);
        
        
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetUnreadNotificationCount?UserID={userId}&ZAT={zat}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        int GetUnreadNotificationCount(Guid userId, string zat);
        
        
       
    }
}
