using Hangout.Web.Core.Follow;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Core.Category
{
    public class Follow
    {
        public static FollowResult FollowCategory(Guid userId, Guid categoryid)
        {
            try
            {
                Core.Cloud.TableStorage.InitializeCategoryTable();

                bool? isfollowing = IsFollowing(userId, categoryid);


                if (isfollowing.HasValue)
                {
                    if (isfollowing.Value)
                    {
                        return FollowResult.AlreadyFollowing;
                    }
                    else
                    {

                        Model.UserCategoryFollow follow = new Model.UserCategoryFollow(userId, categoryid);
                        follow.UserID=userId;
                        follow.CategoryID = categoryid;
                        follow.DateTimeStamp = DateTime.Now;

                        Model.Table.UserCategoryFollows.Execute(TableOperation.InsertOrReplace(follow));
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

        public static FollowResult UnFollowCategory(Guid userId, Guid categoryId)
        {
            try
            {

                Core.Cloud.TableStorage.InitializeCategoryTable();

                bool? isfollowing = IsFollowing(userId, categoryId);


                if (isfollowing.HasValue)
                {
                    if (!isfollowing.Value)
                    {
                        return FollowResult.AlreadyUnfollowing;
                    }
                    else
                    {
                        //delete follow logic goes here.

                        Model.UserCategoryFollow follow = GetUserCategoryFollow(userId, categoryId);

                        Model.Table.UserCategoryFollows.Execute(TableOperation.Delete(follow));

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

        public static bool? IsFollowing(Guid userId, Guid categoryId)
        {
            try
            {
                Core.Cloud.TableStorage.InitializeCategoryTable();

                if (GetUserCategoryFollow(userId, categoryId) == null)
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

        public static Model.UserCategoryFollow GetUserCategoryFollow(Guid userId, Guid categoryId)
        {
            
            try
            {
                Core.Cloud.TableStorage.InitializeUserCategoryFollowsTable();
                return Model.Table.UserCategoryFollows.ExecuteQuery(new TableQuery<Model.UserCategoryFollow>().Where(TableQuery.CombineFilters(TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal,userId), TableOperators.And, TableQuery.GenerateFilterConditionForGuid("CategoryID", QueryComparisons.Equal, categoryId)))).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return null;
            }
        }

        public static List<Model.Category> GetCategoryFollowing(Guid userId)
        {
            Core.Cloud.TableStorage.InitializeUserCategoryFollowsTable();
            List<Guid> cateogryIds =  Model.Table.UserCategoryFollows.ExecuteQuery(new TableQuery<Model.UserCategoryFollow>().Where(TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId))).Select(o=>o.CategoryID).ToList();

            List<Model.Category> category = new List<Model.Category>();

            foreach (Guid id in cateogryIds)
            {
                category.Add(Core.Category.Category.GetCategoryByID(id));
            }

            return category;
        }

       

        public static void NotifyCategoryFollow(Guid userId, Guid categoryId)
        {
            

            List<Guid> notify = Core.Users.Follow.GetFollowersList(userId, 9999999, new List<Guid>());

            Model.UserProfile user = Core.Accounts.User.GetUserProfile(userId);
            Model.Category ig = Core.Category.Category.GetCategoryByID(categoryId);


            string desc = "<bold> " + user.Name + " </bold> follows <bold> " + ig.Name + " </bold>";


            if (notify.Contains(userId))
            {
                notify.Remove(userId);
            }



            List<Guid> x = new List<Guid>();
            x.Add(userId);
            foreach (Guid i in notify)
            {
                Core.Notifications.Notification.AddNotification(i, "","", desc, x, categoryId.ToString(), userId.ToString(), "Category", "User", "CategoryFollow");
            }

            
        }

        internal static List<Guid> GetLocalPeopleWhoFollowCategory(Guid userId, Guid categoryId)
        {
            Core.Cloud.TableStorage.InitializeUserCategoryFollowsTable();

            return Model.Table.UserCategoryFollows.ExecuteQuery(new TableQuery<Model.UserCategoryFollow>().Where(
                TableQuery.CombineFilters(
                TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.UserCategoryFollow.GetPartitionKey(userId)), TableOperators.And,
                TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId)), TableOperators.And, TableQuery.GenerateFilterCondition("CategoryID", QueryComparisons.Equal, categoryId.ToString())
                ))).Select(o => o.CategoryID).ToList(); ;
            
        }
    }
}