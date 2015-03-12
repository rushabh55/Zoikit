using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.Objects.Accounts
{
    public class CompactUser
    {
        public string Name { get; set; }
        public Guid  UserID { get; set; }
        public string  ProfilePicURL { get; set; }

    }
}