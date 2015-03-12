using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;

namespace Hangout.Web.Model
{
    public class NotificationUser : TableEntity
    {

        public NotificationUser()
        {

        }

        public NotificationUser(Guid notificationID,Guid userId)
        {
            this.PartitionKey = notificationID.ToString();
            this.RowKey = Guid.NewGuid().ToString();
            NotificationuserId = new Guid(this.RowKey);
            NotificationID = notificationID;
            UserID=userId;
        }

        public Guid NotificationuserId { get; set; }

        public Guid UserID { get; set; }

        public Guid NotificationID { get; set; }

        public static string GetPartitionKey(Guid NotifcationId)
        {
            return NotifcationId.ToString();
        }
    }


}