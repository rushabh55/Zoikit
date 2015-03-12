using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Hangout.Services.Core.Tag
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TagService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TagService.svc or TagService.svc.cs at the Solution Explorer and start debugging.
    public class TagService : ITagService
    {
        Hangout.Web.Services.Tag.Tag obj = new Web.Services.Tag.Tag();

        public Web.Core.Follow.FollowResult FollowTag(Guid userId, Guid tokenId, Guid cityId, string zat)
        {
            return obj.FollowTag(userId, tokenId, cityId, zat);
        }

        public Web.Core.Follow.FollowResult UnfollowTag(Guid userId, Guid tokenId, Guid cityId, string zat)
        {
            return obj.UnfollowTag(userId, tokenId, cityId, zat);
        }

        public List<Web.Services.Objects.Tag.UserTag> GetTagsFollowed(Guid userId, int take, List<Guid> skipList, Guid cityId, string zat)
        {
            return obj.GetTagsFollowed(userId,take,cityId, skipList, zat);
        }

        public List<Guid> GetTagIDFollowedByUser(Guid userId, int take, List<Guid> skipList, Guid cityId, string zat)
        {
            return obj.GetTagIDFollowed(userId,take,skipList, cityId, zat);
        }


        public List<Web.Services.Objects.Tag.UserTag> GetTagsFollowedByUser(Guid meId, Guid userId, int take, List<Guid> skipList, Guid cityId, string zat)
        {
            return obj.GetTagsFollowed(meId, userId, cityId, take, skipList, zat);
        }



        public Web.Services.Objects.Tag.UserTag GetTagByID(Guid userId, Guid tokenId, Guid cityId, string zat)
        {
            return obj.GetTagByID(userId, tokenId,cityId, zat);
        }

        public List<Web.Services.Objects.Tag.UserTag> GetTagsInBuzz(Guid userId, Guid buzzId, Guid cityId,  string zat)
        {
            return obj.GetTagsInBuzz(userId, buzzId, cityId, zat);
        }

        public Web.Core.Follow.FollowResult FollowTagByName(Guid userId, string tokenname, Guid cityId, string zat)
        {
            return obj.FollowTag(userId, tokenname,cityId, zat);
        }

        public Web.Services.Objects.Tag.UserTag GetTagByName(Guid userId, string name, Guid cityId, string zat)
        {
            return obj.GetTagByName(userId,name,cityId,zat);
        }

    }
}
