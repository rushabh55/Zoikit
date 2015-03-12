using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Core.Follow
{
    public class Token
    {
        public static FollowResult Follow(int userid, int tokenid, ref Model.HangoutDBEntities context)
        {
            try
            {
                bool? isfollowing = IsFollowing(userid, tokenid, context);


                if (isfollowing.HasValue)
                {
                    if (isfollowing.Value)
                    {
                        return FollowResult.AlreadyFollowing;
                    }
                    else
                    {

                        Model.UserTokenFollow follow = new Model.UserTokenFollow();
                        follow.UserID = userid;
                        follow.TokenID = tokenid;
                        follow.DateTimeStamp = DateTime.Now;

                        context.AddToUserTokenFollows(follow);

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

        public static FollowResult UnFollow(int userid, int tokenid, ref Model.HangoutDBEntities context)
        {
            try
            {
                bool? isfollowing = IsFollowing(userid, tokenid, context);


                if (isfollowing.HasValue)
                {
                    if (!isfollowing.Value)
                    {
                        return FollowResult.AlreadyUnfollowing;
                    }
                    else
                    {
                        //delete follow logic goes here.

                        Model.UserTokenFollow follow = GetUserTokenFollow(userid, tokenid, context);

                        context.UserTokenFollows.DeleteObject(follow);

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
                if (GetUserTokenFollow(userid, followUserid, context) == null)
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

        public static Model.UserTokenFollow GetUserTokenFollow(int userId, int tokenId, Model.HangoutDBEntities context)
        {
            try
            {
                return context.UserTokenFollows.Where(o => o.UserID == userId &&o.TokenID==tokenId).SingleOrDefault();
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex, ref context);
                return null;
            }
        }

        public static List<int> GetTokenIdFollowing(int userid, Model.HangoutDBEntities context)
        {
            return context.UserTokenFollows.Where(o => o.UserID==userid).Select(o => o.TokenID).ToList();
        }

       

    }
}