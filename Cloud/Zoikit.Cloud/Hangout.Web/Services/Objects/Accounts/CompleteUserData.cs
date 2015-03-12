using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.Objects.Accounts
{
    public class CompleteUserData
    {
        public FacebookData FacebookData { get; set; }
        public FoursquareData FoursquareData { get; set; }
        public TwitterData TwitterData { get; set; }
        public UserData  UserData { get; set; }
    }
}