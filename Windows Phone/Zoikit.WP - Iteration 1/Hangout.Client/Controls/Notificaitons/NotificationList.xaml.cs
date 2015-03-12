using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Hangout.Client.Controls.Notificaitons
{
    public partial class NotificationList : UserControl
    {
        public NotificationList()
        {
            InitializeComponent();
        }

       
       

        private System.Collections.ObjectModel.ObservableCollection<NotificationService.Notification> notifications;

        public System.Collections.ObjectModel.ObservableCollection<NotificationService.Notification> Notifications
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


        public delegate void IdHelper(Guid id);
        public delegate void StringHelper(string name);
        public event EventHandler RefreshRequested;
        public event IdHelper BuzzSelected;
        public event EventHandler MoreDataRequested; 
        public event StringHelper TagSelected;
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

        void x_GoToUserTextsPage(Guid id)
        {
            if (GoToUserTextsPage != null)
            {
                GoToUserTextsPage(id);
            }
        }

        void x_TrophySelected(Guid id)
        {
            if (TrophySelected != null)
            {
                TrophySelected(id);
            }
        }

        void x_UserSelected(Guid id)
        {
            if (UserSelected != null)
            {
                UserSelected(id);
            }
        }

        void x_TagSelected(string name)
        {
            if (TagSelected != null)
            {
                TagSelected(name);
            }
        }

        void x_CategorySelected(Guid id)
        {
            if (CategorySelected != null)
            {
                CategorySelected(id);
            }
        }

        void x_VenueSelected(Guid id)
        {
            if (VenueSelected != null)
            {
                VenueSelected(id);
            }
        }

        void x_BuzzSelected(Guid id)
        {
            if (BuzzSelected != null)
            {
                BuzzSelected(id);
            }
        }

        private void NotificationLB_RefreshRequested(object sender, EventArgs e)
        {
            if(RefreshRequested!=null)
            {
                RefreshRequested(null, null);
            }
        }

        private void NotificationLB_DataRequested(object sender, EventArgs e)
        {
            if(MoreDataRequested!=null)
            {
                MoreDataRequested(null, null);
            }
        }

    }
}
