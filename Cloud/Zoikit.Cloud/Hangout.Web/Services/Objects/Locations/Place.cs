using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.Objects.Locations
{
    public class Place
    {
        public Guid PlaceID { get; set; }
        public string FoursquarePlaceId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Twitter { get; set; }
        public Location Location { get; set; }
        public int NoOfFollowing { get; set; }
        public bool IsFollowing { get; set; }
        public string FoursquareCannonicalURL { get; set; }
        public List<Objects.Tag.Tag> Tags { get; set; }
        public bool IsCheckedIn { get; set; }
        public int NoOfCheckedIn { get; set; }
    }
}