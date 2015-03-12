using Facebook;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hangout.Cloud.Agent.Core
{
    class Facebook
    {
        public static void Run(Guid userId)
        {
            UpdateFacebookUserData(userId);
            GetFacebookLikes(userId);
            RefreshFacebookFriends(userId);
        }

        private static void UpdateFacebookUserData(Guid userId)
        {
            Web.Core.Accounts.Facebook.UpdateFacebookData(Web.Core.Accounts.Facebook.GetFacebookData(userId));
        }

        public static void GetFacebookLikes(Guid userId)
        {
            Web.Core.Tags.FacebookTag.RefreshTags(userId);
        }

        public static void RefreshFacebookFriends(Guid userId)
        {
            Web.Core.Accounts.Facebook.UpdateFacebookFriends(userId);
        }
            


    }
}
