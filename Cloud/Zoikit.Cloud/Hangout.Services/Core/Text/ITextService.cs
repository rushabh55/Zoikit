using Hangout.Web.Core.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Hangout.Services.Core.Text
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITextService" in both code and config file together.
    [ServiceContract]
    public interface ITextService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetUserText?From={fromId}&To={toId}&ZAT={zat}&Results={take}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        Hangout.Web.Services.Objects.Text.UserText GetUserText(Guid fromId, Guid toId, List<Guid> skipList, int take, string zat);
        
        
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetText?UserID={userId}&ZAT={zat}&Results={take}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<Hangout.Web.Services.Objects.Text.Text> GetText(Guid userId, List<Guid> skipList, int take, string zat);
        
        
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "SendText?From={fromId}&To={toId}&ZAT={zat}&Text={text}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        TextSentStatus SendText(Guid fromId, Guid toId, string text, string zat);
         
        
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "MarkAsRead?From={fromId}&To={toId}&ZAT={zat}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        Web.Services.Objects.Service.Result MarkAsRead(Guid fromId,Guid toId, string zat);
         
        
        [OperationContract]
         [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "MarkAllAsRead?UserID={userId}&ZAT={zat}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        Web.Services.Objects.Service.Result MarkAllAsRead(Guid userId, string zat);
        
        
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetTextAfter?From={fromId}&To={toId}&ZAT={zat}&TextID={textId}", BodyStyle = WebMessageBodyStyle.Wrapped)]
         List<Hangout.Web.Services.Objects.Text.Text> GetTextsAfter(Guid fromId, Guid toId, Guid textId, string zat);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetUnreadMessagesCount?UserID={userId}&ZAT={zat}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        int GetUnreadMessagesCount(Guid userId, string zat);

    }
}
