using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Hangout.Services.Windows.Location
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IUserLocationService" in both code and config file together.
    [ServiceContract]
    public interface IUserLocationService
    {
        [OperationContract]
       
        Web.Services.Objects.Locations.Location UpdateUserLocation(Guid userId, double latitude, double longitude, string zat);
        [OperationContract]
        
        Hangout.Web.Services.Objects.Locations.City GetCityByLocation(Guid userId, double latitude, double longitude, string zat);
        [OperationContract]
        
        List<Hangout.Web.Services.Objects.Locations.City> SearchCities(Guid userId, string zat, string query);
    }
}
