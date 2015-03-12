using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Core.Compatibility
{
    public class Compatibility
    {
        public static void UpdateCompatibility(Guid userId)
        {
            //delete all the previous computed compatibility :)

            DeleteCompatibilities(userId);
            System.Diagnostics.Trace.WriteLine("Deleted all the Compatibilities for " + userId);
            //get all the users who are in the radius of 50 Km from that user. 

            List<Guid> userIds = Core.Location.City.GetuserIdsFromUserCity(userId);

            if (userIds != null)
            {

                foreach (Guid friend in userIds)
                {
                    //compute the score of this user :)
                    int score = 0;

                    int NoOfCommonPeopleBeingFollowed = Core.Users.Users.GetNoOfMutualPeopleFollowed(userId, friend);

                    int NoOfCommonCategoriesBeingFollowed = Core.Category.User.GetNoOfMutualCategoriesFollowed(userId, friend);

                    int NoOfCommonTagsBeingFollowed = Core.Tags.User.GetNoOfMutualTagsFollowed(userId, friend);

                    bool? IsFollowing = Core.Users.Follow.IsFollowing(userId, friend);

                    //compute compatibility

                    score += NoOfCommonPeopleBeingFollowed * 10;
                    score += NoOfCommonCategoriesBeingFollowed * 5;
                    score += NoOfCommonTagsBeingFollowed * 5;
                    if (IsFollowing == true)
                    {
                        score += 10;
                    }

                    Model.Compatibility compatibility = new Model.Compatibility();
                    compatibility.User1ID = userId;
                    compatibility.User2ID = friend;
                    compatibility.Score = score;


                    Model.Table.Compatibility.Execute(TableOperation.Insert(compatibility));



                }
                            
            }

            //update user background data
            Web.Core.BackgroundData.UserBackgroundData.UpdateCompatibilityDateTimeStamp(userId);
        }

        private static void DeleteCompatibilities(Guid userId )
        {
            List<Model.Compatibility> list = Model.Table.Compatibility.ExecuteQuery(new TableQuery<Model.Compatibility>().Where(TableQuery.CombineFilters(
            TableQuery.GenerateFilterConditionForGuid("User1ID", QueryComparisons.Equal, userId), TableOperators.Or, TableQuery.GenerateFilterConditionForGuid("User2ID", QueryComparisons.Equal, userId)))).ToList();

            foreach (Model.Compatibility c in list)
            {
                Model.Table.Compatibility.Execute(TableOperation.Delete(c));
            }

            
        }

        public static List<Guid> GetCompatibleUsers(Guid userId)
        {
            List<Guid> userIds = new List<Guid>();
            userIds.AddRange(Model.Table.Compatibility.ExecuteQuery(new TableQuery<Model.Compatibility>().Where(
            TableQuery.GenerateFilterConditionForGuid("User1ID", QueryComparisons.Equal, userId))).Select(o=>o.User2ID).ToList());
            userIds.AddRange(Model.Table.Compatibility.ExecuteQuery(new TableQuery<Model.Compatibility>().Where(
            TableQuery.GenerateFilterConditionForGuid("User2ID", QueryComparisons.Equal, userId))).Select(o => o.User1ID).ToList());
            return userIds;
        }


    }
}