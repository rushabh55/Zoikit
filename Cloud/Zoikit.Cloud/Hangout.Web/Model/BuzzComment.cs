using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;

namespace Hangout.Web.Model
{
    public class BuzzComment : TableEntity
    {

        public BuzzComment()
        {
        }

        public BuzzComment(Guid userId,Guid buzzId, string text)
        {
            this.PartitionKey = buzzId.ToString();
            this.RowKey = (DateTime.MaxValue.Ticks - DateTime.Now.Ticks).ToString();
            BuzzCommentID = Guid.NewGuid();
            BuzzID = buzzId;
            CommentText = text;
            DateTimeStamp = DateTime.Now;
            UserID=userId;

        }

        public Guid BuzzID { get; set; }

        public string CommentText { get; set; }

        public DateTime DateTimeStamp { get; set; }

        public Guid UserID { get; set; }

        public Guid BuzzCommentID { get; set; }

        internal static string GetPartitionKey(Guid buzzId)
        {
            return buzzId.ToString();
        }
    }


}