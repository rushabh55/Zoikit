using Hangout.Web.Core.Buzz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Hangout.Services.Core.Buzz
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IBuzzService" in both code and config file together.
    [ServiceContract]
    public interface IBuzzService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetBuzz?UserID={userId}&ZAT={zat}&Take={take}&Lat={lat}&Lon={lon}&CityID={cityId}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<Web.Services.Objects.Buzz.Buzz> GetBuzz(Guid userId, int take,List<Guid> skipList, double lat,double lon, Guid cityId, string zat);
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetBuzzFollowed?UserID={userId}&ZAT={zat}&Take={take}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<Web.Services.Objects.Buzz.Buzz> GetBuzzFollowed(Guid userId,int take,List<Guid> skipList, string zat);
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "FollowBuzz?UserID={userId}&ZAT={zat}&BuzzID={buzzId}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        Web.Core.Follow.FollowResult FollowBuzz(Guid userId, Guid buzzId, string zat);
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "UnfollowBuzz?UserID={userId}&ZAT={zat}&BuzzID={buzzId}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        Web.Core.Follow.FollowResult UnfollowBuzz(Guid userId, Guid buzzId, string zat);
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetBuzzBefore?UserID={userId}&ZAT={zat}&BuzzID={buzzId}&Lat={lat}&Lon={lon}&CityID={cityId}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<Web.Services.Objects.Buzz.Buzz> GetBuzzBefore(Guid buzzId, Guid userId, double lat,double lon, Guid cityId, string zat);
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetBuzzFollowedByUser?MeID={meId}&UserID={userId}&ZAT={zat}&Take={take}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<Web.Services.Objects.Buzz.Buzz> GetBuzzFollowedByUser(Guid meId, Guid userId,int take,List<Guid> skipList, string zat);
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetLocalBuzzByTag?UserID={userId}&ZAT={zat}&Take={take}&CityID={cityId}&TagID={tagId}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<Web.Services.Objects.Buzz.Buzz> GetLocalBuzzByTag(Guid userId, Guid tagId, Guid cityId, int take, List<Guid> skipList, string zat);
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetBuzzByID?UserID={userId}&ZAT={zat}&BuzzID={buzzId}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        Web.Services.Objects.Buzz.Buzz GetBuzzByID(Guid userId, Guid buzzId, string zat);
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetBuzzComments?UserID={userId}&BuzzID={buzzId}&ZAT={zat}&Take={take}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<Web.Services.Objects.Buzz.BuzzComment> GetBuzzComments(Guid userId, Guid buzzId, int take, List<Guid> skipList, string zat);
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "AddBuzzComments?UserID={userId}&BuzzID={buzzId}&LastCommentID={lastCommentId}&ZAT={zat}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<Web.Services.Objects.Buzz.BuzzComment> AddBuzzComment(Guid userId, Guid buzzId, string comment,Guid lastCommentId, string zat);
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "InsertBuzz?UserID={userId}&ZAT={zat}&Lat={lat}&Lon={lon}&CityID={cityId}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        Web.Core.Buzz.BuzzSaveStatus InsertBuzz(Guid userId, string text, double lat, double lon, Guid cityId, string imageUri, string zat);
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "UpdateBuzz?UserID={userId}&ZAT={zat}&BuzzID={buzzId}&Lat={lat}&Lon={lon}&CityID={cityId}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        Web.Core.Buzz.BuzzSaveStatus UpdateBuzz(Guid userId, Guid buzzId, string text, double lat, double lon, Guid cityId, string imageUri, string zat);
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "SearchBuzz?UserID={userId}&ZAT={zat}&Take={take}&Lat={lat}&Lon={lon}&CityID={cityId}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<Web.Services.Objects.Buzz.Buzz> SearchBuzz(Guid userId, string searchtext, int take, List<Guid> skipList, double lat,double lon,Guid cityId, string zat);
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetLastBuzz?UserID={userId}&ZAT={zat}&MeID={MeId}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        Web.Services.Objects.Buzz.Buzz GetLastBuzz(Guid userId,Guid meId,string zat);
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetUsersWhoLikeBuzz?UserID={userId}&ZAT={zat}&BuzzID={buzzId}&Take={take}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<Web.Services.Objects.Accounts.User> GetUsersWhoLikeBuzz(Guid userId, int take, List<Guid> skipList, Guid buzzId, string zat);
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetUserBuzz?UserID={userId}&ZAT={zat}&Take={take}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<Web.Services.Objects.Buzz.Buzz> GetUserBuzz(Guid userId, List<Guid> skipList, int take,string zat);
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetLandingPageBuzz?Take={take}&Lat={lat}&Lon={lon}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<Web.Services.Objects.Buzz.Buzz> GetLandingPageBuzz(double lat,double lon, int take, List<Guid> skipList);
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetUsersWhoAmplify?BuzzID={buzzId}&Take={take}&Amplify={amplify}&UserID={userId}&ZAT={zat}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<Hangout.Web.Services.Objects.Accounts.User> GetUsersWhoAmplify(Guid buzzId, int take, List<Guid> skipList, bool amplify, Guid userid, string zat);
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "Amplify?BuzzID={buzzId}&Amplify={amplify}&UserID={userId}&ZAT={zat}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        AmplifyStatus Amplify(Guid buzzId, Guid userId, bool amplify, string zat);
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "UndoAmplification?BuzzID={buzzId}&Amplify={amplify}&UserID={userId}&ZAT={zat}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        AmplifyStatus UndoAmplification(Guid buzzId, Guid userId, bool amplify, string zat);
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetDeAmplifyCount?BuzzID={buzzId}&UserID={userId}&ZAT={zat}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        int GetDeAmplifyCount(Guid buzzId, Guid userId, string zat);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetAmplifyCount?BuzzID={buzzId}&UserID={userId}&ZAT={zat}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        int GetAmplifyCount(Guid buzzId, Guid userId, string zat);
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "UploadImage?UserID={userId}&ZAT={zat}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        string UploadImage(Guid userid, string zat, byte[] image);


        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetBuzzCommentsBefore?UserID={userId}&BuzzID={buzzId}&LastCommentID={lastcommentId}&ZAT={zat}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<Web.Services.Objects.Buzz.BuzzComment> GetBuzzCommentsBefore(Guid userId, Guid buzzId, Guid lastCommentId, string zat);


    }
}
