using Hangout.Web.Core.Accounts;
using Hangout.Web.Services.Objects.Accounts;
using Hangout.Web.Services.Objects.Status;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.Account
{
    public class Account
    {

       

        public Services.Objects.Accounts.UserData RegisterUser(string accessTag)
        {
            try
            {
                Core.Accounts.UserData userdata = (Core.Accounts.UserData) Hangout.Web.Core.Accounts.User.RegisterUser(accessTag);
               
                return Convert(userdata);
               

                return null;
            }
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(Guid.Empty, Web.Core.ClientType.WindowsAzure, ex);
                return null;
            }
        }

        private static Objects.Accounts.UserData Convert(Core.Accounts.UserData userdata)
        {
            Objects.Accounts.UserData ud= new Objects.Accounts.UserData { Age = userdata.Profile.Age, Bio = userdata.Profile.Bio, Birthday = userdata.Profile.Birthday, DateTimeStamp = userdata.Profile.DateTimeStamp, DateTimeUpdated = userdata.Profile.DateTimeUpdated, DefaultLengthUnits = userdata.Profile.DefaultLengthUnits, FirstName = userdata.Profile.FirstName,  LargeProfilePicURL = userdata.Profile.LargeProfilePicURL, LastName = userdata.Profile.LastName, Name = userdata.Profile.Name, Phone = userdata.Profile.Phone, ProfilePicURL = userdata.Profile.ProfilePicURL, RelationshipStatus = userdata.Profile.RelationshipStatus, Timezone = (float?)userdata.Profile.TimeZone, UserID = userdata.User.UserID };
            if(userdata.Profile.Gender==null)
            {
                ud.Gender = null;
            }
            else
            {
                if(userdata.Profile.Gender)
                {
                    ud.Gender = true;
                }
                else
                {
                    ud.Gender = false;
                }
            }


            return ud;
        }

        public Status UpdateUserData(Objects.Accounts.UserData userData, string accessTag)
        {
            try
            {
                if (userData != null)
                {
                    if (Web.Core.Accounts.User.IsValid(userData.UserID, accessTag))
                    {
                        //create a user data

                        Model.UserProfile profile = new Model.UserProfile();

                        

                        profile.UserID = userData.UserID;
                        profile.Age = (int)userData.Age;


                        profile.Bio = userData.Bio;

                        profile.ProfilePicURL = userData.ProfilePicURL;
                        profile.Birthday = (DateTime)userData.Birthday;
                        profile.DateTimeUpdated = DateTime.Now;
                        profile.DefaultLengthUnits = userData.DefaultLengthUnits;
                        profile.FirstName = userData.FirstName;
                        
                        if (userData.Gender.HasValue)
                        {
                            if (userData.Gender.Value)
                            {
                                profile.Gender = true;
                            }
                            else
                            {
                                profile.Gender = false;
                            }
                        }
                        
                        profile.LargeProfilePicURL = userData.LargeProfilePicURL;
                        profile.LastName = userData.LastName;
                        profile.Name = userData.Name;
                        profile.Phone = userData.Phone;
                        profile.ProfilePicURL = userData.ProfilePicURL;
                        profile.RelationshipStatus = userData.RelationshipStatus;
                        profile.TimeZone = (float)userData.Timezone;


                        Core.Accounts.UserData UserData = new Core.Accounts.UserData();
                        UserData.Profile = profile;
                        
                        



                        Hangout.Web.Core.Accounts.User.UpdateUserData(UserData);



                        return Status.OK;

                    }
                    else {
                        return Status.InvalidZAT;
                        }

                   
                }

                return Status.Error;
            }
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userData.UserID, Web.Core.ClientType.WindowsAzure, ex);
                return Status.Error;
            }
        }


        public  Web.Services.Objects.Accounts.UserProfile GetUserProfile(Guid userId, string accesstoken)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(accesstoken))
                {
                    Web.Model.UserProfile profile= Hangout.Web.Core.Accounts.User.GetUserProfile(userId);
                    return new Objects.Accounts.UserProfile{Age=(int)profile.Age,Gender=(bool)profile.Gender,RelationshipStatus=profile.RelationshipStatus,User=new Objects.Accounts.User{Name=profile.Name,ProfilePicURL=profile.ProfilePicURL,AboutUs=profile.Bio,UserID=profile.UserID}};

                }
                else { throw new Web.Core.Exceptions.Account.UnAuthorisedAccess(); }
            }
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return null;
            }
        }



        public static Services.Objects.Accounts.UserData ConvertToUserData(Web.Core.Accounts.UserData data)
        {
            if (data.User.UserID == null||data.User.UserID==Guid.Empty)
            {
                return null;
            }
            Services.Objects.Accounts.UserData u = new Objects.Accounts.UserData();
            if (data.Profile != null)
            {
                u.Age = data.Profile.Age;
                u.Bio = data.Profile.Bio;
                u.Birthday = data.Profile.Birthday;
                u.DateTimeStamp = data.Profile.DateTimeStamp;
                u.DateTimeUpdated = data.Profile.DateTimeUpdated;
                u.DefaultLengthUnits = data.Profile.DefaultLengthUnits;
                u.FirstName = data.Profile.FirstName;
                u.Gender = data.Profile.Gender;
                u.LargeProfilePicURL = data.Profile.LargeProfilePicURL;
                u.LastName = data.Profile.LastName;
                u.Name = data.Profile.Name;
                u.Phone = data.Profile.Phone;
                u.ProfilePicURL = data.Profile.ProfilePicURL;
                u.RelationshipStatus = data.Profile.RelationshipStatus;
                u.Timezone = (float)data.Profile.TimeZone;
                

            }
            u.Email = data.User.Email;



            u.DateTimeStamp = data.User.DateTimeStamp;
            u.DateTimeUpdated = data.User.DateTimeUpdated;
            u.UserID = data.User.UserID;
            u.Username = data.User.Username;
            u.ZAT = data.User.ZAT.ToString();


            return u;


        }



        public Objects.Accounts.CompleteUserData GetCompleteUserData(string zat)
        {
            
            Guid id = Core.Accounts.User.GetuserIdByZAT(zat);
            if (id == null)
            {
                return null;
            }
            else
            {
                return GetCompleteUserData(id, zat);
            }
        }

       
        public Objects.Accounts.CompleteUserData GetCompleteUserData(Guid userId,string zat)
        {

            CompleteUserData data=new CompleteUserData();
            if(userId==null)
            {
                return null;
            
            }
            Services.Account.Facebook fb=new Facebook();
            data.FacebookData = fb.GetFacebookData(userId, zat);

            Services.Account.Foursquare fq = new Foursquare();
            data.FoursquareData = fq.GetFoursquareData(userId, zat);

            Services.Account.Twitter t = new Twitter();
            data.TwitterData= t.GetTwitterData(userId, zat);



            data.UserData = GetUserData(userId);

            return data;

            
        }

        public Objects.Accounts.UserData GetUserData(Guid userId)
        {
            return ConvertToUserData(Core.Accounts.User.GetUserData(userId));
        }


        
        public List<string> GetAvatars(Guid userId, string zat)
        {
            if (Web.Core.Accounts.User.IsValid(zat))
            {
                return Core.Accounts.User.GetAvatars();
                

            }
            else { throw new Web.Core.Exceptions.Account.UnAuthorisedAccess(); }
        }

        public string SaveProfileImage(Guid userId, byte[] image, string zat)
        {

            if (Web.Core.Accounts.User.IsValid(zat))
            {
                return Core.Accounts.User.SaveProfileImage(userId, image);


            }
            else { throw new Web.Core.Exceptions.Account.UnAuthorisedAccess(); }
           
        }


        public void UpdateActivityLog(Guid userId, string zat)
        {
            if (Web.Core.Accounts.User.IsValid(userId,zat))
            {
                 Core.Accounts.User.UpdateActivityLog(userId);


            }
            else { throw new Web.Core.Exceptions.Account.UnAuthorisedAccess(); }
        }

        internal static CompactUser GetUser(Guid id)
        {
            CompactUser cu = new CompactUser();
            cu.UserID = id;
            Model.UserProfile profile = Core.Accounts.User.GetUserProfile(id);
            cu.ProfilePicURL = profile.ProfilePicURL;
            cu.Name = profile.Name;

            return cu;

        }



        public Status DeleteUser(Guid userId, string zat)
        {
            try
            {


                if (Web.Core.Accounts.User.IsValid(userId, zat))
                {
                    Model.User user = Core.Accounts.User.GetUser(userId);
                    Model.UserProfile profile = Core.Accounts.User.GetUserProfile(userId);
                    Model.FacebookData fb = Core.Accounts.Facebook.GetFacebookData(userId);
                    Model.TwitterData tw = Core.Accounts.Twitter.GetTwitterData(userId);

                    if (user != null)
                    {
                        Model.Table.Users.Execute(TableOperation.Delete(user));

                    }

                    if (profile != null)
                    {
                        Model.Table.UserProfile.Execute(TableOperation.Delete(profile));
                    }

                    if(fb!=null)
                    {
                        Model.Table.FacebookData.Execute(TableOperation.Delete(fb));
                    }

                    if(tw!=null)
                    {
                        Model.Table.TwitterData.Execute(TableOperation.Delete(tw));
                    }
                }


                return Status.OK;
            }
            catch
            {
                return Status.Error;
            }
        }

        public bool ConfirmEmail(Guid userid, string code)
        {
            Model.User user = Core.Accounts.User.GetUser(userid);
            if (user.EmailConfirmed == null)
                return true;
            if (user.EmailConfirmed)
                return true;

            if(user.EmailConfirmationCode == code)
            {
                user.EmailConfirmed = true;
                Model.Table.Users.Execute(TableOperation.Replace(user));
                return true;
            }

            return false;
        }

        public bool IsConfirmEmail(Guid userid)
        {
            Model.User user = Core.Accounts.User.GetUser(userid);
            if (user.EmailConfirmed == null)
                return true;
            if (user.EmailConfirmed)
                return true;
            return false;
        }

        public void ResendConformationMail(Guid userid)
        {
            Model.User user=Core.Accounts.User.GetUser(userid);
            Core.Mail.Mail.SendRegistrationMail(user.Username, user.Email);
        }
    }
}