using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Tasks;
using System.IO;
using Microsoft.Xna.Framework;

namespace Hangout.Client.Accounts
{
    public partial class ProfilePicture : PhoneApplicationPage
    {


        bool changed = false;

        public ProfilePicture()
        {
            InitializeComponent();
            
        }

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {

            if (!Core.Network.NetworkStatus.IsNetworkAvailable())
            {
                MessageBox.Show("We can't detect any data connection on your phone. Please try again when your phone has a stable data connection", "Data Network Not Found", MessageBoxButton.OK);
                new Game().Exit();
            }
                

            CurrentPicImage.Source = new BitmapImage(new Uri(Settings.Settings.ProfileImageURL, UriKind.RelativeOrAbsolute));
            LoadAvatars();
        }


        private void NavigateToProfile()
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Profile, UriKind.RelativeOrAbsolute));
            });
        }

        private void LoadAvatars()
        {
            Services.AccountServiceClient.GetAvatarsCompleted += AccountServiceClient_GetAvatarsCompleted;
            Services.AccountServiceClient.GetAvatarsAsync(Core.User.User.UserID, Core.User.User.ZAT);

        }

        void AccountServiceClient_GetAvatarsCompleted(object sender, AccountService.GetAvatarsCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result.Count > 0)
                {
                    AvatarStatusTB.Visibility = System.Windows.Visibility.Collapsed;
                    AvatarLoadPG.Visibility = System.Windows.Visibility.Collapsed;
                    ProfilePicLB.ItemsSource = e.Result.ToList();
                }
                else
                {
                    AvatarStatusTB.Text = "Oops, Its windy up there. Please try again after sometime.";
                    AvatarLoadPG.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
            else
            {
                // Network problem.
                AvatarStatusTB.Text = "It looks like you're having a problem with your network. Try again after some time.";
                AvatarLoadPG.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        public string CurrentPic { get; set; }

        private void ApplicationBarSaveButton_Click(object sender, EventArgs e)
        {
            if (changed)
            {
                ApplicationBar.IsVisible = false;
                //UPLOAD AND GET THE URL
                textloader.DisplayText("Uploading...");
                HidePage.Begin();
                Services.AccountServiceClient.SaveProfileImageCompleted += AccountServiceClient_SaveProfileImageCompleted;
                MemoryStream stream=new MemoryStream();
                WriteableBitmap wb=new WriteableBitmap((BitmapImage)CurrentPicImage.Source);
                wb.SaveJpeg(stream,((BitmapImage)CurrentPicImage.Source).PixelWidth,((BitmapImage)CurrentPicImage.Source).PixelHeight,0,70);
                Services.AccountServiceClient.SaveProfileImageAsync(Core.User.User.UserID,stream.ToArray(), Core.User.User.ZAT);

            }
            else
            {
                GoBack();
            }
        }

        void AccountServiceClient_SaveProfileImageCompleted(object sender, AccountService.SaveProfileImageCompletedEventArgs e)
        {
            if (e.Error == null)
            {
               SelectedURL = e.Result;
                GoBack();
            }
            else
            {
                MessageBox.Show(ErrorText.Description, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                ShowPage.Begin();
            }
        }

        private void ProfileLB_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (ProfilePicLB.SelectedItem != null)
            {
                if (MessageBoxResult.OK == MessageBox.Show("Do you want to select this avatar?","Select avatar",MessageBoxButton.OKCancel))
                {
                    SelectedURL = ProfilePicLB.SelectedItem.ToString();
                    CurrentPicImage.Source = new BitmapImage(new Uri(ProfilePicLB.SelectedItem.ToString(), UriKind.RelativeOrAbsolute));
                   
                    GoBack();
                }
            }
        }

        private void GoBack()
        {
            if (NavigationService.CanGoBack)
            {
                if (!String.IsNullOrWhiteSpace(SelectedURL))
                {
                    Settings.Settings.TempProfileImage = SelectedURL;
                }

                NavigationService.GoBack();
            }
        }

        private void GoToGallery_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            PhotoChooserTask photoChooserTask = new PhotoChooserTask();
            photoChooserTask.Completed += photoChooserTask_Completed;
            photoChooserTask.Show();
        }

        void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                changed = true;
                //Code to display the photo on the page in an image control named myImage.
                System.Windows.Media.Imaging.BitmapImage bmp = new System.Windows.Media.Imaging.BitmapImage();
                bmp.SetSource(e.ChosenPhoto);
                CurrentPicImage.Source = bmp;
            }
            

        }

        private void Camera_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            CameraCaptureTask cameraCaptureTask;
            cameraCaptureTask = new CameraCaptureTask();
            cameraCaptureTask.Completed += new EventHandler<PhotoResult>(cameraCaptureTask_Completed);
            cameraCaptureTask.Show();
        }

        void cameraCaptureTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                changed = true;
                //Code to display the photo on the page in an image control named myImage.
                System.Windows.Media.Imaging.BitmapImage bmp = new System.Windows.Media.Imaging.BitmapImage();
                bmp.SetSource(e.ChosenPhoto);
                CurrentPicImage.Source = bmp;
            }
        }



        public string SelectedURL { get; set; }
    }
}