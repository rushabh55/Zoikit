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

namespace Hangout.Client.Controls.Buzz
{
    public partial class BuzzInputControl : UserControl
    {
        public BuzzInputControl()
        {
            InitializeComponent();
        }


        bool IsPostBack = false;

        public event EventHandler OkBtnTapped;

        private int? categoryId;

        public int? CategoryId
        {
            get
            {
                return categoryId;
            }
            set
            {
                categoryId = value;

                if (categoryId != null)
                {
                    CategorySP.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        bool LoadPlaces = false;

        public event EventHandler SavingBuzz;

        public event BuzzSavedHelper BuzzSaved;

        public delegate void BuzzSavedHelper(BuzzService.HangoutSaveStatus status);


        private void TextBlock_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (AdvanceSP.Visibility == System.Windows.Visibility.Collapsed)
            {
                //ShowAdvancedLBL.Visibility = System.Windows.Visibility.Collapsed;
                AdvanceSP.Visibility = System.Windows.Visibility.Visible;
                if (!CategoryLoaded)
                {
                    LoadCategories();
                }
            }
        }

        private void LoadCategories()
        {
            if (categoryId == null)
            {
                AdvancedSPProgressBar.Visibility = System.Windows.Visibility.Visible;
                Services.CategoryServiceClient.GetAllCategoriesCompleted += CategoryServiceClient_GetAllCategoriesCompleted;
                Services.CategoryServiceClient.GetAllCategoriesAsync(Core.User.User.UserID, Core.User.User.ZAT, CategoryService.ClientType.WindowsPhone7);
            }
        }

        void CategoryServiceClient_GetAllCategoriesCompleted(object sender, CategoryService.GetAllCategoriesCompletedEventArgs e)
        {
           
            Services.CategoryServiceClient.GetAllCategoriesCompleted -= CategoryServiceClient_GetAllCategoriesCompleted;
            AdvancedSPProgressBar.Visibility = System.Windows.Visibility.Collapsed;
            if (e.Error == null)
            {
                CategoryLoaded = true;
                CategoryService.UserCategory none = new CategoryService.UserCategory();
                none.Category = new CategoryService.Category();
                none.Category.CategoryID = 0;
                none.Category.Name = "None";
                e.Result.Add(none);
                CategoryPicker.ItemsSource = e.Result.OrderBy(o => o.Category.CategoryID).ToList();
                CategoryPicker.IsEnabled = true;
            }
        }

        public bool CategoryLoaded { get; set; }

        private void OkBtn_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //validity check :)
            if (String.IsNullOrWhiteSpace(BuzzTB.Text))
            {
                MessageBox.Show("Do let us know the buzz around you right now.","What's the Buzz?",MessageBoxButton.OK);
                return;
            }
            if (TwitterSP.Visibility == System.Windows.Visibility.Visible)
            {
                if ((bool)TwitterBtn.IsChecked)
                {
                    if (BuzzTB.Text.Count() > 140)
                    {
                        MessageBox.Show("Your buzz is too long for twitter. Its "+BuzzTB.Text.Count()+" characters now. Make it 140.", "Buzz too long for Twitter?", MessageBoxButton.OK);
                        return;
                    }
                }
                
            }

            if (Location == null)
            {
                MessageBox.Show("We're currently getting your location. Please wait for a few moments and then save");
                return;
            }

            if (!CategoryLoaded && AdvanceSP.Visibility == System.Windows.Visibility.Visible)
            {
                MessageBox.Show("Please wait, We're loading categories");
                return;

            }

            

            this.Visibility = System.Windows.Visibility.Collapsed;
            


            //now save the details. :)

            int? venueId=null;
            int? cityId=null;

            if(VenueSeleted)
            {
                venueId=SelectedVenue.VenueID;
            }

            if(CitySelected)
            {
                cityId=SelectedCity.Id;
            }

            if (CategoryId == null)
            {

                if (CategoryPicker.SelectedIndex != 0)
                {
                    if (CategoryPicker.SelectedItem != null)
                    {
                        CategoryService.Category cat = ((CategoryService.UserCategory)CategoryPicker.SelectedItem).Category;
                        categoryId = cat.CategoryID;
                    }
                }
            }
            else
            {
                categoryId = CategoryId;
            }


            DateTime? HangoutDateTime=null;

            if (this.DatePicker.Value != null&&this.TimePicker.Value!=null)
            {
                HangoutDateTime=this.DatePicker.Value.Value.Date+this.TimePicker.Value.Value.TimeOfDay;
            }


            Services.BuzzServiceClient.InsertBuzzCompleted += BuzzServiceClient_InsertBuzzCompleted;
            Services.BuzzServiceClient.InsertBuzzAsync(Core.User.User.UserID, BuzzTB.Text.Trim(), new BuzzService.Location1 { Latitude = Location.Latitude, Longitude = Location.Longitude }, HangoutDateTime, venueId, cityId, categoryId, Core.User.User.ZAT);

            if (SavingBuzz != null)
            {
                SavingBuzz(null, new EventArgs());
            }
            
        }

        void BuzzServiceClient_InsertBuzzCompleted(object sender, BuzzService.InsertBuzzCompletedEventArgs e)
        {
            Services.BuzzServiceClient.InsertBuzzCompleted -= BuzzServiceClient_InsertBuzzCompleted;
            if (e.Error == null)
            {
                if (BuzzSaved != null)
                {
                    BuzzTB.IsEnabled = true;
                    BuzzSaved(e.Result);
                }

                if ((bool)FacebookBtn.IsChecked)
                {
                    if (Settings.Settings.FacebookData != null)
                    {
                        Services.FacebookServiceClient.PostFacebookStatusAsync(Settings.Settings.FacebookData.AccessToken, BuzzTB.Text, null, "");
                    }
                }

                if ((bool)TwitterBtn.IsChecked)
                {
                    if (Settings.Settings.TwitterData != null)
                    {
                        Services.TwitterServiceClient.PostTweetAsync(Settings.Settings.TwitterData.AccessToken, Settings.Settings.TwitterData.AccessTokenSecret, BuzzTB.Text);
                    }
                }
            }
        }

        public event EventHandler Canceled;

        private void CancelBtn_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (CitySelected)
            {
                Clear(null,SelectedCity);
            }
            if (VenueSeleted)
            {
                Clear(SelectedVenue,null);
            }

            this.Visibility = System.Windows.Visibility.Collapsed;

            if (Canceled != null)
            {
                Canceled(null, new EventArgs());
            }
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (!DesignerProperties.IsInDesignTool)
            {
                if (!Core.Authentication.Accounts.LoggedInToFacebook() && !Core.Authentication.Accounts.LoggedInToTwitter())
                {
                    PostToSocialNetworksSP.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    if (!Core.Authentication.Accounts.LoggedInToFacebook())
                    {
                        FacebookSP.Visibility = System.Windows.Visibility.Collapsed;
                    }

                    if (!Core.Authentication.Accounts.LoggedInToTwitter())
                    {
                        TwitterSP.Visibility = System.Windows.Visibility.Collapsed;
                    }
                }


                if (!IsPostBack)
                {
                    VenueList.VenueCheckInBtnTapped += VenueList_VenueCheckInBtnTapped;

                    DatePicker.Value = null;
                    BuzzTB.Text = "";
                    TimePicker.Value = null;
                    CategoryPicker.IsEnabled = false;
                    LocationPanelSP.Visibility = System.Windows.Visibility.Collapsed;
                    VenueList.VenueSelected += VenueList_VenueSelected;
                    CityList.CitySelected += CityList_CitySelected;
                    Core.Location.CurrentLocation.GetLocationCompleted += CurrentLocation_GetLocationCompleted;
                    Core.Location.CurrentLocation.GetCurrentLocation();
                }
                else
                {
                    IsPostBack = false;
                }
            }
        }

        void VenueList_VenueCheckInBtnTapped(VenueService.Venue venue)
        {
            HideLocationsPanel();
            VenueSearchTB.TextChanged -= VenueSearchTB_TextChanged_1;
            VenueSearchTB.TextChanged -= VenueSearchTB_TextChanged_1;
            VenueSearchTB.TextChanged -= VenueSearchTB_TextChanged_1;

            VenueSearchTB.Text = venue.Name;
            VenueSearchTB.TextChanged += VenueSearchTB_TextChanged_1;
            SelectedVenue = venue;
            VenueSeleted = true;
            CitySelected = false;
           
        }

        bool VenueSeleted = false;
        bool CitySelected = false;

        void VenueList_VenueSelected(VenueService.Venue venue)
        {
            HideLocationsPanel();
            VenueSearchTB.TextChanged -= VenueSearchTB_TextChanged_1;
            VenueSearchTB.TextChanged -= VenueSearchTB_TextChanged_1;
            VenueSearchTB.TextChanged -= VenueSearchTB_TextChanged_1;

            VenueSearchTB.Text = venue.Name;
            VenueSearchTB.TextChanged += VenueSearchTB_TextChanged_1;
            SelectedVenue = venue;
            VenueSeleted = true;
            CitySelected = false;
            
        }

        private void HideLocationsPanel()
        {
            LocationPanelSP.Visibility = System.Windows.Visibility.Collapsed;
            VenueList.Venues = null;
            CityList.Cities = null;
        }

        void CityList_CitySelected(UserLocationService.City city)
        {
            HideLocationsPanel();
            VenueSearchTB.TextChanged -= VenueSearchTB_TextChanged_1;
            VenueSearchTB.TextChanged -= VenueSearchTB_TextChanged_1;
            VenueSearchTB.TextChanged -=VenueSearchTB_TextChanged_1;
            VenueSearchTB.Text = city.Name;
            VenueSearchTB.TextChanged += VenueSearchTB_TextChanged_1;
            SelectedCity = city;
            CitySelected = true;
            VenueSeleted = false;
           
        }

        void CurrentLocation_GetLocationCompleted()
        {
            Core.Location.CurrentLocation.GetLocationCompleted -= CurrentLocation_GetLocationCompleted;
            Location = Core.Location.CurrentLocation.Location;
        }

        
       

        public UserLocationService.City City { get; set; }

        private void VenueSearchTB_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            if (LoadPlaces)
            {
                if (VenueSearchTB.Text.Trim().Count() >= 3)
                {
                    AdvancedSPProgressBar.Visibility = System.Windows.Visibility.Visible;
                    LocationPanelSP.Visibility = System.Windows.Visibility.Visible;
                    Services.VenueServiceClient.SearchVenuesCompleted += VenueServiceClient_SearchVenuesCompleted;
                    Services.VenueServiceClient.SearchVenuesAsync(Core.User.User.UserID, Location.Latitude, Location.Longitude, VenueSearchTB.Text.Trim(), Core.User.User.ZAT);
                    Services.UserLocationServiceClient.SearchCitiesCompleted += UserLocationServiceClient_SearchCitiesCompleted;
                    Services.UserLocationServiceClient.SearchCitiesAsync(Core.User.User.UserID, Core.User.User.ZAT, VenueSearchTB.Text.Trim());
                }
            }
        }

        void UserLocationServiceClient_SearchCitiesCompleted(object sender, UserLocationService.SearchCitiesCompletedEventArgs e)
        {
            //AdvancedSPProgressBar.Visibility = System.Windows.Visibility.Collapsed;
            Services.UserLocationServiceClient.SearchCitiesCompleted -= UserLocationServiceClient_SearchCitiesCompleted;
            if (e.Error == null)
            {
               
                CitiesSP.Visibility = System.Windows.Visibility.Visible;
                CityList.DisplayNoCitiesFound = true;
                if (e.Result == null)
                {
                    CityList.Cities = null;
                }
                else
                {
                    CityList.Cities = e.Result.ToList();
                }   
                
            }
        }

        void VenueServiceClient_SearchVenuesCompleted(object sender, VenueService.SearchVenuesCompletedEventArgs e)
        {
            AdvancedSPProgressBar.Visibility = System.Windows.Visibility.Collapsed;
            PlacesSP.Visibility = System.Windows.Visibility.Visible;
            Services.VenueServiceClient.SearchVenuesCompleted -= VenueServiceClient_SearchVenuesCompleted;
            if (e.Error == null)
            {
                if (e.Result == null)
                {
                    VenueList.Venues = null;
                }
                else
                {
                    VenueList.Venues = e.Result.ToList();
                }
            }
        }



        public UserLocationService.Location Location { get; set; }

        private VenueService.Venue selectedVenue;

        public VenueService.Venue SelectedVenue
        {
            get
            {
                return selectedVenue;
            }

            set
            {
                selectedVenue = value;
                if (selectedVenue != null)
                {
                    selectedCity = null;

                    VenueSearchTB.TextChanged -= VenueSearchTB_TextChanged_1;
                    VenueSearchTB.TextChanged -= VenueSearchTB_TextChanged_1;
                    VenueSearchTB.TextChanged -= VenueSearchTB_TextChanged_1;
                    VenueSearchTB.TextChanged -= VenueSearchTB_TextChanged_1;

                    VenueSearchTB.Text = selectedVenue.Name;
                    VenueSearchTB.TextChanged += VenueSearchTB_TextChanged_1;
                    VenueSeleted = true;
                    CitySelected = false;
                }

            }
        }

        private UserLocationService.City selectedCity;

        public UserLocationService.City SelectedCity
        {
            get
            {
                return selectedCity;
            }

            set
            {
                selectedVenue = null;
                selectedCity = value;
                if (selectedCity != null)
                {
                    VenueSearchTB.TextChanged -= VenueSearchTB_TextChanged_1;
                    VenueSearchTB.TextChanged -= VenueSearchTB_TextChanged_1;
                    VenueSearchTB.TextChanged -= VenueSearchTB_TextChanged_1;
                    VenueSearchTB.TextChanged -= VenueSearchTB_TextChanged_1;
                    VenueSearchTB.Text = selectedCity.Name;
                    VenueSearchTB.TextChanged += VenueSearchTB_TextChanged_1;
                    VenueSeleted = false;
                    CitySelected = true;
                }

            }
        }

        public void Clear(VenueService.Venue venue,UserLocationService.City city)
        {
            BuzzTB.Text = "";
            if (venue != null)
            {
                SelectedVenue = venue;
            }
            if (city != null)
            {
                SelectedCity = city;
            }
            VenueList.Venues = null;
            CityList.CityLB = null;
            LocationPanelSP.Visibility = Visibility;
            DatePicker.Value = null;
            TimePicker.Value = null;
            VenueSearchTB.TextChanged -= VenueSearchTB_TextChanged_1;
            VenueSearchTB.Text = "";
            VenueSearchTB.TextChanged += VenueSearchTB_TextChanged_1;
            
            if (CategoryLoaded)
            {
                CategoryPicker.SelectedIndex = 0;
            }
            HideLocationsPanel();
           
            

        }

        private void DatePicker_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            IsPostBack = true;
        }

        private void TimePicker_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            IsPostBack = true;
        }

        private void VenueSearchTB_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            LoadPlaces = true;
            VenueSearchTB.TextChanged+=VenueSearchTB_TextChanged_1;
        }

        private void TextBlock_Tap_2(object sender, System.Windows.Input.GestureEventArgs e)
        {
           // ShowAdvancedLBL.Visibility = System.Windows.Visibility.Visible;
            AdvanceSP.Visibility = System.Windows.Visibility.Collapsed;
        }



    }
}
