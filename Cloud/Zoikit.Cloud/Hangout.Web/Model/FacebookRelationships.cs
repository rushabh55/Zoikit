using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;

namespace Hangout.Web.Model
{
    public class FacebookRelationships : TableEntity
    {

        public FacebookRelationships()
        {

        }

        public FacebookRelationships(long FacebookUser1,long FacebookUser2)
        {
            this.PartitionKey = FacebookUser1.ToString();
            this.RowKey = Guid.NewGuid().ToString();
            RelationshipID = new Guid(this.RowKey);
            FacebookUser1ID = FacebookUser1;
            FacebookUser2ID = FacebookUser2;
        }

        public Guid RelationshipID { get; set; }
        public long FacebookUser2ID { get; set; }
        public long FacebookUser1ID { get; set; }
        public DateTime DateTimeStamp { get; set; }

    }


}