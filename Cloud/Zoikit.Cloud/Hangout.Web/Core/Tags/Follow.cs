using Hangout.Web.Core.Follow;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Core.Tags
{
    public class Follow
    {
        public static FollowResult FollowTag(Guid userId, Guid tagid,Guid cityId,bool exp)
        {
            try
            {

                
                Core.Cloud.TableStorage.InitializeUserTagFollowTable();

                bool? isfollowing = IsFollowing(userId, tagid,cityId);


                if (isfollowing.HasValue)
                {
                    if (isfollowing.Value)
                    {
                        return FollowResult.AlreadyFollowing;
                    }
                    else
                    {
                        Model.UserTagFollow follow = new Model.UserTagFollow(userId,tagid,cityId);
                        follow.UserID=userId;
                        follow.TagID = tagid;
                        follow.DateTimeStamp = DateTime.Now;
                        follow.Explicit = exp;
                        Model.Table.UserTagFollow.Execute(TableOperation.Insert(follow));

                        ChangeLocalTagFollowCount(tagid, cityId, 1);

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

        public static FollowResult UnFollowTag(Guid userId, Guid tagid, Guid cityId)
        {
            try
            {

                
                Core.Cloud.TableStorage.InitializeUserTagFollowTable();
                bool? isfollowing = IsFollowing(userId, tagid,cityId);


                if (isfollowing.HasValue)
                {
                    if (!isfollowing.Value)
                    {
                        return FollowResult.AlreadyUnfollowing;
                    }
                    else
                    {
                        //delete follow logic goes here.

                        Model.UserTagFollow follow = GetUserTagFollow(userId, tagid,cityId);

                        Model.Table.UserTagFollow.Execute(TableOperation.Delete(follow));
                        ChangeLocalTagFollowCount(tagid, cityId, -1);

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

        public static bool? IsFollowing(Guid userId, Guid tagId,Guid cityId)
        {
            try
            {
                if (GetUserTagFollow(userId, tagId, cityId) == null)
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

        public static Model.UserTagFollow GetUserTagFollow(Guid userId,Guid tagId,Guid cityId )
        {
            
            Core.Cloud.TableStorage.InitializeUserTagFollowTable();

            try
            {
                return Model.Table.UserTagFollow.ExecuteQuery(new TableQuery<Model.UserTagFollow>().Where(TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, Model.UserTagFollow.GetRowKey(userId)), TableOperators.And, TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal,Model.UserTagFollow.GetPartitionKey(cityId,tagId))))).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return null;
            }
        }

        public static List<Guid> GetTagIdAllFollowing(Guid userId,int take,List<Guid> skipList)
        {
            
            Core.Cloud.TableStorage.InitializeUserTagFollowTable();

            return Model.Table.UserTagFollow.ExecuteQuery(new TableQuery<Model.UserTagFollow>().Where(
                TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId)
            ).Take(Core.Smart.ListAlgo.TakeCount(take,skipList.Count))).Where(o=>!skipList.Contains(o.TagID)).OrderBy(o => new Random().Next(1, 1000)).Take(take).Select(o=>o.TagID).ToList();

            
        }


        public static List<Guid> GetAllTagIdFollowing(Guid userId, int take, List<Guid> skipList)
        {

           
            Core.Cloud.TableStorage.InitializeUserTagFollowTable();

            return Model.Table.UserTagFollow.ExecuteQuery(new TableQuery<Model.UserTagFollow>().Where(
                TableQuery.CombineFilters(
                TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId),
                TableOperators.And,TableQuery.GenerateFilterConditionForBool("Explicit", QueryComparisons.Equal, true)
            )).Take(Core.Smart.ListAlgo.TakeCount(take,skipList.Count))).Where(o=>!skipList.Contains(o.TagID)).OrderBy(o => new Random().Next(1, 1000)).Take(take).Select(o=>o.TagID).ToList();

           
        }

        public static List<Guid> GetTagIdFollowing(Guid userId, int take, List<Guid> skipList)
        {

            
            Core.Cloud.TableStorage.InitializeUserTagFollowTable();

           return  Model.Table.UserTagFollow.ExecuteQuery(new TableQuery<Model.UserTagFollow>().Where(TableQuery.CombineFilters(TableQuery.GenerateFilterConditionForBool("Explicit", QueryComparisons.Equal, true), TableOperators.And,TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId)))).Where(o=>!skipList.Contains(o.TagID)).ToList().OrderBy(o => new Random().Next(1, 1000)).Take(take).Select(o=>o.TagID).ToList();
        }


      

        internal static List<Model.Tag> GetAllTagsFollowing(Guid userId)
        {

           
            Core.Cloud.TableStorage.InitializeUserTagFollowTable();

            List<Model.UserTagFollow> userTagFollow = Model.Table.UserTagFollow.ExecuteQuery(new TableQuery<Model.UserTagFollow>().Where(TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId))).ToList();

            List<Model.Tag> tags = new List<Model.Tag>();

            foreach (Model.UserTagFollow tf in userTagFollow)
            {
                tags.Add(Core.Tags.Tags.GetTagByID(tf.TagID));
            }


            return tags;
        }

        internal static List<Model.Tag> GetTagsFollowing(Guid userId)
        {

           
            Core.Cloud.TableStorage.InitializeUserTagFollowTable();
            List<Model.UserTagFollow> userTagFollow = Model.Table.UserTagFollow.ExecuteQuery(new TableQuery<Model.UserTagFollow>().Where(TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId))).ToList();

            List<Model.Tag> tags = new List<Model.Tag>();

            foreach (Model.UserTagFollow tf in userTagFollow)
            {
                tags.Add(Core.Tags.Tags.GetTagByID(tf.TagID));
            }


            return tags;
        }

        internal static int GetTotalNoOfPeopleFollowing(Guid id,Guid cityId)
        {


            return GetLocalTagFollowCount(id, cityId);
            
        }

        internal static int GetNoOfPeopleFollowingByUserCity(Guid userId, Guid id)
        {
            
            Core.Cloud.TableStorage.InitializeUserTagFollowTable();

            Model.City city = Core.Location.UserLocation.GetUserCity(userId);
            if (city == null)
            {
                return 0;
            }

            return Core.Tags.Follow.GetLocalFollowersByCity(city.CityID,id,99999999,new List<Guid>()).Count();
        }

        public static List<Guid> GetLocalFollowersByTag(Guid tagId, Guid cityId, int take, List<Guid> skipList)
        {
            Core.Cloud.TableStorage.InitializeUserTagFollowTable();
         return Model.Table.UserTagFollow.ExecuteQuery(new TableQuery<Model.UserTagFollow>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.UserTagFollow.GetPartitionKey(cityId,tagId)))).ToList().Select(o=>o.UserID).Where(o=>!skipList.Contains(o)).Distinct().Take(take).ToList();
        }

        public static List<Model.User> GetLocalFollowersByCity(Guid cityId, Guid tagId, int take, List<Guid> skipList)
        {
            Core.Cloud.TableStorage.InitializeUserLocationsTable();
            Core.Cloud.TableStorage.InitializeUserTagFollowTable();

            List<Guid> localUserIDs = Model.Table.UserLocations.ExecuteQuery(new TableQuery<Model.UserLocation>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.UserLocation.GetPartitonKeyByCityID(cityId)))).ToList().Select(o => o.UserID).ToList();

            List<Guid> tagFollow = Model.Table.UserTagFollow.ExecuteQuery(new TableQuery<Model.UserTagFollow>().Where(TableQuery.GenerateFilterConditionForGuid("TagID", QueryComparisons.Equal, tagId))).ToList().Select(o => o.UserID).ToList();

            return Core.Accounts.User.GetUsersById(localUserIDs.Where(o => tagFollow.Contains(o) && !skipList.Contains(o)).Take(take).ToList());
        }

        public static List<Guid> GetLocalBuzzByTag(Guid userId, Guid tagId, int take, Guid cityId, List<Guid> skipList)
        {
            Core.Cloud.TableStorage.InitializeBuzzByTag();

           return Model.Table.BuzzByTag.ExecuteQuery(new TableQuery<Model.BuzzByTag>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.BuzzByTag.GetPartitionKey(tagId,cityId)))).ToList().Select(o => o.BuzzID).Where(o=>!skipList.Contains(o)).Take(take).ToList();

            

        }

        public static void NotifyTagFollowed(Guid userId, Guid tagid)
        {

            Model.Location loc = Core.Location.UserLocation.GetLastLocation(userId);

            if (loc != null)
            {
                Guid cityId = loc.CityID;
                List<Guid> notify = Core.Users.Follow.GetFollowersList(userId, 9999999, new List<Guid>());


                notify.AddRange(Core.Tags.Follow.GetLocalFollowersByCity(cityId, tagid, 9999999, new List<Guid>()).Select(o=>o.UserID));

                notify = notify.Distinct().ToList();
                Model.UserProfile user = Core.Accounts.User.GetUserProfile(userId);
                Model.Tag tag = Core.Tags.Tags.GetTagByID(tagid);

                Model.UserTagFollow tagFollow = GetUserTagFollow(userId, tagid, cityId);

                if (tagFollow.Explicit == true)
                {

                    string tagname = tag.Name;

                    if (!tagname.StartsWith("#"))
                    {
                        tagname = "#" + tagname;
                    }

                    string desc = "<bold> " + user.Name + " </bold> loves <bold> " + tagname + " </bold>";


                    if (notify.Contains(userId))
                    {
                        notify.Remove(userId);
                    }



                    List<Guid> x = new List<Guid>();
                    x.Add(userId);
                    foreach (Guid i in notify)
                    {
                        Core.Notifications.Notification.AddNotification(i, "", "", desc, x, tag.Name, userId.ToString(), "Tag", "User", "TagFollow");
                    }
                }
            }

            
        }


        public static int GetLocalTagFollowCount(Guid tagId, Guid cityId)
        {
            Core.Cloud.TableStorage.InitializeLocalTagFollowCount();

            Model.LocalTagFollowCount tag = Model.Table.LocalTagFollowCount.ExecuteQuery(new TableQuery<Model.LocalTagFollowCount>().Where(TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, cityId.ToString()), TableOperators.And,
               TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, tagId.ToString())))).FirstOrDefault();

            if(tag==null)
            {
                return 0;
            }

            return tag.Count;
        }

        public static void ChangeLocalTagFollowCount(Guid tagId, Guid cityId, int change)
        {
            Core.Cloud.TableStorage.InitializeLocalTagFollowCount();

            Model.LocalTagFollowCount tag = Model.Table.LocalTagFollowCount.ExecuteQuery(new TableQuery<Model.LocalTagFollowCount>().Where(TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, cityId.ToString()), TableOperators.And,
              TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, tagId.ToString())))).FirstOrDefault();

            if(tag==null)
            {
                tag = new Model.LocalTagFollowCount(tagId, cityId);
                
            }

            tag.Count = tag.Count + change;

            if(tag.Count<0)
            {
                tag.Count = 0;
            }

            Model.Table.LocalTagFollowCount.Execute(TableOperation.InsertOrReplace(tag));

        }

        internal static List<Guid> DiscoverLocalTags(Guid userId, int tagRand, Guid cityId, List<Guid> skipList)
        {
            Core.Cloud.TableStorage.InitializeLocalTagFollowCount();

            return Model.Table.LocalTagFollowCount.ExecuteQuery(new TableQuery<Model.LocalTagFollowCount>().Where(TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, cityId.ToString()), TableOperators.And,
               TableQuery.GenerateFilterConditionForInt("Count", QueryComparisons.GreaterThan, 0)))).ToList().Select(o => o.TagID).Where(o => !skipList.Contains(o)).Distinct().Take(tagRand).ToList();
        }
    }
}