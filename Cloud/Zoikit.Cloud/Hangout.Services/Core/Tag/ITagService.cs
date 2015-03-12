using Hangout.Web.Core;
using Hangout.Web.Core.Follow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Hangout.Services.Core.Tag
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITagService" in both code and config file together.
    [ServiceContract]
    public interface ITagService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetTagsFollowed?UserID={userId}&ZAT={zat}&Results={take}&CityID={cityId}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<Web.Services.Objects.Tag.UserTag> GetTagsFollowed(Guid userId, int take,List<Guid> skipList,Guid cityId,string zat);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetTagIDFollowedByUser?UserID={userId}&ZAT={zat}&Results={take}&CityID={cityId}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<Guid> GetTagIDFollowedByUser(Guid userId, int take, List<Guid> skipList, Guid cityId, string zat);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "FollowTag?UserID={userId}&ZAT={zat}&TagID={tokenId}&CityID={cityId}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        FollowResult FollowTag(Guid userId, Guid tokenId, Guid cityId, string zat);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "UnfollowTag?UserID={userId}&ZAT={zat}&TagID={tokenId}&CityID={cityId}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        FollowResult UnfollowTag(Guid userId, Guid tokenId, Guid cityId, string zat);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetTagsFollowedByUser?MeID={meId}&UserID={userId}&ZAT={zat}&Results={take}&CityID={cityId}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<Web.Services.Objects.Tag.UserTag> GetTagsFollowedByUser(Guid meId, Guid userId, int take, List<Guid> skipList, Guid cityId, string zat);

       
       
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetTagsByID?UserID={userId}&ZAT={zat}&TagID={tokenId}&CityID={cityId}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        Hangout.Web.Services.Objects.Tag.UserTag GetTagByID(Guid userId, Guid tokenId, Guid cityId, string zat);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetTagsInBuzz?UserID={userId}&ZAT={zat}&BuzzID={buzzId}&CityID={cityId}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<Web.Services.Objects.Tag.UserTag> GetTagsInBuzz(Guid userId, Guid buzzId, Guid cityId, string zat);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "FollowTagByName?UserID={userId}&ZAT={zat}&TagName={tokenname}&CityID={cityId}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        Web.Core.Follow.FollowResult FollowTagByName(Guid userId, string tokenname, Guid cityId, string zat);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetTagByName?UserID={userId}&ZAT={zat}&TagName={tokenname}&CityID={cityId}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        Web.Services.Objects.Tag.UserTag GetTagByName(Guid userId, string name, Guid cityId, string zat);

    }
}
