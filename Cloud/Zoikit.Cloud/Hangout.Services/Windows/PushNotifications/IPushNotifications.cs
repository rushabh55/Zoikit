using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Hangout.Services.Windows.PushNotifications
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPushNotifications" in both code and config file together.
    [ServiceContract]
    public interface IPushNotifications
    {
        [OperationContract]

        void Subscribe(Guid userId, string channelUri, string zat);
        [OperationContract]
        void Unsubscribe(Guid userId, string zat);

        [OperationContract]
        Hangout.Web.Model.PushNotification GetPushNotification(string zat);
        [OperationContract]
        bool Exists(Guid userId, string zat);
    }
}
