using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Hangout.Web.Model
{
    public class FoursquareData : TableEntity
    {

        public FoursquareData()
        {
        }

        public FoursquareData(long foursquareID)
        {
            this.PartitionKey = FoursquareData.GetPartitionKey(foursquareID);
            this.RowKey = foursquareID.ToString();
            FoursquareID = foursquareID;
        }

        public Guid UserID { get; set; }
        public long FoursquareID { get; set; }
        public string AccessToken { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhotoPrefix { get; set; }
        public string PhotoSuffix { get; set; }
        public bool Gender { get; set; }
        public string Homecity { get; set; }
        public string Bio { get; set; }
        public string Email { get; set; }
        public string Twitter { get; set; }
        public string Facebook { get; set; }
        public string Phone { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public DateTime DateTimeUpdated { get; set; }

        public static string GetPartitionKey(long fourSquareID)
        {
            return (fourSquareID % 10000).ToString();
        }


    }


}