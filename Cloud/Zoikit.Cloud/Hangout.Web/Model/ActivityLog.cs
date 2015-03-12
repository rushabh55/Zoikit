using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;

namespace Hangout.Web.Model
{
    public class ActivityLog : TableEntity
    {
        public ActivityLog() { }
        public ActivityLog(Guid userId, DateTime dateTimeStamp)
        {
            this.PartitionKey = userId.ToString();
            this.RowKey = Guid.NewGuid().ToString();
            UserID = userId;
            ActivityLogID = new Guid(this.RowKey);
            this.DateTimeStamp = dateTimeStamp;
        }

        public Guid UserID { get; set; }

        public Guid ActivityLogID { get; set; }

        public DateTime DateTimeStamp { get; set; }

    }


}