using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Hangout.Client.Core.Authentication
{
    public class Accounts
    {
        public static bool LoggedInToFacebook()
        {
            try
            {
                return (Settings.Settings.FacebookData!=null);
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                return false;
            }

        }

        public static bool LoggedInToFoursquare()
        {
            try
            {
                if (Settings.Settings.FoursquareData == null)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                return false;
            }

        }

        public static bool LoggedOffAllAccounts()
        {
            if (!LoggedInToFacebook() && !LoggedInToFoursquare()&&!LoggedInToApp())
            {
                return true;
            }

            return false;
        }

        private static bool LoggedInToApp()
        {
            if (Settings.Settings.UserData == null)
            {
                return false;
            }
            if ((Settings.Settings.UserData.Username == null||String.IsNullOrEmpty(Settings.Settings.UserData.Username)))
            {
                return false;
            }

            return true;
        }

        public static void DeleteUserData()
        {
            try
            {
                Settings.Settings.UserData = null;
                Settings.Settings.UnreadNotifications = 0;
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                
            }
        }

        public static void DeleteFacebookData()
        {
            try
            {
                Settings.Settings.FacebookUserFullName = "";
                Settings.Settings.FacebookUserID = -1;
                Settings.Settings.FacebookAccessToken = "";
                
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
               
            }
        }

        public static void DeleteFoursquareData()
        {
            try
            {
                Settings.Settings.FoursquareData = null;
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);

            }
        }

        


        public static AccountService.UserData Convert(FoursquareService.UserData userData)
        {
            if (userData.UserID == 0)
            {
                return null;
            }
            AccountService.UserData data = new AccountService.UserData();
            data.Age = userData.Age;
            data.Bio = userData.Bio;
            data.Birthday = userData.Birthday;
            data.DateTimeStamp = userData.DateTimeStamp;
            data.DateTimeUpdated = userData.DateTimeUpdated;
            data.DefaultLengthUnits = userData.DefaultLengthUnits;
            data.Email = userData.Email;
            data.FirstName = userData.FirstName;
            data.Gender = userData.Gender;
            data.LargeProfilePicURL = userData.LargeProfilePicURL;
            data.LastName = userData.LastName;
            data.Name = userData.Name;
            data.Phone = userData.Phone;
            data.ProfilePicURL = userData.ProfilePicURL;
            data.RelationshipStatus = userData.RelationshipStatus;
            if (userData.Timezone != null)
            {
                data.Timezone = (float)userData.Timezone;
            }
            data.UserID = userData.UserID;
            data.Username= userData.Username;
            data.ZAT = userData.ZAT.ToString();


            return data;
        }

        internal static AccountService.UserData Convert(FacebookService.UserData userData)
        {
            if (userData.UserID == 0)
            {
                return null;
            }
            AccountService.UserData data = new AccountService.UserData();
            data.Age = userData.Age;
            data.Bio = userData.Bio;
            data.Birthday = userData.Birthday;
            data.DateTimeStamp = userData.DateTimeStamp;
            data.DateTimeUpdated = userData.DateTimeUpdated;
            data.DefaultLengthUnits = userData.DefaultLengthUnits;
            data.Email = userData.Email;
            data.FirstName = userData.FirstName;
            data.Gender = userData.Gender;
            data.LargeProfilePicURL = userData.LargeProfilePicURL;
            data.LastName = userData.LastName;
            data.Name = userData.Name;
            data.Phone = userData.Phone;
            data.ProfilePicURL = userData.ProfilePicURL;
            data.RelationshipStatus = userData.RelationshipStatus;
            if (userData.Timezone != null)
            {
                data.Timezone = (float)userData.Timezone;
            }
            data.UserID = userData.UserID;
            data.Username = userData.Username;
            data.ZAT = userData.ZAT.ToString();


            return data;
        }

        internal static bool LoggedInToZoikit()
        {
            if (Settings.Settings.UserData != null)
            {
                if (String.IsNullOrWhiteSpace(Settings.Settings.UserData.Username))
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

        internal static bool LoggedInToTwitter()
        {
            try
            {
                return (Settings.Settings.TwitterData != null);
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                return false;
            }
        }
    }
}
