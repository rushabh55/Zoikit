using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Hangout.Client.Buzz
{
    public partial class Buzz : PhoneApplicationPage
    {
        public Buzz()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
           
            this.BuzzSV.Height = 650;

            BuzzItem.CommentOnBuzzSelected += BuzzItem_CommentOnBuzzSelected;
            BuzzItem.NavigateToFacebook += BuzzItem_NavigateToFacebook;
            BuzzItem.NavigateToTwitter += BuzzItem_NavigateToTwitter;
            BuzzItem.PeopleSelected += BuzzItem_PeopleSelected;
            BuzzItem.TagSelected += BuzzItem_TagSelected;
            BuzzItem.UserSelected += BuzzItem_UserSelected;
            

            ShowBuzzLoading.Begin();

            var id = "";

           
            if (NavigationContext.QueryString.ContainsKey("id"))
            {
                id = NavigationContext.QueryString["id"];



                Services.BuzzServiceClient.GetBuzzByIDCompleted += BuzzServiceClient_GetBuzzByIDCompleted;
                Services.BuzzServiceClient.GetBuzzByIDAsync(Core.User.User.UserID, Guid.Parse(id), Core.User.User.ZAT);
            }
            else
            {
                NaviagteToDashboard();
            }
        }



        void BuzzItem_PeopleSelected(string text, Guid id)
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

        void BuzzItem_TagSelected(string name)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Tag + "?name=" + name, UriKind.RelativeOrAbsolute));
            });
        }

        void BuzzItem_NavigateToTwitter()
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.TwitterConnect, UriKind.RelativeOrAbsolute));
            });
        }

        void BuzzItem_NavigateToFacebook()
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.FacebookConnect, UriKind.RelativeOrAbsolute));
            });
        }

        void BuzzItem_UserSelected(Guid id)
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

        void BuzzItem_CommentOnBuzzSelected(Guid id)
        {
            NavigateToBuzzCommentPage(id);
        }

        private void NavigateToBuzzCommentPage(Guid id)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.BuzzComments + "?id=" + id.ToString(), UriKind.RelativeOrAbsolute));
            });
        }

        void BuzzServiceClient_GetBuzzByIDCompleted(object sender, BuzzService.GetBuzzByIDCompletedEventArgs e)
        {

            Services.BuzzServiceClient.GetBuzzByIDCompleted -= BuzzServiceClient_GetBuzzByIDCompleted;

            if(e.Error==null)
            {
                if(e.Result!=null)
                {

                    BuzzItem.BuzzData = e.Result;
                    ShowBuzz.Begin();
                }
                else
                {
                    ShowNoBuzz.Begin();
                }
            }
            else
            {
                ShowBuzzError.Begin();
            }
        }

        private void NaviagteToDashboard()
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Dashboard, UriKind.RelativeOrAbsolute));
            });
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NaviagteToDashboard();
        }
    }
}