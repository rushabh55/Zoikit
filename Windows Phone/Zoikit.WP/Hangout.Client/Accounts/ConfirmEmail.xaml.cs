using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Hangout.Client.Accounts
{
    public partial class ConfirmEmail : PhoneApplicationPage
    {
        public ConfirmEmail()
        {
            InitializeComponent();
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            NavigateToDashboard();

        }

        private void ApplicationBarIconButton_Click_2(object sender, EventArgs e)
        {
            NavigateToAccountPage();
        }

        private void NavigateToAccountPage()
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Accounts, UriKind.RelativeOrAbsolute));
            });
        }

        private void NavigateToDashboard()
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Dashboard, UriKind.RelativeOrAbsolute));
            });
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ValidatePage())
            {
                Services.AppAuthenticationServiceClient.ConfirmEmailCompleted += AppAuthenticationServiceClient_ConfirmEmailCompleted;
                Services.AppAuthenticationServiceClient.ConfirmEmailAsync(Core.User.User.UserID, ConfirmationCodeTB.Text.Trim(), Core.User.User.ZAT);
            }
        }

        void AppAuthenticationServiceClient_ConfirmEmailCompleted(object sender, AppAuthenticationService.ConfirmEmailCompletedEventArgs e)
        {
            Services.AppAuthenticationServiceClient.ConfirmEmailCompleted -= AppAuthenticationServiceClient_ConfirmEmailCompleted;
            if (e.Error != null || e.Result == null)
            {
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                return;
            }

            if (e.Result)
            {
                MessageBox.Show("Your email now confirmed. Thank you. ", "Email confirmed", MessageBoxButton.OK);
            }
            else
            {
                MessageBox.Show("We're sorry. The confirmation code you entered is incorrect. Please try again. ", "Wrong code.", MessageBoxButton.OK);
            }
        }

        private bool ValidatePage()
        {
            if (String.IsNullOrWhiteSpace(ConfirmationCodeTB.Text))
            {
                MessageBox.Show("Please enter confirmation code that's sent to your email.");
                return false;
            }

            return true;
        }
    }
}