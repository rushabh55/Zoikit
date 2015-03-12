using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Hangout.Client.Extras
{
    public partial class Settings : PhoneApplicationPage
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void locOffRB_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            MessageBox.Show("We're sorry, We need location services to be turned on for this app to be working properly. We cant turn off location services at this point of time. We apologize for inconvinience", "Turn off location services", MessageBoxButton.OK);
            locOnRB.IsChecked = true;
        }

        private void ApplicationBarIconButton_Click(object sender, System.EventArgs e)
        {
            if((bool)toastOnRB.IsChecked)
            {
                Hangout.Client.Settings.Settings.ToastEnabled = true;
                Core.PushNotification.PushNotification.BindToast();
            }
            else
            {
                Hangout.Client.Settings.Settings.ToastEnabled = false;
                Core.PushNotification.PushNotification.UnBindToShellToast();
            }



            NavigateBack();
        }

        private void NavigateBack()
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
                return;
            }

            //dashboard
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Dashboard, UriKind.RelativeOrAbsolute));
            });
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
                if(Hangout.Client.Settings.Settings.ToastEnabled)
                {
                    toastOnRB.IsChecked = true;
                }
                else
                {
                    toastOffRB.IsChecked = true;
                }
        }
    }
}