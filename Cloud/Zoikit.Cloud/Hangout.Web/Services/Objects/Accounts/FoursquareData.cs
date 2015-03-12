using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.Objects.Accounts
{
    public class FoursquareData
    {
        public Guid UserID { get; set; }
        public long FoursquareID { get; set; }
        public string AccessToken { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhotoPrefix { get; set; }
        public string PhotoSuffix { get; set; }
        public bool? Gender { get; set; }
        public string HomeCity { get; set; }
        public string Bio { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Twitter { get; set; }
        public string Facebook { get; set; }
        public DateTime DateTimeUpdated { get; set; }
        public DateTime DateTimeAdded { get; set; }
    }
}