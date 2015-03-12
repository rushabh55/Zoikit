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
    public partial class UserHeader : UserControl
    {
        public UserHeader()
        {
            InitializeComponent();
        }

        private UserService.CompleteUserProfile profile;

        private void UnFollowBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (this.UserID != null)
            {
                if (UserID != Core.User.User.UserID)
                {
                    if (profile.IsFollowing)
                    {
                        profile.IsFollowing = false;
                        ShowFollowBtn.Begin();
                        Services.UserServiceClient.UnfollowUserCompleted += UserServiceClient_UnfollowUserCompleted;
                        Services.UserServiceClient.UnfollowUserAsync(Core.User.User.UserID, profile.UserID, Core.User.User.ZAT);
                    }
                }
            }
        }

        void UserServiceClient_UnfollowUserCompleted(object sender, UserService.UnfollowUserCompletedEventArgs e)
        {
            Services.UserServiceClient.UnfollowUserCompleted -= UserServiceClient_UnfollowUserCompleted;

            if (e.Error != null)
            {
                profile.IsFollowing = true;
                ShowUnfollowBtn.Begin();
                MessageBox.Show(Messages.CantUnfollow, Messages.Sorry, MessageBoxButton.OK);
            }
        }



        void UserServiceClient_FollowUserCompleted(object sender, UserService.FollowUserCompletedEventArgs e)
        {
            Services.UserServiceClient.FollowUserCompleted -= UserServiceClient_FollowUserCompleted;

            if (e.Error != null)
            {
                profile.IsFollowing = false;
                ShowFollowBtn.Begin();
                MessageBox.Show(Messages.CantFollow, Messages.Sorry, MessageBoxButton.OK);
            }
        }

        private void FollowBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (this.profile != null)
            {
                if (this.profile.UserID != Core.User.User.UserID)
                {
                    if (!profile.IsFollowing)
                    {
                        profile.IsFollowing = true;
                        ShowUnfollowBtn.Begin();
                        Services.UserServiceClient.FollowUserCompleted += UserServiceClient_FollowUserCompleted;
                        Services.UserServiceClient.FollowUserAsync(Core.User.User.UserID, profile.UserID, Core.User.User.ZAT);
                    }
                }
            }
        }

        public UserService.CompleteUserProfile Profile
        {
            get
            {
                return profile;
            }

            set
            {
                profile = value;
                FillCompleteuserData(this.Profile);
            }
        }

        private void FillCompleteuserData(UserService.CompleteUserProfile completeUserProfile)
        {
            if (completeUserProfile != null)
            {
               
                UserID = completeUserProfile.UserID;
                ProfileImage.Source = new BitmapImage(new Uri(completeUserProfile.ProfilePicURL, UriKind.RelativeOrAbsolute));
                NameLBL.Text = completeUserProfile.Name;

                if (completeUserProfile.UserID == Core.User.User.UserID)
                {
                    FollowUnFollowGrid.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    if (completeUserProfile.IsFollowing)
                    {
                        ShowUnfollowBtn.Begin();
                    }
                    else
                    {
                        ShowFollowBtn.Begin();
                    }
                }

                if (completeUserProfile.AboutUs == null || completeUserProfile.AboutUs == "")
                {
                    AboutMeLBL.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    AboutMeLBL.Text = completeUserProfile.AboutUs;
                }

                if (completeUserProfile.City == null)
                {
                    LocationSP.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    LocationLBL.Text = completeUserProfile.City.Name + ", " + completeUserProfile.City.Country.Name;
                }

                Following.Text = completeUserProfile.FollowingCount + " Following";
                Followers.Text = completeUserProfile.FollowersCount + " Followers";
                Shouts.Text = completeUserProfile.BuzzCount + " Shouts";
            }
        }


        private void Followers_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (PeopleSelected != null)
            {
                PeopleSelected("Followers", UserID);
            }
        }

        public delegate void IdHelper(Guid id);
        public delegate void EventHelper();
        public delegate void PeopleHelper(string name, Guid id);
        public event EventHelper Error;
        public event IdHelper MessageSelected;
        public event IdHelper ProfileImageSelected; 
        public event PeopleHelper PeopleSelected;

        private void MessageBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (MessageSelected != null)
            {
                MessageSelected(profile.UserID);
            }
        }

        private void Following_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (PeopleSelected != null)
            {
                PeopleSelected("Following", UserID);
            }
        }

        private void Shouts_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //do nothing. :) 
        }


        public Guid UserID { get; set; }

        private void ProfileImage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (UserID != null && UserID != new Guid())
            {
                if (ProfileImageSelected != null)
                {
                    ProfileImageSelected(UserID);
                }
            }
        }
    }
}
