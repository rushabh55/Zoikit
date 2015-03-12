using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace Hangout.Web.Model
{
    public class Avatars : TableEntity
    {

        public Avatars() { }
        public Avatars(string url)
        {
            this.PartitionKey = (1).ToString(); //fixed partition string. 
            this.RowKey = Guid.NewGuid().ToString();
            AvatarID = new Guid(this.RowKey);
            this.URL = url;
        }


        public string URL { get; set; }

        public Guid AvatarID { get; set; }


        public string AvatarType { get; set; }
    }


}