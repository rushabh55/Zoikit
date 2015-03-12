using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.Notification
{
    public class Notification
    {
        

        public Web.Services.Objects.Service.Result MarkAllAsRead(Guid userId, string accesstoken)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId, accesstoken))
                {
                    Web.Core.Notifications.Notification.MarkAllNotificationsAsRead(userId);
                    return Objects.Service.Result.OK;
                }
                else
                {
                    throw new Web.Core.Exceptions.Account.UnAuthorisedAccess();
                }
            }
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return Objects.Service.Result.Error;
            }
        }


        public List<Services.Objects.Notification.Notification> GetNotifications(Guid userId, int take, List<Guid> skipList, string accesstoken)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId, accesstoken))
                {
                    return Convert(Web.Core.Notifications.Notification.GetNotifications(userId, take, skipList));
                }
                else
                {
                    throw new Web.Core.Exceptions.Account.UnAuthorisedAccess();
                }
            }
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return null;
            }
        }

        private List<Objects.Notification.Notification> Convert(List<Model.Notifications> list)
        {
            List<Objects.Notification.Notification> l = new List<Objects.Notification.Notification>();

            foreach(Model.Notifications x in list)
            {
                l.Add(Convert(x));
            }

            return l;
        }

        public Objects.Notification.Notification GetNotification(Guid notificationId, Guid userId, string accesstoken)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId,accesstoken))
                {
                    return Convert(Web.Core.Notifications.Notification.GetNotification(notificationId));
                }
                else
                {
                    throw new Web.Core.Exceptions.Account.UnAuthorisedAccess();
                }
            }
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(Guid.Empty, Web.Core.ClientType.WindowsAzure, ex);
                return null;
            }

        }

        private Objects.Notification.Notification Convert(Model.Notifications notification)
        {

            if (notification != null)
            {
                Objects.Notification.Notification x = new Objects.Notification.Notification();
                x.NotificationID = notification.NotificationID;
                x.MarkAsRead = (bool)notification.MarkAsRead;
                x.DatetimePosted = notification.DateTimeStamp;
                x.Description = notification.Description;
                x.Param1 = notification.Param1;
                x.Param2 = notification.Param2;
                x.ProfilePicURL = notification.ProfilePicURL;
                x.Title = notification.Title;
                x.Type1 = notification.Type1;
                x.Type2 = notification.Type2;
                x.UserID = notification.UserID;
                Services.Users.User user = new Users.User();
                x.UserList = new List<Objects.Accounts.CompactUser>();
                foreach (Model.User u in Core.Notifications.Notification.GetNotificationUsers(notification.NotificationID))
                {
                    x.UserList.Add(user.Convert(Core.Accounts.User.GetUserData(u.UserID)));
                }

                return x;
            }

            return null;


        }

        public Objects.Notification.NotificationData GetLastUnreadNotifications(Guid userId, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId,zat))
                {
                    Objects.Notification.Notification notification= Convert(Web.Core.Notifications.Notification.GetLastUnreadNotification(userId));
                    if (notification == null)
                    {
                        return new Objects.Notification.NotificationData { Notification = null, Count = 0 };
                    }
                    else
                    {
                        return new Objects.Notification.NotificationData { Notification = notification, Count = Core.Notifications.Notification.GetUnreadNotificationCount(userId) };
                    }
                }
                else
                {
                    throw new Web.Core.Exceptions.Account.UnAuthorisedAccess();
                }
            }
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(Guid.Empty, Web.Core.ClientType.WindowsAzure, ex);
                return null;
            }
        }


        public int GetUnreadNotificationCount(Guid userId, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId, zat))
                {
                    return Core.Notifications.Notification.GetUnreadNotificationCount(userId);
                }
                else
                {
                    throw new Web.Core.Exceptions.Account.UnAuthorisedAccess();
                }
            }
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(Guid.Empty, Web.Core.ClientType.WindowsAzure, ex);
                return 0;
            }
        }

        public void ClearTile(Guid userId, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId, zat))
                {
                   Web.Core.PushNotifications.PushNotifications.ClearWindows8tile(userId);
                }
                else
                {
                    throw new Web.Core.Exceptions.Account.UnAuthorisedAccess();
                }
            }
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(Guid.Empty, Web.Core.ClientType.WindowsAzure, ex);
                
            }
        }

        public List<Objects.Notification.Notification> GetNewNotifications(Guid userId, List<Guid> skipList, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId,zat))
                {
                    return Convert(Web.Core.Notifications.Notification.GetNewNotifications(userId,skipList));
                }
                else
                {
                    throw new Web.Core.Exceptions.Account.UnAuthorisedAccess();
                }
            }
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return null;
            }
        }
    }
}