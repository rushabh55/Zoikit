using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;

namespace Hangout.Web.Model
{
    public class FoursquareRelationships : TableEntity
    {
        public FoursquareRelationships()
        {
        }

        public FoursquareRelationships(long FoursquareUser1,long FoursquareUser2)
        {
            this.PartitionKey = FoursquareUser1.ToString();
            this.RowKey = Guid.NewGuid().ToString();
            RelationshipID = new Guid(this.RowKey);
            FoursquareUser1ID = FoursquareUser1;
            FoursquareUser2ID = FoursquareUser2;
        }

        public Guid RelationshipID { get; set; }
        public long FoursquareUser2ID { get; set; }
        public long FoursquareUser1ID { get; set; }
        public DateTime DateTimeStamp { get; set; }

    }


}