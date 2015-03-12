using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Hangout.Services.Windows.Discover
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IDiscoverService" in both code and config file together.
    [ServiceContract]
    public interface IDiscoverService
    {
        [OperationContract]
        List<Web.Services.Objects.Discover.DiscoverObj> GetItems(Guid userId, int take, List<Guid> skipList,Guid cityId, string zat);
    }
}
