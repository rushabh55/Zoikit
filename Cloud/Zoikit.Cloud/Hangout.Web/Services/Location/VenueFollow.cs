using Hangout.Web.Core.Follow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.Location
{
    public class PlaceFollow
    {
       


        public Web.Core.Follow.FollowResult FollowPlace(Guid userId, Guid placeId, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(zat))
                {
                    Web.Core.Follow.FollowResult res= Core.Location.UserPlaceFollow.FollowPlace(userId, placeId);

                    if (res == FollowResult.Following)
                    {
                        Core.Cloud.Queue.AddMessage("PlaceFollow:"+placeId+":User:"+userId);
                        Core.Cloud.Queue.AddMessage("Trophy:" + userId);
                        //Core.Score.UserScore.IncreaseUserScore(userId, 10);
                       
                    }

                    return res;

                }

                else
                {
                    throw new UnauthorizedAccessException();
                }
            }

            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return FollowResult.Error;
            }
        }

        public Web.Core.Follow.FollowResult UnfollowPlace(Guid userId, Guid placeId, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(zat))
                {
                    Web.Core.Follow.FollowResult res= Core.Location.UserPlaceFollow.UnFollowPlace(userId, placeId);

                  

                    return res;
                }

                else
                {
                    throw new UnauthorizedAccessException();
                }
            }

            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return FollowResult.Error;
            }
        }

        public bool IsFollowing(Guid userId, Guid placeId,string zat)
        {

            try
            {
                if (Web.Core.Accounts.User.IsValid(zat))
                {
                    if (Core.Location.UserPlaceFollow.GetPlaceFollow(userId, placeId) == null)
                    {
                        return false;
                    }

                    return true;
                }

                else
                {
                    throw new UnauthorizedAccessException();
                }
            }

            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return false;
            }

        }

       

     

        public List<Objects.Accounts.User> GetFollowersUserList(Guid userId,Guid placeId,int count,List<Guid> skip,string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(zat))
                {
                    Services.Users.User obj = new Users.User();
                    return obj.Convert(userId,Core.Location.UserPlaceFollow.GetFollowersUserList(placeId, count, skip));
                }

                else
                {
                    throw new UnauthorizedAccessException();
                }
            }

            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return null;
            }
        }

        public int GetNoOfFollowers(Guid userId,string zat,Guid placeId)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(zat))
                {
                    return Core.Location.UserPlaceFollow.GetNoOfFollowers(placeId);
                }

                else
                {
                    throw new UnauthorizedAccessException();
                }
            }

            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return 0;
            }
            
        }

        public List<Objects.Locations.Place> GetPlaceFollowing(Guid userId,int take,List<Guid> skipList, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(zat))
                {
                    Place v = new Place();
                    return v.Convert(userId,Core.Location.UserPlaceFollow.GetPlaceFollowing(userId,take,skipList));
                }

                else
                {
                    throw new UnauthorizedAccessException();
                }
            }

            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return null;
            }
        }

        public List<Objects.Locations.Place> GetPlaceFollowing(Guid meId,Guid userId,int take,List<Guid> skip, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(zat))
                {
                    List<Model.Place> list = Core.Location.UserPlaceFollow.GetPlaceFollowing(userId,take,skip);
                    List<Objects.Locations.Place> newList = new List<Objects.Locations.Place>();

                    foreach (Model.Place v in list)
                    {
                        Objects.Locations.Place obj = new Objects.Locations.Place();
                        obj.PlaceID = v.PlaceID;
                        obj.FoursquarePlaceId = v.FoursquarePlaceID;
                        obj.Location = Services.Location.Location.GetLocation(v.LocationID);
                        obj.Name = v.Name;
                        obj.FoursquareCannonicalURL = v.FoursquareCannonicalURL;
                        obj.Phone = v.Phone;
                        obj.NoOfFollowing = Core.Location.Place.GetNoOfFollowers(v.PlaceID);
                        obj.IsFollowing = Core.Location.UserPlaceFollow.IsFollowing(meId, v.PlaceID);
                        obj.Twitter = v.Twitter;
                        obj.Tags = Services.Tag.Tag.ConvertTag(Core.Location.Place.GetPlaceTags(obj.PlaceID));
                        newList.Add(obj);
                    }

                    return newList;
                   
                }

                else
                {
                    throw new UnauthorizedAccessException();
                }
            }

            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return null;
            }
        }

    }
}