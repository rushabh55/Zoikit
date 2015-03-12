using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.Objects.Buzz
{
    public class BuzzComment
    {
        public Guid CommentID { get; set; }
        public Services.Objects.Accounts.CompactUser User { get; set; }
        public string Comment { get; set; }
        public DateTime DatePosted { get; set; }
    }
}