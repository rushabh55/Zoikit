using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.Objects.User
{
    public class CompleteUserProfile
    {
        public Guid UserID { get; set; }
        public string Name { get; set; }
        public string ProfilePicURL { get; set; }
        public string AboutUs { get; set; }
        public bool IsFollowing { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
        public int BuzzCount { get; set; }
        public Objects.Locations.City City { get; set; }

    }
}