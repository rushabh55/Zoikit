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
    public partial class Text : PhoneApplicationPage
    {

        public List<TextService.Text>  Texts { get; set; }

        public Text()
        {
            InitializeComponent();
            Loaded += Text_Loaded;
            Notifier.NotificationClicked += Notifier_NotificationClicked;
            Notifier.NotificationDisplayed += Notifier_NotificationDisplayed;
            Notifier.NotificationHidden += Notifier_NotificationHidden;
        }

        void Text_Loaded(object sender, RoutedEventArgs e)
        {
            if (!Core.Network.NetworkStatus.IsNetworkAvailable())
            {


                
                ShowTestError.Begin();
                return;
            }
            Core.PushNotification.PushNotification.RealTimeText += PushNotification_RealTimeText;

            Texts = new List<TextService.Text>();
            LoadTexts();
        }

        void PushNotification_RealTimeText(TextService.Text text)
        {
            if (Texts.Where(o => o.User.UserID==text.User.UserID).Count() > 0)
            {
                //remove
                Texts.Remove(Texts.Where(o => o.User.UserID == text.User.UserID).FirstOrDefault());
            }

            Texts.Add(text);
            Texts = Texts.OrderByDescending(o => o.DateTimeStamp).ToList();
            RefreshList();
        }

        private void LoadTexts()
        {
           
            List<int> skipList = new List<int>();

            if (Texts == null)
            {
                Texts = new List<TextService.Text>();
            }

            if (Texts.Count > 0)
            {
                skipList.AddRange(Texts.Select(o => o.User.UserID).ToList());
            }
            else
            {
                skipList = new List<int>();
            }

            Services.TextServiceClient.GetTextCompleted += TextServiceClient_GetTextCompleted;
            Services.TextServiceClient.GetTextAsync(Core.User.User.UserID, new System.Collections.ObjectModel.ObservableCollection<int>(skipList), 10, Core.User.User.ZAT);
        }

        void TextServiceClient_GetTextCompleted(object sender, TextService.GetTextCompletedEventArgs e)
        {
            
            Services.TextServiceClient.GetTextCompleted -= TextServiceClient_GetTextCompleted;
            MorePG.Visibility = System.Windows.Visibility.Collapsed;
           
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
                        RefreshList();
                    }

                    if (e.Result.Count < 10)
                    {
                        MoreBtn.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    else
                    {
                        MoreBtn.Visibility = System.Windows.Visibility.Visible;
                    }

                    if (Texts.Count > 0)
                    {
                        ShowTextx.Begin();
                    }
                    else
                    {
                        showNoText.Begin();
                    }
                }
                else
                {
                    showNoText.Begin();
                }
            }
            else
            {
                ShowTestError.Begin();
            }
        }

        private void RefreshList()
        {
            TextLB.ItemsSource = null;
            TextLB.ItemsSource = Texts;
        }

        #region Notification

        void Notifier_NotificationHidden(object sender, EventArgs e)
        {
            showLBL.Begin();
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

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigateToDashboard();
        }

        private void TextListItem_Loaded_1(object sender, RoutedEventArgs e)
        {
            TextListItem t = sender as TextListItem;
            t.TextSelected += t_TextSelected;
        }

        void t_TextSelected(TextService.Text text)
        {
            NavigateToTextPage(text.User.UserID);
        }

        private void NavigateToTextPage(int id)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.UserText+"?id="+id, UriKind.RelativeOrAbsolute));
            });
        }

        private void MoreBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MorePG.Visibility = System.Windows.Visibility.Visible;
            MoreBtn.Visibility = System.Windows.Visibility.Collapsed;
            LoadTexts();
        }

        private void ApplicationBarIconButton_Click_1(object sender, System.EventArgs e)
        {
            Services.TextServiceClient.MarkAllAsReadAsync(Core.User.User.UserID, Core.User.User.ZAT);
            Texts.ForEach(o => o.MarkAsRead = true);
            RefreshList();

        }

    }
}