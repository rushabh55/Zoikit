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
using System.Windows.Navigation;
using Microsoft.Xna.Framework;

namespace Hangout.Client.Accounts
{
    public partial class Profile : PhoneApplicationPage
    {

       
       
        bool BirthdayEdited = false;

        public Profile()
        {
            InitializeComponent();
            Notifier.NotificationClicked += Notifier_NotificationClicked;
            Notifier.NotificationDisplayed += Notifier_NotificationDisplayed;
            Notifier.NotificationHidden += Notifier_NotificationHidden;
            TiltEffect.TiltableItems.Add(typeof(Canvas));
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.NavigationMode == NavigationMode.New)
            {
                LoadProfile();
            }

            if (String.IsNullOrWhiteSpace(Settings.Settings.TempProfileImage))
            {
                if (Settings.Settings.ProfileImageURL != "")
                {
                    ProfileImage.Source = new BitmapImage(new Uri(Settings.Settings.ProfileImageURL, UriKind.RelativeOrAbsolute));
                }
            }
            else
            {
                ProfileImage.Source = new BitmapImage(new Uri(Settings.Settings.TempProfileImage, UriKind.RelativeOrAbsolute));
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

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {


            if (!Core.Network.NetworkStatus.IsNetworkAvailable())
            {
                MessageBox.Show("We can't detect any data connection on your phone. Please try again when your phone has a stable data connection", "Data Network Not Found", MessageBoxButton.OK);
                new Game().Exit();
            }
                

            textLoader.DisplayText("Updating your profile...");
            
            Core.Location.CurrentLocation.PositionChanged += locationObj_PositionChanged;
            
            try
            {
                BirthdayDP.ValueChanged += new EventHandler<DateTimeValueChangedEventArgs>(BirthdayDP_ValueChanged);
                
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void locationObj_PositionChanged(System.Device.Location.GeoPositionChangedEventArgs<System.Device.Location.GeoCoordinate> e)
        {
            NavigateToDashboard();
        }

        

        

        void BirthdayDP_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            BirthdayEdited = true;
        }

        private void LoadProfile()
        {
            try
            {
                if (Settings.Settings.UserData != null)
                {
                    if (Settings.Settings.UserData != null)
                    {
                        
                        if (Settings.Settings.UserData.Name != null)
                        {
                            NameTB.Text = Settings.Settings.UserData.Name;
                        }
                        if (!BirthdayEdited) //if birthday is not edited then load it from settings. :)
                        {

                            if (Settings.Settings.UserData.Birthday != null)
                            {
                                BirthdayDP.Value = Settings.Settings.UserData.Birthday.Value;
                            }
                        }
                        if (Settings.Settings.UserData.Gender != null)
                        {
                            if ((bool)Settings.Settings.UserData.Gender)
                            {
                                MaleRB.IsChecked = true;
                            }
                            else
                            {
                                FemaleRB.IsChecked = true;
                            }
                        }

                        if (String.IsNullOrEmpty(Settings.Settings.UserData.ProfilePicURL))
                        {
                            ProfileImage.Source = new BitmapImage(new Uri(Settings.Settings.UserData.ProfilePicURL, UriKind.RelativeOrAbsolute));
                        }

                      

                        if (Settings.Settings.UserData.RelationshipStatus != null)
                        {
                            string rs = Settings.Settings.UserData.RelationshipStatus;
                            if (rs.ToLower() == "single")
                            {
                                RelationShipStatusLP.SelectedIndex = 0;
                            }
                            if (rs.ToLower() == "in a relationship")
                            {
                                RelationShipStatusLP.SelectedIndex = 1;
                            }
                            if (rs.ToLower() == "engaged")
                            {
                                RelationShipStatusLP.SelectedIndex = 2;
                            }
                            if (rs.ToLower() == "married")
                            {
                                RelationShipStatusLP.SelectedIndex = 3;
                            }
                        }



                        if (Settings.Settings.UserData.Bio != null)
                        {
                            BioTB.Text = Settings.Settings.UserData.Bio;
                        }

                        if (Settings.Settings.UserData.Phone != null)
                        {
                            //PhoneTB.Text = Settings.Settings.UserData.Phone;
                        }

                        if (String.IsNullOrWhiteSpace(Settings.Settings.UserData.DefaultLengthUnits))
                        {
                            MilesRB.IsChecked = true;
                        }
                        else
                        {
                            if (Settings.Settings.UserData.DefaultLengthUnits == "Miles")
                            {
                                MilesRB.IsChecked = true;
                            }

                            if (Settings.Settings.UserData.DefaultLengthUnits == "Km")
                            {
                                KmRB.IsChecked = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }


            
        }

        

        private void NavigateBack()
        {
            try
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
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            try
            {
                //save btn
                SaveProfile();
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private void SaveProfile()
        {
            try
            {
                //save profile
                if (ValidateProfile())
                {
                    HidePage.Begin();
                    this.ApplicationBar.IsVisible = false;
                    Core.Location.CurrentLocation.UserLocationUpdateCompleted += CurrentLocation_UserLocationUpdateCompleted;
                    Core.Location.CurrentLocation.StartWatcher();
                    Core.Location.CurrentLocation.UpdateLocationToServer();
                    


                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }

        }

        void CurrentLocation_UserLocationUpdateCompleted()
        {
            SaveUserProfile();
        }

        private void SaveUserProfile()
        {
            if (Settings.Settings.UserData != null)
            {
                AccountService.UserData obj = Settings.Settings.UserData;
                obj.Name = NameTB.Text.Trim();
                obj.Phone = "13579024681";


                if ((bool)MaleRB.IsChecked)
                {
                    obj.Gender = true;
                }
                else
                {
                    obj.Gender = false;
                }

                obj.Birthday = BirthdayDP.Value;

                string relationship = "Single";

                if (RelationShipStatusLP.SelectedIndex == 1)
                {
                    relationship = "In a Relationship";
                }

                if (RelationShipStatusLP.SelectedIndex == 2)
                {
                    relationship = "Engaged";
                }

                if (RelationShipStatusLP.SelectedIndex == 3)
                {
                    relationship = "Married";
                }

                obj.RelationshipStatus = relationship;

                obj.Bio = BioTB.Text.Trim();

                if ((bool)MilesRB.IsChecked)
                {
                    obj.DefaultLengthUnits = "Miles";
                }
                else
                {
                    obj.DefaultLengthUnits = "Km";
                }

                obj.UserID = Core.User.User.UserID;
                obj.Age = DateTime.Now.Year - obj.Birthday.Value.Year;


                if (String.IsNullOrWhiteSpace(Settings.Settings.TempProfileImage))
                {
                    if (Settings.Settings.ProfileImageURL != "")
                    {
                        obj.ProfilePicURL = Settings.Settings.ProfileImageURL;
                    }
                }
                else
                {
                    obj.ProfilePicURL = Settings.Settings.TempProfileImage;
                }

                //update the phone storage
                Settings.Settings.UserData = obj;


                //update the server! :)
                Services.AccountServiceClient.UpdateUserDataCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_UpdateUserDataCompleted);
                Services.AccountServiceClient.UpdateUserDataAsync(obj, Settings.Settings.UserData.ZAT);


            }
            else
            {
                MessageBox.Show("There was an connecting with the server. Please try again later", "Error", MessageBoxButton.OK);
            }
        }

        void client_UpdateUserDataCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Services.AccountServiceClient.UpdateUserDataCompleted -= new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_UpdateUserDataCompleted);
            try
            {
                if (e.Error == null)
                {
                    Services.AccountServiceClient.GetCompleteUserDataCompleted += AccountServiceClient_GetCompleteUserDataCompleted;
                    Services.AccountServiceClient.GetCompleteUserDataAsync(Core.User.User.ZAT);

                    
                }
                else
                {
                    ShowPage.Begin();
                    this.ApplicationBar.IsVisible = true;
                    MessageBox.Show(ErrorText.Description,ErrorText.Caption,MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void AccountServiceClient_GetCompleteUserDataCompleted(object sender, AccountService.GetCompleteUserDataCompletedEventArgs e)
        {
            Services.AccountServiceClient.GetCompleteUserDataCompleted -= AccountServiceClient_GetCompleteUserDataCompleted;
            if (e.Error != null)
            {
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                ShowPage.Begin();
                return;
            }

            if (e.Result != null)
            {
                
                    Settings.Settings.UserData = e.Result.UserData;
                    Settings.Settings.FacebookData = e.Result.FacebookData;
                    Settings.Settings.FoursquareData = e.Result.FoursquareData;

                    //ADD TWITTER HERE TOO 


               
            }
            else
            {
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                ShowPage.Begin();
                return;
            }




            Core.Location.CurrentLocation.StartWatcher();
            NavigateToDashboard();
        }

        

        

        private bool ValidateProfile()
        {
            try
            {
                if (String.IsNullOrWhiteSpace(NameTB.Text))
                {
                    MessageBox.Show("Please enter your name.","Enter Name",MessageBoxButton.OK);
                    return false;
                }

                if (!BirthdayDP.Value.HasValue)
                {
                    MessageBox.Show("Please enter your Birthday.","Enter your birthday",MessageBoxButton.OK);
                    return false;
                }
                

                if (DateTime.Now.Year - BirthdayDP.Value.Value.Year <= 13)
                {
                    MessageBox.Show("You must be atleast 13 years old to use this app.", "You're too young", MessageBoxButton.OK);
                    return false;
                }

                if (DateTime.Now.Year - BirthdayDP.Value.Value.Year <= 0)
                {
                    MessageBox.Show("Please enter your valid Date Of Birth", "Enter valid Birthday", MessageBoxButton.OK);
                    return false;
                }

                if (MaleRB.IsChecked == null && FemaleRB.IsChecked == null)
                {
                    MessageBox.Show("Please select your gender.","Select your gender",MessageBoxButton.OK);
                    return false;
                }

                if (RelationShipStatusLP.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select your relationship status.", "Are you in a relation?", MessageBoxButton.OK);
                    return false;
                }



                if (String.IsNullOrWhiteSpace(BioTB.Text))
                {
                    MessageBox.Show("Please enter your bio", "We want to know more about you", MessageBoxButton.OK);
                    return false;
                }

                int count = BioTB.Text.ToCharArray().Count();

                if (count > 500)
                {
                    MessageBox.Show("Your Bio shouldnot be more than 500 characters long. Its now " + count + " charcters long.", "OMG! Your bio is too long. Make it short", MessageBoxButton.OK);
                    return false;
                }

               

                if (!(bool)MilesRB.IsChecked && !(bool)KmRB.IsChecked)
                {
                    MessageBox.Show("Please select your preferred length units","How do you measure diatance?",MessageBoxButton.OK);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                return false;
            }
        }

        private void ApplicationBarIconButton_Click_2(object sender, EventArgs e)
        {
            try
            {
                //account
                if (Core.User.User.ValidateUserProfile())
                {
                    if (MessageBoxResult.OK == MessageBox.Show("Do you want to save your profile before leaving?", "Save Profile", MessageBoxButton.OKCancel))
                    {
                        SaveProfile();
                    }
                    else
                    {
                        NavigateToDashboard();
                    }
                }
                else
                {
                    MessageBox.Show("Please fillup your user profile before you navigate to account", "Let us know more about you", MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
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

        //private string ValidatePhoneNumber()
        //{
        //    try
        //    {
        //        return PhoneTB.Text;
        //    }
        //    catch (Exception ex)
        //    {
        //        Core.Exceptions.ExceptionReporting.Report(ex);
        //        MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
        //        return "";
        //    }
        //}

        private void ApplicationBarIconButton_Account(object sender, System.EventArgs e)
        {
            try
            {
                //account
                if (Core.User.User.ValidateUserProfile())
                {
                    if (MessageBoxResult.OK == MessageBox.Show("Do you want to save your profile before leaving?", "Save Profile", MessageBoxButton.OKCancel))
                    {
                        SaveProfile();
                    }
                    else
                    {
                        NavigateToAccount();
                    }
                }
                else
                {
                    MessageBox.Show("Please fillup your user profile before you navigate to account", "Let us know more about you", MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }
		
		private void NavigateToAccount()
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

        

        private void ProfilePicCanvas_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
        	// TODO: Add event handler implementation here.
        }

        #endregion

        private void ProfilePicCanvas_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigateToProfileImage();
        }

        private void NavigateToProfileImage()
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.ProfilePicture, UriKind.RelativeOrAbsolute));
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