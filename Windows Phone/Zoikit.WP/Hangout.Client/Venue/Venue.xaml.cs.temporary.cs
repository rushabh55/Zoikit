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


        bool peopleLoaded = false;

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
            try
            {
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

                LoadPeopleNearVenue();
                LoadPeopleFollowVenue();
            }
            catch (System.Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private void LoadPeopleFollowVenue()
        {
            FollowersLB.MoreBtn.Visibility = System.Windows.Visibility.Collapsed;
            List<int> skiplist = new List<int>();
            if (FollowersLB.Users != null)
            {
                if (FollowersLB.Users.Count > 0)
                {
                    skiplist = FollowersLB.Users.Select(o => o.UserID).ToList();
                }
            }
            Services.UserServiceClient.GetPeopleWhoFollowVenueCompleted += UserServiceClient_GetPeopleWhoFollowVenueCompleted;
            Services.UserServiceClient.GetPeopleWhoFollowVenueAsync(Core.User.User.UserID, VenueID, 5, new System.Collections.ObjectModel.ObservableCollection<int>(skiplist), Core.User.User.ZAT);
        }

        void UserServiceClient_GetPeopleWhoFollowVenueCompleted(object sender, UserService.GetPeopleWhoFollowVenueCompletedEventArgs e)
        {
            
            Services.UserServiceClient.GetPeopleWhoFollowVenueCompleted -= UserServiceClient_GetPeopleWhoFollowVenueCompleted;

            FollowersLB.MorePG.Visibility = System.Windows.Visibility.Collapsed;
            if (e.Error != null)
            {
                FollowersStatusLBL.Text = "Oops, Looks like your network has gone bad lately. Or there might even be some turbulance up there in the cloud.";
                FollowersLoader.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                if (FollowersLB.Users == null)
                {
                    FollowersLB.Users = new List<UserService.User>();
                }
                FollowersLB.Users.AddRange(e.Result.ToList());
                FollowersLB.RefreshList();
                if (e.Result.Count == 10)
                {
                    FollowersLB.MoreBtn.Visibility = System.Windows.Visibility.Visible;
                }

                if (FollowersLB.Users.Count == 0)
                {
                    FollowersLB.Visibility = System.Windows.Visibility.Collapsed;
                    NoFollowersLBL.Visibility = System.Windows.Visibility.Visible;
                }
            }

            

            if (peopleLoaded)
            {
                //ShowPeople.Begin();
            }
            else
            {
                peopleLoaded = true;
            }

        }

        private void LoadPeopleNearVenue()
        {
            PeopleNearVenueLB.MoreBtn.Visibility = System.Windows.Visibility.Collapsed;
            List<int> skiplist = new List<int>();
            if (PeopleNearVenueLB.Users != null)
            {
                if (FollowersLB.Users.Count > 0)
                {
                    skiplist = PeopleNearVenueLB.Users.Select(o => o.UserID).ToList();
                }
            }

            Services.UserServiceClient.PeopleNearVenueCompleted += UserServiceClient_PeopleNearVenueCompleted;
            Services.UserServiceClient.PeopleNearVenueAsync(Core.User.User.UserID, VenueID, 5, new System.Collections.ObjectModel.ObservableCollection<int>(skiplist), Core.User.User.ZAT);
        }

        void UserServiceClient_PeopleNearVenueCompleted(object sender, UserService.PeopleNearVenueCompletedEventArgs e)
        {
           Services.UserServiceClient.GetPeopleWhoFollowVenueCompleted -= UserServiceClient_GetPeopleWhoFollowVenueCompleted;

            PeopleNearVenueLB.MorePG.Visibility = System.Windows.Visibility.Collapsed;
            if (e.Error != null)
            {
                FollowersStatusLBL.Text = "Oops, Looks like your network has gone bad lately. Or there might even be some turbulance up there in the cloud.";
                FollowersLoader.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                if (PeopleNearVenueLB.Users == null)
                {
                    PeopleNearVenueLB.Users = new List<UserService.User>();
                }
                if (e.Result != null)
                {
                    PeopleNearVenueLB.Users.AddRange(e.Result.ToList());
                    PeopleNearVenueLB.RefreshList();
                    if (e.Result.Count == 10)
                    {
                        PeopleNearVenueLB.MoreBtn.Visibility = System.Windows.Visibility.Visible;
                    }
                }
               

                if (PeopleNearVenueLB.Users.Count == 0)
                {
                    PeopleNearVenueLB.Visibility = System.Windows.Visibility.Collapsed;
                    NoNearPeopleLBL.Visibility = System.Windows.Visibility.Visible;
                }
            }

            

            if (peopleLoaded)
            {
                //ShowPeople.Begin();
            }
            else
            {
                peopleLoaded = true;
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
            FollowersLB.MoreBtnClicked += FollowersLB_MoreBtnClicked;
            PeopleNearVenueLB.MoreBtnClicked += PeopleNearVenueLB_MoreBtnClicked;
            

            
        }

        void PeopleNearVenueLB_MoreBtnClicked(object sender, EventArgs e)
        {
            PeopleNearVenueLB.Visibility = System.Windows.Visibility.Collapsed;
            LoadPeopleNearVenue();
        }

        void FollowersLB_MoreBtnClicked(object sender, EventArgs e)
        {
            FollowersLB.MoreBtn.Visibility = System.Windows.Visibility.Collapsed;
            LoadPeopleFollowVenue();
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
        	// TODO: Add event handler implementation here.
        }

        private void MapBtn_Click(object sender, System.EventArgs e)
        {
        	// TODO: Add event handler implementation here.
        }
    }
}