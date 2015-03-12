using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Hangout.Client.Controls.Notifications
{
    public partial class NotificationList : UserControl
    {
        public NotificationList()
        {
            InitializeComponent();
        }

        private List<NotificationService.Notification> notifications;

        public List<NotificationService.Notification> Notifications
        {
            get
            {
                return notifications;
            }

            set
            {
                notifications = value;
                NotificationsLB.ItemsSource = null;
                NotificationsLB.ItemsSource = Notifications;
            }
        }


        public delegate void IdHelper(int id);

        public event IdHelper BuzzSelected;
        public event EventHandler MoreBtnClicked;
        public event IdHelper TagSelected;
        public event IdHelper CategorySelected;
        public event IdHelper UserSelected;
        public event IdHelper VenueSelected;
        public event IdHelper TrophySelected;

        public void RefreshList()
        {
            NotificationsLB.DataContext = null;
            NotificationsLB.ItemsSource = null;
            NotificationsLB.DataContext = notifications;
            NotificationsLB.ItemsSource = notifications;
        }

        private void MoreBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (MoreBtnClicked != null)
            {
                MoreBtnClicked(null, new EventArgs());
            }
        }

        public void ShowDownProgressBar()
        {
            DownProgressBar.Visibility = System.Windows.Visibility.Visible;
            MoreBtn.Visibility = System.Windows.Visibility.Collapsed;
        }

        public void HideDownPanel()
        {
            DownProgressBar.Visibility = System.Windows.Visibility.Collapsed;
            MoreBtn.Visibility = System.Windows.Visibility.Collapsed;
        }

        public void ShowUpProgressBar()
        {
            UpProgressBar.Visibility = System.Windows.Visibility.Visible;
        }

        public void HideUpPanel()
        {
            UpProgressBar.Visibility = System.Windows.Visibility.Collapsed;
            
        }

        public void ShowMoreButton()
        {
            DownProgressBar.Visibility = System.Windows.Visibility.Collapsed;
            MoreBtn.Visibility = System.Windows.Visibility.Visible;
        }

        private void NotificationItem_Loaded_1(object sender, RoutedEventArgs e)
        {
            NotificationListItem x = (NotificationListItem)sender;
            x.BuzzSelected += x_BuzzSelected;
            x.VenueSelected += x_VenueSelected;
            x.CategorySelected += x_CategorySelected;
            x.TagSelected += x_TagSelected;
            x.UserSelected += x_UserSelected;
            x.TrophySelected += x_TrophySelected;
            x.GoToUserTextsPage += x_GoToUserTextsPage;
            x.GoToTextsPage += x_GoToTextsPage;

        }


        public event EventHandler GoToTextsPage;

        public event IdHelper GoToUserTextsPage;

        void x_GoToTextsPage(object sender, EventArgs e)
        {
            if (GoToTextsPage != null)
            {
                GoToTextsPage(null, new EventArgs());
            }
        }

        void x_GoToUserTextsPage(int id)
        {
            if (GoToUserTextsPage != null)
            {
                GoToUserTextsPage(id);
            }
        }

        void x_TrophySelected(int id)
        {
            if (TrophySelected != null)
            {
                TrophySelected(id);
            }
        }

        void x_UserSelected(int id)
        {
            if (UserSelected != null)
            {
                UserSelected(id);
            }
        }

        void x_TagSelected(int id)
        {
            if (TagSelected != null)
            {
                TagSelected(id);
            }
        }

        void x_CategorySelected(int id)
        {
            if (CategorySelected != null)
            {
                CategorySelected(id);
            }
        }

        void x_VenueSelected(int id)
        {
            if (VenueSelected != null)
            {
                VenueSelected(id);
            }
        }

        void x_BuzzSelected(int id)
        {
            if (BuzzSelected != null)
            {
                BuzzSelected(id);
            }
        }


    }
}
