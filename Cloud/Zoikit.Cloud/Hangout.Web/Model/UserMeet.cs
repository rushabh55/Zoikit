using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace Hangout.Web.Model
{
    public class UserMeet : TableEntity
    {
        public UserMeet()
        {
        }

        public UserMeet(Guid user1Id, Guid user2Id)
        {
            this.PartitionKey = user1Id.ToString();
            this.RowKey = Guid.NewGuid().ToString();
            UserMeetID = new Guid(this.RowKey);
            User1ID = user1Id;
            User2ID = user2Id;
        }

        public Guid UserMeetID { get; set; }
        public Guid User1ID { get; set; }
        public Guid User2ID { get; set; }
        public DateTime DateTimeStamp{ get; set; }

    }


}