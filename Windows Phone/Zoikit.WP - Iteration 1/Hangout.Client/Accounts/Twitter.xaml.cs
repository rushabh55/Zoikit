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
using Hangout.Client.Core.Authentication.Twitter;
using TweetSharp;
using Hammock.Silverlight.Compat;
using Microsoft.Xna.Framework;

namespace Hangout.Client.Accounts
{
    public partial class Twitter : PhoneApplicationPage
    {
        TweetSharp.TwitterService service;
        public Twitter()
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

            


            ShowConnectingYouToTwitter.Begin();
            Authorize();
            

        }

        public void Authorize()
        {
            // Step 1 - Retrieve an OAuth Request Token
            service = new TweetSharp.TwitterService("1i9gU4XCDgwrLrFwIvtQ", "79COrlDe7Kp1jH35FNs8cz8CvTNK2ewVoNoiuuHR6uM");

            // This is the registered callback URL
            service.GetRequestToken(@"http://www.zoikit.com", new Action<OAuthRequestToken, TwitterResponse>(Nav1));

        }

        private void Nav1(OAuthRequestToken requestToken, TwitterResponse arg2)
        {
            Dispatcher.BeginInvoke(() =>
                {

                    // Step 2 - Redirect to the OAuth Authorization URL
                    Uri uri = service.GetAuthorizationUri(requestToken);

                    TwitterWB.Navigated += TwitterWB_Navigated;
                    TwitterWB.Navigate(uri);
                });

        }

        void TwitterWB_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

            if (e.Uri.ToString().Contains(@"oauth/authorize"))
            {
                ShowPage.Begin();
            }

            if (e.Uri.ToString().Contains("zoikit.com/?"))
            {

                 

                ShowYouLookInteresting.Begin();
                //get OAuthtoken and OAuth Cerifier
                var queryString = string.Join(string.Empty, e.Uri.ToString().Split('?').Skip(1));
                NameValueCollection qscoll = System.Web.HttpUtility.ParseQueryString(queryString);
                string oauthToken = qscoll.Where(o => o.Key == "oauth_token").Select(o => o.Value).Single();
                string oauthverifier = qscoll.Where(o => o.Key == "oauth_verifier").Select(o => o.Value).Single();

                var requestToken = new OAuthRequestToken { Token = oauthToken };

                // Step 3 - Exchange the Request Token for an Access Token
                service.GetAccessToken(requestToken, oauthverifier, new Action<OAuthAccessToken, TwitterResponse>(Nav2));
               
              
            }
        }

        void TwitterServiceClient_RegisterUserCompleted(object sender, TwitterService.RegisterUserCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result == TwitterService.AccountStatus.AccountCreated || e.Result == TwitterService.AccountStatus.Updated)
                {
                    Services.TwitterServiceClient.GetUserDataCompleted+=TwitterServiceClient_GetUserDataCompleted;
                    Services.TwitterServiceClient.GetUserDataAsync(AccessToken, AccessTokenSecret,AppID.ZoikitAppID,AppID.ZoikitAppToken);
                }
            }
            else
            {
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                return;
            }
        }

        private void Nav2(OAuthAccessToken accessToken, TwitterResponse arg2)
        {
            // Step 4 - User authenticates using the Access Token
            service.AuthenticateWith(accessToken.Token, accessToken.TokenSecret);

            AccessToken=accessToken.Token;
            AccessTokenSecret=accessToken.TokenSecret;

            Dispatcher.BeginInvoke(() =>
                {

                    if (MessageBoxResult.OK == MessageBox.Show(Messages.PrivacyPolicyText, Messages.PrivacyPolicyCaption, MessageBoxButton.OKCancel))
                    {
                        if (Core.User.User.UserID == new Guid())
                        {
                            //if the user is not registered then..
                            Services.TwitterServiceClient.RegisterUserCompleted += TwitterServiceClient_RegisterUserCompleted;
                            Services.TwitterServiceClient.RegisterUserAsync(accessToken.Token, accessToken.TokenSecret,AppID.ZoikitAppID,AppID.ZoikitAppToken);
                        }
                        else
                        {
                            Services.TwitterServiceClient.UpdateTwitterDataCompleted += TwitterServiceClient_UpdateTwitterDataCompleted;
                            Services.TwitterServiceClient.UpdateTwitterDataAsync(Core.User.User.UserID, accessToken.Token, accessToken.TokenSecret, Core.User.User.ZAT);
                        }
                    }
                    else
                    {
                        Application.Current.Terminate();

                    }
                });
        }

        void TwitterServiceClient_UpdateTwitterDataCompleted(object sender, TwitterService.UpdateTwitterDataCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result == TwitterService.AccountStatus.AccountCreated || e.Result == TwitterService.AccountStatus.Updated)
                {
                    Services.TwitterServiceClient.GetUserDataCompleted += TwitterServiceClient_GetUserDataCompleted;
                    Services.TwitterServiceClient.GetUserDataAsync(AccessToken, AccessTokenSecret,AppID.ZoikitAppID,AppID.ZoikitAppToken);
                }
            }
            else
            {
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                return;
            }
        }

        void TwitterServiceClient_GetUserDataCompleted(object sender, TwitterService.GetUserDataCompletedEventArgs e)
        {
            Services.TwitterServiceClient.GetUserDataCompleted -= TwitterServiceClient_GetUserDataCompleted;
            if (e.Error == null)
            {

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
                }
                else
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
                        Settings.Settings.TwitterData = e.Result.TwitterData;
                    }
                }
            }
            else
            {
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                return;
            }

            if (Core.User.User.ValidateUserProfile())
            {
                NavigateToDashboard();
            }
            else
            {
                NavigateToProfilePage();
            }
        }

        private void NavigateToProfilePage()
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Profile, UriKind.RelativeOrAbsolute));
            });
        }

        private void NavigateToDashboard()
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Dashboard, UriKind.RelativeOrAbsolute));
            });
        }

       

       
       

       
    
public  string AccessToken { get; set; }
public  string AccessTokenSecret { get; set; }
    }
}