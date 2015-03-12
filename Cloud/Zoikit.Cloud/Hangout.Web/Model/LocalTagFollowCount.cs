using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Model
{
    public class LocalTagFollowCount : TableEntity
    {
        public LocalTagFollowCount()
        {

        }

        public LocalTagFollowCount(Guid tagId, Guid cityId)
        {
            this.PartitionKey = cityId.ToString();
            this.RowKey = tagId.ToString();
            Count = 0;
            CityID = cityId;
            TagID = tagId;
        }

        public int Count { get; set; }

        public Guid CityID { get; set; }

        public Guid TagID { get; set; }
    }
}