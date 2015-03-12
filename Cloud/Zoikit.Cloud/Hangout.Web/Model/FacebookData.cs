using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;

namespace Hangout.Web.Model
{
    public class FacebookData : TableEntity
    {

        public FacebookData()
        { }

        public FacebookData(long facebookID)
        {
            this.PartitionKey = FacebookData.GetPartitionKey(facebookID);
            this.RowKey = facebookID.ToString();
            FacebookID = facebookID;
        }

        public Guid UserID { get; set; }

        public string LargeProfilePicURL { get; set; }
        public string AccessToken { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime  Birthday { get; set; }
        public string ProfilePicURL { get; set; }
        public bool Gender { get; set; }
        public int Age { get; set; }
        public float TimeZone { get; set; }

        public string Email { get; set; }
        public string Link { get; set; }
        public string RelationshipStatus { get; set; }
        public string About { get; set; }
        public string Phone { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public DateTime DateTimeUpdated { get; set; }







        public long FacebookID { get; set; }



        public static string GetPartitionKey(long facebookID)
        {
            return (facebookID % 10000).ToString();
        }

        internal static string GetRowKey(long facebookId)
        {
            return facebookId.ToString();
        }

        
    }


}