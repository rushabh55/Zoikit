using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace Hangout.Web.Core.Accounts
{
    public class Foursquare
    {
        public static bool HasFoursquareData(Guid userId)
        {

            Core.Cloud.TableStorage.InitializeFoursuqareData();


            if (Model.Table.FoursquareData.ExecuteQuery(new TableQuery<Model.FoursquareData>().Where(TableQuery.GenerateFilterCondition("FoursquareID", QueryComparisons.Equal, userId.ToString()))).Count()>0)
            {
                return true;
            }

            return false;
        }

        public static bool HasFoursquareData(string accessTag)
        {
            Core.Cloud.TableStorage.InitializeFoursuqareData();

            if (Model.Table.FoursquareData.ExecuteQuery(new TableQuery<Model.FoursquareData>().Where(TableQuery.GenerateFilterCondition("AccessToken", QueryComparisons.Equal, accessTag))).Count()==0)
            {
                return false;
            }

            return true;
        }

       

        public static AccountStatus InsertFoursquareData(string accesstoken)
        {

            Core.Cloud.TableStorage.InitializeFoursuqareData();


                Model.FoursquareData fqdata = GetFoursquareUser(accesstoken);
                if (!HasFoursquareData(accesstoken))
                {
                    //create a new user :)
                    Guid userId = CreateUserData(fqdata);

                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(@"https://api.foursquare.com/v2/users/self?oauth_token=" + accesstoken + "&v=20121116");
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    JObject obj = JObject.Parse(new StreamReader(response.GetResponseStream()).ReadToEnd());
                    if (obj.SelectToken("response") == null)
                    {
                        return AccountStatus.Error;
                    }

                    if (obj.SelectToken("response").SelectToken("user") == null)
                    {
                        return AccountStatus.Error;
                    }

                    JToken user = obj.SelectToken("response").SelectToken("user");

                   



                    if (user.SelectToken("id") == null)
                    {
                        return AccountStatus.Error;
                    }

                    long foursquareId=Convert.ToInt64(user.SelectToken("id").ToString());

                    Model.FoursquareData data = Model.Table.FoursquareData.ExecuteQuery(new TableQuery<Model.FoursquareData>().Where(TableQuery.GenerateFilterCondition("FoursquareID", QueryComparisons.Equal,foursquareId.ToString()))).FirstOrDefault();

                    if (data == null)
                    {
                        data = new Model.FoursquareData(foursquareId);

                        data.AccessToken = accesstoken;
                        data.UserID=userId;
                        
                        data=UpdateFoursquareData(data);
                        Model.Table.FoursquareData.Execute(TableOperation.Insert(data));
                    }
                    else
                    {
                        data.AccessToken = accesstoken;
                        data.UserID=userId;
                        Model.Table.FoursquareData.Execute(TableOperation.Replace(data));
                    }

                    return AccountStatus.AccountCreated;

                }
                else
                {

                    //get foursquare data
                    Model.FoursquareData data = GetFoursquareData(fqdata.AccessToken);
                    //if data exists then update it. :)
                    data=UpdateFoursquareData(data);

                    Model.Table.FoursquareData.Execute(TableOperation.Replace(data));
                    //return the result 
                    return AccountStatus.Updated;
                }
            
          

            
        }

       public static Model.FoursquareData UpdateFoursquareData(Model.FoursquareData fqdata)
        {

            if (fqdata.AccessToken == null || String.IsNullOrEmpty(fqdata.AccessToken))
            {
                throw new ArgumentNullException("Access token not found.");
            }

            if (fqdata.UserID == null || fqdata.UserID == Guid.Empty)
            {
                throw new ArgumentNullException("userId null or 0");
            }
            //get the online data :)
            Model.FoursquareData onlineData = new Model.FoursquareData();

            onlineData = GetFoursquareUser(fqdata.AccessToken);

            //copy all the fields. 

            fqdata.Bio = onlineData.Bio;
            fqdata.DateTimeUpdated = DateTime.Now;
            if (fqdata.DateTimeStamp.Date == DateTime.Parse("1/1/0001").Date)
            {
                fqdata.DateTimeStamp = DateTime.Now;
            }
            fqdata.Email = onlineData.Email;
            fqdata.Facebook = onlineData.Facebook;
            fqdata.FirstName = onlineData.FirstName;
            fqdata.FoursquareID = onlineData.FoursquareID;
            fqdata.Gender = onlineData.Gender;
            fqdata.Homecity = onlineData.Homecity;
            fqdata.LastName = onlineData.LastName;
            fqdata.Phone = onlineData.Phone;
          

            fqdata.PhotoPrefix = onlineData.PhotoPrefix;
            fqdata.PhotoSuffix = onlineData.PhotoSuffix;
            fqdata.Twitter = onlineData.Twitter;

           return fqdata;


        }

        private static Model.FoursquareData GetFoursquareData(string accesstoken)
        {
            return Model.Table.FoursquareData.ExecuteQuery(new TableQuery<Model.FoursquareData>().Where(TableQuery.GenerateFilterCondition("AccessToken", QueryComparisons.Equal, accesstoken.ToString()))).FirstOrDefault();
        }

        private static Model.FoursquareData GetFoursquareUser(string accesstoken)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(@"https://api.foursquare.com/v2/users/self?oauth_token=" + accesstoken + "&v=20121116");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            JObject obj = JObject.Parse(new StreamReader(response.GetResponseStream()).ReadToEnd());
            if (obj.SelectToken("response") == null)
            {
                return null;
            }

            if (obj.SelectToken("response").SelectToken("user") == null)
            {
                return null;
            }

            JToken user = obj.SelectToken("response").SelectToken("user");

            Model.FoursquareData data = new Model.FoursquareData();

           

            if(user.SelectToken("id")==null)
            {
                return null;
            }

            data.AccessToken = accesstoken;

            data.FoursquareID = Convert.ToInt64(user.SelectToken("id").ToString());

            if (user.SelectToken("firstName") != null)
            {

                data.FirstName = user.SelectToken("firstName").ToString();
            }
            if (user.SelectToken("lastName") != null)
            {
                data.LastName = user.SelectToken("lastName").ToString();

            }

            if (user.SelectToken("photo") != null)
            {
                if (user.SelectToken("photo").SelectToken("prefix") != null)
                {
                    data.PhotoPrefix = user.SelectToken("photo").SelectToken("prefix").ToString();

                }

                if (user.SelectToken("photo").SelectToken("suffix") != null)
                {
                    data.PhotoSuffix = user.SelectToken("photo").SelectToken("suffix").ToString();
                }
            }

            if (user.SelectToken("gender") != null)
            {
                if (user.SelectToken("gender").ToString() == "male")
                {
                    data.Gender = true;
                }
                if (user.SelectToken("gender").ToString() == "female")
                {
                    data.Gender = false;
                }
            }


            if (user.SelectToken("honeCity") != null)
            {
                data.Homecity = user.SelectToken("homeCity").ToString();
            }
            else
            {
                data.Homecity = "";
            }
            if (user.SelectToken("bio") != null)
            {
                data.Bio = user.SelectToken("bio").ToString();
            }

            if (user.SelectToken("contact") != null)
            {
                if (user.SelectToken("contact").SelectToken("phone") != null)
                {
                    data.Phone = user.SelectToken("contact").SelectToken("phone").ToString();
                }
                if (user.SelectToken("contact").SelectToken("email") != null)
                {
                    data.Email = user.SelectToken("contact").SelectToken("email").ToString();
                }

                if (user.SelectToken("contact").SelectToken("twitter") != null)
                {
                    data.Twitter = user.SelectToken("contact").SelectToken("twitter").ToString();
                }

                if (user.SelectToken("contact").SelectToken("facebook") != null)
                {
                    data.Facebook = user.SelectToken("contact").SelectToken("facebook").ToString();
                }

            }

                
            data.DateTimeUpdated = DateTime.Now;

            return data;

        }

        public static Model.FoursquareData ParseFoursquareUser(JToken user)
        {
            

            Model.FoursquareData data = new Model.FoursquareData();



            if (user.SelectToken("id") == null)
            {
                return null;
            }

            

            data.FoursquareID = Convert.ToInt64(user.SelectToken("id").ToString());

            if (user.SelectToken("firstName") != null)
            {

                data.FirstName = user.SelectToken("firstName").ToString();
            }
            if (user.SelectToken("lastName") != null)
            {
                data.LastName = user.SelectToken("lastName").ToString();

            }

            if (user.SelectToken("photo") != null)
            {
                if (user.SelectToken("photo").SelectToken("prefix") != null)
                {
                    data.PhotoPrefix = user.SelectToken("photo").SelectToken("prefix").ToString();

                }

                if (user.SelectToken("photo").SelectToken("suffix") != null)
                {
                    data.PhotoSuffix = user.SelectToken("photo").SelectToken("suffix").ToString();
                }
            }

            if (user.SelectToken("gender") != null)
            {
                if (user.SelectToken("gender").ToString() == "male")
                {
                    data.Gender = true;
                }
                if (user.SelectToken("gender").ToString() == "female")
                {
                    data.Gender = false;
                }
            }


            if (user.SelectToken("honeCity") != null)
            {
                data.Homecity = user.SelectToken("homeCity").ToString();
            }
            if (user.SelectToken("bio") != null)
            {
                data.Bio = user.SelectToken("bio").ToString();
            }

            if (user.SelectToken("contact") != null)
            {
                if (user.SelectToken("contact").SelectToken("phone") != null)
                {
                    data.Phone = user.SelectToken("contact").SelectToken("phone").ToString();
                }
                if (user.SelectToken("contact").SelectToken("email") != null)
                {
                    data.Email = user.SelectToken("contact").SelectToken("email").ToString();
                }

                if (user.SelectToken("contact").SelectToken("twitter") != null)
                {
                    data.Twitter = user.SelectToken("contact").SelectToken("twitter").ToString();
                }

                if (user.SelectToken("contact").SelectToken("facebook") != null)
                {
                    data.Facebook = user.SelectToken("contact").SelectToken("facebook").ToString();
                }

            }


            data.DateTimeUpdated = DateTime.Now;

            return data;
        }

        public static AccountStatus InsertFoursquareData(Guid userId, string accesstoken)
        {
            Core.Cloud.TableStorage.InitializeFoursuqareData();

            if (!Accounts.User.DoesUserExists(userId))
            {
                return AccountStatus.Error;
            }


            Model.FoursquareData fqdata = GetFoursquareData(accesstoken);
            if (!HasFoursquareData(accesstoken))
            {


                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(@"https://api.foursquare.com/v2/users/self?oauth_token=" + accesstoken + "&v=20121116");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                JObject obj = JObject.Parse(new StreamReader(response.GetResponseStream()).ReadToEnd());
                if (obj.SelectToken("response") == null)
                {
                    return AccountStatus.Error;
                }

                if (obj.SelectToken("response").SelectToken("user") == null)
                {
                    return AccountStatus.Error;
                }

                JToken user = obj.SelectToken("response").SelectToken("user");





                if (user.SelectToken("id") == null)
                {
                    return AccountStatus.Error;
                }

                long foursquareId = Convert.ToInt64(user.SelectToken("id").ToString());

                Model.FoursquareData data = GetFoursquareData(foursquareId);

                if (data == null)
                {
                    data = new Model.FoursquareData(foursquareId);
                    data.AccessToken = accesstoken;
                    data.UserID=userId;
                    data = UpdateFoursquareData(data);
                    Model.Table.FoursquareData.Execute(TableOperation.Insert(data));
                }
                else
                {
                    data.AccessToken = accesstoken;
                    data.UserID=userId;
                    Model.Table.FoursquareData.Execute(TableOperation.Replace(data));
                }

                return AccountStatus.AccountCreated;
            }
            else
            {
                //if data exists then update it. :)
                UpdateFoursquareData(fqdata);
                //return the result 
                return AccountStatus.Updated;
            }

        }

        private static Model.FoursquareData GetFoursquareData(long foursquareId)
        {
            return Model.Table.FoursquareData.ExecuteQuery(new TableQuery<Model.FoursquareData>().Where(TableQuery.GenerateFilterCondition("FoursquareID", QueryComparisons.Equal, foursquareId.ToString()))).FirstOrDefault();
        }

       

        public static Guid CreateUserData(Model.FoursquareData data)
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
            profile.Bio = data.Bio;
            profile.DateTimeStamp = DateTime.Now;
            profile.DateTimeUpdated = DateTime.Now;
            profile.FirstName = data.FirstName;
            profile.Gender = (bool)data.Gender;
            profile.LargeProfilePicURL = data.PhotoPrefix + "/original/" + data.PhotoSuffix;
            profile.LastName = data.LastName;
            profile.Name = profile.FirstName + " " + profile.LastName;
            profile.Phone = data.Phone;
            profile.ProfilePicURL = profile.LargeProfilePicURL;
            profile.Birthday = Cloud.TableStorage.GetCloudSupportedDateTime();

            Model.Table.Users.Execute(TableOperation.InsertOrReplace(user));
            Model.Table.UserProfile.Execute(TableOperation.InsertOrReplace(profile));

            

            return user.UserID;
        }

        public static Model.FoursquareData GetFoursquareData(Guid userId)
        {
            return Model.Table.FoursquareData.ExecuteQuery(new TableQuery<Model.FoursquareData>().Where(TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId))).FirstOrDefault();
        }

        public static UserData GetUserData(string accesstoken)
        {
            Guid userId=GetuserId(accesstoken);
            if(userId==null)
            {
                return new UserData();
            }
            return Accounts.User.GetUserData(userId);
        }

        public static Guid GetuserId(string accessTag)
        {
            if (HasFoursquareData(accessTag))
            {
                return GetFoursquareData(accessTag).UserID;
            }

            return Guid.Empty;
        }

        






        public static void UpdateFoursquareFriends(Guid userId)
        {
            Model.FoursquareData data = Core.Accounts.Foursquare.GetFoursquareData(userId);

            if (data == null)
            {
                return;
            }

            if (data.AccessToken == null)
            {
                return;
            }

            //construct the URI






            string uri = @"https://api.foursquare.com/v2/users/self/friends?oauth_token=" + data.AccessToken + "&v=20130129";



            //get the list of venues from foursquare. 
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            JObject obj = JObject.Parse(new StreamReader(response.GetResponseStream()).ReadToEnd());

            //save it to the database, if Its not there already. 



            //checks :) 
            if (obj.SelectToken("response") == null)
            {
                return;
            }

            if (obj.SelectToken("response").SelectToken("friends") == null)
            {
                return;
            }

            int count = Convert.ToInt32(obj.SelectToken("response").SelectToken("friends").SelectToken("count").ToString());
            int total = 0;
            bool iterate = true;
           
            while (iterate)
            {



                List<JToken> friends = obj.SelectToken("response").SelectToken("friends").SelectToken("items").ToList();

                foreach (JToken user in friends)
                {
                    //check if this user exists.
                    long foursquareId=Convert.ToInt64(user.SelectToken("id").ToString());
                    Model.FoursquareData usr = GetFoursquareData(foursquareId);

                    if (usr == null)
                    {
                        usr = ParseFoursquareUser(user);

                        if(usr==null)
                        {
                            continue;
                        }

                        usr.DateTimeStamp = DateTime.Now;
                        usr.DateTimeUpdated = DateTime.Now;
                        Model.Table.FoursquareData.Execute(TableOperation.Insert(usr));
                        
                    }

                    //add a relationship.

                    Model.FoursquareRelationships relationship1 = Model.Table.FoursquareRelationships.ExecuteQuery(new TableQuery<Model.FoursquareRelationships>().Where(TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("FoursquareUserID1", QueryComparisons.Equal, usr.FoursquareID.ToString()),TableOperators.And,  TableQuery.GenerateFilterCondition("FoursquareUserID2", QueryComparisons.Equal, data.FoursquareID.ToString())
                        ))).FirstOrDefault();


                    Model.FoursquareRelationships relationship2 = Model.Table.FoursquareRelationships.ExecuteQuery(new TableQuery<Model.FoursquareRelationships>().Where(TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("FoursquareUserID2", QueryComparisons.Equal, usr.FoursquareID.ToString()), TableOperators.And, TableQuery.GenerateFilterCondition("FoursquareUserID1", QueryComparisons.Equal, data.FoursquareID.ToString())
                        ))).FirstOrDefault();


                    if (relationship1 == null)
                    {
                        relationship1 = new Model.FoursquareRelationships();
                        relationship1.DateTimeStamp = DateTime.Now;
                        relationship1.FoursquareUser1ID = usr.FoursquareID;
                        relationship1.FoursquareUser2ID = data.FoursquareID;

                        Model.Table.FacebookRelationships.Execute(TableOperation.Insert(relationship1));
                    }


                    if (relationship2 == null)
                    {
                        relationship2 = new Model.FoursquareRelationships();
                        relationship2.DateTimeStamp = DateTime.Now;
                        relationship2.FoursquareUser2ID = usr.FoursquareID;
                        relationship2.FoursquareUser1ID = data.FoursquareID;

                        Model.Table.FacebookRelationships.Execute(TableOperation.Insert(relationship2));
                    }

                }



                //reiterate 
                if (count < 500)
                {
                    iterate = false;
                }

                //reietrate again :)
                if (iterate)
                {
                    total += count;
                    uri = @"https://api.foursquare.com/v2/users/self/friends?oauth_token=" + data.AccessToken + "&v=20130129&offset=" + total;
                    //get the list of venues from foursquare. 
                    request = (HttpWebRequest)HttpWebRequest.Create(uri);
                    response = (HttpWebResponse)request.GetResponse();
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                    obj = JObject.Parse(new StreamReader(response.GetResponseStream()).ReadToEnd());
                    count = Convert.ToInt32(obj.SelectToken("response").SelectToken("firends").SelectToken("count").ToString());
                }
            }

            


        }

        internal static Model.FoursquareData GetFoursquareDataByEmail(string email)
        {
            Core.Cloud.TableStorage.InitializeFoursuqareData();

            TableQuery<Model.FoursquareData> FoursquareData = new TableQuery<Model.FoursquareData>().Where(
            TableQuery.GenerateFilterCondition("Email", QueryComparisons.Equal, email));

            IEnumerable<Model.FoursquareData> data = Model.Table.FoursquareData.ExecuteQuery(FoursquareData);

            return data.FirstOrDefault();
        }
    }
}