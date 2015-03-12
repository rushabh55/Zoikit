using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.Objects.Buzz
{
    public class Buzz
    {
        public Accounts.CompactUser User { get; set; }
        public bool Liked { get; set; }
        public DateTime Posted { get; set; }
        public string Text { get; set; }
       
        public Guid BuzzID { get; set; }
        public int LikeCount { get; set; }
        public int AmplifyCount { get; set; }
        public int CommentCount { get; set; }

        public bool Amplified { get; set; }

        public bool Deamplified { get; set; }

        public Objects.Locations.City City { get; set; }
        public string ImageURL { get; set; }
        public List<Objects.Buzz.BuzzComment> BuzzComments { get; set; }
        public double Distance { get; set; }
        public Web.Services.Objects.Locations.Location Location { get; set; }
    }
}