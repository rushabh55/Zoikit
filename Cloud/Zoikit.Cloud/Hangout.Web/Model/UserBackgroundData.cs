using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hangout.Web.Model
{
    public class UserBackgroundData : TableEntity
    {
        public UserBackgroundData(Guid userId)
        {
            this.RowKey = userId.ToString();
            this.PartitionKey = Model.UserBackgroundData.GetPartitionKey(userId);
            this.UserID = userId;



        }

        public UserBackgroundData()
        {
        }

        public Guid UserID { get; set; }

        public DateTime RefreshFacebookTokenDateTimeStamp { get; set; }

        public DateTime CompatibilityDateTimeStamp { get; set; }

        public static string GetPartitionKey(Guid userId)
        {
            return userId.ToString().Substring(0, 2);
        }
    }
}
