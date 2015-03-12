using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;

namespace Hangout.Web.Model
{
    public class Role : TableEntity
    {
        public Role()
        {
        }

        public Role(string name)
        {
            this.PartitionKey = (1).ToString();
            this.RowKey = Guid.NewGuid().ToString();
            RoleID = new Guid(this.RowKey);
            Name = name;
        }


        public string Name { get; set; }

        public Guid RoleID { get; set; }

    }


}