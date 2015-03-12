using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Hangout.Services.Windows.User
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

        public List<Web.Services.Objects.Accounts.User> GetUsersFollowed(Guid userId, int take, List<Guid> skipList, string zat)
        {
            return obj.GetUsersFollowed(userId, take, skipList, zat);
        }

        public Web.Services.Objects.User.CompleteUserProfile GetUserCompleteProfile(Guid userId, Guid getprofile, string zat)
        {
            return obj.GetCompleteUserProfile(userId, getprofile, zat);
        }


        public List<Web.Services.Objects.Accounts.User> GetLocalFollowersByTag(Guid userId, Guid tagId, Guid cityId, int take, List<Guid> skipList, string zat)
        {
            return obj.GetLocalFollowersByTag(userId, tagId, cityId, take, skipList, zat);
        }

        public List<Web.Services.Objects.Accounts.User> SearchUsers (Guid userId, string searchtext, int take, List<Guid> skipList, double lat, double lon, Guid cityId, string zat)
        {
            return obj.SearchUsers(userId, searchtext, take, skipList, lat, lon, cityId, zat);
        }




        public List<Web.Services.Objects.Accounts.User> GetUsersFollowing(Guid userId, int take, List<Guid> skipList, string zat)
        {
            return obj.GetUsersFollowing(userId, take, skipList, zat);
        }
    }
}
