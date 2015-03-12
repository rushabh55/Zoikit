using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hangout.Cloud.Agent.Core
{
    class Foursquare
    {
       
        public static void Run(Guid userId)
        {
            UpdateFoursquareUserData(userId);
            SyncFoursquarePlaces(userId);
            RefreshFoursquareFriends(userId);
        }

        private static void UpdateFoursquareUserData(Guid userId)
        {
            Web.Core.Accounts.Foursquare.UpdateFoursquareData(Web.Core.Accounts.Foursquare.GetFoursquareData(userId));
        }

        public static void SyncFoursquarePlaces(Guid userId)
        {
            Web.Core.Location.Place.SyncFoursquarePlaces(userId);
        }

        public static void RefreshFoursquareFriends(Guid userId)
        {
            Web.Core.Accounts.Foursquare.UpdateFoursquareFriends(userId);
        }


    }
}
