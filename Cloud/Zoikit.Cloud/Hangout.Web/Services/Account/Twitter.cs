using Hangout.Web.Core.Accounts;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hangout.Web.Services.Account
{
    public class Twitter
    {

        

        public Objects.Accounts.TwitterData GetTwitterData(Guid userId, string zat)
        {
            try
            {
                if (Core.Accounts.User.IsValid(userId, zat))
                {
                    Core.Cloud.TableStorage.InitializeTwitterDataTable();

                    Model.TwitterData data = Core.Accounts.Twitter.GetTwitterData(userId);
                    if (data == null)
                    {
                        return null;
                    }

                    return new Objects.Accounts.TwitterData { AboutMe = data.Description, AccessToken = data.AccessToken, AccessTokenSecret = data.AccessTokenSecret, DateTimeAdded = data.DateTimeStamp, DateTimeUpdated = data.DateTimeUpdated, FollowersCount = data.FollowersCount, FollowingCount = data.FollowingCount, Link = data.URL, Location = data.Location, ProfilePicURL = data.ProfileImageURL, ScreenName = data.ScreenName, TimeZone = data.TimeZone, TwitterID = data.TwitterID, UserID = data.UserID };
                }

                throw new UnauthorizedAccessException();
            }
            catch (Exception e)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, e);
                return null;
            }
        }

        public Core.Accounts.AccountStatus RegisterUser(string accesstoken, string accessTagSecret)
        {
            AccountStatus status = Core.Accounts.Twitter.InsertTwitterData(accesstoken, accessTagSecret);
            Model.UserProfile profile = Core.Accounts.Twitter.GetUserData(accesstoken, accessTagSecret).Profile;
            if (status == AccountStatus.AccountCreated)
            {
               

                profile.ProfilePicURL = Core.Accounts.User.GetRandomAvatar();

                Model.Table.UserProfile.Execute(TableOperation.InsertOrReplace(profile));

            }

            Web.Core.Cloud.Queue.AddMessage("TwitterService:" + profile.UserID);

            return status;
        }


        public AccountStatus InsertTwitterData(Guid userId, string accesstoken, string accesstokensecret, string zat)
        {
            try
            {
                if (Core.Accounts.User.IsValid(userId, zat))
                {
                    AccountStatus status = Core.Accounts.Twitter.InsertTwitterData(userId, accesstoken, accesstokensecret);
                    Model.UserProfile profile = Core.Accounts.Twitter.GetUserData(accesstoken, accesstokensecret).Profile;
                    if (status == AccountStatus.AccountCreated)
                    {
                        

                        profile.ProfilePicURL = Core.Accounts.User.GetRandomAvatar();

                        Model.Table.UserProfile.Execute(TableOperation.InsertOrReplace(profile));
                    }

                    Web.Core.Cloud.Queue.AddMessage("TwitterService:" + profile.UserID);

                    return status;
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception e)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, e);
                return AccountStatus.Error;
            }

        }

       


        public Services.Objects.Accounts.UserData GetUserData(string accesstoken, string accessTagSecret)
        {
            try
            {

                return Account.ConvertToUserData(Core.Accounts.Twitter.GetUserData(accesstoken, accessTagSecret));

            }
            catch (Exception e)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(Guid.Empty, Web.Core.ClientType.WindowsAzure, e);
                return null;
            }
        }


    }
}
