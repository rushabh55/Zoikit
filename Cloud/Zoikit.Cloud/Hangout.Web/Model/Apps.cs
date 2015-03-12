using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Model
{
    public class Apps : TableEntity
    {
        public Apps() { }
        public Apps(string name)
        {
            this.PartitionKey = (1).ToString();
            this.RowKey = Guid.NewGuid().ToString();
            AppID = new Guid(this.RowKey);
            AppName = name;
            AppToken = Guid.NewGuid();

            this.DateTimeStamp = DateTime.Now;
        }

        public string AppName { get; set; }

        public string AppDescription { get; set; }

        public DateTime DateTimeStamp { get; set; }

        public Guid AppID { get; set; }

        public Guid AppToken { get; set; }

        internal static string GetRowKey(Guid id)
        {
            return id.ToString();
        }

        internal static string GetPartitionKey()
        {
            return (1).ToString();
        }
    }
}