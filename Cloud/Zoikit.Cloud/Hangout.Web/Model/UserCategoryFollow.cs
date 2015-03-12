using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace Hangout.Web.Model
{
    public class UserCategoryFollow : TableEntity
    {

        public UserCategoryFollow()
        {
        }

        public UserCategoryFollow(Guid userId, Guid categoryId)
        {
            this.PartitionKey = Model.UserCategoryFollow.GetPartitionKey(userId);
            this.RowKey = Guid.NewGuid().ToString();
            UserCategoryID = new Guid(this.RowKey);
            DateTimeStamp = DateTime.Now;
            UserID=userId;
            CategoryID = categoryId;


        }


       

        public Guid UserID { get; set; }

        public DateTime DateTimeStamp { get; set; }


        public Guid CategoryID { get; set; }

        public Guid UserCategoryID { get; set; }

        internal static string GetPartitionKey(Guid userId)
        {
            return userId.ToString();
        }
    }


}