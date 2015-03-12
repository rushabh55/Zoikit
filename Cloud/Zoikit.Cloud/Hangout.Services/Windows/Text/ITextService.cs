using Hangout.Web.Core.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Hangout.Services.Windows.Text
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITextService" in both code and config file together.
    [ServiceContract]
    public interface ITextService
    {
        [OperationContract]
        Hangout.Web.Services.Objects.Text.UserText GetUserText(Guid fromId, Guid toId, List<Guid> skipList, int take, string zat);


        [OperationContract]
        List<Hangout.Web.Services.Objects.Text.Text> GetText(Guid userId, List<Guid> skipList, int take, string zat);


        [OperationContract]
        TextSentStatus SendText(Guid fromId, Guid toId, string text, string zat);


        [OperationContract]
        Web.Services.Objects.Service.Result MarkAsRead(Guid fromId, Guid toId, string zat);


        [OperationContract]
        Web.Services.Objects.Service.Result MarkAllAsRead(Guid userId, string zat);


        [OperationContract]
        List<Hangout.Web.Services.Objects.Text.Text> GetTextsAfter(Guid fromId, Guid toId, Guid textId, string zat);

        [OperationContract]
        int GetUnreadMessagesCount(Guid userId, string zat);
    }
}
