using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.Location
{
    public class Location
    {
        

        public Objects.Locations.City GetCityByLocation(Guid userId,double latitude, double longitide,string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(zat))
                {
                    Model.City city = Core.Location.City.GetCityByLocation(latitude, longitide);

                    if (city == null)
                    {
                        return null;
                    }

                    return new Objects.Locations.City{Country=new Objects.Locations.Country{Id=city.CountryID,Name=Core.Location.Country.GetCountryByID(city.CountryID).CountryName},Id=city.CityID,Name=city.Name};
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


        public static Objects.Locations.Location GetLocation(Guid locationId)
        {
            try
            {
                Model.Location location = Hangout.Web.Core.Location.Location.GetLocation(locationId);

                if (location != null)
                {
                   
                        if (location.CityID != null && location.FormattedAddress != null && location.CityID!=Guid.Empty)
                        {
                            return new Objects.Locations.Location { Latitude = location.Latitude, Longitude = location.Longitude, City = new Objects.Locations.City { Id = location.CityID, Name = Core.Location.City.GetCityById(location.CityID).Name , Country = Services.Location.Location.GetCountry(Core.Location.Country.GetCountryByID(Core.Location.City.GetCityById(location.CityID).CountryID))}, LocationID = location.LocationID, Address = location.FormattedAddress };
                        }
                        return new Objects.Locations.Location { Latitude = location.Latitude, Longitude = location.Longitude, LocationID = location.LocationID };
                    

                    
                }

                return null;
                
            }
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(Guid.Empty, Web.Core.ClientType.WindowsAzure, ex);
                return null;
            }
        }

        public static Objects.Locations.Location ConvertLocation(Model.Location location)
        {
            try
            {
                
                if (location != null)
                {

                    if (location.CityID != null && location.FormattedAddress != null && location.CityID != Guid.Empty)
                    {
                       Objects.Locations.Location loc= new Objects.Locations.Location { Latitude = location.Latitude, Longitude = location.Longitude, LocationID = location.LocationID, Address = location.FormattedAddress };
                       Model.City city=Core.Location.City.GetCityById(location.CityID);
                       loc.City  = new Objects.Locations.City { Id=city.CityID,Name=city.Name, Country = Services.Location.Location.GetCountry(Core.Location.Country.GetCountryByID(city.CountryID)) };


                       return loc;
                    }
                    return new Objects.Locations.Location { Latitude = location.Latitude, Longitude = location.Longitude, LocationID = location.LocationID };



                }

                return null;

            }
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(Guid.Empty, Web.Core.ClientType.WindowsAzure, ex);
                return null;
            }
        }

        private static Objects.Locations.Country GetCountry(Model.Country country)
        {
            Objects.Locations.Country c = new Objects.Locations.Country();
            c.Name = country.CountryName;
            c.Id = country.CountryID;

            return c;
        }


        public static Objects.Locations.City ConvertCity(Model.City City)
        {
            Objects.Locations.City city = new Objects.Locations.City();

            city.Name = City.Name;
            city.Id = City.CityID;
            city.Country = new Objects.Locations.Country();
            city.Country.Id = City.CountryID;
            city.Country.Name = Core.Location.Country.GetCountryByID(City.CountryID).CountryName;

            return city;


        }


        public List<Objects.Locations.City> SearchCities(Guid userId, string zat,string query)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(zat))
                {
                    List<Model.City> cities = Core.Location.City.Search(query);

                    if (cities == null)
                    {
                        return new List<Objects.Locations.City>();
                    }

                    if (cities.Count == 0)
                    {
                        return new List<Objects.Locations.City>();
                    }

                    List<Objects.Locations.City> objs = new List<Objects.Locations.City>();

                    foreach(Model.City city in cities)
                    {
                        objs.Add(Location.ConvertCity(city));
                    }

                    return objs;
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

        public static string GetLocationGrid(double latitude, double longitide)
        {
            //lat : -90 - 0 - +90
            //long : -180 - 0 - +180

            return "";
 

        }

        public static List<string> GetNearbyLocationGrid(double latitude, double longitude)
        {
            return new List<string>();
        }

        public double RoundLatitude(double value)
        {
           //convert a long guy to three dec places. :)
           value = Convert.ToDouble(value.ToString("000.000"));

           int offset =(int) (value * 1000) % 10;

           value -= offset / 1000;

           if (value < -180)
           {

           }

           return value; 
        }

        public double RoundLongitude(double value)
        {
            return 0;
        }



    }
}