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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IFoursquareService" in both code and config file together.
    [ServiceContract]
    public interface IFoursquareService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "RegisterUser?AccessToken={accesstoken}&AppID={appId}&AppToken={appToken}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        AccountStatus RegisterUser(string accesstoken,string appId,string appToken);


        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "UpdateFoursquareData?UserID={userId}&AccessToken={accesstoken}&ZAT={zat}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        AccountStatus UpdateFoursquareData(Guid userId,string accesstoken,string zat);
        
        
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetFoursquareData?UserID={userId}&ZAT={zat}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        Web.Services.Objects.Accounts.FoursquareData GetFoursquareData(Guid userId,string zat);
        
        
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetUserData?AccessToken={accesstoken}&AppID={appId}&AppToken={appToken}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        Web.Services.Objects.Accounts.UserData GetUserData(string accesstoken,string appId,string appToken);

    }
}
