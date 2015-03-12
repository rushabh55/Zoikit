using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Hangout.Services.Core.Location
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IUserLocationService" in both code and config file together.
    [ServiceContract]
    public interface IUserLocationService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "UpdateUserLocationByIP?UserID={userId}&ZAT={zat}&IP={ip}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        Web.Services.Objects.Locations.Location UpdateUserLocationByIP(Guid userId, string ip, string zat);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "UpdateUserLocation?UserID={userId}&ZAT={zat}&Lat={latitude}&Lon={longitude}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        Web.Services.Objects.Locations.Location UpdateUserLocation(Guid userId, double latitude, double longitude, string zat);
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetCityByLocation?UserID={userId}&ZAT={zat}&Lat={latitude}&Lon={longitude}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        Hangout.Web.Services.Objects.Locations.City GetCityByLocation(Guid userId, double latitude, double longitude, string zat);
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "SearchCities?UserID={userId}&ZAT={zat}&Query={query}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<Hangout.Web.Services.Objects.Locations.City> SearchCities(Guid userId, string zat, string query);


    }
}
