using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Hangout.Client
{
    public partial class VenueList : UserControl
    {



        public VenueList()
        {
            InitializeComponent();
        }

        public event EventHandler MoreBtnClicked;

        private List<VenueService.Venue> venues;

        public List<VenueService.Venue> Venues
        {

            get
            {
                return venues;
            }

            set
            {
                venues = value;
                VenueLB.ItemsSource = null;
                VenueLB.ItemsSource = Venues;
            }
           
        }

        public delegate void VenueSelectedHelper(VenueService.Venue venue);

        public event VenueSelectedHelper VenueSelected;

        public event VenueSelectedHelper VenueCheckInBtnTapped;

        public int selectedVenueId { get; set; }

        private void VenueLB_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (VenueLB.SelectedItem != null)
            {

                VenueService.Venue venue = (VenueService.Venue)VenueLB.SelectedItem;
                selectedVenueId = venue.VenueID;


                if (VenueSelected != null)
                {
                    VenueSelected(venue);
                }


            }
        }

        private void VenueListItem_Loaded_1(object sender, RoutedEventArgs e)
        {
            VenueListItem x = (VenueListItem)sender;
            x.VenueCheckInTapped += x_VenueCheckInTapped;

           
        }

        void x_VenueCheckInTapped(VenueService.Venue venue)
        {
            if (venue != null)
            {
                if (VenueCheckInBtnTapped != null)
                {
                    VenueCheckInBtnTapped(venue);
                }
            }
        }

        private void MoreBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (MoreBtnClicked != null)
            {
                MoreBtnClicked(null, new EventArgs());
            }
        }

        public void ShowMoreButton()
        {
            MorePG.Visibility = System.Windows.Visibility.Collapsed;
            MoreBtn.Visibility = System.Windows.Visibility.Visible;
        }

        public void ShowProgressBar()
        {
            MorePG.Visibility = System.Windows.Visibility.Visible;
            MoreBtn.Visibility = System.Windows.Visibility.Collapsed;
        }

        public void HideDownPanel()
        {
            MorePG.Visibility = System.Windows.Visibility.Collapsed;
            MoreBtn.Visibility = System.Windows.Visibility.Collapsed;
        }

        public void RefreshList()
        {
            VenueLB.DataContext = null;
            VenueLB.ItemsSource = null;
            VenueLB.DataContext = venues;
            VenueLB.ItemsSource = venues;
        }


        
    }
}
