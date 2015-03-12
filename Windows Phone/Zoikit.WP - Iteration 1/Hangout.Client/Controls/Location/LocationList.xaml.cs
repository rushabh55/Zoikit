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
    public partial class LocationList : UserControl
    {
        public LocationList()
        {
            InitializeComponent();
        }

        private void LocationItemLoaded(object sender, RoutedEventArgs e)
        {
            LocationItem loc = sender as LocationItem;
            loc.LocationSelected += loc_LocationSelected;
        }

        void loc_LocationSelected(UserLocationService.City a)
        {
            if(LocationSelected!=null)
            {
                LocationSelected(a);
            }
        }

        private System.Collections.ObjectModel.ObservableCollection<UserLocationService.City> cities;

        public System.Collections.ObjectModel.ObservableCollection<UserLocationService.City> Cities
        {
            get
            {
                return cities;
            }
            set
            {
                cities = value;
                LocationLB.ItemsSource = null;
                LocationLB.ItemsSource = Cities;
            }
        }


        public delegate void LocationSelectedHelper(UserLocationService.City city);

        public event LocationSelectedHelper LocationSelected; 


       

       

       

        
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LocationLB.IsPullToRefreshEnabled = false;
            LocationLB.DataVirtualizationMode = Telerik.Windows.Controls.DataVirtualizationMode.None;

        }




    }
}
