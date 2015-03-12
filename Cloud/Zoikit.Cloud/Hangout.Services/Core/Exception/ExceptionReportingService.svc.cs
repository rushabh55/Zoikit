using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Hangout.Services.Core.Exception
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ExceptionReportingService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ExceptionReportingService.svc or ExceptionReportingService.svc.cs at the Solution Explorer and start debugging.
    public class ExceptionReportingService : IExceptionReportingService
    {
       
           
        public void AddAnException(Guid userId, string appId, string appToken, string message,string stackTrace)
        {
            

            try
            {
                if (Web.Core.Apps.Apps.IsValid(appId, appToken))
                {
                    Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, appId, message, stackTrace);
                }
                

               
            }
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
            }
        }
    }
}
