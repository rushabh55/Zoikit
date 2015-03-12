using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Hangout.Client
{
    public partial class FreshInstall : PhoneApplicationPage
    {
        public FreshInstall()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    ShowSignInBtns.Begin();
                    BackgroundAnimate.Begin();
                    string temp = "- rediscover local life,- your local network, - discover local buzz, - meet stunning locals ";
                    FadingTextControl.StartTextAnimation(temp.Split(',').ToList()); ;
                });
            }
            catch { }
        }

        private void FacebookButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.FacebookConnect, UriKind.RelativeOrAbsolute));
                });
            }
            catch { }
        }

        private void ZoikitButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.ZoikitConnect, UriKind.RelativeOrAbsolute));
                });
            }
            catch { }
        }

        private void TwitterButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.TwitterConnect, UriKind.RelativeOrAbsolute));
                });
            }
            catch { }
        }
    }
}