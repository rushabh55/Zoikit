using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Hangout.Services.Windows.User
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IUserService" in both code and config file together.
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        Web.Core.Follow.FollowResult FollowUser(Guid userId, Guid followUserId, string zat);
         [OperationContract]
        Web.Core.Follow.FollowResult UnfollowUser(Guid userId, Guid unFollowUserId, string zat);
         [OperationContract]
        List<Web.Services.Objects.Accounts.User> GetUsersFollowing(Guid userId, int take, List<Guid> skipList, string zat);
         [OperationContract]
         List<Web.Services.Objects.Accounts.User> GetUsersFollowed(Guid userId, int take, List<Guid> skipList, string zat);
        [OperationContract]
         Web.Services.Objects.User.CompleteUserProfile GetUserCompleteProfile(Guid userId, Guid getprofile, string zat);


        [OperationContract]
        List<Web.Services.Objects.Accounts.User> GetLocalFollowersByTag(Guid userId, Guid tagId, Guid cityId, int take, List<Guid> skipList, string zat);
        [OperationContract]
        List<Web.Services.Objects.Accounts.User> SearchUsers(Guid userId, string searchtext, int take, List<Guid> skipList, double lat, double lon, Guid cityId, string zat);

    }
}
