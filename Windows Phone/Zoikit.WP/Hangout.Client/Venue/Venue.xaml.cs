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
using Microsoft.Phone.Tasks;

namespace Hangout.Client
{
    public partial class Venue : PhoneApplicationPage
    {
        public Venue()
        {
            InitializeComponent();
            Notifier.NotificationClicked += Notifier_NotificationClicked;
            Notifier.NotificationDisplayed += Notifier_NotificationDisplayed;
            Notifier.NotificationHidden += Notifier_NotificationHidden;
        }

        #region Notification

        void Notifier_NotificationHidden(object sender, EventArgs e)
        {
            ShowTopLBL.Begin();
        }

        void Notifier_NotificationDisplayed(object sender, EventArgs e)
        {
            HideTopLBL.Begin();
        }

        void Notifier_NotificationClicked(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Notification, UriKind.RelativeOrAbsolute));
            });

            return;
        }


        #endregion


        bool nearbyLoaded = false;

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
            try
            {

                if (!Core.Network.NetworkStatus.IsNetworkAvailable())
                {

                    ShowVenueError.Begin();
                    return;
                }
                VenueBuzzControl.DisableLocationVenueSelector();
                this.DataContext = this;
                int id = GetID();
                VenueID = id;
                if (id == -1)
                {
                    NavigateToDashboard();
                    return;
                }
                PlaceLoader.DisplayText("Looking up for a place...");
                ShowPlaceLoader.Begin();
                LoadVenueInfo();

                CheckInPeopleLoader.DisplayText("Looking up for people who are checked-in");
                ShowPeopleLaoder.Begin();
                NearbyPeopleLoader.DisplayText("Looking out for people nearby...");
                ShowNearbyLoader.Begin();
                FollowingLoader.DisplayText("Looking out for people who are following this place");
                ShowFollowingLoader.Begin();
                LoadPeopleCheckedIn();
                LoadFollowers();
            }
            catch (System.Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private void LoadFollowers()
        {
            FollowingList.MoreBtn.Visibility = System.Windows.Visibility.Collapsed;
            List<int> SkipList = new List<int>();
            SkipList.Add(Core.User.User.UserID);
            if (FollowingList.Users == null)
            {
                FollowingList.Users = new List<UserService.User>();
            }

            SkipList.AddRange(FollowingList.Users.Select(o => o.UserID).ToList());

            Services.UserServiceClient.GetPeopleWhoFollowVenueCompleted+=UserServiceClient_GetPeopleWhoFollowVenueCompleted;
            Services.UserServiceClient.GetPeopleWhoFollowVenueAsync(Core.User.User.UserID, GetID(), 5, new System.Collections.ObjectModel.ObservableCollection<int>(SkipList), Core.User.User.ZAT);
        }

        private void LoadPeopleCheckedIn()
        {
            List<int> SkipList = new List<int>();
            SkipList.Add(Core.User.User.UserID);
            if (PoepleCheckedIn.Users == null)
            {
                PoepleCheckedIn.Users = new List<UserService.User>();
            }

            SkipList.AddRange(PoepleCheckedIn.Users.Select(o => o.UserID).ToList());

            Services.UserServiceClient.GetPeopleCheckedInCompleted += UserServiceClient_GetPeopleCheckedInCompleted;
            Services.UserServiceClient.GetPeopleCheckedInAsync(Core.User.User.UserID, GetID(), 99999, new System.Collections.ObjectModel.ObservableCollection<int>(SkipList), Core.User.User.ZAT);
        }

        void UserServiceClient_GetPeopleCheckedInCompleted(object sender, UserService.GetPeopleCheckedInCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result != null)
                {
                    if (e.Result.Count > 0)
                    {
                        if (PoepleCheckedIn.Users == null)
                        {
                            PoepleCheckedIn.Users = new List<UserService.User>();
                        }

                        PoepleCheckedIn.Users.AddRange(e.Result.Distinct());
                        PoepleCheckedIn.RefreshList();
                        PoepleCheckedIn.Users = PoepleCheckedIn.Users.Distinct().ToList();
                    }
                    else
                    {
                        if (PoepleCheckedIn.Users == null)
                        {
                            PoepleCheckedIn.Users = new List<UserService.User>();
                        }
                    }

                    if (PoepleCheckedIn.Users.Count > 0)
                    {
                        ShowPeople.Begin();
                    }
                    else
                    {
                        ShowNoPeople.Begin();
                    }
                }
                else
                {
                    if (PoepleCheckedIn.Users == null)
                    {
                        PoepleCheckedIn.Users = new List<UserService.User>();
                    }

                    if (PoepleCheckedIn.Users.Count == 0)
                    {
                        ShowNoPeople.Begin();
                    }
                }
            }
            else
            {
                ShowPeopleError.Begin();
            }


            if(!nearbyLoaded)
            {
                nearbyLoaded = true;
                LoadPeopleNearVenue();
            }
        }

        

       

        void UserServiceClient_GetPeopleWhoFollowVenueCompleted(object sender, UserService.GetPeopleWhoFollowVenueCompletedEventArgs e)
        {
            Services.UserServiceClient.GetPeopleWhoFollowVenueCompleted -= UserServiceClient_GetPeopleWhoFollowVenueCompleted;
            if (e.Error == null)
            {
                if (e.Result != null)
                {
                    if (FollowingList.Users == null)
                    {
                        FollowingList.Users = new List<UserService.User>();
                    }

                    FollowingList.Users.AddRange(e.Result.ToList());
                    FollowingList.RefreshList();

                    if (FollowingList.Users.Count > 0)
                    {
                        ShowFollowing.Begin();
                    }
                    else
                    {
                        ShowNoFollowing.Begin();
                    }

                    if (e.Result.Count < 5)
                    {
                        FollowingList.HideDownPanel();
                    }
                    else
                    {
                        FollowingList.ShowMoreButton();
                    }
                }
                else
                {
                    ShowNoFollowing.Begin();
                }
            }
            else
            {
                ShowFollowingError.Begin();
            }
        }

        private void LoadPeopleNearVenue()
        {
            NearByList.MoreBtn.Visibility = System.Windows.Visibility.Collapsed;
            List<int> skiplist = new List<int>();
            if (NearByList.Users != null)
            {
                if (NearByList.Users.Count > 0)
                {
                    skiplist.AddRange(NearByList.Users.Select(o => o.UserID).ToList());
                }
            }

            if (PoepleCheckedIn.Users == null)
            {
                PoepleCheckedIn.Users = new List<UserService.User>();
            }
            skiplist.AddRange(PoepleCheckedIn.Users.Select(o => o.UserID).ToList());

            skiplist.Add(Core.User.User.UserID);

            Services.UserServiceClient.PeopleNearVenueCompleted += UserServiceClient_PeopleNearVenueCompleted;
            Services.UserServiceClient.PeopleNearVenueAsync(Core.User.User.UserID, VenueID, 5, new System.Collections.ObjectModel.ObservableCollection<int>(skiplist), Core.User.User.ZAT);
        }

        void UserServiceClient_PeopleNearVenueCompleted(object sender, UserService.PeopleNearVenueCompletedEventArgs e)
        {
            Services.UserServiceClient.PeopleNearVenueCompleted -= UserServiceClient_PeopleNearVenueCompleted;
            if (e.Error == null)
            {
                if (e.Result != null)
                {
                    if (NearByList.Users == null)
                    {
                        NearByList.Users = new List<UserService.User>();
                    }

                    NearByList.Users.AddRange(e.Result.ToList());
                    NearByList.RefreshList();

                    if (NearByList.Users.Count > 0)
                    {
                        ShowNearby.Begin();
                    }
                    else
                    {
                        ShowNoNearby.Begin();
                    }

                    if (e.Result.Count < 5)
                    {
                        NearByList.HideDownPanel();
                    }
                    else
                    {
                        NearByList.ShowMoreButton();
                    }
                }
                else
                {
                    ShowNoNearby.Begin();
                }
            }
            else
            {
                LoadNearbyError.Begin();
            }
        }

        private void LoadVenueInfo()
        {
            Core.Location.CurrentLocation.GetLocationCompleted += CurrentLocation_GetLocationCompleted;
            Core.Location.CurrentLocation.GetCurrentLocation();
            
        }

        void CurrentLocation_GetLocationCompleted()
        {
            Core.Location.CurrentLocation.GetLocationCompleted -= CurrentLocation_GetLocationCompleted;
            Services.VenueServiceClient.GetVenueByIDCompleted += VenueServiceClient_GetVenueByIDCompleted;
            Services.VenueServiceClient.GetVenueByIDAsync(Core.User.User.UserID,Core.Location.CurrentLocation.Location.Latitude,Core.Location.CurrentLocation.Location.Longitude, VenueID, Core.User.User.ZAT);
        }

        void VenueServiceClient_GetVenueByIDCompleted(object sender, VenueService.GetVenueByIDCompletedEventArgs e)
        {
            Services.VenueServiceClient.GetVenueByIDCompleted -= VenueServiceClient_GetVenueByIDCompleted;

            if (e.Error == null)
            {
                if (e.Result != null)
                {
                    VenueBuzzControl.StartBuzzLoading += VenueBuzzControl_StartBuzzLoading;
                    VenueBuzzControl.SavingBuzz += VenueBuzzControl_SavingBuzz;
                    VenueBuzzControl.BuzzLoaded += VenueBuzzControl_BuzzLoaded;
                    VenueBuzzControl.BuzzLoadError += VenueBuzzControl_BuzzLoadError;
                    VenueBuzzControl.BuzzSelectedEvent += VenueBuzzControl_BuzzSelectedEvent;
                    VenueBuzzControl.NoBuzz += VenueBuzzControl_NoBuzz;
                    VenueBuzzControl.Venue = e.Result;
                    VenueInfoControl.Venue = e.Result;
                    ShowPlace.Begin();
                   
                }
                else
                {
                    ShowNoPlace.Begin();
                }
            }
            else
            {
                ShowVenueError.Begin();
            }
            
        }

        void VenueBuzzControl_NoBuzz(object sender, EventArgs e)
        {
            ShowNoBuzz.Begin();
        }

        void VenueBuzzControl_BuzzSelectedEvent(int id)
        {
            Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.Buzz+"?id="+id, UriKind.RelativeOrAbsolute));
                });
        }

        void VenueBuzzControl_BuzzLoadError(object sender, EventArgs e)
        {
            ShowBuzzError.Begin();
        }

        void VenueBuzzControl_BuzzLoaded(object sender, EventArgs e)
        {
            ShowBuzz.Begin();
        }

        void VenueBuzzControl_SavingBuzz(object sender, EventArgs e)
        {
            ShowBuzz.Begin();
        }

        void VenueBuzzControl_StartBuzzLoading(object sender, EventArgs e)
        {
            BuzzLoader.DisplayText("Looking up for buzz...");
            ShowBuzzLoader.Begin();
        }

        private void AttachEvents()
        {
            FollowingList.MoreBtnClicked += FollowersLB_MoreBtnClicked;
            NearByList.MoreBtnClicked += PeopleNearVenueLB_MoreBtnClicked;
            FollowingList.UserSelected += FollowingList_UserSelected;
            NearByList.UserSelected += NearByList_UserSelected;
            PoepleCheckedIn.UserSelected += PoepleCheckedIn_UserSelected;

        }

        void PoepleCheckedIn_UserSelected(int id)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.ViewProfile + "?id=" + id, UriKind.RelativeOrAbsolute));
            });
        }

        void NearByList_UserSelected(int id)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.ViewProfile + "?id=" + id, UriKind.RelativeOrAbsolute));
            });
        }

        void FollowingList_UserSelected(int id)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.ViewProfile + "?id=" + id, UriKind.RelativeOrAbsolute));
            });
        }

        void PeopleNearVenueLB_MoreBtnClicked(object sender, EventArgs e)
        {
            NearByList.MoreBtn.Visibility = System.Windows.Visibility.Collapsed;
            LoadPeopleNearVenue();
        }

        void FollowersLB_MoreBtnClicked(object sender, EventArgs e)
        {
            FollowingList.MoreBtn.Visibility = System.Windows.Visibility.Collapsed;
            LoadFollowers();
        }

        private void dashboardBtn_Click(object sender, EventArgs e)
        {
            NavigateToDashboard();
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



        public int VenueID { get; set; }

        private int GetID()
        {
            try
            {

                int a = -1;
                int.TryParse(NavigationContext.QueryString["id"], out a);
                return a;
            }
            catch (System.Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                return -1;
            }
        }

        private void YellowAddButton_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ShowBuzz.Begin();
            BuzzSV.ScrollToVerticalOffset(0);
            VenueBuzzControl.AddBuzz();
        }

        
        private void DriveBtn_Click(object sender, System.EventArgs e)
        {
            BingMapsDirectionsTask bingMapsDirectionsTask = new BingMapsDirectionsTask();

            // You can specify a label and a geocoordinate for the end point.
            // GeoCoordinate spaceNeedleLocation = new GeoCoordinate(47.6204,-122.3493);
            // LabeledMapLocation spaceNeedleLML = new LabeledMapLocation("Space Needle", spaceNeedleLocation);

            // If you set the geocoordinate parameter to null, the label parameter is used as a search term.
            LabeledMapLocation spaceNeedleLML = new LabeledMapLocation(VenueInfoControl.Venue.Name, new System.Device.Location.GeoCoordinate { Latitude = VenueInfoControl.Venue.Location.Latitude, Longitude = VenueInfoControl.Venue.Location.Longitude });
            LabeledMapLocation you = new LabeledMapLocation("You're here", new System.Device.Location.GeoCoordinate { Latitude = Core.Location.CurrentLocation.Location.Latitude, Longitude = Core.Location.CurrentLocation.Location.Longitude });
            
            bingMapsDirectionsTask.End = spaceNeedleLML;
            bingMapsDirectionsTask.Start = you;

            // If bingMapsDirectionsTask.Start is not set, the user's current location is used as the start point.

            bingMapsDirectionsTask.Show();
        }

        private void MapBtn_Click(object sender, System.EventArgs e)
        {
            BingMapsTask bingMapsTask = new BingMapsTask();

            //Omit the Center property to use the user's current location.
            bingMapsTask.Center = new System.Device.Location.GeoCoordinate { Latitude = VenueInfoControl.Venue.Location.Latitude, Longitude = VenueInfoControl.Venue.Location.Longitude };

            bingMapsTask.SearchTerm = VenueInfoControl.Venue.Name;
            bingMapsTask.ZoomLevel = 2;

            bingMapsTask.Show();
        }
    }
}