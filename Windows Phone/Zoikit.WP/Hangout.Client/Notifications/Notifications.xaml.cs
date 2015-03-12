using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Hangout.Client.Notifications
{
    public partial class Notifications : PhoneApplicationPage
    {
        public Notifications()
        {
            InitializeComponent();
            Settings.Settings.UnreadNotifications = 0;
            Notifier.NotificationClicked += Notifier_NotificationClicked;
            Notifier.NotificationDisplayed += Notifier_NotificationDisplayed;
            Notifier.NotificationHidden += Notifier_NotificationHidden;
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigateToDashboard();
        }

        private void NavigateToDashboard()
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Dashboard, UriKind.RelativeOrAbsolute));
            });

            return;
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

        


        #endregion

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {


            if (!Core.Network.NetworkStatus.IsNetworkAvailable())
            {

                ShowError.Begin();
                return;
            }
            

            NotificationList.UserSelected += NotificationList_UserSelected;
            NotificationList.VenueSelected += NotificationList_VenueSelected;
            NotificationList.TagSelected += NotificationList_TagSelected;
            NotificationList.BuzzSelected += NotificationList_BuzzSelected;
            NotificationList.CategorySelected += NotificationList_CategorySelected;
            NotificationList.MoreBtnClicked += NotificationList_MoreBtnClicked;
            NotificationList.GoToTextsPage += NotificationList_GoToTextsPage;
            NotificationList.GoToUserTextsPage += NotificationList_GoToUserTextsPage;
            NotificationList.TrophySelected += NotificationList_TrophySelected;
            textLoader.DisplayText("Looking up for notifications...");
            //get and save location to the server

            GetNotifications();


        }

        void NotificationList_GoToUserTextsPage(int id)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.UserText + "?id=" + id, UriKind.RelativeOrAbsolute));
            });
        }

        void NotificationList_GoToTextsPage(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Texts, UriKind.RelativeOrAbsolute));
            });
        }

        void NotificationList_TrophySelected(int id)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.ViewProfile + "?id=" + id+"&view=trophy", UriKind.RelativeOrAbsolute));
            });
        }

        void Notifier_NotificationClicked(object sender, EventArgs e)
        {
            Settings.Settings.UnreadNotifications = 0;
            NotificationList.ShowUpProgressBar();
            scrollViewer.ScrollToVerticalOffset(0);
            GetNotifications();
        }

        void NotificationList_MoreBtnClicked(object sender, EventArgs e)
        {
            NotificationList.MoreBtn.Visibility = System.Windows.Visibility.Collapsed;
            NotificationList.ShowDownProgressBar();
            GetNotifications();
        }

        private void GetNotifications()
        {
            List<int> skipList=new List<int>();

            if(NotificationList.Notifications==null)
            {
                NotificationList.Notifications=new List<NotificationService.Notification>();
            }

            skipList=NotificationList.Notifications.Select(o=>o.NotificationID).ToList();

            Services.NotificationServiceClient.GetNotificationsCompleted += NotificationServiceClient_GetNotificationsCompleted;
            Services.NotificationServiceClient.GetNotificationsAsync(Core.User.User.UserID,10,new System.Collections.ObjectModel.ObservableCollection<int>(skipList),Core.User.User.ZAT);
        }

        void NotificationServiceClient_GetNotificationsCompleted(object sender, NotificationService.GetNotificationsCompletedEventArgs e)
        {
            Services.NotificationServiceClient.GetNotificationsCompleted -= NotificationServiceClient_GetNotificationsCompleted;

            NotificationList.HideDownPanel();
            NotificationList.HideUpPanel();

            if (NotificationList.Notifications.Count > 0)
            {
                NotificationList.Notifications.ForEach(o => o.MarkAsRead = true);
            }

            if (e.Error != null)
            {
                ShowError.Begin();
                return;
            }
            if (e.Result != null)
            {
                if (e.Result.Count > 0)
                {
                    NotificationList.Notifications.AddRange(e.Result);
                    NotificationList.Notifications = NotificationList.Notifications.OrderByDescending(o => o.DatetimePosted).ToList();
                }
            }

            if (NotificationList.Notifications.Count > 0)
            {
                NotificationList.RefreshList();
                NotificationList.Notifications.ForEach(x => x.MarkAsRead = true);
                ShowPage.Begin();
            }
            else
            {
                ShowNoNotifications.Begin();
            }
            NotificationList.ShowMoreButton();

        }

        void NotificationList_CategorySelected(int id)
        {
            Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.Category + "?id=" + id, UriKind.RelativeOrAbsolute));
                });
        }

        void NotificationList_BuzzSelected(int id)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Buzz + "?id=" + id, UriKind.RelativeOrAbsolute));
            });
        }

        void NotificationList_TagSelected(int id)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Tag + "?id=" + id, UriKind.RelativeOrAbsolute));
            });
        }

        void NotificationList_VenueSelected(int id)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Venue + "?id=" + id, UriKind.RelativeOrAbsolute));
            });
        }

        void NotificationList_UserSelected(int id)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.ViewProfile + "?id=" + id, UriKind.RelativeOrAbsolute));
            });
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
                {

                    Settings.Settings.UnreadNotifications = 0;
                    Services.NotificationServiceClient.MarkAllAsReadAsync(Core.User.User.UserID, Core.User.User.ZAT);
                });
        }
    }
}