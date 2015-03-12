using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace Hangout.Web.Model
{
    public class Badges : TableEntity
    {

        public Badges()
        { }

        public Badges(string name, string picUrl)
        {
            this.PartitionKey = (1).ToString();
            this.RowKey = Guid.NewGuid().ToString();
            BadgeID = new Guid(this.RowKey);
            BadgeName = name;
            BadgePic=picUrl;
        }

        public string BadgeDescription { get; set; }
        public  string BadgeName { get; set; }
        public string BadgeType { get; set; }
        public  Guid BadgeID { get; set; }
        public  string BadgePic { get; set; }

        
    }


}