using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hangout.Cloud.Agent.Core
{
    class Twitter
    {
        public static void Run(Guid userId)
        {
            UpdateTwitterUserData(userId);
            RefreshTwitterRelationships(userId);
        }

        private static void UpdateTwitterUserData(Guid userId)
        {
            Web.Core.Accounts.Twitter.UpdateTwitterData(userId);
        }

        public static void RefreshTwitterRelationships(Guid userId)
        {
            Web.Core.Accounts.Twitter.RefreshTwitterRelationships(userId);
        }


    }
}
