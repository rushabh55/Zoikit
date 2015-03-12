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

namespace Hangout.Client.Core.Startup
{
    public class Launch
    {
        
        public static void ConnectToPnService()
        {
           
            try
            {
                if (Core.User.User.UserID != new Guid ())
                {
                    Core.PushNotification.PushNotification.ConnectToPushNotificationService(); //connect to PN Service :)
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
            }
        }

        public static void StartLocationTracking()
        {
            try
            {
                if (Core.User.User.UserID != new Guid())
                {
                    CheckBackgroundAgent();
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
            }
        }

        private static void CheckBackgroundAgent()
        {
            try
            {
                
                if (Settings.Settings.TrackMe)
                {
                    // If the task already exists and background agents are enabled for the
                    // application, you must remove the task and then add it again to update 
                    // the schedule
                    if (Core.BackgroundTask.UserLocationUpdateAgent.Find() == null)
                    {
                        Core.BackgroundTask.UserLocationUpdateAgent.StartUserUpdateAgentAgent();
                    }
                    else
                    {
                        if (Core.BackgroundTask.UserLocationUpdateAgent.Find().IsEnabled == false)
                        {
                            Core.BackgroundTask.UserLocationUpdateAgent.StartUserUpdateAgentAgent();
                        }
                    }
                }
                else
                {
                    Core.BackgroundTask.UserLocationUpdateAgent.RemoveAgent();
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
            }
        }
    }
}
