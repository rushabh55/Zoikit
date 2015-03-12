using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace Hangout.Web.Model
{
    public class UserPlaceFollow : TableEntity
    {
        public UserPlaceFollow()
        {

        }

        public UserPlaceFollow(Guid userId, Guid placeId)
        {
            this.PartitionKey = placeId.ToString();
            this.RowKey = Guid.NewGuid().ToString();
            UserPlaceFollowID = new Guid(this.RowKey);
            DateTimeStamp = DateTime.Now;
            UserID=userId;
            PlaceID = placeId;


        }


      

        public Guid UserID { get; set; }

        public DateTime DateTimeStamp { get; set; }

      

        public Guid UserPlaceFollowID { get; set; }

        public Guid PlaceID { get; set; }

        internal static string GetPartitionKey(Guid placeId)
        {
            return placeId.ToString();
        }
    }


}