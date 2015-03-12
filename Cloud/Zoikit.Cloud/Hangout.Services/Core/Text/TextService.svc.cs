using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Hangout.Services.Core.Text
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TextService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TextService.svc or TextService.svc.cs at the Solution Explorer and start debugging.
    public class TextService : ITextService
    {
        Web.Services.Text.Text obj = new Web.Services.Text.Text();

        public Hangout.Web.Services.Objects.Text.UserText GetUserText(Guid fromId, Guid toId, List<Guid> skipList, int take, string zat)
        {
           return  obj.GetUserText(fromId, toId, skipList, take, zat);
        }

        public List<Hangout.Web.Services.Objects.Text.Text> GetText(Guid userId, List<Guid> skipList, int take, string zat)
        {
            return obj.GetText(userId, skipList, take, zat);
        }

        public Web.Core.Text.TextSentStatus SendText(Guid fromId, Guid toId, string text, string zat)
        {
            return obj.SendText(fromId, toId, text, zat);
        }

        public Web.Services.Objects.Service.Result MarkAsRead(Guid fromId,Guid toId, string zat)
        {
                return obj.MarkAsRead(fromId, toId, zat);
        }

        public Web.Services.Objects.Service.Result MarkAllAsRead(Guid userId, string zat)
        {
               return  obj.MarkAllAsRead(userId, zat);
        }

        public List<Web.Services.Objects.Text.Text> GetTextsAfter(Guid fromId, Guid toId, Guid textId, string zat)
        {
            return obj.GetTextsAfter(fromId, toId, textId, zat);
        }


        public int GetUnreadMessagesCount(Guid userId, string zat)
        {
            return obj.GetUnreadMessagesCount(userId,zat);
        }
    }
}
