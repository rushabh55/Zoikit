using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Hangout.Services.Windows.Location
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UserLocationService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select UserLocationService.svc or UserLocationService.svc.cs at the Solution Explorer and start debugging.
    public class UserLocationService : IUserLocationService
    {
        Web.Services.Location.UserLocation obj = new Web.Services.Location.UserLocation();

        public Web.Services.Objects.Locations.Location UpdateUserLocation(Guid userId, double latitude, double longitude, string zat)
        {
            return obj.UpdateUserLocation(userId, latitude, longitude, zat);
        }


        public List<Web.Core.Location.UserLocationData> GetNearestUsers(Guid userId, int count, string accesstoken)
        {
            return obj.GetNearestUsers(userId, count, accesstoken);
        }


        public Hangout.Web.Services.Objects.Locations.City GetCityByLocation(Guid userId, double latitude, double longitide, string zat)
        {
            Web.Services.Location.Location loc = new Web.Services.Location.Location();
            return loc.GetCityByLocation(userId, latitude, longitide, zat);
        }


        public List<Hangout.Web.Services.Objects.Locations.City> SearchCities(Guid userId, string zat, string query)
        {
            Web.Services.Location.Location loc = new Web.Services.Location.Location();
            return loc.SearchCities(userId, zat, query);
        }
    }
}
