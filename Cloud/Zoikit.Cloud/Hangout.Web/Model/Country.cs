using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;

namespace Hangout.Web.Model
{
    public class Country : TableEntity
    {
        public Country()
        {

        }

        public Country(string name)
        {
            this.PartitionKey = (1).ToString();
            this.RowKey = Guid.NewGuid().ToString();
            CountryID = new Guid(this.RowKey);
            CountryName=name;
        }


        

        public Guid CountryID { get; set; }
        public  string CountryName { get; set; }
    }


}