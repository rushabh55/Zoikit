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

namespace Hangout.Client.BackgroundAgent
{
    public static class CurrentLocation
    {

        public delegate void GetLocationCompletedHandler();
        public delegate void StatusChangedHandler(GeoPositionStatusChangedEventArgs e);
        public delegate void UserLocationUpdateCompletedHandler(System.ComponentModel.AsyncCompletedEventArgs e);
        public delegate void PositionChangedHandler(GeoPositionChangedEventArgs<GeoCoordinate> e);

        public static UserLocationService.Location Location { get; set; }

        public static event StatusChangedHandler StatusChanged;

        public static event GetLocationCompletedHandler GetLocationCompleted;

        public static event UserLocationUpdateCompletedHandler UserLocationUpdateCompleted;

        public static event PositionChangedHandler PositionChanged;

        public static GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();



        public static bool UpdateToServer { get; set; }

        public static void StartWatcher()
        {
            try
            {

                if (watcher.Status == GeoPositionStatus.Disabled || watcher.Status == GeoPositionStatus.NoData)
                {
                    watcher.Start();
                }
                UpdateToServer = true;
                AttachEvents();
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
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


        static void watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            try
            {

                if (StatusChanged != null)
                {
                    StatusChanged(e);
                }

            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
               
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




                    if (Settings.UserData.UserID != null && Settings.UserData.UserID!=0)
                    {
                        return;
                    }



                    if (UpdateToServer)
                    {
                        Services.UserLocationServiceClient.UpdateUserLocationCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_UpdateUserLocationCompleted);
                        Services.UserLocationServiceClient.UpdateUserLocationAsync(Settings.UserData.UserID, Location,Settings.UserData.ZAT);
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
               
            }
        }

        static void client_UpdateUserLocationCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {

            try
            {


                Services.UserLocationServiceClient.GetCityByLocationCompleted += UserLocationServiceClient_GetCityByLocationCompleted;
                Services.UserLocationServiceClient.GetCityByLocationAsync(Settings.UserData.UserID, Location.Latitude, Location.Longitude, Settings.UserData.ZAT);



                if (UserLocationUpdateCompleted != null)
                {
                    UserLocationUpdateCompleted(e);
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
               
            }
        }

        static void UserLocationServiceClient_GetCityByLocationCompleted(object sender, UserLocationService.GetCityByLocationCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result == null)
                {
                    Settings.City = e.Result;
                    if (GetLocationCompleted != null)
                    {
                        GetLocationCompleted(); //raise an event
                    }
                }
            }
        }







    }
}
