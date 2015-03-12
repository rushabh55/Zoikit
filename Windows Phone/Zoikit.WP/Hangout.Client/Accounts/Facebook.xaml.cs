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
using System.Text;
using Facebook;
using Microsoft.Xna.Framework;
using System.Windows.Media.Imaging;


namespace Hangout.Client.Accounts
{
    public partial class Facebook : PhoneApplicationPage
    {


        string accessToken;
        private const string ExtendedPermissions = "user_about_me,publish_stream,user_checkins,friends_checkins,user_likes,user_location,email,user_relationships,user_relationship_details,read_friendlists,publish_checkins,offline_access";
        private readonly FacebookClient _fb = new FacebookClient();
        

        public Facebook()
        {
            InitializeComponent();
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
                

                textLoader.DisplayText("We're connecting you to Facebook.");
                FacebookAuthenticationBrowser.Navigate(new Uri(@"https://www.facebook.com/dialog/oauth?client_id="+AppID.FacebookAppID+"&redirect_uri=https://www.facebook.com/connect/login_success.html"+"&scope="+ExtendedPermissions+"&response_type=token&display=touch",UriKind.Absolute));
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }

        }




        
        private void Browser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            try
            {

                if (e.Uri.ToString().Contains(@"m.facebook.com/dialog/oauth?client_id"))
                {
                    textLoader.Visibility = System.Windows.Visibility.Collapsed;
                    ShowBrowser.Begin();
                }

                FacebookOAuthResult oauthResult;
                if (! _fb.TryParseOAuthCallbackUrl(e.Uri, out oauthResult))
                {
                    return;
                }

                if (oauthResult.IsSuccess)
                {
                    HideBrowser.Begin();

                    if (MessageBoxResult.OK == MessageBox.Show(Messages.PrivacyPolicyText, Messages.PrivacyPolicyCaption, MessageBoxButton.OKCancel))
                    {

                        textLoader.Visibility = System.Windows.Visibility.Visible;
                        textLoader.DisplayText("You look quite interestng. We're learning more about you...");
                        accessToken = oauthResult.AccessToken;

                        if (Core.User.User.UserID == -1)
                        {
                            //if the user is not registered then..
                            Services.FacebookServiceClient.RegisterUserCompleted += FacebookServiceClient_RegisterUserCompleted;
                            Services.FacebookServiceClient.RegisterUserAsync(accessToken);
                        }
                        else
                        {
                            Services.FacebookServiceClient.UpdateFacebookDataCompleted += FacebookServiceClient_UpdateFacebookDataCompleted;
                            Services.FacebookServiceClient.UpdateFacebookDataAsync(Core.User.User.UserID, accessToken, Core.User.User.ZAT);
                        }
                        //load all the user profile information and store.

                       
                    }
                    else
                    {
                        new Game().Exit();
                    }


                }
                else
                {
                    MessageBox.Show(oauthResult.ErrorDescription);
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void FacebookServiceClient_UpdateFacebookDataCompleted(object sender, FacebookService.UpdateFacebookDataCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result == FacebookService.AccountStatus.AccountCreated || e.Result == FacebookService.AccountStatus.Updated)
                {
                    Services.FacebookServiceClient.GetUserDataCompleted += FacebookServiceClient_GetUserDataCompleted;
                    Services.FacebookServiceClient.GetUserDataAsync(accessToken);
                }
            }
            else
            {
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                return;
            }
        }

        void FacebookServiceClient_GetUserDataCompleted(object sender, FacebookService.GetUserDataCompletedEventArgs e)
        {
            Services.FacebookServiceClient.GetUserDataCompleted -= FacebookServiceClient_GetUserDataCompleted;
            if (e.Error == null)
            {
                Services.FacebookServiceClient.GetUserDataCompleted -= FacebookServiceClient_GetUserDataCompleted;
                Services.AccountServiceClient.GetCompleteUserDataCompleted += AccountServiceClient_GetCompleteUserDataCompleted;
                Services.AccountServiceClient.GetCompleteUserDataAsync(e.Result.ZAT);
            }
            else
            {
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                return;
            }

        }

        void AccountServiceClient_GetCompleteUserDataCompleted(object sender, AccountService.GetCompleteUserDataCompletedEventArgs e)
        {
            Services.AccountServiceClient.GetCompleteUserDataCompleted -= AccountServiceClient_GetCompleteUserDataCompleted;
            if (e.Error != null)
            {
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                return;
            }

            if (e.Result != null)
            {
                if (Core.Authentication.Accounts.LoggedOffAllAccounts())
                {
                    if (e.Result.UserData != null)
                    {
                        if (!String.IsNullOrWhiteSpace(e.Result.UserData.ProfilePicURL))
                        {
                            Settings.Settings.ProfileImageURL = e.Result.UserData.ProfilePicURL;
                        }
                        Settings.Settings.UserID = e.Result.UserData.UserID;
                        Settings.Settings.ZAT = new Guid(e.Result.UserData.ZAT);
                        Settings.Settings.UserData = e.Result.UserData;
                        Settings.Settings.FacebookData = e.Result.FacebookData;
                        Settings.Settings.FoursquareData = e.Result.FoursquareData;
                        Settings.Settings.TwitterData = e.Result.TwitterData;
                    }

                    //ADD TWITTER HERE TOO 


                }
            }
            else
            {
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                return;
            }

            NavigateToDashboard();
        }

        private void NavigateToDashboard()
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Dashboard, UriKind.RelativeOrAbsolute));
            });
        }

        
        private void NaviagteToAccounts()
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Accounts, UriKind.RelativeOrAbsolute));
            });
        }

        void FacebookServiceClient_RegisterUserCompleted(object sender, FacebookService.RegisterUserCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result == FacebookService.AccountStatus.AccountCreated || e.Result == FacebookService.AccountStatus.Updated)
                {
                    Services.FacebookServiceClient.GetUserDataCompleted += FacebookServiceClient_GetUserDataCompleted;
                    Services.FacebookServiceClient.GetUserDataAsync(accessToken);
                }
            }
            else
            {
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                return;
            }
        }

       

       

        

        private void NavigateToAccounts()
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.Accounts, UriKind.RelativeOrAbsolute));
                });
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
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
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
            
        }

        

        


        

    }
}