using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Hangout.Web.Model
{
    public class Location : TableEntity
    {
        public Location()
        {
        }

        public Location(double latitude, double longitude)
        {
            //get city Id and store it into partition key. 
            Model.Location loc = ReverseGeocode(latitude, longitude);
            this.AddressLine = loc.AddressLine;
            this.AdminDistrict = loc.AdminDistrict;
            this.Altitude = loc.Altitude;
            this.CityID = loc.CityID;
            this.CountryID = loc.CityID;
            this.CountryID = loc.CountryID;
            this.Course = loc.Course;
            this.DateTimeStamp = loc.DateTimeStamp;
            this.District = loc.District;
            this.FormattedAddress = loc.FormattedAddress;
            this.HorizontalAccuracy = loc.HorizontalAccuracy;
            this.Latitude = loc.Latitude;
            this.Longitude = loc.Longitude;
            this.PostalCode = loc.PostalCode;
            this.PostalTown = loc.PostalTown;
            this.Speed = loc.Speed;
            this.VerticalAccuracy = loc.VerticalAccuracy;
            this.PartitionKey = loc.CityID.ToString();
            this.RowKey = Guid.NewGuid().ToString();
            this.LocationID = new Guid(this.RowKey);
        }


        public string Name { get; set; }
        public Guid LocationID { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double Altitude { get; set; }
        public double Course { get; set; }
        public double HorizontalAccuracy { get; set; }
        public double VerticalAccuracy { get; set; }
        public double Speed { get; set; }
        public string AddressLine { get; set; }
        public string AdminDistrict { get; set; }
        public string District { get; set; }
        public string FormattedAddress { get; set; }
        public string PostalCode { get; set; }
        public string PostalTown { get; set; }
        public Guid CityID { get; set; }
        public Guid CountryID { get; set; }
        public DateTime DateTimeStamp { get; set; }


        private static Model.Location ReverseGeocode(double lat, double lon)
        {
            WebRequest request = WebRequest.Create("http://maps.googleapis.com/maps/api/geocode/json?latlng=" + lat + "," + lon + "&sensor=true");
            WebResponse response = request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream());
            JObject obj = JObject.Parse(sr.ReadToEnd());

            if (obj.SelectToken("results") != null)
            {
                Model.Location loc = new Location();

                List<JToken> addComp = obj.SelectToken("results").First().SelectToken("address_components").ToList();

                string city = "", country = "";

                foreach (JToken token in addComp)
                {
                    

                    if (token.SelectToken("types").Where(o => o.ToString() == "locality").Count() > 0)
                    {
                        city = token.SelectToken("long_name").ToString();
                    }

                    if (token.SelectToken("types").Where(o => o.ToString() == "country").Count() > 0)
                    {
                        country = token.SelectToken("long_name").ToString();
                    }

                    if (token.SelectToken("types").Where(o => o.ToString() == "postal_code").Count() > 0)
                    {
                        loc.PostalCode = token.SelectToken("long_name").ToString();
                    }
                    


                }

                if (city != "" && country != "")
                {
                    Model.City c = Core.Location.City.InsertCityIfNotExists(city, country);

                    loc.CountryID = c.CountryID;
                    loc.CityID = c.CityID;
                }

                loc.Altitude = 0.0;
                loc.Course = 0.0;
                loc.DateTimeStamp = DateTime.Now;
                loc.FormattedAddress = obj.SelectToken("results").First().SelectToken("formatted_address").ToString();
                loc.AddressLine = loc.FormattedAddress;
                loc.HorizontalAccuracy = 0.0;
                loc.VerticalAccuracy = 0.0;
                loc.Speed = 0.0;
                loc.PostalTown = city;
                loc.Latitude = lat;
                loc.Longitude = lon;



                return loc;


            }


            return null;
        }
    }


}