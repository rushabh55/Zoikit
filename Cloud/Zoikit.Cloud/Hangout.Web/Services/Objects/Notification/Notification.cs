using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.Objects.Notification
{
    public class Notification
    {
        public Guid NotificationID { get; set; }
        public string Title { get; set; }
        public Guid UserID { get; set; }
        public DateTime? DatetimePosted { get; set; }
        public List<Objects.Accounts.CompactUser> UserList { get; set; }
        public string Description { get; set; }
        public bool MarkAsRead { get; set; }
        public string Param1 { get; set; }
        public string Type1 { get; set; }
        public string Param2 { get; set; }
        public string Type2 { get; set; }
        public string ProfilePicURL { get; set; }
    }
}