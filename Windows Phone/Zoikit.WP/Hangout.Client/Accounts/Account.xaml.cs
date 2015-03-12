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
using Microsoft.Xna.Framework;

namespace Hangout.Client.Accounts
{
    public partial class Account : PhoneApplicationPage
    {

        public Account()
        {
            InitializeComponent();
            Notifier.NotificationClicked += Notifier_NotificationClicked;
            Notifier.NotificationDisplayed += Notifier_NotificationDisplayed;
            Notifier.NotificationHidden += Notifier_NotificationHidden;
        }

        private void FacebookToggleSwitch_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                //navigate to Facebook connect page
                NavigationService.Navigate(new Uri(Navigation.FacebookConnect, UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }

        }

        private void FacebookToggleSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {

                if (MessageBoxResult.OK == MessageBox.Show("Do you want to deactivate connection of facebook with Zoik it?", "Deactivate Facebook", MessageBoxButton.OKCancel))
                {
                    //delete all facebook data!

                    DeleteFacebookData();
                    CheckAllAccounts();
                    FacebookLoggedInPanel.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    FacebookToggleSwitch.Checked -= new EventHandler<RoutedEventArgs>(FacebookToggleSwitch_Checked);
                    FacebookToggleSwitch.IsChecked = true;
                    FacebookToggleSwitch.Checked += new EventHandler<RoutedEventArgs>(FacebookToggleSwitch_Checked);
                }
            }
            catch(Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText,ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private void DeleteFacebookData()
        {
            Core.Authentication.Accounts.DeleteFacebookData();
        }

        private void DeleteAllData()
        {
            Core.Authentication.Accounts.DeleteUserData();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Core.Network.NetworkStatus.IsNetworkAvailable())
                {
                    MessageBox.Show("We can't detect any data connection on your phone. Please try again when your phone has a stable data connection", "Data Network Not Found", MessageBoxButton.OK);
                    new Game().Exit();
                }
                
               
                if (!LoggedInToFacebook())
                {
                    FacebookToggleSwitch.Unchecked -= new EventHandler<RoutedEventArgs>(FacebookToggleSwitch_Unchecked);
                    FacebookToggleSwitch.IsChecked = false;
                    FacebookToggleSwitch.Unchecked += new EventHandler<RoutedEventArgs>(FacebookToggleSwitch_Unchecked);
                    FacebookLoggedInPanel.Visibility = System.Windows.Visibility.Collapsed;

                }
                else
                {

                    //dispaly the content panel and keep the switch as checked.
                    FacebookToggleSwitch.Checked -=FacebookToggleSwitch_Checked;
                    FacebookToggleSwitch.IsChecked = true;
                    FacebookToggleSwitch.Checked +=new EventHandler<RoutedEventArgs>(FacebookToggleSwitch_Checked);
                    FacebookLoggedInPanel.Visibility = System.Windows.Visibility.Visible;
                    FacebookName.Text = Settings.Settings.FacebookData.FirstName + " " + Settings.Settings.FacebookData.LastName;
                }


                if (!LoggedInToTwitter())
                {
                    TwitterToggleSwitch.Unchecked -= new EventHandler<RoutedEventArgs>(TwitterToggleSwitch_Unchecked);
                    TwitterToggleSwitch.IsChecked = false;
                    TwitterToggleSwitch.Unchecked += new EventHandler<RoutedEventArgs>(TwitterToggleSwitch_Unchecked);
                    TwitterLoggedInPanel.Visibility = System.Windows.Visibility.Collapsed;

                }
                else
                {

                    //dispaly the content panel and keep the switch as checked.
                    TwitterToggleSwitch.Checked -= TwitterToggleSwitch_Checked;
                    TwitterToggleSwitch.IsChecked = true;
                    TwitterToggleSwitch.Checked += new EventHandler<RoutedEventArgs>(TwitterToggleSwitch_Checked);
                    TwitterLoggedInPanel.Visibility = System.Windows.Visibility.Visible;
                    TwitterName.Text = Settings.Settings.TwitterData.ScreenName;
                }


                //check foursquare account
                if (!LoggedInToFoursquare())
                {
                    FoursquareToggleSwitch.Unchecked -= FoursquareToggleSwitch_Unchecked;
                    FoursquareToggleSwitch.IsChecked = false;
                    FoursquareToggleSwitch.Unchecked += new EventHandler<RoutedEventArgs>(FoursquareToggleSwitch_Unchecked);
                    FoursquareLoggedInPanel.Visibility = System.Windows.Visibility.Collapsed;

                }
                else
                {

                    //dispaly the content panel and keep the switch as checked.
                    FoursquareToggleSwitch.Checked -= FoursquareToggleSwitch_Checked;
                    FoursquareToggleSwitch.IsChecked = true;
                    FoursquareToggleSwitch.Checked += new EventHandler<RoutedEventArgs>(FoursquareToggleSwitch_Checked);
                    FoursquareLoggedInPanel.Visibility = System.Windows.Visibility.Visible;
                    FoursquareName.Text = Settings.Settings.FoursquareData.FirstName + " " + Settings.Settings.FoursquareData.LastName;
                }

                //check zoik it account
                if (!LoggedInToZoikit())
                {
                    ZoikitToggleSwitch.Unchecked -= ZoikitToggleSwitch_Unchecked;
                    ZoikitToggleSwitch.IsChecked = false;
                    ZoikitToggleSwitch.Unchecked += new EventHandler<RoutedEventArgs>(ZoikitToggleSwitch_Unchecked);
                    ZoikitLoggedInPanel.Visibility = System.Windows.Visibility.Collapsed;

                }
                else
                {

                    //dispaly the content panel and keep the switch as checked.
                    ZoikitToggleSwitch.Checked -= ZoikitToggleSwitch_Checked;
                    ZoikitToggleSwitch.IsChecked = true;
                    ZoikitToggleSwitch.Checked += new EventHandler<RoutedEventArgs>(ZoikitToggleSwitch_Checked);
                    ZoikitLoggedInPanel.Visibility = System.Windows.Visibility.Visible;
                    ZoikitName.Text = Settings.Settings.UserData.Username;
                }

                
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText,ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        #region Notification

        void Notifier_NotificationHidden(object sender, EventArgs e)
        {
            ShowTopLBL.Begin();
        }

        void Notifier_NotificationDisplayed(object sender, EventArgs e)
        {
            HidetopLBL.Begin();
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

        private bool LoggedInToZoikit()
        {
            return Core.Authentication.Accounts.LoggedInToZoikit();
        }

        void FoursquareToggleSwitch_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                //navigate to Facebook connect page
                NavigationService.Navigate(new Uri(Navigation.FoursquareConnect, UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void FoursquareToggleSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {

                if (MessageBoxResult.OK == MessageBox.Show("Do you want to deactivate connection of foursquare with Zoik it?", "Deactivate Foursquare", MessageBoxButton.OKCancel))
                {
                    //delete all facebook data!
                    DeleteFoursquareData();
                    CheckAllAccounts();
                    FoursquareLoggedInPanel.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    FoursquareToggleSwitch.Checked -= new EventHandler<RoutedEventArgs>(FoursquareToggleSwitch_Checked);
                    FoursquareToggleSwitch.IsChecked = true;
                    FoursquareToggleSwitch.Checked += new EventHandler<RoutedEventArgs>(FoursquareToggleSwitch_Checked);
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private void DeleteFoursquareData()
        {
            Core.Authentication.Accounts.DeleteFoursquareData();
        }

        private void CheckAllAccounts()
        {
            if (Core.Authentication.Accounts.LoggedOffAllAccounts())
            {
                DeleteAllData();
            }
        }

        private bool LoggedInToFacebook()
        {
            return Core.Authentication.Accounts.LoggedInToFacebook();

        }

        private bool LoggedInToTwitter()
        {
            return Core.Authentication.Accounts.LoggedInToTwitter();

        }

        private bool LoggedInToFoursquare()
        {
            return Core.Authentication.Accounts.LoggedInToFoursquare();

        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            try
            {
                //Dashboard btn clicked
                if (!Core.Authentication.Accounts.LoggedOffAllAccounts()) //if user is logged in to any one account then
                {
                    NavigationService.Navigate(new Uri(Navigation.Dashboard, UriKind.RelativeOrAbsolute));
                }
                else
                {
                    MessageBox.Show("You need to login to any of the below authentication providers before you're navigated to dashboard.", "Cannot Navigate to Dashboard", MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText,ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private void ApplicationBarProfileButton_Click(object sender, EventArgs e)
        {
            try
            {

                if (!Core.Authentication.Accounts.LoggedOffAllAccounts())
                {
                    NavigateToProfile();
                }
                else
                {
                    Dispatcher.BeginInvoke(() =>
                        {
                            MessageBox.Show("You need to login to any one of below authentication providers before you can view your profile.", "Cannot View Profile", MessageBoxButton.OK);
                        });
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText,ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private void NavigateToProfile()
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.Profile, UriKind.RelativeOrAbsolute));
                });
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText,ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }

        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (!LoggedInToFacebook()&&NavigationService.CanGoBack)
                {
                    NavigationService.RemoveBackEntry();
                    if (NavigationService.CanGoBack)
                    {
                        NavigationService.GoBack();
                    }
                }
                
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText,ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
            
        }


        #region NotificationControl

       

       
       
        private void NavigateToHangout(int id)
        {
            try
            {
                //navigate to hangout page
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.Hangout + "?id=" + id, UriKind.RelativeOrAbsolute));
                });
            }
            catch (System.Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);

            }
        }

        private void NavigateToViewProfile(int id)
        {
            try
            {
                //navigate to hangout page
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.ViewProfile + "?id=" + id, UriKind.RelativeOrAbsolute));
                });
            }
            catch (System.Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);

            }
        }

        #endregion

        private void ZoikitToggleSwitch_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                //navigate to Facebook connect page
                NavigationService.Navigate(new Uri(Navigation.ZoikitConnect, UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private void ZoikitToggleSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {

                if (MessageBoxResult.OK == MessageBox.Show("Do you want to log out from Zoik it?", "Log out?", MessageBoxButton.OKCancel))
                {
                    //delete all facebook data!
                    DeleteZoikitData();
                    CheckAllAccounts();
                    ZoikitLoggedInPanel.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    ZoikitToggleSwitch.Checked -= new EventHandler<RoutedEventArgs>(ZoikitToggleSwitch_Checked);
                    ZoikitToggleSwitch.IsChecked = true;
                    ZoikitToggleSwitch.Checked += new EventHandler<RoutedEventArgs>(ZoikitToggleSwitch_Checked);
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private void DeleteZoikitData()
        {
            Settings.Settings.UserData.Username = "";
        }

        private void ChangePasswordLBL_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigateToChangePassword();
        }

        private void NavigateToChangePassword()
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.ChangePassword, UriKind.RelativeOrAbsolute));
            });
        }

        private void TwitterToggleSwitch_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                //navigate to Facebook connect page
                NavigationService.Navigate(new Uri(Navigation.TwitterConnect, UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private void TwitterToggleSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {

                if (MessageBoxResult.OK == MessageBox.Show("Do you want to deactivate twitter from Zoik it?", "Deactivate Twitter?", MessageBoxButton.OKCancel))
                {
                    //delete all facebook data!
                    DeleteTwitterData();
                    CheckAllAccounts();
                    TwitterLoggedInPanel.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    ZoikitToggleSwitch.Checked -= new EventHandler<RoutedEventArgs>(ZoikitToggleSwitch_Checked);
                    ZoikitToggleSwitch.IsChecked = true;
                    ZoikitToggleSwitch.Checked += new EventHandler<RoutedEventArgs>(ZoikitToggleSwitch_Checked);
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private void DeleteTwitterData()
        {
            Settings.Settings.FoursquareData = null;
        }

        
    }
}