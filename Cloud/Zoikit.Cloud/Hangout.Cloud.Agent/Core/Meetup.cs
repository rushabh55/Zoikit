using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hangout.Cloud.Agent.Core
{
    class Buzz
    {
        
        internal static void BuzzAdded(Guid buzzid, Guid userId)
        {

            Web.Model.UserProfile profile = Web.Core.Accounts.User.GetUserProfile(userId);
            Web.Model.BuzzByID hangout = Hangout.Web.Core.Buzz.Buzz.GetBuzzByID(buzzid);
            List<Guid> userIds = Web.Core.Compatibility.Compatibility.GetCompatibleUsers(userId);
            
        }
    }
}
