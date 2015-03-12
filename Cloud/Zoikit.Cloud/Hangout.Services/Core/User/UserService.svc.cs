using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Hangout.Services.Core.User
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UserService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select UserService.svc or UserService.svc.cs at the Solution Explorer and start debugging.
    public class UserService : IUserService
    {
        Web.Services.Users.User obj = new Web.Services.Users.User();

        public Web.Core.Follow.FollowResult FollowUser(Guid userId, Guid followUserId, string zat)
        {
            return obj.FollowUser(userId, followUserId, zat);
        }

        public Web.Core.Follow.FollowResult UnfollowUser(Guid userId, Guid unFollowUserId, string zat)
        {
            return obj.UnfollowUser(userId, unFollowUserId, zat);
        }

        public List<Web.Services.Objects.Accounts.User> GetUsersFollowed(Guid userId,int take,List<Guid> skipList, string zat)
        {
            return obj.GetUsersFollowed(userId,take,skipList,zat);
        }

        public List<Guid> GetUserIDFollowed(Guid userId,int take,List<Guid> skipList, string zat)
        {
            return obj.GetuserIdFollowed(userId,take,skipList, zat);
        }

        public List<Web.Services.Objects.Accounts.User> GetUsersFollowing(Guid userId, int take, List<Guid> skipList, string zat)
        {
            return obj.GetUsersFollowing(userId,take,skipList, zat);
        }

        public List<Guid> GetUserIDFollowing(Guid userId, int take, List<Guid> skipList, string zat)
        {
            return obj.GetuserIdFollowing(userId,take,skipList, zat);
        }


        public List<Web.Services.Objects.Accounts.User> GetPeopleAroundYou(Guid userId, int count,List<Guid> skipList, string zat)
        {
            return obj.GetPeopleAroundYou(userId, count,skipList, zat);
        }


        public Web.Services.Objects.Accounts.User GetUser(Guid userId, Guid getuserId, string zat)
        {
            return obj.GetUser(userId, getuserId, zat);
        }


        public List<Web.Services.Objects.Accounts.User> GetUsersFollowingByUser(Guid meid, Guid userId, int take, List<Guid> skipList, string zat)
        {
            return obj.GetUsersFollowing(meid, userId, take, skipList, zat);
        }

        public List<Web.Services.Objects.Accounts.User> GetUsersFollowedByUser(Guid meId, Guid userId, int take, List<Guid> skipList, string zat)
        {
            return obj.GetUsersFollowed(meId, userId, take,skipList,zat);
        }


        public List<Web.Services.Objects.Accounts.User> GetLocalFollowersByTag(Guid userId, Guid tagId, Guid cityId,  int take, List<Guid> skipList, string zat)
        {
            return obj.GetLocalFollowersByTag(userId, tagId, cityId,  take, skipList, zat);
        }


        public List<Web.Services.Objects.Accounts.User> GetLocalUsersByCategoryFollowing(Guid userId, Guid categoryId, int take, List<Guid> skipList, string zat)
        {
            return obj.GetLocalUsersByCategoryFollowing(userId, categoryId, take, skipList, zat);
        }

        public List<Web.Services.Objects.Accounts.User> GetPeopleWhoFollowBuzz(Guid userId, Guid buzzId, int take, List<Guid> skipList, string zat)
        {
            return obj.GetPeopleWhoFollowBuzz(userId, buzzId, take, skipList, zat);
        }

        public List<Web.Services.Objects.Accounts.User> GetPeopleWhoFollowPlace(Guid userId, Guid venueId, int take, List<Guid> skipList, string zat)
        {
            return obj.GetPeopleWhoFollowPlace(userId, venueId, take, skipList, zat);
        }


        public List<Web.Services.Objects.Accounts.User> PeopleNearPlace(Guid userId, Guid venueId, int take, List<Guid> skipList, string zat)
        {
            return obj.PeopleNearPlace(userId, venueId, take, skipList, zat);
        }


        public List<Web.Services.Objects.Accounts.User> GetPeopleCheckedIn(Guid userId, Guid venueId, int take, List<Guid> skipList, string zat)
        {
            return obj.GetPeopleCheckedIn(userId, venueId, take, skipList, zat);
        }


        public Web.Services.Objects.User.CompleteUserProfile GetUserCompleteProfile(Guid userId, Guid getprofile, string zat)
        {
            return obj.GetCompleteUserProfile(userId, getprofile, zat);
        }

        public Web.Services.Objects.User.Rep GetUserRep(Guid userId, string zat)
        {
            return obj.GetUserRep(userId,zat);
        }

        public List<Web.Services.Objects.Accounts.User> SearchPeopleAroundYou(Guid userId, string text,int count, List<Guid> skipList,string zat)
        {
            return obj.SearchPeopleAroundYou(userId,text, count,skipList, zat);
        }
    }
}
