using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Hangout.Services.Core.Location
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PlaceService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PlaceService.svc or PlaceService.svc.cs at the Solution Explorer and start debugging.
    public class PlaceService : IPlaceService
    {
        
        
        public List<Web.Services.Objects.Locations.Place> GetRecommenededPlaces(Guid userId, double latitude, double longitude, string zat)
        {
            Web.Services.Location.Place obj = new Web.Services.Location.Place();
            return obj.GetRecommenededPlaces(userId, latitude, longitude, zat);
        }

        public List<Web.Services.Objects.Locations.Place> SearchPlaces(Guid userId, double latitude, double longitude,string query, string zat)
        {
            Web.Services.Location.Place obj = new Web.Services.Location.Place();
            return obj.SearchPlaces(userId, latitude, longitude, query, zat);
        }


        public Web.Core.Follow.FollowResult FollowPlace(Guid userId, Guid venueId, string zat)
        {
            Web.Services.Location.PlaceFollow obj = new Web.Services.Location.PlaceFollow();
            return obj.FollowPlace(userId, venueId, zat);
        }

        public Web.Core.Follow.FollowResult UnfollowPlace(Guid userId, Guid venueId, string zat)
        {
            Web.Services.Location.PlaceFollow obj = new Web.Services.Location.PlaceFollow();
            return obj.UnfollowPlace(userId, venueId, zat);
        }

        public bool IsFollowing(Guid userId, Guid venueid, string zat)
        {
            Web.Services.Location.PlaceFollow obj = new Web.Services.Location.PlaceFollow();
            return obj.IsFollowing(userId, venueid, zat);
        }

        public List<Web.Services.Objects.Accounts.User> GetFollowersUserList(Guid userId, Guid venueid, int count, List<Guid> skip, string zat)
        {
            Web.Services.Location.PlaceFollow obj = new Web.Services.Location.PlaceFollow();
            return obj.GetFollowersUserList(userId, venueid, count, skip, zat);
        }

        public int GetNoOfFollowers(Guid userId, string zat, Guid venueid)
        {
            Web.Services.Location.PlaceFollow obj = new Web.Services.Location.PlaceFollow();
            return obj.GetNoOfFollowers(userId, zat, venueid);
        }

        public List<Web.Services.Objects.Locations.Place> GetPlaceFollowing(Guid userId,int take,List<Guid> skipList, string zat)
        {
            Web.Services.Location.PlaceFollow obj = new Web.Services.Location.PlaceFollow();
            return obj.GetPlaceFollowing(userId,take,skipList, zat);
        }




        public List<Web.Services.Objects.Locations.Place> GetPlaceFollowingByUser(Guid meId,Guid userId,int take,List<Guid> skipList, string zat)
        {
            Web.Services.Location.PlaceFollow obj = new Web.Services.Location.PlaceFollow();
            return obj.GetPlaceFollowing(meId,userId,take,skipList, zat);
        }

        public Hangout.Web.Services.Objects.Locations.Place GetPlaceByID(Guid userId, double lat,double lon,Guid venueId, string zat)
        {
            Web.Services.Location.Place obj = new Web.Services.Location.Place();
            return obj.GetPlaceByID(userId,lat,lon, venueId, zat);
        }


        public bool IsCheckedIn(Guid userId, Guid venueId, double lat, double lon, string zat)
        {
            Web.Services.Location.Place obj = new Web.Services.Location.Place();
            return obj.IsCheckedIn(userId, venueId, lat,lon, zat);
        }

        public bool CanCheckedIn(Guid userId, Guid venueId, double lat, double lon, string zat)
        {
            Web.Services.Location.Place obj = new Web.Services.Location.Place();
            return obj.CanCheckIn(userId, venueId, lat, lon, zat);
        }

        public Web.Core.Location.CheckInStatus Checkout(Guid userId, Guid venueId, bool intended, double lat, double lon, string zat)
        {
            Web.Services.Location.Place obj = new Web.Services.Location.Place();
            return obj.CheckOut(userId, venueId, lat, lon, zat);
        }

        public bool IsCheckedOut(Guid userId, Guid venueId, string zat)
        {
            Web.Services.Location.Place obj = new Web.Services.Location.Place();
            return obj.IsCheckedOut(userId, venueId, zat);
        }

        public Web.Core.Location.CheckInStatus CheckIn(Guid userId, Guid venueId, double lat, double lon, string zat)
        {
            Web.Services.Location.Place obj = new Web.Services.Location.Place();
            return obj.CheckIn(userId, venueId, lat, lon, zat);
        }

        public List<Hangout.Web.Services.Objects.Locations.Place> GetNearbyPlaces(Guid userId, double latitude, double longitude, string zat)
        {
            Web.Services.Location.Place obj = new Web.Services.Location.Place();
            return obj.GetNearbyPlaces(userId, latitude, longitude, zat);
        }
    }
}
