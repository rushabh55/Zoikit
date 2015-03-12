using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Core.Cloud
{
    public class TableStorage
    {

        public static void CreateTablesIfNotExists()
        {
            Core.Cloud.TableStorage.InitializeStorage();

            Model.Table.ActivityLog = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.ActivityLog);
            Model.Table.ActivityLog.CreateIfNotExists();

            Model.Table.AmplifyBuzz = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.AmplifyBuzz);
            Model.Table.AmplifyBuzz.CreateIfNotExists();

            Model.Table.AmplifyBuzzByUser = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.AmplifyBuzzByUser);
            Model.Table.AmplifyBuzzByUser.CreateIfNotExists();

            Model.Table.Apps = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.Apps);
            Model.Table.Apps.CreateIfNotExists();

            Model.Table.UserBuzzInteract = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.UserBuzzInteract);
            Model.Table.UserBuzzInteract.CreateIfNotExists();

            Model.Table.LocalTagFollowCount = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.LocalTagFollowCount);
            Model.Table.LocalTagFollowCount.CreateIfNotExists();

            Model.Table.Avatars = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.Avatars);
            Model.Table.Avatars.CreateIfNotExists();


            Model.Table.Badge = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.Badge);
            Model.Table.Badge.CreateIfNotExists();

            Model.Table.Buzz = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.Buzz);
            Model.Table.Buzz.CreateIfNotExists();

            Model.Table.BuzzByID = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.BuzzByID);
            Model.Table.BuzzByID.CreateIfNotExists();


            Model.Table.BuzzByTag = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.BuzzByTag);
            Model.Table.BuzzByTag.CreateIfNotExists();

            Model.Table.BuzzByUser = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.BuzzByUser);
            Model.Table.BuzzByUser.CreateIfNotExists();

            Model.Table.BuzzComment = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.BuzzComment);
            Model.Table.BuzzComment.CreateIfNotExists();


            Model.Table.BuzzFollow = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.BuzzFollow);
            Model.Table.BuzzFollow.CreateIfNotExists();


            Model.Table.BuzzInCategory = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.BuzzInCategory);
            Model.Table.BuzzInCategory.CreateIfNotExists();


            Model.Table.Category = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.Category);
            Model.Table.Category.CreateIfNotExists();

            Model.Table.City = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.City);
            Model.Table.City.CreateIfNotExists();


            Model.Table.Compatibility = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.Compatibility);
            Model.Table.Compatibility.CreateIfNotExists();


            Model.Table.Country = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.Country);
            Model.Table.Country.CreateIfNotExists();


            Model.Table.Exceptions = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.Exceptions);
            Model.Table.Exceptions.CreateIfNotExists();


            Model.Table.FacebookData = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.FacebookData);
            Model.Table.FacebookData.CreateIfNotExists();


            Model.Table.FacebookRelationships = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.FacebookRelationships);
            Model.Table.FacebookRelationships.CreateIfNotExists();



            Model.Table.FoursquareData = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.FoursquareData);
            Model.Table.FoursquareData.CreateIfNotExists();

            Model.Table.FoursquareRelationships = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.FoursquareRelationships);
            Model.Table.FoursquareRelationships.CreateIfNotExists();


            Model.Table.LocalTagFollowCount = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.LocalTagFollowCount);
            Model.Table.LocalTagFollowCount.CreateIfNotExists();


            Model.Table.Locations = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.Location);
            Model.Table.Locations.CreateIfNotExists();


            Model.Table.Notifications = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.Notifications);
            Model.Table.Notifications.CreateIfNotExists();



            Model.Table.NotificationsUser = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.NotificationUser);
            Model.Table.NotificationsUser.CreateIfNotExists();



            Model.Table.Places = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.Places);
            Model.Table.Places.CreateIfNotExists();


            Model.Table.PlaceTag = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.PlaceTag);
            Model.Table.PlaceTag.CreateIfNotExists();

            Model.Table.PushNotifications = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.PushNotifications);
            Model.Table.PushNotifications.CreateIfNotExists();

            Model.Table.Role = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.Role);
            Model.Table.Role.CreateIfNotExists();

            Model.Table.Tag = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.Tag);
            Model.Table.Tag.CreateIfNotExists();


            Model.Table.TagInBuzz = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.TagInBuzz);
            Model.Table.TagInBuzz.CreateIfNotExists();


            Model.Table.Text = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.Text);
            Model.Table.Text.CreateIfNotExists();


            Model.Table.TwitterData = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.TwitterData);
            Model.Table.TwitterData.CreateIfNotExists();


            Model.Table.TwitterRelationships = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.TwitterRelationships);
            Model.Table.TwitterRelationships.CreateIfNotExists();

            Model.Table.UserBadge = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.UserBadge);
            Model.Table.UserBadge.CreateIfNotExists();

            Model.Table.UserBuzzInteract = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.UserBuzzInteract);
            Model.Table.UserBuzzInteract.CreateIfNotExists();


            Model.Table.UserCategoryFollows = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.UserCategoryFollows);
            Model.Table.UserCategoryFollows.CreateIfNotExists();

            Model.Table.UserCheckins = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.UserCheckins);
            Model.Table.UserCheckins.CreateIfNotExists();

            Model.Table.UserFollow = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.UserFollow);
            Model.Table.UserFollow.CreateIfNotExists();


            Model.Table.UserLocations = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.UserLocations);
            Model.Table.UserLocations.CreateIfNotExists();


            Model.Table.UserMeet = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.UserMeet);
            Model.Table.UserMeet.CreateIfNotExists();


            Model.Table.UserPlaceFollow = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.UserPlaceFollow);
            Model.Table.UserPlaceFollow.CreateIfNotExists();



            Model.Table.UserProfile = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.UserProfile);
            Model.Table.UserProfile.CreateIfNotExists();



            Model.Table.UserReview = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.UserReview);
            Model.Table.UserReview.CreateIfNotExists();


            Model.Table.Users = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.Users);
            Model.Table.Users.CreateIfNotExists();




            Model.Table.UserScore = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.UserScore);
            Model.Table.UserScore.CreateIfNotExists();
            Model.Table.UserTagFollow = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.UserTagFollow);
            Model.Table.UserTagFollow.CreateIfNotExists();

        }
        public static void InitializeStorage()
        {

            if (Model.Table.CloudStorageAccount == null)
            {
                Model.Table.CloudStorageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(
                Settings.CloudStorageString);
            }

            // Create the table client.

            if (Model.Table.TableClient == null)
            {
                Model.Table.TableClient = Model.Table.CloudStorageAccount.CreateCloudTableClient();
            }

        }

        public static void InitializeAppTable()
        {
            InitializeStorage();
            if (Model.Table.Apps == null)
            {
                Model.Table.Apps = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.Apps);
            }
        }

        public static void InitializeAmplifyBuzzTable()
        {
            InitializeStorage();
            if (Model.Table.AmplifyBuzz == null)
            {
                Model.Table.AmplifyBuzz = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.AmplifyBuzz);
            }
        }

        public static void InitializeLocalTagFollowCount()
        {
            InitializeStorage();
            if (Model.Table.LocalTagFollowCount == null)
            {
                Model.Table.LocalTagFollowCount = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.LocalTagFollowCount);
            }
        }


        public static void InitializeAmplifyBuzzByUserTable()
        {
            InitializeStorage();
            if (Model.Table.AmplifyBuzzByUser == null)
            {
                Model.Table.AmplifyBuzzByUser = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.AmplifyBuzzByUser);
            }
        }


        public static void InitializeBuzzByTag()
        {
            InitializeStorage();
            if (Model.Table.BuzzByTag == null)
            {
                Model.Table.BuzzByTag = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.BuzzByTag);
            }
        }


        public static void InitializeUserBuzzInteract()
        {
            InitializeStorage();
            if (Model.Table.UserBuzzInteract == null)
            {
                Model.Table.UserBuzzInteract = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.UserBuzzInteract);
            }
        }


        public static void InitializeBuzzFollowByUserTable()
        {
            InitializeStorage();
            if (Model.Table.BuzzFollowByUser == null)
            {
                Model.Table.BuzzFollowByUser = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.FollowBuzzByUser);
            }
        }

        public static void InitializeBuzzByUserTable()
        {
            InitializeStorage();
            if (Model.Table.BuzzByUser == null)
            {
                Model.Table.BuzzByUser = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.BuzzByUser);
            }
        }

        public static void InitializeBuzzByIDTable()
        {
            InitializeStorage();
            if (Model.Table.BuzzByID == null)
            {
                Model.Table.BuzzByID = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.BuzzByID);
            }
        }

        public static void InitializeActivityLogTable()
        {
            InitializeStorage();
            if (Model.Table.ActivityLog == null)
            {
                Model.Table.ActivityLog = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.ActivityLog);
            }
        }

        public static void InitializeUserTagFollowTable()
        {
            InitializeStorage();
            if (Model.Table.UserTagFollow == null)
            {
                Model.Table.UserTagFollow = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.UserTagFollow);
            }
        }


        public static void InitializeBuzzTable()
        {
            InitializeStorage();
            if (Model.Table.Buzz == null)
            {
                Model.Table.Buzz = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.Buzz);
            }
        }


        public static void InitializeBadgeTable()
        {
            InitializeStorage();
            if (Model.Table.Badge == null)
            {
                Model.Table.Badge = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.Badge);
            }
        }


        public static void InitializeAvatarsTable()
        {
            InitializeStorage();
            if (Model.Table.Avatars == null)
            {
                Model.Table.Avatars = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.Avatars);
            }
        }

        public static void InitializeBuzzCommentTable()
        {
            InitializeStorage();
            if (Model.Table.BuzzComment == null)
            {
                Model.Table.BuzzComment = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.BuzzComment);
            }
        }

        public static void InitializeBuzzFollowTable()
        {
            InitializeStorage();
            if (Model.Table.BuzzFollow == null)
            {
                Model.Table.BuzzFollow = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.BuzzFollow);
            }
        }

        public static void InitializeBuzzInCategoryTable()
        {
            InitializeStorage();
            if (Model.Table.BuzzInCategory == null)
            {
                Model.Table.BuzzInCategory = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.BuzzInCategory);
            }
        }

        public static void InitializeCategoryTable()
        {
            InitializeStorage();
            if (Model.Table.Category == null)
            {
                Model.Table.Category = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.Category);
            }
        }

        public static void InitializeCityTable()
        {
            InitializeStorage();
            if (Model.Table.City == null)
            {
                Model.Table.City = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.City);
            }
        }

        public static void InitializeCountryTable()
        {
            InitializeStorage();
            if (Model.Table.Country == null)
            {
                Model.Table.Country = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.Country);
            }
        }

        public static void InitializeCompatibilityTable()
        {
            InitializeStorage();
            if (Model.Table.Compatibility == null)
            {
                Model.Table.Compatibility = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.Compatibility);
            }
        }

        public static void InitializeUsersTable()
        {
            InitializeStorage();
            if (Model.Table.Users == null)
            {
                Model.Table.Users = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.Users);
            }
        }


        public static void InitializeRoleTable()
        {
            InitializeStorage();
            if (Model.Table.Role == null)
            {
                Model.Table.Role = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.Role);
            }
        }

        public static void InitializePlacesTable()
        {
            InitializeStorage();
            if (Model.Table.Places == null)
            {
                Model.Table.Places = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.Places);
            }
        }

        public static void InitializeTwitterDataTable()
        {
            InitializeStorage();
            if (Model.Table.TwitterData == null)
            {
                Model.Table.TwitterData = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.TwitterData);
            }
        }

        public static void InitializeFacebookDataTable()
        {
            InitializeStorage();
            if (Model.Table.FacebookData == null)
            {
                Model.Table.FacebookData = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.FacebookData);
            }
        }


        public static void InitializeFacebookRelationshipsTable()
        {
            InitializeStorage();
            if (Model.Table.FacebookRelationships == null)
            {
                Model.Table.FacebookRelationships = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.FacebookRelationships);
            }
        }


        public static void InitializeTwitterRelationshipsTable()
        {
            InitializeStorage();
            if (Model.Table.TwitterRelationships == null)
            {
                Model.Table.TwitterRelationships = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.TwitterRelationships);
            }
        }


        public static void InitializeUserProfileTable()
        {
            InitializeStorage();
            if (Model.Table.UserProfile == null)
            {
                Model.Table.UserProfile = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.UserProfile);
            }
        }

        public static void InitializeUserCategoryFollowsTable()
        {
            InitializeStorage();
            if (Model.Table.UserCategoryFollows == null)
            {
                Model.Table.UserCategoryFollows = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.UserCategoryFollows);
            }
        }

        public static void InitializeTagTable()
        {
            InitializeStorage();
            if (Model.Table.Tag == null)
            {
                Model.Table.Tag = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.Tag);
            }
        }

        
        public static void InitializeTagInBuzzTable()
        {
            InitializeStorage();
            if (Model.Table.TagInBuzz == null)
            {
                Model.Table.TagInBuzz = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.TagInBuzz);
            }
        }

        public static void InitializeTagInCategoryTable()
        {
            InitializeStorage();
            if (Model.Table.TagInCategory == null)
            {
                Model.Table.TagInCategory = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.TagInCategory);
            }
        }

        public static void InitializeUserBadgeTable()
        {
            InitializeStorage();
            if (Model.Table.UserBadge == null)
            {
                Model.Table.UserBadge = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.UserBadge);
            }
        }

        public static void InitializeExceptionsTable()
        {
            InitializeStorage();
            if (Model.Table.Exceptions == null)
            {
                Model.Table.Exceptions = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.Exceptions);
            }
        }

        public static void InitializeTextTable()
        {
            InitializeStorage();
            if (Model.Table.Text == null)
            {
                Model.Table.Text = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.Text);
            }
        }

        public static void InitializeNotificationsTable()
        {
            InitializeStorage();
            if (Model.Table.Notifications == null)
            {
                Model.Table.Notifications = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.Notifications);
            }
        }


        public static void InitializeNotificationsUserTable()
        {
            InitializeStorage();
            if (Model.Table.NotificationsUser == null)
            {
                Model.Table.NotificationsUser = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.NotificationUser);
            }
        }


        public static void InitializePlaceTagTable()
        {
            InitializeStorage();
            if (Model.Table.PlaceTag == null)
            {
                Model.Table.PlaceTag = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.PlaceTag);
            }
        }

        public static void InitializePushNotificationsTable()
        {
            InitializeStorage();
            if (Model.Table.PushNotifications == null)
            {
                Model.Table.PushNotifications = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.PushNotifications);
            }
        }

        public static void InitializeUserCheckinsTable()
        {
            InitializeStorage();
            if (Model.Table.UserCheckins == null)
            {
                Model.Table.UserCheckins = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.UserCheckins);
            }
        }


        public static void InitializeUserFollowTable()
        {
            InitializeStorage();
            if (Model.Table.UserFollow == null)
            {
                Model.Table.UserFollow = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.UserFollow);
            }
        }

        public static void InitializeUserReviewTable()
        {
            InitializeStorage();
            if (Model.Table.UserReview == null)
            {
                Model.Table.UserReview = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.UserReview);
            }
        }


        public static void InitializeUserPlaceFollowTable()
        {
            InitializeStorage();
            if (Model.Table.UserPlaceFollow == null)
            {
                Model.Table.UserPlaceFollow = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.UserPlaceFollow);
            }
        }

        public static void InitializeLocationTable()
        {
            InitializeStorage();
            if (Model.Table.Locations == null)
            {
                Model.Table.Locations = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.Location);
            }
        }

        public static void InitializeUserLocationsTable()
        {
            InitializeStorage();
            if (Model.Table.UserLocations == null)
            {
                Model.Table.UserLocations = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.UserLocations);
            }
        }

        public static void InitializeUserMeetTable()
        {
            InitializeStorage();
            if (Model.Table.UserMeet == null)
            {
                Model.Table.UserMeet = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.UserMeet);
            }
        }

        internal static void InitializeFoursuqareData()
        {
            InitializeStorage();
            if (Model.Table.FoursquareData == null)
            {
                Model.Table.FoursquareData = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.FoursquareData);
            }
        }

        internal static DateTime GetCloudSupportedDateTime()
        {
            return DateTime.MinValue.AddYears(1700);
        }


        public void DeleteAll()
        {
            
        }
    }
}
