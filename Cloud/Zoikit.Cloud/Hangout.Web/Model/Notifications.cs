using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;

namespace Hangout.Web.Model
{
    public class Notifications : TableEntity
    {
        public Notifications()
        {
        }

        public Notifications(Guid userId,string title,string description)
        {
            this.PartitionKey = userId.ToString();
            UserID=userId;
            this.RowKey = (DateTime.MaxValue.Ticks - DateTime.Now.Ticks).ToString();
            NotificationID = Guid.NewGuid();
            Title = title;
            Description = description;
            MarkAsRead = false;
            DateTimeStamp = DateTime.Now;

        }

        public bool MarkAsRead { get; set; }
        public string Param1 { get; set; }
        public string Type1 { get; set; }
        public string Param2 { get; set; }
        public string Type2 { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public string MetaData { get; set; }
        public string ProfilePicURL { get; set; }
        public Guid UserID { get; set; }
        public Guid NotificationID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        internal static string GetPartitionKey(Guid userId)
        {
            return userId.ToString();
        }
    }


}