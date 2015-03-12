using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace Hangout.Web.Model
{
    public class TagInCategory : TableEntity
    {
        public TagInCategory()
        {
        }

        public TagInCategory(Guid categoryId,Guid tagId)
        {
            this.PartitionKey = categoryId.ToString();
            this.RowKey = Guid.NewGuid().ToString();
            TagInCategoryID = new Guid(this.RowKey);
            CategoryID = categoryId;
            TagID = tagId;
        }


        

        public Guid TagInCategoryID { get; set; }

        public Guid CategoryID { get; set; }

        public Guid TagID { get; set; }
    }


}