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
using Microsoft.Phone.Shell;
using System.Net.NetworkInformation;

namespace Hangout.Client.Buzz
{
    public partial class Buzz : PhoneApplicationPage
    {
        public Buzz()
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

        private void PostBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(CommentTB.Text))
            {
                int lastcomment=0;
                if(CommentLB.HangoutComment.Count>0)
                {
                    lastcomment=CommentLB.HangoutComment.OrderBy(o=>o.DatePosted).Last().CommentID;
                }
                CommentLB.CommentPG.Visibility = System.Windows.Visibility.Visible;
                Services.BuzzServiceClient.AddBuzzCommentCompleted += BuzzServiceClient_AddBuzzCommentCompleted;
                Services.BuzzServiceClient.AddBuzzCommentAsync(Core.User.User.UserID,BuzzID,CommentTB.Text,lastcomment ,Core.User.User.ZAT);
                CommentTB.Text = "";
            }
        }

        void BuzzServiceClient_AddBuzzCommentCompleted(object sender, BuzzService.AddBuzzCommentCompletedEventArgs e)
        {
            Services.BuzzServiceClient.AddBuzzCommentCompleted -= BuzzServiceClient_AddBuzzCommentCompleted;
            CommentLB.CommentPG.Visibility = System.Windows.Visibility.Collapsed;
            if (e.Error != null)
            {
                ShowCommentsError.Begin();
            }
        }

        private void CommentTB_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (CommentTB.Text.StartsWith("Type your comment"))
            {
                CommentTB.Text = "";
            }
        }

        

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
            try
            {
                
                if (!Core.Network.NetworkStatus.IsNetworkAvailable())
                {
                   
                    ShowBuzzError.Begin();
                    return;
                }
                Core.PushNotification.PushNotification.BuzzCommentNotificationReceived += PushNotification_BuzzCommentNotificationReceived;
                
                this.DataContext = this;
                int id = GetID();
                BuzzID = id;
                if (id == -1)
                {
                    NavigateToDashboard();
                    return;
                }

                TagList.Tokens = new List<TokenService.UserToken>();
                CommentLB.HangoutComment = new List<BuzzService.BuzzComment>();
                UserList.Users = new List<UserService.User>();

                BuzzLoader.DisplayText("Looking up for the Buzz...");
                ShowBuzzLoader.Begin();
                LoadBuzz();

                PoepleLoader.DisplayText("Looking up for people...");
                ShowPeopleLoader.Begin();
                LoadBuzzPeople();

                CommentsLoader.DisplayText("Loading comments...");
                ShowCommentsLoader.Begin();
                LoadBuzzComments();

                TagLoader.DisplayText("Looking up for #Interests");
                ShowTagLoade.Begin();
                LoadTags();

                AttachEvents();
            }
            catch (System.Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void PushNotification_BuzzCommentNotificationReceived(int buzzId)
        {
            if (buzzId == GetID())
            {
                CommentLB.CommentPG.Visibility = System.Windows.Visibility.Visible;
                LoadBuzzComments();
                CommentSV.UpdateLayout();
                CommentLB.UpdateLayout();
                CommentSV.ScrollToVerticalOffset(CommentLB.ActualHeight);
            }
        }

        private void LoadTags()
        {
            Services.TokenServiceClient.GetTokensInBuzzCompleted += TokenServiceClient_GetTokensInBuzzCompleted;
            Services.TokenServiceClient.GetTokensInBuzzAsync(Core.User.User.UserID, BuzzID, Core.User.User.ZAT);
        }

        void TokenServiceClient_GetTokensInBuzzCompleted(object sender, TokenService.GetTokensInBuzzCompletedEventArgs e)
        {
            Services.TokenServiceClient.GetTokensInBuzzCompleted -= TokenServiceClient_GetTokensInBuzzCompleted;

            if (e.Error != null)
            {
                ShowTagError.Begin();
            }
            else
            {
                if (TagList.Tokens == null)
                {
                    TagList.Tokens = new List<TokenService.UserToken>();
                }

                
                TagList.Tokens.AddRange(e.Result);

                if (TagList.Tokens.Count > 0)
                {

                    TagList.RefreshList();
                    ShowTag.Begin();
                }
                else
                {
                    ShowNoTag.Begin();
                }
            }
        }

        private void LoadBuzzComments()
        {
            CommentLB.GetPreviousButton.Visibility = System.Windows.Visibility.Collapsed;
            CommentLB.PreviousPG.Visibility = System.Windows.Visibility.Visible;
            List<int> skiplist=new List<int>();
            if (CommentLB.HangoutComment != null)
            {
                if (CommentLB.HangoutComment.Count > 0)
                {
                    skiplist = CommentLB.HangoutComment.Select(o => o.CommentID).ToList();
                }
            }

            Services.BuzzServiceClient.GetBuzzCommentsCompleted += BuzzServiceClient_GetBuzzCommentsCompleted;
            Services.BuzzServiceClient.GetBuzzCommentsAsync(Core.User.User.UserID, BuzzID, 10,  new System.Collections.ObjectModel.ObservableCollection<int>(skiplist), Core.User.User.ZAT);
        }

        void BuzzServiceClient_GetBuzzCommentsCompleted(object sender, BuzzService.GetBuzzCommentsCompletedEventArgs e)
        {
            Services.BuzzServiceClient.GetBuzzCommentsCompleted -= BuzzServiceClient_GetBuzzCommentsCompleted;
            CommentLB.CommentPG.Visibility = System.Windows.Visibility.Collapsed;
            CommentLB.PreviousPG.Visibility = System.Windows.Visibility.Collapsed;
            if (e.Error != null)
            {
                ShowCommentsError.Begin();
            }
            else
            {
                if (CommentLB.HangoutComment == null)
                {
                    CommentLB.HangoutComment = new List<BuzzService.BuzzComment>();
                }
                CommentLB.HangoutComment.AddRange(e.Result);
                CommentLB.HangoutComment = CommentLB.HangoutComment.OrderBy(o => o.DatePosted).ToList();
                CommentLB.RefreshList();

                if (CommentLB.HangoutComment.Count > 0)
                {
                    ShowComments.Begin();

                }
                else
                {
                    ShowNoComments.Begin();
                }


                if (e.Result.Count ==10)
                {
                    CommentLB.GetPreviousButton.Visibility = System.Windows.Visibility.Visible;
                }
                CommentSV.UpdateLayout();
                CommentLB.UpdateLayout();
                CommentSV.ScrollToVerticalOffset(CommentLB.ActualHeight);

            }
        }

        private void LoadBuzzPeople()
        {
            UserList.MoreBtn.Visibility = System.Windows.Visibility.Collapsed;
            UserList.MorePG.Visibility = System.Windows.Visibility.Visible;
            List<int> skiplist = new List<int>();
            if (UserList.Users != null)
            {
                if (UserList.Users.Count > 0)
                {
                    skiplist = UserList.Users.Select(o => o.UserID).ToList();
                }
            }

            skiplist.Add(Core.User.User.UserID);

            Services.UserServiceClient.GetPeopleWhoFollowBuzzCompleted += UserServiceClient_GetPeopleWhoFollowBuzzCompleted;
            Services.UserServiceClient.GetPeopleWhoFollowBuzzAsync(Core.User.User.UserID, BuzzID, 5, new System.Collections.ObjectModel.ObservableCollection<int>(skiplist), Core.User.User.ZAT);
        }

        void UserServiceClient_GetPeopleWhoFollowBuzzCompleted(object sender, UserService.GetPeopleWhoFollowBuzzCompletedEventArgs e)
        {
            Services.UserServiceClient.GetPeopleWhoFollowBuzzCompleted -= UserServiceClient_GetPeopleWhoFollowBuzzCompleted;

            UserList.MorePG.Visibility = System.Windows.Visibility.Collapsed;

            if (e.Error != null)
            {
                ShowPeopleError.Begin();
            }
            else
            {
                if (e.Result == null)
                {
                    if (UserList.Users == null)
                    {
                        UserList.Users = new List<UserService.User>();
                    }
                    if (UserList.Users.Count > 0)
                    {
                        ShowPeople.Begin();
                    }
                    else
                    {
                        ShowNoPeople.Begin();
                    }
                }
                else
                {
                    if (UserList.Users == null)
                    {
                        UserList.Users = new List<UserService.User>();
                    }
                    UserList.Users.AddRange(e.Result.ToList());

                    if (UserList.Users.Count > 0)
                    {
                        UserList.RefreshList();
                        ShowPeople.Begin();
                    }
                    else
                    {
                        ShowNoPeople.Begin();
                    }

                    if (e.Result.Count >= 5)
                    {
                        UserList.MoreBtn.Visibility = System.Windows.Visibility.Visible;
                    }
                }

               
            }
        }

        private void LoadBuzz()
        {
            Services.BuzzServiceClient.GetBuzzByIDCompleted += BuzzServiceClient_GetBuzzByIDCompleted;
            Services.BuzzServiceClient.GetBuzzByIDAsync(Core.User.User.UserID, BuzzID, Core.User.User.ZAT);
        }

        void BuzzServiceClient_GetBuzzByIDCompleted(object sender, BuzzService.GetBuzzByIDCompletedEventArgs e)
        {
            Services.BuzzServiceClient.GetBuzzByIDCompleted -= BuzzServiceClient_GetBuzzByIDCompleted;

            if (e.Error != null)
            {
                ShowBuzzError.Begin();
            }
            else
            {

                if (e.Result == null)
                {
                    ShowNoBuzz.Begin();
                }
                else
                {
                    BuzzItem.Buzz = e.Result;

                    BuzzItem.UpdateLayout();

                    MainPivot.Height = 750 - (75 + BuzzItem.ActualHeight + 10);
                    CommentSV.Height = 750 - (260 + BuzzItem.ActualHeight + 10);
                    CommentGrid.Height = 750 - (260 + BuzzItem.ActualHeight + 10);
                    ShowBuzz.Begin();
                }
            }
        }

        

        void btn_Click(object sender, EventArgs e)
        {
            //Navigate To Buzz Edit Page. :)
        }

        private void AttachEvents()
        {
            UserList.MoreBtnClicked += UserList_MoreBtnClicked;
            CommentLB.PreviousBtnClicked += CommentLB_PreviousBtnClicked;
            UserList.UserSelected += UserList_UserSelected;
            TagList.TokenSelected += TagList_TokenSelected;
            CommentLB.UserSelected += CommentLB_UserSelected;
        }

        void CommentLB_UserSelected(int id)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.ViewProfile + "?id=" + id, UriKind.RelativeOrAbsolute));
            });
        }

        void TagList_TokenSelected(int id)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Tag + "?id=" + id, UriKind.RelativeOrAbsolute));
            });
        }

        void UserList_UserSelected(int id)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.ViewProfile + "?id=" + id, UriKind.RelativeOrAbsolute));
            });
        }

        void CommentLB_PreviousBtnClicked(object sender, EventArgs e)
        {
            CommentLB.GetPreviousButton.Visibility = System.Windows.Visibility.Collapsed;
            CommentLB.PreviousPG.Visibility = System.Windows.Visibility.Visible;
            LoadBuzzComments();
        }

        void UserList_MoreBtnClicked(object sender, EventArgs e)
        {
            UserList.MoreBtn.Visibility = System.Windows.Visibility.Collapsed;
            UserList.MorePG.Visibility = System.Windows.Visibility.Visible;
            LoadBuzzPeople();
        }

        private int GetID()
        {
            try
            {

                int a = -1;
                int.TryParse(NavigationContext.QueryString["id"], out a);
                return a;
            }
            catch (System.Exception ex)
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


        public int BuzzID { get; set; }
    }
}