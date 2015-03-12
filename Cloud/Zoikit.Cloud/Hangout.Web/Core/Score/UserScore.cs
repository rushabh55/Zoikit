using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Web;

namespace Hangout.Web.Core.Score
{
    public class UserScore
    {

        //public static void InsertDefaultScoreIfNotExists(Guid userId)
        //{

        //    TableQuery<Model.UserScore> scoreQuery = new TableQuery<Model.UserScore>().Where(TableQuery.CombineFilters(
        //    TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId),TableOperators.And,TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal,userId.ToString())));

        //    if (Model.Table.UserScore.ExecuteQuery(scoreQuery).Count() == 0)
        //    {
        //        InsertDefaultScore(userId);
        //    }
        //}

        //public static Model.UserScore IncreaseUserScore(Guid userId, int scoreInc)
        //{
        //    Model.UserScore score = GetLastScore(userId);

        //    if (score == null)
        //    {
        //        score=InsertDefaultScore(userId);
        //    }

        //    return InsertScore(userId, score.CurrentScore + scoreInc, scoreInc);
        //}

        //public static Model.UserScore DecreaseUserScore(Guid userId, int scoreDec)
        //{
        //    Model.UserScore score = GetLastScore(userId);
        //    if (score.CurrentScore - scoreDec <0)
        //    {
        //        return InsertScore(userId, 0,-scoreDec);
        //    }

        //    return  InsertScore(userId, score.CurrentScore - scoreDec, -scoreDec);

        //}

      

        //public static Model.UserScore InsertDefaultScore(Guid userId)
        //{
        //    Model.UserScore sc = new Model.UserScore();
        //    sc.UserID=userId;
        //    sc.ScoreChange = 0;
        //    sc.CurrentScore = 50;
        //    sc.DateTimeStamp = DateTime.Now;

        //    Model.Table.UserScore.Execute(TableOperation.Insert(sc));

        //    return sc;
        //}

        //public static Model.UserScore InsertScore(Guid userId,int score,int scoreChange)
        //{
        //    Model.UserScore sc=GetLastScore(userId);
        //    bool IF = false; //Insert Flag
        //    if (sc == null)
        //    {
        //        IF = true;
        //        sc = new Model.UserScore();
        //        sc.UserID=userId;
        //        sc.ScoreChange = 0;
        //        sc.CurrentScore = score;

        //    }

        //    sc.ScoreChange = 0;
        //    sc.CurrentScore = score;
        //    sc.DateTimeStamp = DateTime.Now;

        //    if (IF)
        //    {
        //        Model.Table.UserScore.Execute(TableOperation.Insert(sc));

        //    }
        //    else
        //    {
        //        Model.Table.UserScore.Execute(TableOperation.Replace(sc));
        //    }

           

        //    return sc;

        //}

        //public static List<Model.User> GetInActiveUsers()
        //{
        //    DateTime mminus7=DateTime.Now-new TimeSpan(7,0,0,0);
        //    return Core.Accounts.User.GetUsersById( Model.Table.UserScore.ExecuteQuery(new TableQuery<Model.UserScore>().Where(
        //    TableQuery.GenerateFilterConditionForDate("DateTimeStamp", QueryComparisons.LessThan, mminus7))).Select(o=>o.UserID).ToList());
            
            
           
        //}


        
    }
}