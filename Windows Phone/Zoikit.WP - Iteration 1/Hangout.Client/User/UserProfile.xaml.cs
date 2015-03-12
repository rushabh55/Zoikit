using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Hangout.Client.User
{
    public partial class UserProfile : PhoneApplicationPage
    {

        Guid userId;
        public UserProfile()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            UserProfileItem.Error += UserItem_Error;
            UserProfileItem.Loading += UserItem_Loading;
            UserProfileItem.Loaded += UserItem_Loaded;
            UserProfileItem.MessageSelected += UserItem_MessageSelected;
            UserProfileItem.Height = double.NaN;
            UserProfileItem.BuzzLoadEnd += UserProfileItem_BuzzLoadEnd;
            ProfileSV.MouseMove += ProfileSV_MouseMove;
            UserProfileItem.BuzzLoading += UserProfileItem_BuzzLoading;
            ProfileSV.Height = 800;
            UserProfileItem.ProfileImageSelected += UserProfileItem_ProfileImageSelected;
            UserProfileItem.Loaded += UserProfileItem_Loaded;
            UserProfileItem.Loading += UserProfileItem_Loading;
            UserProfileItem.Error += UserProfileItem_Error;
            UserProfileItem.MyBuzzLB.CommentOnBuzzSelected += BuzzLB_CommentOnBuzzSelected;
            UserProfileItem.MyBuzzLB.UserSelected += BuzzLB_UserSelected;
            UserProfileItem.MyBuzzLB.NavigateToFacebook += BuzzLB_NavigateToFacebook;
            UserProfileItem.MyBuzzLB.NavigateToTwitter += BuzzLB_NavigateToTwitter;
            UserProfileItem.MyBuzzLB.TagSelected += BuzzLB_TagSelected;
            UserProfileItem.MyBuzzLB.PeopleSelected += MyBuzzLB_PeopleSelected;
            UserProfileItem.PeopleSelected += UserProfileItem_PeopleSelected;
            UserProfileItem.BuzzLoading += UserProfileItem_BuzzLoading;
            UserProfileItem.BuzzLoaded += UserProfileItem_BuzzLoaded;
            UserProfileItem.LayoutRoot.Height = double.NaN;
            UserProfileItem.MAinSP.Height = double.NaN;
            UserProfileItem.MyBuzzLB.Height = double.NaN;
            UserProfileItem.MyBuzzLB.LayoutRoot.Height = double.NaN;
            UserProfileItem.MyBuzzLB.BuzzLB.Height = double.NaN;
            LoadQS();
        }

        void UserProfileItem_BuzzLoaded()
        {
            ProfileBottomPB.Visibility = System.Windows.Visibility.Collapsed;
            ProfileBuzzLoading = false;
        }

        void BuzzLB_PeopleSelected(string text, Guid id)
        {
            NavigateToPeopleList(text, id);
        }

        private void NavigateToPeopleList(string text, Guid id)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.PeopleList + "?text=" + text + "&id=" + id.ToString(), UriKind.RelativeOrAbsolute));
            });
        }

        void BuzzLB_TagSelected(string name)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Tag + "?name=" + name, UriKind.RelativeOrAbsolute));
            });
        }

        void BuzzLB_NavigateToTwitter()
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.TwitterConnect, UriKind.RelativeOrAbsolute));
            });
        }

        void BuzzLB_NavigateToFacebook()
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.FacebookConnect, UriKind.RelativeOrAbsolute));
            });
        }

        

        private void NavigateToBuzzCommentPage(Guid id)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.BuzzComments + "?id=" + id.ToString(), UriKind.RelativeOrAbsolute));
            });
        }

        void BuzzLB_UserSelected(Guid id)
        {
            NavigateToUserProfile(id);
        }

        private void NavigateToUserProfile(Guid id)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.UserProfile + "?id=" + id.ToString(), UriKind.RelativeOrAbsolute));
            });
        }

        void BuzzLB_CommentOnBuzzSelected(Guid id)
        {
            NavigateToBuzzCommentPage(id);
        }


        void UserProfileItem_Error()
        {
            ShowError.Begin();
        }

        void UserProfileItem_Loading()
        {
            ShowLoading.Begin();
        }

        void UserProfileItem_Loaded()
        {
            ShowPage.Begin();
        }

        void UserProfileItem_BuzzLoading()
        {
            ProfileBottomPB.Visibility = System.Windows.Visibility.Visible;
        }

        private void NavigateToEditProfile()
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Profile, UriKind.RelativeOrAbsolute));
            });
        }


        void UserProfileItem_PeopleSelected(string name, Guid id)
        {
            NavigateToPeopleList(name, id);
        }

        void MyBuzzLB_PeopleSelected(string name, Guid id)
        {
            NavigateToPeopleList(name, id);
        }

        void UserProfileItem_ProfileImageSelected(Guid id)
        {
            if (id == Core.User.User.UserID)
            {
                NavigateToEditProfile();

            }
        }

        void UserProfileItem_BuzzLoadEnd()
        {
            ProfileBuzzLoading = false;
        }

        void ProfileSV_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
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

        void UserItem_MessageSelected(Guid id)
        {
            Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.UserText+"?id="+id,UriKind.RelativeOrAbsolute));
                });
        }

        void UserItem_Error()
        {
            ShowError.Begin();
        }

        void UserItem_Loading()
        {
            ShowLoading.Begin();
        }

        void UserItem_Loaded()
        {
            ShowPage.Begin();
        }

        private void LoadQS()
        {  

            if (NavigationContext.QueryString.ContainsKey("id"))
            {
                userId = new Guid(NavigationContext.QueryString["id"]);
                UserProfileItem.LoadUser(userId);
            }
            else
            {
                NavigateToDashboard();
            }


        }

        private void NavigateToDashboard()
        {
            throw new NotImplementedException();
        }

        public bool ProfileBuzzLoading { get; set; }
    }
}