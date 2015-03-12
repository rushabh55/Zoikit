using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace Hangout.Web.Model
{
    public class TwitterData : TableEntity
    {

        public TwitterData()
        {

        }

        public TwitterData(long twitterID)
        {
            this.PartitionKey = Web.Model.TwitterData.GetPartitionKey(twitterID); //smart ;)
            this.RowKey = TwitterID.ToString();
            TwitterID = twitterID;
        }

        public static string GetPartitionKey(long TwitterID)
        {
            return  (TwitterID % 10000).ToString();
        }

        public static string GetRowKey(long TwitterID)
        {
            return TwitterID.ToString();
        }

        public Guid UserID { get; set; }
        

        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }
        public string ScreenName { get; set; }
        public string Language { get; set; }
        public string ProfileImageURL { get; set; }
        public string Location { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
        public string TimeZone { get; set; }

        public string Email { get; set; }
        public string URL { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        
        public DateTime DateTimeStamp { get; set; }
        public DateTime DateTimeUpdated { get; set; }

        public long TwitterID { get; set; }



    }


}