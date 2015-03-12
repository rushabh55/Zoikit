using Hangout.Web.Services.Objects.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Hangout.Services.Windows.Account
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AppAuthentication" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select AppAuthentication.svc or AppAuthentication.svc.cs at the Solution Explorer and start debugging.
    public class AppAuthentication : IAppAuthentication
    {
        Web.Services.Account.HangoutApp obj = new Web.Services.Account.HangoutApp();


        public AppAuthenticationTag Register(string username, string email, string password, string appId, string appToken)
        {
            if (Hangout.Web.Core.Apps.Apps.IsValid(appId, appToken))
            {
                return obj.Register(username, email, password);
            }

            return new AppAuthenticationTag { Status = Web.Core.Accounts.AccountStatus.AppInvalid, Tag = "" };
        }

        public AppAuthenticationTag RegisterByID(Guid userId, string username, string email, string password, string appId, string appToken)
        {
            if (Hangout.Web.Core.Apps.Apps.IsValid(appId, appToken))
            {
                return obj.Register(userId, username, email, password);
            }

            return new AppAuthenticationTag { Status = Web.Core.Accounts.AccountStatus.AppInvalid, Tag = "" };
        }

        public AppAuthenticationTag Login(string username, string password, string appId, string appToken)
        {
            if (Hangout.Web.Core.Apps.Apps.IsValid(appId, appToken))
            {
                return obj.Login(username, password);
            }
            return new AppAuthenticationTag { Status = Web.Core.Accounts.AccountStatus.AppInvalid, Tag = "" };
        }

        public bool ConfirmEmail(Guid userId, string code, string zat)
        {

            return obj.ConfirmEmail(userId, code, zat);
        }

        public bool ChangePassword(Guid userId, string oldPass, string newPass, string appId, string appToken)
        {
            if (Hangout.Web.Core.Apps.Apps.IsValid(appId, appToken))
            {
                return obj.ChangePassword(userId, oldPass, newPass);
            }



            return false;
        }





        public bool ResetPassword(string email, string appId, string appToken)
        {
            if (Hangout.Web.Core.Apps.Apps.IsValid(appId, appToken))
            {
                return obj.ResetPassword(email);
            }
            return false;
        }
    }
}
