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
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using Microsoft.Xna.Framework;

namespace Hangout.Client.Accounts
{
    public partial class ZoikIt : PhoneApplicationPage
    {
        public ZoikIt()
        {
            InitializeComponent();
            TiltEffect.TiltableItems.Add(typeof(TextBlock));
            Notifier.NotificationClicked += Notifier_NotificationClicked;
            Notifier.NotificationDisplayed += Notifier_NotificationDisplayed;
            Notifier.NotificationHidden += Notifier_NotificationHidden;
        }

        private void ForgotPasswordLink_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigateToForgotPassword();
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

       

        private void RegisterLink_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MainPivot.SelectedIndex = 0;
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            //OK
            if (MainPivot.SelectedIndex == 1)
            {
                //login
                if (ValidateLogin())
                {
                    Login();
                }
            }
            if (MainPivot.SelectedIndex == 0)
            {
                //register
                if (ValidateRegister())
                {
                    Register();
                }

            }

        }

        private void Register()
        {
            if(ValidateRegister())
            {
                LoadingVisible();
                if(Core.User.User.UserID!=-1)
                {
                    Services.AppAuthenticationServiceClient.RegisterByIDCompleted+=AppAuthenticationServiceClient_RegisterByIDCompleted;
                    Services.AppAuthenticationServiceClient.RegisterByIDAsync(Core.User.User.UserID,RegUsernameTB.Text.Trim().ToLower(),RegEmailTB.Text.Trim(),RegPassTB.Password.Trim());
                }
                else
                {
                    Services.AppAuthenticationServiceClient.RegisterCompleted += AppAuthenticationServiceClient_RegisterCompleted;
                    Services.AppAuthenticationServiceClient.RegisterAsync(RegUsernameTB.Text.Trim().ToLower(), RegEmailTB.Text.Trim(), RegPassTB.Password.Trim());
                }
            }
        }

        void AppAuthenticationServiceClient_RegisterCompleted(object sender, AppAuthenticationService.RegisterCompletedEventArgs e)
        {
            Services.AppAuthenticationServiceClient.RegisterCompleted -= AppAuthenticationServiceClient_RegisterCompleted;
            if (e.Error != null)
            {
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                CollapseLogin();
                return;
            }
            RegisterCompleted(e.Result);

        }

        void AppAuthenticationServiceClient_RegisterByIDCompleted(object sender, AppAuthenticationService.RegisterByIDCompletedEventArgs e)
        {
            Services.AppAuthenticationServiceClient.RegisterByIDCompleted -= AppAuthenticationServiceClient_RegisterByIDCompleted;
            if (e.Error != null)
            {
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                CollapseLogin();
                return;
            }
            RegisterCompleted(e.Result);


           
        }

        private void RegisterCompleted(AppAuthenticationService.AppAuthenticationToken e)
        {
            if (e.Status == AppAuthenticationService.AccountStatus.AccountCreated)
            {
                if (MessageBoxResult.Cancel == MessageBox.Show(Messages.PrivacyPolicyText, Messages.PrivacyPolicyCaption, MessageBoxButton.OKCancel))
                {
                    new Game().Exit();
                    return;
                }
            }

            if (e.Status == AppAuthenticationService.AccountStatus.AccountCreated || e.Status == AppAuthenticationService.AccountStatus.Updated)
            { 
                Settings.Settings.ZAT = new Guid(e.Token);

                Services.AccountServiceClient.GetCompleteUserDataCompleted += AccountServiceClient_GetCompleteUserDataCompleted;
                Services.AccountServiceClient.GetCompleteUserDataAsync(e.Token);
                return;
            }
            if (e.Status == AppAuthenticationService.AccountStatus.AlreadyRegistered)
            {
                MessageBox.Show("You're already registered with this username, Please login ", "Already Registered", MessageBoxButton.OK);
            }

            if (e.Status == AppAuthenticationService.AccountStatus.Blocked)
            {
                MessageBox.Show("Your account is blocked. Please contact support.", "Blocked", MessageBoxButton.OK);
            }

            if (e.Status == AppAuthenticationService.AccountStatus.Error)
            {
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }

            if (e.Status == AppAuthenticationService.AccountStatus.UsernameInvalid)
            {
                MessageBox.Show("Your username is already taken, Please enter a different username", "Username Taken", MessageBoxButton.OK);
            }
            CollapseLogin();
        }

        private void Login()
        {
            Services.AppAuthenticationServiceClient.LoginCompleted += AppAuthenticationServiceClient_LoginCompleted;
            Services.AppAuthenticationServiceClient.LoginAsync(UsernameTB.Text.Trim(), PasswordTB.Password.Trim());
            
            //SHOW PROGRESS 
            LoadingVisible();



        }

        private void LoadingVisible()
        {
            ApplicationBar.IsVisible = false;
            HidePage.Begin();
            TextLoader.Visibility = System.Windows.Visibility.Visible;
            TextLoader.DisplayText();
        }

        void AppAuthenticationServiceClient_LoginCompleted(object sender, AppAuthenticationService.LoginCompletedEventArgs e)
        {
            
            Services.AppAuthenticationServiceClient.LoginCompleted -= AppAuthenticationServiceClient_LoginCompleted;

            if (e.Error != null)
            {
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                CollapseLogin();
                return;

            }
                

            if (e.Result.Status == AppAuthenticationService.AccountStatus.LoggedIn)
            {
                //load all other user data 
                Settings.Settings.ZAT = new Guid(e.Result.Token);

                Services.AccountServiceClient.GetCompleteUserDataCompleted += AccountServiceClient_GetCompleteUserDataCompleted;
                Services.AccountServiceClient.GetCompleteUserDataAsync(e.Result.Token);

                
            }

            if (e.Result.Status == AppAuthenticationService.AccountStatus.LogInFailed)
            {
                MessageBox.Show("Your username and password didnot match. Please try again.", "Log in failed", MessageBoxButton.OK);
                CollapseLogin();
            }

            if (e.Result.Status == AppAuthenticationService.AccountStatus.UsernameInvalid)
            {
                MessageBox.Show("Your username doesnot exists. Please register.", "Username Invalid", MessageBoxButton.OK);
                CollapseLogin();
            }

            
        }
        private void CollapseLogin()
        {
            ApplicationBar.IsVisible = true;
            ShowPage.Begin();
            TextLoader.Visibility = System.Windows.Visibility.Collapsed;
            TextLoader.HideTxt.Begin();
               
        }
        void AccountServiceClient_GetCompleteUserDataCompleted(object sender, AccountService.GetCompleteUserDataCompletedEventArgs e)
        {
            Services.AccountServiceClient.GetCompleteUserDataCompleted -= AccountServiceClient_GetCompleteUserDataCompleted;
            if (e.Error != null)
            {
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                CollapseLogin();
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
                CollapseLogin();
                return;
            }

            NavigateToDashboard();
            

        }

        private bool ValidateRegister()
        {
            if (String.IsNullOrWhiteSpace(RegUsernameTB.Text))
            {
                MessageBox.Show("Please enter your Username.","Enter Username",MessageBoxButton.OK);
                return false;
            }

            if (String.IsNullOrWhiteSpace(RegEmailTB.Text))
            {
                MessageBox.Show("Please enter your Email", "Enter Email", MessageBoxButton.OK);
                return false;
            }

            if (!IsValidEmail(RegEmailTB.Text))
            {
                MessageBox.Show("Please enter valid Email Address", "Enter Valid Email", MessageBoxButton.OK);
                return false;
            }

            

            if (String.IsNullOrWhiteSpace(RegPassTB.Password))
            {
                MessageBox.Show("Please enter your Password", "Enter Password", MessageBoxButton.OK);
                return false;
            }

            if (RegPassTB.Password.ToString().Count() < 6)
            {
                MessageBox.Show("You have a short password. Consider having a pasword whose minimum length is 6 characters.", "Short Password", MessageBoxButton.OK);
                return false;
            }

            if (String.IsNullOrWhiteSpace(RegConfirmPassTB.Password))
            {
                MessageBox.Show("Please confirm your Password", "Confirm Password", MessageBoxButton.OK);
                return false;
            }

            if (RegPassTB.Password.Trim() != RegConfirmPassTB.Password.Trim())
            {
                MessageBox.Show("Password and confirmation password do not match.", "Passwords donot match.", MessageBoxButton.OK);
                return false;
            }

            return true;

        }       

        private bool ValidateLogin()
        {

            if (String.IsNullOrWhiteSpace(UsernameTB.Text))
            {
                MessageBox.Show("Please enter your Username.", "Enter Username", MessageBoxButton.OK);
                return false;
            }

            if (String.IsNullOrWhiteSpace(PasswordTB.Password))
            {
                MessageBox.Show("Please enter your Password", "Enter Password", MessageBoxButton.OK);
                return false;
            }


            return true;
        }

        

        private void ApplicationBarIconButton_Click_2(object sender, EventArgs e)
        {
            if (Core.Authentication.Accounts.LoggedOffAllAccounts())
            {
                MessageBox.Show("You have to login or register before you can navigate to the dashboard","Cannot Navigate to Dashboard",MessageBoxButton.OK);
                return;
            }
            //Dashboard
            NavigateToDashboard();

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

        private void ApplicationBarIconButton_Click_3(object sender, EventArgs e)
        {
            //ACCOUNTS 
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

        private void NavigateToForgotPassword()
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.ForgotPassword, UriKind.RelativeOrAbsolute));
            });
        }

        private void LoginBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (ValidateLogin())
            {
                Login();
            }
        }

        private void LoginCancelBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            
           
                NavigateToAccountPage();
           
        }

        private void SignUpBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Register();
        }

        private void SignupCancelBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
             NavigateToAccountPage();
        }

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {

            if (!Core.Network.NetworkStatus.IsNetworkAvailable())
            {
                MessageBox.Show("We can't detect any data connection on your phone. Please try again when your phone has a stable data connection", "Data Network Not Found", MessageBoxButton.OK);
                new Game().Exit();
            }
                

            DisplayView();
        }

        private void DisplayView()
        {
            try
            {
                if (NavigationContext.QueryString.ContainsKey("view"))
                {

                    string view = NavigationContext.QueryString["view"].ToString();

                    if (view == "login")
                    {

                        MainPivot.SelectedIndex = 1;
                    }
                }
                
            }
            catch (System.Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                
            }
        }
    }
}