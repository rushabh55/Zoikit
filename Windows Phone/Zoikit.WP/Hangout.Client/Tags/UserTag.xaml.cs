using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Hangout.Client.Tags
{
    public partial class UserTag : PhoneApplicationPage
    {
        public UserTag()
        {
            InitializeComponent();
            Notifier.NotificationClicked += Notifier_NotificationClicked;
            Notifier.NotificationDisplayed += Notifier_NotificationDisplayed;
            Notifier.NotificationHidden += Notifier_NotificationHidden;
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

        void Notifier_NotificationClicked(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Notification, UriKind.RelativeOrAbsolute));
            });

            return;
        }


        #endregion

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

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {

            if (!Core.Network.NetworkStatus.IsNetworkAvailable())
            {

                ShowError.Begin();
                return;
            }

            TagList.UnFollowToken += TagList_UnFollowToken;
            TagList.TokenSelected += TagList_TokenSelected;
            textloader.DisplayText("We're looking for things you love doing.");
            //get and save location to the server
            Settings.Settings.InterestFill = true; //set this guy to true to prevent redirects from dashboard. :)

            if (!PageLoaded)
            {
                PageLoaded = true;
                Core.Location.CurrentLocation.GetLocationCompleted += CurrentLocation_GetLocationCompleted;
                Core.Location.CurrentLocation.GetCurrentLocation();
            }


        }

        void TagList_TokenSelected(int id)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Tag+"?id="+id, UriKind.RelativeOrAbsolute));
            });

            return;
        }

        void TagList_UnFollowToken(TokenService.UserToken token)
        {
            if (TagList.Tokens.Contains(token))
            {
                TagList.Tokens.Remove(token);
                TagList.RefreshList();

                if (TagList.Tokens.Count == 0)
                {
                    ShowNoItems.Begin();
                }
            }
        }

        void CurrentLocation_GetLocationCompleted()
        {
            //Location updated to the server

            // now get the tokens. 
            Services.TokenServiceClient.GetTokensFollowedCompleted += TokenServiceClient_GetTokensFollowedCompleted;
            Services.TokenServiceClient.GetTokensFollowedAsync(Core.User.User.UserID, 500, new System.Collections.ObjectModel.ObservableCollection<int>(), Core.User.User.ZAT);

        }

        void TokenServiceClient_GetTokensFollowedCompleted(object sender, TokenService.GetTokensFollowedCompletedEventArgs e)
        {
            Services.TokenServiceClient.GetTokensFollowedCompleted -= TokenServiceClient_GetTokensFollowedCompleted;

            if (e.Error == null)
            {
                if (e.Result.Count == 0)
                {
                    if (TagList.Tokens != null)
                    {
                        if (TagList.Tokens.Count == 0)
                        {
                            ShowNoItems.Begin();
                        }
                    }
                    else
                    {
                        ShowNoItems.Begin();
                    }

                    
                }
                else
                {
                    if (TagList.Tokens == null)
                    {
                        TagList.Tokens = new List<TokenService.UserToken>();
                    }

                    TagList.Tokens.AddRange(e.Result.ToList());
                    TagList.RefreshList();
                    HideNoItems.Begin();
                }
            }
            else
            {
                ShowError.Begin();
            }

            TagList.MorePG.Visibility = System.Windows.Visibility.Collapsed;


        }

        private void InterestBtn_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(InterestTB.Text))
            {
                MessageBox.Show("Please enter what you like.We'd love to know more about you", "Enter your Interest", MessageBoxButton.OK);
            }
            else
            {
                HideNoItems.Begin();
                TagList.MorePG.Visibility = System.Windows.Visibility.Visible;
                Services.TokenServiceClient.FollowTokenByNameCompleted += TokenServiceClient_FollowTokenByNameCompleted;
                Services.TokenServiceClient.FollowTokenByNameAsync(Core.User.User.UserID, InterestTB.Text.Trim(), Core.User.User.ZAT);
                InterestTB.Text = "";
                
            }
        }

        void TokenServiceClient_FollowTokenByNameCompleted(object sender, TokenService.FollowTokenByNameCompletedEventArgs e)
        {
            Services.TokenServiceClient.FollowTokenByNameCompleted -= TokenServiceClient_FollowTokenByNameCompleted;

            if (e.Error == null)
            {
                LoadWithSkipList();
            }
            else
            {
                ShowError.Begin();
            }
        }

        private void LoadWithSkipList()
        {
            List<int> skipList=new List<int>();

            if(TagList.Tokens!=null)
            {
                skipList.AddRange(TagList.Tokens.Select(o=>o.Token.Id).ToList());
            }

            // now get the tokens. 
            Services.TokenServiceClient.GetTokensFollowedCompleted += TokenServiceClient_GetTokensFollowedCompleted;
            Services.TokenServiceClient.GetTokensFollowedAsync(Core.User.User.UserID, 10, new System.Collections.ObjectModel.ObservableCollection<int>(skipList), Core.User.User.ZAT);
        }





        public bool PageLoaded { get; set; }
    }
}