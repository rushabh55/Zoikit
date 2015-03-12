using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Model
{
    public class BuzzByTag : TableEntity
    {
        public BuzzByTag()
        {

        }

        public BuzzByTag(Guid BuzzId, Guid tagId, Guid cityId)
        {
            this.PartitionKey = GetPartitionKey(tagId, cityId);
            this.RowKey = (DateTime.MaxValue.Ticks - DateTime.Now.Ticks).ToString();
            TagInBuzzID = new Guid();
            BuzzID = BuzzId;
            TagID = tagId;
            DateTimeStamp=DateTime.Now;
        }

        public static string GetPartitionKey(Guid tagId, Guid cityId)
        {
            return cityId.ToString() + "_" + tagId.ToString();
        }

        public Guid TagInBuzzID { get; set; }

        public Guid BuzzID { get; set; }

        public Guid TagID { get; set; }

        public DateTime DateTimeStamp { get; set; }
    }
}