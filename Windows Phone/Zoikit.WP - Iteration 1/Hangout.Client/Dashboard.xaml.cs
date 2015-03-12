using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using System.Collections;
using Windows.UI.Input;
using System.Windows.Controls.Primitives;
using Telerik.Windows.Controls;
using System.Threading;
using Microsoft.Phone.Tasks;
using System.Net.NetworkInformation;

namespace Hangout.Client
{
    public partial class Dashboard : PhoneApplicationPage
    {
        public Dashboard()
        {
            InitializeComponent();

        }





        Core.Buzz.NewBuzz newbuzz = new Core.Buzz.NewBuzz();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                if (!Core.User.User.ValidateUserProfile())
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        NavigationService.Navigate(new Uri(Navigation.Profile, UriKind.RelativeOrAbsolute));
                    });

                    return;
                }


                if (Core.Location.Location.SelectedCity != null)
                {
                    this.ApplicationBar.IsVisible = true;

                    if (SelectedCity != null)
                    {
                        if (SelectedCity.Id == Core.Location.Location.SelectedCity.Id)
                        {
                            return; //donothing and save me some time. :)
                        }
                    }
                    Core.Location.CurrentLocation.LocationUpdateError += CurrentLocation_LocationUpdateError;
                    Core.Location.CurrentLocation.UserLocationUpdateCompleted += CurrentLocation_UserLocationUpdateCompleted;
                    ShowBuzzLoading.Begin();
                    BuzzLB.Buzzs = new System.Collections.ObjectModel.ObservableCollection<BuzzService.Buzz>();



                    SelectedCity = Core.Location.Location.SelectedCity;
                    this.DiscoverLocationPanel.UpdateLocation(Core.Location.Location.SelectedCity);
                    ShowDiscoverLocationPanel.Begin();
                    this.BuzzHeaderPanel.UpdateLocation(Core.Location.Location.SelectedCity);
                    newbuzz.CityID = Core.Location.Location.SelectedCity.Id;
                    ShowBuzzHeader.Begin();
                    LoadBuzz();

                }
                else
                {
                    Core.Location.CurrentLocation.LocationUpdateError += CurrentLocation_LocationUpdateError;
                    Core.Location.CurrentLocation.UserLocationUpdateCompleted += CurrentLocation_UserLocationUpdateCompleted;
                    ShowBuzzLoading.Begin();

                    Core.Location.CurrentLocation.UpdateLocationToServer();
                }

                CheckMessagesUnreadCount();
                CheckNotificationsUnreadCount();
            }
            catch { }


        }

        private void CheckNotificationsUnreadCount()
        {
            try
            {
                Services.NotificationServiceClient.GetUnreadNotificationCountCompleted += NotificationServiceClient_GetUnreadNotificationCountCompleted;
                Services.NotificationServiceClient.GetUnreadNotificationCountAsync(Core.User.User.UserID, Core.User.User.ZAT);
            }
            catch { }
        }

        void NotificationServiceClient_GetUnreadNotificationCountCompleted(object sender, NotificationService.GetUnreadNotificationCountCompletedEventArgs e)
        {
            try
            {
                Services.NotificationServiceClient.GetUnreadNotificationCountCompleted -= NotificationServiceClient_GetUnreadNotificationCountCompleted;
                if (e.Error == null)
                {
                    if (e.Result > 0)
                    {
                        //boom - show icon
                        NewNotificationIcon.Visibility = System.Windows.Visibility.Visible;
                    }
                }
            }
            catch { }
        }

        private void CheckMessagesUnreadCount()
        {
            try
            {
                Services.TextServiceClient.GetUnreadMessagesCountCompleted += TextServiceClient_GetUnreadMessagesCountCompleted;
                Services.TextServiceClient.GetUnreadMessagesCountAsync(Core.User.User.UserID, Core.User.User.ZAT);
            }
            catch { }
        }

        void TextServiceClient_GetUnreadMessagesCountCompleted(object sender, TextService.GetUnreadMessagesCountCompletedEventArgs e)
        {
            try
            {
                Services.TextServiceClient.GetUnreadMessagesCountCompleted -= TextServiceClient_GetUnreadMessagesCountCompleted;

                if (e.Error == null)
                {
                    if (e.Result > 0)
                    {
                        //boom - show icon
                        NewMessageIcon.Visibility = System.Windows.Visibility.Visible;
                    }
                }
            }
            catch { }
        }


        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {




                if (!Core.User.User.ValidateUserProfile())
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        NavigationService.Navigate(new Uri(Navigation.Profile, UriKind.RelativeOrAbsolute));
                    });

                    return;
                }





                AttachEvents();
                InitPage();

                CheckEmailAndRate();
            }
            catch { }



        }

        private void CheckEmailAndRate()
        {
            try
            {
                if (Settings.Settings.LaunchCount > 4)
                {
                    //send the rate thing. 
                    if (!Settings.Settings.Rated)
                    {
                        Settings.Settings.Rated = true;
                        if (MessageBoxResult.OK == MessageBox.Show("If you loved using Zoikit, please do let us know. And yes! Let other people know too! Share the love.", "Rate Zoikit", MessageBoxButton.OKCancel))
                        {
                            //show marketplace
                            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();

                            marketplaceReviewTask.Show();

                        }


                    }

                    if (!Settings.Settings.EmailVerified)
                    {
                        //call the service. 
                        Services.AccountServiceClient.IsConfirmEmailCompleted += AccountServiceClient_IsConfirmEmailCompleted;
                        Services.AccountServiceClient.IsConfirmEmailAsync(Core.User.User.UserID);
                    }
                }
            }
            catch { }
        }

        void AccountServiceClient_IsConfirmEmailCompleted(object sender, AccountService.IsConfirmEmailCompletedEventArgs e)
        {
            try
            {
                Services.AccountServiceClient.IsConfirmEmailCompleted -= AccountServiceClient_IsConfirmEmailCompleted;

                if (e.Error == null)
                {
                    if (e.Result)
                    {
                        Settings.Settings.EmailVerified = true;
                    }
                    else
                    {
                        Services.AccountServiceClient.ResendConfirmationCompleted += AccountServiceClient_ResendConfirmationCompleted;
                        Services.AccountServiceClient.ResendConfirmationAsync(Core.User.User.UserID);

                    }
                }
            }
            catch { }
        }

        void AccountServiceClient_ResendConfirmationCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            //MessageBox.Show("In order to continue using Zoikit and discover awesomeness around you, we request you to activate your email. We've resent the activation mail to your registered email. If you did not receive one. Write to us at 911@zoikit.com. Please be sure to check spam. :) ", "Activate your email ", MessageBoxButton.OK);
            //Application.Current.Terminate();
        }

        private static void AppRateReminder()
        {


        }

        void CurrentLocation_LocationUpdateError(object sender, EventArgs e)
        {
            try
            {
                ShowBuzzError.Begin();
            }
            catch { }
        }




        private void InitPage()
        {
            try
            {
                NotificationList.NotificationsLB.IsPullToRefreshEnabled = true;
                UserProfileItem.MyBuzzLB.Height = UserProfileItem.MyBuzzLB.BuzzLB.Height;
                BuzzLB.BuzzLB.Height = 581;
                BuzzLB.BuzzLB.IsPullToRefreshEnabled = true;
                BuzzLB.BuzzLB.DataVirtualizationMode = Telerik.Windows.Controls.DataVirtualizationMode.OnDemandAutomatic;
                MainTextLB.TextLB.Height = 456;
                DiscoverList.DiscoverLB.DataVirtualizationMode = DataVirtualizationMode.OnDemandAutomatic;
                newbuzz.Init();
                this.DiscoverList.DiscoverLB.DataVirtualizationMode = DataVirtualizationMode.OnDemandAutomatic;
                this.MainTextLB.Height = 650;
                this.MainTextLB.TextLB.Height = 650;
                this.NotificationList.NotificationsLB.Height = 650;
                this.NotificationList.NotificationsLB.DataVirtualizationMode = DataVirtualizationMode.None;
                this.NotificationList.NotificationsLB.ScrollStateChanged += NotificationsLB_ScrollStateChanged;
                SetProfilePage();
            }
            catch { }
        }

        void NotificationsLB_ScrollStateChanged(object sender, ScrollStateChangedEventArgs e)
        {
            try
            {
                if (e.NewState == ScrollState.BottomStretch)
                {
                    LoadNotifications();
                }
            }
            catch { }
        }

        private void SetProfilePage()
        {
            try
            {
                UserProfileItem.Height = double.NaN;
                UserProfileItem.BuzzLoadEnd += UserProfileItem_BuzzLoadEnd;
                ProfileSV.MouseMove += ProfileSV_MouseMove;
                UserProfileItem.BuzzLoading += UserProfileItem_BuzzLoading;
                ProfileSV.Height = 655;
                UserProfileItem.LayoutRoot.Height = double.NaN;
                UserProfileItem.MAinSP.Height = double.NaN;
                UserProfileItem.MyBuzzLB.Height = double.NaN;
                UserProfileItem.MyBuzzLB.LayoutRoot.Height = double.NaN;
                UserProfileItem.MyBuzzLB.BuzzLB.Height = double.NaN;
            }
            catch { }


        }

        void UserProfileItem_BuzzLoadEnd()
        {
            try
            {
                ProfileBuzzLoading = false;
            }
            catch { }
        }

        void ProfileSV_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {

                if (ProfileSV.VerticalOffset >= ProfileSV.ScrollableHeight)
                {
                    //end
                    if (!ProfileBuzzLoading)
                    {
                        ProfileBuzzLoading = true;
                        UserProfileItem.LoadBuzz();
                    }
                }
            }
            catch { }


        }

        private void AttachEvents()
        {
            try
            {

                Core.PushNotification.PushNotification.PushNotificationReceived += PushNotification_PushNotificationReceived;


                BuzzLB.CommentOnBuzzSelected += BuzzLB_CommentOnBuzzSelected;
                BuzzLB.UserSelected += BuzzLB_UserSelected;
                BuzzLB.ProgressBarDisappear += BuzzLB_ProgressBarDisappear;
                BuzzLB.ProgressBarShow += BuzzLB_ProgressBarShow;
                BuzzLB.NavigateToFacebook += BuzzLB_NavigateToFacebook;
                BuzzLB.NavigateToTwitter += BuzzLB_NavigateToTwitter;
                BuzzLB.TagSelected += BuzzLB_TagSelected;
                BuzzLB.PeopleSelected += BuzzLB_PeopleSelected;
                BuzzLB.PullToRefreshEvent += BuzzLB_PullToRefreshEvent;
                BuzzLB.MoreDataRequested += BuzzLB_MoreDataRequested;
                BuzzLB.ScrollingDown += BuzzLB_ScrollingDown;
                BuzzLB.ScrollingTop += BuzzLB_ScrollingTop;
                NotificationList.MoreDataRequested += NotificationList_MoreDataRequested;
                NotificationList.BuzzSelected += NotificationList_BuzzSelected;
                NotificationList.GoToTextsPage += NotificationList_GoToTextsPage;
                NotificationList.GoToUserTextsPage += NotificationList_GoToUserTextsPage;
                NotificationList.TagSelected += NotificationList_TagSelected;
                newbuzz.NewBuzzResult += newbuzz_NewBuzzResult;
                NotificationList.UserSelected += NotificationList_UserSelected;
                NotificationList.RefreshRequested += NotificationList_RefreshRequested;
                BuzzHeaderPanel.NavigateToLocationSelectorPage += BuzzHeaderPanel_NavigateToLocationSelectorPage;
                BuzzHeaderPanel.NavigateToComposeBuzz += BuzzHeaderPanel_NavigateToComposeBuzz;
                DiscoverLocationPanel.NavigateToLocationSelectorPage += DiscoverLocationPanel_NavigateToLocationSelectorPage;

                UserProfileItem.ProfileImageSelected += UserProfileItem_ProfileImageSelected;
                UserProfileItem.Loaded += UserProfileItem_Loaded;
                UserProfileItem.Loading += UserProfileItem_Loading;
                UserProfileItem.Error += UserProfileItem_Error;
                UserProfileItem.MessageSelected += UserProfileItem_MessageSelected;
                UserProfileItem.MyBuzzLB.CommentOnBuzzSelected += BuzzLB_CommentOnBuzzSelected;
                UserProfileItem.MyBuzzLB.UserSelected += BuzzLB_UserSelected;
                UserProfileItem.MyBuzzLB.NavigateToFacebook += BuzzLB_NavigateToFacebook;
                UserProfileItem.MyBuzzLB.NavigateToTwitter += BuzzLB_NavigateToTwitter;
                UserProfileItem.MyBuzzLB.TagSelected += BuzzLB_TagSelected;
                UserProfileItem.MyBuzzLB.PeopleSelected += MyBuzzLB_PeopleSelected;
                UserProfileItem.PeopleSelected += UserProfileItem_PeopleSelected;
                UserProfileItem.BuzzLoading += UserProfileItem_BuzzLoading;
                UserProfileItem.BuzzLoaded += UserProfileItem_BuzzLoaded;

                MainTextLB.DataRequested += MainTextLB_DataRequested;
                MainTextLB.TextSelected += MainTextLB_TextSelected;
                DiscoverList.UserSelected += DiscoverList_UserSelected;

                DiscoverList.TagSelected += DiscoverList_TagSelected;
                DiscoverList.MoreItemsRequested += DiscoverList_MoreItemsRequested;
            }
            catch { }



        }


        void PushNotification_PushNotificationReceived(Microsoft.Phone.Notification.HttpNotificationEventArgs e)
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NewNotificationIcon.Visibility = System.Windows.Visibility.Visible;
                    NewNotificationIcon.Opacity = 1;
                });
            }
            catch { }
        }

        void UserProfileItem_PeopleSelected(string name, Guid id)
        {
            try
            {
                NavigateToPeopleList(name, id);
            }
            catch { }
        }

        void MyBuzzLB_PeopleSelected(string name, Guid id)
        {
            try
            {
                NavigateToPeopleList(name, id);
            }
            catch { }
        }

        void UserProfileItem_ProfileImageSelected(Guid id)
        {
            try
            {
                if (id == Core.User.User.UserID)
                {
                    NavigateToEditProfile();

                }
            }
            catch { }
        }

        private void NavigateToEditProfile()
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.Profile, UriKind.RelativeOrAbsolute));
                });
            }
            catch { }
        }


        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                Application.Current.Terminate();
                base.OnBackKeyPress(e);
            }
            catch { }
        }

        void newbuzz_NewBuzzResult(bool newbuzz)
        {
            try
            {
                if (newbuzz)
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        NewBuzzIcon.Visibility = System.Windows.Visibility.Visible;
                    });
                }
                else
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        NewBuzzIcon.Visibility = System.Windows.Visibility.Collapsed;
                    });
                }
            }
            catch { }
        }

        void NotificationList_UserSelected(Guid id)
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.UserProfile + "?id=" + id, UriKind.RelativeOrAbsolute));
                });
            }
            catch { }
        }

        void NotificationList_TagSelected(string name)
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.Tag + "?name=" + name, UriKind.RelativeOrAbsolute));
                });
            }
            catch { }
        }

        void NotificationList_RefreshRequested(object sender, EventArgs e)
        {
            try
            {

                NewNotificationIcon.Visibility = System.Windows.Visibility.Collapsed;

                System.Collections.ObjectModel.ObservableCollection<Guid> skipList = new System.Collections.ObjectModel.ObservableCollection<Guid>();

                if (NotificationList.Notifications != null)
                {
                    if (NotificationList.Notifications.Count > 0)
                    {
                        skipList = new System.Collections.ObjectModel.ObservableCollection<Guid>(NotificationList.Notifications.Select(o => o.NotificationID));
                    }
                }

                if (skipList.Count == 0)
                {
                    ShowNotificationLoading.Begin();
                }


                Services.NotificationServiceClient.GetNewNotificationsCompleted += NotificationServiceClient_GetNewNotificationsCompleted;
                Services.NotificationServiceClient.GetNewNotificationsAsync(Core.User.User.UserID, skipList, Core.User.User.ZAT);
            }
            catch { }
        }



        void NotificationServiceClient_GetNewNotificationsCompleted(object sender, NotificationService.GetNewNotificationsCompletedEventArgs e)
        {
            try
            {
                NotificationList.NotificationsLB.StopPullToRefreshLoading(true);
                Services.NotificationServiceClient.GetNewNotificationsCompleted -= NotificationServiceClient_GetNewNotificationsCompleted;

                if (e.Error == null)
                {
                    if (e.Result != null)
                    {
                        if (e.Error == null)
                        {
                            if (e.Result != null)
                            {
                                if (NotificationList.Notifications != null)
                                {

                                    for (int i = e.Result.Count - 1; i >= 0; i--)
                                    {
                                        //insert on top. 
                                        NotificationList.Notifications.Insert(0, e.Result[i]);
                                    }
                                }
                                else
                                {
                                    NotificationList.Notifications = e.Result;
                                }

                                if (NotificationList.Notifications == null || NotificationList.Notifications.Count == 0)
                                {
                                    ShowNoNotification.Begin();
                                }
                                else
                                {
                                    ShowNotificationPage.Begin();
                                }

                            }
                            else
                            {
                                if (NotificationList.Notifications == null || NotificationList.Notifications.Count == 0)
                                {
                                    ShowNoNotification.Begin();
                                }
                            }

                        }
                        else
                        {
                            //show error LBL
                            ShowNotificationError.Begin();
                        }
                    }
                }
            }
            catch { }
        }

        void NotificationList_GoToUserTextsPage(Guid id)
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.UserText + "?id=" + id, UriKind.RelativeOrAbsolute));
                });
            }
            catch { }
        }

        void NotificationList_GoToTextsPage(object sender, EventArgs e)
        {
            try
            {
                MainPivot.SelectedIndex = 3;
            }
            catch { }
        }

        void NotificationList_BuzzSelected(Guid id)
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.Buzz + "?id=" + id, UriKind.RelativeOrAbsolute));
                });
            }
            catch { }
        }

        void NotificationList_MoreDataRequested(object sender, EventArgs e)
        {
            try
            {
                LoadNotifications();
            }
            catch { }
        }

        private void LoadNotifications()
        {
            try
            {
                NotificationList.MoreDataRequested -= NotificationList_MoreDataRequested;
                System.Collections.ObjectModel.ObservableCollection<Guid> skipList = new System.Collections.ObjectModel.ObservableCollection<Guid>();

                if (NotificationList.Notifications != null)
                {
                    if (NotificationList.Notifications.Count > 0)
                    {
                        AlertMoreLoadingPB.Visibility = System.Windows.Visibility.Visible;
                        skipList = new System.Collections.ObjectModel.ObservableCollection<Guid>(NotificationList.Notifications.Select(o => o.NotificationID));
                    }
                }

                if (skipList.Count == 0)
                {
                    ShowNotificationLoading.Begin();
                }


                Services.NotificationServiceClient.GetNotificationsCompleted += NotificationServiceClient_GetNotificationsCompleted;
                Services.NotificationServiceClient.GetNotificationsAsync(Core.User.User.UserID, 10, skipList, Core.User.User.ZAT);
            }
            catch { }

        }

        void NotificationServiceClient_GetNotificationsCompleted(object sender, NotificationService.GetNotificationsCompletedEventArgs e)
        {
            try
            {


                AlertMoreLoadingPB.Visibility = System.Windows.Visibility.Collapsed;

                Services.NotificationServiceClient.GetNotificationsCompleted -= NotificationServiceClient_GetNotificationsCompleted;

                if (e.Error == null)
                {
                    if (e.Result != null)
                    {
                        if (NotificationList.Notifications != null)
                        {
                            foreach (NotificationService.Notification x in e.Result)
                            {
                                NotificationList.Notifications.Add(x);
                            }
                        }
                        else
                        {
                            NotificationList.Notifications = e.Result;
                        }

                        if (NotificationList.Notifications == null || NotificationList.Notifications.Count == 0)
                        {
                            ShowNoNotification.Begin();
                        }
                        else
                        {
                            ShowNotificationPage.Begin();
                        }

                    }
                    else
                    {
                        if (NotificationList.Notifications == null || NotificationList.Notifications.Count == 0)
                        {
                            ShowNoNotification.Begin();
                        }
                    }

                }
                else
                {
                    //show error LBL
                    ShowNotificationError.Begin();
                }


                if (e.Result != null)
                {
                    if (e.Result.Count == 10)
                    {
                        NotificationList.MoreDataRequested += NotificationList_MoreDataRequested;
                    }
                }

            }

            catch { }



        }



        void DiscoverLocationPanel_NavigateToLocationSelectorPage()
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.LocationPicker, UriKind.RelativeOrAbsolute));
                });
            }
            catch { }
        }

        void DiscoverList_MoreItemsRequested(object sender, EventArgs e)
        {
            try
            {
                LoadDiscoverItems();
            }
            catch { }
        }



        void DiscoverList_TagSelected(String name)
        {
            try
            {
                if (name.StartsWith("#"))
                {
                    name = name.TrimStart('#');
                }
                NavigateToTag(name);
            }
            catch { }
        }

        private void NavigateToTag(String name)
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.Tag + "?name=" + name.ToString(), UriKind.RelativeOrAbsolute));
                });
            }
            catch { }
        }

        void DiscoverList_UserSelected(Guid id)
        {
            try
            {
                NavigateToUserProfile(id);
            }
            catch { }
        }



        void MainTextLB_TextSelected(Guid id)
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.UserText + "?id=" + id, UriKind.RelativeOrAbsolute));
                });
            }
            catch { }
        }



        void UserProfileItem_BuzzLoaded()
        {
            try
            {
                ProfileBottomPB.Visibility = System.Windows.Visibility.Collapsed;
                ProfileBuzzLoading = false;
            }
            catch { }
        }

        void UserProfileItem_BuzzLoading()
        {
            try
            {
                ProfileBottomPB.Visibility = System.Windows.Visibility.Visible;
            }
            catch { }
        }

        void UserProfileItem_MessageSelected(Guid id)
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.ShowMessagesByUser + "?id=" + id.ToString(), UriKind.RelativeOrAbsolute));
                });
            }
            catch { }
        }

        void UserProfileItem_Error()
        {
            try
            {
                ShowProfileError.Begin();
            }
            catch { }
        }

        void UserProfileItem_Loading()
        {
            try
            {
                ShowProfileLoading.Begin();
            }
            catch { }
        }

        void UserProfileItem_Loaded()
        {
            try
            {
                ShowProfilePage.Begin();
            }
            catch { }
        }





        void BuzzHeaderPanel_NavigateToComposeBuzz()
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.AddBuzz, UriKind.RelativeOrAbsolute));
                });
            }
            catch { }
        }

        void BuzzHeaderPanel_NavigateToLocationSelectorPage()
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.LocationPicker, UriKind.RelativeOrAbsolute));
                });
            }
            catch { }
        }

        void BuzzLB_ScrollingTop()
        {
            try
            {
                HideBuzzHeader.Begin();
            }
            catch { }
        }

        void BuzzLB_ScrollingDown()
        {
            try
            {
                ShowBuzzHeader.Begin();
            }
            catch { }
        }

        void BuzzLB_MoreDataRequested()
        {
            try
            {
                BuzzLB.BuzzLoadingProgressBarVisible();
                LoadBuzz();
            }
            catch { }
        }

        void BuzzLB_PullToRefreshEvent()
        {
            try
            {
                NewBuzzIcon.Visibility = System.Windows.Visibility.Collapsed;
                GetNewBuzz();
            }
            catch { }
        }

        private void GetNewBuzz()
        {
            try
            {
                if (BuzzLB.Buzzs != null)
                {
                    if (BuzzLB.Buzzs.Count > 0)
                    {
                        Services.BuzzServiceClient.GetBuzzBeforeCompleted += BuzzServiceClient_GetBuzzBeforeCompleted;
                        Services.BuzzServiceClient.GetBuzzBeforeAsync(BuzzLB.Buzzs.First().BuzzID, Core.User.User.UserID, Core.Location.CurrentLocation.Location.Latitude, Core.Location.CurrentLocation.Location.Longitude, Core.Location.CurrentLocation.Location.City.Id, Core.User.User.ZAT);
                    }
                }
            }
            catch { }

        }

        void BuzzServiceClient_GetBuzzBeforeCompleted(object sender, BuzzService.GetBuzzBeforeCompletedEventArgs e)
        {
            try
            {
                Services.BuzzServiceClient.GetBuzzBeforeCompleted -= BuzzServiceClient_GetBuzzBeforeCompleted;

                if (e.Error == null)
                {
                    if (e.Result != null)
                    {
                        if (BuzzLB.Buzzs == null)
                        {
                            BuzzLB.Buzzs = new System.Collections.ObjectModel.ObservableCollection<BuzzService.Buzz>();
                        }

                        for (int i = e.Result.Count - 1; i >= 0; i--)
                        {
                            BuzzLB.Buzzs.Insert(0, e.Result[i]);
                        }

                    }
                }
                else
                {
                    //Show error LBL
                    ShowBuzzError.Begin();
                }

                BuzzLB.StopPullToRefreshLoading();
            }
            catch { }
        }

        void BuzzLB_PeopleSelected(string text, Guid id)
        {
            try
            {
                NavigateToPeopleList(text, id);
            }
            catch { }
        }

        private void NavigateToPeopleList(string text, Guid id)
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.PeopleList + "?text=" + text + "&id=" + id.ToString(), UriKind.RelativeOrAbsolute));
                });
            }
            catch { }
        }

        void BuzzLB_TagSelected(string name)
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.Tag + "?name=" + name, UriKind.RelativeOrAbsolute));
                });
            }
            catch { }
        }

        void BuzzLB_NavigateToTwitter()
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.TwitterConnect, UriKind.RelativeOrAbsolute));
                });
            }
            catch { }
        }

        void BuzzLB_NavigateToFacebook()
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.FacebookConnect, UriKind.RelativeOrAbsolute));
                });
            }
            catch { }
        }

        void BuzzLB_ProgressBarShow()
        {
            try
            {
                ShowProgressBar();
            }
            catch { }
        }

        void BuzzLB_ProgressBarDisappear()
        {
            try
            {
                HideProgressBar();
            }
            catch { }
        }

        void BuzzLB_UserSelected(Guid id)
        {
            try
            {
                NavigateToUserProfile(id);
            }
            catch { }
        }

        private void NavigateToUserProfile(Guid id)
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.UserProfile + "?id=" + id.ToString(), UriKind.RelativeOrAbsolute));
                });
            }
            catch { }
        }

        void BuzzLB_CommentOnBuzzSelected(Guid id)
        {
            try
            {
                NavigateToBuzzCommentPage(id);
            }
            catch { }
        }

        private void NavigateToBuzzCommentPage(Guid id)
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.BuzzComments + "?id=" + id.ToString(), UriKind.RelativeOrAbsolute));
                });
            }
            catch { }
        }

        void CurrentLocation_UserLocationUpdateCompleted()
        {
            try
            {
                if (Core.Location.CurrentLocation.Location != null)
                {

                    BuzzHeaderPanel.UpdateLocation(Core.Location.CurrentLocation.Location);
                    DiscoverLocationPanel.UpdateLocation(Core.Location.CurrentLocation.Location);
                    ShowDiscoverLocationPanel.Begin();
                    SelectedCity = Core.Location.CurrentLocation.Location.City;
                    Core.Location.Location.SelectedCity = Core.Location.CurrentLocation.Location.City;
                    this.ApplicationBar.IsVisible = true;
                    newbuzz.CityID = SelectedCity.Id;
                    ShowBuzzHeader.Begin();
                    LoadActivePage();


                    if (PendingDiscoverLoad)
                    {
                        LoadDiscoverItems();
                    }
                }
                else
                {
                    MessageBox.Show("We can't get your location at this point in time. We cannot function without your lcoation. If you have location services turned off please turn that on from phone settings.", "Location services turned off? ", MessageBoxButton.OK);
                    Application.Current.Terminate();
                }
            }
            catch { }
        }

        private void LoadActivePage()
        {
            try
            {
                if (MainPivot.SelectedIndex == 0)
                {
                    LoadBuzz();
                }
                if (MainPivot.SelectedIndex == 1)
                {
                    LoadDiscoverItems();
                }
                if (MainPivot.SelectedIndex == 2)
                {
                    LoadNotifications();

                }
                if (MainPivot.SelectedIndex == 3)
                {
                    LoadText();
                }
            }
            catch { }


        }



        private void LoadText()
        {
            try
            {


                MainTextLB.DataRequested -= MainTextLB_DataRequested;
                System.Collections.ObjectModel.ObservableCollection<Guid> skipList = new System.Collections.ObjectModel.ObservableCollection<Guid>();

                if (MainTextLB.Texts != null)
                {
                    if (MainTextLB.Texts.Count > 0)
                    {
                        MoreTextPB.Visibility = System.Windows.Visibility.Visible;
                        skipList = new System.Collections.ObjectModel.ObservableCollection<Guid>(MainTextLB.Texts.Select(o => o.TextId));
                    }
                }

                if (skipList.Count == 0)
                {
                    ShowTextLoading.Begin();
                }


                Services.TextServiceClient.GetTextCompleted += TextServiceClient_GetTextCompleted;
                Services.TextServiceClient.GetTextAsync(Core.User.User.UserID, skipList, 10, Core.User.User.ZAT);
            }
            catch { }

        }

        void TextServiceClient_GetTextCompleted(object sender, TextService.GetTextCompletedEventArgs e)
        {
            try
            {
                MoreTextPB.Visibility = System.Windows.Visibility.Collapsed;

                Services.TextServiceClient.GetTextCompleted -= TextServiceClient_GetTextCompleted;

                if (e.Error == null)
                {
                    if (e.Result != null)
                    {
                        if (MainTextLB.Texts != null)
                        {
                            foreach (TextService.Text x in e.Result)
                            {
                                if (MainTextLB.Texts.Where(o => o.User.UserID == x.User.UserID).Count() > 0)
                                {
                                    //remove. 
                                    TextService.Text temp = MainTextLB.Texts.Where(o => o.User.UserID == x.User.UserID).FirstOrDefault();

                                    if (temp != null)
                                    {
                                        MainTextLB.Texts.Remove(temp);
                                    }
                                }

                                MainTextLB.Texts.Add(x);
                            }
                        }
                        else
                        {
                            MainTextLB.Texts = e.Result;
                        }

                        if (MainTextLB.Texts == null || MainTextLB.Texts.Count == 0)
                        {
                            ShowNoText.Begin();
                        }
                        else
                        {
                            ShowText.Begin();
                        }

                    }
                    else
                    {
                        if (MainTextLB.Texts == null || MainTextLB.Texts.Count == 0)
                        {
                            ShowNoText.Begin();
                        }
                    }

                }
                else
                {
                    //show error LBL
                    ShowTextError.Begin();
                }


                if (e.Result != null)
                {
                    if (e.Result.Count == 10)
                    {
                        MainTextLB.DataRequested += MainTextLB_DataRequested;
                    }
                }

                MainTextLB.CollapseLoader();
            }
            catch { }
        }

        void MainTextLB_DataRequested(object sender, EventArgs e)
        {
            try
            {
                LoadText();
            }
            catch { }
        }

        private void LoadBuzz()
        {
            try
            {
                BuzzLB.MoreDataRequested -= BuzzLB_MoreDataRequested;
                System.Collections.ObjectModel.ObservableCollection<Guid> skipList = new System.Collections.ObjectModel.ObservableCollection<Guid>();

                if (BuzzLB.Buzzs != null)
                {
                    if (BuzzLB.Buzzs.Count > 0)
                    {
                        BuzzPB.Visibility = System.Windows.Visibility.Visible;
                        skipList = new System.Collections.ObjectModel.ObservableCollection<Guid>(BuzzLB.Buzzs.Select(o => o.BuzzID));
                    }
                }

                if (skipList.Count == 0)
                {
                    ShowBuzzLoading.Begin();
                }

                if (SelectedCity != null && SelectedCity != Core.Location.CurrentLocation.Location.City)
                {
                    Services.BuzzServiceClient.GetBuzzCompleted += BuzzServiceClient_GetBuzzCompleted;
                    Services.BuzzServiceClient.GetBuzzAsync(Core.User.User.UserID, 10, skipList, 0.0, 0.0, SelectedCity.Id, Core.User.User.ZAT);
                }
                else
                {
                    Services.BuzzServiceClient.GetBuzzCompleted += BuzzServiceClient_GetBuzzCompleted;
                    Services.BuzzServiceClient.GetBuzzAsync(Core.User.User.UserID, 10, skipList, Core.Location.CurrentLocation.Location.Latitude, Core.Location.CurrentLocation.Location.Longitude, Core.Location.CurrentLocation.Location.City.Id, Core.User.User.ZAT);
                }
            }
            catch { }
        }

        public int ProgressBarCount { get; set; }

        private void ShowProgressBar()
        {
            try
            {
                ProgressBarCount++;
                MainProgressBar.Visibility = System.Windows.Visibility.Visible;
            }
            catch { }
        }

        private void HideProgressBar()
        {
            try
            {
                --ProgressBarCount;

                if (ProgressBarCount == 0)
                {
                    MainProgressBar.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
            catch { }
        }


        void BuzzServiceClient_GetBuzzCompleted(object sender, BuzzService.GetBuzzCompletedEventArgs e)
        {
            try
            {

                BuzzPB.Visibility = System.Windows.Visibility.Collapsed;

                Services.BuzzServiceClient.GetBuzzCompleted -= BuzzServiceClient_GetBuzzCompleted;

                if (e.Error == null)
                {
                    if (e.Result != null)
                    {
                        if (BuzzLB.Buzzs != null)
                        {
                            foreach (BuzzService.Buzz x in e.Result)
                            {
                                if (BuzzLB.Buzzs.Where(o => o.BuzzID == x.BuzzID).Count() == 0)
                                {
                                    BuzzLB.Buzzs.Add(x);
                                }

                            }
                        }
                        else
                        {
                            BuzzLB.Buzzs = e.Result;
                        }

                        if (BuzzLB.Buzzs == null || BuzzLB.Buzzs.Count == 0)
                        {
                            ShowNoBuzz.Begin();
                        }
                        else
                        {
                            ShowBuzzPage.Begin();
                        }

                    }
                    else
                    {
                        if (BuzzLB.Buzzs == null || BuzzLB.Buzzs.Count == 0)
                        {
                            ShowNoBuzz.Begin();
                        }
                    }

                }
                else
                {
                    //show error LBL
                    ShowBuzzError.Begin();
                }


                if (e.Result != null)
                {
                    if (e.Result.Count == 10)
                    {
                        BuzzLB.MoreDataRequested += BuzzLB_MoreDataRequested;
                    }
                }

                BuzzLB.BuzzLoadingProgressBarCollapse();
                UpdateNewBuzzIndex();
                ActivateNewBuzz();
            }
            catch { }

        }

        private void UpdateNewBuzzIndex()
        {
            try
            {
                if (BuzzLB.Buzzs.Count == 0)
                {
                    newbuzz.BuzzID = new Guid();
                }
                else
                {
                    newbuzz.BuzzID = BuzzLB.Buzzs.First().BuzzID;
                }
            }
            catch { }
        }

        private void ActivateNewBuzz()
        {
            try
            {

                if (!NewBuzzIconActivated)
                {
                    NewBuzzIconActivated = true;

                    Thread oThread = new Thread(new ThreadStart(newbuzz.LoopForNewBuzz));

                    // Start the thread
                    oThread.Start();

                }
            }
            catch { }
        }



        private void ProfileIcon_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                ProfilePageHeaderSB.Begin();
                MainPivot.SelectedItem = ProfilePivot;
            }
            catch { }
        }

        private void MessageIcon_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                MailPageHeaderSB.Begin();
                MainPivot.SelectedItem = MailPivot;
            }
            catch { }
        }

        private void AlertIcon_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                AlertPageHeaderSB.Begin();
                MainPivot.SelectedItem = AlertPivot;
            }
            catch { }
        }

        private void DiscoverIcon_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                DiscoverPageHeaderSB.Begin();
                MainPivot.SelectedItem = DiscoverPivot;
            }
            catch { }
        }

        private void BuzzIcon_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                BuzzPageHeaderSB.Begin();
                MainPivot.SelectedItem = BuzzPivot;
            }
            catch { }
        }

        private void ComposeBuzzBtn_Click(object sender, EventArgs e)
        {
            try
            {
                NavigateToComposeBuzz();
            }
            catch { }
        }

        private void NavigateToComposeBuzz()
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.AddBuzz, UriKind.RelativeOrAbsolute));
                });
            }
            catch { }
        }

        private void LoadDiscoverItems()
        {
            try
            {
                if (Core.Location.Location.SelectedCity == null)
                {
                    PendingDiscoverLoad = true;
                    ShowDiscoverLoading.Begin();
                }
                else
                {

                    PendingDiscoverLoad = false;

                    this.DiscoverList.MoreItemsRequested -= DiscoverList_MoreItemsRequested;

                    System.Collections.ObjectModel.ObservableCollection<Guid> skipList = new System.Collections.ObjectModel.ObservableCollection<Guid>();

                    if (this.DiscoverList.DiscoverListData != null)
                    {


                        if (this.DiscoverList.DiscoverListData.Count > 0)
                        {

                            DiscoverPB.Visibility = System.Windows.Visibility.Visible;

                            skipList = new System.Collections.ObjectModel.ObservableCollection<Guid>(this.DiscoverList.DiscoverListData.Select(o => o.Tag).Where(o => o != null).Select(o => o.Tag.Id).ToList());
                            //add

                            skipList.Concat(new System.Collections.ObjectModel.ObservableCollection<Guid>(this.DiscoverList.DiscoverListData.Select(o => o.User).Where(o => o != null).Select(o => o.UserID).ToList()));

                        }
                    }

                    if (skipList.Count == 0)
                    {
                        ShowDiscoverLoading.Begin();
                    }


                    Services.DiscoverServiceClient.GetItemsCompleted += DiscoverServiceClient_GetItemsCompleted;
                    Services.DiscoverServiceClient.GetItemsAsync(Core.User.User.UserID, 10, skipList, Core.Location.CurrentLocation.Location.City.Id, Core.User.User.ZAT);
                }
            }
            catch { }


        }

        void DiscoverServiceClient_GetItemsCompleted(object sender, DiscoverService.GetItemsCompletedEventArgs e)
        {
            try
            {
                DiscoverPB.Visibility = System.Windows.Visibility.Collapsed;

                Services.DiscoverServiceClient.GetItemsCompleted -= DiscoverServiceClient_GetItemsCompleted;

                if (e.Error == null)
                {
                    if (e.Result != null)
                    {
                        if (this.DiscoverList.DiscoverListData != null)
                        {
                            foreach (DiscoverService.DiscoverObj x in e.Result)
                            {
                                this.DiscoverList.DiscoverListData.Add(x);
                            }
                        }
                        else
                        {
                            this.DiscoverList.DiscoverListData = e.Result;
                        }

                        if (this.DiscoverList.DiscoverListData == null || this.DiscoverList.DiscoverListData.Count == 0)
                        {
                            ShowNoDiscover.Begin();
                        }
                        else
                        {
                            ShowDiscoverPage.Begin();
                        }

                    }
                    else
                    {
                        if (this.DiscoverList.DiscoverListData == null || this.DiscoverList.DiscoverListData.Count == 0)
                        {
                            ShowNoDiscover.Begin();
                        }
                    }

                }
                else
                {
                    //show error LBL
                    ShowDiscoverErrorBEgin.Begin();
                }


                this.DiscoverList.MoreItemsRequested += DiscoverList_MoreItemsRequested;

                this.DiscoverList.BuzzLoadingProgressBarCollapse();
            }
            catch { }
        }

        private void MainPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (MainPivot.SelectedItem == BuzzPivot)
                {
                    BuzzPageHeaderSB.Begin();
                }

                if (MainPivot.SelectedItem == AlertPivot)
                {
                    AlertPageHeaderSB.Begin();


                    if (NotificationList.Notifications == null || NotificationList.Notifications.Count == 0)
                    {
                        NewNotificationIcon.Visibility = System.Windows.Visibility.Collapsed;
                    }


                    if (!NotificationsLoaded)
                    {

                        NotificationsLoaded = true;
                        LoadNotifications();
                    }
                }


                if (MainPivot.SelectedItem == DiscoverPivot)
                {
                    DiscoverPageHeaderSB.Begin();
                    if (!DiscoverPage)
                    {
                        DiscoverPage = true;
                        LoadDiscoverItems();
                    }
                }

                if (MainPivot.SelectedItem == MailPivot)
                {
                    MailPageHeaderSB.Begin();
                    NewMessageIcon.Visibility = System.Windows.Visibility.Collapsed;
                    if (!MailPage)
                    {

                        MailPage = true;
                        Core.PushNotification.PushNotification.RealTimeText += PushNotification_RealTimeText;
                        LoadText();
                    }
                }

                if (MainPivot.SelectedItem == ProfilePivot)
                {
                    ProfilePageHeaderSB.Begin();

                    if (!ProfilePage)
                    {
                        ProfilePage = true;
                        UserProfileItem.LoadUser(Core.User.User.UserID);
                    }
                }
            }
            catch { }


        }

        void PushNotification_RealTimeText(TextService.Text text)
        {
            try
            {

                if (MainPivot.SelectedItem != MailPivot)
                {
                    NewMessageIcon.Visibility = System.Windows.Visibility.Visible;
                }

                if (MainTextLB.Texts.Where(o => o.User.UserID == text.User.UserID).Count() > 0)
                {
                    //remove
                    MainTextLB.Texts.Remove(MainTextLB.Texts.Where(o => o.User.UserID == text.User.UserID).FirstOrDefault());
                }

                MainTextLB.Texts.Add(text);
                MainTextLB.Texts = new System.Collections.ObjectModel.ObservableCollection<TextService.Text>(MainTextLB.Texts.OrderByDescending(o => o.DateTimeStamp));
            }
            catch { }

        }



        private void editProfileBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.Profile, UriKind.RelativeOrAbsolute));
                });
            }
            catch { }
        }


        public UserLocationService.City SelectedCity { get; set; }

        public bool ProfilePage { get; set; }






        public bool alreadyHookedScrollEvents { get; set; }



        public bool ProfileBuzzLoading { get; set; }

        public bool MailPage { get; set; }

        public bool DiscoverPage { get; set; }

        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to log out?", "Log out?", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    //log out. 
                    Core.Authentication.Accounts.Logout();

                    //clear stack. 

                    //navigate to start. 
                    Dispatcher.BeginInvoke(() =>
                    {
                        NavigationService.Navigate(new Uri(Navigation.Start, UriKind.RelativeOrAbsolute));
                    });
                }
            }
            catch { }
        }

        private void ApplicationBarMenuItem_Click_1(object sender, EventArgs e)
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.About, UriKind.RelativeOrAbsolute));
                });
            }
            catch { }
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            try
            {
                //search page
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.Search, UriKind.RelativeOrAbsolute));
                });
            }
            catch { }

        }

        public bool PendingDiscoverLoad { get; set; }

        public bool NewBuzzIconActivated { get; set; }

        private void ApplicationBarMenuItem_Click_2(object sender, EventArgs e)
        {
            try
            {
                //settings
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.Settings, UriKind.RelativeOrAbsolute));
                });
            }
            catch { }

        }

        public bool NotificationsLoaded { get; set; }
    }
}