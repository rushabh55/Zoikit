using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.Objects.Tag
{
    public class UserTag
    {
        public Objects.Tag.Tag Tag{ get; set; }
        public bool Following { get; set; }
        public int NoOfLocalPeopleFollowing { get; set; }
    }
}