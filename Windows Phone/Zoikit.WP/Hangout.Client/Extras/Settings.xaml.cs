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

namespace Hangout.Client.Extras
{
    public partial class Settings : PhoneApplicationPage
    {

        

        public Settings()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                UpdateTrackMeButton();
                CheckBackgroundAgent();
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private void UpdateTrackMeButton()
        {
            try
            {

                if (Hangout.Client.Settings.Settings.TrackMe)
                {
                    //update the apbar button :)
                    LTSwitch.Checked -= new EventHandler<RoutedEventArgs>(LTSwitch_Checked);
                    LTSwitch.IsChecked = true;
                    LTSwitch.Checked += new EventHandler<RoutedEventArgs>(LTSwitch_Checked);
                }
                else
                {
                    //update the action button :)
                    LTSwitch.Unchecked -= new EventHandler<RoutedEventArgs>(LTSwitch_Unchecked);
                    LTSwitch.IsChecked = false;
                    LTSwitch.Unchecked += new EventHandler<RoutedEventArgs>(LTSwitch_Unchecked);
                }

            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }

        }


        private void CheckBackgroundAgent()
        {
            try
            {
                if (Hangout.Client.Settings.Settings.TrackMe)
                {
                    // If the task already exists and background agents are enabled for the
                    // application, you must remove the task and then add it again to update 
                    // the schedule
                    if (Core.BackgroundTask.UserLocationUpdateAgent.Find() == null)
                    {
                        Core.BackgroundTask.UserLocationUpdateAgent.StartUserUpdateAgentAgent();
                    }

                    if (Core.BackgroundTask.UserLocationUpdateAgent.Find().IsEnabled == false)
                    {
                        Core.BackgroundTask.UserLocationUpdateAgent.StartUserUpdateAgentAgent();
                    }
                }
                else
                {
                    Core.BackgroundTask.UserLocationUpdateAgent.RemoveAgent();
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private void LTSwitch_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                //checked :)
                LoadingPG.Visibility = System.Windows.Visibility.Visible;
                Hangout.Client.Settings.Settings.TrackMe = true;
                CheckBackgroundAgent();
                LoadingPG.Visibility = System.Windows.Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private void LTSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadingPG.Visibility = System.Windows.Visibility.Visible;
                Hangout.Client.Settings.Settings.TrackMe = false;
                CheckBackgroundAgent();
                LoadingPG.Visibility = System.Windows.Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            try
            {
                //dashboard :)
                NavigateToDashboard();
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

       
    }
}