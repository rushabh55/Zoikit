using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading;

namespace Hangout.Cloud.Agent.Agents
{
    class FacebookAgent
    {

        public void Run()
        {
            //check compatibility everyday :)
            while (true)
            {
                //get all the users with outdated compatibility
               
                try
                {
                    DateTime minus7 = DateTime.Now - new TimeSpan(7, 0, 0, 0);

                    List<Web.Model.FacebookData> users = Hangout.Web.Model.Table.FacebookData.ExecuteQuery(new TableQuery<Hangout.Web.Model.FacebookData>().Where(TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("UserID", QueryComparisons.NotEqual, Guid.Empty.ToString()), TableOperators.And, TableQuery.GenerateFilterConditionForDate("DateTimeStamp", QueryComparisons.LessThan, minus7)))).ToList();
                        
                        

                    foreach (Web.Model.FacebookData user in users)
                    {

                        Guid i = user.UserID;

                        Core.Facebook.Run(i); //update everything. :)

                        //Restamp updated. :)
                        user.DateTimeUpdated = DateTime.Now;
                        Web.Model.Table.FacebookData.Execute(TableOperation.InsertOrReplace(user));

                    }
                }

                catch (Exception ex)
                {
                    Web.Core.Exceptions.ExceptionReporting.AddAnException(Guid.Empty, Web.Core.ClientType.WindowsAzure, ex);
                }


                Thread.Sleep(new TimeSpan(7, 0, 0, 0)); //sleep for a week :)
            }
        }
    }
}
