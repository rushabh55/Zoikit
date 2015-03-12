using Hangout.Web.Core.Follow;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Core.Buzz
{
    public class Follow
    {
        public static FollowResult LikeBuzz(Guid userId, Guid buzzid)
        {
            try
            {
                bool? isfollowing = Liked(userId, buzzid);


                if (isfollowing.HasValue)
                {
                    if (isfollowing.Value)
                    {
                        return FollowResult.AlreadyFollowing;
                    }
                    else
                    {
                        Model.BuzzFollow follow = new Model.BuzzFollow(buzzid,userId);
                        follow.UserID=userId;
                        follow.BuzzID = buzzid;
                        follow.DateTimeStamp = DateTime.Now;
                        Model.Table.BuzzFollow.Execute(TableOperation.InsertOrReplace(follow));

                        //increment count. 

                        return FollowResult.Following;
                    }
                }
                else
                {
                    return FollowResult.Error;
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return FollowResult.Error;
            }


        }

        public static void IncreaseLikeCount(Guid buzzid)
        {

            

            Model.BuzzByID buzz = Core.Buzz.Buzz.GetBuzzByID(buzzid);
            if (buzz != null)
            {
                buzz.LikeCount++;
                Model.Table.BuzzByID.Execute(TableOperation.InsertOrReplace(buzz));

            }
            else
            {
                return;
            }

            Model.Buzz buzz1 = Core.Buzz.Buzz.GetBuzzByID(buzzid, buzz.CityID);
            if (buzz1 != null)
            {
                buzz1.LikeCount++;
                Model.Table.Buzz.Execute(TableOperation.InsertOrReplace(buzz1));
            }


            Model.BuzzByUser buzz2 = Core.Buzz.Buzz.GetBuzzByUser(buzzid, buzz.UserID);
            if (buzz2 != null)
            {
                buzz2.LikeCount++;
                Model.Table.BuzzByUser.Execute(TableOperation.InsertOrReplace(buzz1));
            }
        }

        public static FollowResult UnLikeBuzz(Guid userId, Guid buzzid)
        {
            try
            {
                bool? isfollowing = Liked(userId, buzzid);


                if (isfollowing.HasValue)
                {
                    if (!isfollowing.Value)
                    {
                        return FollowResult.AlreadyUnfollowing;
                    }
                    else
                    {
                        //delete follow logic goes here.

                        Model.BuzzFollow follow = GetBuzzLiked(userId, buzzid);


                        Model.Table.BuzzFollow.Execute(TableOperation.Delete(follow));

                        return FollowResult.Unfollowed;

                    }
                }
                else
                {
                    return FollowResult.Error;
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return FollowResult.Error;
            }
        }

        public static void DecreaseLikeCount(Guid buzzid)
        {
            Model.BuzzByID buzz = Core.Buzz.Buzz.GetBuzzByID(buzzid);
            if (buzz != null)
            {
                buzz.LikeCount--;
                Model.Table.BuzzByID.Execute(TableOperation.InsertOrReplace(buzz));

            }
            else
            {
                return;
            }

            Model.Buzz buzz1 = Core.Buzz.Buzz.GetBuzzByID(buzzid, buzz.CityID);
            if (buzz1 != null)
            {
                buzz1.LikeCount--;
                Model.Table.Buzz.Execute(TableOperation.InsertOrReplace(buzz1));
            }


            Model.BuzzByUser buzz2 = Core.Buzz.Buzz.GetBuzzByUser(buzzid, buzz.UserID);
            if (buzz2 != null)
            {
                buzz2.LikeCount--;
                Model.Table.BuzzByUser.Execute(TableOperation.InsertOrReplace(buzz1));
            }
        }

        public static bool? Liked(Guid userId,Guid buzzId)
        {
            try
            {
                if (GetBuzzLiked(userId,buzzId) == null)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return null;
            }

        }

        public static Model.BuzzFollow GetBuzzLiked(Guid userId, Guid buzzId)
        {
            try
            {

                Core.Cloud.TableStorage.InitializeBuzzFollowTable();

                TableQuery<Model.BuzzFollow> buzzQ = new TableQuery<Model.BuzzFollow>().Where(TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, buzzId.ToString()),TableOperators.And,TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, userId.ToString())));

                Model.BuzzFollow res = Model.Table.BuzzFollow.ExecuteQuery(buzzQ).FirstOrDefault();

                return res;
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return null;
            }
        }

        public static List<Guid> GetBuzzIdLiked(Guid userId,int take,List<Guid> skipList)
        {
            if (skipList == null)
            {
                skipList = new List<Guid>();
            }

            Web.Core.Cloud.TableStorage.InitializeBuzzFollowTable();

            return Model.Table.BuzzFollow.ExecuteQuery(
                        new TableQuery<Model.BuzzFollow>().Where(
                        TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId))).Where(o=>!skipList.Contains(o.BuzzID)).ToList().Select(o => o.BuzzID).Take(take).ToList();
        }




        internal static List<Model.Buzz> GetBuzzLiked(Guid userId)
        {

             TableQuery<Model.BuzzFollow> buzzQ = new TableQuery<Model.BuzzFollow>().Where(
             TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId));

             List<Model.BuzzFollow> res = Model.Table.BuzzFollow.ExecuteQuery(buzzQ).ToList();

           



            return Core.Buzz.Buzz.GetBuzzByIDs(res.Select(o => o.BuzzID));
            
           
        }


        public static int GetNoOfLikes(Guid buzzId)
        {
            return GetUsersWhoLikesBuzz(buzzId).Count();
        }

       public static void NotifyLikeBuzz(Guid userId, Guid buzzId)
        {
            Core.Cloud.TableStorage.InitializeNotificationsTable();
            Core.Cloud.TableStorage.InitializeNotificationsUserTable();

            List<Guid> userIds = Core.Buzz.Follow.GetUsersWhoLikesBuzz(buzzId);
            Model.BuzzByID buzz = Core.Buzz.Buzz.GetBuzzByID(buzzId);
            Model.UserProfile user = Core.Accounts.User.GetUserProfile(userId);
            //now start notifting users. :)
            foreach(Guid id in userIds)
            {

                if (id == userId)
                {
                    continue; //Dont notify the current user who followed the buzz. 
                }

                //remove all the user notifications. :)
                List<Model.Notifications> notifications =Model.Table.Notifications.ExecuteQuery(
                        new TableQuery<Model.Notifications>().Where(TableQuery.CombineFilters(
                        TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId)
                        ,TableOperators.And,
                        TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("MetaData", QueryComparisons.Equal, "BuzzFollow")
                        ,TableOperators.And,
                        TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("Param1", QueryComparisons.Equal, buzzId.ToString())
                        ,TableOperators.And,
                        TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("Type1", QueryComparisons.Equal, "Buzz")
                        ,TableOperators.And,
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

                Guid lastuserId=temp.LastOrDefault();
                Model.UserProfile profile=Core.Accounts.User.GetUserProfile(lastuserId);
               
                string desc="";
                if(temp.Count-1>0)
                {
                    desc="<bold> "+profile.Name+" </bold> and "+(temp.Count-1)+" other people followed \""+buzz.Text+"\"";
                }
                else
                {
                    desc = "<bold> " + profile.Name + " </bold> followed \"" + buzz.Text + "\"";
                }

                Core.Notifications.Notification.AddNotification(userId, "","", desc, temp, buzzId.ToString(),userId.ToString(), "Buzz", "User", "BuzzFollow");
               
            }


           //users notified...  Not get the list of users who follow userId

            List<Guid> following = Core.Users.Follow.GetFollowersList(userId, 9999999, new List<Guid>());

            

            foreach (Guid i in following)
            {
                if (!userIds.Contains(i))
                {
                    //now notify this user because he wasnt notified earlier. But with a dufferent message. :)

                    string desc = "<bold> " + user.Name + " </bold> followed the buzz \"" + buzz.Text + "\"";
                    List<Guid> t = new List<Guid>();
                    t.Add(userId);
                    Core.Notifications.Notification.AddNotification(i, "","", desc, t, buzzId.ToString(), userId.ToString(), "Buzz", "User", "BuzzFollow");
                }
            }
        }

        public static List<Guid> GetUsersWhoLikesBuzz(Guid buzzId,int take,List<Guid> skipList)
        {
             Core.Cloud.TableStorage.InitializeBuzzFollowTable();

             TableQuery<Model.BuzzFollow> buzzQ = new TableQuery<Model.BuzzFollow>().Where(
             TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, buzzId.ToString()));
             return Model.Table.BuzzFollow.ExecuteQuery(buzzQ).ToList().Select(o => o.UserID).Where(o=>!skipList.Contains(o)).Take(take).ToList();
        }

        public static List<Guid> GetUsersWhoLikesBuzz(Guid buzzId)
        {
            Core.Cloud.TableStorage.InitializeBuzzFollowTable();

            TableQuery<Model.BuzzFollow> buzzQ = new TableQuery<Model.BuzzFollow>().Where(
            TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, buzzId.ToString()));
            return Model.Table.BuzzFollow.ExecuteQuery(buzzQ).ToList().Select(o => o.UserID).ToList();
        }
    }
}