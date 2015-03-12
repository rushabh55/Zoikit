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
using System.Windows.Media.Imaging;
using Hangout.Client.UserLocationService;
using System.ComponentModel;

namespace Hangout.Client.Categories
{
    public partial class Categories : PhoneApplicationPage
    {
        public Categories()
        {
            InitializeComponent();
            Notifier.NotificationClicked += Notifier_NotificationClicked;
            Notifier.NotificationDisplayed += Notifier_NotificationDisplayed;
            Notifier.NotificationHidden += Notifier_NotificationHidden;

        }

        #region Notification

        void Notifier_NotificationHidden(object sender, EventArgs e)
        {
            ShowTopLBL.Begin();
        }

        void Notifier_NotificationDisplayed(object sender, EventArgs e)
        {
            HideTopLBL.Begin();
        }

        void Notifier_NotificationClicked(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Notification, UriKind.RelativeOrAbsolute));
            });

            return;
        }


        #endregion

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (!DesignerProperties.IsInDesignTool)
            {
                try
                {

                    if (!Core.Network.NetworkStatus.IsNetworkAvailable())
                    {
                        
                        ShowCategoryError.Begin();
                        return;
                    }

                    this.DataContext = this;
                    int id = GetID();
                    CategoryID = id;
                    if (id == -1)
                    {
                        NavigateToDashboard();
                        return;
                    }
                    CategoryLoader.DisplayText("Looking up for a category");
                    ShowCategoryLoader.Begin();
                    LoadCategoryInfo();
                    AttachEvents();
                    CategoryBuzzControl.DisableLocationVenueSelector();
                }
                catch (System.Exception ex)
                {
                    Core.Exceptions.ExceptionReporting.Report(ex);
                    MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                }
            }
        }

        private void AttachEvents()
        {
            CategoryBuzzControl.BuzzLoaded += CategoryBuzzControl_BuzzLoaded;
            CategoryBuzzControl.BuzzLoadError += CategoryBuzzControl_BuzzLoadError;
            CategoryBuzzControl.NoBuzz += CategoryBuzzControl_NoBuzz;
            CategoryBuzzControl.StartBuzzLoading += CategoryBuzzControl_StartBuzzLoading;
            FollowersLB.MoreBtnClicked += FollowersLB_MoreBtnClicked;
            LocationSelectorControl.VenueSelected += LocationSelectorControl_VenueSelected;
            LocationSelectorControl.CitySelected += LocationSelectorControl_CitySelected;
            CategoryBuzzControl.ChangeLocationTapped += CategoryBuzzControl_ChangeLocationTapped;
            CategoryBuzzControl.SavingBuzz += CategoryBuzzControl_SavingBuzz;
            FollowersLB.UserSelected += FollowersLB_UserSelected;
            CategoryBuzzControl.BuzzSelectedEvent += CategoryBuzzControl_BuzzSelectedEvent;

        }

        void CategoryBuzzControl_BuzzSelectedEvent(int id)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Buzz + "?id=" + id, UriKind.RelativeOrAbsolute));
            });
        }

        void FollowersLB_UserSelected(int id)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.ViewProfile + "?id=" + id, UriKind.RelativeOrAbsolute));
            });
        }

        void CategoryBuzzControl_SavingBuzz(object sender, EventArgs e)
        {
            ShowBuzzPage.Begin();
        }

        void CategoryBuzzControl_StartBuzzLoading(object sender, EventArgs e)
        {
            BuzzLoader.DisplayText("Looking up for buzz...");
            ShowBuzzLoader.Begin();
        }

        void CategoryBuzzControl_NoBuzz(object sender, EventArgs e)
        {
            ShowNoBuzz.Begin();
        }

        void CategoryBuzzControl_BuzzLoadError(object sender, EventArgs e)
        {
            ShowBuzzError.Begin();
        }

        void CategoryBuzzControl_BuzzLoaded(object sender, EventArgs e)
        {
            ShowBuzzPage.Begin();
        }

        void CategoryBuzzControl_ChangeLocationTapped(object sender, EventArgs e)
        {
            this.ApplicationBar.IsVisible = false;
            LocationSelectorControl.Visibility = System.Windows.Visibility.Visible;
        }

        void LocationSelectorControl_CitySelected(City city)
        {
            CategoryBuzzControl.City = city;
            LocationSelectorControl.Visibility = System.Windows.Visibility.Collapsed;
            this.ApplicationBar.IsVisible = true;
        }

        void LocationSelectorControl_VenueSelected(VenueService.Venue venue)
        {
            CategoryBuzzControl.Venue = venue;
            LocationSelectorControl.Visibility = System.Windows.Visibility.Collapsed;
            this.ApplicationBar.IsVisible = true;
        }

        void FollowersLB_MoreBtnClicked(object sender, EventArgs e)
        {
            FollowersLB.ShowProgressBar();

            List<int> SkipList = new List<int>();
            SkipList.AddRange(FollowersLB.Users.Select(o => o.UserID));
            SkipList.Add(Core.User.User.UserID);
             Services.UserServiceClient.GetLocalUsersByCategoryFollowingCompleted += UserServiceClient_GetLocalUsersByCategoryFollowingCompleted;
             if (FollowersLB.Users == null)
             {
                 FollowersLB.Users = new List<UserService.User>();
             }
             
             Services.UserServiceClient.GetLocalUsersByCategoryFollowingAsync(Core.User.User.UserID, CategoryID, 5, new System.Collections.ObjectModel.ObservableCollection<int>(SkipList), Core.User.User.ZAT);
            
            
        }

        private void LoadCategoryInfo()
        {
            Services.CategoryServiceClient.GetCategoyrByIDCompleted += CategoryServiceClient_GetCategoyrByIDCompleted;
            Services.CategoryServiceClient.GetCategoyrByIDAsync(Core.User.User.UserID, CategoryID, Core.User.User.ZAT);
        }

        TextBlock CategoryNameLBL;
        Image PicImage;

        void CategoryServiceClient_GetCategoyrByIDCompleted(object sender, CategoryService.GetCategoyrByIDCompletedEventArgs e)
        {
            Services.CategoryServiceClient.GetCategoyrByIDCompleted -= CategoryServiceClient_GetCategoyrByIDCompleted;

            if (e.Error == null)
            {
                if (e.Result != null)
                {

                    
                    CategoryNameLBL.Text = e.Result.Category.Name.ToLower();

                    PicImage.Source = new BitmapImage(new Uri(e.Result.Category.PicURL,UriKind.RelativeOrAbsolute));
                    
                    ShowCategoryPage.Begin();
                    if ((bool)e.Result.Following)
                    {
                        ShowUnfollow.Begin();
                    }
                    else
                    {
                        ShowFollow.Begin();
                    }
                    LoadBuzz();
                    LoadPeople();
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

        public void FollowCategory(int id)
        {
            ShowUnfollow.Begin();
            Services.CategoryServiceClient.FollowCategoryCompleted += CategoryServiceClient_FollowCategoryCompleted;
            Services.CategoryServiceClient.FollowCategoryAsync(Core.User.User.UserID, id, Core.User.User.ZAT);
        }

        void CategoryServiceClient_FollowCategoryCompleted(object sender, CategoryService.FollowCategoryCompletedEventArgs e)
        {
            Services.CategoryServiceClient.FollowCategoryCompleted -= CategoryServiceClient_FollowCategoryCompleted;
            if (e.Error == null)
            {
                if (e.Result == CategoryService.FollowResult.Following)
                {


                }
            }
        }

        public void UnfollowCategory(int id)
        {
            ShowFollow.Begin();
            Services.CategoryServiceClient.UnfollowCategoryCompleted += CategoryServiceClient_UnfollowCategoryCompleted;
            Services.CategoryServiceClient.UnfollowCategoryAsync(Core.User.User.UserID, id, Core.User.User.ZAT);
        }

        void CategoryServiceClient_UnfollowCategoryCompleted(object sender, CategoryService.UnfollowCategoryCompletedEventArgs e)
        {
            Services.CategoryServiceClient.UnfollowCategoryCompleted -= CategoryServiceClient_UnfollowCategoryCompleted;
            if (e.Error == null)
            {
                if (e.Result == CategoryService.FollowResult.Unfollowed)
                {

                }
            }
        }

        private void LoadPeople()
        {
            PeopleLoader.DisplayText("Looking up for people...");
            ShowPeopleLoader.Begin();
            List<int> SkipList = new List<int>();
            SkipList.Add(Core.User.User.UserID);
            Services.UserServiceClient.GetLocalUsersByCategoryFollowingCompleted += UserServiceClient_GetLocalUsersByCategoryFollowingCompleted;
            Services.UserServiceClient.GetLocalUsersByCategoryFollowingAsync(Core.User.User.UserID, CategoryID, 5, new System.Collections.ObjectModel.ObservableCollection<int>(SkipList), Core.User.User.ZAT);
        }

        void UserServiceClient_GetLocalUsersByCategoryFollowingCompleted(object sender, UserService.GetLocalUsersByCategoryFollowingCompletedEventArgs e)
        {
            Services.UserServiceClient.GetLocalUsersByCategoryFollowingCompleted -= UserServiceClient_GetLocalUsersByCategoryFollowingCompleted;
            if (e.Error == null)
            {
                if (e.Result != null)
                {

                    if (FollowersLB.Users == null)
                    {
                        FollowersLB.Users = new List<UserService.User>();
                    }
                    
                    FollowersLB.Users.AddRange(e.Result.ToList());
                    FollowersLB.RefreshList();
                    
                    if (FollowersLB.Users.Count > 0)
                    {
                        ShowPeoplePage.Begin();
                    }
                    else
                    {
                        ShowNoPeople.Begin();
                    }

                    if (e.Result.Count < 5)
                    {
                        FollowersLB.HideDownPanel();
                    }
                    else
                    {
                        FollowersLB.ShowMoreButton();
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

        private void LoadBuzz()
        {
            CategoryBuzzControl.CategoryId = CategoryID;
            CategoryBuzzControl.City = Settings.Settings.City;
        }

        
        private void BuzzAddButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ShowBuzzPage.Begin();
            BuzzSV.ScrollToVerticalOffset(0);
            CategoryBuzzControl.AddBuzz();
        }

        

        private void FollowBtn_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            FollowCategory(CategoryID);
        }

        private void UnfollowBtn_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            UnfollowCategory(CategoryID);
        }

        private int GetID()
        {
            try
            {
                int a = -1;
                int.TryParse(NavigationContext.QueryString["id"], out a);
                return a;
            }
            catch (System.Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                return -1;
            }
        }

        private void dashboardBtn_Click(object sender, EventArgs e)
        {
            NavigateToDashboard();
        }

        private void NavigateToDashboard()
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    if (NavigationService.CanGoBack)
                    {
                        var uri = NavigationService.BackStack.First().Source;

                        if (uri.ToString() == "/MainPage.xaml")
                        {
                            NavigationService.GoBack();
                        }
                        else
                        {
                            NavigationService.Navigate(new Uri(Navigation.Dashboard, UriKind.RelativeOrAbsolute));
                        }
                    }
                    else
                    {
                        NavigationService.Navigate(new Uri(Navigation.Dashboard, UriKind.RelativeOrAbsolute));
                    }

                });
            }
            catch (System.Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }



        public int CategoryID { get; set; }


        


        public string PicImg { get; set; }

        public string CategoryName { get; set; }

        private void CategoryName_Loaded_1(object sender, RoutedEventArgs e)
        {
            CategoryNameLBL = sender as TextBlock;
        }

        private void CategoryImage_Loaded_1(object sender, RoutedEventArgs e)
        {
            PicImage = sender as Image;
        }
    }
}