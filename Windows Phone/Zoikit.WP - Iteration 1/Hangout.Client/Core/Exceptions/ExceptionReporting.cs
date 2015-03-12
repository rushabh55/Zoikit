using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Hangout.Client.Core.Exceptions
{
    public class ExceptionReporting
    {
        
       

        public static void Report(System.Exception e)
        {
            try
            {
                Services.ExceptionReportingServiceClient.AddAnExceptionAsync(Core.User.User.UserID,AppID.ZoikitAppID,AppID.ZoikitAppToken,e.Message,e.StackTrace);
            }
            catch
            {
                //nothing here to do. :P
            }
        }
    }
}
