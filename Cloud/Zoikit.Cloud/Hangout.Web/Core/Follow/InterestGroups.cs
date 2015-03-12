using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Core.Follow
{
    public class InterestGroups
    {
        public static FollowResult Follow(int userid, int interestgroupid, ref Model.HangoutDBEntities context)
        {
            try
            {
                bool? isfollowing = IsFollowing(userid, interestgroupid, context);


                if (isfollowing.HasValue)
                {
                    if (isfollowing.Value)
                    {
                        return FollowResult.AlreadyFollowing;
                    }
                    else
                    {

                        Model.UserInterestGroupFollow follow = new Model.UserInterestGroupFollow();
                        follow.UserID = userid;
                        follow.InterestGroupID = interestgroupid;
                        follow.DateTimeStamp = DateTime.Now;

                        context.AddToUserInterestGroupFollows(follow);

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

        public static FollowResult UnFollow(int userid, int interestgroupid, ref Model.HangoutDBEntities context)
        {
            try
            {
                bool? isfollowing = IsFollowing(userid, interestgroupid, context);


                if (isfollowing.HasValue)
                {
                    if (!isfollowing.Value)
                    {
                        return FollowResult.AlreadyUnfollowing;
                    }
                    else
                    {
                        //delete follow logic goes here.

                        Model.UserInterestGroupFollow follow = GetUserInterestGroupFollow(userid, interestgroupid, context);

                        context.UserInterestGroupFollows.DeleteObject(follow);

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

        public static bool? IsFollowing(int userid, int interestgroupid, Model.HangoutDBEntities context)
        {
            try
            {
                if (GetUserInterestGroupFollow(userid, interestgroupid, context) == null)
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

        public static Model.UserInterestGroupFollow GetUserInterestGroupFollow(int userId, int interestgroupid, Model.HangoutDBEntities context)
        {
            try
            {
                return context.UserInterestGroupFollows.Where(o => o.UserID == userId&&o.InterestGroupID==interestgroupid).SingleOrDefault();
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex, ref context);
                return null;
            }
        }

        public static List<int> GetInterestGroupsFollowing(int userid, Model.HangoutDBEntities context)
        {
            return context.UserInterestGroupFollows.Where(o => o.UserID == userid).Select(o => o.InterestGroupID.Value).ToList();
        }
    }
}