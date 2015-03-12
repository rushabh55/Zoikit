using Hangout.Web.Services.Objects.Status;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Core.Buzz
{
    public class Amplify
    {
        public static int GetAmplifyCount(Guid buzzId)
        {
            Core.Cloud.TableStorage.InitializeAmplifyBuzzTable();

            TableQuery<Model.AmplifyBuzz> buzzQ = new TableQuery<Model.AmplifyBuzz>().Where(TableQuery.CombineFilters(
            TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.AmplifyBuzz.GetPartitionKey(buzzId)),TableOperators.And,TableQuery.GenerateFilterConditionForBool("Amplify", QueryComparisons.Equal, true)));
            return Model.Table.AmplifyBuzz.ExecuteQuery(buzzQ).Count();
        }

        public static int GetDeAmplifyCount(Guid buzzId)
        {
            Core.Cloud.TableStorage.InitializeAmplifyBuzzTable();

            TableQuery<Model.AmplifyBuzz> buzzQ = new TableQuery<Model.AmplifyBuzz>().Where(TableQuery.CombineFilters(
            TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.AmplifyBuzz.GetPartitionKey(buzzId)), TableOperators.And, TableQuery.GenerateFilterConditionForBool("Amplify", QueryComparisons.Equal, false)));
            return Model.Table.AmplifyBuzz.ExecuteQuery(buzzQ).Count();
        }

        

        public static void ReduceDeamplificationCount(Guid buzzId)
        {
            Model.BuzzByID buzz = Core.Buzz.Buzz.GetBuzzByID(buzzId);
            if (buzz != null)
            {
                buzz.DeAmplifyCount--;
                Model.Table.BuzzByID.Execute(TableOperation.InsertOrReplace(buzz));

            }
            else
            {
                return;
            }

            Model.Buzz buzz1 = Core.Buzz.Buzz.GetBuzzByID(buzzId, buzz.CityID);
            if (buzz1 != null)
            {
                buzz1.DeAmplifyCount--;
                Model.Table.Buzz.Execute(TableOperation.InsertOrReplace(buzz1));
            }
            

            Model.BuzzByUser buzz2 = Core.Buzz.Buzz.GetBuzzByUser(buzzId, buzz.UserID);
            if (buzz2 != null)
            {
                buzz2.DeAmplifyCount--;
                Model.Table.BuzzByUser.Execute(TableOperation.InsertOrReplace(buzz1));
            }
            
        }

        public static void ReduceAmplificationCount(Guid buzzId)
        {
            Model.BuzzByID buzz = Core.Buzz.Buzz.GetBuzzByID(buzzId);
            if (buzz != null)
            {
                buzz.AmplifyCount--;
                Model.Table.BuzzByID.Execute(TableOperation.InsertOrReplace(buzz));

            }
            else
            {
                return;
            }

            Model.Buzz buzz1 = Core.Buzz.Buzz.GetBuzzByID(buzzId, buzz.CityID);
            if (buzz1 != null)
            {
                buzz1.AmplifyCount--;
                Model.Table.Buzz.Execute(TableOperation.InsertOrReplace(buzz1));
            }
           
            Model.BuzzByUser buzz2 = Core.Buzz.Buzz.GetBuzzByUser(buzzId, buzz.UserID);
            if (buzz2 != null)
            {
                buzz2.AmplifyCount--;
                Model.Table.BuzzByUser.Execute(TableOperation.InsertOrReplace(buzz1));
            }
           
            
        }

        public static AmplifyStatus Undo(Guid buzzId, Guid userId, bool amplify)
        {
            try
            {
                Core.Cloud.TableStorage.InitializeAmplifyBuzzTable();
                Core.Cloud.TableStorage.InitializeAmplifyBuzzByUserTable();

                if (AlreadyAmplified(userId, buzzId, amplify))
                {
                    //delete the opposite amplificaton. 
                    Model.AmplifyBuzz buzz = GetAmplification(userId, buzzId);
                    Model.Table.AmplifyBuzz.Execute(TableOperation.Delete(buzz));

                    //delete the opposite amplificaton. 
                    Model.AmplifyBuzzByUser bz = GetAmplificationByUser(userId, buzzId);
                    if (bz != null)
                    {
                        Model.Table.AmplifyBuzzByUser.Execute(TableOperation.Delete(bz));
                    }

                    if (amplify)
                    {


                        

                        return AmplifyStatus.AmplificationUndoed;
                    }

                    else
                    {
                       

                        return AmplifyStatus.DeAmplificationUndoed;
                    }
                }

                return AmplifyStatus.NotFound;
            }
            catch (Exception e)
            {
                if (amplify)
                {
                    return AmplifyStatus.AmplificationUndoError;
                }
                else
                {
                    return AmplifyStatus.DeamplificationUndoError;
                }
            }
        }

        public static AmplifyStatus AmplifyBuzz(Guid buzzId, Guid userId, bool amplify)
        {
            try
            {
                Core.Cloud.TableStorage.InitializeAmplifyBuzzTable();
                Core.Cloud.TableStorage.InitializeAmplifyBuzzByUserTable();

                if (!AlreadyAmplified(userId, buzzId, amplify))
                {
                    if (AlreadyAmplified(userId, buzzId))
                    {
                        //delete the opposite amplificaton. 
                        Model.AmplifyBuzz buzz = GetAmplification(userId, buzzId);
                        Model.Table.AmplifyBuzz.Execute(TableOperation.Delete(buzz));

                        //delete the opposite amplificaton. 
                        Model.AmplifyBuzzByUser bz = GetAmplificationByUser(userId, buzzId);
                        Model.Table.AmplifyBuzzByUser.Execute(TableOperation.Delete(bz));
                    }

                    Model.AmplifyBuzz b = new Model.AmplifyBuzz(userId, buzzId, amplify);
                    Model.Table.AmplifyBuzz.Execute(TableOperation.InsertOrReplace(b));


                    Model.AmplifyBuzzByUser b1 = new Model.AmplifyBuzzByUser(userId, buzzId, amplify);
                    Model.Table.AmplifyBuzzByUser.Execute(TableOperation.InsertOrReplace(b1));

                    if (amplify)
                    {
                        

                        return AmplifyStatus.Amplified;
                    }
                    else
                    {
                       

                        return AmplifyStatus.DeAmplified;
                    }

                }

                if (amplify)
                {
                    return AmplifyStatus.AlreadyAmplified;
                }
                else
                {
                    return AmplifyStatus.AlreadyDeAmplified;
                }



            }
            catch (Exception e)
            {
                if (amplify)
                {
                    return AmplifyStatus.AmplificationError;
                }
                else
                {
                    return AmplifyStatus.DeamplificationError;
                }
            }

        }

        private static Model.AmplifyBuzzByUser GetAmplificationByUser(Guid userId, Guid buzzId)
        {
            Core.Cloud.TableStorage.InitializeAmplifyBuzzByUserTable();

            TableQuery<Model.AmplifyBuzzByUser> buzzQ = new TableQuery<Model.AmplifyBuzzByUser>().Where(TableQuery.CombineFilters(
            TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.AmplifyBuzzByUser.GetPartitionKey(userId)), TableOperators.And, TableQuery.GenerateFilterConditionForGuid("BuzzID", QueryComparisons.Equal,buzzId)));
            return Model.Table.AmplifyBuzz.ExecuteQuery(buzzQ).FirstOrDefault();
        }

        public static void IncreaseDeamplificationCount(Guid buzzId)
        {
            Model.BuzzByID buzz = Core.Buzz.Buzz.GetBuzzByID(buzzId);
            if (buzz != null)
            {
                buzz.DeAmplifyCount++;
                Model.Table.BuzzByID.Execute(TableOperation.InsertOrReplace(buzz));

            }
            else
            {
                return;
            }

            Model.Buzz buzz1 = Core.Buzz.Buzz.GetBuzzByID(buzzId, buzz.CityID);
            if (buzz1 != null)
            {
                buzz1.DeAmplifyCount++;
                Model.Table.Buzz.Execute(TableOperation.InsertOrReplace(buzz1));
            }
            

            Model.BuzzByUser buzz2 = Core.Buzz.Buzz.GetBuzzByUser(buzzId, buzz.UserID);
            if (buzz2 != null)
            {
                buzz2.DeAmplifyCount++;
                Model.Table.BuzzByUser.Execute(TableOperation.InsertOrReplace(buzz1));
            }
            
        }

        public static void IncreaseAmplificationCount(Guid buzzId)
        {
            Model.BuzzByID buzz = Core.Buzz.Buzz.GetBuzzByID(buzzId);
            if (buzz != null)
            {
                buzz.AmplifyCount++;
                Model.Table.BuzzByID.Execute(TableOperation.InsertOrReplace(buzz));

            }
            else
            {
                return;
            }

            Model.Buzz buzz1 = Core.Buzz.Buzz.GetBuzzByID(buzzId,buzz.CityID);
            if (buzz1 != null)
            {
                buzz1.AmplifyCount++;
                Model.Table.Buzz.Execute(TableOperation.InsertOrReplace(buzz1));
            }
            

            Model.BuzzByUser buzz2 = Core.Buzz.Buzz.GetBuzzByUser(buzzId, buzz.UserID);
            if (buzz2 != null)
            {
                buzz2.AmplifyCount++;
                Model.Table.BuzzByUser.Execute(TableOperation.InsertOrReplace(buzz1));
            }
            
           
        }

        public static List<Guid> GetUsersWhoAmplify(Guid buzzId, int take, List<Guid> skipList, bool amplify)
        {
            Core.Cloud.TableStorage.InitializeAmplifyBuzzTable();

            if (skipList == null)
            {
                skipList = new List<Guid>();
            }

           

            return Model.Table.AmplifyBuzz.ExecuteQuery(
                        new TableQuery<Model.AmplifyBuzz>().Where(TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.AmplifyBuzz.GetPartitionKey(buzzId)),
                        TableOperators.And, TableQuery.GenerateFilterConditionForBool("Amplify", QueryComparisons.Equal, amplify)))).Where(o => !skipList.Contains(o.UserID)).ToList().Select(o => o.UserID).Take(take).ToList();


        }

        

        public static bool AlreadyAmplified(Guid userId, Guid buzzId)
        {
            if(GetAmplification(userId,buzzId)==null)
            {
                return false;
            }

            return true;
        }

        public static bool AlreadyAmplified(Guid userId, Guid buzzId,bool amplify)
        {
            Model.AmplifyBuzz ab = GetAmplification(userId, buzzId);

            if (ab == null)
                return false;

            if(ab.Amplify==amplify)
            {
                return true;
            }

            return false;
        }

        public static Model.AmplifyBuzz GetAmplification(Guid userid,Guid buzzId)
        {
            Core.Cloud.TableStorage.InitializeAmplifyBuzzTable();

            TableQuery<Model.AmplifyBuzz> buzzQ = new TableQuery<Model.AmplifyBuzz>().Where(TableQuery.CombineFilters(
            TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.AmplifyBuzz.GetPartitionKey(buzzId)), TableOperators.And, TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, userid.ToString())));
            return Model.Table.AmplifyBuzz.ExecuteQuery(buzzQ).FirstOrDefault();
        }



        public static void NotifyAmplifyBuzz(Guid userId, Guid buzzId)
        {
            Core.Cloud.TableStorage.InitializeNotificationsTable();
            Core.Cloud.TableStorage.InitializeNotificationsUserTable();

            List<Guid> userIds = Core.Buzz.Follow.GetUsersWhoLikesBuzz(buzzId);
            userIds.AddRange(Core.Buzz.Amplify.GetUsersWhoAmplify(buzzId, 999999999, new List<Guid>(), true));
            userIds = userIds.Distinct().ToList();
            Model.BuzzByID buzz = Core.Buzz.Buzz.GetBuzzByID(buzzId);
            Model.UserProfile user = Core.Accounts.User.GetUserProfile(userId);
            //now start notifting users. :)
            foreach (Guid id in userIds)
            {

                if (id == userId)
                {
                    continue; //Dont notify the current user who followed the buzz. 
                }

                //remove all the user notifications. :)
                List<Model.Notifications> notifications = Model.Table.Notifications.ExecuteQuery(
                        new TableQuery<Model.Notifications>().Where(TableQuery.CombineFilters(
                        TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId)
                        , TableOperators.And,
                        TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("MetaData", QueryComparisons.Equal, "BuzzAmplified")
                        , TableOperators.And,
                        TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("Param1", QueryComparisons.Equal, buzzId.ToString())
                        , TableOperators.And,
                        TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("Type1", QueryComparisons.Equal, "Buzz")
                        , TableOperators.And,
                        TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.GreaterThan, Model.Notifications.GetPartitionKey(userId)))))))).ToList();




                foreach (Model.Notifications n in notifications)
                {
                    Model.Table.Notifications.Execute(TableOperation.Delete(n));
                }



                List<Guid> temp = new List<Guid>(userIds);

                if (temp.Contains(userId))
                {
                    temp.Remove(userId);
                }

                Guid lastuserId = temp.LastOrDefault();
                Model.UserProfile profile = Core.Accounts.User.GetUserProfile(lastuserId);

                string desc = "";
               
                desc = "<bold> " + profile.Name + " </bold> amplified \"" + buzz.Text + "\"";
                

                Core.Notifications.Notification.AddNotification(userId, "", "", desc, temp, buzzId.ToString(), userId.ToString(), "Buzz", "User", "BuzzAmplified");

            }


            //users notified...  Not get the list of users who follow userId

            List<Guid> following = Core.Users.Follow.GetFollowersList(userId, 9999999, new List<Guid>());



            foreach (Guid i in following)
            {
                if (!userIds.Contains(i))
                {
                    //now notify this user because he wasnt notified earlier. But with a dufferent message. :)

                    string desc = "<bold> " + user.Name + " </bold> amplified the buzz \"" + buzz.Text + "\"";
                    List<Guid> t = new List<Guid>();
                    t.Add(userId);
                    Core.Notifications.Notification.AddNotification(i, "", "", desc, t, buzzId.ToString(), userId.ToString(), "Buzz", "User", "BuzzAmplified");
                }
            }
        }
    }
}