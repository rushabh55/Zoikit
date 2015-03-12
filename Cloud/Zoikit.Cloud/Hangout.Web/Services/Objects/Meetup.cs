using Hangout.Web.Services.Objects.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.Objects
{
    public class Meetup
    {
        public Guid buzzId { get; set; }
        public string StatusText { get; set; }
        public Accounts.User User { get; set; }
        public DateTime DateTimePosted { get; set; }
        public bool IsPinned { get; set; }
        public DateTime HangoutDateTime { get; set; }
       
        
    }
}