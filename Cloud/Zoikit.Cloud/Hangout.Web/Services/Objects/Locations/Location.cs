using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.Objects.Locations
{
    public class Location
    {
        public Guid LocationID { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Address { get; set; }
        public Objects.Locations.City City { get; set; }
    }
}