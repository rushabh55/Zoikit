using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;

namespace Hangout.Client.Accounts
{
    public partial class ForgotPassword : PhoneApplicationPage
    {
        public ForgotPassword()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (!Core.Network.NetworkStatus.IsNetworkAvailable())
            {
                MessageBox.Show("We can't detect any data connection on your phone. Please try again when your phone has a stable data connection", "Data Network Not Found", MessageBoxButton.OK);
                Application.Current.Terminate();
            }
                
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

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            
            
        }

        private void ApplicationBarIconButton_Click_2(object sender, EventArgs e)
        {
            //OK.
            if (ValidateProfile())
            {
                Services.AppAuthenticationServiceClient.ResetPasswordCompleted += AppAuthenticationServiceClient_ResetPasswordCompleted;
                Services.AppAuthenticationServiceClient.ResetPasswordAsync(EmailUsernameTB.Text, AppID.ZoikitAppID, AppID.ZoikitAppToken);
                LoadingVisible();
            }

           
        }

    

        private void LoadingVisible()
        {
            ApplicationBar.IsVisible = false;
            ShowLoader.Begin();
            
        }


        void AppAuthenticationServiceClient_ResetPasswordCompleted(object sender, AppAuthenticationService.ResetPasswordCompletedEventArgs e)
        {
            Services.AppAuthenticationServiceClient.ResetPasswordCompleted -= AppAuthenticationServiceClient_ResetPasswordCompleted;

            if (e.Error != null)
            {
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                CollapseLoader();
                return;
            }

            if (e.Result)
            {
                MessageBox.Show("Password reset is successful. Please check your email for your new password and you can now login with your new password.", "Successful", MessageBoxButton.OK);
                Core.Authentication.Accounts.DeleteFacebookData();
                Core.Authentication.Accounts.DeleteFoursquareData();
                Core.Authentication.Accounts.DeleteUserData();
                Core.Authentication.Accounts.DeleteTwitterData();
                NavigateToZoikit();
            }
            else
            {
                MessageBox.Show("We're sorry. This email is not registered with us.", "Email not registered", MessageBoxButton.OK);
                CollapseLoader();
            }
            
        }

        private void NavigateToZoikit()
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.ZoikitConnect+"?view=login", UriKind.RelativeOrAbsolute));
            });
        }

        private bool ValidateProfile()
        {
            if (String.IsNullOrWhiteSpace(EmailUsernameTB.Text))
            {
                MessageBox.Show("Please enter your Email","Enter email",MessageBoxButton.OK);
                return false;
            }

            if (!IsValidEmail(EmailUsernameTB.Text))
            {
                MessageBox.Show("Please enter a valid email address","Enter a valid email",MessageBoxButton.OK);
                return false;
            }

            return true;
        }

        private void NavgigateToAccountPage()
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Accounts, UriKind.RelativeOrAbsolute));
            });
        }

        

        private bool IsValidEmail(string email)
        {
            string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            Regex ValidEmailRegex = new Regex(validEmailPattern, RegexOptions.IgnoreCase);

            bool isValid = ValidEmailRegex.IsMatch(email);

            return isValid;
        }

        private void CollapseLoader()
        {
            ApplicationBar.IsVisible = true;
            ShowPage.Begin();
            //TextLoader.Visibility = System.Windows.Visibility.Collapsed;
            //TextLoader.HideTxt.Begin();

        }

        

        private void TypeUsernameLBL_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            EmailUsernameTB.Focus();
        }

        private void EmailUsernameTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (EmailUsernameTB.Text.Count() == 0)
            {
                TypeEmail.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                TypeEmail.Visibility = System.Windows.Visibility.Collapsed;
            }
        }


    }
}