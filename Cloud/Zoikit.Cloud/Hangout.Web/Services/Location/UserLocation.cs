using MaxMind.GeoIP2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.Location
{
    public class UserLocation
    {



        public Web.Services.Objects.Locations.Location UpdateUserLocation(Guid userId, double latitude, double longitide, string zat)
        {
            try
            {
                    if (Web.Core.Accounts.User.IsValid(userId,zat))
                    {
                        Model.Location loc = new Model.Location(latitude,longitide);
                        Model.Location l= Hangout.Web.Core.Location.UserLocation.UpdateUserLocation(userId,loc);
                        Objects.Locations.Location servLoc =  Web.Services.Location.Location.ConvertLocation(l);
                        return servLoc;
                    }

                    return null;
                   
                
            }
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(Guid.Empty, Web.Core.ClientType.WindowsAzure, ex);
                return null;
            }
        }


        public  List<Web.Core.Location.UserLocationData> GetNearestUsers(Guid userId, int count, string accesstoken)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId, accesstoken))
                {
                    return Hangout.Web.Core.Location.UserLocation.GetNearestUsers(userId, count);
                }

                else { throw new Web.Core.Exceptions.Account.UnAuthorisedAccess(); }
            }
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return null;
            }
        }



        public Objects.Locations.Location UpdateUserLocation(Guid userId, string ip, string zat)
        {
            try
            {

                if (Web.Core.Accounts.User.IsValid(userId, zat))
                {

                    var client = new WebServiceClient(42, "abcdef12345");
                    var omni = client.Omni("128.101.101.101");

                    Model.Location loc = new Model.Location(Double.Parse(omni.Location.Latitude.ToString()),Double.Parse(omni.Location.Longitude.ToString()));
                    Model.Location l = Hangout.Web.Core.Location.UserLocation.UpdateUserLocation(userId, loc);

                    return Web.Services.Location.Location.ConvertLocation(l);

                }

                return null;


            }
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(Guid.Empty, Web.Core.ClientType.WindowsAzure, ex);
                return null;
            }
        }
    }
}