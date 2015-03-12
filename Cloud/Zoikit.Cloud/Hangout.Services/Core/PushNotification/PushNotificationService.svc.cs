using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Hangout.Services.Core.PushNotification
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PushNotification" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PushNotification.svc or PushNotification.svc.cs at the Solution Explorer and start debugging.
    public class PushNotification : IPushNotificationService
    {

        Web.Services.PushNotification.PushNotification obj = new Web.Services.PushNotification.PushNotification();


        
        public void Subscribe(Guid userId,string channelUri, string accesstoken)
        {
            obj.Subscribe(userId, new Uri(channelUri), accesstoken);
        }

        public void Unsubscribe(Guid userId, string accesstoken)
        {
            obj.Unsubscribe(userId, accesstoken);
        }

       
        public Web.Model.PushNotification GetPushNotification(string accesstoken)
        {
            Guid userId = Guid.Empty;
            return obj.GetPushNotification(userId, accesstoken);
        }

        public bool Exists(Guid userId, string accesstoken)
        {
            return obj.Exists(userId, accesstoken);
        }


        
    }
}
