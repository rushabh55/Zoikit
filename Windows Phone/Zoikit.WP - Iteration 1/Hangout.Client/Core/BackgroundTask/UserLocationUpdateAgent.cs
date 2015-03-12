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
using Microsoft.Phone.Scheduler;

namespace Hangout.Client.Core.BackgroundTask
{
    public class UserLocationUpdateAgent
    {
        static PeriodicTask periodicTask;

        static string periodicTaskName = "UserLocationUpdateAgent";

        static public bool agentsAreEnabled = true;

        public  static void RemoveAgent()
        {
            try
            {
                if (Find() != null)
                {
                    ScheduledActionService.Remove(periodicTaskName);
                }

            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        public static void StartUserUpdateAgentAgent()
        {
            try
            {
                // Variable for tracking enabled status of background agents for this app.
                agentsAreEnabled = true;

                // Obtain a reference to the period task, if one exists
                periodicTask = ScheduledActionService.Find(periodicTaskName) as PeriodicTask;

                // If the task already exists and background agents are enabled for the
                // application, you must remove the task and then add it again to update 
                // the schedule
                if (periodicTask != null)
                {
                    RemoveAgent();
                }

                periodicTask = new PeriodicTask(periodicTaskName);

                // The description is required for periodic agents. This is the string that the user
                // will see in the background services Settings page on the device.
                periodicTask.Description = "Wassup's Meetup Background Service";

                // Place the call to Add in a try block in case the user has disabled agents.
                try
                {
                    ScheduledActionService.Add(periodicTask);
                    // If debugging is enabled, use LaunchForTest to launch the agent in one minute.

                }
                catch (InvalidOperationException exception)
                {
                    if (exception.Message.Contains("BNS Error: The action is disabled"))
                    {

                        agentsAreEnabled = false;

                        //set the tracker to false;
                    }

                    if (exception.Message.Contains("BNS Error: The maximum number of ScheduledActions of this type have already been added."))
                    {
                        // No user action required. The system prompts the user when the hard limit of periodic tasks has been reached.

                    }
                    //set the tracker to false;
                }
                catch (Exception ex)
                {
                    Core.Exceptions.ExceptionReporting.Report(ex);
                    MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        public static PeriodicTask Find()
        {
            try
            {
                return ScheduledActionService.Find(periodicTaskName) as PeriodicTask;

            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                return null;
            }
        }
    }
}
