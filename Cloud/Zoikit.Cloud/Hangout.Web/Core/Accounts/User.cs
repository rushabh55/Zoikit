using Facebook;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;


namespace Hangout.Web.Core.Accounts
{
    public class User
    {

        public static UserData? RegisterUser(string accessTag)
        {
            try
            {

                //if user is registered then update the details else create a new user

                FacebookClient client = new FacebookClient(accessTag);

                string result = client.Get("me").ToString();



                JObject obj = JObject.Parse(result);

                string name = "", link = "", username = "", email = "", relationshipStatus = "",firstname="",lastname="";
                long id = -1;
                float timezone = 0.0f;
                bool? gender=null;
                DateTime? dob = null;

                if (obj.SelectToken("id") != null)
                {
                    id = long.Parse(obj.SelectToken("id").ToString());
                }
                else
                {
                    return new UserData();
                }
                

                Model.User user;
                bool newuser = false,newProfile=false;;

               
               
                user = new Model.User();
                newuser = true;
                


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
                    if(relationshipStatus!="In a relationship"&&relationshipStatus!="Married"&&relationshipStatus!="Engaged")
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
                    if(tmp=="male")
                    {
                        gender=true;
                    }
                    else if (tmp=="female")
                    {
                        gender=false;
                    }
                    else
                    {
                        gender=null;
                    }


                }

                //user.AccessToken = accessTag;
                //user.Activated = true;

                user = new Model.User();

                user.Blocked = false;
                user.DateTimeStamp = DateTime.Now;
                user.DateTimeUpdated = DateTime.Now;
                user.Email = email;
                //user.FacebookuserId = id;
                user.Username = username;

                
                //create a user profile
                Model.UserProfile profile=null;


                if (newuser)
                {
                    profile = new Model.UserProfile();
                    newProfile = true;

                    Model.Table.Users.Execute(TableOperation.Insert(user));


                }
                else
                {

                    Model.Table.Users.Execute(TableOperation.Replace(user));
                    if (user.UserID!=null &&user.UserID!=Guid.Empty)
                    {
                        profile = GetUserProfile(user.UserID);
                    }
                    if (profile == null)
                    {
                        newProfile = true;
                        profile = new Model.UserProfile();
                    }
                    
                }

                profile.UserID = user.UserID;
                profile.Name = name;
                profile.RelationshipStatus = relationshipStatus;
                profile.TimeZone = timezone;
                profile.DateTimeStamp = DateTime.Now;
                profile.DateTimeUpdated = DateTime.Now;
                profile.Gender = (bool)gender;
                profile.FirstName = firstname;
                profile.LastName = lastname;

                if (dob != null)
                {
                    profile.Birthday = (DateTime)dob;
                }

                if (profile.Birthday != null)
                {
                    profile.Age=DateTime.Now.Year-((DateTime)(profile.Birthday)).Year;
                }

                


                WebRequest request = WebRequest.Create(string.Format("https://graph.facebook.com/{0}/picture", 1));
                WebResponse response = request.GetResponse();
                string pictureUrl = response.ResponseUri.ToString();

                request = WebRequest.Create(string.Format("https://graph.facebook.com/{0}/picture?type=large", 1));
                response = request.GetResponse();
                string largepictureUrl = response.ResponseUri.ToString();

                profile.ProfilePicURL = pictureUrl;
                profile.LargeProfilePicURL = largepictureUrl;

                if (newProfile)
                {
                    Model.Table.UserProfile.Execute(TableOperation.Insert(profile));
                }
                else
                {
                    Model.Table.UserProfile.Execute(TableOperation.Replace(profile));
                }

                Core.Cloud.Queue.RefreshFacebookLikes(user.UserID);
                

                return new UserData { Profile=GetUserProfile(profile.UserID),User=GetUser(user.UserID) };
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;

            }


        }

        public static Model.UserProfile GetUserProfile(Guid userId)
        {



            return GetUserProfileByuserId(userId);

            
            
        }

        private static Model.UserProfile GetUserProfileByuserId(Guid userId)
        {

            Core.Cloud.TableStorage.InitializeUserProfileTable();


            Model.UserProfile profile = Model.Table.UserProfile.ExecuteQuery(new TableQuery<Model.UserProfile>().Where(TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.UserProfile.GetPartitionKey(userId)),TableOperators.And,
                TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, userId.ToString())))).FirstOrDefault();
            return profile;
        }

        private static void UpdateProfilePic(Guid userId, string profilePic)
        {
            Core.Cloud.TableStorage.InitializeUserProfileTable();

            Model.UserProfile profile = GetUserProfileByuserId(userId);

            if (profile != null)
            {
                profile.ProfilePicURL = profilePic;
                Model.Table.UserProfile.Execute(TableOperation.Replace(profile));
            }
        }

        private static string GetFacebookProfilePic(Guid userId)
        {
            Model.FacebookData fd = Facebook.GetFacebookData(userId);

            if (fd != null)
            {
                string at = fd.AccessToken;

                fd = Facebook.GetFacebookUser(at);

                return fd.ProfilePicURL;

            }

            return null;
        }

       

        
        public static Model.User GetUser(Guid userId)
        {
            Model.User user = GetUserByID(userId);
            return user;
        }

        private static Model.User GetUserByID(Guid userId)
        {
            Core.Cloud.TableStorage.InitializeUsersTable();


            Model.User user = Model.Table.Users.ExecuteQuery(
                new TableQuery<Model.User>().Where(
                TableQuery.CombineFilters(
                TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("PartitionKey",QueryComparisons.Equal, Model.User.GetPartitionKey(userId)),TableOperators.And,
                TableQuery.GenerateFilterConditionForGuid("UserID",QueryComparisons.Equal, userId)),TableOperators.And,
                TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, Model.User.GetRowKey(userId))))).FirstOrDefault();
            return user;
        }

        public static UserData GetUserData(Guid userId)
        {
            UserData data = new UserData();
            data.User = GetUser(userId);
            data.Profile = GetUserProfile(userId);
            return data;
        }

        public static void UpdateUserData(UserData userData)
        {
            UpdateUserProfile(userData.Profile);
        }

        public static void UpdateUser(Model.User user)
        {
            if (user.UserID != null&&user.UserID!=Guid.Empty)
            {
                Model.User u = GetUser(user.UserID);
                
                //copy all the properties except id :)
                //u.AccessToken = user.AccessToken;
                u.Activated = user.Activated;
                u.Blocked = user.Blocked;
                u.DateTimeStamp = user.DateTimeStamp;
                u.DateTimeUpdated = DateTime.Now;
                u.Email = user.Email;
                
                //u.FacebookuserId = user.FacebookuserId;
                
                u.RoleID = user.RoleID;
                u.SessionKey = user.SessionKey;
                u.Username = user.Username;


                Model.Table.Users.Execute(TableOperation.Replace(u));
            }
        }

        public static void UpdateUserProfile(Model.UserProfile profile)
        {
            if (profile.UserID != null&&profile.UserID!=Guid.Empty)
            {
                Model.UserProfile u = GetUserProfile(profile.UserID);
                bool flag = false;
                if(u==null)
                {
                    flag = true;
                    u = new Model.UserProfile();
                    u.UserID = profile.UserID;
                 
                }

                //copy profile :)
                
                    u.Age = DateTime.Now.Year - profile.Birthday.Year;


                    u.Age = profile.Age;
                
                u.Bio = profile.Bio;
                u.Birthday = profile.Birthday;
                u.DateTimeUpdated = DateTime.Now;
                u.FirstName = profile.FirstName;
                u.LastName = profile.LastName;
                u.Gender = profile.Gender;
                u.Name = profile.Name;
                u.Phone = profile.Phone;
                u.ProfilePicURL = profile.ProfilePicURL;
                u.RelationshipStatus = profile.RelationshipStatus;
                u.TimeZone = profile.TimeZone;
                u.DefaultLengthUnits = profile.DefaultLengthUnits;

                if (flag)
                {
                    u.DateTimeStamp = DateTime.Now;
                    Model.Table.UserProfile.Execute(TableOperation.Insert(u));
                }
                else
                {
                    Model.Table.UserProfile.Execute(TableOperation.Replace(u));
                }


                
            }
        }

        public static List<UserData> GetUserData(List<Guid> userIds)
        {
            List<UserData> data = new List<UserData>();

            foreach (Guid id in userIds)
            {
                data.Add(GetUserData(id));
            }

            return data;
        }

        public static bool IsValid(string zat)
        {
            Core.Cloud.TableStorage.InitializeUsersTable();

            if (Model.Table.Users.ExecuteQuery(new TableQuery<Model.User>().Where(TableQuery.GenerateFilterCondition("ZAT", QueryComparisons.Equal, zat.ToString()))).FirstOrDefault() == null)
            {
                return false;
            }

            return true;
        }

        public static bool IsValid(Guid userId,string zat)
        {

            Core.Cloud.TableStorage.InitializeUsersTable();

            if(String.IsNullOrWhiteSpace(zat))
            {
                return false;
            }
            Guid a;
            if (Guid.TryParse(zat, out a))
            {


                TableQuery<Model.User> user = new TableQuery<Model.User>().Where(TableQuery.CombineFilters(
                TableQuery.CombineFilters(
            TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.User.GetPartitionKey(userId)), TableOperators.And,
                TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, userId.ToString())), TableOperators.And , TableQuery.GenerateFilterCondition("ZAT", QueryComparisons.Equal, zat.ToString())));

                IEnumerable<Model.User> data = Model.Table.Users.ExecuteQuery(user);

                if (data.FirstOrDefault() == null)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            

            return true;
        }

        public static bool DoesUserExists(Guid userId)
        {
            Core.Cloud.TableStorage.InitializeUsersTable();

            TableQuery<Model.User> user = new TableQuery<Model.User>().Where(
                TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId));

            IEnumerable<Model.User> data = Model.Table.Users.ExecuteQuery(user);

            if (data.Count() > 0)
            {
                return true;
            }

            return false;
        }

        public static Guid GetIfUserExists(string email)
        {
            
            Model.User res = GetUserByEmail(email);

            if (res != null)
            {
                return res.UserID;
            }


            Model.FacebookData fbRes = Core.Accounts.Facebook.GetFacebookDataByEmail(email);

            if (fbRes != null)
            {
                return fbRes.UserID;
            }

            Model.TwitterData twRes = Core.Accounts.Twitter.GetTwitterDataByEmail(email);

            if (twRes != null)
            {
                return twRes.UserID;
            }

            Model.FoursquareData fqRes = Core.Accounts.Foursquare.GetFoursquareDataByEmail(email);

            if (fqRes != null)
            {
                return fqRes.UserID;
            }

            return Guid.Empty;
           
        }

        private static Model.User GetUserByEmail(string email)
        {
            //check main table

            Core.Cloud.TableStorage.InitializeUsersTable();

            TableQuery<Model.User> user = new TableQuery<Model.User>().Where(
            TableQuery.GenerateFilterCondition("Email", QueryComparisons.Equal, email));

            IEnumerable<Model.User> res = Model.Table.Users.ExecuteQuery(user);

            if (res.Count() > 0)
            {
                return res.FirstOrDefault();
            }

            return null;
        }






        internal static Guid GetuserId(string username)
        {
            Core.Cloud.TableStorage.InitializeUsersTable();

            Model.User user = Model.Table.Users.ExecuteQuery(new TableQuery<Model.User>().Where(TableQuery.GenerateFilterCondition("Username", QueryComparisons.Equal, username.ToString()))).FirstOrDefault();
            if (user != null)
            {
                return user.UserID;
            }
            return Guid.Empty;
        }

        internal static Guid GetuserIdByZAT(string zat)
        {
            Core.Cloud.TableStorage.InitializeUsersTable();

            Model.User user = Model.Table.Users.ExecuteQuery(new TableQuery<Model.User>().Where(TableQuery.GenerateFilterCondition("ZAT", QueryComparisons.Equal, zat.ToString()))).FirstOrDefault();
            if (user != null)
            {
                return user.UserID;
            }

            return Guid.Empty;
        }



        internal static string GetZAT(string username)
        {
            Core.Cloud.TableStorage.InitializeUsersTable();

            username = username.ToLower();

            TableQuery<Model.User> user = new TableQuery<Model.User>().Where(
            TableQuery.GenerateFilterCondition("Username", QueryComparisons.Equal, username));

            IEnumerable<Model.User> res = Model.Table.Users.ExecuteQuery(user);

            if (res.Count() > 0)
            {
                return res.FirstOrDefault().ZAT;
            }

            return null;
          
           
        }

        internal static bool DoesUserExists(string username)
        {

            Core.Cloud.TableStorage.InitializeUsersTable();

           TableQuery<Model.User> user = new TableQuery<Model.User>().Where(
           TableQuery.GenerateFilterCondition("Username", QueryComparisons.Equal, username));

            IEnumerable<Model.User> res = Model.Table.Users.ExecuteQuery(user);

            int c = res.Count();

            if (c > 0)
            {
                return true;
            }

            return false;
        }

        internal static List<string> GetAvatars()
        {

            Core.Cloud.TableStorage.InitializeAvatarsTable();

            TableQuery<Model.Avatars> q = new TableQuery<Model.Avatars>();

            IEnumerable<Model.Avatars> res = Model.Table.Avatars.ExecuteQuery(q);

            List<string> list = res.Select(o => o.URL).ToList();

            List<string> newList = new List<string>();

            foreach (string x in list)
            {
        
                newList.Add(Settings.Root + x);
            }

            return newList;
        }

        public static string SaveProfileImage(Guid userId,byte[] image)
        {
            MemoryStream streams = new MemoryStream();

              

            // create storage account
            var account = CloudStorageAccount.Parse(Settings.CloudStorageString);

            // create blob client
            CloudBlobClient blobStorage = account.CreateCloudBlobClient();

            CloudBlobContainer container = blobStorage.GetContainerReference("profilepics");
            container.CreateIfNotExists(); // adding this for safety

            string uniqueBlobName = "image_"+userId+".jpg";

            CloudBlockBlob blob = container.GetBlockBlobReference(uniqueBlobName);

            blob.DeleteIfExists(); //delete if the file exists

            blob.Properties.ContentType = "image\\jpeg";


            BlobContainerPermissions containerPermissions = new BlobContainerPermissions();

            // The public access setting explicitly specifies that the container is private, 
            // so that it can't be accessed anonymously.
            containerPermissions.PublicAccess = BlobContainerPublicAccessType.Container;

            //Set the permission policy on the container.
            container.SetPermissions(containerPermissions);


            blob.UploadFromByteArray(image,0,image.Count());

            
            streams.Close();

            string profileUrl=blob.Uri.ToString();

            

            Model.UserProfile profile = Core.Accounts.User.GetUserProfile(userId);

            profile.ProfilePicURL = profileUrl;

            Model.Table.UserProfile.Execute(TableOperation.Replace(profile));

            return profileUrl;
        }

        public static string GetRandomAvatar()
        {
            Core.Cloud.TableStorage.InitializeAvatarsTable();
            return GetRandomMaleAvatar();
        }

        private static string GetRandomMaleAvatar()
        {
            Core.Cloud.TableStorage.InitializeAvatarsTable();
            return Settings.Root+Model.Table.Avatars.ExecuteQuery(new TableQuery<Model.Avatars>().Where(TableQuery.GenerateFilterCondition("AvatarType",QueryComparisons.Equal,"Male"))).ToList().OrderBy(o => new Random().Next(1, 5)).Select(o => o.URL).FirstOrDefault();
        }

        private static string GetRandomFemaleAvatar()
        {
            Core.Cloud.TableStorage.InitializeAvatarsTable();
            return Settings.Root+Model.Table.Avatars.ExecuteQuery(new TableQuery<Model.Avatars>().Where(TableQuery.GenerateFilterCondition("AvatarType",QueryComparisons.Equal,"Female"))).OrderBy(o => new Random().Next(1, 5)).FirstOrDefault();
        }

        internal static void UpdateActivityLog(Guid userId)
        {
            Core.Cloud.TableStorage.InitializeActivityLogTable();
            Model.ActivityLog log = new Model.ActivityLog(userId, DateTime.Now);
            Model.Table.ActivityLog.Execute(TableOperation.Insert(log));
        }

        internal static List<Model.User> GetUsersById(List<Guid> list)
        {
            List<Model.User> users = new List<Model.User>();

            foreach (Guid g in list)
            {
                users.Add(Core.Accounts.User.GetUser(g));
            }


            return users;
        }

        internal static List<Model.UserProfile> GetUserProfilesById(List<Guid> list)
        {
            List<Model.UserProfile> users = new List<Model.UserProfile>();

            foreach (Guid g in list)
            {
                users.Add(Core.Accounts.User.GetUserProfileByuserId(g));
            }


            return users;
            
        }

        internal static void InsertAvatar(string type, string url)
        {
            Core.Cloud.TableStorage.InitializeAvatarsTable();

            Model.Avatars obj = new Model.Avatars(url);
            obj.AvatarType = type;

            Model.Table.Avatars.Execute(TableOperation.Insert(obj));

        }
    }
}