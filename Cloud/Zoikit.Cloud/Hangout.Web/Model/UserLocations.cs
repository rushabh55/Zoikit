using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace Hangout.Web.Model
{
    public class UserLocation : TableEntity
    {
        public UserLocation()
        {
        }

        public UserLocation(Guid userId, Guid locationId)
        {

            Core.Cloud.TableStorage.InitializeLocationTable();

            
            Model.Location loc = Model.Table.Locations.ExecuteQuery(new TableQuery<Model.Location>().Where(TableQuery.GenerateFilterConditionForGuid("LocationID", QueryComparisons.Equal, locationId))).FirstOrDefault();
            this.PartitionKey = loc.PartitionKey;
            this.RowKey = (DateTime.MaxValue.Ticks - DateTime.Now.Ticks).ToString();
            UserLocationID = Guid.NewGuid();
            DateTimeStamp = DateTime.Now;
            UserID=userId;
            LocationID = locationId;


        }


        

        public Guid UserID { get; set; }

        public DateTime DateTimeStamp { get; set; }

       

        public Guid LocationID { get; set; }

        public Guid UserLocationID { get; set; }

        internal static string GetPartitonKeyByCityID(Guid cityId)
        {
            return cityId.ToString();
        }
    }


}