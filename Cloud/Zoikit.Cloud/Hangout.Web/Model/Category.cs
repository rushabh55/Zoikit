using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace Hangout.Web.Model
{
    public class Category : TableEntity
    {
        public Category()
        {
        }

        public Category(string name)
        {
            this.PartitionKey = (1).ToString();
            this.RowKey = Guid.NewGuid().ToString();
            CategoryID = new Guid(this.RowKey);
            Name = name;
            
        }


        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CategoryID { get; set; }
        public string SmallPicURL { get; set; }
        public string MedPicURL { get; set; }
        public string LargePicURL { get; set; }


    }


}