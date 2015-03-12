using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.Objects.Accounts
{
    public class User
    {
        public Guid UserID { get; set; }
        public string Name { get; set; }
        public string ProfilePicURL { get; set; }
        public string AboutUs { get; set; }
        public bool IsFollowing { get; set; }
        public int CommonItems { get; set; }

    }
}