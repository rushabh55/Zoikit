using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.Objects.Accounts
{
    public class TwitterData
    {
        public Guid UserID { get; set; }

        public string ScreenName { get; set; }

        public string ProfilePicURL { get; set; }

        public string TimeZone { get; set; }

        public string Link { get; set; }

        public string AboutMe { get; set; }

        public long? TwitterID { get; set; }

        public DateTime DateTimeUpdated { get; set; }

        public DateTime DateTimeAdded { get; set; }

        public string AccessToken { get; set; }

        public string AccessTokenSecret { get; set; }

        public int? FollowingCount { get; set; }

        public int? FollowersCount { get; set; }

        public string Location { get; set; }
    }
}