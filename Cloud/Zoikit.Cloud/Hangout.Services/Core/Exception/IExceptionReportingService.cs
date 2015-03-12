using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Hangout.Services.Core.Exception
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IExceptionReportingService" in both code and config file together.
    [ServiceContract]
    public interface IExceptionReportingService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "AddAnException?UserID={userId}&AppID={appId}&AppToken={apptoken}&Message={message}&StackTrace={stackTrace}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        void AddAnException(Guid userId, string appId,string apptoken, string message, string stackTrace);
    }
}
