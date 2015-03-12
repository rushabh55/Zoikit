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
using System.Windows.Media.Imaging;

namespace Hangout.Client.Accounts
{
    public partial class Foursquare : PhoneApplicationPage
    {

        string accessToken;

        public Foursquare()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!Core.Network.NetworkStatus.IsNetworkAvailable())
            {
                MessageBox.Show("We can't detect any data connection on your phone. Please try again when your phone has a stable data connection", "Data Network Not Found", MessageBoxButton.OK);
                new Game().Exit();
            }
                

            textLoader.DisplayText("Connecting to Foursquare");
            FoursquareWB.Navigate(new Uri(@"https://foursquare.com/oauth2/authenticate?client_id=" + AppID.FoursquareAppID + "&response_type=token&redirect_uri=http://www.zoik.it&display=touch", UriKind.RelativeOrAbsolute));
        }

        private void FoursquareWB_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

            if (e.Uri.ToString().Contains(@"access_token"))
            {

                HideBrowser.Begin();
                
                
                if(!Shown)
                {
                    Shown = true;
                    MessageBoxResult obj = MessageBox.Show(Messages.PrivacyPolicyText, Messages.PrivacyPolicyCaption, MessageBoxButton.OKCancel);
                    if (obj == MessageBoxResult.Cancel)
                    {
                        if (Core.Authentication.Accounts.LoggedOffAllAccounts())
                        {
                            new Game().Exit();
                        }
                        else
                        {
                            if (NavigationService.CanGoBack)
                            {
                                NavigationService.GoBack();
                            }
                            else
                            {
                                NavigateToDashboard();
                            }
                        }

                        return;

                    }
                    else
                    {
                        Accepted = true;
                    }
                    textLoader.DisplayText("You look interesting. We're learning more about you...");

                    
                    LoopInServer(e);
                }
                else if (Shown&&Accepted)
                {
                    LoopInServer(e);
                }

                
                return;
            }

            if (e.Uri.ToString().Contains("foursquare.com/oauth2/authenticate"))
            {
                //login page loaded. :)
                ShowBrowser.Begin();

                return;
            }
        }

        private void LoopInServer(System.Windows.Navigation.NavigationEventArgs e)
        {
            Shown = true;
            accessToken = e.Uri.ToString().Split('=')[1]; //got the accesss token :)

            if (Core.User.User.UserID == -1)
            {
                //if the user is not registered then..
                Services.FoursquareServiceClient.RegisterUserCompleted += FoursquareServiceClient_RegisterUserCompleted;
                Services.FoursquareServiceClient.RegisterUserAsync(accessToken);
            }
            else
            {
                Services.FoursquareServiceClient.UpdateFoursquareDataCompleted += FoursquareServiceClient_UpdateFoursquareDataCompleted;
                Services.FoursquareServiceClient.UpdateFoursquareDataAsync(Core.User.User.UserID, accessToken, Core.User.User.ZAT);
            }
        }

        private void NavigateToDashboard()
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Dashboard, UriKind.RelativeOrAbsolute));
            });
        }

        void FoursquareServiceClient_UpdateFoursquareDataCompleted(object sender, FoursquareService.UpdateFoursquareDataCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result == FoursquareService.AccountStatus.AccountCreated || e.Result == FoursquareService.AccountStatus.Updated)
                {
                    Services.FoursquareServiceClient.GetUserDataCompleted += FoursquareServiceClient_GetUserDataCompleted;
                    Services.FoursquareServiceClient.GetUserDataAsync(accessToken);
                }




            }

            else
            {
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                NavigateToDashboard();
                return;
            }
        }

        void FoursquareServiceClient_GetUserDataCompleted(object sender, FoursquareService.GetUserDataCompletedEventArgs e)
        {
            Services.FoursquareServiceClient.GetUserDataCompleted -= FoursquareServiceClient_GetUserDataCompleted;
            if (e.Error != null)
            {
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                NavigateToDashboard();
                return;
            }
            if (e.Result != null)
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
                    //ADD TWITTER HERE TOO 


                
            }
            else
            {
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                return;
            }

            NavigateToDashboard();
        }

        


        void FoursquareLoadHandler_FoursquareUpdateCompleted(object sender, EventArgs e)
        {
            //now navigate to accounts 
            NaviagteToAccounts();
        }

        

        private void NaviagteToAccounts()
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Accounts,UriKind.RelativeOrAbsolute));
            });
        }

        void AccountLoadHandler_FoursquareDataUpdateCompleted(object sender, EventArgs e)
        {
            //navigate to profile page. :)
            NavigateToProfile();
        }

        private void NavigateToProfile()
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Profile,UriKind.RelativeOrAbsolute));
            });
        }

        void FoursquareServiceClient_RegisterUserCompleted(object sender, FoursquareService.RegisterUserCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result == FoursquareService.AccountStatus.AccountCreated || e.Result == FoursquareService.AccountStatus.Updated)
                {
                    Services.FoursquareServiceClient.GetUserDataCompleted += FoursquareServiceClient_GetUserDataCompleted;
                    Services.FoursquareServiceClient.GetUserDataAsync(accessToken);
                }
            }

            else
            {
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                return;
            }

        }




        public bool Accepted { get; set; }

        public bool Shown { get; set; }
    }
}