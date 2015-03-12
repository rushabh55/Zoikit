using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace Hangout.Web.Model
{
    public class Compatibility : TableEntity
    {

        public Compatibility()
        {
        }

        public Compatibility(Guid user1,Guid user2)
        {
            this.PartitionKey = user1.ToString();
            this.RowKey = Guid.NewGuid().ToString();
            User1ID=user1;
            User2ID=user2;
            
        }

        public int Score { get; set; }
        public  Guid User1ID { get; set; }
        public  Guid User2ID { get; set; }
    }


}