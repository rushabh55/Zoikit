using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Hangout.Services.Windows.Tag
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TagService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TagService.svc or TagService.svc.cs at the Solution Explorer and start debugging.
    public class TagService : ITagService
    {


        Hangout.Web.Services.Tag.Tag obj = new Web.Services.Tag.Tag();

        public Web.Services.Objects.Tag.UserTag GetTagByName(Guid userId, string name, Guid cityId, string zat)
        {
            return obj.GetTagByName(userId, name, cityId, zat);
        }

        public Web.Core.Follow.FollowResult FollowTag(Guid userId, Guid tokenId, Guid cityId, string zat)
        {
            return obj.FollowTag(userId, tokenId, cityId, zat);
        }

        public Web.Core.Follow.FollowResult UnfollowTag(Guid userId, Guid tokenId, Guid cityId, string zat)
        {
            return obj.UnfollowTag(userId, tokenId, cityId, zat);
        }
    }
}
