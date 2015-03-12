using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace Hangout.Web.Model
{
    public class TwitterRelationships : TableEntity
    {
        public TwitterRelationships()
        {
        }

        public TwitterRelationships(long TwitterUser1, long TwitterUser2)
        {
            this.PartitionKey = TwitterRelationships.GetPartitionKey(TwitterUser1);
            this.RowKey = Guid.NewGuid().ToString();
            RelationshipID = new Guid(this.RowKey);
            TwitterUser1ID = TwitterUser1;
            TwitterUser2ID = TwitterUser2;
        }

        public static string GetPartitionKey(long TwitterUser1ID)
        {
            return TwitterUser1ID.ToString();
        }

        public Guid RelationshipID { get; set; }
        public long TwitterUser2ID { get; set; }
        public long TwitterUser1ID { get; set; }
        public DateTime DateTimeStamp { get; set; }

    }


}