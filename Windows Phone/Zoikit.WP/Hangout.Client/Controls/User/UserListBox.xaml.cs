using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections;
using System.Collections.Generic;

namespace Hangout.Client.Controls.User
{
    public partial class UserListBox : UserControl
    {

        public delegate void UserProfileSelectedHandler(UserService.User profile);

        public event UserProfileSelectedHandler UserProfileSelected;

        public event EventHandler MoreBtnClicked;

        public UserListBox()
        {
            // Required to initialize variables
            InitializeComponent();
        }

        private List<UserService.User> users;

        public List<UserService.User> Users
        {

            get
            {
                return users;
            }

            set
            {
                users = value;
                UsersLB.ItemsSource = null;
                UsersLB.ItemsSource = users;
            }
        }

        private void UsersLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (UsersLB.SelectedItem != null)
                {
                    if (UserProfileSelected != null)
                    {
                        UserProfileSelected((UserService.User)UsersLB.SelectedItem);
                    }
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }



        public delegate void CategorySelectedHelper(int id);

        public event CategorySelectedHelper CategorySelected;

        public delegate void TagSelectedHelper(int id);

        public event TagSelectedHelper TagSelected;

        public delegate void UserSelectedHelper(int id);

        public event UserSelectedHelper UserSelected;


        private void UserLBItem_Loaded_1(object sender, RoutedEventArgs e)
        {
            UserListboxItem x = (UserListboxItem)sender;

            x.TagSelected += x_TagSelected;
            x.CategorySelected += x_CategorySelected;
            x.UserSelected += x_UserSelected;

        }

        void x_UserSelected(int id)
        {
            if (UserSelected != null)
            {
                UserSelected(id);
            }
        }

        void x_CategorySelected(int id)
        {
            if (CategorySelected != null)
            {
                CategorySelected(id);
            }
        }

        void x_TagSelected(int id)
        {
            if (TagSelected != null)
            {
                TagSelected(id);
            }
        }

        private void MoreBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (MoreBtnClicked != null)
            {
                MoreBtnClicked(null, new EventArgs());
            }
        }

        public void ShowMoreButton()
        {
            MorePG.Visibility = System.Windows.Visibility.Visible;
            MoreBtn.Visibility = System.Windows.Visibility.Collapsed;
        }

        public void ShowProgressBar()
        {
            MorePG.Visibility = System.Windows.Visibility.Collapsed;
            MoreBtn.Visibility = System.Windows.Visibility.Visible;
        }

        public void HideDownPanel()
        {
            MorePG.Visibility = System.Windows.Visibility.Collapsed;
            MoreBtn.Visibility = System.Windows.Visibility.Collapsed;
        }

        public void RefreshList()
        {
            UsersLB.DataContext = null;
            UsersLB.ItemsSource = null;
            UsersLB.DataContext = users;
            UsersLB.ItemsSource = users;
        }




    }
}