using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.Objects.Accounts
{
    public class UserProfile
    {
        public User User { get; set; }
        public bool Gender { get; set; }
        public int Age { get; set; }
        public string RelationshipStatus { get; set; }

    }
}