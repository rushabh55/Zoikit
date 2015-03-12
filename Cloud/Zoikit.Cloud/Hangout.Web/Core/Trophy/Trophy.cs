using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Core.Trophy
{
    public class Trophy
    {
        //public static List<Model.Badges> GetUserTrophies(Guid userId)
        //{
        //    return GetUserAchievements(userId).Where(o=>o.BadgeType=="Trophy").ToList();

            
        //}

        //public static List<Model.Badges> GetUserBadges(Guid userId)
        //{
        //    return GetUserAchievements(userId).Where(o => o.BadgeType == "Badge").ToList();
        //}

        //public static List<Model.Badges> GetUserAchievements(Guid userId)
        //{

        //    List<Model.UserBadge> userbadges =  Model.Table.UserBadge.ExecuteQuery(new TableQuery<Model.UserBadge>().Where(TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId))).ToList();

        //    List<Model.Badges> badge =new List<Model.Badges>();

        //    foreach(Model.UserBadge userbadge in userbadges)
        //    {
        //        badge.Add(GetAchievement(userbadge.BadgeID));

        //    }


        //    return badge;
        //}



        //public static List<Model.Badges> UpdateServerBadgePicURL(List<Model.Badges> badges)
        //{
        //    List<Model.Badges> b = new List<Model.Badges>();
        //    foreach (Model.Badges badge in badges)
        //    {
        //        badge.BadgePic = Settings.Root+Settings.TrophyImages+badge.BadgePic;
        //        b.Add(badge);
        //    }

        //    return b;
        //}


        //public static List<Model.Badges> CheckBadgeAwarded(Guid userId)
        //{

        //    List<Model.Badges> badge = new List<Model.Badges>();

        //    int noOfFollowers = Core.Users.Follow.GetFollowersList(userId, 9999999, new List<Guid>()).Count();
        //    int noOfFollowing = Core.Users.Follow.GetFollowingList(userId, 9999999, new List<Guid>()).Count();

        //   Model.UserScore score = Core.Score.UserScore.GetLastScore(userId);
        //   int totalScore = 0;
        //   if (score != null)
        //   {
        //       totalScore = score.CurrentScore;
        //   }

        //   int noOfPlaceFollow = Core.Location.UserPlaceFollow.GetNoOfPlaceFollows(userId);

        //   int noOfPlaceCheckedIn = Core.Location.Place.NoOfPlacesCheckedIn(userId);


        //   List<Model.Category> igs = Core.Category.Follow.GetCategoryFollowing(userId);

        //   int igsCount = 0;

        //   if (igs != null)
        //   {
        //       igsCount = igs.Count;
        //   }

        //   bool IsTechFollowed = false;
        //   bool IsAdventureFollowed = false;
        //   bool IsSportsFollowed = false;
        //   bool IsNightlifeFollowed = false;


        //   if (igs.Where(o => o.Name == "Technology").Count() > 0)
        //   {
        //       IsTechFollowed = true;
        //   }

        //   if (igs.Where(o => o.Name == "Nightlife").Count() > 0)
        //   {
        //       IsNightlifeFollowed = true;
        //   }

        //   if (igs.Where(o => o.Name == "Sports").Count() > 0)
        //   {
        //       IsSportsFollowed = true;
        //   }
        //   if (igs.Where(o => o.Name == "Adventure").Count() > 0)
        //   {
        //       IsAdventureFollowed = true;
        //   }


        //  // bool userInteracted = Core.Buzz.Buzz.DoesUserMakesOtherInteract(userId);

        //   bool userInteracted = true;

        //   int NoOfBuzz = Core.Buzz.Buzz.GetNoOfBuzzFollowed(userId);


        //    //now start awarding trophies. :)

        //   if (noOfFollowers > 0)
        //   {
        //       Model.Badges b=AwardAchievement(userId, "1NoOfFollowers");

        //       if (b != null)
        //       {
        //           badge.Add(b);
        //       }
        //   }

        //   if (noOfFollowers > 4)
        //   {
        //       Model.Badges b=AwardAchievement(userId, "5NoOfFollowers");

        //       if (b != null)
        //       {
        //           badge.Add(b);
        //       }
        //   }

        //   if (noOfFollowers > 9)
        //   {
        //       Model.Badges b=AwardAchievement(userId, "10NoOfFollowers");

        //       if (b != null)
        //       {
        //           badge.Add(b);
        //       }
        //   }

        //   if (noOfFollowing > 0)
        //   {
        //       Model.Badges b=AwardAchievement(userId, "NoOfFollowing");

        //       if (b != null)
        //       {
        //           badge.Add(b);
        //       }
        //   }

        //   if (IsAdventureFollowed)
        //   {
        //       Model.Badges b=AwardAchievement(userId, "AdventureFollowed");

        //       if (b != null)
        //       {
        //           badge.Add(b);
        //       }
        //   }

        //   if (IsSportsFollowed)
        //   {
        //       Model.Badges b=AwardAchievement(userId, "SportsFollowed");

        //       if (b != null)
        //       {
        //           badge.Add(b);
        //       }
        //   }

        //   if (IsNightlifeFollowed)
        //   {
        //       Model.Badges b=AwardAchievement(userId, "NightlifeFollowed");

        //       if (b != null)
        //       {
        //           badge.Add(b);
        //       }
        //   }

        //   if (IsTechFollowed)
        //   {
        //       Model.Badges b=AwardAchievement(userId, "TechFollowed");

        //       if (b != null)
        //       {
        //           badge.Add(b);
        //       }
        //   }


        //   if (totalScore >= 100)
        //   {
        //       Model.Badges b=AwardAchievement(userId, "100Score");

        //       if (b != null)
        //       {
        //           badge.Add(b);
        //       }
        //   }

        //   if (totalScore >= 500)
        //   {
        //       Model.Badges b=AwardAchievement(userId, "500Score");

        //       if (b != null)
        //       {
        //           badge.Add(b);
        //       }
        //   }

        //   if (totalScore >= 1000)
        //   {
        //       Model.Badges b=AwardAchievement(userId, "1000Score");

        //       if (b != null)
        //       {
        //           badge.Add(b);
        //       }
        //   }

        //   if (NoOfBuzz > 0)
        //   {
        //       Model.Badges b=AwardAchievement(userId, "1Buzz");

        //       if (b != null)
        //       {
        //           badge.Add(b);
        //       }
        //   }

        //   if (NoOfBuzz > 4)
        //   {
        //       Model.Badges b=AwardAchievement(userId, "5Buzz");

        //       if (b != null)
        //       {
        //           badge.Add(b);
        //       }
        //   }

        //   if (userInteracted)
        //   {
        //       Model.Badges b=AwardAchievement(userId, "UserInteracted");

        //       if (b != null)
        //       {
        //           badge.Add(b);
        //       }
        //   }

        //   if (igsCount > 0)
        //   {
        //       Model.Badges b=AwardAchievement(userId, "CategoryFollowed");

        //       if (b != null)
        //       {
        //           badge.Add(b);
        //       }
        //   }

        //   if (noOfPlaceCheckedIn > 0)
        //   {
        //       Model.Badges b=AwardAchievement(userId, "1PlaceCheckIn");

        //       if (b != null)
        //       {
        //           badge.Add(b);
        //       }
        //   }

        //   if (noOfPlaceCheckedIn > 4)
        //   {
        //       Model.Badges b=AwardAchievement(userId, "5PlaceCheckIn");

        //       if (b != null)
        //       {
        //           badge.Add(b);
        //       }
        //   }

        //   if (noOfPlaceCheckedIn > 9)
        //   {
        //       Model.Badges b=AwardAchievement(userId, "10PlaceCheckIn");

        //       if (b != null)
        //       {
        //           badge.Add(b);
        //       }
        //   }

        //   if (noOfPlaceFollow > 0)
        //   {
        //       Model.Badges b=AwardAchievement(userId, "PlaceFollow");

        //       if (b != null)
        //       {
        //           badge.Add(b);
        //       }
        //   }

        //    foreach(Model.Badges x in badge)
        //    {
        //        Core.Notifications.Notification.AddNotification(userId, "You've got an achievement", Settings.Root + Settings.TrophyImages + x.BadgePic, x.BadgeDescription, null, userId.ToString(), "View", "User", "Trophy", "TrophyAwarded");
        //    }

        //   return badge;

        //}

       



        //public static bool HasAcheievement(Guid userId, string badgename)
        //{
        //    if (Model.Table.UserBadge.ExecuteQuery(new TableQuery<Model.UserBadge>().Where(
        //        TableQuery.CombineFilters(
        //        TableQuery.CombineFilters(
        //        TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId)
        //        ,TableOperators.And,
        //        TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.UserBadge.GetPartitionKey(userId)))
        //        ,TableOperators.And,
        //        TableQuery.GenerateFilterCondition("BadgeName", QueryComparisons.Equal, badgename.ToString())))).Count() > 0)
        //    {
        //        return true;
        //    }

        //    return false;
        //}


        //public static Model.Badges AwardAchievement(Guid userId,string name)
        //{
        //    try
        //    {
        //        if (HasAcheievement(userId, name))
        //        {
        //            return null;
        //        }

        //        Model.UserBadge ub = new Model.UserBadge();
        //        ub.BadgeID = GetAchievementByName(name).BadgeID;
        //        ub.DateTimeStamp = DateTime.Now;
        //        ub.UserID=userId;

        //        Model.Table.Badge.Execute(TableOperation.Insert(ub));

        //        return GetAchievement(ub.BadgeID);
        //    }
        //    catch
        //    {
        //        return null;
        //    }

        //}

        //public static Model.Badges GetAchievement(Guid trophyId)
        //{

        //    return Model.Table.Badge.ExecuteQuery(new TableQuery<Model.Badges>().Where(TableQuery.GenerateFilterCondition("BadgeID", QueryComparisons.Equal, trophyId.ToString()))).FirstOrDefault();
            
        //}

        //public static Model.Badges GetAchievementByName(string name)
        //{

        //    return Model.Table.Badge.ExecuteQuery(new TableQuery<Model.Badges>().Where(TableQuery.GenerateFilterCondition("BadgeName", QueryComparisons.Equal, name.ToString()))).FirstOrDefault();

        //}

        
    }
}