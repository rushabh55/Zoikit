using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;

namespace Hangout.Web.Model
{
    public class Buzz : TableEntity
    {

        public Buzz()
        {
        }

        public Buzz(Guid userId,Guid cityId)
        {
            this.PartitionKey = cityId.ToString();
            this.RowKey = (DateTime.MaxValue.Ticks - DateTime.Now.Ticks).ToString();
            CityID = cityId;
            BuzzID = Guid.NewGuid();
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


        public static string GetPartitionKey(Guid cityId)
        {
            return cityId.ToString();
        }
    }


}