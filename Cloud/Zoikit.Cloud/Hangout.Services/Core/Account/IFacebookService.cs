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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IFacebookService" in both code and config file together.
    [ServiceContract]
    public interface IFacebookService
    {
        

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "RegisterUser?FbAccessToken={accesstoken}&AppID={appId}&AppToken={appToken}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        AccountStatus RegisterUser(string accesstoken,string appId, string appToken);

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "UpdateFbData?UserID={userId}&FbAccessToken={accesstoken}&ZAT={zat}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        [OperationContract]
        AccountStatus UpdateFacebookData(Guid userId, string accesstoken, string zat);
        
        
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetFbData?UserID={userId}&ZAT={zat}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        Web.Services.Objects.Accounts.FacebookData GetFacebookData(Guid userId, string zat);
        
        
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetUserData?FbAccessToken={accesstoken}&AppID={appId}&AppToken={appToken}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        Web.Services.Objects.Accounts.UserData GetUserData(string accesstoken, string appId, string appToken);
        
        
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "PostFbStatus?FbAccessToken={accesstoken}&Status={Status}&AppID={appId}&AppToken={appToken}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        void PostFacebookStatus(String accessToken, String Status, string appId, string appToken);
    }
}
