using Hangout.Web.Core.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Hangout.Services.Windows.Account
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITwitterService" in both code and config file together.
    [ServiceContract]
    public interface ITwitterService
    {
        [OperationContract]
        AccountStatus RegisterUser(string accesstoken, string accessTokenSecret, string appId, string appToken);


        [OperationContract]
        
        AccountStatus UpdateTwitterData(Guid userId, string accesstoken, string accessTokenSecret, string zat);


        [OperationContract]
        
        Web.Services.Objects.Accounts.TwitterData GetTwitterData(Guid userId, string zat);


        [OperationContract]
        
        Web.Services.Objects.Accounts.UserData GetUserData(string accesstoken, string accessTokenSecret, string appId, string appToken);


        [OperationContract]
        
        void PostTweet(string accesstoken, string accessTokenSecret, string tweet, string appId, string appToken);
    }
}
