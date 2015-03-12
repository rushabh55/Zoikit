using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Model
{
    public static class Table
    {
        public static Microsoft.WindowsAzure.Storage.Table.CloudTable UserFollow;

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable ActivityLog { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable Avatars { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable Badge { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable UserBuzzInteract { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable UserReview { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable Users { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable BuzzByID { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable LocalTagFollowCount { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable BuzzByUser { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable AmplifyBuzzByUser { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable BuzzFollowByUser { get; set; }
        public static Microsoft.WindowsAzure.Storage.Table.CloudTable BuzzByTag { get; set; }


        public static Microsoft.WindowsAzure.Storage.Table.CloudTable UserScore { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable UserTagFollow { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable UserProfile { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable UserPlaceFollow { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable UserMeet { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable UserLocations { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable UserCheckins { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable UserCategoryFollows { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable UserBadge { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable AmplifyBuzz { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable TwitterRelationships { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable TwitterData { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable Text { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable TagInCategory { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable TagInBuzz { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable Tag { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable Role { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable PushNotifications { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable PlaceTag { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable NotificationsUser { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable Notifications { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable Locations { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable FoursquareRelationships { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable FoursquareData { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable FacebookRelationships { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable FacebookData { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable Exceptions { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable Country { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable Compatibility { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable City { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable Category { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable BuzzInCategory { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable BuzzFollow { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable Places { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable Apps { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable BuzzComment { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable Buzz { get; set; }

        

        public static Microsoft.WindowsAzure.Storage.CloudStorageAccount CloudStorageAccount { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTableClient TableClient { get; set; }

        public static Microsoft.WindowsAzure.Storage.Table.CloudTable UserBackgroundData { get; set; }
        
    }
}