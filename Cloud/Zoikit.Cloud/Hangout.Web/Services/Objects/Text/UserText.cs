using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.Objects.Text
{
    public class UserText
    {
        public Services.Objects.Accounts.CompactUser User { get; set; }
        public List<Text> Texts { get; set; }
    }
}