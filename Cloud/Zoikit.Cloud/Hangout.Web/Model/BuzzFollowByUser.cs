using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Model
{
    public class BuzzFollowByUser : TableEntity
    {
        public BuzzFollowByUser()
        {
        }


        public BuzzFollowByUser(Guid buzzID,Guid userId)
        {
            this.PartitionKey = userId.ToString();
            this.RowKey = (DateTime.MaxValue.Ticks - DateTime.Now.Ticks).ToString();
            BuzzFollowID = Guid.NewGuid();
            BuzzID = buzzID;
            UserID=userId;
            DateTimeStamp = DateTime.Now;
        }

        public  Guid BuzzFollowID { get; set; }
        public Guid BuzzID { get; set; }
        public Guid UserID { get; set; }
        public DateTime DateTimeStamp { get; set; }


        internal static string GetPartitionKey(Guid userId)
        {
            return userId.ToString();
        }
    }
}