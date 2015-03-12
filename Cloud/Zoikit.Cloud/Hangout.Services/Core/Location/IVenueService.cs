using Hangout.Web.Core.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Hangout.Services.Core.Location
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPlaceService" in both code and config file together.
    [ServiceContract]
    public interface IPlaceService
    {
        [OperationContract]
        List<Hangout.Web.Services.Objects.Locations.Place> GetRecommenededPlaces(Guid userId, double latitude, double longitude, string zat);
         [OperationContract]
        List<Hangout.Web.Services.Objects.Locations.Place> SearchPlaces(Guid userId, double latitude, double longitude, string query, string zat);
         [OperationContract]
        Web.Core.Follow.FollowResult FollowPlace(Guid userId, Guid venueId, string zat);
         [OperationContract]
        Web.Core.Follow.FollowResult UnfollowPlace(Guid userId, Guid venueId, string zat);
         [OperationContract]
        bool IsFollowing(Guid userId, Guid venueid, string zat);
         [OperationContract]
        List<Hangout.Web.Services.Objects.Accounts.User> GetFollowersUserList(Guid userId, Guid venueid, int count, List<Guid> skip, string zat);
         [OperationContract]
        int GetNoOfFollowers(Guid userId, string zat, Guid venueid);
         [OperationContract]
         List<Hangout.Web.Services.Objects.Locations.Place> GetPlaceFollowing(Guid userId, int take, List<Guid> skipList, string zat);
         [OperationContract]
         List<Hangout.Web.Services.Objects.Locations.Place> GetPlaceFollowingByUser(Guid meId, Guid userId, int take, List<Guid> skipList, string zat);
         [OperationContract]
         Hangout.Web.Services.Objects.Locations.Place GetPlaceByID(Guid userId, double lat, double lon, Guid venueId, string zat);
        [OperationContract]
         bool IsCheckedIn(Guid userId, Guid venueId, double lat, double lon, string zat);
        [OperationContract]
        bool CanCheckedIn(Guid userId, Guid venueId, double lat, double lon, string zat);
        [OperationContract]
        CheckInStatus Checkout(Guid userId, Guid venueId, bool intended, double lat, double lon, string zat);
        [OperationContract]
        bool IsCheckedOut(Guid userId, Guid venueId, string zat);
        [OperationContract]
        CheckInStatus CheckIn(Guid userId, Guid venueId, double lat, double lon, string zat);
        [OperationContract]
        List<Hangout.Web.Services.Objects.Locations.Place> GetNearbyPlaces(Guid userId, double latitude, double longitude, string zat);


        
    }
}
