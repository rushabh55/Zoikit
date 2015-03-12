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
    public partial class LocationPanel : UserControl
    {
        public LocationPanel()
        {
            InitializeComponent();
        }

        public delegate void EventHelper();

      

        public event EventHelper NavigateToLocationSelectorPage;

       

        

        private void LocationBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (NavigateToLocationSelectorPage != null)
            {
                NavigateToLocationSelectorPage();
            }
        }

        private void TextBlock_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (NavigateToLocationSelectorPage != null)
            {
                NavigateToLocationSelectorPage();
            }
        }

        private void TextBlock_Tap_2(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (NavigateToLocationSelectorPage != null)
            {
                NavigateToLocationSelectorPage();
            }
        }

        public void UpdateLocation(UserLocationService.Location location)
        {
            if (location != null)
            {
                CityTB.Text = location.City.Name;
                CountryTB.Text = location.City.Country.Name;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        internal void UpdateLocation(UserLocationService.City SelectedCity)
        {
            CityTB.Text = SelectedCity.Name;
            CountryTB.Text = SelectedCity.Country.Name;
        }
    }
}
