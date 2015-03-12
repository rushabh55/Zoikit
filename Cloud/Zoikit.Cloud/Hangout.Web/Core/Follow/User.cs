using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Core.Follow
{
    public class User
    {
        public static FollowResult Follow(int userid, int followUserid, ref Model.HangoutDBEntities context)
        {
            try
            {
                 bool? isfollowing=IsFollowing(userid, followUserid, context);


                 if (isfollowing.HasValue)
                 {
                     if (isfollowing.Value)
                     {
                         return FollowResult.AlreadyFollowing;
                     }
                     else
                     {

                         Model.UserFollow follow = new Model.UserFollow();
                         follow.UserID = userid;
                         follow.UserFollowID = followUserid;
                         follow.DateTimeStamp = DateTime.Now;

                         context.AddToUserFollows(follow);

                         context.SaveChanges();

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
                Core.Exceptions.ExceptionReporting.AddAnException(userid, Web.Core.ClientType.WindowsAzure, ex, ref context);
                return FollowResult.Error;
            }

            
        }

        public static FollowResult UnFollow(int userid, int followUserid, ref Model.HangoutDBEntities context)
        {
            try
            {
                bool? isfollowing=IsFollowing(userid, followUserid, context);


                if (isfollowing.HasValue)
                {
                    if (!isfollowing.Value)
                    {
                        return FollowResult.AlreadyUnfollowing;
                    }
                    else
                    {
                        //delete follow logic goes here.

                        Model.UserFollow follow=GetUserFollow(userid,followUserid, context);

                        context.UserFollows.DeleteObject(follow);

                        context.SaveChanges();

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
                Core.Exceptions.ExceptionReporting.AddAnException(userid, Web.Core.ClientType.WindowsAzure, ex, ref context);
                return FollowResult.Error;
            }
        }

        public static bool? IsFollowing(int userid, int followUserid, Model.HangoutDBEntities context)
        {
            try
            {
                if (GetUserFollow(userid, followUserid, context) == null)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.AddAnException(userid, Web.Core.ClientType.WindowsAzure, ex, ref context);
                return null;
            }

        }

        public static Model.UserFollow GetUserFollow(int userId, int userFollowId, Model.HangoutDBEntities context)
        {
            try
            {
                return context.UserFollows.Where(o => o.UserID == userId && o.FollowUserID == userFollowId).SingleOrDefault();
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex, ref context);
                return null;
            }
        }

        public static List<int> GetFollowersList(int userid, Model.HangoutDBEntities context)
        {
            return context.UserFollows.Where(o => o.FollowUserID == userid).Select(o => o.UserID.Value).ToList();
        }

        public static List<int> GetFollowingList(int userid, Model.HangoutDBEntities context)
        {
            return context.UserFollows.Where(o => o.UserID.Value == userid).Select(o => o.FollowUserID.Value).ToList();
        }

        public static int GetNoOfFollowers(int userid, Model.HangoutDBEntities context)
        {
           return context.UserFollows.Where(o => o.FollowUserID == userid).Select(o => o.UserID.Value).Count();
        }

        public static int GetNoOfFollowing(int userid, Model.HangoutDBEntities context)
        {
            return context.UserFollows.Where(o => o.UserID.Value == userid).Select(o => o.FollowUserID.Value).Count();
        }


        
    }
}