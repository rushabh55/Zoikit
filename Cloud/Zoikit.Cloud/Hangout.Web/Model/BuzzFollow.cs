using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace Hangout.Web.Model
{
    public class BuzzFollow : TableEntity
    {
        public BuzzFollow()
        {
        }


        public BuzzFollow(Guid buzzID,Guid userId)
        {
            this.PartitionKey = buzzID.ToString();
            this.RowKey = userId.ToString();
            BuzzFollowID = Guid.NewGuid();
            BuzzID = buzzID;
            UserID=userId;
            DateTimeStamp = DateTime.Now;
        }

        public  Guid BuzzFollowID { get; set; }
        public Guid BuzzID { get; set; }
        public Guid UserID { get; set; }
        public DateTime DateTimeStamp { get; set; }


        internal static string GetPartitionKey(Guid buzzId)
        {
            return buzzId.ToString();
        }
    }


}