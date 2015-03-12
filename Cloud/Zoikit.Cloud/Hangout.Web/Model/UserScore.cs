using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace Hangout.Web.Model
{
    public class UserScore : TableEntity
    {
        public UserScore()
        {

        }

        public UserScore(Guid userId)
        {
            this.PartitionKey = userId.ToString();
            this.RowKey = (DateTime.MaxValue.Ticks - DateTime.Now.Ticks).ToString();
            UserScoreID = Guid.NewGuid();
            UserID=userId;
        }


        public static string GetPartitionKey(Guid userId)
        {
            return userId.ToString();
        }

        public int ScoreChange { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public int CurrentScore { get; set; }
        public Guid UserID { get; set; }
        public string Description { get; set; }
        public Guid UserScoreID { get; set; }
    }


}