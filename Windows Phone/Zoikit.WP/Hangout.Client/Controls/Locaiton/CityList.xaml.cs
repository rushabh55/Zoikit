using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Hangout.Client.Controls.Locaiton
{
    public partial class CityList : UserControl
    {


        public delegate void CitySelectedHelper(UserLocationService.City city);

        public event CitySelectedHelper CitySelected;


        public CityList()
        {
            InitializeComponent();
        }

        private List<UserLocationService.City> cities;

        public List<UserLocationService.City> Cities
        {
            get
            {
                try
                {
                    return cities;
                }
                catch (Exception ex)
                {
                    Core.Exceptions.ExceptionReporting.Report(ex);
                    return new List<UserLocationService.City>();
                }
            }
            set
            {
                try
                {
                    cities = value;

                    if (cities == null)
                    {
                        if (CityLB != null)
                        {
                            CityLB.DataContext = null;
                            CityLB.ItemsSource = null;
                            if (DisplayNoCitiesFound)
                            {
                                NoLBL.Visibility = System.Windows.Visibility.Visible;
                            }

                            CityLB.Visibility = System.Windows.Visibility.Collapsed;
                        }

                        return;
                    }


                    if (cities.Count > 0)
                    {
                        CityLB.DataContext = null;
                        CityLB.ItemsSource = null;
                        CityLB.DataContext = cities;
                        CityLB.ItemsSource = cities;
                        CityLB.Visibility = System.Windows.Visibility.Visible;
                        if (DisplayNoCitiesFound)
                        {
                            NoLBL.Visibility = System.Windows.Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        CityLB.Visibility = System.Windows.Visibility.Collapsed;
                        if (DisplayNoCitiesFound)
                        {
                            NoLBL.Visibility = System.Windows.Visibility.Visible;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Core.Exceptions.ExceptionReporting.Report(ex);
                   
                }

            }
        }

        private void CityLB_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (CitySelected != null)
            {
                if (CityLB.SelectedItem != null)
                {
                    CitySelected((UserLocationService.City)CityLB.SelectedValue);
                }
            }
        }

        public bool DisplayNoCitiesFound { get; set; }

        private void AddButton_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (CitySelected != null)
            {
                CitySelected((UserLocationService.City)((AddButton)sender).DataContext);
            }
        }
    }
}

