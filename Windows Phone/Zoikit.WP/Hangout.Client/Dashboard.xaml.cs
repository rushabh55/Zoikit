using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using System.Device.Location;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Notification;
using Hangout.Client.UserLocationService;
using Microsoft.Xna.Framework;
using System.ComponentModel;
using Hangout.Client.Controls.Notifications;

namespace Hangout.Client
{
    public partial class Dashboard : PhoneApplicationPage
    {




        GeoCoordinateWatcher watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);

        public GeoPosition<GeoCoordinate> MyLocation;

        public Dashboard()
        {
            try
            {
                InitializeComponent();
                
            }
            catch (System.Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        #region Notification

        
        void Notifier_NotificationClicked(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Notification, UriKind.RelativeOrAbsolute));
            });

            return;
        }


        #endregion

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!DesignerProperties.IsInDesignTool)
            {

                if (!Core.Network.NetworkStatus.IsNetworkAvailable())
                {

                    ShowNoNetwork.Begin();
                    return;
                }

                if (Settings.Settings.FirstLaunch)
                {

                }

                Load();
            }

        }

        void TutorialControl_TutorialCompleted(object sender, EventArgs e)
        {
            //TutorialControl.TutorialCompleted -= new EventHandler(TutorialControl_TutorialCompleted);
            //TutorialControl.Visibility = System.Windows.Visibility.Collapsed;
            ApplicationBar.IsVisible = true;
            Load();
        }

        private void Load()
        {
            try
            {
                //check of the user is registered
                if (Core.User.User.UserID == -1) //if not registered, then redirect to account page :)
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        NavigationService.Navigate(new Uri(Navigation.Accounts, UriKind.RelativeOrAbsolute));
                    });

                    return;
                }

                if (!Core.User.User.ValidateUserProfile())
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        NavigationService.Navigate(new Uri(Navigation.Profile, UriKind.RelativeOrAbsolute));
                    });

                    return;
                }

                if (!Settings.Settings.InterestFill)
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        NavigationService.Navigate(new Uri(Navigation.UserTag, UriKind.RelativeOrAbsolute));
                    });

                    return;
                }

                if (!Settings.Settings.UpdatedActivityLog)
                {
                    Settings.Settings.UpdatedActivityLog = true;
                    Services.AccountServiceClient.UpdateActivityLogAsync(Core.User.User.UserID, Core.User.User.ZAT);
                }

               
                ShowPage.Begin();

                Dispatcher.BeginInvoke(() =>
                    {
                        BuzzLoader.DisplayText("Looking up for buzz...");
                        ShowBuzzLoader.Begin();
                        Core.Startup.Launch.ConnectToPnService();
                        Core.Startup.Launch.StartLocationTracking();
                        StartGeoWatcher();
                        PeopleLoader.DisplayText("Looking up for people around you...");
                        ShowPeopleLoader.Begin();

                        PlacesLoader.DisplayText("Looking up for places...");
                        LoadCategories();
                        AttachEventHandlers();

                        Services.NotificationServiceClient.GetUnreadNotificationCountCompleted += NotificationServiceClient_GetUnreadNotificationCountCompleted;
                        Services.NotificationServiceClient.GetUnreadNotificationCountAsync(Core.User.User.UserID, Core.User.User.ZAT);


                        Services.TextServiceClient.GetUnreadMessagesCountCompleted += TextServiceClient_GetUnreadMessagesCountCompleted;
                        Services.TextServiceClient.GetUnreadMessagesCountAsync(Core.User.User.UserID, Core.User.User.ZAT);
                        Core.Location.CurrentLocation.UserLocationUpdateCompleted += CurrentLocation_UserLocationUpdateCompleted;
                        Core.Location.CurrentLocation.UpdateLocationToServer();
                    });
                    
               



            }
            catch (System.Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void NotificationServiceClient_GetUnreadNotificationCountCompleted(object sender, NotificationService.GetUnreadNotificationCountCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                Settings.Settings.UnreadNotifications = e.Result;
            }
        }

        void TextServiceClient_GetUnreadMessagesCountCompleted(object sender, TextService.GetUnreadMessagesCountCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                Settings.Settings.UnreadMesssages = e.Result;

            }
        }

        void CurrentLocation_UserLocationUpdateCompleted()
        {
            Dispatcher.BeginInvoke(() =>
                {
                    Core.Location.CurrentLocation.NoLocationFound -= CurrentLocation_NoLocationFound;
                    Core.Location.CurrentLocation.LocationUpdateError -= CurrentLocation_LocationUpdateError;

                    Core.Location.CurrentLocation.UserLocationUpdateCompleted -= CurrentLocation_UserLocationUpdateCompleted;
                    Services.UserLocationServiceClient.GetCityByLocationCompleted += UserLocationServiceClient_GetCityByLocationCompleted;
                    Services.UserLocationServiceClient.GetCityByLocationAsync(Core.User.User.UserID, Core.Location.CurrentLocation.Location.Latitude, Core.Location.CurrentLocation.Location.Longitude, Core.User.User.ZAT);
                    LoadPeopleNearby();
                });
        }



        private void LoadCategories()
        {

            if (Core.User.User.UserID != -1)
            {
                CategoryLoader.DisplayText("Looking up for categories...");
                ShowCategoryLoader.Begin();
                //load all categories 
                Services.CategoryServiceClient.GetAllCategoriesCompleted += CategoryServiceClient_GetAllCategoriesCompleted;
                Services.CategoryServiceClient.GetAllCategoriesAsync(Core.User.User.UserID, Core.User.User.ZAT, CategoryService.ClientType.WindowsPhone7);
            }
        }

        void CategoryServiceClient_GetAllCategoriesCompleted(object sender, CategoryService.GetAllCategoriesCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result != null)
                {
                    if (e.Result.Count > 0)
                    {
                        MainCategoryList.Categories = e.Result.ToList();
                        ShowCategory.Begin();
                    }
                    else
                    {
                        ShowNoCategory.Begin();
                    }
                }
                else
                {
                    ShowNoCategory.Begin();
                }

            }
            else
            {
                ShowCategoryError.Begin();
            }

        }

        private void LoadPeopleNearby()
        {
            PeopleList.ShowProgressBar();
            List<int> skipList = new List<int>();
            skipList.Add(Core.User.User.UserID);

            if (PeopleList.Users != null)
            {
                if (PeopleList.Users.Count > 0)
                {
                    skipList.AddRange(PeopleList.Users.Select(o => o.UserID).ToList());
                }
            }

            Services.UserServiceClient.GetPeopleAroundYouCompleted += UserServiceClient_GetPeopleAroundYouCompleted;
            Services.UserServiceClient.GetPeopleAroundYouAsync(Core.User.User.UserID, 5, new System.Collections.ObjectModel.ObservableCollection<int>(skipList),Core.User.User.ZAT);
        }

        void UserServiceClient_GetPeopleAroundYouCompleted(object sender, UserService.GetPeopleAroundYouCompletedEventArgs e)
        {
            PeopleList.MorePG.Visibility = System.Windows.Visibility.Collapsed;
            Services.UserServiceClient.GetPeopleAroundYouCompleted -= UserServiceClient_GetPeopleAroundYouCompleted;
            if (e.Error == null)
            {
                if (e.Result != null)
                {

                    if (PeopleList.Users == null)
                    {
                        PeopleList.Users = new List<UserService.User>();
                    }

                    PeopleList.Users.AddRange(e.Result.ToList());
                    PeopleList.Users = PeopleList.Users.Distinct().ToList();
                    PeopleList.RefreshList();

                    if (PeopleList.Users.Count > 0)
                    {
                        ShowPeople.Begin();
                    }
                    else
                    {
                        ShowNoPeople.Begin();
                    }

                    if (e.Result.Count < 5)
                    {
                        PeopleList.HideDownPanel();
                    }
                    else
                    {
                        PeopleList.ShowMoreButton();
                    }
                }
                else
                {
                    ShowNoPeople.Begin();
                }
            }
            else
            {
                ShowPeopleError.Begin();
            }

        }

        private void AttachEventHandlers()
        {
            YouControl.NotificationClicked += YouControl_NotificationClicked;
            YouControl.AccountClicked += YouControl_AccountClicked;
            YouControl.ProfileClicked += YouControl_ProfileClicked;
            MainCategoryList.CategorySelected += MainCategoryList_CategorySelected;
            PeopleList.TagSelected += PeopleList_TagSelected;
            PeopleList.UserSelected += PeopleList_UserSelected;
            PeopleList.MoreBtnClicked += PeopleList_MoreBtnClicked;
            BuzzMainControl.UserSelectedEvent += BuzzMainControl_UserSelectedEvent;
            BuzzMainControl.ChangeLocationTapped += BuzzMainControl_ChangeLocationTapped;
            YouControl.InterestClicked += YouControl_InterestClicked;
            VenueLocator.VenueSelected += VenueLocator_VenueSelected;
            VenueLocator.CitySelected += VenueLocator_CitySelected;
            BuzzMainControl.NoBuzz += BuzzMainControl_NoBuzz;
            BuzzMainControl.StartBuzzLoading += BuzzMainControl_StartBuzzLoading;
            BuzzMainControl.SavingBuzz += BuzzMainControl_SavingBuzz;
            BuzzMainControl.BuzzLoadError += BuzzMainControl_BuzzLoadError;
            BuzzMainControl.BuzzLoaded += BuzzMainControl_BuzzLoaded;
            BuzzMainControl.Canceled += BuzzMainControl_Canceled;
            VenueLocator.ShowError += VenueLocator_ShowError;
            VenueLocator.ShowNoPage += VenueLocator_ShowNoPage;
            VenueLocator.ShowPage += VenueLocator_ShowPage;
            VenueLocator.ShowLoaderEvent += VenueLocator_ShowLoaderEvent;
            YouControl.AboutUsClicked += YouControl_AboutUsClicked;
            YouControl.HelpClicked += YouControl_HelpClicked;
            YouControl.UserClicked += YouControl_UserClicked;
            YouControl.LoadingStarted += YouControl_LoadingStarted;
            YouControl.LoadCompleted += YouControl_LoadCompleted;
            YouControl.Error += YouControl_Error;
            Core.Location.CurrentLocation.NoLocationFound += CurrentLocation_NoLocationFound;
            Core.Location.CurrentLocation.LocationUpdateError += CurrentLocation_LocationUpdateError;
            BuzzMainControl.BuzzSelectedEvent += BuzzMainControl_BuzzSelectedEvent;
            YouControl.MessageClicked += YouControl_MessageClicked;
            
        }

        void YouControl_MessageClicked(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Texts, UriKind.RelativeOrAbsolute));
            });

            return;
        }

        void BuzzMainControl_BuzzSelectedEvent(int id)
        {
            NavigateToBuzz(id);
        }

        private void NavigateToBuzz(int id)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Buzz+"?id="+id, UriKind.RelativeOrAbsolute));
            });

            return;
        }

        void CurrentLocation_LocationUpdateError(object sender, EventArgs e)
        {
            Core.Location.CurrentLocation.LocationUpdateError -= CurrentLocation_LocationUpdateError;
            ShowNoNetwork.Begin();
        }

        void CurrentLocation_NoLocationFound(object sender, EventArgs e)
        {
            Core.Location.CurrentLocation.NoLocationFound -= CurrentLocation_NoLocationFound;
            ShowNoLocation.Begin();
        }

        void YouControl_Error(object sender, EventArgs e)
        {
            ShowYouError.Begin();
        }

        void YouControl_LoadCompleted(object sender, EventArgs e)
        {
            ShowYou.Begin();
        }

        void YouControl_LoadingStarted(object sender, EventArgs e)
        {
            YouLoader.DisplayText("Looking up your profile");
            ShowYouLoader.Begin();
        }

        void YouControl_UserClicked(object sender, EventArgs e)
        {
            NavigateToUserProfile(Core.User.User.UserID);
        }

        void YouControl_HelpClicked(object sender, EventArgs e)
        {
            NavigateToHelp();
        }

        private void NavigateToHelp()
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Help, UriKind.RelativeOrAbsolute));
            });

            return;
        }

        void YouControl_AboutUsClicked(object sender, EventArgs e)
        {
            NavigateToAboutUs();
        }

        private void NavigateToAboutUs()
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.About, UriKind.RelativeOrAbsolute));
            });

            return;
        }

        void VenueLocator_ShowLoaderEvent(object sender, EventArgs e)
        {
            ShowPlaceLoader.Begin();
        }

        void VenueLocator_ShowPage(object sender, EventArgs e)
        {
            ShowPlace.Begin();
        }

        void VenueLocator_ShowNoPage(object sender, EventArgs e)
        {
            ShowNoPlace.Begin();
        }

        void VenueLocator_ShowError(object sender, EventArgs e)
        {
            ShowPlaceError.Begin();
        }

        void PeopleList_MoreBtnClicked(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(PeopleSearchText))
            {
                SearchPeople();
            }
            else
            {
                PeopleList.MoreBtn.Visibility = System.Windows.Visibility.Collapsed;
                LoadPeopleNearby();
            }
        }

        void BuzzMainControl_Canceled(object sender, EventArgs e)
        {
            if (BuzzMainControl.BuzzLB.Buzzes != null)
            {
                if (BuzzMainControl.BuzzLB.Buzzes.Count == 0)
                {
                    ShowNoBuzz.Begin();
                }
            }
            else
            {
                ShowNoBuzz.Begin();
            }
        }

        void BuzzMainControl_BuzzLoaded(object sender, EventArgs e)
        {
            ShowBuzz.Begin();
        }

        void BuzzMainControl_BuzzLoadError(object sender, EventArgs e)
        {
            ShowBuzzError.Begin();
        }

        void BuzzMainControl_SavingBuzz(object sender, EventArgs e)
        {
            ShowBuzz.Begin();
        }

        void BuzzMainControl_StartBuzzLoading(object sender, EventArgs e)
        {
            ShowBuzzLoader.Begin();
        }

        void BuzzMainControl_NoBuzz(object sender, EventArgs e)
        {
            ShowNoBuzz.Begin();
        }

        void VenueLocator_CitySelected(City city)
        {
            BuzzMainControl.City = city;
            MainPanorama.DefaultItem = BuzzItem;
        }

        void VenueLocator_VenueSelected(VenueService.Venue venue)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Venue + "?id=" + venue.VenueID, UriKind.RelativeOrAbsolute));
            });

            return;
        }

        void YouControl_NotificationClicked(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Notification, UriKind.RelativeOrAbsolute));
            });

            return;
        }

        void YouControl_InterestClicked(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.UserTag, UriKind.RelativeOrAbsolute));
            });

            return;
        }

        void YouControl_ProfileClicked(object sender, EventArgs e)
        {

            NavigateToProfile();
        }

        void YouControl_AccountClicked(object sender, EventArgs e)
        {
            NavigateToAccount();
        }

       
        void BuzzMainControl_ChangeLocationTapped(object sender, EventArgs e)
        {
            MainPanorama.DefaultItem = Places;
        }

        void BuzzMainControl_UserSelectedEvent(int id)
        {
            NavigateToUserProfile(id);
        }



        void PeopleList_UserSelected(int id)
        {
            NavigateToUserProfile(id);
        }

        private void NavigateToUserProfile(int id)
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.ViewProfile + "?id=" + id, UriKind.RelativeOrAbsolute));
                });
            }
            catch (System.Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void PeopleList_TagSelected(int id)
        {
            MessageBox.Show("Tag : " + id);
        }

       
        private void NavogateToCategory(int id)
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.Category + "?id=" + id, UriKind.RelativeOrAbsolute));
                });
            }
            catch (System.Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void MainCategoryList_CategorySelected(int categoryId)
        {
            NavogateToCategory(categoryId);
        }



        private void StartGeoWatcher()
        {
            try
            {
                Core.Location.CurrentLocation.GetLocationCompleted += CurrentLocation_GetLocationCompleted;
                Core.Location.CurrentLocation.StartWatcher(); //get the location and update that to the DB :)
                Core.Location.CurrentLocation.GetCurrentLocation();
            }
            catch (System.Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void CurrentLocation_GetLocationCompleted()
        {
            
            
        }

        void UserLocationServiceClient_GetCityByLocationCompleted(object sender, GetCityByLocationCompletedEventArgs e)
        {
            Services.UserLocationServiceClient.GetCityByLocationCompleted -= UserLocationServiceClient_GetCityByLocationCompleted;
            if (e.Error == null)
            {
                Settings.Settings.City = e.Result;
                CurrentCity = e.Result;
                BuzzMainControl.City = CurrentCity; //set current city to Buzz MAin control.
            }
        }



        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //profile
                NavigateToProfile();
            }
            catch (System.Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }

        }



        private void NavigateToProfile()
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.Profile, UriKind.RelativeOrAbsolute));
                });
            }
            catch (System.Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }

        }



        private void NavigateToAccount()
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.Accounts, UriKind.RelativeOrAbsolute));
                });
            }
            catch (System.Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }

        }





        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {

            base.OnNavigatingFrom(e);
        }


        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }



        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            new Game().Exit();
        }


        public Location CurrentLocation { get; set; }

        public City CurrentCity { get; set; }

        private void BuzzAddButton_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ShowBuzz.Begin();
            BuzzMainControl.HidesearchIfVisible();
            BuzzSV.ScrollToVerticalOffset(0);
            BuzzMainControl.AddBuzz();
        }

        private void Notifier_Loaded_1(object sender, RoutedEventArgs e)
        {
            NotificationControl Notifier = (NotificationControl)sender;
            Notifier.NotificationClicked += Notifier_NotificationClicked;
            Notifier.DisplayBackground();
        }

        private void BuzzRefreshBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            BuzzMainControl.RefreshBuzz();
            ShowBuzz.Begin();
        }

        private void BuzzSearchBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            BuzzSV.ScrollToVerticalOffset(0);
            BuzzMainControl.SearchSP.Visibility = System.Windows.Visibility.Visible;
        }

        private void PeopleSearchBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            scrollViewer.ScrollToVerticalOffset(0);
            PeopleSearchSP.Visibility = System.Windows.Visibility.Visible;
        }

        private void SearchTB_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            
        }

        private void SearchPeople()
        {
            PeopleList.ShowProgressBar();
            List<int> skipList = new List<int>();
            skipList.Add(Core.User.User.UserID);

            if (PeopleList.Users != null)
            {
                if (PeopleList.Users.Count > 0)
                {
                    skipList.AddRange(PeopleList.Users.Select(o => o.UserID).ToList());
                }
            }


            Services.UserServiceClient.SearchPeopleAroundYouCompleted += UserServiceClient_SearchPeopleAroundYouCompleted;
            Services.UserServiceClient.SearchPeopleAroundYouAsync(Core.User.User.UserID, PeopleSearchText, 5, new System.Collections.ObjectModel.ObservableCollection<int>(skipList), Core.User.User.ZAT);
        }

        void UserServiceClient_SearchPeopleAroundYouCompleted(object sender, UserService.SearchPeopleAroundYouCompletedEventArgs e)
        {
            PeopleList.MorePG.Visibility = System.Windows.Visibility.Collapsed;
            Services.UserServiceClient.GetPeopleAroundYouCompleted -= UserServiceClient_GetPeopleAroundYouCompleted;
            if (e.Error == null)
            {
                if (e.Result != null)
                {

                    if (PeopleList.Users == null)
                    {
                        PeopleList.Users = new List<UserService.User>();
                    }

                    PeopleList.Users.AddRange(e.Result.ToList());
                    PeopleList.Users = PeopleList.Users.Distinct().ToList();
                    PeopleList.RefreshList();

                    if (PeopleList.Users.Count > 0)
                    {
                        ShowPeople.Begin();
                    }
                    else
                    {
                        ShowNoPeople.Begin();
                    }

                    if (e.Result.Count < 5)
                    {
                        PeopleList.HideDownPanel();
                    }
                    else
                    {
                        PeopleList.ShowMoreButton();
                    }
                }
                else
                {
                    ShowNoPeople.Begin();
                }
            }
            else
            {
                ShowPeopleError.Begin();
            }

        }

        private void SearchCancelBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            PeopleSearchSP.Visibility = System.Windows.Visibility.Collapsed;
            SearchTB.Text = "";
            PeopleList.HideDownPanel();
            PeopleList.MorePG.Visibility = System.Windows.Visibility.Visible;
            PeopleList.Users = new List<UserService.User>();
            PeopleList.RefreshList();
            PeopleSearchText = "";
            
            LoadPeopleNearby();
            
        }



        public string PeopleSearchText { get; set; }

        private void SearchBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (SearchTB.Text.Trim().Count() >= 3)
            {
                //start searching...
                PeopleSearchText = SearchTB.Text.Trim();

                PeopleList.Users = new List<UserService.User>();
                PeopleList.RefreshList();

                ShowPeopleLoader.Begin();



                SearchPeople();
            }
            else
            {
                MessageBox.Show("Add more characters to search.", "Search Text too short", MessageBoxButton.OK);
            }
        }
    }
}