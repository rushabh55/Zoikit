using Hangout.Web.Core.Accounts;
using Hangout.Web.Services.Objects.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Hangout.Services.Core.Account
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAppAuthentication" in both code and config file together.
    [ServiceContract]
    public interface IAppAuthentication
    {
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "RegisterUser?Username={username}&Email={email}&password={password}&AppID={appId}&AppToken={appToken}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        AppAuthenticationTag Register(string username, string email, string password,string appId,string appToken);


        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "RegisterUserByID?UserID={userid}&Username={username}&Email={email}&password={password}&AppID={appId}&AppToken={appToken}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        AppAuthenticationTag RegisterByID(Guid userId, string username, string email, string password,string appId,string appToken);


        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "LogIn?Username={username}&Password={password}&AppID={appId}&AppToken={appToken}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        AppAuthenticationTag Login(string username, string password, string appId, string appToken);


        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "ConfirmEmail?UserID={userid}&EmailCode={code}&ZAT={zat}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        bool ConfirmEmail(Guid userId, string code, string zat);


        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "ChangePassword?UserID={userId}&OldPassword={oldPass}&NewPassword={newPass}&AppID={appId}&AppToken={appToken}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        bool ChangePassword(Guid userId, string oldPass, string newPass, string appId, string appToken);


        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "ResetPassword?Email={email}&AppID={appId}&AppToken={appToken}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        bool ResetPassword(string email, string appId, string appToken);

        
        
    }
}
