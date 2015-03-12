using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.ComponentModel;

namespace Hangout.Client
{
    public partial class LocationSelector : UserControl
    {


        public delegate void VenueEventHelper(VenueService.Venue venue);

        public delegate void CityEventHelper(UserLocationService.City city);

        public event CityEventHelper CitySelected;

        public event VenueEventHelper VenueSelected;

        public LocationSelector()
        {
            InitializeComponent();
        }

        private void CloseBtn_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (!DesignerProperties.IsInDesignTool)
            {
                LocationSelect.VenueSelected += LocationSelect_VenueSelected;
                LocationSelect.CitySelected += LocationSelect_CitySelected;
            }

        }

        void LocationSelect_CitySelected(UserLocationService.City city)
        {
            if (CitySelected != null)
            {
                CitySelected(city);
            }
        }

        void LocationSelect_VenueSelected(VenueService.Venue venue)
        {
            if (VenueSelected != null)
            {
                VenueSelected(venue);
            }
        }

        public void DisableVenues()
        {
            LocationSelect.VenuesSP.Visibility = System.Windows.Visibility.Collapsed;
            LocationSelect.SearchTB.WatermarkText = "Search cities...";
        }

    }
}
