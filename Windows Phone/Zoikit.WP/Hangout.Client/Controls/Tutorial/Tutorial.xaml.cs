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
using System.Windows.Media.Imaging;
using Microsoft.Xna.Framework.Input.Touch;

namespace Hangout.Client.Controls.Tutorial
{
    public partial class Tutorial : UserControl
    {

        private BitmapImage tn = new BitmapImage();

        public event EventHandler TutorialCompleted;

        int TotalImages = 0;

        private int currentImage = 0;

        String prefixName = String.Empty;


        public int CurrentImage
        {
            get
            {
                return currentImage;
            }

            set
            {
                currentImage = value;
                if (currentImage > TotalImages)
                {
                    TutorialImage.Visibility = System.Windows.Visibility.Visible;
                    if (TutorialCompleted != null)
                    {
                        TutorialImage.Visibility = System.Windows.Visibility.Collapsed;
                        currentImage = 0;
                        TotalImages = 0;
                        //TutorialCompleted(null, new EventArgs());
                    }
                }
                else
                {
                    ChangeImage();
                }
            }
        }

        private void ChangeImage()
        {
            try
            {
                tn.CreateOptions = BitmapCreateOptions.None;
                tn.SetSource(Application.GetResourceStream(new Uri(@"Images/Tutorial/" + prefixName + currentImage + ".jpg", UriKind.Relative)).Stream);
                TutorialImage.Source = tn;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        public Tutorial()
        {
            InitializeComponent();
        }

        private Visibility tutorialVisibility;

        public Visibility TutorialVisibility
        {
            get
            {
                return tutorialVisibility;
            }

            set
            {
                tutorialVisibility = value;
                
                if (tutorialVisibility == System.Windows.Visibility.Visible)
                {
                    CurrentImage = 0;
                }

                LayoutRoot.Visibility = tutorialVisibility;
            }
        }

        private void TutorialImage_Tap(object sender, GestureEventArgs e)
        {
            CurrentImage++;
        }

        private void ScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            TouchPanel.EnabledGestures = GestureType.Flick | GestureType.FreeDrag | GestureType.HorizontalDrag;
        }

        
        TranslateTransform t = new TranslateTransform();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            prefixName = "Home";
            TotalImages = 8;
            TutorialImage.Visibility = System.Windows.Visibility.Visible;
            ChangeImage();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            prefixName = "Profile";
            TotalImages = 4;
            TutorialImage.Visibility = System.Windows.Visibility.Visible;
            ChangeImage();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            prefixName = "Create";
            TotalImages = 2;
            TutorialImage.Visibility = System.Windows.Visibility.Visible;
            ChangeImage();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            prefixName = "Pinned";
            TotalImages = 8;
            TutorialImage.Visibility = System.Windows.Visibility.Visible;
            ChangeImage();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            prefixName = "Hangout";
            TotalImages = 10;
            TutorialImage.Visibility = System.Windows.Visibility.Visible;
            ChangeImage();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            prefixName = "Accounts";
            TotalImages = 1;
            TutorialImage.Visibility = System.Windows.Visibility.Visible;
            ChangeImage();
        }
    }
}
