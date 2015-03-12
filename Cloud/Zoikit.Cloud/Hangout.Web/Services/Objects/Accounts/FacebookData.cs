using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hangout.Web.Services.Objects.Accounts
{
    public class FacebookData
    {
        public Guid UserID { get; set; }

        public DateTime? Birthday { get; set; }

        public string ProfilePicURL { get; set; }

        public int? Age { get; set; }

        public float? TimeZone { get; set; }

        public  string  Link { get; set; }

        public string RelationshipStatus { get; set; }

        public string AboutMe { get; set; }

        public string  Phone { get; set; }

        public string  LargeProfilePicURL { get; set; }

        public string LastName { get; set; }

        public bool? Gender { get; set; }

        public long? FacebookID { get; set; }

        public string FirstName { get; set; }

        public string Email { get; set; }

        public DateTime DateTimeUpdated { get; set; }

        public DateTime DateTimeAdded { get; set; }

        public string AccessToken { get; set; }
    }
}
