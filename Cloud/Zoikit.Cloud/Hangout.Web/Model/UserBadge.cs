using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace Hangout.Web.Model
{
    public class UserBadge : TableEntity
    {
        public UserBadge()
        {
        }

        public UserBadge(Guid userId,Guid badgeId)
        {
            this.PartitionKey = userId.ToString();
            this.RowKey = Guid.NewGuid().ToString();
            UserBadgeID = new Guid(this.RowKey);
            UserID=userId;
            BadgeID = badgeId;
        }

       
        public DateTime DateTimeStamp { get; set; }

        public string Description { get; set; }


        public Guid UserBadgeID { get; set; }

        public Guid UserID { get; set; }

        public Guid BadgeID { get; set; }

        public static string GetPartitionKey(Guid userId)
        {
            return userId.ToString();
        }
    }


}