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
using System.Collections.ObjectModel;
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework;

namespace Hangout.Client.Accounts
{
    public partial class ViewProfile : PhoneApplicationPage
    {



        bool CategoryLoaded, PlacesLoaded, FollowersLoaded, FollowingLoaded, TagsLoaded, BuzzLoaded;

        private int UserID;

        public ViewProfile()
        {
            InitializeComponent();

            Notifier.NotificationClicked += Notifier_NotificationClicked;
            Notifier.NotificationDisplayed += Notifier_NotificationDisplayed;
            Notifier.NotificationHidden += Notifier_NotificationHidden;
        }

        private int GetID()
        {
            try
            {

                int a = -1;
                int.TryParse(NavigationContext.QueryString["id"], out a);
                return a;
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                return -1;
            }
        }

        private bool IsTrophyView()
        {
            try
            {
                if(NavigationContext.QueryString.ContainsKey("view"))
                {
                    if (NavigationContext.QueryString["view"] == "trophy")
                    {
                        return true;
                    }
                }
                return false;
                
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                return false;
            }
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

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                if (!Core.Network.NetworkStatus.IsNetworkAvailable())
                {
                    MessageBox.Show("We can't detect any data connection on your phone. Please try again when your phone has a stable data connection", "Data Network Not Found", MessageBoxButton.OK);
                    new Game().Exit();
                }
                
                int id = GetID();
                UserID = id;
                if (id == -1)
                {
                    NavigateToDashboard();
                    return;
                }
                LoadUserInfo();
                AttachEvents();

                if (IsTrophyView())
                {
                    MainPivot.SelectedIndex = 1;
                }

                
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }

        }

        private void AttachEvents()
        {
            UserInfo.MessageBtnClicked += UserInfo_MessageBtnClicked;
            UserInfo.TagCountClicked += UserInfo_TagCountClicked;
            UserInfo.VenuesCountClicked += UserInfo_VenuesCountClicked;
            UserInfo.BuzzCountClicked += UserInfo_BuzzCountClicked;
            UserInfo.FollowingCountClicked += UserInfo_FollowingCountClicked;
            UserInfo.FollowersCountClicked += UserInfo_FollowersCountClicked;
            UserInfo.CategorieCountClicked += UserInfo_CategorieCountClicked;
            UserInfo.TrophyCountClicked += UserInfo_TrophyCountClicked;
            UserInfo.BadgeCountClicked += UserInfo_BadgeCountClicked;
            UserBuzzList.MorebuttonTapped += UserBuzzList_MorebuttonTapped;
            FollowersLB.MoreBtnClicked += FollowersLB_MoreBtnClicked;
            FollowingLB.MoreBtnClicked += FollowingLB_MoreBtnClicked;
            TagList.MoreBtnClicked += TagList_MoreBtnClicked;
            PlacesList.MoreBtnClicked += PlacesList_MoreBtnClicked;
            TagList.TokenSelected += TagList_TokenSelected;
            FollowersLB.UserSelected += FollowersLB_UserSelected;
            FollowingLB.UserSelected += FollowingLB_UserSelected;
            PlacesList.VenueSelected += PlacesList_VenueSelected;
            UserCategoryList.CategorySelected += UserCategoryList_CategorySelected;
            UserBuzzList.BuzzSelected += UserBuzzList_BuzzSelected;
            

        }

        void UserBuzzList_BuzzSelected(int id)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Buzz + "?id=" + id, UriKind.RelativeOrAbsolute));
            });
        }

        void UserCategoryList_CategorySelected(int categoryId)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Category + "?id=" + categoryId, UriKind.RelativeOrAbsolute));
            });
        }

        void PlacesList_VenueSelected(VenueService.Venue venue)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Venue + "?id=" + venue.VenueID, UriKind.RelativeOrAbsolute));
            });
        }

        void FollowingLB_UserSelected(int id)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.ViewProfile + "?id=" + id, UriKind.RelativeOrAbsolute));
            });
        }

        void FollowersLB_UserSelected(int id)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.ViewProfile+ "?id=" +id, UriKind.RelativeOrAbsolute));
            });
        }

        void UserInfo_BadgeCountClicked(UserService.CompleteUserProfile User)
        {
            MainPivot.SelectedIndex = 1;
        }

        void UserInfo_TrophyCountClicked(UserService.CompleteUserProfile User)
        {
            MainPivot.SelectedIndex = 1;
        }

        

       

        void UserInfo_CategorieCountClicked(UserService.CompleteUserProfile User)
        {
            MainPivot.SelectedIndex = 6;
        }

        void UserInfo_FollowersCountClicked(UserService.CompleteUserProfile User)
        {
            MainPivot.SelectedIndex = 4;
        }

        void UserInfo_FollowingCountClicked(UserService.CompleteUserProfile User)
        {
            MainPivot.SelectedIndex = 5;
        }

        void UserInfo_BuzzCountClicked(UserService.CompleteUserProfile User)
        {
            MainPivot.SelectedIndex = 2;
        }

        void UserInfo_VenuesCountClicked(UserService.CompleteUserProfile User)
        {
            MainPivot.SelectedIndex = 7;
        }

        void UserInfo_TagCountClicked(UserService.CompleteUserProfile User)
        {
            MainPivot.SelectedIndex = 3;
        }

        void UserInfo_MessageBtnClicked(UserService.CompleteUserProfile User)
        {
            Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.UserText + "?id=" + User.UserID, UriKind.RelativeOrAbsolute));
                });
        }

        void TagList_TokenSelected(int id)
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.Tag + "?id=" + id, UriKind.RelativeOrAbsolute));
                });
            }
            catch (System.Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void PlacesList_MoreBtnClicked(object sender, EventArgs e)
        {
            PlacesList.ShowProgressBar();
            Services.VenueServiceClient.GetVenueFollowingByUserCompleted += VenueServiceClient_GetVenueFollowingByUserCompleted;
            if (PlacesList.Venues == null)
            {
                Services.VenueServiceClient.GetVenueFollowingByUserAsync(Core.User.User.UserID, UserID, 10, new System.Collections.ObjectModel.ObservableCollection<int>(), Core.User.User.ZAT);
            }
            else
            {
                Services.VenueServiceClient.GetVenueFollowingByUserAsync(Core.User.User.UserID, UserID, 10, new System.Collections.ObjectModel.ObservableCollection<int>(PlacesList.Venues.Select(o=>o.VenueID)), Core.User.User.ZAT);
            }
        }

        void TagList_MoreBtnClicked(object sender, EventArgs e)
        {
            TagList.ShowProgressBar();
            Services.TokenServiceClient.GetTokensFollowedByUserCompleted += TokenServiceClient_GetTokensFollowedByUserCompleted;
            if (TagList.Tokens == null)
            {
                Services.TokenServiceClient.GetTokensFollowedByUserAsync(Core.User.User.UserID, UserID, 10, new System.Collections.ObjectModel.ObservableCollection<int>(), Core.User.User.ZAT);
            }
            else
            {
                Services.TokenServiceClient.GetTokensFollowedByUserAsync(Core.User.User.UserID, UserID, 10, new System.Collections.ObjectModel.ObservableCollection<int>(TagList.Tokens.Select(o=>o.Token.Id)), Core.User.User.ZAT);
            }
        }

        void FollowingLB_MoreBtnClicked(object sender, EventArgs e)
        {
            FollowingLB.ShowProgressBar();
            Services.UserServiceClient.GetUsersFollowedByUserCompleted += UserServiceClient_GetUsersFollowedByUserCompleted;
            if (FollowingLB.Users == null)
            {
                Services.UserServiceClient.GetUsersFollowedByUserAsync(Core.User.User.UserID, UserID, 5, new System.Collections.ObjectModel.ObservableCollection<int>(), Core.User.User.ZAT);
            }
            else
            {
                Services.UserServiceClient.GetUsersFollowedByUserAsync(Core.User.User.UserID, UserID, 5, new System.Collections.ObjectModel.ObservableCollection<int>(FollowingLB.Users.Select(o=>o.UserID)), Core.User.User.ZAT);
            }
        }

        void FollowersLB_MoreBtnClicked(object sender, EventArgs e)
        {
            FollowersLB.ShowProgressBar();
            Services.UserServiceClient.GetUsersFollowingByUserCompleted += UserServiceClient_GetUsersFollowingByUserCompleted;
            if (FollowersLB.Users == null)
            {
                Services.UserServiceClient.GetUsersFollowingByUserAsync(Core.User.User.UserID, UserID, 5, new System.Collections.ObjectModel.ObservableCollection<int>(), Core.User.User.ZAT);
            }
            else
            {
                Services.UserServiceClient.GetUsersFollowingByUserAsync(Core.User.User.UserID, UserID, 5, new System.Collections.ObjectModel.ObservableCollection<int>(FollowersLB.Users.Select(o=>o.UserID)), Core.User.User.ZAT);

            }
        }

        void UserBuzzList_MorebuttonTapped(object sender, EventArgs e)
        {
            UserBuzzList.ShowProgressBar();
            Services.BuzzServiceClient.GetBuzzFollowedByUserCompleted += BuzzServiceClient_GetBuzzFollowedByUserCompleted;
            if (UserBuzzList.Buzzes == null)
            {
                Services.BuzzServiceClient.GetBuzzFollowedByUserAsync(Core.User.User.UserID, UserID, 10, new System.Collections.ObjectModel.ObservableCollection<int>(), Core.User.User.ZAT);
            }
            else
            {
                Services.BuzzServiceClient.GetBuzzFollowedByUserAsync(Core.User.User.UserID, UserID, 10, new ObservableCollection<int>(UserBuzzList.Buzzes.Select(o => o.BuzzID)), Core.User.User.ZAT);
            }
        }

       

        private void LoadUserInfo()
        {
            InfoLoader.DisplayText("Looking up for user...");
            ShowInfoLoader.Begin();
            Services.UserServiceClient.GetUserCompleteProfileCompleted += UserServiceClient_GetUserCompleteProfileCompleted;
            Services.UserServiceClient.GetUserCompleteProfileAsync(Core.User.User.UserID, UserID, Core.User.User.ZAT);
            
        }

        void UserServiceClient_GetUserCompleteProfileCompleted(object sender, UserService.GetUserCompleteProfileCompletedEventArgs e)
        {
            Services.UserServiceClient.GetUserCompleteProfileCompleted -= UserServiceClient_GetUserCompleteProfileCompleted;
            if (e.Error == null)
            {
                if (e.Result != null)
                {
                    ShowInfo.Begin();
                    UserInfo.Profile = e.Result;

                    if (e.Result.UserID == Core.User.User.UserID)
                    {
                        AddEdit();
                    }
                }
                else
                {
                    ShowNoInfo.Begin();
                }
            }
            else
            {
                ShowInfoError.Begin();
            }
        }

        private void AddEdit()
        {
            // <shell:ApplicationBarIconButton x:Name="EditBuzzBtn" IconUri="" Text="edit" Click="dashboardBtn_Click"/>

            
            ApplicationBarIconButton btn = new ApplicationBarIconButton { Text = "edit", IconUri = new Uri("/Assets/AppBar/appbar.edit.rest.png", UriKind.RelativeOrAbsolute), IsEnabled = true };
            btn.Click += btn_Click;
            if (!ApplicationBar.Buttons.Contains(btn))
            {
                ApplicationBar.Buttons.Add(btn);
            }
        }

        void btn_Click(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            { 
                NavigationService.Navigate(new Uri(Navigation.Profile, UriKind.RelativeOrAbsolute));
            });
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


        
        private void dashboardBtn_Click(object sender, EventArgs e)
        {
            try
            {
                NavigateToDashboard();
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        #region NotificationControl

        

       


        private void NavigateToHangout(int id)
        {
            try
            {
                //navigate to hangout page
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.Hangout + "?id=" + id, UriKind.RelativeOrAbsolute));
                });
            }
            catch (System.Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);

            }
        }

        

        #endregion

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        private void MainPivot_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (MainPivot.SelectedIndex == 1)
            {
                if (!TrophiesLoaded)
                {
                    TrophyList.TrophySelected += TrophyList_TrophySelected;
                    BadheList.TrophySelected += TrophyList_TrophySelected;
                    ShowTrophyLoader.Begin();
                    TrophyLoader.DisplayText("Looking up for Trophies");
                    TrophiesLoaded = true;
                    LoadTrophies();
                }
            }

            if (MainPivot.SelectedIndex == 2)
            {
                
                //hangouts.
                if (!BuzzLoaded)
                {
                    BuzzLoader.DisplayText("Looking up for buzz...");
                    ShowBuzzLoader.Begin();
                    BuzzLoaded = true;
                    LoadBuzz();
                }

            }

            if (MainPivot.SelectedIndex == 3)
            {
               
                //tags
                if (!TagsLoaded)
                {
                    TagLoader.DisplayText("Looking up for Tags...");
                    ShowTagLoader.Begin();
                    TagsLoaded = true;
                    LoadTags();
                }

            }

            if (MainPivot.SelectedIndex == 4)
            {
                
                //followers
                if (!FollowersLoaded)
                {
                    FolowersLoader.DisplayText("Looking up for followers...");
                    ShowFollowersLoader.Begin();
                    FollowersLoaded = true;
                    LoadFollowers();

                }
            }

            if (MainPivot.SelectedIndex == 5)
            {
                
                //following
                if (!FollowingLoaded)
                {
                    FollowingLoader.DisplayText("Looking up for people following...");
                    ShowFollowingLoader.Begin();
                    FollowingLoaded = true;
                    LoadFollowing();
                }
            }

            if (MainPivot.SelectedIndex == 6)
            {
                
                //categories.
                if (!CategoryLoaded)
                {
                    CategoryLoader.DisplayText("looking up for categories...");
                    ShowCategoryLoader.Begin();
                    CategoryLoaded = false;
                    LoadCategories();
                }
            }

            if (MainPivot.SelectedIndex == 7)
            {
                
                //places. 
                if (!PlacesLoaded)
                {
                    VenueLoader.DisplayText("Looking up for places...");
                    ShowPlaceLoader.Begin();
                    PlacesLoaded = true;
                    LoadPlaces();
                }
            }


        }

        void TrophyList_TrophySelected(TrophyService.Trophy trophy)
        {
            MessageBox.Show(trophy.Description, trophy.Name,MessageBoxButton.OK);
        }

        private void LoadTrophies()
        {
            Services.TrophyServiceClient.GetUserAchievementsCompleted += TrophyServiceClient_GetUserAchievementsCompleted;
            Services.TrophyServiceClient.GetUserAchievementsAsync(GetID(), Core.User.User.ZAT);
        }

        void TrophyServiceClient_GetUserAchievementsCompleted(object sender, TrophyService.GetUserAchievementsCompletedEventArgs e)
        {
            Services.TrophyServiceClient.GetUserAchievementsCompleted -= TrophyServiceClient_GetUserAchievementsCompleted;

            if (e.Error == null)
            {
                if (e.Result != null)
                {
                    if (e.Result.Count > 0)
                    {
                        if (e.Result.Where(o => o.Type == "Trophy").Count() == 0)
                        {
                            TrophySP.Visibility = System.Windows.Visibility.Collapsed;
                        }
                        else
                        {
                            TrophyList.Trophies = e.Result.Where(o => o.Type == "Trophy").ToList();
                        }

                        if (e.Result.Where(o => o.Type == "Badge").Count() == 0)
                        {
                            BadgeSP.Visibility = System.Windows.Visibility.Collapsed;
                        }
                        else
                        {
                           BadheList.Trophies = e.Result.Where(o => o.Type == "Badge").ToList();
                        }

                        ShowTrophyPage.Begin();
                    }
                    else
                    {
                        ShowNoTrophyPage.Begin();
                    }

                }
                else
                {
                    ShowNoTrophyPage.Begin();
                }
            }
            else
            {
                ShowTrophyError.Begin();
            }

        }

        private void LoadCategories()
        {
            Services.CategoryServiceClient.GetCategoriesFollowedByUserCompleted += CategoryServiceClient_GetCategoriesFollowedByUserCompleted;
            Services.CategoryServiceClient.GetCategoriesFollowedByUserAsync(Core.User.User.UserID, UserID, Core.User.User.ZAT);
        }

        void CategoryServiceClient_GetCategoriesFollowedByUserCompleted(object sender, CategoryService.GetCategoriesFollowedByUserCompletedEventArgs e)
        {
            Services.CategoryServiceClient.GetCategoriesFollowedByUserCompleted -= CategoryServiceClient_GetCategoriesFollowedByUserCompleted;

            if (e.Error == null)
            {

                if (e.Result != null)
                {
                    if (e.Result.Count > 0)
                    {
                        UserCategoryList.Categories = e.Result.ToList();
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

        private void LoadFollowing()
        {
            Services.UserServiceClient.GetUsersFollowedByUserCompleted += UserServiceClient_GetUsersFollowedByUserCompleted;
            Services.UserServiceClient.GetUsersFollowedByUserAsync(Core.User.User.UserID, UserID,5,new System.Collections.ObjectModel.ObservableCollection<int>(), Core.User.User.ZAT);
        }

        void UserServiceClient_GetUsersFollowedByUserCompleted(object sender, UserService.GetUsersFollowedByUserCompletedEventArgs e)
        {
            Services.UserServiceClient.GetUsersFollowedByUserCompleted -= UserServiceClient_GetUsersFollowedByUserCompleted;
            if (e.Error == null)
            {
                if (e.Result != null)
                {
                   
                    if (FollowingLB.Users == null)
                    {
                        FollowingLB.Users = new List<UserService.User>();
                    }
                    if (e.Result.Count > 0)
                    {
                        ShowFollowing.Begin();

                        FollowingLB.Users.AddRange(e.Result.ToList());
                        FollowingLB.RefreshList();
                    }

                    if (FollowingLB.Users.Count > 0)
                    {
                        ShowFollowing.Begin();
                    }
                    else
                    {
                        ShowNoFollowing.Begin();
                    }
                   
                    if (e.Result.Count < 5)
                    {
                        FollowingLB.HideDownPanel();
                    }
                    else
                    {
                        FollowingLB.ShowMoreButton();
                    }
                }
                else
                {
                    ShowNoFollowing.Begin();
                }
            }

            else
            {
                ShowFollowingError.Begin();
            }
        }

        private void LoadFollowers()
        {
            Services.UserServiceClient.GetUsersFollowingByUserCompleted += UserServiceClient_GetUsersFollowingByUserCompleted;
            Services.UserServiceClient.GetUsersFollowingByUserAsync(Core.User.User.UserID, UserID,5,new System.Collections.ObjectModel.ObservableCollection<int>(), Core.User.User.ZAT);
        }

        void UserServiceClient_GetUsersFollowingByUserCompleted(object sender, UserService.GetUsersFollowingByUserCompletedEventArgs e)
        {
            Services.UserServiceClient.GetUsersFollowingByUserCompleted -= UserServiceClient_GetUsersFollowingByUserCompleted;
            if (e.Error == null)
            {
                if (e.Result != null)
                {
                    
                    if (FollowersLB.Users == null)
                    {
                        FollowersLB.Users = new List<UserService.User>();
                    }
                    if (e.Result.Count > 0)
                    {
                       
                        FollowersLB.Users = e.Result.ToList();
                        FollowersLB.RefreshList();
                    }

                    if (FollowersLB.Users.Count > 0)
                    {
                        ShowFollowers.Begin();
                    }
                    else
                    {
                        ShowNoFollowers.Begin();
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
                    ShowNoFollowers.Begin();
                }
            }
            else
            {
                ShowFollowersError.Begin();
            }
             
        }

        private void LoadTags()
        {
            Services.TokenServiceClient.GetTokensFollowedByUserCompleted += TokenServiceClient_GetTokensFollowedByUserCompleted;
            Services.TokenServiceClient.GetTokensFollowedByUserAsync(Core.User.User.UserID, UserID,10,new System.Collections.ObjectModel.ObservableCollection<int>(), Core.User.User.ZAT);
            
        }

        void TokenServiceClient_GetTokensFollowedByUserCompleted(object sender, TokenService.GetTokensFollowedByUserCompletedEventArgs e)
        {
            
            Services.TokenServiceClient.GetTokensFollowedByUserCompleted-= TokenServiceClient_GetTokensFollowedByUserCompleted;
            if (e.Error == null)
            {
                if (e.Result != null)
                {
                   
                    if (TagList.Tokens == null)
                    {
                        TagList.Tokens = new List<TokenService.UserToken>();
                    }
                    if (e.Result.Count > 0)
                    {
                       
                        TagList.Tokens.AddRange(e.Result.ToList());
                        TagList.RefreshList();
                    }


                    if (TagList.Tokens.Count > 0)
                    {
                        ShowTag.Begin();
                    }
                    else
                    {
                        ShowNoTag.Begin();
                    }

                   
                    if (e.Result.Count < 10)
                    {
                        TagList.HideDownPanel();
                    }
                    else
                    {
                        TagList.ShowMoreButton();
                    }
                }
                else
                {
                    ShowNoTag.Begin();
                }
            }
            else
            {
                ShowTagError.Begin();
            }
        }

        private void LoadPlaces()
        {
            Services.VenueServiceClient.GetVenueFollowingByUserCompleted += VenueServiceClient_GetVenueFollowingByUserCompleted;
            Services.VenueServiceClient.GetVenueFollowingByUserAsync(Core.User.User.UserID, UserID,10,new System.Collections.ObjectModel.ObservableCollection<int>(), Core.User.User.ZAT);
        }

        void VenueServiceClient_GetVenueFollowingByUserCompleted(object sender, VenueService.GetVenueFollowingByUserCompletedEventArgs e)
        {
            Services.VenueServiceClient.GetVenueFollowingByUserCompleted -= VenueServiceClient_GetVenueFollowingByUserCompleted;
            if (e.Error == null)
            {
                if (e.Result != null)
                {
                    
                    if (PlacesList.Venues == null)
                    {
                        PlacesList.Venues = new List<VenueService.Venue>();
                    }
                    if (e.Result.Count > 0)
                    {
                        
                        PlacesList.Venues.AddRange(e.Result.ToList());
                        PlacesList.RefreshList();
                    }

                    if (PlacesList.Venues.Count > 0)
                    {
                        ShowPlace.Begin();
                    }
                    else
                    {
                        ShowNoPlace.Begin();
                    }
                    
                   
                    if (e.Result.Count < 10)
                    {
                        PlacesList.HideDownPanel();
                    }
                    else
                    {
                        PlacesList.ShowMoreButton();
                    }
                }
                else
                {
                    ShowNoPlace.Begin();
                }
            }
            else
            {
                ShowPlaceError.Begin();
            }
        }

        private void LoadBuzz()
        {
            Services.BuzzServiceClient.GetBuzzFollowedByUserCompleted += BuzzServiceClient_GetBuzzFollowedByUserCompleted;
            Services.BuzzServiceClient.GetBuzzFollowedByUserAsync(Core.User.User.UserID, UserID,10,new System.Collections.ObjectModel.ObservableCollection<int>(), Core.User.User.ZAT);
        }

        void BuzzServiceClient_GetBuzzFollowedByUserCompleted(object sender, BuzzService.GetBuzzFollowedByUserCompletedEventArgs e)
        {
            Services.BuzzServiceClient.GetBuzzFollowedByUserCompleted -= BuzzServiceClient_GetBuzzFollowedByUserCompleted;
            if (e.Error == null)
            {
                if (e.Result != null)
                {
                    
                    if (UserBuzzList.Buzzes == null)
                    {
                        UserBuzzList.Buzzes = new List<BuzzService.Buzz>();
                    }
                    if (e.Result.Count > 0)
                    {
                       
                        UserBuzzList.Buzzes.AddRange(e.Result.ToList());
                        UserBuzzList.RefreshList();
                    }

                    if (UserBuzzList.Buzzes.Count > 0)
                    {
                        ShowBuzz.Begin();
                    }
                    else
                    {
                        ShowNoBuzz.Begin();
                    }

                    if (e.Result.Count < 10)
                    {
                        UserBuzzList.HideDownPanel();
                    }
                    else
                    {
                        UserBuzzList.ShowMoreButton();
                    }
                }
                else
                {
                    ShowNoBuzz.Begin();
                }
            }
            else
            {
                ShowBuzzError.Begin();
            }

        }


        public bool TrophiesLoaded { get; set; }
    }
}