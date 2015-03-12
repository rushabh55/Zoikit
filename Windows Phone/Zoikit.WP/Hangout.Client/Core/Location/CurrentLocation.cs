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
using System.Device.Location;

namespace Hangout.Client.Core.Location
{
    public static class CurrentLocation
    {




        public delegate void GetLocationCompletedHandler();
        public delegate void StatusChangedHandler(GeoPositionStatusChangedEventArgs e);
        public delegate void UserLocationUpdateCompletedHandler();
        public delegate void PositionChangedHandler(GeoPositionChangedEventArgs<GeoCoordinate> e);

        public static UserLocationService.Location Location { get; set; }

        public static event StatusChangedHandler StatusChanged;

        public static event GetLocationCompletedHandler GetLocationCompleted;

        public static event UserLocationUpdateCompletedHandler UserLocationUpdateCompleted;

        public static event PositionChangedHandler PositionChanged;

        public static GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();

       public static  bool LocationUpdated;


        public static bool UpdateToServer { get; set; }

        public static void StartWatcher()
        {
            try
            {
                AttachEvents();
                if (watcher.Status == GeoPositionStatus.Disabled||watcher.Status==GeoPositionStatus.NoData)
                {
                    watcher.Start();
                }
                UpdateToServer = true;
                
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private static void AttachEvents()
        {
            try
            {
               
                watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);
                watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(watcher_StatusChanged);
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        public static void StopWatcher()
        {
            try
            {
                DetachEvents();
                watcher.Stop();
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        public static void DetachEvents()
        {
            try
            {
                watcher.PositionChanged -= new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);
                watcher.StatusChanged -= new EventHandler<GeoPositionStatusChangedEventArgs>(watcher_StatusChanged);
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }


        public static void GetCurrentLocation()
        {
            StartWatcher();
            if (Location != null)
            {
                if (GetLocationCompleted != null)
                {
                    GetLocationCompleted(); //raise an event, if we have no location then stay silent, 
                }
            }
        }

        public static event EventHandler NoLocationFound;

        static void watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            try
            {
                if (e.Status == GeoPositionStatus.Disabled || e.Status == GeoPositionStatus.NoData)
                {
                    //No location 
                    if (NoLocationFound != null)
                    {
                        NoLocationFound(null, new EventArgs());
                    }
                }

                if (StatusChanged != null)
                {
                    StatusChanged(e);
                }

            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }


        }

        static void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            try
            {
                if (watcher.Status == GeoPositionStatus.Ready)
                {



                    //connect to the server here and update the position :)
                   
                    Location = new UserLocationService.Location();
                    if (double.IsNaN(e.Position.Location.Latitude))
                    {
                        return;
                    }
                    Location.Latitude = e.Position.Location.Latitude;

                    if (double.IsNaN(e.Position.Location.Longitude))
                    {
                        return;
                    }


                    Location.Longitude = e.Position.Location.Longitude;

                   


                    if (Core.Authentication.Accounts.LoggedOffAllAccounts())
                    {
                        
                        return;
                    }
                    

                    
                    if (UpdateToServer)
                    {
                        Services.UserLocationServiceClient.UpdateUserLocationCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_UpdateUserLocationCompleted);
                        Services.UserLocationServiceClient.UpdateUserLocationAsync(Core.User.User.UserID, Location, Core.User.User.ZAT);
                    }

                    if (PositionChanged != null)
                    {
                        PositionChanged(e);
                    }

                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }
        public static event EventHandler LocationUpdateError;

        static void client_UpdateUserLocationCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            
            try
            {
                if (e.Error != null)
                {
                    if (LocationUpdateError != null)
                    {
                        LocationUpdateError(null, new EventArgs());
                    }
                    
                }


                Services.UserLocationServiceClient.GetCityByLocationCompleted += UserLocationServiceClient_GetCityByLocationCompleted;
                Services.UserLocationServiceClient.GetCityByLocationAsync(Core.User.User.UserID,Location.Latitude,Location.Longitude,Core.User.User.ZAT);


                LocationUpdated = true;
                if (UserLocationUpdateCompleted != null)
                {
                    UserLocationUpdateCompleted();
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        static void UserLocationServiceClient_GetCityByLocationCompleted(object sender, UserLocationService.GetCityByLocationCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result == null)
                {
                    Settings.Settings.City = e.Result;
                    if (GetLocationCompleted != null)
                    {
                        GetLocationCompleted(); //raise an event
                    }
                }
            }

            if (e.Error != null)
            {
                if (LocationUpdateError != null)
                {
                    LocationUpdateError(null, new EventArgs());
                }
                    
            }
        }








        internal static void UpdateLocationToServer()
        {
            
            if (LocationUpdated)
            {

                if (UserLocationUpdateCompleted != null)
                {
                    UserLocationUpdateCompleted();
                }
            }
            else
            {
                if (Location == null)
                {
                    StartWatcher();
                }
                else
                {
                    if (!Core.Authentication.Accounts.LoggedOffAllAccounts())
                    {
                        if (UpdateToServer)
                        {
                            Services.UserLocationServiceClient.UpdateUserLocationCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_UpdateUserLocationCompleted);
                            Services.UserLocationServiceClient.UpdateUserLocationAsync(Core.User.User.UserID, Location, Core.User.User.ZAT);
                        }
                    }
                }

            }
        }
    }
}
