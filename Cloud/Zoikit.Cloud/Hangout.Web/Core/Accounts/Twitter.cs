using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TweetSharp;

namespace Hangout.Web.Core.Accounts
{
    public class Twitter
    {
       
        public static AccountStatus InsertTwitterData(string accesstoken, string accessTagSecret)
        {
            Model.TwitterData fqdata = GetTwitterUser(accesstoken,accessTagSecret);
            
            if (!HasTwitterData(accesstoken))
            {
                service = new TwitterService("1i9gU4XCDgwrLrFwIvtQ", "79COrlDe7Kp1jH35FNs8cz8CvTNK2ewVoNoiuuHR6uM");
                service.AuthenticateWith(accesstoken, accessTagSecret);
                TwitterUser obj = service.VerifyCredentials(new VerifyCredentialsOptions());

                Model.TwitterData data = GetTwitterDataByID(obj.Id);

                if (data != null)
                {

                    Guid userId;
                    if (data.UserID == null)
                    {
                       userId = CreateUserData(fqdata);
                       data.UserID=userId;
                    }

                    data.AccessToken = accesstoken;
                    data.AccessTokenSecret = accessTagSecret;

                    UpdateTwitterData(data,false);

                   
                }
                else
                {
                    //create a new user :)
                    
                    Guid userId = CreateUserData(fqdata);
                    data = new Model.TwitterData();
                    data.AccessToken = accesstoken;
                    data.AccessTokenSecret = accessTagSecret;
                    data.UserID=userId;

                  
                    UpdateTwitterData(data,true);
                }

                return AccountStatus.AccountCreated;

            }
            else
            {

                //get foursquare data
                Model.TwitterData data = GetTwitterData(fqdata.AccessToken);
                //if data exists then update it. :)
                UpdateTwitterData(data,false);
                //return the result 
                return AccountStatus.Updated;
            }
            
            
        }

        private static Model.TwitterData GetTwitterDataByID(long id)
        {
            TableQuery<Model.TwitterData> TwitterData = new TableQuery<Model.TwitterData>().Where(TableQuery.CombineFilters(
            TableQuery.GenerateFilterCondition("TwitterID", QueryComparisons.Equal, id.ToString()), TableOperators.And, TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.TwitterData.GetPartitionKey(id)), TableOperators.And, TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, Model.TwitterData.GetRowKey(id)))));

            IEnumerable<Model.TwitterData> res = Model.Table.TwitterData.ExecuteQuery(TwitterData);

            Model.TwitterData data = res.FirstOrDefault();
            return data;
        }

        public static void UpdateTwitterData(Model.TwitterData fqdata, bool insertFlag)
        {

            if (fqdata.AccessToken == null || String.IsNullOrEmpty(fqdata.AccessToken))
            {
                throw new ArgumentNullException("Access token not found.");
            }

            if (fqdata.UserID == Guid.Empty)
            {
                throw new ArgumentNullException("UserID Empty");
            }
            //get the online data :)
            Model.TwitterData onlineData = new Model.TwitterData();

            onlineData = GetTwitterUser(fqdata.AccessToken,fqdata.AccessTokenSecret);

            //copy all the fields. 


            fqdata.DateTimeUpdated = DateTime.Now;
            fqdata.DateTimeStamp = DateTime.Now;

            fqdata.AccessToken = onlineData.AccessToken;
            fqdata.AccessTokenSecret = onlineData.AccessTokenSecret;
            fqdata.Description = onlineData.Description;
            fqdata.FollowersCount = onlineData.FollowersCount;
            fqdata.FollowingCount = onlineData.FollowingCount;
            fqdata.Language = onlineData.Language;
            fqdata.Location = onlineData.Location;

            fqdata.Name = onlineData.Name;
            fqdata.ProfileImageURL = onlineData.ProfileImageURL;
            fqdata.ScreenName = onlineData.ScreenName;
            fqdata.TimeZone = onlineData.TimeZone;
            fqdata.TwitterID = onlineData.TwitterID;
            fqdata.URL = onlineData.URL;
            fqdata.PartitionKey = Model.TwitterData.GetPartitionKey(fqdata.TwitterID);
            fqdata.RowKey = Model.TwitterData.GetRowKey(fqdata.TwitterID);

            if (insertFlag)
            {
                //add to table
                Model.Table.TwitterData.Execute(TableOperation.Insert(fqdata));
            }
            else
            {
                //replace
                //add to table
                Model.Table.TwitterData.Execute(TableOperation.Replace(fqdata));
            }
           
            
        }

        public static Guid CreateUserData(Model.TwitterData data)
        {


            Core.Cloud.TableStorage.InitializeUsersTable();
            Core.Cloud.TableStorage.InitializeUserProfileTable();

            Model.User user = new Model.User(data.Email);
            user.Activated = true;
            user.Blocked = false;
            user.DateTimeStamp = DateTime.Now;
            user.DateTimeUpdated = DateTime.Now;
            user.Email = data.Email;
            user.ZAT = Guid.NewGuid().ToString();
            Model.UserProfile profile = new Model.UserProfile(user.UserID);
            profile.DateTimeStamp = DateTime.Now;
            profile.DateTimeUpdated = DateTime.Now;
            profile.Birthday = new DateTime(1990, 1, 1);
            profile.Gender = false;
            profile.Name = "";
            profile.ProfilePicURL = data.ProfileImageURL;
            Model.Table.UserProfile.Execute(TableOperation.Insert(profile));
            Model.Table.Users.Execute(TableOperation.Insert(user));
            return user.UserID;
        }

        public static Model.TwitterData GetTwitterData(string accesstoken)
        {

            Core.Cloud.TableStorage.InitializeTwitterDataTable();

            TableQuery<Model.TwitterData> TwitterData = new TableQuery<Model.TwitterData>().Where(
               TableQuery.GenerateFilterCondition("AccessToken", QueryComparisons.Equal, accesstoken));

            IEnumerable<Model.TwitterData> res = Model.Table.TwitterData.ExecuteQuery(TwitterData);

            return res.FirstOrDefault();
        }

        public static Model.TwitterData GetTwitterUser(string accesstoken,string secret)
        {

            service = new TwitterService("1i9gU4XCDgwrLrFwIvtQ", "79COrlDe7Kp1jH35FNs8cz8CvTNK2ewVoNoiuuHR6uM");

            service.AuthenticateWith(accesstoken, secret);

            TwitterUser obj = service.VerifyCredentials(new VerifyCredentialsOptions());

            if (obj == null)
            {
                return null;
            }


            Model.TwitterData user = new Model.TwitterData(obj.Id);

            user.AccessToken = accesstoken;
            user.AccessTokenSecret = secret;
            user.TwitterID = obj.Id;
            user.URL = obj.Url;
            user.ScreenName = obj.ScreenName;
            user.DateTimeStamp = DateTime.Now;
            user.DateTimeUpdated = DateTime.Now;
            user.Description = obj.Description;
            user.FollowersCount = obj.FollowersCount;
            user.FollowingCount = 0;
            user.Language = obj.Language;
            user.Location = obj.Location;
            user.Name = obj.Name;
            user.ProfileImageURL = obj.ProfileImageUrl;
            user.TimeZone = obj.TimeZone;
           
            return user;
        }



        public static void UpdateTwitterData(Guid userId)
        {

            service = new TwitterService("1i9gU4XCDgwrLrFwIvtQ", "79COrlDe7Kp1jH35FNs8cz8CvTNK2ewVoNoiuuHR6uM");

            


            Model.TwitterData user = GetTwitterData(userId);

            if (user != null)
            {

                service.AuthenticateWith(user.AccessToken, user.AccessTokenSecret);

                TwitterUser obj = service.VerifyCredentials(new VerifyCredentialsOptions());

                if (obj == null)
                {
                    return;
                }

                
                user.TwitterID = obj.Id;
                user.URL = obj.Url;
                user.ScreenName = obj.ScreenName;
                user.DateTimeStamp = DateTime.Now;
                user.DateTimeUpdated = DateTime.Now;

                user.Description = obj.Description;
                user.FollowersCount = obj.FollowersCount;
                user.FollowingCount = 0;
                user.Language = obj.Language;
                user.Location = obj.Location;
                user.Name = obj.Name;
                user.ProfileImageURL = obj.ProfileImageUrl;
                user.TimeZone = obj.TimeZone;

                Model.Table.TwitterData.Execute(TableOperation.Replace(user));
            }

        }

        public static Core.Accounts.UserData GetUserData(string accesstoken, string accessTagSecret)
        {
            Guid? userId = GetuserId(accesstoken);
            if (userId == null)
            {
                return new UserData();
            }
            return Accounts.User.GetUserData((Guid)userId);
        }

        public static Guid? GetuserId(string accesstoken)
        {
            if (HasTwitterData(accesstoken))
            {
                return GetTwitterData(accesstoken).UserID;
            }

            return null;
        }


        public static bool HasTwitterkData(Guid userId)
        {
            if (GetTwitterData(userId)==null)
            {
                return false;
            }

            return true;
        }

        public static bool HasTwitterData(string accessTag)
        {
            if (GetTwitterData(accessTag)== null)
            {
                return false;
            }

            return true;
        }

        public static TwitterService service { get; set; }



        public static AccountStatus InsertTwitterData(Guid userId, string accesstoken, string accesstokensecret)
        {
            if (!Accounts.User.DoesUserExists(userId))
            {
                return AccountStatus.Error;
            }


            Model.TwitterData fqdata = GetTwitterData(accesstoken);
            if (!HasTwitterData(accesstoken))
            {

                service = new TwitterService("1i9gU4XCDgwrLrFwIvtQ", "79COrlDe7Kp1jH35FNs8cz8CvTNK2ewVoNoiuuHR6uM");
                service.AuthenticateWith(accesstoken, accesstokensecret);
                TwitterUser obj = service.VerifyCredentials(new VerifyCredentialsOptions());

                Model.TwitterData data = GetTwitterDataByID(obj.Id);

                if (data != null)
                {
                    
                    data.UserID=userId;
                    data.AccessToken = accesstoken;
                    data.AccessTokenSecret = accesstokensecret;
                    UpdateTwitterData(data,false);


                }
                else
                {
                    data = new Model.TwitterData();
                    data.AccessToken = accesstoken;
                    data.AccessTokenSecret = accesstokensecret;
                    data.UserID=userId;
                    
                    UpdateTwitterData(data,true);
                    
                }

                return AccountStatus.AccountCreated;
            }
            else
            {
                //if data exists then update it. :)
                UpdateTwitterData(fqdata,false);
                //return the result 
                return AccountStatus.Updated;
            }
        }

        public static Model.TwitterData GetTwitterData(Guid userId)
        {
            Core.Cloud.TableStorage.InitializeTwitterDataTable();

            TableQuery<Model.TwitterData> TwitterData = new TableQuery<Model.TwitterData>().Where(
            TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId));

            IEnumerable<Model.TwitterData> data = Model.Table.TwitterData.ExecuteQuery(TwitterData);

            return data.FirstOrDefault();
        }

        public static void RefreshTwitterRelationships(Guid userId)
        {
            Model.TwitterData user = GetTwitterData(userId);

            if (user != null)
            {


                service = new TwitterService("1i9gU4XCDgwrLrFwIvtQ", "79COrlDe7Kp1jH35FNs8cz8CvTNK2ewVoNoiuuHR6uM");
                service.AuthenticateWith(user.AccessToken, user.AccessTokenSecret);

               ListFriendsOptions obj=new ListFriendsOptions();
                obj.Cursor=-1;
                obj.IncludeUserEntities=true;
                obj.ScreenName=user.ScreenName;
                obj.SkipStatus=true;
                obj.UserId=user.TwitterID;

               TwitterCursorList<TwitterUser> friends = service.ListFriends(obj);
               while (friends.NextCursor!=null)
               {
                   foreach (TwitterUser tu in friends)
                   {
                       Model.TwitterData x = GetTwitterUser(tu);

                       //check for relationships. :)

                       TableQuery<Model.TwitterRelationships> rel = new TableQuery<Model.TwitterRelationships>().Where(TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.TwitterRelationships.GetPartitionKey(user.TwitterID)), TableOperators.And, TableQuery.GenerateFilterCondition("TwitterUser2ID", QueryComparisons.Equal, Model.TwitterData.GetPartitionKey(x.TwitterID))));

                       Model.TwitterRelationships relationship = Model.Table.TwitterData.ExecuteQuery(rel).FirstOrDefault();


                      

                       if (relationship == null)
                       {
                           relationship = new Model.TwitterRelationships(user.TwitterID, x.TwitterID);
                           relationship.DateTimeStamp = DateTime.Now;

                           Model.Table.TwitterRelationships.Execute(TableOperation.Insert(relationship));

                       }
                   }

                   obj.Cursor = friends.NextCursor;
                   friends = service.ListFriends(obj);
               }
             }
         }
        


        public static Model.TwitterData GetTwitterUser(TwitterUser obj)
        {

            if (obj == null)
            {
                return null;
            }

            Model.TwitterData user = GetTwitterDataByID(obj.Id);
            TableOperation op;
            if (user == null)
            {
                user = new Model.TwitterData();
                op = TableOperation.Insert(user);
            }
            else
            {
                op = TableOperation.Replace(user);
            }

            user.URL = obj.Url;
            user.ScreenName = obj.ScreenName;
            user.DateTimeStamp = DateTime.Now;
            user.DateTimeUpdated = DateTime.Now;

            user.Description = obj.Description;
            user.FollowersCount = obj.FollowersCount;
            user.FollowingCount = 0;
            user.Language = obj.Language;
            user.Location = obj.Location;
            user.Name = obj.Name;
            user.ProfileImageURL = obj.ProfileImageUrl;
            user.TimeZone = obj.TimeZone;
            user.TwitterID = obj.Id;
            Model.Table.TwitterData.Execute(op);

            return user;


        }

        internal static Model.TwitterData GetTwitterDataByEmail(string email)
        {

            Core.Cloud.TableStorage.InitializeTwitterDataTable();

            TableQuery<Model.TwitterData> TwitterData = new TableQuery<Model.TwitterData>().Where(
            TableQuery.GenerateFilterCondition("Email", QueryComparisons.Equal, email));

            IEnumerable<Model.TwitterData> data = Model.Table.TwitterData.ExecuteQuery(TwitterData);

            return data.FirstOrDefault();
        }
    }
}