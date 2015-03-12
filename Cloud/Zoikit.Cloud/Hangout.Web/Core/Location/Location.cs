using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Core.Location
{
    public class Location
    {

        public static Model.Location GetLocation(Guid locationId)
        {

            if(locationId==null||locationId==new Guid())
            {
                return null;
            }
            Core.Cloud.TableStorage.InitializeLocationTable();

            return Model.Table.Locations.ExecuteQuery(new TableQuery<Model.Location>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, locationId.ToString()))).FirstOrDefault();
        }

        public static void ReverseGeocodeRequest(ref Model.Location l)
        {
            GeocodeService.GeocodeServiceClient client = new GeocodeService.GeocodeServiceClient("BasicHttpBinding_IGeocodeService");
            var response = client.ReverseGeocode(new GeocodeService.ReverseGeocodeRequest { Location = new GeocodeService.Location { Latitude = l.Latitude, Longitude = l.Longitude }, Credentials = new GeocodeService.Credentials { ApplicationId = "ArUCmVVpgJv12YfInKr0GtEhFhE4SjXGbxdShaJBbneeWIXfUZ5HtdA2GsRFm_U7" } });
            if (response.Results.Count() > 0)
            {
                var result = response.Results[0].Address;
                l.AddressLine = result.AddressLine;
                l.FormattedAddress = result.FormattedAddress;
                l.AdminDistrict = result.AdminDistrict;
                l.CountryID = Core.Location.Country.InsertCountryIfNotExists(result.CountryRegion);
                l.District = result.District;
                l.PostalCode = result.PostalCode;
                l.PostalTown = result.PostalTown;
                l.CityID = Core.Location.City.InsertCityIfNotExists(result.Locality, result.CountryRegion).CityID;
            }


        }

        public static Model.Location InsertLocation(Model.Location location)
        {
            
            if (location != null)
            {
                Model.Location l = new Model.Location(location.Latitude,location.Longitude);
                l.Altitude = location.Altitude;
                l.Course = location.Course;
                l.DateTimeStamp = DateTime.Now;
                l.HorizontalAccuracy = location.HorizontalAccuracy;
                l.Latitude = location.Latitude;
                l.Longitude = location.Longitude;
                l.Speed = location.Speed;
                l.VerticalAccuracy = location.VerticalAccuracy;

                ReverseGeocodeRequest(ref l);

                Model.Table.Locations.Execute(TableOperation.Insert(l));


                return l;
            }

            return null;
        }

        internal static Model.Location GetLocation(Guid locId, Guid cityId)
        {
            Core.Cloud.TableStorage.InitializeLocationTable();

            return Model.Table.Locations.ExecuteQuery(new TableQuery<Model.Location>().Where(TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, cityId.ToString()),TableOperators.And,
                TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, locId.ToString())))).FirstOrDefault();
        }
    }
}