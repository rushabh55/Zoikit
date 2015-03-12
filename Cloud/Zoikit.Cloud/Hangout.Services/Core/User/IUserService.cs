using Hangout.Web.Core;
using Hangout.Web.Core.Follow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Hangout.Services.Core.User
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IUserService" in both code and config file together.
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        List<Web.Services.Objects.Accounts.User> GetUsersFollowed(Guid userId, int take, List<Guid> skipList, string zat);

        [OperationContract]
        List<Guid> GetUserIDFollowed(Guid userId, int take, List<Guid> skipList, string zat);

        [OperationContract]
        List<Web.Services.Objects.Accounts.User> GetUsersFollowing(Guid userId, int take, List<Guid> skipList, string zat);

        [OperationContract]
        List<Guid> GetUserIDFollowing(Guid userId, int take, List<Guid> skipList, string zat);

        [OperationContract]
        FollowResult FollowUser(Guid userId, Guid followUserId, string zat);

        [OperationContract]
        FollowResult UnfollowUser(Guid userId, Guid unFollowUserId, string zat);
        
        [OperationContract]
        List<Hangout.Web.Services.Objects.Accounts.User> GetPeopleAroundYou(Guid userId, int count,List<Guid> skipList, string zat);

        [OperationContract]
        Web.Services.Objects.Accounts.User GetUser(Guid userId, Guid getuserId, string zat);

        [OperationContract]
        List<Web.Services.Objects.Accounts.User> GetUsersFollowingByUser(Guid meid, Guid userId, int take, List<Guid> skipList, string zat);

        [OperationContract]
        List<Web.Services.Objects.Accounts.User> GetUsersFollowedByUser(Guid meId, Guid userId, int take, List<Guid> skipList, string zat);

        [OperationContract]
        List<Web.Services.Objects.Accounts.User> GetLocalFollowersByTag(Guid userId, Guid tokenId, Guid cityId,  int take, List<Guid> skipList, string zat);

        [OperationContract]
        List<Hangout.Web.Services.Objects.Accounts.User> GetLocalUsersByCategoryFollowing(Guid userId, Guid categoryId, int take, List<Guid> skipList, string zat);


        [OperationContract]
        List<Web.Services.Objects.Accounts.User> GetPeopleWhoFollowBuzz(Guid userId, Guid buzzId, int take, List<Guid> skipList, string zat);

         [OperationContract]
        List<Web.Services.Objects.Accounts.User> GetPeopleWhoFollowPlace(Guid userId, Guid venueId, int take, List<Guid> skipList, string zat);

         [OperationContract]
         List<Web.Services.Objects.Accounts.User> PeopleNearPlace(Guid userId, Guid venueId, int take, List<Guid> skipList, string zat);

         [OperationContract]
         List<Web.Services.Objects.Accounts.User> GetPeopleCheckedIn(Guid userId, Guid venueId, int take, List<Guid> skipList, string zat);

         [OperationContract]
         Hangout.Web.Services.Objects.User.CompleteUserProfile GetUserCompleteProfile(Guid userId, Guid getprofile, string zat);

         [OperationContract]
         Web.Services.Objects.User.Rep GetUserRep(Guid userId, string zat);
         [OperationContract]
         List<Web.Services.Objects.Accounts.User> SearchPeopleAroundYou(Guid userId, string text, int count, List<Guid> skipList, string zat);
    }
}
