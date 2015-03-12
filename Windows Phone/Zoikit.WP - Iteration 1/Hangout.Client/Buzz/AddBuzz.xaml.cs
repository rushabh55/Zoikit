using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using System.IO;
using System.Windows.Media.Imaging;

namespace Hangout.Client.Buzz
{
    public partial class AddBuzz : PhoneApplicationPage
    {

        bool CameraImage = false;
        bool AlbumImage = false;
        bool LocationGrab = true; 
        public AddBuzz()
        {
            InitializeComponent();
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            //ok Btn
            SaveBuzz();
            
        }

        private void SaveBuzz()
        {
            this.Focus();

            if ((AlbumImage || CameraImage) || BuzzTB.Text.Trim().Count() != 0)
            {

            }
            else
            {
                MessageBox.Show("Please enter your buzz in textbox below and shout-out. ", "Empty shout", MessageBoxButton.OK);
                return;
            }

            ShowLoading.Begin();

           

            //image first. 
            if(AlbumImage||CameraImage)
            {
                if(BuzzImage.Source!=null)
                {
                    MemoryStream stream=new MemoryStream();
                    WriteableBitmap wb=new WriteableBitmap((BitmapImage)BuzzImage.Source);
                    wb.SaveJpeg(stream,((BitmapImage)BuzzImage.Source).PixelWidth,((BitmapImage)BuzzImage.Source).PixelHeight,0,100);

                    Services.BuzzServiceClient.UploadImageCompleted += BuzzServiceClient_UploadImageCompleted;
                    Services.BuzzServiceClient.UploadImageAsync(Core.User.User.UserID,Core.User.User.ZAT,stream.ToArray());
                }
            }
            else
            {
                UploadBuzz("");
            }
           
        }

        void BuzzServiceClient_UploadImageCompleted(object sender, BuzzService.UploadImageCompletedEventArgs e)
        {

            Services.BuzzServiceClient.UploadImageCompleted -= BuzzServiceClient_UploadImageCompleted;

           if(e.Error==null)
           {
               if(e.Result.ToString()!="")
               {
                   UploadBuzz(e.Result.ToString());
               }
               else
               {
                   MessageBox.Show("We can't save this image.", "We're sorry", MessageBoxButton.OK);
                   ShowPage.Begin();
               }
           }
           else
           {
               MessageBox.Show("We can't save this image.", "We're sorry", MessageBoxButton.OK);
               ShowPage.Begin();
           }
        }

        private void UploadBuzz(string imageUrl)
        {
            Services.BuzzServiceClient.InsertBuzzCompleted += BuzzServiceClient_InsertBuzzCompleted;
            if(SelectedCity.Id==Core.Location.CurrentLocation.Location.City.Id)
            {
                 Services.BuzzServiceClient.InsertBuzzAsync(Core.User.User.UserID, BuzzTB.Text.Trim(), Core.Location.CurrentLocation.Location.Latitude, Core.Location.CurrentLocation.Location.Longitude,Core.Location.CurrentLocation.Location.City.Id, imageUrl, Core.User.User.ZAT);
            }
            else
            {
                Services.BuzzServiceClient.InsertBuzzAsync(Core.User.User.UserID, BuzzTB.Text.Trim(), 0.0, 0.0, SelectedCity.Id, imageUrl, Core.User.User.ZAT);
            }

                   
                    
        }

        void BuzzServiceClient_InsertBuzzCompleted(object sender, BuzzService.InsertBuzzCompletedEventArgs e)
        {
            Services.BuzzServiceClient.InsertBuzzCompleted -= BuzzServiceClient_InsertBuzzCompleted;

            if (e.Error == null)
            {
                if (e.Result == BuzzService.BuzzSaveStatus.Saved || e.Result == BuzzService.BuzzSaveStatus.Spam)
                {
                    NavigateToDashboard();
                }
                else
                {
                    MessageBox.Show("We can't save this buzz.", "We're sorry", MessageBoxButton.OK);
                    ShowPage.Begin();
                }
            }
            else
            {
                MessageBox.Show("We can't save this buzz.", "We're sorry", MessageBoxButton.OK);
                ShowPage.Begin();
            }


        }

        private void NavigateToDashboard()
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Dashboard, UriKind.RelativeOrAbsolute));
            });
        }

        private void LocationBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if(LocationGrab)
            {
                LocationGrab = false;
                LocationGreySB.Begin();
            }
            else
            {
                LocationGrab = true;
                LocationLightSB.Begin();
            }
        }

        

      

      
        void chooser_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {

                if (CameraImage)
                {
                    CameraImage = false;
                    CameraGreySB.Begin();
                }

                //Code to display the photo on the page in an image control named myImage.
                System.Windows.Media.Imaging.BitmapImage bmp = new System.Windows.Media.Imaging.BitmapImage();
                bmp.SetSource(e.ChosenPhoto);
                BuzzImage.Source = bmp;
                CameraImage = false;
                AlbumImage = true;

            }
            else
            {
                AlbumGreySB.Begin();
                AlbumImage = false;
            }

            if(AlbumImage||CameraImage)
            {
                BuzzImage.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                BuzzImage.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void BuzzTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextCountLBL.Text = (160 - BuzzTB.Text.Count()).ToString();

            if(BuzzTB.Text.Count()==0)
            {
                TypeBuzzLBL.Visibility = System.Windows.Visibility.Visible;
                OKBtn.IsEnabled = false;
            }
            else
            {
                TypeBuzzLBL.Visibility = System.Windows.Visibility.Collapsed;
                OKBtn.IsEnabled = true;
                
            }



        }

        private void TypeBuzzLBL_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            BuzzTB.Focus();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeAppbar();

            

            if (Core.Location.Location.SelectedCity != null)
            {
                SelectedCity = Core.Location.Location.SelectedCity;
               
            }
            else
            {
                Core.Location.Location.SelectedCity = Core.Location.CurrentLocation.Location.City;
                SelectedCity = Core.Location.Location.SelectedCity;
               
            }
            LocationLBL.Text = SelectedCity.Name + ", " + SelectedCity.Country.Name;
        }

        private void InitializeAppbar()
        {
            if (OKBtn == null)
            {
                OKBtn = new ApplicationBarIconButton(new Uri("/Assets/AppBar/check.png", UriKind.RelativeOrAbsolute));
                OKBtn.Text = "shout";
            }
        }

        public string ImageURL { get; set; }

        private void CameraBtn_Click(object sender, EventArgs e)
        {
            if (AlbumImage)
            {
                BuzzImage.Visibility = System.Windows.Visibility.Collapsed;
                AlbumGreySB.Begin();
                AlbumImage = false;
                BuzzImage.Source = null;
            }
            else
            {

                PhotoChooserTask chooser = new PhotoChooserTask();
                chooser.Completed += chooser_Completed;
                chooser.PixelHeight = 400;
                chooser.PixelWidth = 400;
                chooser.ShowCamera = true;
                chooser.Show();
                AlbumLightSB.Begin();
            }
        }

        private void LocationSP_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.LocationPicker, UriKind.RelativeOrAbsolute));
            });
        }

        public UserLocationService.City SelectedCity { get; set; }
    }
}