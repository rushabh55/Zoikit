using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Hangout.Client
{
    public partial class UserShortList : UserControl
    {

        public delegate void UserSelectedHelper(int id);

        public event UserSelectedHelper UserSelected;


        private List<NotificationService.CompactUser> users;


        public List<NotificationService.CompactUser> Users
        {
            get
            {
                return users;
            }

            set
            {
                if (value == null)
                {
                    //collapse
                    Collapse();

                    return;
                }

                if (value.Count == 0)
                {
                    //collapse
                    Collapse();
                    return;


                }

                users = value;

                UserShortDisplayLB.DataContext = null;
                UserShortDisplayLB.ItemsSource = null;
                UserShortDisplayLB.DataContext = users;
                UserShortDisplayLB.ItemsSource = users;
                MakeVisible();


            }
        }

        private void MakeVisible()
        {
            this.Visibility = System.Windows.Visibility.Visible;
        }

        private void Collapse()
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
        }

        public UserShortList()
        {
            InitializeComponent();
        }

        private void UserShortDisplayLB_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (UserShortDisplayLB.SelectedItem != null)
            {
                NotificationService.CompactUser user = (NotificationService.CompactUser)UserShortDisplayLB.SelectedItem;

                if (user != null && UserSelected != null)
                {
                    UserSelected(user.UserID);
                }

            }
        }
    }
}
