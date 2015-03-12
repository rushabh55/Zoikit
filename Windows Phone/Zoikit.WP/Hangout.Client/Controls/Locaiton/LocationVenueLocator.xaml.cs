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

namespace Hangout.Client.Controls
{
    public partial class LocationVenueLocator : UserControl
    {
        public LocationVenueLocator()
        {
            InitializeComponent();
        }


        public delegate void VenueEventHelper(VenueService.Venue venue);

        public delegate void CityEventHelper(UserLocationService.City city);

        public event CityEventHelper CitySelected;

        public event VenueEventHelper VenueSelected;

        public event EventHandler ShowLoaderEvent;

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (!DesignerProperties.IsInDesignTool)
            {
                if (!Core.Authentication.Accounts.LoggedOffAllAccounts())
                {
                    AttachEvents();
                    if (ShowLoaderEvent != null)
                    {
                        ShowLoaderEvent(null, new EventArgs());
                    }
                    Core.Location.CurrentLocation.GetLocationCompleted += CurrentLocation_GetLocationCompleted;
                    Core.Location.CurrentLocation.GetCurrentLocation();
                }
            }
        }

        private void AttachEvents()
        {
            CityList.CitySelected += CityList_CitySelected;
            MyCityList.CitySelected += CityList_CitySelected;
            RecommendedVenueList.VenueSelected += RecommendedVenueList_VenueSelected;
            SearchVenueList.VenueSelected += RecommendedVenueList_VenueSelected;
            PlacesFollowingList.VenueSelected += RecommendedVenueList_VenueSelected;
            NearbyVenuesList.VenueSelected += NearbyVenuesList_VenueSelected;
        }

        void NearbyVenuesList_VenueSelected(VenueService.Venue venue)
        {
            if (VenueSelected != null)
            {
                VenueSelected(venue);
            }
        }

       

        void RecommendedVenueList_VenueSelected(VenueService.Venue venue)
        {
            if (VenueSelected != null)
            {
                VenueSelected(venue);
            }
        }

        void CityList_CitySelected(UserLocationService.City city)
        {
            if (CitySelected != null)
            {
                CitySelected(city);
            }
        }

        void CurrentLocation_GetLocationCompleted()
        {
           
            Core.Location.CurrentLocation.GetLocationCompleted -= CurrentLocation_GetLocationCompleted;

            //get places nearby
            Services.VenueServiceClient.GetNearbyVenuesCompleted += VenueServiceClient_GetNearbyVenuesCompleted;
            Services.VenueServiceClient.GetNearbyVenuesAsync(Core.User.User.UserID, Core.Location.CurrentLocation.Location.Latitude, Core.Location.CurrentLocation.Location.Longitude, Core.User.User.ZAT);

            //get current city
            Services.UserLocationServiceClient.GetCityByLocationCompleted += UserLocationServiceClient_GetCityByLocationCompleted;
            Services.UserLocationServiceClient.GetCityByLocationAsync(Core.User.User.UserID, Core.Location.CurrentLocation.Location.Latitude, Core.Location.CurrentLocation.Location.Longitude, Core.User.User.ZAT);
            
            //get recommended places
            Services.VenueServiceClient.GetRecommenededVenuesCompleted += VenueServiceClient_GetRecommenededVenuesCompleted;
            Services.VenueServiceClient.GetRecommenededVenuesAsync(Core.User.User.UserID, Core.Location.CurrentLocation.Location.Latitude, Core.Location.CurrentLocation.Location.Longitude, Core.User.User.ZAT);
            
            //get places following
            Services.VenueServiceClient.GetVenueFollowingCompleted += VenueServiceClient_GetVenueFollowingCompleted;
            Services.VenueServiceClient.GetVenueFollowingAsync(Core.User.User.UserID, 10,new System.Collections.ObjectModel.ObservableCollection<int>(),Core.User.User.ZAT);
        }

        void VenueServiceClient_GetNearbyVenuesCompleted(object sender, VenueService.GetNearbyVenuesCompletedEventArgs e)
        {
            Services.VenueServiceClient.GetNearbyVenuesCompleted -= VenueServiceClient_GetNearbyVenuesCompleted;

            if (e.Error == null)
            {
                if (e.Result != null)
                {
                    if (e.Result.Count == 0)
                    {
                        NearbyVenues.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    else
                    {
                        NearbyVenues.Visibility = System.Windows.Visibility.Visible;
                        NearbyVenuesList.Venues = e.Result.ToList();

                        if (ShowPage != null)
                        {
                            ShowPage(null, new EventArgs());
                        }

                    }

                }
                else
                {
                    NearbyVenues.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
            else
            {
                NearbyVenues.Visibility = System.Windows.Visibility.Collapsed;
                if (ShowError != null)
                {
                    ShowError(null, new EventArgs());
                }
            }
        }

        void VenueServiceClient_GetVenueFollowingCompleted(object sender, VenueService.GetVenueFollowingCompletedEventArgs e)
        {
            
            Services.VenueServiceClient.GetVenueFollowingCompleted -= VenueServiceClient_GetVenueFollowingCompleted;
            
            if (e.Error == null)
            {
                if (e.Result != null)
                {
                    if (e.Result.Count == 0)
                    {
                        PlacesFollowingSP.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    else
                    {
                        PlacesFollowingSP.Visibility = System.Windows.Visibility.Visible;
                        PlacesFollowingList.Venues = e.Result.ToList();

                        if (ShowPage != null)
                        {
                            ShowPage(null,new EventArgs());
                        }

                    }

                }
                else
                {
                    PlacesFollowingSP.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
            else
            {
                PlacesFollowingSP.Visibility = System.Windows.Visibility.Collapsed;
                if (ShowError != null)
                {
                    ShowError(null, new EventArgs());
                }
            }
        }

        public event EventHandler ShowError;
        public event EventHandler ShowPage;
        public event EventHandler ShowNoPage;

        void VenueServiceClient_GetRecommenededVenuesCompleted(object sender, VenueService.GetRecommenededVenuesCompletedEventArgs e)
        {
            
            Services.VenueServiceClient.GetRecommenededVenuesCompleted-= VenueServiceClient_GetRecommenededVenuesCompleted;
            if (e.Error == null)
            {
                if (e.Result != null)
                {
                    
                    RecommendedVenues.Visibility = System.Windows.Visibility.Visible;
                    RecommendedVenueList.Venues = e.Result.ToList();
                    if (ShowPage != null)
                    {
                        ShowPage(null, new EventArgs());
                    }

                }
                else
                {
                    RecommendedVenues.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
            else
            {
                RecommendedVenues.Visibility = System.Windows.Visibility.Collapsed;
                if (ShowError != null)
                {
                    ShowError(null, new EventArgs());
                }
            }
        }

        void UserLocationServiceClient_GetCityByLocationCompleted(object sender, UserLocationService.GetCityByLocationCompletedEventArgs e)
        {
            
            Services.UserLocationServiceClient.GetCityByLocationCompleted -= UserLocationServiceClient_GetCityByLocationCompleted;

            if (e.Error == null)
            {
                if (e.Result != null)
                {
                    List<UserLocationService.City> cities=new List<UserLocationService.City>();
                    cities.Add(e.Result);
                    MyCitySP.Visibility = System.Windows.Visibility.Visible;
                    MyCityList.Cities = cities;
                    if (ShowPage != null)
                    {
                        ShowPage(null, new EventArgs());
                    }
                    
                }
                else
                {
                    MyCitySP.Visibility = System.Windows.Visibility.Collapsed;

                }
            }
            else
            {
                MyCitySP.Visibility = System.Windows.Visibility.Collapsed;
                if (ShowError != null)
                {
                    ShowError(null, new EventArgs());
                }
            }
        }

        private void SearchBtn_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (ShowPage != null)
            {
                ShowPage(null, new EventArgs());
            }
            
            if (!String.IsNullOrWhiteSpace(SearchTB.Text))
            {
                SearchPG.Opacity = 1;

                Services.VenueServiceClient.SearchVenuesCompleted += VenueServiceClient_SearchVenuesCompleted;
                Services.VenueServiceClient.SearchVenuesAsync(Core.User.User.UserID, Core.Location.CurrentLocation.Location.Latitude, Core.Location.CurrentLocation.Location.Longitude, SearchTB.Text.Trim(), Core.User.User.ZAT);
                
                Services.UserLocationServiceClient.SearchCitiesCompleted += UserLocationServiceClient_SearchCitiesCompleted;
                Services.UserLocationServiceClient.SearchCitiesAsync(Core.User.User.UserID, Core.User.User.ZAT, SearchTB.Text.Trim());
            }
            else
            {
                MessageBox.Show("Please enter search query");
            }
        }

        void UserLocationServiceClient_SearchCitiesCompleted(object sender, UserLocationService.SearchCitiesCompletedEventArgs e)
        {
           
            Services.UserLocationServiceClient.SearchCitiesCompleted -= UserLocationServiceClient_SearchCitiesCompleted;
            if (e.Error == null)
            {
                if (e.Result != null)
                {
                    if (e.Result.Count == 0)
                    {
                        CitySP.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    else
                    {
                        CitySP.Visibility = System.Windows.Visibility.Visible;
                        CityList.Cities = e.Result.ToList();
                        if (ShowPage != null)
                        {
                            ShowPage(null, new EventArgs());
                        }
                    }
                }
                else
                {
                    CitySP.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
            else
            {
                CitySP.Visibility = System.Windows.Visibility.Collapsed;
                if (ShowError != null)
                {
                    ShowError(null, new EventArgs());
                }
            }

        }

        void VenueServiceClient_SearchVenuesCompleted(object sender, VenueService.SearchVenuesCompletedEventArgs e)
        {
           
            SearchPG.Opacity = 0;
            SearchPlaceSP.Visibility = System.Windows.Visibility.Visible;
            Services.VenueServiceClient.SearchVenuesCompleted -= VenueServiceClient_SearchVenuesCompleted;
            if (e.Error == null)
            {
                if (e.Result != null)
                {
                    SearchVenueList.Venues = e.Result.ToList();
                    if (ShowPage != null)
                    {
                        ShowPage(null, new EventArgs());
                    }
                }
                else
                {
                    SearchVenueList.Venues = new List<VenueService.Venue>();
                }
            }
            else
            {
                SearchVenueList.Venues = new List<VenueService.Venue>();
                if (ShowError != null)
                {
                    ShowError(null, new EventArgs());
                }
            }
           
        }


    }

}
