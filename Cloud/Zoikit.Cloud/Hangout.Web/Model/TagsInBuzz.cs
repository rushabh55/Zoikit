using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace Hangout.Web.Model
{
    public class TagInBuzz : TableEntity
    {
        public TagInBuzz()
        {

        }

        public TagInBuzz(Guid BuzzId, Guid tagId, Guid cityId)
        {
            this.PartitionKey = cityId.ToString();
            this.RowKey = BuzzId.ToString();
            TagInBuzzID = new Guid();
            BuzzID = BuzzId;
            TagID = tagId;
            DateTimeStamp = DateTime.Now;
        }

        public Guid TagInBuzzID { get; set; }

        public Guid BuzzID { get; set; }

        public Guid TagID { get; set; }

        public DateTime DateTimeStamp { get; set; }



    }


}