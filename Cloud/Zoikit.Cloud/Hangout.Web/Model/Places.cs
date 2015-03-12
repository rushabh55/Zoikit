using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hangout.Web.Model
{
    public class Place : TableEntity
    {
        public Place()
        {

        }

        public Place(string name, Guid locationId)
        {
            this.PartitionKey = locationId.ToString();
            PlaceID = Guid.NewGuid();
            LocationID = locationId;
            this.RowKey = PlaceID.ToString();
            Name = name;
        }

        public Guid PlaceID { get; set; }

        public Guid LocationID { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string FoursquarePlaceID { get; set; }

        public string Twitter { get; set; }

        public string FoursquareCannonicalURL { get; set; }

    }
}
