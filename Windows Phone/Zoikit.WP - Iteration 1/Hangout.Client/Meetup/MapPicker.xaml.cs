using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using Hangout.Client.GeoLocationSearchService;
using System.Windows.Navigation;

namespace Hangout.Client.Meetup
{
    public partial class MapPicker : PhoneApplicationPage
    {

        string currentCity;
        private int noOfRequests = 0;

        public MapPicker()
        {
            InitializeComponent();
        }

        Pushpin TapPushpin;

        

        private void GestureListener_Tap(object sender, Microsoft.Phone.Controls.GestureEventArgs e)
        {
            try
            {
                ShowProgressBar();
                TapPushpin = new Pushpin();
                TapPushpin.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(p_Tap);

                // get the tapped position relative to the map
                var point = new Point(e.GetPosition(MapControl).X, e.GetPosition(MapControl).Y);
                // get the geolocation of that position
                var location = MapControl.ViewportPointToLocation(point);

                // move the pushpin

                TapPushpin.Location = location;

                TapPushpin.Content = "Loading…";

                // get the address
                // prepare the request
                var request = new GeocodeService.ReverseGeocodeRequest()
                {
                    Location = new GeocodeService.Location()
                    {
                        Latitude = location.Latitude,
                        Longitude = location.Longitude,

                    },
                    Credentials = new GeocodeService.Credentials()
                    {
                        ApplicationId = "Arotrs02ggb1_4iuti_1RBp7LhhhZeIA01ZwDcD92XCxMJAcxyI_PVya15Z6yi0i"
                    }
                };
                // prepare the service
               
                Services.GeocodeServiceClient.ReverseGeocodeCompleted += new EventHandler<GeocodeService.ReverseGeocodeCompletedEventArgs>(service_ReverseGeocodeCompleted);
                // make the request
                Services.GeocodeServiceClient.ReverseGeocodeAsync(request);
                MapControl.Children.Add(TapPushpin);
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

       

        void service_ReverseGeocodeCompleted(object sender, GeocodeService.ReverseGeocodeCompletedEventArgs e)
        {
            Services.GeocodeServiceClient.ReverseGeocodeCompleted -= service_ReverseGeocodeCompleted;
            try
            {
                HideProgressBar();
                // if there is any address
                if (e.Result.Results.Count > 0)
                    // use the first one
                    TapPushpin.Content = e.Result.Results[0].DisplayName;
                // else show invalid position
                else
                    TapPushpin.Content = "";
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
           
           
                
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ShowProgressBar();
                //get current location and center that down!. Dont add a pushpin. 
                Core.Location.CurrentLocation.PositionChanged += CurrentLocation_PositionChanged;
                Core.Location.CurrentLocation.StartWatcher();
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void CurrentLocation_PositionChanged(System.Device.Location.GeoPositionChangedEventArgs<System.Device.Location.GeoCoordinate> e)
        {
            try
            {

                Core.Location.CurrentLocation.StopWatcher();
                Core.Location.CurrentLocation.PositionChanged -= CurrentLocation_PositionChanged;

                //center that down :)

                AddAPushPin(e.Position.Location);

                var request = new GeocodeService.ReverseGeocodeRequest()
                {
                    Location = new GeocodeService.Location()
                    {
                        Latitude = e.Position.Location.Latitude,
                        Longitude = e.Position.Location.Longitude

                    },
                    Credentials = new GeocodeService.Credentials()
                    {
                        ApplicationId = "Arotrs02ggb1_4iuti_1RBp7LhhhZeIA01ZwDcD92XCxMJAcxyI_PVya15Z6yi0i"
                    }
                };
                // prepare the service

                Services.GeocodeServiceClient.ReverseGeocodeCompleted += new EventHandler<GeocodeService.ReverseGeocodeCompletedEventArgs>(MyLocReverseGeoCode_Completed);
                // make the request
                Services.GeocodeServiceClient.ReverseGeocodeAsync(request);

                CurrentLoc = e.Position.Location;
                MapControl.Center = e.Position.Location;
                MapControl.ZoomLevel = 15;
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }

        }

        void MyLocReverseGeoCode_Completed(object sender, GeocodeService.ReverseGeocodeCompletedEventArgs e)
        {
            try
            {

                HideProgressBar();
                // if there is any address
                if (e.Result.Results.Count > 0)
                {
                    currentCity = e.Result.Results[0].Address.Locality;
                    if (String.IsNullOrWhiteSpace(currentCity))
                    {
                        currentCity = e.Result.Results[0].Address.PostalTown;
                    }
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private void AddAPushPin(System.Device.Location.GeoCoordinate geoCoordinate)
        {
            try
            {
                Pushpin p = new Pushpin();
                p.Content = "My Location";
                p.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(p_Tap);
                p.Location = geoCoordinate;
                MapControl.Children.Add(p);
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }


        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SearchTB.Text.ToLower() == "search")
                {
                    MessageBox.Show("Please enter Search Text");
                    SearchTB.Text = "";
                    return;
                }

                if (String.IsNullOrWhiteSpace(SearchTB.Text))
                {
                    MessageBox.Show("Please enter search text");
                    return;
                }

                MapControl.Children.Clear();
                ShowProgressBar();
                
                Services.GeoLocationSearchServiceClient.SearchCompleted += new EventHandler<GeoLocationSearchService.SearchCompletedEventArgs>(client_SearchCompleted);

                noOfRequests = 0;
                StructuredSearchQuery query = new StructuredSearchQuery();
                query.Keyword = SearchTB.Text;
                

                StructuredSearchQuery query2 = new StructuredSearchQuery();
                query2.Location = currentCity;


                
                var request = new GeoLocationSearchService.SearchRequest()
                {
                    SearchOptions = new GeoLocationSearchService.SearchOptions()
                    {
                        Count = 20,
                    },
                    StructuredQuery = query,
                    Credentials = new GeoLocationSearchService.Credentials()
                    {
                        ApplicationId = "Arotrs02ggb1_4iuti_1RBp7LhhhZeIA01ZwDcD92XCxMJAcxyI_PVya15Z6yi0i"
                    }                    
                };

                var request2 = new GeoLocationSearchService.SearchRequest()
                {
                    SearchOptions = new GeoLocationSearchService.SearchOptions()
                    {
                        Count = 20,
                    },
                    StructuredQuery = query2,
                    Credentials = new GeoLocationSearchService.Credentials()
                    {
                        ApplicationId = "Arotrs02ggb1_4iuti_1RBp7LhhhZeIA01ZwDcD92XCxMJAcxyI_PVya15Z6yi0i"
                    }
                };

                var request3 = new GeoLocationSearchService.SearchRequest()
                {
                    SearchOptions = new GeoLocationSearchService.SearchOptions()
                    {
                        Count = 20,
                    },
                    Query = SearchTB.Text,
                    Credentials = new GeoLocationSearchService.Credentials()
                    {
                        ApplicationId = "Arotrs02ggb1_4iuti_1RBp7LhhhZeIA01ZwDcD92XCxMJAcxyI_PVya15Z6yi0i"
                    }
                };

                Services.GeoLocationSearchServiceClient.SearchAsync(request);
                Services.GeoLocationSearchServiceClient.SearchAsync(request2);
                Services.GeoLocationSearchServiceClient.SearchAsync(request3);
            }

            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }


        }

        void client_SearchCompleted(object sender, GeoLocationSearchService.SearchCompletedEventArgs e)
        {
            noOfRequests++;

            if (noOfRequests >= 3)
            {
                Services.GeoLocationSearchServiceClient.SearchCompleted -= client_SearchCompleted;
                if(e.Error != null)
                    if(e.Result != null)
                        if (e.Result.ResultSets[0].Results.Count == 0)
                        {
                            MessageBox.Show("We couldn't find " + SearchTB.Text + ". Please consider changing your search query", "We're Sorry", MessageBoxButton.OK);
                        }
            }
            try
            {
                HideProgressBar();
                if (e.Error == null)
                {
                    MapControl.Children.Clear();
                    
                   
                    foreach (GeoLocationSearchService.SearchResultBase x in e.Result.ResultSets[0].Results)
                    {
                        foreach (GeoLocationSearchService.GeocodeLocation l in x.LocationData.Locations)
                        {
                            Pushpin p = new Pushpin();
                            p.Content = x.Name;
                            p.Location = new System.Device.Location.GeoCoordinate()
                            {
                                Latitude = l.Latitude,
                                Longitude = l.Longitude

                            };
                            p.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(p_Tap);

                            MapControl.Children.Add(p);

                        }
                    }

                    MapControl.ZoomLevel = 12;
                }
                else
                {
                    MessageBox.Show(ErrorText.Description, ErrorText.Caption, MessageBoxButton.OK);
                    return;
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void p_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                Pushpin p = (Pushpin)sender;

                if (p.Content.ToString() == "Loading..")
                {
                    MessageBox.Show("Pick this pushpin after it finishes Loading");
                    return;
                }
                if (MessageBoxResult.OK == MessageBox.Show("Do you want to Pick " + p.Content, "Pick " + p.Content, MessageBoxButton.OKCancel))
                {
                    

                    if (String.IsNullOrWhiteSpace(p.Content.ToString()))
                    {
                        p.Content = "Selected Location";
                    }

                    
                    NavigationService.Navigate(new Uri(Navigation.InitiateHangout + "?Name=" + p.Content + "&lat=" + p.Location.Latitude.ToString() + "&lon=" + p.Location.Longitude.ToString(), UriKind.RelativeOrAbsolute));
                    
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }



        public System.Device.Location.GeoCoordinate CurrentLoc { get; set; }

        private void SearchTB_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                SearchTB.Text = "";
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }


        #region ProgressBar

        int progresscount = 0;

        private void HideProgressBar()
        {
            try
            {
                progresscount = 0;
                if (progresscount <= 0)
                {
                    progresscount = 0;
                    PG.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private void ShowProgressBar()
        {
            try
            {
                progresscount++;
                if (progresscount > 0)
                {
                    PG.Visibility = System.Windows.Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        #endregion




    }
}
