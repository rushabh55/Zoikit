using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Hangout.Services.Core.Account
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TwitterService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TwitterService.svc or TwitterService.svc.cs at the Solution Explorer and start debugging.
    public class TwitterService : ITwitterService
    {
        Web.Services.Account.Twitter obj = new Web.Services.Account.Twitter();


        public Web.Core.Accounts.AccountStatus RegisterUser(string accesstoken, string accessTokenSecret, string appId, string appToken)
        {
            if(Web.Core.Apps.Apps.IsValid(appId,appToken))
            {
                return obj.RegisterUser(accesstoken, accessTokenSecret);
            }

            return Web.Core.Accounts.AccountStatus.AppInvalid;
            
        }

        public Web.Core.Accounts.AccountStatus UpdateTwitterData(Guid userId, string accesstoken, string accessTokenSecret, string zat)
        {
            return obj.InsertTwitterData(userId, accesstoken,accessTokenSecret, zat);
        }

        public Web.Services.Objects.Accounts.TwitterData GetTwitterData(Guid userId, string zat)
        {
            return obj.GetTwitterData(userId, zat);


        }

        public Web.Services.Objects.Accounts.UserData GetUserData(string accesstoken, string accessTokenSecret, string appId, string appToken)
        {

            if(Web.Core.Apps.Apps.IsValid(appId,appToken))
            {
                return obj.GetUserData(accesstoken, accessTokenSecret);
            }

            return null;
            
        }

        public void PostTweet(string accesstoken, string accessTokenSecret, string tweet, string appId, string appToken)
        {
            if(Web.Core.Apps.Apps.IsValid(appId,appToken))
            {
                Web.Core.Social.Twitter.PostTweet(accesstoken, accessTokenSecret, tweet);
            }
            
        }
    }
}
