using Hangout.Web.Core.Buzz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Hangout.Services.Windows.Buzz
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "BuzzService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select BuzzService.svc or BuzzService.svc.cs at the Solution Explorer and start debugging.
    public class BuzzService : IBuzzService
    {
        Web.Services.Buzz.Buzz obj = new Web.Services.Buzz.Buzz();

        public Web.Services.Objects.Buzz.Buzz GetLastBuzz(Guid userId, Guid meId, string zat)
        {
            return obj.GetLastBuzz(userId, meId, zat);
        }

        public List<Web.Services.Objects.Accounts.User> GetUsersWhoLikeBuzz(Guid userId, int take, List<Guid> skipList, Guid buzzId, string zat)
        {
            return obj.GetUsersInBuzz(userId,take,skipList, buzzId, zat);
        }

        public List<Web.Services.Objects.Buzz.Buzz> GetUserBuzz(Guid userId, int take, List<Guid> skipList, string zat)
        {
            return obj.GetUserBuzz(userId, skipList,take, zat);
        }

        public List<Web.Services.Objects.Buzz.Buzz> GetBuzz(Guid userId, int take, List<Guid> skipList, double lat, double lon, Guid cityId, string zat)
        {
            return obj.GetBuzz(userId, take, skipList, lat, lon, cityId, zat);
        }

        public List<Web.Services.Objects.Buzz.Buzz> SearchBuzz(Guid userId, string searchtext, int take, List<Guid> skipList, double lat, double lon, Guid cityId, string zat)
        {
            return obj.SearchBuzz(userId, searchtext, take, skipList, lat, lon, cityId, zat);
        }

        public List<Web.Services.Objects.Buzz.Buzz> GetBuzzFollowed(Guid userId, int take, List<Guid> skipList, string zat)
        {
            return obj.GetBuzzFollowed(userId, take, skipList, zat);
        }

        public Web.Core.Follow.FollowResult FollowBuzz(Guid userId, Guid buzzId, string zat)
        {
            return obj.FollowBuzz(userId, buzzId, zat);
        }

        public Web.Core.Follow.FollowResult UnfollowBuzz(Guid userId, Guid buzzId, string zat)
        {
            return obj.UnfollowBuzz(userId, buzzId, zat);
        }


        public List<Web.Services.Objects.Buzz.Buzz> GetBuzzBefore(Guid buzzId, Guid userId, double lat, double lon, Guid cityId, string zat)
        {
            return obj.GetBuzzBefore(buzzId, userId, lat, lon, cityId, zat);
        }

        public List<Web.Services.Objects.Buzz.Buzz> GetBuzzFollowedByUser(Guid meId, Guid userId, int take, List<Guid> skipList, string zat)
        {
            return obj.GetBuzzFollowed(meId, userId, take, skipList, zat);
        }

        public List<Web.Services.Objects.Buzz.Buzz> GetLocalBuzzByTag(Guid userId, Guid tagId, Guid cityId, int take, List<Guid> skipList, string zat)
        {
            return obj.GetLocalBuzzByTag(userId, tagId,   cityId,take, skipList, zat);
        }

        public Web.Services.Objects.Buzz.Buzz GetBuzzByID(Guid userId, Guid buzzId, string zat)
        {
            return obj.GetBuzzByID(userId, buzzId, zat);
        }

        public List<Web.Services.Objects.Buzz.BuzzComment> GetBuzzComments(Guid userId, Guid buzzId, int take, List<Guid> skipList, string zat)
        {
            return obj.GetBuzzComments(userId, buzzId, take, skipList, zat);
        }


        public List<Web.Services.Objects.Buzz.BuzzComment> AddBuzzComment(Guid userId, Guid buzzId, string comment, Guid lastCommentId, string zat)
        {
            return obj.AddBuzzComment(userId, buzzId, comment,lastCommentId, zat);
        }

        public List<Web.Services.Objects.Buzz.BuzzComment> GetBuzzCommentsBefore (Guid userId, Guid buzzId,  Guid lastCommentId, string zat)
        {
            return obj.GetBuzzCommentsBefore(userId, buzzId, lastCommentId, zat);
        }

        public Web.Core.Buzz.BuzzSaveStatus InsertBuzz(Guid userId, string text, double lat, double lon, Guid cityId, string imageUri, string accesstoken)
        {
            return obj.InsertBuzz(userId, text, lat, lon, cityId, imageUri, accesstoken);
        }

        public Web.Core.Buzz.BuzzSaveStatus UpdateBuzz(Guid userId, Guid buzzId, string text, double lat, double lon, Guid cityId, string imageUri, string accesstoken)
        {
            return obj.UpdateBuzz(userId, buzzId, text, lat, lon, cityId, imageUri, accesstoken);
        }


        public List<Web.Services.Objects.Buzz.Buzz> GetLandingPageBuzz(double lat, double lon, int take, List<Guid> skipList)
        {
            throw new NotImplementedException();
        }

        public int GetAmplifyCount(Guid buzzId, Guid userId, string zat)
        {
            return obj.GetAmplifyCount(buzzId, userId, zat);
        }

        public int GetDeAmplifyCount(Guid buzzId, Guid userId, string zat)
        {
            return obj.GetDeAmplifyCount(buzzId, userId, zat);


        }

        public AmplifyStatus UndoAmplification(Guid buzzId, Guid userId, bool amplify, string zat)
        {
            return obj.UndoAmplification(buzzId, userId, amplify, zat);
        }

        public AmplifyStatus Amplify(Guid buzzId, Guid userId, bool amplify, string zat)
        {
            return obj.Amplify(buzzId, userId, amplify, zat);




        }

        public List<Hangout.Web.Services.Objects.Accounts.User> GetUsersWhoAmplify(Guid buzzId, int take, List<Guid> skipList, bool amplify, Guid userid, string zat)
        {
            return obj.GetUsersWhoAmplify(buzzId, take, skipList, amplify, userid, zat);
        }


        public string UploadImage(Guid userid, string zat, byte[] image)
        {
            return obj.UploadImage(userid, zat, image);
        }

        public bool HasNewBuzz(Guid userId, Guid cityId, Guid lastBuzzId, string zat)
        {
            return obj.HasNewBuzz(userId, cityId, lastBuzzId, zat);
        }

        
    }
}
