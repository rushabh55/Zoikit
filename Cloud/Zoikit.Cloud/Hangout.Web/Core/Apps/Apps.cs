using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Core.Apps
{
    public class Apps
    {
        public static Model.Apps GetApp(Guid appId)
        {
            return Model.Table.Apps.ExecuteQuery(new TableQuery<Model.Apps>().Where(TableQuery.GenerateFilterCondition("AppID", QueryComparisons.Equal, appId.ToString()))).FirstOrDefault();
        }

        public static bool IsValid(string appId, string appToken)
        {

            Guid id, token;

            if (Guid.TryParse(appId, out id) && Guid.TryParse(appToken, out token))
            {

                Core.Cloud.TableStorage.InitializeAppTable();

                TableQuery<Model.Apps> appQuery = new TableQuery<Model.Apps>().Where(
                    TableQuery.CombineFilters(
                     TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, Model.Apps.GetRowKey(id)), 
                    TableOperators.And,
                     TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.Apps.GetPartitionKey()), 
                    TableOperators.And,
                    TableQuery.GenerateFilterConditionForGuid("AppID", QueryComparisons.Equal, id))), 
                    TableOperators.And,
                    TableQuery.GenerateFilterConditionForGuid("AppToken", QueryComparisons.Equal, token)));

                IEnumerable<Model.Apps> apps = Model.Table.Apps.ExecuteQuery(appQuery);

                if (apps.Count() > 0)
                {
                    return true;

                }
            }

            return false;
        }

        public static Model.Apps AddApp(string name, string description)
        {
            Core.Cloud.TableStorage.InitializeAppTable();

            Model.Apps app = new Model.Apps(name);
            app.AppDescription = description;

            Model.Table.Apps.Execute(TableOperation.Insert(app));

            return app;
        }
    }
}