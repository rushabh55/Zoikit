using System;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;

namespace Hangout.Client.Settings
{
    public static class Settings
    {

        //events :)

        public static event EventHandler NotificationCountChanged;
        public static event EventHandler MessageCountChanged;

        // Our isolated storage settings
        static IsolatedStorageSettings isolatedStore = IsolatedStorageSettings.ApplicationSettings;

        // The isolated storage key names of our settings
        static  string PushNotificationsKeyName = "PushnotificationsKey";
        static  string FbAccessTokenKeyName = "FBAccessToken";
        static  string FbAccessTokenExpiresKeyName = "FBAccessTokenExpires";
        static string ZATKeyName = "ZatKeyName";
        static  string FacebookUserIDKeyName = "FacebookUserIdKeyName";
        static  string UserKeyName = "UserKeyName";
        static  string UserIdKeyName = "UserID";
        static  string UserDataKeyName = "UserDataKeyName";
        static  string TrackMeKeyname = "TrackMeKeyName";
        static string FirstLaunchKeyname = "FirstLaunchKeyName";
        static string FoursquareDataKeyName = "FqData";
        static string FacebookDataKeyName = "FbData";
        static string CityKeyNAme = "City";
        static string ProfileImgKeyName = "Pic";
        static string ProfileImageURLKey="ImageURL";
        static string TwitterKey = "TK";
        static string InterestFillKey = "IF";
        static string UnreadNotificationsKey = "UN";
        static string UnreadMessageKey = "UM";

        static  int UserIdDefault = -1;
        static UserLocationService.City CityDefault = null;
        static Guid? ZatDefault = null;
        static  string UsernameDefault = "";
        static  long FacebookUserIdDefault = -1;
        static  string PushNotificationDefault = "";
        static  string FbAccessTokenDefault = "";
        static DateTime FbAccessTokenExpiresDefault = DateTime.Now.AddDays(-30); //Datetime value that already expired. This is default.
        static AccountService.UserData UserDataDefault = null;
        static  bool TrackMeDefault = true;
        static BitmapImage ProfilePicDefault = null;
        static bool FirstLaunchDefault = true;
        static int FoursquareUserIdDefault = -1;
        static string FoursquareAccessTokenDefault = "";
        static string FoursquareUserFullNameDefault = "";
        static AccountService.FoursquareData FoursquareDataDefault = null;
        static AccountService.FacebookData FacebookDataDefault = null;
        static string ProfileImageURLDefualt = "";
        static AccountService.TwitterData TwitterDefualt = null;
        static bool InterestFillDefault = false;
        static int UnreadNotificationsDefault = 0;
        static int UnreadMessageDefault = 0;

        /// <summary>
        /// Constructor that gets the application settings.
        /// </summary>
       

        /// <summary>
        /// Update a setting value for our application. If the setting does not
        /// exist, then add the setting.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool AddOrUpdateValue(string key, Object value)
        {
            try
            {
                bool valueChanged = false;

                // If the key exists
                if (isolatedStore.Contains(key))
                {
                    // If the value has changed
                    if (isolatedStore[key] != value)
                    {
                        // Store the new value
                        isolatedStore[key] = value;
                        valueChanged = true;
                    }
                }
                // Otherwise create the key.
                else
                {
                    isolatedStore.Add(key, value);
                    valueChanged = true;
                }

                return valueChanged;
            }
            catch (System.Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                return false;
            }
        }

        /// <summary>
        /// Get the current value of the setting, or if it is not found, set the 
        /// setting to the default setting.
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="Key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static valueType GetValueOrDefault<valueType>(string Key, valueType defaultValue)
        {
            try
            {
                valueType value;

                // If the key exists, retrieve the value.
                if (isolatedStore.Contains(Key))
                {
                    value = (valueType)isolatedStore[Key];
                }
                // Otherwise, use the default value.
                else
                {
                    value = defaultValue;
                }

                return value;
            }
            catch (System.Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                return defaultValue;
            }
        }

        /// <summary>
        /// Save the settings.
        /// </summary>
        public static void Save()
        {
            try
            {

                isolatedStore.Save();
            }
            catch (System.Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                
            }
        }

        /// <summary>
        /// Property to get and set a Push Notification Key.
        /// </summary>
        public static string PushNotificationSetting
        {
            get
            {
                return GetValueOrDefault<string>(PushNotificationsKeyName, PushNotificationDefault);
            }
            set
            {
                AddOrUpdateValue(PushNotificationsKeyName, value);
                Save();
            }
        }

        public static long FacebookUserID
        {
            get
            {
                return GetValueOrDefault<long>(FacebookUserIDKeyName, FacebookUserIdDefault);
            }
            set
            {
                AddOrUpdateValue(FacebookUserIDKeyName, value);
                Save();
            }
        }

        public static int UserID
        {
            get
            {
                return GetValueOrDefault<int>(UserIdKeyName, UserIdDefault);
            }
            set
            {
                AddOrUpdateValue(UserIdKeyName, value);
                Save();
            }
        }



        /// <summary>
        /// Property to get and set Facebook AccessToken
        /// </summary>
        public static string FacebookAccessToken
        {
            get
            {
                return GetValueOrDefault<string>(FbAccessTokenKeyName, FbAccessTokenDefault);
            }
            set
            {
                AddOrUpdateValue(FbAccessTokenKeyName, value);
                Save();
            }
        }
  
        public static string FacebookUserFullName
        {
            get
            {
                return GetValueOrDefault<string>(UserKeyName, UsernameDefault);
            }
            set
            {
                AddOrUpdateValue(UserKeyName, value);
                Save();
            }
        }


        /// <summary>
        /// Property to get and set Facebook Access Token Expires value
        /// </summary>


        public static AccountService.UserData UserData
        {
            get
            {
                return GetValueOrDefault<AccountService.UserData>(UserDataKeyName, UserDataDefault);
            }
            set
            {
                AddOrUpdateValue(UserDataKeyName, value);
                Save();
            }
        }



        public static bool TrackMe
        {
            get
            {
                return GetValueOrDefault<bool>(TrackMeKeyname, TrackMeDefault);
            }
            set
            {
                AddOrUpdateValue(TrackMeKeyname, value);
                Save();
            }
        }

        public static bool FirstLaunch
        {
            get
            {
                return GetValueOrDefault<bool>(FirstLaunchKeyname, FirstLaunchDefault);
            }
            set
            {
                AddOrUpdateValue(FirstLaunchKeyname, value);
                Save();
            }
        }


        public static Guid? ZAT
        {
            get
            {
                return GetValueOrDefault<Guid?>(ZATKeyName,ZatDefault);
            }
            set
            {
                AddOrUpdateValue(ZATKeyName, value);
                Save();
            }
        }





        public static AccountService.FoursquareData FoursquareData
        {
            get
            {
                return GetValueOrDefault<AccountService.FoursquareData>(FoursquareDataKeyName, FoursquareDataDefault);
            }
            set
            {
                AddOrUpdateValue(FoursquareDataKeyName, value);
                Save();
            }
        }

        public static AccountService.FacebookData FacebookData
        {
            get
            {
                return GetValueOrDefault<AccountService.FacebookData>(FacebookDataKeyName, FacebookDataDefault);
            }
            set
            {
                AddOrUpdateValue(FacebookDataKeyName, value);
                Save();
            }
        }

        public static UserLocationService.City City
        {
            get
            {
                return GetValueOrDefault<UserLocationService.City>(CityKeyNAme, CityDefault);
            }
            set
            {
                AddOrUpdateValue(CityKeyNAme, value);
                Save();
            }
        }


        public static string ProfileImageURL
        {
            get
            {
                return GetValueOrDefault<string>(ProfileImageURLKey,ProfileImageURLDefualt);
            }
            set
            {
                AddOrUpdateValue(ProfileImageURLKey, value);
                Save();
            }
        }

        public static string TempProfileImage { get; set; }

        public static AccountService.TwitterData TwitterData
        {
            get
            {
                return GetValueOrDefault<AccountService.TwitterData>(TwitterKey, TwitterDefualt);
            }
            set
            {
                AddOrUpdateValue(TwitterKey, value);
                Save();
            }
        }

        public static bool InterestFill
        {
            get
            {
                return GetValueOrDefault<bool>(InterestFillKey,InterestFillDefault);
            }
            set
            {
                AddOrUpdateValue(InterestFillKey, value);
                Save();
            }
        }

        public static int UnreadNotifications
        {
            get
            {
                return GetValueOrDefault<int>(UnreadNotificationsKey, UnreadNotificationsDefault);
            }
            set
            {
                AddOrUpdateValue(UnreadNotificationsKey, value);
                Save();

                if (NotificationCountChanged != null)
                {
                    NotificationCountChanged(null, new EventArgs());
                }
            }
        }

        public static int UnreadMesssages
        {
            get
            {
                return GetValueOrDefault<int>(UnreadMessageKey, UnreadMessageDefault);
            }
            set
            {
                AddOrUpdateValue(UnreadMessageKey, value);
                Save();

                if (MessageCountChanged != null)
                {
                    MessageCountChanged(null, new EventArgs());
                }
            }
        }

        public static bool UpdatedActivityLog;

    }
}
