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
using System.Windows.Navigation;
using System.IO.IsolatedStorage;
using Facebook;


namespace Hangout.Client.Meetup
{
    public partial class InitiateHangout : PhoneApplicationPage
    {


        
        DateTime datetime = DateTime.Now.AddYears(-1);
        
        public InitiateHangout()
        {
            InitializeComponent();
        }

        public System.Device.Location.GeoPosition<System.Device.Location.GeoCoordinate> MyLocation { get; set; }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            //dashboard
            NavigateToDashboard();

        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            //shout
            if (ValidatePage())
            {
                HangoutActive = true;
                if (LocationActive)
                {
                    AddHangoutRequest();
                }
            }
        }

        private void AddHangoutRequest()
        {

            try
            {
            ClearState();
            if (ValidatePage())
            {
                HidePage.Begin();
                ShowProgressBar();
                ApplicationBar.IsVisible = false;
                DateTime hangoutdatetime = DatePicker.Value.Value.Date + TimePicker.Value.Value.TimeOfDay;
                while (hangoutdatetime <= DateTime.Now)
                {
                    hangoutdatetime = hangoutdatetime.AddHours(1);
                }
                location.ClientDateTimeStamp = DateTime.Now;
                
                //Services.MeetupServiceClient.InsertHangoutCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_InsertHangoutCompleted);
                //Services.MeetupServiceClient.InsertHangoutAsync(Core.User.User.UserID, false, DescriptionTB.Text, StatusTextTB.Text, GetScreenshotLocation(), location, hangoutdatetime, DateTime.Now, Settings.Settings.FacebookAccessToken);
            }

            }

            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private MeetupService.Location GetScreenshotLocation()
        {
            try
            {
                MeetupService.Location location = new MeetupService.Location();
                if (double.IsNaN(MyLocation.Location.Latitude))
                {
                    return new MeetupService.Location();
                }
                location.Latitude = MyLocation.Location.Latitude;

                if (double.IsNaN(MyLocation.Location.Longitude))
                {
                    return new MeetupService.Location();
                }


                location.Longitude = MyLocation.Location.Longitude;
                if (!double.IsNaN(MyLocation.Location.Altitude))
                {
                    location.Altitude = MyLocation.Location.Altitude;
                }
                if (!double.IsNaN(MyLocation.Location.Course))
                {
                    location.Course = MyLocation.Location.Course;
                }
                if (!double.IsNaN(MyLocation.Location.HorizontalAccuracy))
                {
                    location.HorizontalAccuracy = MyLocation.Location.HorizontalAccuracy;
                }

                if (!double.IsNaN(MyLocation.Location.VerticalAccuracy))
                {
                    location.VerticalAccuracy = MyLocation.Location.VerticalAccuracy;
                }

                if (!double.IsNaN(MyLocation.Location.Speed))
                {
                    location.Speed = MyLocation.Location.Speed;
                }

                location.ClientDateTimeStamp = DateTime.Now;
                //location.UserID = Core.User.User.UserID;

                return location;

            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }

            return null;
        }

        void client_InsertHangoutCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
           // Services.MeetupServiceClient.InsertHangoutCompleted -= new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_InsertHangoutCompleted);
            try
            {
                HideProgressBar();
                if (e.Error == null)
                {
                    if ((bool)FacebookToggleSwitch.IsChecked)
                    {
                        ShareOnFacebook();
                    }
                   
                    MessageBox.Show("We've added your hangout request successfully.","You're done!",MessageBoxButton.OK);
                    NavigateToDashboard();
                }
                else
                {
                    MessageBox.Show("We have turbulance up there in the cloud, Please try again later", "Turbulance", MessageBoxButton.OK);
                    ShowPage.Begin();
                    ApplicationBar.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }


        private void ShareOnFacebook()
        {
            try
            {
                Services.FacebookServiceClient.PostFacebookStatusCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(facebookServiceClient_PostFacebookStatusCompleted);
                
                Services.FacebookServiceClient.PostFacebookStatusAsync(Settings.Settings.FacebookAccessToken, StatusTextTB.Text.Trim(), new Dictionary<string, object>(), "1");
                //var client = new FacebookClient(Settings.Settings.FacebookAccessToken);

                //var args = new Dictionary<string, object>();
                //args["name"] = "";
                //args["link"] = "";
                //args["caption"] = "";
                //args["description"] = "";
                //args["message"] = StatusTextTB.Text.Trim();
                //args["actions"] = "";
                ////args["place"] = "Mumbai";
               

                //client.PostAsync("me/feed", args);
                //client.PostCompleted += new EventHandler<FacebookApiEventArgs>(client_PostCompleted);

            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }


        }

        void facebookServiceClient_PostFacebookStatusCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Services.FacebookServiceClient.PostFacebookStatusCompleted -= facebookServiceClient_PostFacebookStatusCompleted;
                
        }

        void client_PostCompleted(object sender, FacebookApiEventArgs e)
        {
           
  
        }

       

        private bool ValidatePage()
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(StatusTextTB.Text))
                {
                    if (DatePicker.Value.HasValue)
                    {
                        if (DatePicker.Value.Value.Date >= DateTime.Now.Date)
                        {
                            if (TimePicker.Value.HasValue)
                            {
                                if (DatePicker.Value.Value.Date+TimePicker.Value.Value.TimeOfDay>DateTime.Now)
                                {
                                    if (location != null)
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        MessageBox.Show("Please select your location");
                                        NavigateToMapPicker();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("You cannot schedule an hangout in the past");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Please enter your time");
                            }
                        }
                        else
                        {
                            MessageBox.Show("You cannot schedule an Hangout in the past");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please enter hangout date");
                    }
                }
                else
                {
                    MessageBox.Show("Please enter your Shout out text");
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }



            return false;
        }

        private void NavigateToDashboard()
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    if (NavigationService.CanGoBack)
                    {
                        var uri = NavigationService.BackStack.First().Source;

                        if (uri.ToString() == "/MainPage.xaml")
                        {
                            NavigationService.GoBack();
                        }
                        else
                        {
                            NavigationService.Navigate(new Uri(Navigation.Dashboard, UriKind.RelativeOrAbsolute));
                        }
                    }
                    else
                    {
                        NavigationService.Navigate(new Uri(Navigation.Dashboard, UriKind.RelativeOrAbsolute));
                    }

                });
            }
            catch (System.Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        string name;
        MeetupService.Location location;
        String address = "";

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs MyLocation)
        {

            try
            {
                DatePicker.ValueChanged += new EventHandler<DateTimeValueChangedEventArgs>(DatePicker_ValueChanged);
                TimePicker.ValueChanged += new EventHandler<DateTimeValueChangedEventArgs>(TimePicker_ValueChanged);
                ShowProgressBar();
                string name, latitude, longitude;
                if (NavigationContext.QueryString.TryGetValue("Name", out name))
                {
                    if (NavigationContext.QueryString.TryGetValue("lat", out latitude))
                    {
                        if (NavigationContext.QueryString.TryGetValue("lon", out longitude))
                        {

                            

                            GetState();
                            location = new MeetupService.Location();
                            location.Latitude = Double.Parse(latitude);
                            location.Longitude = Double.Parse(longitude);
                            WhereTB.Text = name;

                            var request = new GeocodeService.ReverseGeocodeRequest()
                            {
                                Credentials = new GeocodeService.Credentials()
                                {
                                    ApplicationId = "Arotrs02ggb1_4iuti_1RBp7LhhhZeIA01ZwDcD92XCxMJAcxyI_PVya15Z6yi0i"
                                },
                                Location = new GeocodeService.Location()
                                {
                                    Latitude = location.Latitude,
                                    Longitude = location.Longitude
                                }

                            };
                            ShowProgressBar();
                            Services.GeocodeServiceClient.ReverseGeocodeCompleted += new EventHandler<GeocodeService.ReverseGeocodeCompletedEventArgs>(client_ReverseGeocodeCompleted);
                            Services.GeocodeServiceClient.ReverseGeocodeAsync(request);
                            NavigationContext.QueryString.Clear();

                            NavigationService.RemoveBackEntry();
                            NavigationService.RemoveBackEntry();
                        }
                    }
                }
                else
                {
                    if (!UserDateTimeEditied)
                    {
                        DatePicker.Value = DateTime.Now;
                        var updated = DateTime.Now.AddHours(1);
                        TimePicker.Value = new DateTime(updated.Year, updated.Month, updated.Day,
                                            updated.Hour, 0, 0, DateTime.Now.Kind);
                    }
                }



                Core.Location.CurrentLocation.PositionChanged += new Core.Location.CurrentLocation.PositionChangedHandler(CurrentLocation_PositionChanged);
                Core.Location.CurrentLocation.StartWatcher();
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }

        }

        void TimePicker_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            TimePicker.Value = e.NewDateTime;

        }

        void DatePicker_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            DatePicker.Value = e.NewDateTime;
        }

        void client_ReverseGeocodeCompleted(object sender, GeocodeService.ReverseGeocodeCompletedEventArgs e)
        {
            try
            {
                HideProgressBar();
                if (e.Error == null)
                {
                    try
                    {
                        address = e.Result.Results[0].Address.AddressLine;
                    }
                    catch
                    {

                    }
                }
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
                HideProgressBar();
                if (e.Position != null)
                {
                    LocationActive = true;
                    if (HangoutActive)
                    {
                        AddHangoutRequest();
                    }
                    this.MyLocation = e.Position;
                    Core.Location.CurrentLocation.StopWatcher();
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        






        public bool LocationActive { get; set; }

        public bool HangoutActive { get; set; }

        public bool UserDateTimeEditied;
        
        private void WhereTB_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigateToMapPicker();
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private void NavigateToMapPicker()
        {
            try
            {
                //navigate to map picker
                SaveState();
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.MapPicker, UriKind.RelativeOrAbsolute));
                });
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

       

        private void StatusTextTB_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!UserDateTimeEditied)
                {
                    if (!String.IsNullOrWhiteSpace(StatusTextTB.Text))
                    {
                        ShowProgressBar();
                        
                        Services.TextServiceClient.GetDateTimeCompleted += new EventHandler<TextService.GetDateTimeCompletedEventArgs>(client_GetDateTimeCompleted);
                        Services.TextServiceClient.GetDateTimeAsync(Core.User.User.UserID, StatusTextTB.Text, DateTime.Now, Settings.Settings.FacebookAccessToken);
                    }

                }

            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void client_GetDateTimeCompleted(object sender, TextService.GetDateTimeCompletedEventArgs e)
        {
            try
            {
                HideProgressBar();
                if (e.Error == null)
                {
                    DatePicker.Value = e.Result;
                    TimePicker.Value = e.Result;
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private void TimePicker_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                UserDateTimeEditied = true;
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private void DatePicker_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                UserDateTimeEditied = true;
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                base.OnNavigatedTo(e);
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }

        }

        private void GetState()
        {

            try
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains("StatusTB"))
                {
                    StatusTextTB.Text = IsolatedStorageSettings.ApplicationSettings["StatusTB"].ToString();
                }

                if (IsolatedStorageSettings.ApplicationSettings.Contains("DescriptionTB"))
                {
                    DescriptionTB.Text = IsolatedStorageSettings.ApplicationSettings["DescriptionTB"].ToString();
                }

                if (IsolatedStorageSettings.ApplicationSettings.Contains("When"))
                {
                    DatePicker.Value = (DateTime?)IsolatedStorageSettings.ApplicationSettings["When"];
                }

                if (IsolatedStorageSettings.ApplicationSettings.Contains("Time"))
                {
                    TimePicker.Value = (DateTime?)IsolatedStorageSettings.ApplicationSettings["Time"];
                }

                if (IsolatedStorageSettings.ApplicationSettings.Contains("Where"))
                {
                    WhereTB.Text = IsolatedStorageSettings.ApplicationSettings["Where"].ToString();
                }

                if (IsolatedStorageSettings.ApplicationSettings.Contains("Address"))
                {
                    address = IsolatedStorageSettings.ApplicationSettings["Address"].ToString();
                }

                if (IsolatedStorageSettings.ApplicationSettings.Contains("Location"))
                {
                    location = (MeetupService.Location)IsolatedStorageSettings.ApplicationSettings["Location"];
                }

            }

            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }
        


        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {

            try
            {

                base.OnNavigatedFrom(e);
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }

        }

        private void SaveState()
        {
            //Save State :)
            try
            {
                IsolatedStorageSettings.ApplicationSettings["StatusTB"] = StatusTextTB.Text;
                IsolatedStorageSettings.ApplicationSettings["DescriptionTB"] = DescriptionTB.Text;
                IsolatedStorageSettings.ApplicationSettings["Where"] = WhereTB.Text;
                IsolatedStorageSettings.ApplicationSettings["When"] = DatePicker.Value.Value;
                IsolatedStorageSettings.ApplicationSettings["Time"] = TimePicker.Value.Value;
                IsolatedStorageSettings.ApplicationSettings["Address"] = address;
                IsolatedStorageSettings.ApplicationSettings["Location"] = location;
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }

           
        }

        private static void ClearState()
        {
            try
            {
                IsolatedStorageSettings.ApplicationSettings.Remove("StatusTB");
                IsolatedStorageSettings.ApplicationSettings.Remove("DescriptionTB");
                IsolatedStorageSettings.ApplicationSettings.Remove("Where");
                IsolatedStorageSettings.ApplicationSettings.Remove("When");
                IsolatedStorageSettings.ApplicationSettings.Remove("Time");
                IsolatedStorageSettings.ApplicationSettings.Remove("Address");
                IsolatedStorageSettings.ApplicationSettings.Remove("Location");
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
       
        