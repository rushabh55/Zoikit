using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.Score
{
    public class Score
    {
        

        //public  Web.Model.UserScore IncreaseUserScore(Guid userId, int scoreInc, string accesstoken)
        //{
        //    try
        //    {
        //        if (Web.Core.Accounts.User.IsValid(userId, accesstoken))
        //        {
        //           Web.Model.UserScore score= Web.Core.Score.UserScore.IncreaseUserScore(userId, scoreInc);
        //           Core.Cloud.Queue.AddMessage("Trophy:" + userId);

        //           return score;
        //        }
        //        else { throw new Web.Core.Exceptions.Account.UnAuthorisedAccess(); }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
        //        return null;
        //    }
        //}

        //public  Web.Model.UserScore DecreaseUserScore(Guid userId, int scoreDec, string accesstoken)
        //{
        //    try
        //    {
        //        if (Web.Core.Accounts.User.IsValid(userId, accesstoken))
        //        {
        //            return Web.Core.Score.UserScore.DecreaseUserScore(userId, scoreDec);
        //        }
        //        else { throw new Web.Core.Exceptions.Account.UnAuthorisedAccess(); }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
        //        return null;
        //    }

        //}

        //public  int? GetLastScore(Guid userId,string zat)
        //{
        //    try
        //    {
        //        if (Web.Core.Accounts.User.IsValid(zat))
        //        {
        //            Web.Model.UserScore score = Web.Core.Score.UserScore.GetLastScore(userId);
        //            if (score != null)
        //            {
        //                return score.CurrentScore;
        //            }

        //            return null;
        //        }
        //        else { throw new Web.Core.Exceptions.Account.UnAuthorisedAccess(); }
                   
                
        //    }
        //    catch (System.Exception ex)
        //    {
        //        Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
        //        return null;
        //    }
        //}

        //public  Web.Model.UserScore InsertDefaultScore(Guid userId, string accesstoken)
        //{
        //    try
        //    {
        //        if (Web.Core.Accounts.User.IsValid(userId, accesstoken))
        //        {
        //            return Web.Core.Score.UserScore.InsertDefaultScore(userId);
        //        }
        //        else { throw new Web.Core.Exceptions.Account.UnAuthorisedAccess(); }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
        //        return null;
        //    }
        //}

        //public  Web.Model.UserScore InsertScore(Guid userId, int score, int scoreChange, string accesstoken)
        //{
        //    try
        //    {
        //        if (Web.Core.Accounts.User.IsValid(userId, accesstoken))
        //        {
        //            return Web.Core.Score.UserScore.InsertScore(userId, score, scoreChange);
        //        }
        //        else { throw new Web.Core.Exceptions.Account.UnAuthorisedAccess(); }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
        //        return null;
        //    }
        //}
    }
}