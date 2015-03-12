using Hangout.Web.Core.Accounts;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.Account
{
    public class Foursquare
    {
      


        public AccountStatus InsertFoursquareData(string accesstoken)
        {

            AccountStatus status= Core.Accounts.Foursquare.InsertFoursquareData(accesstoken);
            Model.UserProfile profile = Core.Accounts.Foursquare.GetUserData(accesstoken).Profile;
            if (status == AccountStatus.AccountCreated)
            {
               

                profile.ProfilePicURL = Core.Accounts.User.GetRandomAvatar();

                Model.Table.UserProfile.Execute(TableOperation.InsertOrReplace( profile));
            }


            Web.Core.Cloud.Queue.AddMessage("FoursquareService:" + profile.UserID);

            return status;
        }


        public AccountStatus InsertFoursquareData(Guid userId, string accesstoken,string zat)
        {
            try
            {
                if (Core.Accounts.User.IsValid(userId, zat))
                {
                   AccountStatus status= Core.Accounts.Foursquare.InsertFoursquareData(userId, accesstoken);
                   Model.UserProfile profile = Core.Accounts.Foursquare.GetUserData(accesstoken).Profile;
                    if (status == AccountStatus.AccountCreated)
                    {
                        

                        profile.ProfilePicURL = Core.Accounts.User.GetRandomAvatar();

                        Model.Table.UserProfile.Execute(TableOperation.InsertOrReplace(profile));
                    }

                    Web.Core.Cloud.Queue.AddMessage("FoursquareService:" + profile.UserID);


                    return status;
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch(Exception e)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(Guid.Empty, Web.Core.ClientType.WindowsAzure, e);
                return AccountStatus.Error;
            }
            
        }

        public Services.Objects.Accounts.FoursquareData GetFoursquareData(Guid userId,string zat)
        {
            try
            {
                if (Core.Accounts.User.IsValid(userId, zat))
                {

                    Core.Cloud.TableStorage.InitializeFoursuqareData();

                    Model.FoursquareData data = Core.Accounts.Foursquare.GetFoursquareData(userId);
                    if (data == null)
                    {
                        return null;
                    }

                    return new Objects.Accounts.FoursquareData { AccessToken = data.AccessToken, Bio = data.Bio, DateTimeAdded = data.DateTimeStamp, DateTimeUpdated = data.DateTimeUpdated, Email = data.Email, Facebook = data.Facebook, FirstName = data.FirstName, FoursquareID = data.FoursquareID, Gender = data.Gender, HomeCity = data.Homecity, LastName = data.LastName, Phone = data.Phone, PhotoPrefix = data.PhotoPrefix, PhotoSuffix = data.PhotoSuffix, Twitter = data.Twitter, UserID = data.UserID };
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

                return Account.ConvertToUserData(Core.Accounts.Foursquare.GetUserData(accesstoken));
               
            }
            catch (Exception e)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(Guid.Empty, Web.Core.ClientType.WindowsAzure, e);
                return null;
            }
        }

       
    }
}