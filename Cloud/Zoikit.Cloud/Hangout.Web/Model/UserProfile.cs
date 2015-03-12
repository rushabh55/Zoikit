using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;

namespace Hangout.Web.Model
{
    public class UserProfile : TableEntity 
    {
        public UserProfile()
        {

        }

        public UserProfile(Guid userId)
        {
            this.PartitionKey = GetPartitionKey(userId);
            this.RowKey = userId.ToString();
            UserID = new Guid(this.RowKey);
            DateTimeStamp = DateTime.Now;
            
           
        }

        public static string GetPartitionKey(Guid userId)
        {
            return userId.ToString().Substring(0, 2); 
        }


      

        public Guid UserID { get; set; }

        public DateTime DateTimeStamp { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string ProfilePicURL { get; set; }
        public bool Gender { get; set; }
        public float TimeZone { get; set; }
        public DateTime DateTimeUpdated { get; set; }
        public string RelationshipStatus { get; set; }
        public string Bio { get; set; }
        public string Phone { get; set; }
        public string LargeProfilePicURL { get; set; }
        public string DefaultLengthUnits { get; set; }


        public int FollowersCount { get; set; }

        public int FollowingCount { get; set; }

        public int BuzzCount { get; set; }
        public int Age { get; set; }
    }


}