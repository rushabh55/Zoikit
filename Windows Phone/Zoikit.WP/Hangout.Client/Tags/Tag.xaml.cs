using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Collections.ObjectModel;

namespace Hangout.Client.Tags
{
    public partial class Tag : PhoneApplicationPage
    {
        public Tag()
        {
            InitializeComponent();
            Notifier.NotificationClicked += Notifier_NotificationClicked;
            Notifier.NotificationDisplayed += Notifier_NotificationDisplayed;
            Notifier.NotificationHidden += Notifier_NotificationHidden;
        }

        private int TagID;

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Core.Network.NetworkStatus.IsNetworkAvailable())
                {

                    ShowTagError.Begin();
                    return;
                }

                FollowersLB.UserSelected += FollowersLB_UserSelected;
                FollowersLB.TagSelected += FollowersLB_TagSelected;
                TagBuzzList.UserSelected += TagBuzzList_UserSelected;
                TagBuzzList.BuzzSelected += TagBuzzList_BuzzSelected;

                int id = GetID();
                TagID = id;
                if (id == -1)
                {
                    NavigateToDashboard();
                    return;
                }
                LoadTokenInfo();
                AttachEvents();
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void TagBuzzList_BuzzSelected(int id)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Buzz+"?id="+id, UriKind.RelativeOrAbsolute));
            });
        }

        void TagBuzzList_UserSelected(int id)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.ViewProfile + "?id=" + id, UriKind.RelativeOrAbsolute));
            });
        }

        void FollowersLB_TagSelected(int id)
        {
            if (id != GetID())
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.Tag + "?id=" + id, UriKind.RelativeOrAbsolute));
                });
            }
        }

        void FollowersLB_UserSelected(int id)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.ViewProfile + "?id=" + id, UriKind.RelativeOrAbsolute));
            });
        }

        private void AttachEvents()
        {
            FollowersLB.MoreBtnClicked += FollowersLB_MoreBtnClicked;
            TagBuzzList.MorebuttonTapped += TagBuzzList_MorebuttonTapped;
            FollowersLB.UserSelected+=FollowersLB_UserSelected;
        }

        void TagBuzzList_MorebuttonTapped(object sender, EventArgs e)
        {
            TagBuzzList.ShowProgressBar();
            Services.BuzzServiceClient.GetLocalBuzzByTokenCompleted+=BuzzServiceClient_GetLocalBuzzByTokenCompleted;
            if (TagBuzzList.Buzzes == null)
            {
                Services.BuzzServiceClient.GetLocalBuzzByTokenAsync(Core.User.User.UserID, TagID, 10, new System.Collections.ObjectModel.ObservableCollection<int>(), Core.User.User.ZAT);
            }
            else
            {
                Services.BuzzServiceClient.GetLocalBuzzByTokenAsync(Core.User.User.UserID, TagID, 10, new ObservableCollection<int>(TagBuzzList.Buzzes.Select(o => o.BuzzID)), Core.User.User.ZAT);
            }
        }

        void FollowersLB_MoreBtnClicked(object sender, EventArgs e)
        {
            FollowersLB.ShowProgressBar();
            Services.UserServiceClient.GetLocalFollowersByTokenCompleted += UserServiceClient_GetLocalFollowersByTokenCompleted;
            List<int> skipList = new List<int>();
            skipList.Add(Core.User.User.UserID);
            if (FollowersLB.Users == null)
            {
                Services.UserServiceClient.GetLocalFollowersByTokenAsync(Core.User.User.UserID, TagID, 5, new System.Collections.ObjectModel.ObservableCollection<int>(skipList), Core.User.User.ZAT);
            }
            else
            {
                skipList.AddRange(FollowersLB.Users.Select(o => o.UserID).ToList());
                Services.UserServiceClient.GetLocalFollowersByTokenAsync(Core.User.User.UserID, TagID, 5, new System.Collections.ObjectModel.ObservableCollection<int>(skipList), Core.User.User.ZAT);
            }
        }

        private void LoadTokenInfo()
        {
            TagLoader.DisplayText("Looking up for an Interest...");
            ShowTagLoader.Begin();
            Services.TokenServiceClient.GetTokenByIDCompleted += TokenServiceClient_GetTokenByIDCompleted;
            Services.TokenServiceClient.GetTokenByIDAsync(Core.User.User.UserID, TagID, Core.User.User.ZAT);
        }

        void TokenServiceClient_GetTokenByIDCompleted(object sender, TokenService.GetTokenByIDCompletedEventArgs e)
        {
            Services.TokenServiceClient.GetTokenByIDCompleted -= TokenServiceClient_GetTokenByIDCompleted;

            if (e.Error == null)
            {
                if (e.Result != null)
                {
                    Token=e.Result;
                    FollTagData(e.Result);
                    ShowTag.Begin();
                    LoadBuzz();
                    LoadPeople();
                }
                else
                {
                    ShowNoInterestPage.Begin();
                }
            }
            else
            {
                ShowTagError.Begin();
            }
        }

        private void FollTagData(TokenService.UserToken userToken)
        {
            if (!userToken.Token.Name.StartsWith("#"))
            {
                userToken.Token.Name = "#" + userToken.Token.Name;
            }

            TagName.Text = userToken.Token.Name;

            if (userToken.Following)
            {
                ShowUnfollow.Begin();
            }
            else
            {
                ShowFollow.Begin();
            }

            if (userToken.NoOfPeopleFollowing > 1)
            {
                PeopleCountLBL.Text = (userToken.NoOfPeopleFollowing + " people around you love "+userToken.Token.Name).ToUpper();
            }
            else
            {
                if (userToken.Following)
                {
                    PeopleCountLBL.Text = "You love " + userToken.Token.Name;
                }
                else
                {
                    PeopleCountLBL.Text = "Love " + userToken.Token.Name + "?";
                }
            }
        }

        private void LoadPeople()
        {
            List<int> skipList = new List<int>();
            skipList.Add(Core.User.User.UserID);
            PeopleLoader.DisplayText("Looking up for people...");
            ShowPeopleLoader.Begin();
            Services.UserServiceClient.GetLocalFollowersByTokenCompleted += UserServiceClient_GetLocalFollowersByTokenCompleted;
            Services.UserServiceClient.GetLocalFollowersByTokenAsync(Core.User.User.UserID, TagID, 5, new System.Collections.ObjectModel.ObservableCollection<int>(skipList), Core.User.User.ZAT);
        }

        void UserServiceClient_GetLocalFollowersByTokenCompleted(object sender, UserService.GetLocalFollowersByTokenCompletedEventArgs e)
        {
            Services.UserServiceClient.GetLocalFollowersByTokenCompleted -= UserServiceClient_GetLocalFollowersByTokenCompleted;

            if (e.Error == null)
            {
                if (e.Result != null)
                {

                    if (FollowersLB.Users == null)
                    {
                        FollowersLB.Users = new List<UserService.User>();
                    }
                    if (e.Result.Count != 0)
                    {
                        FollowersLB.Users.AddRange(e.Result.ToList());
                        FollowersLB.RefreshList();
                    }
                   
                    if (e.Result.Count < 5)
                    {
                        FollowersLB.HideDownPanel();
                    }
                    else
                    {
                        FollowersLB.ShowMoreButton();
                    }

                    if (FollowersLB.Users.Count > 0)
                    {
                        ShowPeoplePage.Begin();
                    }
                    else
                    {
                        ShowNoPeoplePage.Begin();
                    }
                }
                else
                {
                    ShowNoPeoplePage.Begin();
                }
            }
            else
            {
                ShowPeopleError.Begin();
            }
        }

       
        private void LoadBuzz()
        {
            BuzzLoader.DisplayText("Looking up for buzz...");
            ShowBuzzLoader.Begin();
            Services.BuzzServiceClient.GetLocalBuzzByTokenCompleted += BuzzServiceClient_GetLocalBuzzByTokenCompleted;
            Services.BuzzServiceClient.GetLocalBuzzByTokenAsync(Core.User.User.UserID, TagID, 10, new System.Collections.ObjectModel.ObservableCollection<int>(), Core.User.User.ZAT);
            
        }

        void BuzzServiceClient_GetLocalBuzzByTokenCompleted(object sender, BuzzService.GetLocalBuzzByTokenCompletedEventArgs e)
        {
            Services.BuzzServiceClient.GetLocalBuzzByTokenCompleted -= BuzzServiceClient_GetLocalBuzzByTokenCompleted;

            if (e.Error == null)
            {
                if (e.Result != null)
                {

                    if (TagBuzzList.Buzzes == null)
                    {
                        TagBuzzList.Buzzes = new List<BuzzService.Buzz>();
                    }
                    if (e.Result.Count != 0)
                    {
                        TagBuzzList.Buzzes.AddRange(e.Result.ToList());
                        TagBuzzList.RefreshList();
                    }
                   

                    if (e.Result.Count < 10)
                    {
                        TagBuzzList.HideDownPanel();
                    }
                    else
                    {
                        TagBuzzList.ShowMoreButton();
                    }

                    if (TagBuzzList.Buzzes.Count > 0)
                    {
                        ShowBuzzPage.Begin();
                    }
                    else
                    {
                        ShowNoBuzz.Begin();
                    }
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

        private void NavigateToDashboard()
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    if (NavigationService.CanGoBack)
                    {
                        var uri = NavigationService.BackStack.First().Source;

                        if (uri.ToString() == "/MainPage.xaml")
                        {
                            NavigationService.GoBack();
                        }
                        else
                        {
                            NavigationService.Navigate(new Uri(Navigation.Dashboard, UriKind.RelativeOrAbsolute));
                        }
                    }
                    else
                    {
                        NavigationService.Navigate(new Uri(Navigation.Dashboard, UriKind.RelativeOrAbsolute));
                    }

                });
            }
            catch (System.Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private int GetID()
        {
            try
            {

                int a = -1;
                int.TryParse(NavigationContext.QueryString["id"], out a);
                return a;
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                return -1;
            }
        }

        private void dashboardBtn_Click(object sender, EventArgs e)
        {
            NavigateToDashboard();
        }

        private void UnfollowCanvas_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Unfollow();
        }

        private void FollowCanvas_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Follow();
        }

        

        private void Follow()
        {
            Services.TokenServiceClient.FollowTokenCompleted += TokenServiceClient_FollowTokenCompleted;
            Services.TokenServiceClient.FollowTokenAsync(Core.User.User.UserID, GetID(), Core.User.User.ZAT);
            Token.NoOfPeopleFollowing++;
            Token.Following = true;
            ShowUnfollow.Begin();
        }

       

        void TokenServiceClient_FollowTokenCompleted(object sender, TokenService.FollowTokenCompletedEventArgs e)
        {
            Services.TokenServiceClient.FollowTokenCompleted -= TokenServiceClient_FollowTokenCompleted;
            if (e.Error == null)
            {
                //do nothing

            }
            else
            {
                Token.NoOfPeopleFollowing--;
                Token.Following = false;

                ShowFollow.Begin();
               
            }
        }

        

        private void UnfollowBtn_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Unfollow();
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

        private void Unfollow()
        {
            Services.TokenServiceClient.UnfollowTokenCompleted += TokenServiceClient_UnfollowTokenCompleted;
            Services.TokenServiceClient.UnfollowTokenAsync(Core.User.User.UserID, GetID(), Core.User.User.ZAT);
            Token.NoOfPeopleFollowing--;
            Token.Following = false;
           
            ShowFollow.Begin();
            
        }

        void TokenServiceClient_UnfollowTokenCompleted(object sender, TokenService.UnfollowTokenCompletedEventArgs e)
        {
            Services.TokenServiceClient.FollowTokenCompleted -= TokenServiceClient_FollowTokenCompleted;
            if (e.Error == null)
            {
                //do nothing

            }
            else
            {
                Token.NoOfPeopleFollowing++;
                Token.Following = true;
                ShowUnfollow.Begin();
                
            }
        }

        public TokenService.UserToken Token { get; set; }
    }
}