using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Hangout.Services.Windows.Discover
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DiscoverService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select DiscoverService.svc or DiscoverService.svc.cs at the Solution Explorer and start debugging.
    public class DiscoverService : IDiscoverService
    {

        public List<Web.Services.Objects.Discover.DiscoverObj> GetItems(Guid userId, int take, List<Guid> skipList, Guid cityId, string zat)
        {
            return Web.Services.Discover.Discover.GetItems(userId, take, skipList, cityId, zat);

        }
    }
}
