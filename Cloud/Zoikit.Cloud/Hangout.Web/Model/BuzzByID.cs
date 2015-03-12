using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Model
{
    public class BuzzByID : TableEntity
    {

        public BuzzByID()
        {
        }

        public BuzzByID(Guid buzzId, Guid userId)
        {
            this.PartitionKey = BuzzByID.GetPartitionKey(buzzId);
            this.RowKey = buzzId.ToString();
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


        public static string GetPartitionKey(Guid buzzid)
        {
            return buzzid.ToString().Substring(0, 8);
        }

        internal static string GetRowKey(Guid buzzId)
        {
            return buzzId.ToString();
        }
    }
}