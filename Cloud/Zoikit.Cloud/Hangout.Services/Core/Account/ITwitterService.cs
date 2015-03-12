using Hangout.Web.Core.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Hangout.Services.Core.Account
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITwitterService" in both code and config file together.
    [ServiceContract]
    public interface ITwitterService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "RegisterUser?AccessToken={accesstoken}&AccessTokenSecret={accessTokenSecret}&AppID={appId}&AppToken={appToken}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        AccountStatus RegisterUser(string accesstoken,string accessTokenSecret,string appId,string appToken);
        
        
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "UpdateTwitterData?UserID={userId}&AccessToken={accesstoken}&AccessTokenSecret={accessTokenSecret}&ZAT={zat}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        AccountStatus UpdateTwitterData(Guid userId, string accesstoken,string accessTokenSecret, string zat);
        
        
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetTwitterData?UserID={userId}&ZAT={zat}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        Web.Services.Objects.Accounts.TwitterData GetTwitterData(Guid userId, string zat);
        
        
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetUserData?AccessToken={accesstoken}&AccessTokenSecret={accessTokenSecret}&AppID={appId}&AppToken={appToken}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        Web.Services.Objects.Accounts.UserData GetUserData(string accesstoken, string accessTokenSecret,string appId,string appToken);
        
        
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "PostTweet?AccessToken={accesstoken}&AccessTokenSecret={accessTokenSecret}&Tweet={tweet}&AppID={appId}&AppToken={appToken}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        void PostTweet(string accesstoken, string accessTokenSecret,string tweet,string appId,string appToken);
    }
}
