using Hangout.Web.Services.Objects.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Hangout.Services.Windows.Account
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAccountService" in both code and config file together.
    [ServiceContract]
    public interface IAccountService
    {
        [OperationContract]
        bool IsConfirmEmail(Guid userid);
        [OperationContract]
        bool ConfirmEmail(Guid userid, string code);

        [OperationContract]
        void ResendConfirmation(Guid userid);

        [OperationContract]
        
        Status UpdateUserData(Web.Services.Objects.Accounts.UserData userData, string zat);
        [OperationContract]
        
        Web.Services.Objects.Accounts.UserData GetUserData(Guid userId, string zat);
        [OperationContract]
        
        Web.Services.Objects.Accounts.CompleteUserData GetCompleteUserData(string zat);
        [OperationContract]
       
        List<string> GetAvatars(Guid userId, string zat);
        [OperationContract]
        
        string SaveProfileImage(Guid userId, byte[] image, string zat);
        [OperationContract]
        
        void UpdateActivityLog(Guid userId, string zat);

        Status DeleteUser(Guid userId, string zat);
    }
}
