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
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace Hangout.Client.Meetup
{
    public partial class Hangouts : PhoneApplicationPage
    {

        
        private int infoLoadPoiints;

       
        List<ApplicationBarIconButton> buttons = new List<ApplicationBarIconButton>();

        public int HangoutID { get; set; }
       
        
        public MeetupService.MeetupData Hangout { get; set; }
        public AccountService.UserProfile UserProfile { get; set; }

        public Hangouts()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private int GetID()
        {
            try
            {
                int a = -1;
                int.TryParse(NavigationContext.QueryString["id"], out a);
                return a;
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                return -1;
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = GetID();
                HangoutID = id;
                if (id == -1)
                {
                    NavigateToDashboard();
                    return;
                }

                AddHAngoutDetailsEventHandlers();
                AddDashboardBtn();
                GetHangout(id);
                LoadButtons();
                GetUsers(id);
                GetHangoutComments(id);
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private void AddHAngoutDetailsEventHandlers()
        {
            try
            {
                HangoutDetails.ViewLocationClicked += new EventHandler(HangoutDetails_ViewLocationClicked);
                HangoutDetails.Loaded += new EventHandler(HangoutDetails_Loaded);
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void HangoutDetails_Loaded(object sender, EventArgs e)
        {
            try
            {
                infoLoadPoiints++;
                CheckInfoLoaded();

            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private void CheckInfoLoaded()
        {
            try
            {
                if (infoLoadPoiints == 2)
                {
                    InfoPG.Visibility = System.Windows.Visibility.Collapsed;
                    ShowInfoPage.Begin();
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void HangoutDetails_ViewLocationClicked(object sender, EventArgs e)
        {
            try
            {
                BingMapsDirectionsTask bingMapsDirectionsTask = new BingMapsDirectionsTask();

                // You can specify a label and a geocoordinate for the end point.
                // GeoCoordinate spaceNeedleLocation = new GeoCoordinate(47.6204,-122.3493);
                // LabeledMapLocation spaceNeedleLML = new LabeledMapLocation("Space Needle", spaceNeedleLocation);

                // If you set the geocoordinate parameter to null, the label parameter is used as a search term.
                LabeledMapLocation end = new LabeledMapLocation("Hangout's here", new System.Device.Location.GeoCoordinate() { Latitude = HangoutDetails.HangoutLocation.Latitude, Longitude = HangoutDetails.HangoutLocation.Longitude });
                LabeledMapLocation start = new LabeledMapLocation("You're here", HangoutDetails.MyLocation.Position.Location);
                bingMapsDirectionsTask.End = end;
                bingMapsDirectionsTask.Start = start;

                // If bingMapsDirectionsTask.Start is not set, the user's current Core.Location.CurrentLocation is used as the start point.

                bingMapsDirectionsTask.Show();
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        #region AppBar

        private void AddDashboardBtn()
        {

            try
            {
                if (buttons.Where(o => o.Text == "dashboard").Count() > 0)
                {
                    buttons.Remove(buttons.Where(o => o.Text == "dashboard").FirstOrDefault());
                }

                ApplicationBarIconButton dashboard = new ApplicationBarIconButton();
                dashboard.Click += new EventHandler(dashboardBtn_Click);
                dashboard.Text = "dashboard";
                dashboard.IconUri = new Uri(@"/icons/Dashbord.png", UriKind.Relative);
                buttons.Add(dashboard);

                RefreshButtons();
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }

            
        }

       

        private void RefreshButtons()
        {
            try
            {
                ApplicationBar.Buttons.Clear();
                buttons = buttons.OrderBy(o => o.Text).ToList();
                foreach (ApplicationBarIconButton btn in buttons)
                {
                    ApplicationBar.Buttons.Add(btn);
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }

        }

        private void LoadButtons()
        {
            try
            {
                //check joined
                Services.MeetupServiceClient.JoinedInHangoutCompleted += new EventHandler<MeetupService.JoinedInHangoutCompletedEventArgs>(client_JoinedInHangoutCompleted);
                Services.MeetupServiceClient.JoinedInHangoutAsync(Core.User.User.UserID, HangoutID, Settings.Settings.FacebookAccessToken);
                //checkIn?
                Services.MeetupServiceClient.CanCheckInCompleted += new EventHandler<MeetupService.CanCheckInCompletedEventArgs>(client_CanCheckInCompleted);
                Services.MeetupServiceClient.CanCheckInAsync(Core.User.User.UserID, HangoutID, Settings.Settings.FacebookAccessToken);
                //check pinned
                Services.MeetupServiceClient.IsPinnedCompleted += new EventHandler<MeetupService.IsPinnedCompletedEventArgs>(client_IsPinnedCompleted);
                Services.MeetupServiceClient.IsPinnedAsync(Core.User.User.UserID, HangoutID, Settings.Settings.FacebookAccessToken);
                //is checkedin?
                Services.MeetupServiceClient.IsCheckedInCompleted += new EventHandler<MeetupService.IsCheckedInCompletedEventArgs>(client_IsCheckedInCompleted);
                Services.MeetupServiceClient.IsCheckedInAsync(Core.User.User.UserID, HangoutID, Settings.Settings.FacebookAccessToken);
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void client_IsCheckedInCompleted(object sender, MeetupService.IsCheckedInCompletedEventArgs e)
        {
            Services.MeetupServiceClient.IsCheckedInCompleted -= new EventHandler<MeetupService.IsCheckedInCompletedEventArgs>(client_IsCheckedInCompleted);
            try
            {
                if (e.Error == null)
                {
                    if (e.Result)
                    {
                        //if user is checked in 
                        RemoveJoinUnjoinBtn();
                    }

                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void client_CanCheckInCompleted(object sender, MeetupService.CanCheckInCompletedEventArgs e)
        {
            Services.MeetupServiceClient.CanCheckInCompleted -= new EventHandler<MeetupService.CanCheckInCompletedEventArgs>(client_CanCheckInCompleted);
            try
            {
                if (e.Error == null)
                {
                    if (e.Result)
                    {
                        RemoveCheckInBtn();
                        AddCheckInBtn();
                    }
                    else
                    {
                        RemoveCheckInBtn();
                    }
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        

        private void AddCheckInBtn()
        {
            try
            {
                ApplicationBarIconButton checkin = new ApplicationBarIconButton();
                checkin.Click += new EventHandler(checkin_Click);
                checkin.Text = "checkin";
                checkin.IconUri = new Uri(@"/icons/appbar.check.rest.png", UriKind.Relative);
                buttons.Add(checkin);

                RefreshButtons();
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void checkin_Click(object sender, EventArgs e)
        {
            try
            {
                //checkin
                MainPG.Visibility = System.Windows.Visibility.Visible;
               
                Core.Location.CurrentLocation.PositionChanged += new Core.Location.CurrentLocation.PositionChangedHandler(location_PositionChanged);
                Core.Location.CurrentLocation.StartWatcher();
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void location_PositionChanged(System.Device.Location.GeoPositionChangedEventArgs<System.Device.Location.GeoCoordinate> e)
        {
            try
            {
                Core.Location.CurrentLocation.PositionChanged -= new Core.Location.CurrentLocation.PositionChangedHandler(location_PositionChanged);
                Core.Location.CurrentLocation.StopWatcher();
                //checkin with this Core.Location.CurrentLocation
                Services.MeetupServiceClient.CheckInCompleted += new EventHandler<MeetupService.CheckInCompletedEventArgs>(client_CheckInCompleted);
                Services.MeetupServiceClient.CheckInAsync(Core.User.User.UserID, HangoutID, DateTime.Now, e.Position.Location.Latitude, e.Position.Location.Longitude, Settings.Settings.FacebookAccessToken);
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void client_CheckInCompleted(object sender, MeetupService.CheckInCompletedEventArgs e)
        {
            Services.MeetupServiceClient.CheckInCompleted -= new EventHandler<MeetupService.CheckInCompletedEventArgs>(client_CheckInCompleted);
            try
            {
                MainPG.Visibility = System.Windows.Visibility.Collapsed;
                if (e.Error == null)
                {
                    MainPG.Visibility = System.Windows.Visibility.Collapsed;
                    if (e.Result == MeetupService.CheckInStatus.Success)
                    {
                        MessageBox.Show("You've successfully checked in to this hangout", " Check-in Completed", MessageBoxButton.OK);
                        NavigateToDashboard();
                    }

                    if (e.Result == MeetupService.CheckInStatus.AlreadyCheckedIn)
                    {
                        MessageBox.Show("You're salready checked-in to this hangout", "Already Checked-in", MessageBoxButton.OK);

                    }

                    if (e.Result == MeetupService.CheckInStatus.Error)
                    {
                        MessageBox.Show(ErrorText.Description, ErrorText.Caption, MessageBoxButton.OK);
                        NavigateToDashboard();
                    }

                    if (e.Result == MeetupService.CheckInStatus.HangoutNotFound)
                    {
                        MessageBox.Show("We couldnt find this Hangout", "Hangout not found", MessageBoxButton.OK);
                        NavigateToDashboard();
                    }

                    if (e.Result == MeetupService.CheckInStatus.HangoutTimePassed)
                    {
                        MessageBox.Show("This hangout already happened in the past. You can't check-in.", "Can't check-in", MessageBoxButton.OK);

                    }

                    if (e.Result == MeetupService.CheckInStatus.NotAtHangoutLocation)
                    {
                        MessageBox.Show("We notice you're not at a Hangout Core.Location.CurrentLocation. We cant check you in.", "Can't check-in", MessageBoxButton.OK);

                    }

                    if (e.Result == MeetupService.CheckInStatus.NotJoinedInHangout)
                    {
                        MessageBox.Show("Please join this Hangout before you check in.", "Can't check-in", MessageBoxButton.OK);

                    }

                    if (e.Result == MeetupService.CheckInStatus.EarlyCheckin)
                    {
                        MessageBox.Show("You're checking-in too early. Please wait for the Hangout time and then check-in", "Can't check-in", MessageBoxButton.OK);

                    }
                }
                else
                {
                    MessageBox.Show(ErrorText.Description, ErrorText.Caption, MessageBoxButton.OK);
                    NavigateToDashboard();
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
             
        }

        

        private void RemoveCheckInBtn()
        {
            try
            {
                if (buttons.Where(o => o.Text == "checkin").Count() > 0)
                {
                    buttons.Remove(buttons.Where(o => o.Text == "checkin").FirstOrDefault());
                    RefreshButtons();
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
            
        }

        void client_IsPinnedCompleted(object sender, MeetupService.IsPinnedCompletedEventArgs e)
        {
            Services.MeetupServiceClient.IsPinnedCompleted -= new EventHandler<MeetupService.IsPinnedCompletedEventArgs>(client_IsPinnedCompleted);
            try
            {
                if (e.Error == null)
                {
                    if (e.Result)
                    {

                        AddUnpinButton();

                    }
                    else
                    {
                        AddPinButton();

                    }
                }

            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private void AddUnpinButton()
        {
            try
            {
                if (buttons.Where(o => o.Text == "pin" || o.Text == "unpin").Count() > 0)
                {
                    buttons.Remove(buttons.Where(o => o.Text == "pin" || o.Text == "unpin").FirstOrDefault());
                }

                ApplicationBarIconButton unpin = new ApplicationBarIconButton();
                unpin.Click += new EventHandler(unpin_Click);
                unpin.Text = "unpin";
                unpin.IconUri = new Uri(@"/icons/Pin Cross.PNG", UriKind.Relative);
                buttons.Add(unpin);

                RefreshButtons();
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void unpin_Click(object sender, EventArgs e)
        {
            try
            {
                Services.MeetupServiceClient.UnPinHangoutCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_UnPinHangoutCompleted);
                Services.MeetupServiceClient.UnPinHangoutAsync(Core.User.User.UserID, HangoutID, Settings.Settings.FacebookAccessToken);
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void client_UnPinHangoutCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Services.MeetupServiceClient.UnPinHangoutCompleted -= new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_UnPinHangoutCompleted);
            try
            {
                if (e.Error == null)
                {
                    AddPinButton();
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private void AddPinButton()
        {
            try
            {
                if (buttons.Where(o => o.Text == "pin" || o.Text == "unpin").Count() > 0)
                {
                    buttons.Remove(buttons.Where(o => o.Text == "pin" || o.Text == "unpin").FirstOrDefault());
                }

                ApplicationBarIconButton pin = new ApplicationBarIconButton();
                pin.Click += new EventHandler(pin_Click);
                pin.Text = "pin";
                pin.IconUri = new Uri(@"/icons/Pin.PNG", UriKind.Relative);
                buttons.Add(pin);

                RefreshButtons();
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void pin_Click(object sender, EventArgs e)
        {
            try
            {
                Services.MeetupServiceClient.PinHangoutCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_PinHangoutCompleted);
                Services.MeetupServiceClient.PinHangoutAsync(Core.User.User.UserID, HangoutID, Settings.Settings.FacebookAccessToken);
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void client_PinHangoutCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Services.MeetupServiceClient.PinHangoutCompleted -= new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_PinHangoutCompleted);
            try
            {
                if (e.Error == null)
                {
                    AddUnpinButton();
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        

        void client_JoinedInHangoutCompleted(object sender, MeetupService.JoinedInHangoutCompletedEventArgs e)
        {
            Services.MeetupServiceClient.JoinedInHangoutCompleted += new EventHandler<MeetupService.JoinedInHangoutCompletedEventArgs>(client_JoinedInHangoutCompleted);
            try
            {
                if (e.Error == null)
                {
                    if (e.Result)
                    {
                        //unjoin btn
                        AddUnjoinBtn();

                    }
                    else
                    {
                        //join btn
                        AddJoinBtn();
                    }
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private void AddJoinBtn()
        {
            try
            {
                RemoveJoinUnjoinBtn();

                ApplicationBarIconButton join = new ApplicationBarIconButton();
                join.Click += new EventHandler(join_Click);
                join.Text = "join";
                join.IconUri = new Uri(@"/icons/Profile +.PNG", UriKind.Relative);
                buttons.Add(join);

                RefreshButtons();
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void join_Click(object sender, EventArgs e)
        {
            try
            {
                Services.MeetupServiceClient.JoinHangoutCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_JoinHangoutCompleted);
                Services.MeetupServiceClient.JoinHangoutAsync(Core.User.User.UserID, HangoutID, DateTime.Now, Settings.Settings.FacebookAccessToken);
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }

        }

        void client_JoinHangoutCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Services.MeetupServiceClient.JoinHangoutCompleted -= new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_JoinHangoutCompleted);
            try
            {
                if (e.Error == null)
                {
                    AddUnjoinBtn();
                    Services.MeetupServiceClient.CanCheckInCompleted += new EventHandler<MeetupService.CanCheckInCompletedEventArgs>(client_CanCheckInCompleted);
                    Services.MeetupServiceClient.CanCheckInAsync(Core.User.User.UserID, HangoutID, Settings.Settings.FacebookAccessToken);
                    GetUsers(GetID());
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private void AddUnjoinBtn()
        {
            try
            {
                RemoveJoinUnjoinBtn();

                ApplicationBarIconButton unjoin = new ApplicationBarIconButton();
                unjoin.Click += new EventHandler(unjoin_Click);
                unjoin.Text = "unjoin";
                unjoin.IconUri = new Uri(@"/icons/Profile -.PNG", UriKind.Relative);
                buttons.Add(unjoin);

                RefreshButtons();

            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void unjoin_Click(object sender, EventArgs e)
        {
            try
            {
                Services.MeetupServiceClient.UnJoinHangoutCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_UnJoinHangoutCompleted);
                Services.MeetupServiceClient.UnJoinHangoutAsync(Core.User.User.UserID, HangoutID, DateTime.Now, Settings.Settings.FacebookAccessToken);
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void client_UnJoinHangoutCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Services.MeetupServiceClient.UnJoinHangoutCompleted -= new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_UnJoinHangoutCompleted);
            try
            {
                if (e.Error == null)
                {
                    AddJoinBtn();
                    Services.MeetupServiceClient.CanCheckInCompleted += new EventHandler<MeetupService.CanCheckInCompletedEventArgs>(client_CanCheckInCompleted);
                    Services.MeetupServiceClient.CanCheckInAsync(Core.User.User.UserID, HangoutID, Settings.Settings.FacebookAccessToken);
                    GetUsers(GetID());
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private void RemoveJoinUnjoinBtn()
        {
            try
            {
                if (buttons.Where(o => o.Text == "join" || o.Text == "unjoin").Count() > 0)
                {
                    buttons.Remove(buttons.Where(o => o.Text == "join" || o.Text == "unjoin").FirstOrDefault());
                    RefreshButtons();
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }


#endregion

        
        private void GetHangoutComments(int id)
        {
            try
            {
                BoardPG.Visibility = System.Windows.Visibility.Visible;
                Services.MeetupServiceClient.GetHangoutCommentsCompleted += new EventHandler<MeetupService.GetHangoutCommentsCompletedEventArgs>(client_GetHangoutCommentsCompleted);
                Services.MeetupServiceClient.GetHangoutCommentsAsync(id, Settings.Settings.FacebookAccessToken);
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void client_GetHangoutCommentsCompleted(object sender, MeetupService.GetHangoutCommentsCompletedEventArgs e)
        {
            Services.MeetupServiceClient.GetHangoutCommentsCompleted -= new EventHandler<MeetupService.GetHangoutCommentsCompletedEventArgs>(client_GetHangoutCommentsCompleted);
            try
            {
                if (e.Error == null)
                {

                    CommentLB.HangoutComment = e.Result.ToList();

                }
                else
                {
                    MessageBox.Show(ErrorText.Description, ErrorText.Caption, MessageBoxButton.OK);
                }
                BoardPG.Visibility = System.Windows.Visibility.Collapsed;
                ShowBoard.Begin();
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private void GetUsers(int id)
        {
            try
            {
                UsersPG.Visibility = System.Windows.Visibility.Visible;

                UserLB.UserProfileSelected += UserLB_UserProfileSelected;
                Services.MeetupServiceClient.GetUsersInHangoutCompleted += new EventHandler<MeetupService.GetUsersInHangoutCompletedEventArgs>(client_GetUsersInHangoutCompleted);
                Services.MeetupServiceClient.GetUsersInHangoutAsync(id, Settings.Settings.FacebookAccessToken);
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void UserLB_UserProfileSelected(UserService.User profile)
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.ViewProfile + "?id=" + profile.UserID, UriKind.RelativeOrAbsolute));
                });
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void UserLB_UserProfileSelected(AccountService.UserProfile profile)
        {
            
        }

        void client_GetUsersInHangoutCompleted(object sender, MeetupService.GetUsersInHangoutCompletedEventArgs e)
        {
            Services.MeetupServiceClient.GetUsersInHangoutCompleted -= new EventHandler<MeetupService.GetUsersInHangoutCompletedEventArgs>(client_GetUsersInHangoutCompleted);
            try
            {
                if (e.Error == null)
                {
                    Core.Converters.MeetupUserToAccountUserConverter obj = new Core.Converters.MeetupUserToAccountUserConverter();
                    List<AccountService.UserProfile> profiles = new List<AccountService.UserProfile>();
                    foreach (MeetupService.User1 profile in e.Result.ToList())
                    {
                        profiles.Add((AccountService.UserProfile)obj.Convert(profile, null, null, null));
                    }
                    //UserLB.Users = profiles.ToList();
                   
                    UsersPG.Visibility = System.Windows.Visibility.Collapsed;

                }
                else
                {
                    MessageBox.Show(ErrorText.Description, ErrorText.Caption, MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private void GetHangout(int id)
        {
            try
            {
                Services.MeetupServiceClient.GetHangoutCompleted += new EventHandler<MeetupService.GetHangoutCompletedEventArgs>(client_GetHangoutCompleted);
                Services.MeetupServiceClient.GetHangoutAsync(id, Settings.Settings.FacebookAccessToken);
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
            
        }

        void client_GetHangoutCompleted(object sender, MeetupService.GetHangoutCompletedEventArgs e)
        {
            Services.MeetupServiceClient.GetHangoutCompleted -= new EventHandler<MeetupService.GetHangoutCompletedEventArgs>(client_GetHangoutCompleted);
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.Hangout.User.UserID != 0)
                    {
                        Hangout = e.Result;
                        LoadUserProfile(e.Result.Hangout.User.UserID);
                    }
                }
                else
                {
                    MessageBox.Show(ErrorText.Description, ErrorText.Caption, MessageBoxButton.OK);
                    NavigateToDashboard();
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private void LoadUserProfile(int p)
        {
            try
            {
                AccountService.AccountServiceClient acclient = new AccountService.AccountServiceClient();
                acclient.GetUserProfileCompleted += new EventHandler<AccountService.GetUserProfileCompletedEventArgs>(client_GetUserProfileCompleted);
                acclient.GetUserProfileAsync(p, Settings.Settings.FacebookAccessToken);

            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void client_GetUserProfileCompleted(object sender, AccountService.GetUserProfileCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result != null)
                    {
                        UserProfile = e.Result;
                        FillData();
                    }
                    else
                    {
                        MessageBox.Show(ErrorText.Description, ErrorText.Caption, MessageBoxButton.OK);
                        NavigateToDashboard();
                    }
                }
                else
                {
                    MessageBox.Show(ErrorText.Description, ErrorText.Caption, MessageBoxButton.OK);
                    NavigateToDashboard();
                }
                infoLoadPoiints++;
                CheckInfoLoaded();
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private void FillData()
        {
            try
            {
                UserInfo.Hangout = Hangout;
                UserInfo.Profile = UserProfile;
                HangoutDetails.Hangout = Hangout;
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private void dashboardBtn_Click(object sender, EventArgs e)
        {
            try
            {
                NavigateToDashboard();
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
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

        private void PostBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
//                var xaml = @"<Section xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
//            <Paragraph>
//                <Underline Foreground=""LightBlue""> " + CommentTB.Text + @" ;) </Underline>
//            </Paragraph>
//            <Paragraph Foreground=""Red"">
//                <Bold> " + CommentTB.Text + @"</Bold>
//            </Paragraph>
//        </Section>";

//                rtb.Xaml = xaml;

//                return;
                string comment = CommentTB.Text;
                if (!String.IsNullOrWhiteSpace(comment) && !comment.Contains("Type your comment"))
                {

                    BoardPG.Visibility = System.Windows.Visibility.Visible;
                    CommentTB.Text = "";
                    Services.MeetupServiceClient.AddHangoutCommentCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_AddHangoutCommentCompleted);
                    Services.MeetupServiceClient.AddHangoutCommentAsync(Core.User.User.UserID, HangoutID, comment, DateTime.Now, Settings.Settings.FacebookAccessToken);
                    //refresh comments
                    GetCommentsAfter();

                }
                else
                {
                    MessageBox.Show("Please enter a valid comment");
                }

            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
            
        }

        private void GetCommentsAfter()
        {
            try
            {
                //get the last item
                MeetupService.HangoutComment1 last = CommentLB.HangoutComment.LastOrDefault();
                DateTime after = DateTime.Now.AddYears(-10);

                if (last != null)
                {
                    after = last.DatePosted;
                }

                Services.MeetupServiceClient.GetHangoutCommentsAfterCompleted += new EventHandler<MeetupService.GetHangoutCommentsAfterCompletedEventArgs>(client_GetHangoutCommentsAfterCompleted);
                Services.MeetupServiceClient.GetHangoutCommentsAfterAsync(GetID(), after, Settings.Settings.FacebookAccessToken);
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void client_GetHangoutCommentsAfterCompleted(object sender, MeetupService.GetHangoutCommentsAfterCompletedEventArgs e)
        {
            Services.MeetupServiceClient.GetHangoutCommentsAfterCompleted -= new EventHandler<MeetupService.GetHangoutCommentsAfterCompletedEventArgs>(client_GetHangoutCommentsAfterCompleted);

            try
            {
                if (e.Error == null)
                {
                    if (e.Result.Count > 0)
                    {
                        CommentLB.HangoutComment.AddRange(e.Result);
                    }
                }
                else
                {
                    MessageBox.Show(ErrorText.Description, ErrorText.Caption, MessageBoxButton.OK);
                    NavigateToDashboard();
                }
                BoardPG.Visibility = System.Windows.Visibility.Collapsed;
            }

            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }


        }

        void client_AddHangoutCommentCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Services.MeetupServiceClient.AddHangoutCommentCompleted -= new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_AddHangoutCommentCompleted);
            try
            {
                if (e.Error == null)
                {
                    GetHangoutComments(HangoutID); //refresh comments :)
                }
                else
                {
                    MessageBox.Show(ErrorText.Description, ErrorText.Caption, MessageBoxButton.OK);
                    NavigateToDashboard();
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        #region NotificationControl

        public void AttachEvents()
        {
            try
            {
                NotificationControl.NavigateToHangout += new Controls.Notifications.NotificationReceived.IdHelper(NotificationControl_NavigateToHangout);
                NotificationControl.NavigateToProfile += new Controls.Notifications.NotificationReceived.IdHelper(NotificationControl_NavigateToProfile);
            }
            catch (System.Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);

            }
        }

        void NotificationControl_NavigateToProfile(int id)
        {
            try
            {
                NavigateToViewProfile(id);
            }
            catch (System.Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);

            }
        }

        void NotificationControl_NavigateToHangout(int id)
        {
            try
            {
                NavigateToHangout(id);
            }
            catch (System.Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);

            }
        }

        private void NavigateToHangout(int id)
        {
            try
            {
                //navigate to hangout page
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.Hangout + "?id=" + id, UriKind.RelativeOrAbsolute));
                });
            }
            catch (System.Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);

            }
        }

        private void NavigateToViewProfile(int id)
        {
            try
            {
                //navigate to hangout page
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.ViewProfile + "?id=" + id, UriKind.RelativeOrAbsolute));
                });
            }
            catch (System.Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);

            }
        }

        #endregion    

        private void Image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {

                //                var xaml = @"<Section xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
                //            <Paragraph>
                //                <Underline Foreground=""LightBlue""> " + CommentTB.Text + @" ;) </Underline>
                //            </Paragraph>
                //            <Paragraph Foreground=""Red"">
                //                <Bold> " + CommentTB.Text + @"</Bold>
                //            </Paragraph>
                //        </Section>";

                //                rtb.Xaml = xaml;

                //                return;
                string comment = CommentTB.Text;
                if (!String.IsNullOrWhiteSpace(comment) && !comment.ToLower().StartsWith("comment"))
                {

                    BoardPG.Visibility = System.Windows.Visibility.Visible;
                    CommentTB.Text = "";
                    Services.MeetupServiceClient.AddHangoutCommentCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_AddHangoutCommentCompleted);
                    Services.MeetupServiceClient.AddHangoutCommentAsync(Core.User.User.UserID, HangoutID, comment, DateTime.Now, Settings.Settings.FacebookAccessToken);
                    //refresh comments
                    GetCommentsAfter();

                }
                else
                {
                    MessageBox.Show("Please enter a comment");
                }

            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private void CommentTB_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (CommentTB.Text == "Type your comment here..")
                CommentTB.Text = "";
        }
       
    }
}