using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;

namespace Hangout.Web.Model
{
    public class BuzzInCategory : TableEntity
    {
        public BuzzInCategory()
        {
        }

        public BuzzInCategory(Guid buzzId,Guid categoryId)
        {
            this.PartitionKey = categoryId.ToString();
            this.RowKey = Guid.NewGuid().ToString();
            BuzzInCategoryID = new Guid(this.RowKey);
            DateTimeStamp = DateTime.Now;
            BuzzID = buzzId;
            CategoryID = categoryId;
        }


        public Guid BuzzInCategoryID { get; set; }

        public Guid BuzzID { get; set; }

        public Guid CategoryID { get; set; }

        public DateTime DateTimeStamp { get; set; }

        internal static string GetPartitionKey(Guid categoryId)
        {
            return categoryId.ToString();
        }
    }


}