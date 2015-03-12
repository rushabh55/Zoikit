using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace Hangout.Web.Model
{
    public class UserReview : TableEntity
    {
        public UserReview()
        {
        }

        public UserReview(Guid fromuserId,Guid touserId)
        {
            this.PartitionKey = touserId.ToString();
            this.RowKey = Guid.NewGuid().ToString();
            UserReviewID = new Guid(this.RowKey);
            FromuserId = fromuserId;
            TouserId = touserId;


        }


        public string Review { get; set; }

        public int Rating { get; set; }

        public Guid TouserId { get; set; }

        public Guid FromuserId { get; set; }

        public Guid UserReviewID { get; set; }
    }


}