using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Model
{
    public class UserBuzzInteract : Microsoft.WindowsAzure.Storage.Table.TableEntity
    {
        public UserBuzzInteract()
        {

        }


        public UserBuzzInteract(Guid userId, Guid buzzId)
        {
            this.RowKey = (DateTime.MaxValue.Ticks - DateTime.Now.Ticks).ToString();
            this.PartitionKey = userId.ToString();
            this.UserID = userId;
            this.BuzzID = buzzId;
            this.DateTimeStamp = DateTime.Now;
        }

        public Guid UserID { get; set; }
        public Guid BuzzID { get; set; }

        public DateTime DateTimeStamp { get; set; }

    }
}