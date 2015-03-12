using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Hangout.Web;
using Hangout.Web.Core.Accounts;
using Hangout.Web.Services.Objects.Status;

namespace Hangout.Services.Core.Account
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AccountService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select AccountService.svc or AccountService.svc.cs at the Solution Explorer and start debugging.
    public class AccountService : IAccountService
    {

        Web.Services.Account.Account obj = new Web.Services.Account.Account();

       
        public Status UpdateUserData(Web.Services.Objects.Accounts.UserData userData, string zat)
        {
            return obj.UpdateUserData(userData, zat);
        }

        public Web.Services.Objects.Accounts.UserData GetUserData(Guid userId, string zat)
        {
            if (Web.Core.Accounts.User.IsValid(userId, zat))
            {

                return obj.GetUserData(userId);
            }
            return null;

        }

        public Web.Services.Objects.Accounts.CompleteUserData GetCompleteUserData(string zat)
        {
            return obj.GetCompleteUserData(zat);
        }

        public List<string> GetAvatars(Guid userId,string zat)
        {
            return obj.GetAvatars(userId,zat);
        }


        public string SaveProfileImage(Guid userId, byte[] image, string zat)
        {
            return obj.SaveProfileImage(userId, image, zat);
        }

        public void UpdateActivityLog(Guid userId, string zat)
        {
            obj.UpdateActivityLog(userId, zat);
        }


        public Status DeleteUser(Guid userId, string zat)
        {
            return obj.DeleteUser(userId, zat);

        }
    }
}
