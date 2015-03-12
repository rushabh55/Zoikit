using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace Hangout.Web.Model
{
    public class UserTagFollow : TableEntity
    {
        public UserTagFollow()
        {

        }

        public UserTagFollow(Guid userId, Guid tagId,Guid cityId)
        {
            this.PartitionKey = GetPartitionKey(cityId, tagId);
            this.RowKey = userId.ToString();
            UserTagID = Guid.NewGuid();
            DateTimeStamp = DateTime.Now;
            UserID=userId;
            TagID = tagId;
        }

        public bool Explicit { get; set; }

        public Guid UserID { get; set; }

        public DateTime DateTimeStamp { get; set; }

        public Guid TagID { get; set; }

        public Guid UserTagID { get; set; }

        internal static string GetPartitionKey(Guid cityId,Guid tagId)
        {
            if(tagId.ToString().CompareTo(cityId.ToString())<0)
            {
                return tagId.ToString() + "_" + cityId.ToString();
            }
            else
            {
                return cityId.ToString() + "_" + tagId.ToString();
            }
        }

        internal static string GetRowKey(Guid userId)
        {
            return userId.ToString();
        }
    }


}