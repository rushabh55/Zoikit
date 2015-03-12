using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace Hangout.Web.Model
{
    public class UserFollow : TableEntity
    {
        public UserFollow()
        {
        }

        public UserFollow(Guid userId, Guid followuserId)
        {
            this.PartitionKey = followuserId.ToString();
            this.RowKey = userId.ToString();
            UserFollowID = Guid.NewGuid();
            DateTimeStamp = DateTime.Now;
            UserID=userId;
            FollowuserId = followuserId; 


        }


        public static string GetPartitionKey(Guid followuserId)
        {
            return followuserId.ToString();

        }


        public Guid FollowuserId { get; set; }

        public Guid UserID { get; set; }

        public DateTime DateTimeStamp { get; set; }

        public Guid UserFollowID { get; set; }
    }


}