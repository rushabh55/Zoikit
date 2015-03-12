using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Hangout.Services.Core.Account
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "FacebookService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select FacebookService.svc or FacebookService.svc.cs at the Solution Explorer and start debugging.
    public class FacebookService : IFacebookService
    {
        Web.Services.Account.Facebook obj = new Web.Services.Account.Facebook();



        public Web.Core.Accounts.AccountStatus RegisterUser(string accesstoken, string appId, string appToken)
        {
            if (Hangout.Web.Core.Apps.Apps.IsValid(appId, appToken))
            {
                return obj.InsertFacebookData(accesstoken);
            }

            return Web.Core.Accounts.AccountStatus.AppInvalid;
        }

        public Web.Core.Accounts.AccountStatus UpdateFacebookData(Guid userId, string accesstoken, string zat)
        {
            

            return obj.InsertFacebookData(userId, accesstoken, zat);
        }


        public Web.Services.Objects.Accounts.FacebookData GetFacebookData(Guid userId, string zat)
        {
            return obj.GetFacebookData(userId, zat);
        }


        public Web.Services.Objects.Accounts.UserData GetUserData(string accesstoken, string appId, string appToken)
        {
            if (Hangout.Web.Core.Apps.Apps.IsValid(appId, appToken))
            {
                return obj.GetUserData(accesstoken);
            }

            return null;
        }


        public void PostFacebookStatus(string accessToken, string Status, string appId, string appToken)
        {
            if (Hangout.Web.Core.Apps.Apps.IsValid(appId, appToken))
            {
                Web.Core.Social.Facebook.ShareOnFacebook(accessToken, Status);
            }

        }

        
    }
}
