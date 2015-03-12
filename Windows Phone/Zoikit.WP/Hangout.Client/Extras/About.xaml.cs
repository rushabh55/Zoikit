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
using Microsoft.Phone.Tasks;
using System.Reflection;

namespace Hangout.Client.Extras
{
    public partial class About : PhoneApplicationPage
    {
        public About()
        {
            InitializeComponent();
            Notifier.NotificationClicked += Notifier_NotificationClicked;
            Notifier.NotificationDisplayed += Notifier_NotificationDisplayed;
            Notifier.NotificationHidden += Notifier_NotificationHidden;
            var nameHelper = new AssemblyName(Assembly.GetExecutingAssembly().FullName);
            VersionLBL.Text = nameHelper.Version.ToString();
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


        private void ApplicationBarIconButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                NavigateToDashboard();
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText,ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private void RateBtn_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                // TODO: Add event handler implementation here.
                MarketplaceReviewTask review = new MarketplaceReviewTask();
                review.Show();
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText,ErrorText.GenericErrorCaption, MessageBoxButton.OK);
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

        private void ApplicationBarIconButton_Click_1(object sender, System.EventArgs e)
        {
            Microsoft.Phone.Tasks.EmailComposeTask task = new EmailComposeTask();
            task.To = @"contactus@guidsoftware.com";
            task.Subject = @"Zoik It! Feedback";
            task.Show();
        }
    }
}