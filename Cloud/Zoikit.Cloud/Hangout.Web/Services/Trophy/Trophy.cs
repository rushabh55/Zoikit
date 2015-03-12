using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.Trophy
{
    public class Trophy
    {
        //public  List<Objects.Trophy> GetUserTrophies(Guid userId,string zat)
        //{
        //    try
        //    {
        //        if (Web.Core.Accounts.User.IsValid(zat))
        //        {
        //            return Web.Core.Trophy.Trophy.GetUserTrophies(userId).Select(o => new Objects.Trophy { TrophyPic = Settings.Root+Settings.TrophyImages+o.BadgePic, Description = o.BadgeDescription, Name = o.BadgeName, TrophyID = o.BadgeID, Type = o.BadgeType }).ToList();
        //        }
        //        else
        //        {
        //            throw new UnauthorizedAccessException();
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
        //        return null;
        //    }
        //}

        //public List<Objects.Trophy> GetUserBadges(Guid userId, string zat)
        //{
        //    try
        //    {

        //        if (Web.Core.Accounts.User.IsValid( zat))
        //        {
        //            return Web.Core.Trophy.Trophy.GetUserBadges(userId).Select(o => new Objects.Trophy { TrophyPic = o.BadgePic, Description = o.BadgeDescription, Name = o.BadgeName, TrophyID = o.BadgeID, Type = o.BadgeType }).ToList();
        //        }
        //        else
        //        {
        //            throw new UnauthorizedAccessException();
        //        }

        //    }
        //    catch (System.Exception ex)
        //    {
        //        Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
        //        return null;
        //    }
        //}

        //public List<Objects.Trophy> GetUserAchievements(Guid userId, string zat)
        //{
        //    try
        //    {
        //        if (Web.Core.Accounts.User.IsValid(zat))
        //        {
        //            return Web.Core.Trophy.Trophy.GetUserAchievements(userId).Select(o => new Objects.Trophy { TrophyPic = o.BadgePic, Description = o.BadgeDescription, Name = o.BadgeName, TrophyID = o.BadgeID, Type = o.BadgeType }).ToList();
        //        }
        //        else
        //        {
        //            throw new UnauthorizedAccessException();
        //        }

        //    }
        //    catch (System.Exception ex)
        //    {
        //        Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
        //        return null;
        //    }
        //}
    }
}