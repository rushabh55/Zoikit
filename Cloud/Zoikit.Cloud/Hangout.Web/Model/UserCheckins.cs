using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace Hangout.Web.Model
{
    public class UserCheckins : TableEntity
    {
        public UserCheckins()
        {

        }

        public UserCheckins(Guid userId,Guid placeId)
        {
            this.PartitionKey = placeId.ToString();
            this.RowKey = (DateTime.MaxValue.Ticks - DateTime.Now.Ticks).ToString();
            UserCheckInID = Guid.NewGuid();
            PlaceID = placeId;
            UserID=userId;
        }

        public bool CheckedIn { get; set; }
        public Guid PlaceID { get; set; }
        public Guid UserID { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public string FoursquareCheckinID { get; set; }
        public Guid UserCheckInID { get; set; }

        internal static string GetPartitionKey(Guid placeId)
        {
            return placeId.ToString();
        }
    }


}