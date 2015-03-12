using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.Objects.Accounts
{
    public class UserData
    {
        public Guid UserID { get; set; }
        public DateTime? DateTimeStamp { get; set; }
        public DateTime? DateTimeUpdated { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public DateTime? Birthday { get; set; }
        public string ProfilePicURL { get; set; }
        public bool? Gender{ get; set; }
        public int? Age { get; set; }
        public float? Timezone { get; set; }
        public string RelationshipStatus { get; set; }
        public string Bio { get; set; }
        public string Phone { get; set; }
        public string  LargeProfilePicURL { get; set; }
        public string  DefaultLengthUnits { get; set; }
        public string Email { get; set; }
        public string ZAT { get; set; }
        public string Username { get; set; }

    }
}