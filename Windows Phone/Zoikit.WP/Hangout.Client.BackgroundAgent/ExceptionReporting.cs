using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hangout.Client.BackgroundAgent.Core.Exceptions
{
    class ExceptionReporting
    {
        public static void Report(System.Exception e)
        {
            try
            {
                Services.ExceptionReportingServiceClient.AddAnExceptionAsync(Settings.UserData.UserID, "Windows Phone", e.Message, e.StackTrace);
            }
            catch
            {
                //nothing here to do. :P
            }
        }
    }
}
