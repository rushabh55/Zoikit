using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.Location
{
    public class Place
    {
       

        public List<Objects.Locations.Place> GetRecommenededPlaces(Guid userId, double latitude, double longitude,string zat)
        {
            try
            {
                    if (Web.Core.Accounts.User.IsValid(userId,zat))
                    {
                        List<Model.Place> places = Web.Core.Location.Place.GetRecommendedPlaces(userId, latitude, longitude);

                        List<Objects.Locations.Place> ven=new List<Objects.Locations.Place>();
                        foreach(Model.Place  v in places)
                        {
                            Objects.Locations.Place obj = Convert(userId,latitude,longitude,v);
                            ven.Add(obj);
                        }

                        return ven;
                    }
                    else { throw new Web.Core.Exceptions.Account.UnAuthorisedAccess(); }
                
            }
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(Guid.Empty, Web.Core.ClientType.WindowsAzure, ex);
                return null;
            }
        }

        public List<Objects.Locations.Place> GetNearbyPlaces(Guid userId, double latitude, double longitude, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId, zat))
                {
                    List<Model.Place> places = Web.Core.Location.Place.GetNearbyPlaces(userId, latitude, longitude);

                    List<Objects.Locations.Place> ven = new List<Objects.Locations.Place>();
                    foreach (Model.Place v in places)
                    {
                        Objects.Locations.Place obj = Convert(userId, latitude, longitude, v);
                        ven.Add(obj);
                    }

                    return ven;
                }
                else { throw new Web.Core.Exceptions.Account.UnAuthorisedAccess(); }

            }
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(Guid.Empty, Web.Core.ClientType.WindowsAzure, ex);
                return null;
            }
        }

        public List<Objects.Locations.Place> SearchPlaces(Guid userId, double latitude, double longitude,string query, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId, zat))
                {
                    List<Model.Place> places = Web.Core.Location.Place.SearchPlaces(userId, latitude, longitude,query);

                    List<Objects.Locations.Place> ven = new List<Objects.Locations.Place>();
                    foreach (Model.Place v in places)
                    {
                        Objects.Locations.Place obj = Convert(userId,latitude,longitude,v);
                        ven.Add(obj);
                    }

                    return ven;
                }
                else { throw new Web.Core.Exceptions.Account.UnAuthorisedAccess(); }

            }
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return null;
            }
        }

        public Objects.Locations.Place Convert(Guid userId,double lat,double lon,Model.Place v)
        {
            Objects.Locations.Place obj = new Objects.Locations.Place();
            obj.PlaceID = v.PlaceID;
            obj.FoursquarePlaceId = v.FoursquarePlaceID;
            obj.Location = Services.Location.Location.GetLocation(v.LocationID);
            obj.Name = v.Name;
            obj.FoursquareCannonicalURL = v.FoursquareCannonicalURL;
            obj.Phone = v.Phone;
            obj.NoOfFollowing = Core.Location.Place.GetNoOfFollowers(v.PlaceID);
            obj.IsFollowing = Core.Location.UserPlaceFollow.IsFollowing(userId, v.PlaceID);
            obj.Twitter = v.Twitter;
            obj.Tags = Services.Tag.Tag.ConvertTag(Core.Location.Place.GetPlaceTags(obj.PlaceID));
            obj.IsCheckedIn = Core.Location.Place.IsCheckedIn(userId,v.PlaceID, lat, lon);
            obj.NoOfCheckedIn = Core.Location.Place.GetNoOfPeopleCurrentlyCheckedIn(v.PlaceID);
            return obj;
        }

        public Objects.Locations.Place Convert(Guid userId, Model.Place v)
        {
            Objects.Locations.Place obj = new Objects.Locations.Place();
            obj.PlaceID = v.PlaceID;
            obj.FoursquarePlaceId = v.FoursquarePlaceID;
            obj.Location = Services.Location.Location.GetLocation(v.LocationID);
            obj.Name = v.Name;
            obj.FoursquareCannonicalURL = v.FoursquareCannonicalURL;
            obj.Phone = v.Phone;
            obj.NoOfFollowing = Core.Location.Place.GetNoOfFollowers(v.PlaceID);
            obj.IsFollowing = Core.Location.UserPlaceFollow.IsFollowing(userId, v.PlaceID);
            obj.Twitter = v.Twitter;
            obj.Tags = Services.Tag.Tag.ConvertTag(Core.Location.Place.GetPlaceTags(obj.PlaceID));
            obj.IsCheckedIn = false;
            obj.NoOfCheckedIn = Core.Location.Place.GetNoOfPeopleCurrentlyCheckedIn(v.PlaceID);
            return obj;
        }

        public List<Objects.Locations.Place> Convert(Guid userId,List<Model.Place> places)
        {
            List<Objects.Locations.Place> placeList = new List<Objects.Locations.Place>();

            foreach (Model.Place v in places)
            {
                placeList.Add(Convert(userId, v));
            }

            return placeList;
        }

        public List<Objects.Locations.Place> Convert(Guid userId,double lat,double lon, List<Model.Place> places)
        {
            List<Objects.Locations.Place> placeList = new List<Objects.Locations.Place>();

            foreach (Model.Place v in places)
            {
                placeList.Add(Convert(userId,lat,lon, v));
            }

            return placeList;
        }




        public Objects.Locations.Place GetPlaceByID(Guid userId,double lat,double lon, Guid placeId, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId, zat))
                {
                    Model.Place place = Core.Location.Place.GetPlace(placeId);

                    return Convert(userId,lat,lon, place);
                }
                else { throw new Web.Core.Exceptions.Account.UnAuthorisedAccess(); }

            }
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return null;
            }
        }

        public bool IsCheckedIn(Guid userId, Guid placeId, double lat, double lon, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId, zat))
                {
                    return Core.Location.Place.IsCheckedIn(userId, placeId, lat, lon);

                   
                }
                else { throw new Web.Core.Exceptions.Account.UnAuthorisedAccess(); }

            }
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return false;
            }
        }

        public bool IsCheckedOut(Guid userId, Guid placeId, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId, zat))
                {
                    return Core.Location.Place.IsCheckedOut(userId, placeId);


                }
                else { throw new Web.Core.Exceptions.Account.UnAuthorisedAccess(); }

            }
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return false;
            }
        }

        
        public Core.Location.CheckInStatus CheckIn(Guid userId, Guid placeId, double lat, double lon, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId, zat))
                {
                    Core.Location.CheckInStatus res= Core.Location.Place.CheckIn(userId, placeId, lat, lon);

                    if (res == Core.Location.CheckInStatus.CheckedIn)
                    {
                        Core.Cloud.Queue.AddMessage("PlaceCheckIn:" + placeId + ":User:" + userId);
                        Core.Cloud.Queue.AddMessage("Trophy:" + userId);
                       
                    }

                    return res;
                }
                else { throw new Web.Core.Exceptions.Account.UnAuthorisedAccess(); }

            }
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return Core.Location.CheckInStatus.Error;
            }
        }

        public Core.Location.CheckInStatus CheckOut(Guid userId, Guid placeId, double lat, double lon, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId, zat))
                {
                    return Core.Location.Place.Checkout(userId, placeId,true, lat, lon);


                }
                else { throw new Web.Core.Exceptions.Account.UnAuthorisedAccess(); }

            }
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return Core.Location.CheckInStatus.Error;
            }
        }

        public bool CanCheckIn(Guid userId, Guid placeId, double lat, double lon, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId, zat))
                {
                    return Core.Location.Place.CanCheckedIn(userId, placeId, lat, lon);
                }
                else { throw new Web.Core.Exceptions.Account.UnAuthorisedAccess(); }

            }
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return false;
            }
        }
    }
}