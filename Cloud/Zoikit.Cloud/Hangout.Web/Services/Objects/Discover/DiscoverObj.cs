using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.Objects.Discover
{
    public class DiscoverObj
    {
        public Objects.Accounts.User User { get; set; }

        public Objects.Tag.UserTag Tag { get; set; }
    }
}