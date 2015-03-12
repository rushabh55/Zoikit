using Hangout.Web.Services.Objects.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Hangout.Services.Windows.Account
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAppAuthentication" in both code and config file together.
    [ServiceContract]
    public interface IAppAuthentication
    {
        [OperationContract]

        AppAuthenticationTag Register(string username, string email, string password, string appId, string appToken);


        [OperationContract]

        AppAuthenticationTag RegisterByID(Guid userId, string username, string email, string password, string appId, string appToken);


        [OperationContract]

        AppAuthenticationTag Login(string username, string password, string appId, string appToken);


        [OperationContract]

        bool ConfirmEmail(Guid userId, string code, string zat);


        [OperationContract]

        bool ChangePassword(Guid userId, string oldPass, string newPass, string appId, string appToken);


        [OperationContract]

        bool ResetPassword(string email, string appId, string appToken);
    }
}
