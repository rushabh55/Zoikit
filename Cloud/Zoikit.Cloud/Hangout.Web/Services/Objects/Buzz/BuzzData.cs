using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.Objects.Buzz
{
    public class BuzzData
    {
        public Buzz Buzz { get; set; }
        public string Description { get; set; }
        public Locations.Location Location { get; set; }
        
    }
}