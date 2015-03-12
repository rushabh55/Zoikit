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
    public partial class Start : PhoneApplicationPage
    {
        public Start()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Core.Authentication.Accounts.LoggedOffAllAccounts())
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        NavigationService.Navigate(new Uri(Navigation.FreshInstall, UriKind.RelativeOrAbsolute));
                    });

                    return;
                }

                if (!Core.User.User.ValidateUserProfile())
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        NavigationService.Navigate(new Uri(Navigation.Profile, UriKind.RelativeOrAbsolute));
                    });

                    return;
                }



                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.Dashboard, UriKind.RelativeOrAbsolute));
                });



                return;
            }
            catch { }
           
        }
    }
}