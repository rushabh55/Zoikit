using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Core.Location
{
    public struct UserLocationData
    {
        public Core.Accounts.UserData UserData;
        public double Distance;
        public Model.Location location;
    }
}