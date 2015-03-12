using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.Objects
{
    public class Trophy
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid TrophyID { get; set; }
        public string TrophyPic { get; set; }
        public string Type { get; set; }
    }
}