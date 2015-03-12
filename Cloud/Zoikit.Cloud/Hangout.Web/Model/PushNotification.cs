using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;

namespace Hangout.Web.Model
{
    public class PushNotification : TableEntity
    {
        public PushNotification()
        {
        }

        public PushNotification(Guid userId,string uri)
        {
            //get user location. 
            this.PartitionKey = Core.Location.UserLocation.GetLastLocation(userId).CityID.ToString();
            PushNotificationID = Guid.NewGuid().ToString();
            this.RowKey = userId.ToString();
            URI = uri;
            UserID=userId;
        }


        public static string GetPartitionKey(Guid userId)
        {
            return userId.ToString();
        }

        public Guid UserID { get; set; }
        public string URI { get; set; }

        public string PushNotificationID { get; set; }
    }


}