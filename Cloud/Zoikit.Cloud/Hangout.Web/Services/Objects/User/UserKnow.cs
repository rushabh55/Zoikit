using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.Objects.User
{
    public class UserKnow
    {
        public Guid UserID { get; set; }
        public Uri ProfilePicURL { get; set; }
        public string Name { get; set; }
        public SocialNetworkType Type { get; set; }
    }
}