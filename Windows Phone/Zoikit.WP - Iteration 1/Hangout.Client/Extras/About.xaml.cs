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

namespace Hangout.Client.Extras
{
    public partial class About : PhoneApplicationPage
    {
        public About()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            //version.
            version.Text = System.Reflection.Assembly.GetExecutingAssembly()
                    .FullName.Split('=')[1].Split(',')[0];
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            if(NavigationService.CanGoBack)
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

        private void version_Copy_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //feedback
            EmailComposeTask emailComposeTask = new EmailComposeTask();

            emailComposeTask.Subject = "Feedback for Zoik it!";
            emailComposeTask.Body = "";
            emailComposeTask.To = "911@zoikit.com";
           

            emailComposeTask.Show();
        }

        private void version_Copy1_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
                //privacy
           MessageBox.Show(Messages.PrivacyPolicyText, Messages.PrivacyPolicyCaption, MessageBoxButton.OK);
        }
    }
}