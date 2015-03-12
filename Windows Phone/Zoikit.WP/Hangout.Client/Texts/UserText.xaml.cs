using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Hangout.Client.Texts
{
    public partial class UserText : PhoneApplicationPage
    {


        public List<TextService.Text> Texts { get; set; }
       
        public UserText()
        {
            InitializeComponent();
            Loaded += UserText_Loaded;
            Notifier.NotificationClicked += Notifier_NotificationClicked;
            Notifier.NotificationDisplayed += Notifier_NotificationDisplayed;
            Notifier.NotificationHidden += Notifier_NotificationHidden;
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

        void UserText_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                try
                {

                    Texts = new List<TextService.Text>();

                    if (!Core.Network.NetworkStatus.IsNetworkAvailable())
                    {

                        ShowTextError.Begin();
                        return;
                    }


                    Core.PushNotification.PushNotification.RealTimeText += PushNotification_RealTimeText;

                    int id = GetID();
                    ToUserID = id;
                    if (id == -1)
                    {
                        NavigateToDashboard();
                        return;
                    }
                    LoadUserText();
                    
                }
                catch (Exception ex)
                {
                    Core.Exceptions.ExceptionReporting.Report(ex);
                    MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                }
               
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void PushNotification_RealTimeText(TextService.Text text)
        {
            if (text.User.UserID == ToUserID)
            {
                Texts.Add(text);
                Texts = Texts.OrderBy(o => o.DateTimeStamp).ToList();
                RefreshList();
                TextLB.UpdateLayout();
                MainSV.UpdateLayout();
                MainSV.ScrollToVerticalOffset(TextLB.ActualHeight);
                
            }
        }

        private void LoadUserText()
        {
           PreviousPG.Visibility = System.Windows.Visibility.Visible;

            if (Texts == null)
            {
                Texts = new List<TextService.Text>();

            }

            List<int> SkipList = Texts.Select(o => o.TextId).ToList();
            Services.TextServiceClient.GetUserTextCompleted += TextServiceClient_GetUserTextCompleted;       
            Services.TextServiceClient.GetUserTextAsync(Core.User.User.UserID, ToUserID, new System.Collections.ObjectModel.ObservableCollection<int>(SkipList), 10, Core.User.User.ZAT);

        }

        void TextServiceClient_GetUserTextCompleted(object sender, TextService.GetUserTextCompletedEventArgs e)
        {
            Services.TextServiceClient.MarkAsReadAsync(ToUserID, Core.User.User.UserID, Core.User.User.ZAT);
            PreviousPG.Visibility = System.Windows.Visibility.Collapsed;
            Services.TextServiceClient.GetUserTextCompleted -= TextServiceClient_GetUserTextCompleted;       
            if (e.Error == null)
            {
                if (e.Result != null)
                {
                    NAMELBL.Text = e.Result.User.Name;
                    if (Texts == null)
                    {
                        Texts = new List<TextService.Text>();
                    }
                    if (e.Result.Texts != null)
                    {
                        if (e.Result.Texts.Count != 0)
                        {
                            Texts.AddRange(e.Result.Texts);
                            Texts = Texts.OrderBy(o => o.DateTimeStamp).ToList();
                            RefreshList();
                            if (!PrevClicked)
                            {
                                
                                        TextLB.UpdateLayout();
                                        MainSV.UpdateLayout();
                                        MainSV.ScrollToVerticalOffset(TextLB.ActualHeight);
                                   
                            }
                        }

                        if (e.Result.Texts.Count < 10)
                        {
                            PrevBtn.Visibility = System.Windows.Visibility.Collapsed;
                        }
                        else
                        {
                            PrevBtn.Visibility = System.Windows.Visibility.Visible;
                        }
                    }

                    if (Texts.Count > 0)
                    {
                        ShowTextPage.Begin();
                    }
                    else
                    {
                        ShowNoText.Begin();
                    }
                }
                else
                {
                    ShowNoText.Begin();
                }
            }
            else
            {
                ShowTextError.Begin();
            }
        }

       

        private void RefreshList()
        {
            TextLB.ItemsSource = null;
            TextLB.ItemsSource = Texts;
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


        private void dashboardBtn_Click(object sender, EventArgs e)
        {
            NavigateToDashboard();
        }

        #region Notification

        void Notifier_NotificationHidden(object sender, EventArgs e)
        {
            ShowLBL.Begin();
        }

        void Notifier_NotificationDisplayed(object sender, EventArgs e)
        {
            HideLBL.Begin();
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

        private void PrevBtn_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            PrevClicked = true;
            LoadUserText();
        }

        public int ToUserID { get; set; }

        private void PostBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(CommentTB.Text))
            {
                
                MorePG.Visibility = System.Windows.Visibility.Visible;
                Services.TextServiceClient.SendTextCompleted+=TextServiceClient_SendTextCompleted;
                Services.TextServiceClient.SendTextAsync(Core.User.User.UserID,ToUserID,CommentTB.Text.Trim(),Core.User.User.ZAT);
                CommentTB.Text = "";
                TextLB.UpdateLayout();
                MainSV.UpdateLayout();
                MainSV.ScrollToVerticalOffset(TextLB.ActualHeight);
            }
        }

        void TextServiceClient_SendTextCompleted(object sender, TextService.SendTextCompletedEventArgs e)
        {
            Services.TextServiceClient.SendTextCompleted -= TextServiceClient_SendTextCompleted;
            if(e.Error==null)
            {
                if(e.Result==TextService.TextSentStatus.Error)
                {
                    MorePG.Visibility = System.Windows.Visibility.Collapsed;
                    MessageBox.Show("Can't send your text. Problem with your network","Turbulance up there",MessageBoxButton.OK);
                }
                else
                {

                    int lasttxt = 0;

                    if (Texts.Count > 0)
                    {
                        lasttxt = Texts.OrderBy(o => o.DateTimeStamp).Select(o=>o.TextId).Last();
                    }
 	                Services.TextServiceClient.GetTextsAfterCompleted += TextServiceClient_GetTextsAfterCompleted;
                    Services.TextServiceClient.GetTextsAfterAsync(Core.User.User.UserID, ToUserID, lasttxt, Core.User.User.ZAT);
                }
            }
            else
            {
                 MorePG.Visibility = System.Windows.Visibility.Collapsed;
                MessageBox.Show("Can't send your text. Problem with your network","Turbulance up there",MessageBoxButton.OK);
            }
        }

        void TextServiceClient_GetTextsAfterCompleted(object sender, TextService.GetTextsAfterCompletedEventArgs e)
        {
            Services.TextServiceClient.MarkAsReadAsync(ToUserID, Core.User.User.UserID, Core.User.User.ZAT);
            MorePG.Visibility = System.Windows.Visibility.Collapsed;
            Services.TextServiceClient.GetTextsAfterCompleted -= TextServiceClient_GetTextsAfterCompleted;
            if (e.Error == null)
            {
                if (e.Result != null)
                {

                    if (Texts == null)
                    {
                        Texts = new List<TextService.Text>();
                    }
                    if (e.Result.Count != 0)
                    {
                        Texts.AddRange(e.Result);
                        Texts = Texts.OrderBy(o => o.DateTimeStamp).ToList();
                        RefreshList();
                        TextLB.UpdateLayout();
                        MainSV.UpdateLayout();
                        MainSV.ScrollToVerticalOffset(TextLB.ActualHeight);
                       
                    }

                    if (e.Result.Count < 10)
                    {
                        PrevBtn.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    else
                    {
                        PrevBtn.Visibility = System.Windows.Visibility.Visible;
                    }

                    if (Texts.Count > 0)
                    {
                        ShowTextPage.Begin();
                    }
                    else
                    {
                        ShowNoText.Begin();
                    }
                }
                else
                {
                    ShowNoText.Begin();
                }
            }
            else
            {
                ShowTextError.Begin();
            }
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigateToDashboard();
        }

        private void CommentTB_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (CommentTB.Text.StartsWith("Type your text here"))
            {
                CommentTB.Text = "";
            }
        }


        public bool PrevClicked { get; set; }
    }
}