using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Threading;

namespace Hangout.Client
{
    public partial class VenueListItem : UserControl
    {

        public VenueService.Venue Venue
        {
            get { return (VenueService.Venue)GetValue(VenueProperty); }

            set
            {
                SetValue(VenueProperty, value);
            }
        }

       


        public VenueListItem()
        {
            InitializeComponent();
            TiltEffect.TiltableItems.Add(typeof(Grid));
        }

        

        public static readonly DependencyProperty VenueProperty =
       DependencyProperty.Register(
           "Venue",
           typeof(VenueService.Venue),
           typeof(VenueListItem),
           new PropertyMetadata(null, VenueValueChanged));


        private static void VenueValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Client.VenueListItem obj = (Client.VenueListItem)d;
            obj.DisplayData((VenueService.Venue)e.NewValue);
        }

        private void DisplayData(VenueService.Venue venue)
        {
            VenueName.Text = venue.Name;
            if (venue.Location != null)
            {
                if (venue.Location.Address != null)
                {
                    VenueAddress.Text = venue.Location.Address;
                }
                else
                {
                    VenueAddress.Text = " ";
                }
            }
            else
            {
                VenueAddress.Text = " ";
            }

            if (venue.IsFollowing)
            {
                ShowUnfollowBtn.Begin();
            }
            else
            {
                ShowFollowBtn.Begin();
            }

            if (venue.NoOfFollowing == 0)
            {
                if (venue.IsFollowing)
                {
                    StatusLB.Text = "YOU FOLLOW THIS PLACE";
                }
                else
                {
                    StatusLB.Text = "FOLLOW THIS PLACE";
                }
            }
            else
            {
                if (venue.IsFollowing && venue.NoOfFollowing == 1)
                {
                    StatusLB.Text = "YOU FOLLOW THIS PLACE";
                }
                else
                {
                    StatusLB.Text = venue.NoOfFollowing + " people following".ToUpper();
                }
            }

            if (venue.IsCheckedIn)
            {
                if (venue.NoOfCheckedIn > 1)
                {
                    CheckInLBL.Text = venue.NoOfCheckedIn + " of people currently checked in";
                    StartStoryboard();
                }
                
                CheckInBtn.IsEnabled = false;
            }
            else
            {
                if (venue.NoOfCheckedIn > 1)
                {
                    CheckInLBL.Text = venue.NoOfCheckedIn + " of people currently checked in";
                    StartStoryboard();
                }
                CheckInBtn.IsEnabled = true;
            }

            FollowBtn.DataContext = venue;
            UnfollowBtn.DataContext = venue;
            
            
        }

        private int storyBoardCount = 0;
        DispatcherTimer showStoryboardsTimer = new DispatcherTimer();
        private void StartStoryboard()
        {

            showStoryboardsTimer.Interval = new TimeSpan(0, 0, 0); //tick immediately during the first thing
            showStoryboardsTimer.Tick += showStoryboardsTimer_Tick;
            showStoryboardsTimer.Start();

        }

        void showStoryboardsTimer_Tick(object sender, EventArgs e)
        {
            showStoryboardsTimer.Interval += new TimeSpan(0, 0, new Random().Next(4, 6));
            storyBoardCount++;

            if (storyBoardCount == 3)
            {
                storyBoardCount = 1;
            }


            if (storyBoardCount == 1)
            {
                ShowPanel1.Begin();
            }
            if (storyBoardCount == 2)
            {
                ShowPanel2.Begin();
            }

        }

        private void LayoutRoot_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (Venue != null && VenueSelected != null)
            {
                VenueSelected(Venue);
            }
        }

        public void FollowVenue(int id)
        {
            Services.VenueServiceClient.FollowVenueCompleted += VenueServiceClient_FollowVenueCompleted;
            Services.VenueServiceClient.FollowVenueAsync(Core.User.User.UserID, id, Core.User.User.ZAT);
        }

        void VenueServiceClient_FollowVenueCompleted(object sender, VenueService.FollowVenueCompletedEventArgs e)
        {
            Services.VenueServiceClient.FollowVenueCompleted -= VenueServiceClient_FollowVenueCompleted;
            if (e.Error == null)
            {
                if (e.Result == VenueService.FollowResult.Following)
                {


                }
            }
        }

        public void UnfollowVenue(int id)
        {
            Services.VenueServiceClient.UnfollowVenueCompleted += VenueServiceClient_UnfollowVenueCompleted;
            Services.VenueServiceClient.UnfollowVenueAsync(Core.User.User.UserID, id, Core.User.User.ZAT);
        }

        void VenueServiceClient_UnfollowVenueCompleted(object sender, VenueService.UnfollowVenueCompletedEventArgs e)
        {
            Services.VenueServiceClient.UnfollowVenueCompleted -= VenueServiceClient_UnfollowVenueCompleted;
            if (e.Error == null)
            {
                if (e.Result == VenueService.FollowResult.Unfollowed)
                {

                }
            }
        }


        public delegate void VenueSelectedHelper(VenueService.Venue venue);

        public event VenueSelectedHelper VenueSelected;

        public event VenueSelectedHelper VenueCheckInTapped;

        private void CheckInButton_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if(MessageBoxResult.OK==MessageBox.Show("Do you want to check in at "+Venue.Name+"?","Check-in at "+Venue.Name,MessageBoxButton.OKCancel))
            {
                CheckInBtn.IsEnabled = false;

                Core.Location.CurrentLocation.GetLocationCompleted+=CurrentLocation_GetLocationCompleted;
                Core.Location.CurrentLocation.GetCurrentLocation();

               
            }
           
            
        }

        void CurrentLocation_GetLocationCompleted()
        {   
             Core.Location.CurrentLocation.GetLocationCompleted-=CurrentLocation_GetLocationCompleted;
             Services.VenueServiceClient.CheckInCompleted += VenueServiceClient_CheckInCompleted;
             Services.VenueServiceClient.CheckInAsync(Core.User.User.UserID, Venue.VenueID, Core.Location.CurrentLocation.Location.Latitude, Core.Location.CurrentLocation.Location.Longitude, Core.User.User.ZAT);
        }

        void VenueServiceClient_CheckInCompleted(object sender, VenueService.CheckInCompletedEventArgs e)
        {

            Services.VenueServiceClient.CheckInCompleted -= VenueServiceClient_CheckInCompleted;

            if (e.Error == null)
            {
                if (e.Result == VenueService.CheckInStatus.AlreadyCheckedIn)
                {
                    MessageBox.Show("You've already checked-in at this place.", "Already Checked-in", MessageBoxButton.OK);
                    if (Venue != null)
                    {
                        if (VenueCheckInTapped != null)
                        {
                            VenueCheckInTapped(Venue);
                        }
                    }

                }

                if (e.Result == VenueService.CheckInStatus.CheckedIn)
                {
                    MessageBox.Show("You've checked-in at this place.", "Check-in Successful", MessageBoxButton.OK);
                    if (Venue != null)
                    {
                        if (VenueCheckInTapped != null)
                        {
                            VenueCheckInTapped(Venue);
                        }
                    }

                }

                if (e.Result == VenueService.CheckInStatus.CheckedOut)
                {
                    CheckInBtn.IsEnabled = true;
                    MessageBox.Show("You've checked-out at this place.", "Checked-out", MessageBoxButton.OK);
                }

                if (e.Result == VenueService.CheckInStatus.Error)
                {
                    CheckInBtn.IsEnabled = true;
                    MessageBox.Show("Oops, There is a problem with your network. We cant communicate with the cloud.", "Unable to check-in", MessageBoxButton.OK);
                }
                if (e.Result == VenueService.CheckInStatus.FarAway)
                {
                    CheckInBtn.IsEnabled = true;
                    MessageBox.Show("You're far away from this place. Please be near or at this place and then Check-in", "You're away from "+Venue.Name, MessageBoxButton.OK);
                }
            }
            else
            {
                MessageBox.Show("Oops, There is a problem with your network. We cant communicate with the cloud.", "Unable to check-in", MessageBoxButton.OK);
            }
        }

        private void FollowBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            VenueService.Venue ven = (sender as Canvas).DataContext as VenueService.Venue;

            if (ven != null)
            {
                FollowVenue(ven.VenueID);
                Venue.IsFollowing = true;
                Venue.NoOfFollowing++;
                DisplayData(Venue);
                ShowUnfollowBtn.Begin();
            }
        }

        private void UnfollowBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            VenueService.Venue ven = (sender as Canvas).DataContext as VenueService.Venue;

            if (ven != null)
            {
                UnfollowVenue(ven.VenueID);
                Venue.IsFollowing = false;
                Venue.NoOfFollowing--;
                DisplayData(Venue);
                ShowFollowBtn.Begin();
            }
        }


    }
}
