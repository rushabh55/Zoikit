using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Input;

namespace Hangout.Client.Location
{
    public partial class LocationPicker : PhoneApplicationPage
    {
        public LocationPicker()
        {
            InitializeComponent();
        }

        private void TypeSearchLBL_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            SearchTB.Focus();
        }

        private void TopLoaderCollapsed()
        {
            TopPB.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void TopLoaderVisible()
        {
            TopPB.Visibility = System.Windows.Visibility.Visible;
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            

            if (SearchTB.Text.Count() > 0)
            {
                TypeSearchLBL.Visibility = System.Windows.Visibility.Collapsed;


                if(SearchTB.Text.Count()>=3)
                {
                    SearchCities(SearchTB.Text);
                }

            }
            else
            {
                TypeSearchLBL.Visibility = System.Windows.Visibility.Visible;
            }

            
        }

        private void SearchCities(string p)
        {
            TopLoaderVisible();
            ActivateSearchPivot();
            ShowSearchLoading.Begin();
            Services.UserLocationServiceClient.SearchCitiesCompleted += UserLocationServiceClient_SearchCitiesCompleted;
            Services.UserLocationServiceClient.SearchCitiesAsync(Core.User.User.UserID, Core.User.User.ZAT, p);
        }

        private void ActivateSearchPivot()
        {
            if(MainPivot.SelectedIndex!=1)
            {
                MainPivot.SelectedIndex = 1;
            }
        }

        void UserLocationServiceClient_SearchCitiesCompleted(object sender, UserLocationService.SearchCitiesCompletedEventArgs e)
        {
            Services.UserLocationServiceClient.SearchCitiesCompleted -= UserLocationServiceClient_SearchCitiesCompleted;

            TopLoaderCollapsed();

            if (e.Error == null)
            {
                if (e.Result != null)
                {

                    SearchLocationList.Cities = e.Result;


                    if (SearchLocationList.Cities == null || SearchLocationList.Cities.Count == 0)
                    {
                        ShowSearchNoLocations.Begin();
                    }
                    else
                    {
                        ShowSearchPage.Begin();
                    }

                }
                else
                {
                    if (SearchLocationList.Cities == null || SearchLocationList.Cities.Count == 0)
                    {
                        ShowSearchNoLocations.Begin();
                    }
                }

            }
            else
            {
                //show error LBL
                ShowSearchError.Begin();
            }


           


        }

        private void SearchTB_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (SearchTB.Text.Count() >= 3)
                {
                        //update comment. 

                        TopLoaderVisible();

                        SearchCities(SearchTB.Text);

                }
                else
                {
                    MessageBox.Show("Please enter three or more characters", "Too Short", MessageBoxButton.OK);
                }
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            RecentLocationList.LocationSelected += RecentLocationList_LocationSelected;
            SearchLocationList.LocationSelected += SearchLocationList_LocationSelected;

            //load recent. 
            LoadRecent();

            ShowSearchStart.Begin();

            RecentLocationList.Height = 575;
            SearchLocationList.Height = 575;
            RecentLocationList.LocationLB.Height = 575;
            SearchLocationList.LocationLB.Height = 575;

        }

        private void LoadRecent()
        {
            try
            {
                System.Collections.ObjectModel.ObservableCollection<UserLocationService.City> list = Core.Location.Location.GetLocationList();
                if(list.Count==0)
                {
                    ShowRecentNoLocations.Begin();
                }
                else
                {
                    RecentLocationList.Cities = list;
                    ShowRecentPage.Begin();
                }
            }
            catch
            {
                ShowRecentZoikitError.Begin();
            }
        }

        void SearchLocationList_LocationSelected(UserLocationService.City city)
        {
            SaveCity(city);
        }

        private void SaveCity(UserLocationService.City city)
        {
            //add to phone list. 
            Core.Location.Location.AddLocation(city);
            //save static
            Core.Location.Location.SelectedCity = city;

            NaviagteBack();

        }

        void RecentLocationList_LocationSelected(UserLocationService.City city)
        {
            SaveCity(city);
        }

       

        private void NaviagteBack()
        {
            Dispatcher.BeginInvoke(() =>
            {
                if (NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                }
                else
                {
                    NavigationService.Navigate(new Uri(Navigation.Dashboard, UriKind.RelativeOrAbsolute));
                }
            });
        }


        
    }
}