﻿#define DEBUG_AGENT

using System.Windows;
using Microsoft.Phone.Scheduler;
using System.Device.Location;
using System;
using Microsoft.Phone.Shell;

namespace Hangout.Client.BackgroundAgent
{
    public class ScheduledAgent : ScheduledTaskAgent
    {

       
        private static volatile bool _classInitialized;

        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        public ScheduledAgent()
        {
            if (!_classInitialized)
            {
                _classInitialized = true;
                // Subscribe to the managed exception handler
                Deployment.Current.Dispatcher.BeginInvoke(delegate
                {
                    Application.Current.UnhandledException += ScheduledAgent_UnhandledException;
                });
            }

        }

        /// Code to execute on Unhandled Exceptions
        private void ScheduledAgent_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        /// <summary>
        /// Agent that runs a scheduled task
        /// </summary>
        /// <param name="task">
        /// The invoked task
        /// </param>
        /// <remarks>
        /// This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        protected override void OnInvoke(ScheduledTask task)
        {
                if (Settings.UserData.UserID != 0 && Settings.UserData.UserID != -1 && !String.IsNullOrWhiteSpace(Settings.UserData.ZAT))
                {
                    //Add code to perform your task in background
                    CurrentLocation.GetCurrentLocation();
                }
            
        }

       
        
        
    }
}