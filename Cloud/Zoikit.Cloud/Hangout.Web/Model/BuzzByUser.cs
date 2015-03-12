using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Model
{
    public class BuzzByUser : TableEntity
    {
        public BuzzByUser()
        {
        }

        public BuzzByUser(Guid buzzId, Guid userId)
        {
            this.PartitionKey = userId.ToString();
            this.RowKey = (DateTime.MaxValue.Ticks - DateTime.Now.Ticks).ToString();
            BuzzID = buzzId;
            UserID=userId;
            DateTimeStamp = DateTime.Now;
           
        }

        public Guid ScreenshotLocationID { get; set; }
        public Guid PlaceID { get; set; }
        public Guid CityID { get; set; }
        public string Text { get; set; }

        public string ImageURL { get; set; }
        public Guid UserID { get; set; }
        public  Guid BuzzID { get; set; }
        public DateTime DateTimeStamp { get; set; }

        public int LikeCount { get; set; }
        public int AmplifyCount { get; set; }

        public int DeAmplifyCount { get; set; }

        public int CommentCount { get; set; }


        public static string GetPartitionKey(Guid userid)
        {
            return userid.ToString();
        }
    }
}