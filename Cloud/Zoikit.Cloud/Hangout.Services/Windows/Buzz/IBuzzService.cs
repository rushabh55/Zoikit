using Hangout.Web.Core.Buzz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Hangout.Services.Windows.Buzz
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IBuzzService" in both code and config file together.
    [ServiceContract]
    public interface IBuzzService
    {
        [OperationContract]
        bool HasNewBuzz(Guid userId, Guid cityId, Guid lastBuzzId, string zat);

        [OperationContract]
        
        List<Web.Services.Objects.Buzz.Buzz> GetBuzz(Guid userId, int take, List<Guid> skipList, double lat, double lon, Guid cityId, string zat);
        [OperationContract]
        
        List<Web.Services.Objects.Buzz.Buzz> GetBuzzFollowed(Guid userId, int take, List<Guid> skipList, string zat);
        [OperationContract]
        
        Web.Core.Follow.FollowResult FollowBuzz(Guid userId, Guid buzzId, string zat);
        [OperationContract]
        
        Web.Core.Follow.FollowResult UnfollowBuzz(Guid userId, Guid buzzId, string zat);
        [OperationContract]
        
        List<Web.Services.Objects.Buzz.Buzz> GetBuzzBefore(Guid buzzId, Guid userId, double lat, double lon, Guid cityId, string zat);
        [OperationContract]
        
        List<Web.Services.Objects.Buzz.Buzz> GetBuzzFollowedByUser(Guid meId, Guid userId, int take, List<Guid> skipList, string zat);
        [OperationContract]

        List<Web.Services.Objects.Buzz.Buzz> GetLocalBuzzByTag(Guid userId, Guid tagId, Guid cityId, int take, List<Guid> skipList, string zat);
        [OperationContract]
        
        Web.Services.Objects.Buzz.Buzz GetBuzzByID(Guid userId, Guid buzzId, string zat);
        [OperationContract]
        
        List<Web.Services.Objects.Buzz.BuzzComment> GetBuzzComments(Guid userId, Guid buzzId, int take, List<Guid> skipList, string zat);
        [OperationContract]
        
        List<Web.Services.Objects.Buzz.BuzzComment> AddBuzzComment(Guid userId, Guid buzzId, string comment,Guid lastCommentId, string zat);
        [OperationContract]
        
        Web.Core.Buzz.BuzzSaveStatus InsertBuzz(Guid userId, string text, double lat, double lon, Guid cityId, string imageURi,string zat);
        [OperationContract]
        
        Web.Core.Buzz.BuzzSaveStatus UpdateBuzz(Guid userId, Guid buzzId, string text, double lat, double lon, Guid cityId, string imageURi, string zat);
        [OperationContract]
        
        List<Web.Services.Objects.Buzz.Buzz> SearchBuzz(Guid userId, string searchtext, int take, List<Guid> skipList, double lat, double lon, Guid cityId, string zat);
        [OperationContract]
        
        Web.Services.Objects.Buzz.Buzz GetLastBuzz(Guid userId, Guid meId, string zat);
        [OperationContract]

        List<Web.Services.Objects.Accounts.User> GetUsersWhoLikeBuzz(Guid userId,int take, List<Guid> skipList, Guid buzzId, string zat);
        [OperationContract]

        List<Web.Services.Objects.Buzz.Buzz> GetUserBuzz(Guid userId, int take, List<Guid> skipList, string zat);
        [OperationContract]
        
        List<Web.Services.Objects.Buzz.Buzz> GetLandingPageBuzz(double lat, double lon, int take, List<Guid> skipList);
        [OperationContract]
        
        List<Hangout.Web.Services.Objects.Accounts.User> GetUsersWhoAmplify(Guid buzzId, int take, List<Guid> skipList, bool amplify, Guid userid, string zat);
        [OperationContract]
        
        AmplifyStatus Amplify(Guid buzzId, Guid userId, bool amplify, string zat);
        [OperationContract]
        
        AmplifyStatus UndoAmplification(Guid buzzId, Guid userId, bool amplify, string zat);
        [OperationContract]
        
        int GetDeAmplifyCount(Guid buzzId, Guid userId, string zat);

        [OperationContract]
        
        int GetAmplifyCount(Guid buzzId, Guid userId, string zat);


         [OperationContract]
        string UploadImage(Guid userid, string zat, byte[] image);

         [OperationContract]
         List<Web.Services.Objects.Buzz.BuzzComment> GetBuzzCommentsBefore(Guid userId, Guid buzzId, Guid lastCommentId, string zat);

    }
}
