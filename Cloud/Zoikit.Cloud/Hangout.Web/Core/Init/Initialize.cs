using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Core.Init
{
    public class Initialize
    {
        public static void InitializeStorage()
        {
            CreateTables();
            AddAndroidApp();
            AddAvtars();
        }

        private static void CreateTables()
        {
            // Retrieve the storage account from the connection string.
            Model.Table.CloudStorageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(
               Settings.CloudStorageString);

            // Create the table client.
            Model.Table.TableClient = Model.Table.CloudStorageAccount.CreateCloudTableClient();

            // Create the CloudTable :)

            Model.Table.ActivityLog = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.ActivityLog);
            Model.Table.ActivityLog.CreateIfNotExists();

            Model.Table.Avatars = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.Avatars);
            Model.Table.Avatars.CreateIfNotExists();

            Model.Table.Badge = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.Badge);
            Model.Table.Badge.CreateIfNotExists();

            Model.Table.Buzz = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.Buzz);
            Model.Table.Buzz.CreateIfNotExists();


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

            Model.Table.Locations = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.Location);
            Model.Table.Locations.CreateIfNotExists();


            Model.Table.Notifications = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.Notifications);
            Model.Table.Notifications.CreateIfNotExists();

            Model.Table.NotificationsUser = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.NotificationUser);
            Model.Table.NotificationsUser.CreateIfNotExists();

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

            Model.Table.TagInCategory = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.TagInCategory);
            Model.Table.Category.CreateIfNotExists();

            Model.Table.Text = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.Text);
            Model.Table.Text.CreateIfNotExists();

            Model.Table.TwitterData = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.TwitterData);
            Model.Table.TwitterData.CreateIfNotExists();

            Model.Table.TwitterRelationships = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.TwitterRelationships);
            Model.Table.TwitterRelationships.CreateIfNotExists();

            Model.Table.UserBadge = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.UserBadge);
            Model.Table.UserBadge.CreateIfNotExists();

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

            Model.Table.Apps = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.Apps);
            Model.Table.Apps.CreateIfNotExists();

            Model.Table.Places = Model.Table.TableClient.GetTableReference(CloudSettings.TableReference.Places);
            Model.Table.Places.CreateIfNotExists();
        }

        private static void AddAvtars()
        {
            for (int i = 1; i <= 10; i++)
            {
                if (i <= 5)
                {
                    Core.Accounts.User.InsertAvatar("Male", "/Images/Avatar/" + i + ".png");

                }
                else
                {
                    Core.Accounts.User.InsertAvatar("Female", "/Images/Avatar/" + i + ".png");
                }
            }
             
        }

        private static void AddAndroidApp()
        {
            Core.Apps.Apps.AddApp("Official Android App", "Official Android App for Zoik it!");
        }
    }
}