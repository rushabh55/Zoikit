using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.Objects.Text
{
    public class Text
    {
        public Services.Objects.Accounts.CompactUser User { get; set; }
        public bool MarkAsRead { get; set; }
        public Guid TextId { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public string TextMessage { get; set; }
    }
}