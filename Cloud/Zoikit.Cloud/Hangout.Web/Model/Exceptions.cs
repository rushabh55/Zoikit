using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace Hangout.Web.Model
{
    public class Exceptions : TableEntity
    {
        public Exceptions()
        {

        }

        public Exceptions(Guid userId)
        {
            this.PartitionKey = userId.ToString();
            this.RowKey = Guid.NewGuid().ToString();
            ExceptionID = new Guid(this.RowKey);
            UserID=userId;
        }

        public string ExceptionMessage { get; set; }
        public Guid UserID { get; set; }
        public Guid ExceptionID { get; set; }
        public string ClientType { get; set; }
        public Guid InnerException { get; set; }
        public string StackTrace { get; set; }
        public DateTime DateTimeStamp { get; set; }


    }


}