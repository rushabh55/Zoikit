using Hangout.Web.Core.Follow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Hangout.Services.Windows.Tag
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITagService" in both code and config file together.
    [ServiceContract]
    public interface ITagService
    {
        [OperationContract]
       
        Web.Services.Objects.Tag.UserTag GetTagByName(Guid userId, string name, Guid cityId, string zat);


        [OperationContract]
       
        FollowResult FollowTag(Guid userId, Guid tokenId, Guid cityId, string zat);

        [OperationContract]
       
        FollowResult UnfollowTag(Guid userId, Guid tokenId, Guid cityId, string zat);
    }
}
