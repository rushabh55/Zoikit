using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace Hangout.Web.Model
{
    public class Tag : TableEntity
    {

        public Tag()
        {

        }


        public Tag(string name)
        {


            try
            {
                this.PartitionKey = name.Substring(0, 2);

            }
            catch
            {
                try
                {
                    this.PartitionKey = name.Substring(0, 1);
                }
                catch
                {
                    this.PartitionKey = name;
                }
            }

            this.RowKey = name;
            TagID = Guid.NewGuid();
            Name = name;


        }


        public string Name { get; set; }

        public string Description { get; set; }

        public string URL { get; set; }

        public Guid TagID { get; set; }

        internal static string GetPartitionKey(string name)
        {
            return name.Substring(0, 2);
        }
    }


}