using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Hangout.Client.Controls.Location
{
    public partial class LocationItem : UserControl
    {
        public LocationItem()
        {
            InitializeComponent();
        }

        public delegate void LocationSelectedHelper(UserLocationService.City a);

        public event LocationSelectedHelper LocationSelected;

        private void LayoutRoot_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if(LocationSelected!=null)
            {
                LocationSelected(City);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }


       

        public UserLocationService.City City
        {
            get { return (UserLocationService.City)GetValue(LocationProperty); }

            set
            {
                SetValue(LocationProperty, value);
            }
        }

       

        public static readonly DependencyProperty LocationProperty =
        DependencyProperty.Register(
            "City",
            typeof(UserLocationService.City),
            typeof(Client.Controls.Location.LocationItem),
            new PropertyMetadata(null, LocationValueChanged));


        private static void LocationValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Hangout.Client.Controls.Location.LocationItem obj = (Hangout.Client.Controls.Location.LocationItem)d;
            obj.DisplayData((UserLocationService.City)e.NewValue);
        }

        public void DisplayData(UserLocationService.City city)
        {
            if (city != null)
            {
                CityTB.Text = city.Name;
                if (city.Country != null)
                {
                    CountryTB.Text = city.Country.Name;
                }
            }
        }





      
    }
}
