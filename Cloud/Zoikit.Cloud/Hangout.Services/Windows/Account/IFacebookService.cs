using Hangout.Web.Core.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Hangout.Services.Windows.Account
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IFacebookService" in both code and config file together.
    [ServiceContract]
    public interface IFacebookService
    {
        [OperationContract]
        
        AccountStatus RegisterUser(string accesstoken, string appId, string appToken);

        
        [OperationContract]
        AccountStatus UpdateFacebookData(Guid userId, string accesstoken, string zat);


        [OperationContract]
       
        Web.Services.Objects.Accounts.FacebookData GetFacebookData(Guid userId, string zat);


        [OperationContract]
        
        Web.Services.Objects.Accounts.UserData GetUserData(string accesstoken, string appId, string appToken);


        [OperationContract]
        
        void PostFacebookStatus(String accessToken, String Status, string appId, string appToken);
    }
}
