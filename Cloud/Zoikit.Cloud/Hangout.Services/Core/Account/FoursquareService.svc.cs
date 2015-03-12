using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Hangout.Services.Core.Account
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "FoursquareService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select FoursquareService.svc or FoursquareService.svc.cs at the Solution Explorer and start debugging.
    public class FoursquareService : IFoursquareService
    {
       
        Web.Services.Account.Foursquare obj = new Web.Services.Account.Foursquare();


        public Web.Core.Accounts.AccountStatus RegisterUser(string accesstoken, string appId, string appToken)
        {
            if (Web.Core.Apps.Apps.IsValid(appId, appToken))
            {
                return obj.InsertFoursquareData(accesstoken);
            }

            return Web.Core.Accounts.AccountStatus.AppInvalid;
        }

        public Web.Core.Accounts.AccountStatus UpdateFoursquareData(Guid userId, string accesstoken, string zat)
        {
            return obj.InsertFoursquareData(userId,accesstoken,zat);
        }


        public Web.Services.Objects.Accounts.FoursquareData GetFoursquareData(Guid userId, string zat)
        {
            return obj.GetFoursquareData(userId,zat);
        }


        public Web.Services.Objects.Accounts.UserData GetUserData(string accesstoken, string appId, string appToken)
        {
            if (Web.Core.Apps.Apps.IsValid(appId, appToken))
            {
                return obj.GetUserData(accesstoken);
            }
            return null;
        }
    }
}
