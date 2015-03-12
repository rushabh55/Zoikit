using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Core.Buzz
{
    public class UserBuzzInteract
    {
        public static void AddInteract(Guid buzzId, Guid userId)
        {
            if(!DoesInteractExist(buzzId,userId))
            {
                Model.UserBuzzInteract interact = new Model.UserBuzzInteract(userId, buzzId);

                Model.Table.UserBuzzInteract.Execute(TableOperation.InsertOrReplace(interact));
                

            }
        }

        public static void RemoveInteract(Guid buzzId, Guid userId)
        {
            if (!DoesInteractExist(buzzId, userId))
            {
                Model.UserBuzzInteract interact = Model.Table.UserBuzzInteract.ExecuteQuery(new TableQuery<Model.UserBuzzInteract>().Where(TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, userId.ToString()), TableOperators.And, TableQuery.GenerateFilterConditionForGuid("BuzzID", QueryComparisons.Equal, buzzId)))).FirstOrDefault();

                Model.Table.UserBuzzInteract.Execute(TableOperation.Delete(interact));


            }
        }

        public static bool DoesInteractExist(Guid buzzId, Guid userId)
        {
            Core.Cloud.TableStorage.InitializeUserBuzzInteract();

            if( Model.Table.UserBuzzInteract.ExecuteQuery(new TableQuery<Model.UserBuzzInteract>().Where(TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, userId.ToString()),TableOperators.And,TableQuery.GenerateFilterConditionForGuid("BuzzID", QueryComparisons.Equal, buzzId)))).FirstOrDefault()==null)
            {
                return false;
            }

            return true; 
        }
    }
}