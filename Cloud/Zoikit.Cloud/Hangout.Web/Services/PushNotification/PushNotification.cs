using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.PushNotification
{
    public class PushNotification
    {
        
        public void Subscribe(Guid userId, Uri channelUri, string accesstoken)
        {
            try
            {
                if (channelUri == null || userId == Guid.Empty || userId == null || accesstoken == "")
                {
                    return;
                }
                if (Web.Core.Accounts.User.IsValid(userId, accesstoken))
                {
                    Web.Core.PushNotifications.PushNotifications.Subscribe(userId, channelUri);
                }
                else { throw new Web.Core.Exceptions.Account.UnAuthorisedAccess(); }
            }
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);

            }
        }

        public void Unsubscribe(Guid userId, string accesstoken)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId, accesstoken))
                {
                    Web.Core.PushNotifications.PushNotifications.Unsubscribe(userId);
                }
                else { throw new Web.Core.Exceptions.Account.UnAuthorisedAccess(); }
            }
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);

            }
        }

        public Web.Model.PushNotification GetPushNotification(Guid userId, string accesstoken)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId, accesstoken))
                {
                    return Web.Core.PushNotifications.PushNotifications.GetPushNotification(userId);
                }
                else { throw new Web.Core.Exceptions.Account.UnAuthorisedAccess(); }

            }
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return null;
            }
        }

        public bool Exists(Guid userId, string accesstoken)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId, accesstoken))
                {
                    return Web.Core.PushNotifications.PushNotifications.Exists(userId);
                }
                else { throw new Web.Core.Exceptions.Account.UnAuthorisedAccess(); }
            }
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return false;
            }
        }
    }
}