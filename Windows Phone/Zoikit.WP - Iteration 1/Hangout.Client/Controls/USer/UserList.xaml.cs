using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Hangout.Client.Controls.User
{
    public partial class UserList : UserControl
    {
        public UserList()
        {
            InitializeComponent();
        }

        private System.Collections.ObjectModel.ObservableCollection<UserService.User> userList;

        public System.Collections.ObjectModel.ObservableCollection<UserService.User> Users
        {
            get
            {
                return userList;
            }
            set
            {
                userList = value;
                UserLB.ItemsSource = null;
                UserLB.ItemsSource = Users;
            }
        }

        public delegate void IdHelper(Guid id);

        public delegate void EventHelper();

        public event IdHelper UserSelected;

        public event EventHelper DataRequested;

        private void UserLB_DataRequested(object sender, System.EventArgs e)
        {
        	// TODO: Add event handler implementation here.
            if(DataRequested!=null)
            {
                DataRequested();
            }
        }

        private void UserItem_Loaded(object sender, RoutedEventArgs e)
        {
            UserListItem i = sender as UserListItem;
            i.UserSelected += i_UserSelected;
            
        }

        void i_UserSelected(Guid id)
        {
            if(UserSelected!=null)
            {
                UserSelected(id);
            }
        }


        internal void BuzzLoadingProgressBarCollapse()
        {
            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UserLB.DataVirtualizationMode = Telerik.Windows.Controls.DataVirtualizationMode.OnDemandAutomatic;
        }
    }
}
