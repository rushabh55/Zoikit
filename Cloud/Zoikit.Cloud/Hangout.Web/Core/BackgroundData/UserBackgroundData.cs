using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Hangout.Web.Core.BackgroundData
{
    public class UserBackgroundData
    {
        public static List<Guid> GetOutDatedCompatibilityScoreuserIds(TimeSpan olderThan)
        {
            try
            {
                DateTime minusmin=DateTime.Now-olderThan;

                List<Guid> ids = new List<Guid>();

                List<Guid> userIds = Model.Table.UserBackgroundData.ExecuteQuery(new TableQuery<Model.UserBackgroundData>()).Select(o => o.UserID).ToList();

                ids.AddRange(Model.Table.Users.ExecuteQuery(new TableQuery<Model.User>()).Select(o => o.UserID).Where(o=>!userIds.Contains(o)).ToList());

                ids.AddRange(Model.Table.UserBackgroundData.ExecuteQuery(new TableQuery<Model.UserBackgroundData>().Where(TableQuery.GenerateFilterCondition("CompatibilityCheckDateTimeStamp", QueryComparisons.Equal, ""))).Select(o => o.UserID).ToList());//add all the user which dont have compatibility score. :)
                
                ids.AddRange(Model.Table.UserBackgroundData.ExecuteQuery(new TableQuery<Model.UserBackgroundData>().Where(TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("CompatibilityCheckDateTimeStamp", QueryComparisons.NotEqual, ""),TableOperators.And,TableQuery.GenerateFilterCondition("CompatibilityCheckDateTimeStamp",QueryComparisons.GreaterThan,minusmin.ToString())))).Select(o=>o.UserID).ToList()); //add all the outdated users
                
                return ids;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                return null;
            }


        }

        public static void UpdateCompatibilityDateTimeStamp(Guid userId)
        {
            
            Model.UserBackgroundData data;
            if (!HasBackgroundInfo(userId))
            {
                data = new Model.UserBackgroundData(userId);
                data.UserID=userId;
                
            }
            else
            {
                data = GetBackgroundInfo(userId);
            }

            data.CompatibilityDateTimeStamp = DateTime.Now;//update compatibility date time :)

            Model.Table.UserBackgroundData.Execute(TableOperation.InsertOrReplace(data));
        }

        public static bool HasBackgroundInfo(Guid userId)
        {
            if (Model.Table.UserBackgroundData.ExecuteQuery(new TableQuery<Model.UserBackgroundData>().Where(TableQuery.CombineFilters(TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId),TableOperators.And,TableQuery.GenerateFilterCondition("PartitionKey",QueryComparisons.Equal,Model.UserBackgroundData.GetPartitionKey(userId).ToString())))).FirstOrDefault()==null)
            {
                return false;
            }

            return true;
        }

        public static Model.UserBackgroundData GetBackgroundInfo(Guid userId)
        {
            return Model.Table.UserBackgroundData.ExecuteQuery(new TableQuery<Model.UserBackgroundData>().Where(TableQuery.CombineFilters(TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId), TableOperators.And, TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.UserBackgroundData.GetPartitionKey(userId).ToString())))).FirstOrDefault();
           
        }
    }
}