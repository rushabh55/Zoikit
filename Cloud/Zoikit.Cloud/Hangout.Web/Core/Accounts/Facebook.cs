using Facebook;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace Hangout.Web.Core.Accounts
{
    public class Facebook
    {
        public static bool HasFacebookData(Guid userId)
        {
            TableQuery<Model.FacebookData> facebookData = new TableQuery<Model.FacebookData>().Where(
            TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId));
            if (Model.Table.FacebookData.ExecuteQuery(facebookData).Count()==0)
            {
                return false;
            }

            return true;
        }

        public static bool HasFacebookData(string accessTag)
        {

            Core.Cloud.TableStorage.InitializeFacebookDataTable();

            TableQuery<Model.FacebookData> facebookData = new TableQuery<Model.FacebookData>().Where(
            TableQuery.GenerateFilterCondition("AccessToken", QueryComparisons.Equal, accessTag));
            if (Model.Table.FacebookData.ExecuteQuery(facebookData).Count() == 0)
            {
                return false;
            }

            return true;
        }



        public static AccountStatus InsertFacebookData(string accesstoken)
        {

            Core.Cloud.TableStorage.InitializeFacebookDataTable();


            Model.FacebookData fqdata = GetFacebookUser(accesstoken);
            Guid userId=Core.Accounts.User.GetIfUserExists(fqdata.Email);

            if ( userId== Guid.Empty || userId == null)
            {
                if (!HasFacebookData(accesstoken))
                {
                    //create a new user :)
                    userId = CreateUserData(fqdata);

                    FacebookClient client = new FacebookClient(accesstoken);
                    string result = client.Get("me").ToString();

                    JObject obj = JObject.Parse(result);


                    long facebookId;
                    if (obj.SelectToken("id") != null)
                    {
                        facebookId = long.Parse(obj.SelectToken("id").ToString());
                    }
                    else
                    {
                        return AccountStatus.Error;
                    }


                    Model.FacebookData data = GetFacebookDataByID(facebookId);

                    if (data != null)
                    {
                        data.AccessToken = accesstoken;
                        data.UserID=userId;
                        data = UpdateFacebookData(data);

                        Model.Table.FacebookData.Execute(TableOperation.Replace(data));

                    }
                    else
                    {
                        data = new Model.FacebookData();
                        data.AccessToken = accesstoken;
                        data.UserID=userId;
                        
                        data= UpdateFacebookData(data);

                        Model.Table.FacebookData.Execute(TableOperation.Insert(data));

                    }

                    

                    return AccountStatus.AccountCreated;

                }
                else
                {

                    //get foursquare data
                    Model.FacebookData data = GetFacebookData(fqdata.AccessToken);
                    //if data exists then update it. :)
                    data = UpdateFacebookData(data);

                    Model.Table.FacebookData.Execute(TableOperation.Replace(data));
                    //return the result 
                    return AccountStatus.Updated;
                }
            }
            else
            {
                Model.FacebookData data = GetFacebookData(userId);


                if (Core.Accounts.User.GetUser(userId) == null)
                {
                    userId = CreateUser(fqdata);
                }

                if (Core.Accounts.User.GetUserProfile(userId) == null)
                {
                    
                    CreateUserProfile(userId,fqdata);
                }

               

                bool flag = false;
                if (data == null)
                {
                    flag = true;
                    data = new Model.FacebookData();
                    data.UserID = Core.Accounts.User.GetIfUserExists(fqdata.Email);
                }
                data.AccessToken = accesstoken;




                data = UpdateFacebookData(data);



                data.DateTimeUpdated = DateTime.Now;

                if (flag)
                {
                    data.DateTimeStamp = DateTime.Now;
                    Model.Table.FacebookData.Execute(TableOperation.InsertOrReplace(data));
                }
                else
                {
                    Model.Table.FacebookData.Execute(TableOperation.Replace(data));
                }
                return AccountStatus.Updated;
            }
        }

        private static void CreateUserProfile(Guid userId, Model.FacebookData data)
        {

            Core.Cloud.TableStorage.InitializeUserProfileTable();

            Model.UserProfile profile = new Model.UserProfile(userId);
            profile.DateTimeStamp = DateTime.Now;
            profile.DateTimeUpdated = DateTime.Now;
            profile.FirstName = data.FirstName;
            profile.Gender = data.Gender;
            profile.LargeProfilePicURL = data.LargeProfilePicURL;
            profile.LastName = data.LastName;
            profile.Name = profile.FirstName + " " + profile.LastName;
            profile.Phone = data.Phone;
            profile.ProfilePicURL = profile.LargeProfilePicURL;

            if (profile.Birthday == DateTime.MinValue)
                profile.Birthday = Core.Cloud.TableStorage.GetCloudSupportedDateTime();

            

            Model.Table.UserProfile.Execute(TableOperation.InsertOrReplace(profile));





          
        }

        public static Guid CreateUserData(Model.FacebookData data)
        {
            Core.Cloud.TableStorage.InitializeUsersTable();
            Core.Cloud.TableStorage.InitializeUserProfileTable();

            Model.User user = new Model.User();
            user.Activated = true;
            user.Blocked = false;
            user.DateTimeStamp = DateTime.Now;
            user.DateTimeUpdated = DateTime.Now;
            user.Email = data.Email;
            user.ZAT = Guid.NewGuid().ToString();
            Model.UserProfile profile = new Model.UserProfile(user.UserID);
            profile.DateTimeStamp = DateTime.Now;
            profile.DateTimeUpdated = DateTime.Now;
            profile.FirstName = data.FirstName;
            profile.Gender = data.Gender;
            profile.LargeProfilePicURL = data.LargeProfilePicURL;
            profile.LastName = data.LastName;
            profile.Name = profile.FirstName + " " + profile.LastName;
            profile.Phone = data.Phone;
            profile.ProfilePicURL = profile.LargeProfilePicURL;

            if (profile.Birthday == DateTime.MinValue)
                profile.Birthday = Core.Cloud.TableStorage.GetCloudSupportedDateTime();

            Model.Table.Users.Execute(TableOperation.InsertOrReplace(user));

            Model.Table.UserProfile.Execute(TableOperation.InsertOrReplace(profile));

            return user.UserID;
        }

        

        private static Guid CreateUser(Model.FacebookData data)
        {

            Core.Cloud.TableStorage.InitializeUsersTable();

            Model.User user = new Model.User();
            user.Activated = true;
            user.Blocked = false;
            user.DateTimeStamp = DateTime.Now;
            user.DateTimeUpdated = DateTime.Now;
            user.Email = data.Email;
            user.ZAT = Guid.NewGuid().ToString();

            Model.Table.Users.Execute(TableOperation.InsertOrReplace(user));


            return user.UserID;


        }

        private static Model.FacebookData GetFacebookDataByID(long facebookId)
        {
            TableQuery<Model.FacebookData> facebookData = new TableQuery<Model.FacebookData>().Where(TableQuery.CombineFilters(
            TableQuery.GenerateFilterCondition("FacebookID", QueryComparisons.Equal, facebookId.ToString()), TableOperators.And, TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, Model.FacebookData.GetRowKey(facebookId)), TableOperators.And, TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.FacebookData.GetPartitionKey(facebookId)))));

            Model.FacebookData data = Model.Table.FacebookData.ExecuteQuery(facebookData).FirstOrDefault();
            return data;
        }

        public static Model.FacebookData UpdateFacebookData(Model.FacebookData fqdata)
        {

            if (fqdata.AccessToken == null || String.IsNullOrEmpty(fqdata.AccessToken))
            {
                throw new ArgumentNullException("Access token not found.");
            }

            if (fqdata.UserID == null||fqdata.UserID==Guid.Empty)
            {
                throw new ArgumentNullException("userId null or 0");
            }
            //get the online data :)
            Model.FacebookData onlineData = new Model.FacebookData();

            onlineData = GetFacebookUser(fqdata.AccessToken);

            //copy all the fields. 

            fqdata.RowKey = Model.FacebookData.GetRowKey(onlineData.FacebookID);
            fqdata.PartitionKey = Model.FacebookData.GetPartitionKey(onlineData.FacebookID);
            fqdata.DateTimeUpdated = DateTime.Now;
            if (fqdata.DateTimeStamp.Date == DateTime.Parse("1/1/0001").Date)
            {
                fqdata.DateTimeStamp = DateTime.Now;
            }
            fqdata.FirstName = onlineData.FirstName;
            fqdata.FacebookID = onlineData.FacebookID;
            fqdata.Gender = onlineData.Gender;
            fqdata.LastName = onlineData.LastName;
            fqdata.LargeProfilePicURL = onlineData.LargeProfilePicURL;
            fqdata.Email = onlineData.Email;
            fqdata.Phone = onlineData.Phone;
            fqdata.ProfilePicURL = onlineData.ProfilePicURL;
            
            //cleanup
            if (fqdata.About == null)
                fqdata.About = String.Empty;
            
                fqdata.Age = 0;
                if (fqdata.Birthday == DateTime.MinValue)
                    fqdata.Birthday = new DateTime(1990, 1, 1);
            if (fqdata.Email == null)
                fqdata.Email = String.Empty;
            if (fqdata.LargeProfilePicURL == null)
                fqdata.LargeProfilePicURL = String.Empty;
            if (fqdata.LastName == null)
                fqdata.LastName = String.Empty;
            if (fqdata.Link == null)
                fqdata.Link = String.Empty;
            if (fqdata.Phone == null)
                fqdata.Phone = String.Empty;
             if (fqdata.ProfilePicURL == null)
                fqdata.ProfilePicURL = String.Empty;
             if (fqdata.RelationshipStatus == null)
                 fqdata.RelationshipStatus = String.Empty;
             
                 fqdata.TimeZone = 0;
             if (fqdata.UserID == null)
                 fqdata.UserID = Guid.Empty;


            return fqdata;
        }

        private static Model.FacebookData GetFacebookData(string accesstoken)
        {
            TableQuery<Model.FacebookData> data = new TableQuery<Model.FacebookData>().Where(TableQuery.GenerateFilterCondition("AccessToken", QueryComparisons.Equal, accesstoken));

            return Model.Table.FacebookData.ExecuteQuery(data).FirstOrDefault();
        }

        public static Model.FacebookData GetFacebookUser(string accesstoken)
        {



            FacebookClient client = new FacebookClient(accesstoken);

            string result = client.Get("me").ToString();



            JObject obj = JObject.Parse(result);

            string name = "", link = "", username = "", email = "", relationshipStatus = "", firstname = "", lastname = "";
            long id = -1;
            float timezone = 0.0f;
            bool? gender = null;
            DateTime? dob = null;

            if (obj.SelectToken("id") != null)
            {
                id = long.Parse(obj.SelectToken("id").ToString());
            }
            else
            {
                return null;
            }


            Model.FacebookData user;
            

           

            if (obj.SelectToken("name") != null)
            {
                name = obj.SelectToken("name").ToString();
            }

            if (obj.SelectToken("id") != null)
            {
                id = long.Parse(obj.SelectToken("id").ToString());
            }

            if (obj.SelectToken("link") != null)
            {
                link = obj.SelectToken("link").ToString();
            }

            if (obj.SelectToken("username") != null)
            {
                username = obj.SelectToken("username").ToString();
            }


            if (obj.SelectToken("email") != null)
            {
                email = obj.SelectToken("email").ToString();
            }

            if (obj.SelectToken("relationship_status") != null)
            {
                relationshipStatus = obj.SelectToken("relationship_status").ToString();
                if (relationshipStatus != "In a relationship" && relationshipStatus != "Married" && relationshipStatus != "Engaged")
                {
                    relationshipStatus = "Single";
                }
            }
            else
            {
                relationshipStatus = "Single"; //Default it to single :)
            }

            if (obj.SelectToken("timezone") != null)
            {
                timezone = float.Parse(obj.SelectToken("timezone").ToString());
            }

            if (obj.SelectToken("first_name") != null)
            {
                firstname = obj.SelectToken("first_name").ToString();
            }

            if (obj.SelectToken("last_name") != null)
            {
                lastname = obj.SelectToken("last_name").ToString();
            }

            if (obj.SelectToken("birthday_date") != null)
            {
                dob = DateTime.Parse(obj.SelectToken("birthday_date").ToString());
            }


            if (obj.SelectToken("gender") != null)
            {
                string tmp = obj.SelectToken("gender").ToString();
                if (tmp == "male")
                {
                    gender = true;
                }
                else if (tmp == "female")
                {
                    gender = false;
                }
                else
                {
                    gender = null;
                }


            }

            //user.AccessToken = accessTag;
            //user.Activated = true;

            user = new Model.FacebookData();
            user.AccessToken = accesstoken;
            user.Link = link;
            user.DateTimeStamp = DateTime.Now;
            user.DateTimeUpdated = DateTime.Now;
            user.FacebookID = id;
            user.Email = email;
            user.RelationshipStatus = relationshipStatus;
            user.TimeZone = timezone;
            user.Gender = (bool)gender;
            user.FirstName = firstname;
            user.LastName = lastname;

            if (dob != null)
            {
                user.Birthday = (DateTime)dob;
            }

            if (user.Birthday != null)
            {
                user.Age = DateTime.Now.Year - ((DateTime)(user.Birthday)).Year;
            }

           
            string pictureUrl = @"https://graph.facebook.com/"+id+"/picture";

            string largepictureUrl = @"https://graph.facebook.com/" + id + "/picture?type=large";
            user.ProfilePicURL = pictureUrl;
            user.LargeProfilePicURL = largepictureUrl;


            return user;
        }

        public static AccountStatus InsertFacebookData(Guid userId, string accesstoken)
        {
            if (!Accounts.User.DoesUserExists(userId))
            {
                return AccountStatus.Error;
            }


            Model.FacebookData fqdata = GetFacebookData(accesstoken);
            if (!HasFacebookData(accesstoken))
            {

                FacebookClient client = new FacebookClient(accesstoken);
                string result = client.Get("me").ToString();

                JObject obj = JObject.Parse(result);


                long facebookId;
                if (obj.SelectToken("id") != null)
                {
                    facebookId = long.Parse(obj.SelectToken("id").ToString());
                }
                else
                {
                    return AccountStatus.Error;
                }

                Model.FacebookData data = GetFacebookDataByID(facebookId);

                if (data != null)
                {
                    data.AccessToken = accesstoken;
                    data.UserID=userId;
                    UpdateFacebookData(data);
                    return AccountStatus.AccountCreated;
                }

                else
                {
                    data = new Model.FacebookData();
                    data.AccessToken = accesstoken;
                    data.UserID=userId;
                    data = UpdateFacebookData(data);
                    Model.Table.FacebookData.Execute(TableOperation.Insert(data));
                    return AccountStatus.AccountCreated;
                }
            }
            else
            {
                //if data exists then update it. :)
                UpdateFacebookData(fqdata);
                //return the result 
                return AccountStatus.Updated;
            }

        }

        

        public static Model.FacebookData GetFacebookData(Guid userId)
        {
            Web.Core.Cloud.TableStorage.InitializeFacebookDataTable();

            TableQuery<Model.FacebookData> data = new TableQuery<Model.FacebookData>().Where(TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId));

            return Model.Table.FacebookData.ExecuteQuery(data).FirstOrDefault();
        }

        public static UserData GetUserData(string accesstoken)
        {
            Guid userId = GetuserId(accesstoken);
            if (userId == null)
            {
                return new UserData();
            }
            return Accounts.User.GetUserData(userId);
        }

        public static Guid GetuserId(string accessTag)
        {
            if (HasFacebookData(accessTag))
            {
                return GetFacebookData(accessTag).UserID;
            }

            return Guid.Empty;
        }



        public static void UpdateFacebookFriends(Guid userId)
        {

            Model.FacebookData user = Core.Accounts.Facebook.GetFacebookData(userId);
            if (user != null)
            {
                if (user.AccessToken != null)
                {

                    var client = new FacebookClient(user.AccessToken);

                    dynamic myInfo = client.Get("/me/friends");
                    if (myInfo != null)
                    {
                        foreach (dynamic friend in myInfo.data)
                        {
                            UpdateFacebookFriendUser(userId, Convert.ToInt64(friend.id),friend.name);
                        }
                    }
                }
            }
        }

        private static void UpdateFacebookFriendUser(Guid userId, long friendId, string name)
        {
            Model.FacebookData user = GetFacebookData(friendId);
            Model.FacebookData user1 = GetFacebookData(userId);

            if(user1!=null)
            {
                if (user == null)
                {
                    user = new Model.FacebookData(friendId);
                    user.FacebookID = friendId;



                    try
                    {
                        user.FirstName = name.Split(' ')[0];
                        try
                        {
                            user.LastName = name.Remove(0, user.FirstName.Count() + 1);
                        }
                        catch { }
                        user.ProfilePicURL = @"https://graph.facebook.com/"+user.FacebookID+"/picture";
                        user.DateTimeStamp = DateTime.Now;
                        user.DateTimeUpdated = DateTime.Now;
                    }
                    catch
                    {

                    }


                    Model.Table.FacebookData.Execute(TableOperation.Insert(user));
                }


                //get the relationship. :)

                Model.FacebookRelationships relationship1 = GetFacebookRelationship(user1.FacebookID, user.FacebookID);
                Model.FacebookRelationships relationship2 = GetFacebookRelationship(user.FacebookID, user1.FacebookID);

                if (relationship1 == null)
                {
                    //create one. :)

                    relationship1 = new Model.FacebookRelationships();
                    relationship1.FacebookUser1ID = user1.FacebookID;
                    relationship1.FacebookUser2ID = user.FacebookID;
                    relationship1.DateTimeStamp = DateTime.Now;
                    Model.Table.FacebookRelationships.Execute(TableOperation.Insert(relationship1));
                }

                if (relationship2 == null)
                {
                    relationship2 = new Model.FacebookRelationships();
                    relationship2.FacebookUser1ID = user.FacebookID;
                    relationship2.FacebookUser2ID = user1.FacebookID;
                    relationship2.DateTimeStamp = DateTime.Now;
                    Model.Table.FacebookRelationships.Execute(TableOperation.Insert(relationship2));
                }

                



            }
        }

       public static Model.FacebookRelationships GetFacebookRelationship(long id1, long id2)
        {
            return Model.Table.FacebookRelationships.ExecuteQuery(new TableQuery<Model.FacebookRelationships>().Where(TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("FacebookUser1ID", QueryComparisons.Equal, id1.ToString()),TableOperators.And,TableQuery.GenerateFilterCondition("FacebookUserID2",QueryComparisons.Equal,id2.ToString())))).FirstOrDefault();
        }

       public static Model.FacebookData GetFacebookData(long userId)
        {
            return Model.Table.FacebookData.ExecuteQuery(new TableQuery<Model.FacebookData>().Where(TableQuery.GenerateFilterCondition("FacebookID", QueryComparisons.Equal, userId.ToString()))).FirstOrDefault();
        }

        public static Model.FacebookData GetFacebookDataByEmail(string email)
        {

            Core.Cloud.TableStorage.InitializeFacebookDataTable();

            TableQuery<Model.FacebookData> facebookData = new TableQuery<Model.FacebookData>().Where(
            TableQuery.GenerateFilterCondition("Email", QueryComparisons.Equal, email));

            IEnumerable<Model.FacebookData> data=Model.Table.FacebookData.ExecuteQuery(facebookData);

            if (data == null)
                return null;


            return data.FirstOrDefault();
        }




        internal static bool DoesUserExists(long id)
        {
            throw new NotImplementedException();
        }
    }
}