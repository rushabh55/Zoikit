using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Hangout.Client
{
    public static class Services
    {
        public static FacebookService.FacebookServiceClient FacebookServiceClient { get; set; }
        public static AccountService.AccountServiceClient AccountServiceClient { get; set; }
        public static ExceptionReportingService.ExceptionReportingServiceClient ExceptionReportingServiceClient { get; set; }

        public static UserService.UserServiceClient UserServiceClient { get; set; }
        public static UserLocationService.UserLocationServiceClient UserLocationServiceClient { get; set; }
        
        public static AppAuthenticationService.AppAuthenticationClient AppAuthenticationServiceClient { get; set; }

        public static TwitterService.TwitterServiceClient TwitterServiceClient { get; set; }

        public static BuzzService.BuzzServiceClient BuzzServiceClient { get; set; }

        public static TagService.TagServiceClient TagServiceClient { get; set; }
        public static PushNotificationService.PushNotificationsClient PushNotificationServiceClient { get; set; }
        public static TextService.TextServiceClient TextServiceClient { get; set; }

        public static DiscoverService.DiscoverServiceClient DiscoverServiceClient { get; set; }

        public static NotificationService.NotificationServiceClient NotificationServiceClient { get; set; }
        public static void Close()
        {
            AccountServiceClient.CloseAsync();
            ExceptionReportingServiceClient.CloseAsync();
            TagServiceClient.CloseAsync();
            UserServiceClient.CloseAsync();
            TextServiceClient.CloseAsync();
            UserLocationServiceClient.CloseAsync();
            DiscoverServiceClient.CloseAsync();
            FacebookServiceClient.CloseAsync();
            NotificationServiceClient.CloseAsync();
            AppAuthenticationServiceClient.CloseAsync();
            PushNotificationServiceClient.CloseAsync();
            TwitterServiceClient.CloseAsync();
            BuzzServiceClient.CloseAsync();
        }

        public static void Open()
        {
            if (AccountServiceClient == null)
            {
                AccountServiceClient = new AccountService.AccountServiceClient();
            }

            if (AccountServiceClient.State != System.ServiceModel.CommunicationState.Opened&&AccountServiceClient.State!=System.ServiceModel.CommunicationState.Opening)
            {

                AccountServiceClient.OpenAsync();
            }


            if (PushNotificationServiceClient == null)
            {
                PushNotificationServiceClient = new PushNotificationService.PushNotificationsClient();
            }

            if (PushNotificationServiceClient.State != System.ServiceModel.CommunicationState.Opened && PushNotificationServiceClient.State != System.ServiceModel.CommunicationState.Opening)
            {

                PushNotificationServiceClient.OpenAsync();
            }

            if (DiscoverServiceClient == null)
            {
                DiscoverServiceClient = new DiscoverService.DiscoverServiceClient();
            }

            if (DiscoverServiceClient.State != System.ServiceModel.CommunicationState.Opened && DiscoverServiceClient.State != System.ServiceModel.CommunicationState.Opening)
            {

                DiscoverServiceClient.OpenAsync();
            }


            if (TagServiceClient == null)
            {
                TagServiceClient = new TagService.TagServiceClient();
            }

            if (TagServiceClient.State != System.ServiceModel.CommunicationState.Opened && TagServiceClient.State != System.ServiceModel.CommunicationState.Opening)
            {

                TagServiceClient.OpenAsync();
            }

            if (NotificationServiceClient == null)
            {
                NotificationServiceClient = new NotificationService.NotificationServiceClient();
            }

            if (NotificationServiceClient.State != System.ServiceModel.CommunicationState.Opened && NotificationServiceClient.State != System.ServiceModel.CommunicationState.Opening)
            {

                NotificationServiceClient.OpenAsync();
            }

            if (TextServiceClient == null)
            {
               TextServiceClient = new TextService.TextServiceClient();
            }

            if (TextServiceClient.State != System.ServiceModel.CommunicationState.Opened && TextServiceClient.State != System.ServiceModel.CommunicationState.Opening)
            {

                TextServiceClient.OpenAsync();
            }



            if (UserServiceClient == null)
            {
                UserServiceClient = new UserService.UserServiceClient();
            }

            if (UserServiceClient.State != System.ServiceModel.CommunicationState.Opened && UserServiceClient.State != System.ServiceModel.CommunicationState.Opening)
            {

                UserServiceClient.OpenAsync();
            }

            if (BuzzServiceClient == null)
            {
                BuzzServiceClient = new BuzzService.BuzzServiceClient();
            }

            if (BuzzServiceClient.State != System.ServiceModel.CommunicationState.Opened && BuzzServiceClient.State != System.ServiceModel.CommunicationState.Opening)
            {

               BuzzServiceClient.OpenAsync();
            }

            if (ExceptionReportingServiceClient == null)
            {
                ExceptionReportingServiceClient = new ExceptionReportingService.ExceptionReportingServiceClient();
            }

            if (ExceptionReportingServiceClient.State != System.ServiceModel.CommunicationState.Opened && ExceptionReportingServiceClient.State != System.ServiceModel.CommunicationState.Opening)
            {

                ExceptionReportingServiceClient.OpenAsync();
            }

           
           
           
            if (UserLocationServiceClient == null)
            {
                UserLocationServiceClient = new UserLocationService.UserLocationServiceClient();
            }
            if (UserLocationServiceClient.State != System.ServiceModel.CommunicationState.Opened && UserLocationServiceClient.State != System.ServiceModel.CommunicationState.Opening)
            {
                UserLocationServiceClient.OpenAsync();
            }

           

            if (FacebookServiceClient == null)
                FacebookServiceClient = new FacebookService.FacebookServiceClient();
            if (FacebookServiceClient.State != System.ServiceModel.CommunicationState.Opened && FacebookServiceClient.State != System.ServiceModel.CommunicationState.Opening)
            {
                FacebookServiceClient.OpenAsync();
            }


           

            

           
           

            if (TwitterServiceClient == null)
            {
                TwitterServiceClient = new TwitterService.TwitterServiceClient();

            }
            if (TwitterServiceClient.State != System.ServiceModel.CommunicationState.Opened && TwitterServiceClient.State != System.ServiceModel.CommunicationState.Opening)
            {
                TwitterServiceClient.OpenAsync();
            }

            if (AppAuthenticationServiceClient == null)
            {
                AppAuthenticationServiceClient = new AppAuthenticationService.AppAuthenticationClient();

            }
            if (AppAuthenticationServiceClient.State != System.ServiceModel.CommunicationState.Opened && AppAuthenticationServiceClient.State != System.ServiceModel.CommunicationState.Opening)
            {

                AppAuthenticationServiceClient.OpenAsync();
            }
        }
    }
}
