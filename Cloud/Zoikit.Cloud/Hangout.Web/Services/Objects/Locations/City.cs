using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.Objects.Locations
{
    public class City
    {
        public  Guid Id { get; set; }
        public string Name { get; set; }
        public Objects.Locations.Country Country{ get; set; }
    }
}