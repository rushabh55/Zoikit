using Hangout.Web.Core.Follow;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Core.Location
{
    public class UserPlaceFollow
    {
        public static FollowResult FollowPlace(Guid userId, Guid placeid)
        {
            try
            {
                bool? isfollowing = IsFollowing(userId, placeid);


                if (isfollowing.HasValue)
                {
                    if (isfollowing.Value)
                    {
                        return FollowResult.AlreadyFollowing;
                    }
                    else
                    {

                        Model.UserPlaceFollow follow = new Model.UserPlaceFollow();
                        follow.UserID=userId;
                        follow.PlaceID = placeid;
                        follow.DateTimeStamp = DateTime.Now;

                        Model.Table.UserPlaceFollow.Execute(TableOperation.InsertOrReplace(follow));

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

        public static FollowResult UnFollowPlace(Guid userId, Guid placeid)
        {
            try
            {
                bool? isfollowing = IsFollowing(userId, placeid);


                if (isfollowing.HasValue)
                {
                    if (!isfollowing.Value)
                    {
                        return FollowResult.AlreadyUnfollowing;
                    }
                    else
                    {
                        //delete follow logic goes here.

                        Model.UserPlaceFollow follow = GetPlaceFollow(userId, placeid);

                        Model.Table.UserPlaceFollow.Execute(TableOperation.Delete(follow));

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

        public static bool IsFollowing(Guid userId, Guid placeid)
        {
            try
            {
                if (GetPlaceFollow(userId, placeid) == null)
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

        public static Model.UserPlaceFollow GetPlaceFollow(Guid userId, Guid placeId)
        {
            try
            {
                return Model.Table.UserPlaceFollow.ExecuteQuery(new TableQuery<Model.UserPlaceFollow>().Where(TableQuery.CombineFilters(
                    TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.UserPlaceFollow.GetPartitionKey(placeId)), TableOperators.And,
                    TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId)),TableOperators.And, TableQuery.GenerateFilterCondition("PlaceID", QueryComparisons.Equal, placeId.ToString())
                    ))).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return null;
            }
        }

        public static List<Guid> GetFollowersList(Guid placeId)
        {
            return Model.Table.UserPlaceFollow.ExecuteQuery(new TableQuery<Model.UserPlaceFollow>().Where(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.UserPlaceFollow.GetPartitionKey(placeId))
                    )).Select(o => o.UserID).ToList();
        }

        public static List<Model.User> GetFollowersUserList(Guid placeid,int take,List<Guid> skip)
        {
            List<Guid> users = Model.Table.UserPlaceFollow.ExecuteQuery(new TableQuery<Model.UserPlaceFollow>().Where(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.UserPlaceFollow.GetPartitionKey(placeid))
                    )).Select(o => o.UserID).ToList();

            List<Model.User> filtered = new List<Model.User>();

            int t = 0;//take

            foreach (Guid u in users)
            {
                if (!skip.Contains(u))
                {
                    take++;
                    filtered.Add(Core.Accounts.User.GetUser(u));

                }

                if (t >= take)
                {
                    break;
                }
            }

            return filtered;
        }

        public static int GetNoOfFollowers(Guid placeid)
        {
            return Model.Table.UserPlaceFollow.ExecuteQuery(new TableQuery<Model.UserPlaceFollow>().Where(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.UserPlaceFollow.GetPartitionKey(placeid))
                    )).Count();
        }

        public static List<Model.Place> GetPlaceFollowing(Guid userId,int take,List<Guid> skipList)
        {
            List<Guid> places =  Model.Table.UserPlaceFollow.ExecuteQuery(new TableQuery<Model.UserPlaceFollow>().Where(
                    TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId)
                    )).Where(o => !skipList.Contains(o.PlaceID)).ToList().OrderBy(o => new Random().Next(1, 1000)).Take(take).Select(o => o.PlaceID).ToList();

            List<Model.Place> list = new List<Model.Place>();
            foreach (Guid placeId in places)
            {
                list.Add(Core.Location.Place.GetPlace(placeId));
            }


            return list;
        }


        public static void NotifyPlaceFollow(Guid placeId, Guid userId)
        {
            List<Guid> Followers = Core.Location.UserPlaceFollow.GetFollowersList(placeId);  //users who follow Place :)
            Followers.AddRange(Core.Users.Follow.GetFollowersList(userId, 999999, new List<Guid>())); // now contains people who follow the user. 

            Followers = Followers.Distinct().ToList(); //remove all the dup values. :)

            if (Followers.Contains(userId))
            {
                Followers.Remove(userId);
            }

            Model.Place ven = Core.Location.Place.GetPlace(placeId);
            Model.UserProfile user = Core.Accounts.User.GetUserProfile(userId);
            //now start notifting users. :)

            foreach (Guid id in Followers)
            {
                //remove all the user notifications. :)
                string vid = placeId.ToString();
                    
                

                List<Guid> temp = Core.Location.UserPlaceFollow.GetFollowersList(placeId);
                

                if (temp.Contains(id))
                {
                    temp.Remove(id);
                }

                int folCount = temp.Count;

                temp = temp.Take(5).ToList();

                string desc = "";
                if (temp.Count - 1 > 0)
                {
                    desc = user.Name + " and " + (folCount - 1) + " other people followed @" +ven.Name  + "";
                }
                else
                {
                    desc = user.Name + " followed @" + ven.Name+ "";
                }

                Core.Notifications.Notification.AddNotification(id, "","", desc, temp, placeId.ToString(), userId.ToString(), "Place", "User", "UserPlaceFollow");

            }

            //and We're now done! :)
        }

        internal static int GetNoOfPlaceFollows(Guid userId)
        {
            return Model.Table.UserPlaceFollow.ExecuteQuery(new TableQuery<Model.UserPlaceFollow>().Where(
                    TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId)
                    )).Count();
        }

        internal static int GetNoOfMutualPlacesFollowed(Guid user1, Guid user2)
        {
            List<Guid> user1Places = GetPlaceFollowing(user1, 9999999, new List<Guid>()).Select(o => o.PlaceID).ToList();
            List<Guid> user2Places = GetPlaceFollowing(user2, 9999999, new List<Guid>()).Select(o => o.PlaceID).ToList();

            return user1Places.Where(o => user2Places.Contains(o)).Count();
        }
    }
}