
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace Hangout.Web.Model
{
    public class PlaceTag : TableEntity
    {
        public PlaceTag()
        {
        }

        public PlaceTag(Guid tagId, Guid placeId)
        {
            this.PartitionKey = tagId.ToString();
            this.RowKey = Guid.NewGuid().ToString();
            PlaceTagID = new Guid(this.RowKey);
            DateTimeStamp = DateTime.Now;
            TagID = tagId;
            PlaceID = placeId;


        }

        public DateTime DateTimeStamp { get; set; }

        public Guid PlaceID { get; set; }

        public Guid TagID { get; set; }

        public Guid PlaceTagID { get; set; }

        internal static string GetPartitionKey(Guid tagId)
        {
            return tagId.ToString();
        }
    }


}