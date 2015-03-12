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

namespace Hangout.Client.Meetup
{
    public partial class HangoutPinned : PhoneApplicationPage
    {

        MeetupService.Meetup CurrentHangout;

       
        
        public HangoutPinned()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                HangoutList.HangoutLB.Height = 570;
                HangoutList.ShowMoreButton = false;
                HangoutList.HangoutSelected += new Controls.Hangouts.HangoutList.HangoutSelectedHelper(HangoutList_HangoutSelected);
                HangoutList.ProfileNameSeletcted += new Controls.Hangouts.HangoutList.ProfileNameSelectedHelper(HangoutList_ProfileNameSeletcted);
                HangoutList.ProfilePicSeletcted += new Controls.Hangouts.HangoutList.ProfilePicSelectedHelper(HangoutList_ProfilePicSeletcted);
                HangoutList.PinSeletcted += new Controls.Hangouts.HangoutList.PinSelectedHelper(HangoutList_PinSeletcted);
                HangoutList.UnpinSeletcted += new Controls.Hangouts.HangoutList.UnPinSelectedHelper(HangoutList_UnpinSeletcted);
                HangoutList.ShoutNewHangoutSelected += new EventHandler(HangoutList_ShoutNewHangoutSelected);

                Services.MeetupServiceClient.GetAllPinnedCompleted += new EventHandler<MeetupService.GetAllPinnedCompletedEventArgs>(client_GetAllPinnedCompleted);
                Services.MeetupServiceClient.GetAllPinnedAsync(Core.User.User.UserID, Settings.Settings.FacebookAccessToken);
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private void DetachEvents()
        {
            try
            {
                HangoutList.ShoutNewHangoutSelected -= HangoutList_ShoutNewHangoutSelected;
                HangoutList.HangoutSelected -= HangoutList_HangoutSelected;
                HangoutList.ProfileNameSeletcted -= HangoutList_ProfileNameSeletcted;
                HangoutList.ProfilePicSeletcted -= HangoutList_ProfilePicSeletcted;
                HangoutList.PinSeletcted -= HangoutList_PinSeletcted;
                HangoutList.UnpinSeletcted -= HangoutList_UnpinSeletcted;
                
            }
            catch (System.Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void HangoutList_ShoutNewHangoutSelected(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(()=>
            {
                NavigationService.Navigate(new Uri(Navigation.InitiateHangout, UriKind.RelativeOrAbsolute));
            });
        }

        void client_GetAllPinnedCompleted(object sender, MeetupService.GetAllPinnedCompletedEventArgs e)
        {
            Services.MeetupServiceClient.GetAllPinnedCompleted -= new EventHandler<MeetupService.GetAllPinnedCompletedEventArgs>(client_GetAllPinnedCompleted);
            try
            {
                PG.Visibility = System.Windows.Visibility.Collapsed;
                if (e.Error == null)
                {
                    ShowPage.Begin();
                    HangoutList.Hangouts = e.Result.ToList();
                    HangoutList.RefreshList();
                }
                else
                {
                    MessageBox.Show(ErrorText.Description, ErrorText.Caption, MessageBoxButton.OK);
                    NavigateToDashboard();
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }

        }



        void HangoutList_PinSeletcted(MeetupService.Meetup data)
        {
            try
            {
                
                Services.MeetupServiceClient.PinHangoutCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_PinHangoutCompleted);
                CurrentHangout = data;
                Services.MeetupServiceClient.PinHangoutAsync(Core.User.User.UserID, data.HangoutID, Settings.Settings.FacebookAccessToken);
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void client_PinHangoutCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Services.MeetupServiceClient.PinHangoutCompleted -= new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_PinHangoutCompleted);
            try
            {
                if (e.Error == null)
                {
                    HangoutList.DisplayUnPinImage(CurrentHangout);
                }
                else
                {
                    MessageBox.Show(ErrorText.Description, ErrorText.Caption, MessageBoxButton.OK);
                    NavigateToDashboard();
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void HangoutList_ProfilePicSeletcted(MeetupService.Meetup data)
        {
            try
            {
                if (data.User.UserID != Core.User.User.UserID)
                {
                    NavigateToViewProfile(data.User.UserID);
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void HangoutList_ProfileNameSeletcted(MeetupService.Meetup data)
        {
            try
            {
                if (data.User.UserID != Core.User.User.UserID)
                {
                    NavigateToViewProfile(data.User.UserID);
                }
            }
            catch (Exception ex)
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
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void HangoutList_HangoutSelected(MeetupService.Meetup e)
        {
            try
            {
                //navigate to hangout page
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.Hangout + "?id=" + e.HangoutID, UriKind.RelativeOrAbsolute));
                });
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void HangoutList_UnpinSeletcted(MeetupService.Meetup data)
        {
            try
            {
                
                Services.MeetupServiceClient.UnPinHangoutCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_UnPinHangoutCompleted);
                CurrentHangout = data;
                Services.MeetupServiceClient.UnPinHangoutAsync(Core.User.User.UserID, data.HangoutID, Settings.Settings.FacebookAccessToken);
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void client_UnPinHangoutCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Services.MeetupServiceClient.UnPinHangoutCompleted -= new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_UnPinHangoutCompleted);
            try
            {
                if (e.Error == null)
                {
                    HangoutList.RemoveHangoutData(CurrentHangout);
                    HangoutList.RefreshList();
                }
                else
                {
                    MessageBox.Show(ErrorText.Description, ErrorText.Caption, MessageBoxButton.OK);
                    NavigateToDashboard();
                }
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

        #region NotificationControl

        public void AttachEvents()
        {
            try
            {
                NotificationControl.NavigateToHangout += new Controls.Notifications.NotificationReceived.IdHelper(NotificationControl_NavigateToHangout);
                NotificationControl.NavigateToProfile += new Controls.Notifications.NotificationReceived.IdHelper(NotificationControl_NavigateToProfile);
            }
            catch (System.Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);

            }
        }

        void NotificationControl_NavigateToProfile(int id)
        {
            try
            {
                NavigateToViewProfile(id);
            }
            catch (System.Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);

            }
        }

        void NotificationControl_NavigateToHangout(int id)
        {
            try
            {
                NavigateToHangout(id);
            }
            catch (System.Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);

            }
        }

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

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            DetachEvents();
            base.OnNavigatedFrom(e);
        }

       

        #endregion
    }
}