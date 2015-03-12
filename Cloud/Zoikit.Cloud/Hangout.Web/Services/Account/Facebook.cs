using Hangout.Web.Core.Accounts;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.Account
{
    public class Facebook
    {
         

        public AccountStatus InsertFacebookData(string accesstoken)
        {
            AccountStatus status= Core.Accounts.Facebook.InsertFacebookData(accesstoken);

            Model.UserProfile profile = Core.Accounts.Facebook.GetUserData(accesstoken).Profile;

            if (status == AccountStatus.AccountCreated)
            {
                Model.Table.UserProfile.Execute(TableOperation.InsertOrReplace(profile));
            }

            Web.Core.Cloud.Queue.AddMessage("FacebookService:" + profile.UserID);

            return status;
        }


        public AccountStatus InsertFacebookData(Guid userId, string accesstoken, string zat)
        {
            try
            {
                if (Core.Accounts.User.IsValid(userId, zat))
                {
                    AccountStatus status= Core.Accounts.Facebook.InsertFacebookData(userId, accesstoken);

                    Model.UserProfile profile = Core.Accounts.Facebook.GetUserData(accesstoken).Profile;

                    if (status == AccountStatus.AccountCreated)
                    {
                        
                        Model.Table.UserProfile.Execute(TableOperation.InsertOrReplace(profile));

                        //insert facebook likes. :) - REPORT TO WORKER :)

                       
                    }

                    Web.Core.Cloud.Queue.AddMessage("FacebookService:" + profile.UserID);

                    return status;
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception e)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(Guid.Empty, Web.Core.ClientType.WindowsAzure, e);
                return AccountStatus.Error;
            }

        }

        public Services.Objects.Accounts.FacebookData GetFacebookData(Guid userId, string zat)
        {
            try
            {
                if (Core.Accounts.User.IsValid(userId, zat))
                {
                    Core.Cloud.TableStorage.InitializeFacebookDataTable();


                    Model.FacebookData data = Core.Accounts.Facebook.GetFacebookData(userId);
                    if (data == null)
                    {
                        return null;
                    }

                    return new Objects.Accounts.FacebookData { Birthday=data.Birthday,Age=data.Age,TimeZone=(float?)data.TimeZone,RelationshipStatus=data.RelationshipStatus,AboutMe=data.About,Phone=data.Phone,ProfilePicURL=data.ProfilePicURL,LargeProfilePicURL=data.LargeProfilePicURL, AccessToken = data.AccessToken,  DateTimeAdded = data.DateTimeStamp, DateTimeUpdated = data.DateTimeUpdated, Email = data.Email,  FirstName = data.FirstName, FacebookID = data.FacebookID, Gender = data.Gender,  LastName = data.LastName,  UserID = data.UserID };
                }

                throw new UnauthorizedAccessException();
            }
            catch (Exception e)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(Guid.Empty, Web.Core.ClientType.WindowsAzure, e);
                return null;
            }

        }


        public Services.Objects.Accounts.UserData GetUserData(string accesstoken)
        {
            try
            {

                return Account.ConvertToUserData(Core.Accounts.Facebook.GetUserData(accesstoken));

            }
            catch (Exception e)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(Guid.Empty, Web.Core.ClientType.WindowsAzure, e);
                return null;
            }
        }
    }
}