using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace Hangout.Web.Model
{
    public class City : TableEntity
    {
        public City()
        {
        }

        public City(string name, Guid CountryId)
        {
            
            this.RowKey = Guid.NewGuid().ToString();
            this.PartitionKey = GetPartitionKey(new Guid(this.RowKey));
            CityID = new Guid(this.RowKey);
            CountryID = CountryId;
        }

        public static string GetPartitionKey(Guid guid)
        {
            return guid.ToString().Substring(0, 3);
        }

        
        public string Name { get; set; }

        public Guid CountryID { get; set; }

        public Guid CityID { get; set; }

    }

   
}