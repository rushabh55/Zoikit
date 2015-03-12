using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework;

namespace Hangout.Client.Accounts
{
    public partial class ChangePassword : PhoneApplicationPage
    {
        public ChangePassword()
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

        private void NavigateToForgotPassword()
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.ForgotPassword, UriKind.RelativeOrAbsolute));
            });
        }

        private void NavgigateToAccountPage()
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

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            //Dashboard!
            NavigateToDashboard();

        }

        private void ApplicationBarIconButton_Click_2(object sender, EventArgs e)
        {
            SavePassword();
        }

        private void SavePassword()
        {
            //SAVE
            if (ValidateProfile())
            {
                LoadingVisible();
                Services.AppAuthenticationServiceClient.ChangePasswordCompleted += AppAuthenticationServiceClient_ChangePasswordCompleted;
                Services.AppAuthenticationServiceClient.ChangePasswordAsync(Core.User.User.UserID, CurrentPassTB.Password.Trim(), NewPassTB.Password.Trim());
            }
        }

        void AppAuthenticationServiceClient_ChangePasswordCompleted(object sender, AppAuthenticationService.ChangePasswordCompletedEventArgs e)
        {
            Services.AppAuthenticationServiceClient.ChangePasswordCompleted -= AppAuthenticationServiceClient_ChangePasswordCompleted;
            if (e.Error != null)
            {
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                CollapseLoader();
                return;
            }

           

            if (e.Result)
            {
                MessageBox.Show("Your password change was successful", "Password Change", MessageBoxButton.OK);
                NavigateToDashboard();
            }
            else
            {
                MessageBox.Show("Your current password is incorrect, Please try again. ", "Password Change Unsuccessful", MessageBoxButton.OK);
                CollapseLoader();
                CurrentPassTB.Password= "";
                NewPassTB.Password = "";
                ConfirmPassTB.Password= "";

                return;
            }

            
        }

        private bool ValidateProfile()
        {
            if (String.IsNullOrWhiteSpace(CurrentPassTB.Password))
            {
                MessageBox.Show("Please enter your current password","Enter your current password.", MessageBoxButton.OK);
                return false;
            }

            if (String.IsNullOrWhiteSpace(NewPassTB.Password))
            {
                MessageBox.Show("Please enter your new password","Enter your new password", MessageBoxButton.OK);
                return false;
            }

            if (String.IsNullOrWhiteSpace(ConfirmPassTB.Password))
            {
                MessageBox.Show("Please confirm your password password", "Confirm your password", MessageBoxButton.OK);
                return false;
            }

            if (NewPassTB.Password.Trim() != ConfirmPassTB.Password.Trim())
            {
                MessageBox.Show("Password and confirmation password do not match.", "Passwords don't match", MessageBoxButton.OK);
                return false;
            }

            return true;
        }

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (!Core.Network.NetworkStatus.IsNetworkAvailable())
            {
                MessageBox.Show("We can't detect any data connection on your phone. Please try again when your phone has a stable data connection", "Data Network Not Found", MessageBoxButton.OK);
                new Game().Exit();
            }
                

            if (!Core.Authentication.Accounts.LoggedInToZoikit())
            {
                MessageBox.Show("You're nto logged in to Zoik it! account. Please login", "Not logged in", MessageBoxButton.OK);
                NavigateToDashboard();
                return;
            }
        }

        private void LoadingVisible()
        {
            ApplicationBar.IsVisible = false;
            HidePage.Begin();
            
            this.TextLoader.Visibility = System.Windows.Visibility.Visible;
            this.TextLoader.DisplayText("We're changing your password");
        }

        private void CollapseLoader()
        {
            ApplicationBar.IsVisible = true;
            ShowPage.Begin();
            TextLoader.Visibility = System.Windows.Visibility.Collapsed;
            TextLoader.HideTxt.Begin();

        }

        

        private void Button_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
        	NavigateToForgotPassword();
        }

        private void ForgotPasswordLink_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }

        private void ChangePasswordButton_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            SavePassword();
        }

        

        


    }
}