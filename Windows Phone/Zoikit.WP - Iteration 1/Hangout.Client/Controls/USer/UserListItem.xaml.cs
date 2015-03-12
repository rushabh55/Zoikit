using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;

namespace Hangout.Client.Controls.User
{
    public partial class UserListItem : UserControl
    {
        public UserListItem()
        {
            InitializeComponent();
        }

        public UserService.User User
        {
            get { return (UserService.User)GetValue(BuzzProperty); }

            set
            {
                SetValue(BuzzProperty, value);
            }
        }

        public delegate void IdHelper(Guid id);

        

        public event IdHelper UserSelected;

       

        public static readonly DependencyProperty BuzzProperty =
        DependencyProperty.Register(
            "User",
            typeof(UserService.User),
            typeof(UserListItem),
            new PropertyMetadata(null, UserValueChanged));


        private static void UserValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UserListItem obj = (UserListItem)d;
            obj.DisplayData((UserService.User)e.NewValue);
        }

        public void DisplayData(UserService.User user)
        {
            if(user!=null)
            {

                NameLBL.Text = user.Name;
                AboutMeLBL.Text = user.AboutUs;
                
                if(user.UserID==Core.User.User.UserID)
                {
                    FollowBtn.Visibility = System.Windows.Visibility.Collapsed;
                    UnFollowBtn.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    if(user.IsFollowing)
                    {
                        FollowBtn.Visibility = System.Windows.Visibility.Visible;
                        UnFollowBtn.Visibility = System.Windows.Visibility.Visible;
                        ShowUnFollow.Begin();
                        
                    }
                    else
                    {
                        FollowBtn.Visibility = System.Windows.Visibility.Visible;
                        UnFollowBtn.Visibility = System.Windows.Visibility.Visible;
                        ShowFollow.Begin();
                    }

                }

                ProfileImage.Source = new BitmapImage(new Uri(user.ProfilePicURL, UriKind.RelativeOrAbsolute));
            }
        }

        private void StackPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if(UserSelected!=null)
            {
                UserSelected(User.UserID);
            }
        }

        private void ProfileImage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (UserSelected != null)
            {
                UserSelected(User.UserID);
            }
        }

        private void UnFollowBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (this.User != null)
            {
                if (this.User.UserID != Core.User.User.UserID)
                {
                    if (User.IsFollowing)
                    {
                        User.IsFollowing = false;
                        ShowFollow.Begin();
                        Services.UserServiceClient.UnfollowUserCompleted += UserServiceClient_UnfollowUserCompleted;
                        Services.UserServiceClient.UnfollowUserAsync(Core.User.User.UserID, User.UserID, Core.User.User.ZAT);
                    }
                }
            }
        }

        void UserServiceClient_UnfollowUserCompleted(object sender, UserService.UnfollowUserCompletedEventArgs e)
        {
            Services.UserServiceClient.UnfollowUserCompleted -= UserServiceClient_UnfollowUserCompleted;

            if (e.Error != null)
            {
                User.IsFollowing = true;
                ShowUnFollow.Begin();
                MessageBox.Show(Messages.CantUnfollow, Messages.Sorry, MessageBoxButton.OK);
            }
        }

      

        void UserServiceClient_FollowUserCompleted(object sender, UserService.FollowUserCompletedEventArgs e)
        {
            Services.UserServiceClient.FollowUserCompleted -= UserServiceClient_FollowUserCompleted;

            if (e.Error != null)
            {
                User.IsFollowing = false;
                ShowFollow.Begin();
                MessageBox.Show(Messages.CantFollow, Messages.Sorry, MessageBoxButton.OK);
            }
        }

        private void FollowBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (this.User != null)
            {
                if (this.User.UserID != Core.User.User.UserID)
                {
                    if (!User.IsFollowing)
                    {
                        User.IsFollowing = true;
                        ShowUnFollow.Begin();
                        Services.UserServiceClient.FollowUserCompleted += UserServiceClient_FollowUserCompleted;
                        Services.UserServiceClient.FollowUserAsync(Core.User.User.UserID, User.UserID, Core.User.User.ZAT);
                    }
                }
            }
        }



        



        
    }
}
