using Hangout.Web.Core.Follow;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Hangout.Web.Core.Users
{
    public class Follow
    {
        public static FollowResult FollowUser(Guid userId, Guid followuserId)
        {
            try
            {
                 bool? isfollowing=IsFollowing(userId, followuserId);


                 if (isfollowing.HasValue)
                 {
                     if (isfollowing.Value)
                     {
                         return FollowResult.AlreadyFollowing;
                     }
                     else
                     {

                         Core.Cloud.TableStorage.InitializeUserFollowTable();

                         Model.UserFollow follow = new Model.UserFollow(userId, followuserId);
                         follow.UserID=userId;
                         follow.FollowuserId = followuserId;
                         follow.DateTimeStamp = DateTime.Now;

                         Model.Table.UserFollow.Execute(TableOperation.Insert(follow));

                         Core.Social.Facebook.CheckFacebookFollow(userId, followuserId);


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

        public static FollowResult UnFollowUser(Guid userId, Guid followuserId)
        {
            try
            {
                bool? isfollowing=IsFollowing(userId, followuserId);


                if (isfollowing.HasValue)
                {
                    if (!isfollowing.Value)
                    {
                        return FollowResult.AlreadyUnfollowing;
                    }
                    else
                    {

                        Core.Cloud.TableStorage.InitializeUserFollowTable();
                        //delete follow logic goes here.

                        Model.UserFollow follow=GetUserFollow(userId,followuserId);

                        Model.Table.UserFollow.Execute(TableOperation.Delete(follow));

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

        public static bool IsFollowing(Guid userId, Guid followuserId)
        {
            try
            {
                if (GetUserFollow(userId, followuserId) == null)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return false;
            }

        }

        public static Model.UserFollow GetUserFollow(Guid userId, Guid userFollowId)
        {
            try
            {
                Core.Cloud.TableStorage.InitializeUserFollowTable();

                return Model.Table.UserFollow.ExecuteQuery(new TableQuery<Model.UserFollow>().Where(TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, userId.ToString()),
                    TableOperators.And, 
                    TableQuery.GenerateFilterCondition("PartitionKey",QueryComparisons.Equal, userFollowId.ToString()))
                    )).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return null;
            }
        }

        public static List<Guid> GetFollowersList(Guid userId, int take, List<Guid> skipList)
        {
            Core.Cloud.TableStorage.InitializeUserFollowTable();

            return Model.Table.UserFollow.ExecuteQuery(new TableQuery<Model.UserFollow>().Where(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, userId.ToString())
                    )).Where(o=>!skipList.Contains(o.UserID)).Select(o => o.UserID).ToList().OrderBy(o => new Random().Next(1, 1000)).Take(take).ToList();
        }

        public static List<Guid> GetFollowingList(Guid userId, int take, List<Guid> skipList)
        {
            Core.Cloud.TableStorage.InitializeUserFollowTable();

            return Model.Table.UserFollow.ExecuteQuery(new TableQuery<Model.UserFollow>().Where(
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, userId.ToString())
                    )).Where(o => !skipList.Contains(o.FollowuserId)).Select(o => o.FollowuserId).ToList().OrderBy(o => new Random().Next(1, 1000)).Take(take).ToList();
        }

        public static List<Model.User> GetFollowersUserList(Guid userId, int take, List<Guid> skipList)
        {

            List<Guid> userIds= GetFollowersList(userId,take,skipList);

            List<Model.User> users=new List<Model.User>();

            foreach(Guid id in userIds)
            {
                users.Add(Core.Accounts.User.GetUser(id));
            }

            return users;
        }

        public static List<Model.User> GetFollowingUserList(Guid userId, int take, List<Guid> skipList)
        {
            List<Guid> userIds = GetFollowingList(userId, take, skipList);

            List<Model.User> users = new List<Model.User>();

            foreach (Guid id in userIds)
            {
                users.Add(Core.Accounts.User.GetUser(id));
            }

            return users;
        }

        public static int GetNoOfFollowers(Guid userId)
        {
           return Model.Table.UserFollow.ExecuteQuery(new TableQuery<Model.UserFollow>().Where(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, userId.ToString())
                    
                    )).Count();
        }

        public static int GetNoOfFollowing(Guid userId)
        {
            return Model.Table.UserFollow.ExecuteQuery(new TableQuery<Model.UserFollow>().Where(
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, userId.ToString())
                    )).Count();
        }




        public static void NotifyUserFollow(Guid userId, Guid followuserId)
        {
            List<Guid> notify = GetFollowersList(userId,9999999,new List<Guid>());

             Model.UserProfile user=Core.Accounts.User.GetUserProfile(userId);
             Model.UserProfile Followuser=Core.Accounts.User.GetUserProfile(followuserId);

             if (user != null && Followuser != null)
             {

                 string desc = "<bold> "+user.Name + " </bold> followed <bold> " + Followuser.Name + " </bold>";


                 if (notify.Contains(userId))
                 {
                     notify.Remove(userId);
                 }

                 if (notify.Contains(followuserId))
                 {
                     notify.Remove(followuserId);
                 }

                 List<Guid> x = new List<Guid>();
                 x.Add(userId);
                 foreach (Guid i in notify)
                 {
                     Core.Notifications.Notification.AddNotification(i, "", "", desc, x, followuserId.ToString(), userId.ToString(), "User", "User", "UserFollow");
                 }

                 //Now notify the one who got followed.
                 Core.Notifications.Notification.AddNotification(followuserId, "", "", "<bold> " + user.Name + " </bold> followed you", x, userId.ToString(), followuserId.ToString(), "User", "User", "UserFollow");
             }
        }

        public static void IncreaseFollowCount(Guid userId)
        {
            Model.UserProfile profile = Core.Accounts.User.GetUserProfile(userId);

            profile.FollowersCount++;

            Model.Table.UserProfile.Execute(TableOperation.Replace(profile));
        }

        public static void IncreaseFollowingCount(Guid userId)
        {
            Model.UserProfile profile = Core.Accounts.User.GetUserProfile(userId);

            profile.FollowingCount++;

            Model.Table.UserProfile.Execute(TableOperation.Replace(profile));
        }


        public static void DecreaseFollowCount(Guid userId)
        {
            Model.UserProfile profile = Core.Accounts.User.GetUserProfile(userId);

            profile.FollowersCount--;

            Model.Table.UserProfile.Execute(TableOperation.Replace(profile));
        }

        public static void DecreaseFollowingCount(Guid userId)
        {
            Model.UserProfile profile = Core.Accounts.User.GetUserProfile(userId);

            profile.FollowingCount--;

            Model.Table.UserProfile.Execute(TableOperation.Replace(profile));
        }
    }
}