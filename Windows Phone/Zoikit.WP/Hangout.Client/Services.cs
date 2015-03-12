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
        public static GeocodeService.GeocodeServiceClient GeocodeServiceClient { get; set; }
        public static GeoLocationSearchService.SearchServiceClient GeoLocationSearchServiceClient { get; set; }
        public static NotificationService.NotificationServiceClient NotificationServiceClient { get; set; }
        public static PushNotificationService.PushNotificationServiceClient PushNotificationServiceClient { get; set; }
        public static TextService.TextServiceClient TextServiceClient { get; set; }
        public static TrophyService.TrophyServiceClient TrophyServiceClient { get; set; }
        public static UserLocationService.UserLocationServiceClient UserLocationServiceClient { get; set; }
        public static UserScoreService.UserScoreServiceClient UserScoreServiceClient { get; set; }
        public static FoursquareService.FoursquareServiceClient FoursquareServiceClient { get; set; }
        public static AppAuthenticationService.AppAuthenticationClient AppAuthenticationServiceClient { get; set; }
        public static CategoryService.CategoryServiceClient CategoryServiceClient { get; set; }
        public static UserService.UserServiceClient UserServiceClient{ get; set; }
        public static BuzzService.BuzzServiceClient BuzzServiceClient { get; set; }
        public static TokenService.TokenServiceClient TokenServiceClient { get; set; }
        public static TwitterService.TwitterServiceClient TwitterServiceClient { get; set; }


        public static VenueService.VenueServiceClient VenueServiceClient { get; set; }

        public static void Close()
        {
            AccountServiceClient.CloseAsync();
            ExceptionReportingServiceClient.CloseAsync();
            GeocodeServiceClient.CloseAsync();
            GeoLocationSearchServiceClient.CloseAsync();
            NotificationServiceClient.CloseAsync();
            PushNotificationServiceClient.CloseAsync();
            TextServiceClient.CloseAsync();
            TrophyServiceClient.CloseAsync();
            UserLocationServiceClient.CloseAsync();
            UserScoreServiceClient.CloseAsync();
            FacebookServiceClient.CloseAsync();
            FoursquareServiceClient.CloseAsync();
            AppAuthenticationServiceClient.CloseAsync();
            CategoryServiceClient.CloseAsync();
            VenueServiceClient.CloseAsync();
            UserServiceClient.CloseAsync();
            BuzzServiceClient.CloseAsync();
            TokenServiceClient.CloseAsync();
            TwitterServiceClient.CloseAsync();
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

            if (ExceptionReportingServiceClient == null)
            {
                ExceptionReportingServiceClient = new ExceptionReportingService.ExceptionReportingServiceClient();
            }

            if (ExceptionReportingServiceClient.State != System.ServiceModel.CommunicationState.Opened && ExceptionReportingServiceClient.State != System.ServiceModel.CommunicationState.Opening)
            {

                ExceptionReportingServiceClient.OpenAsync();
            }

            if (GeocodeServiceClient == null)
            {
                GeocodeServiceClient = new GeocodeService.GeocodeServiceClient("BasicHttpBinding_IGeocodeService");
            }

            if (GeocodeServiceClient.State != System.ServiceModel.CommunicationState.Opened && GeocodeServiceClient.State != System.ServiceModel.CommunicationState.Opening)
            {

                GeocodeServiceClient.OpenAsync();
            }

            if (GeoLocationSearchServiceClient == null)
            {
                GeoLocationSearchServiceClient = new GeoLocationSearchService.SearchServiceClient();
            }

            if (GeoLocationSearchServiceClient.State != System.ServiceModel.CommunicationState.Opened && GeoLocationSearchServiceClient.State != System.ServiceModel.CommunicationState.Opening)
            {

                GeoLocationSearchServiceClient.OpenAsync();
            }

           

            if (NotificationServiceClient == null)
            {
                NotificationServiceClient = new NotificationService.NotificationServiceClient();
            }

            if (NotificationServiceClient.State != System.ServiceModel.CommunicationState.Opened && NotificationServiceClient.State != System.ServiceModel.CommunicationState.Opening)
            {

                NotificationServiceClient.OpenAsync();
            }

            if (PushNotificationServiceClient == null)
            {
                PushNotificationServiceClient = new PushNotificationService.PushNotificationServiceClient();
            }

            if (PushNotificationServiceClient.State != System.ServiceModel.CommunicationState.Opened && PushNotificationServiceClient.State != System.ServiceModel.CommunicationState.Opening)
            {

                PushNotificationServiceClient.OpenAsync();
            }

            if (TextServiceClient == null)
            {
                TextServiceClient = new TextService.TextServiceClient();
            }

            if (TextServiceClient.State != System.ServiceModel.CommunicationState.Opened && TextServiceClient.State != System.ServiceModel.CommunicationState.Opening)
            {
                TextServiceClient.OpenAsync();
            }
            
            if(TrophyServiceClient==null)
            {
                TrophyServiceClient = new TrophyService.TrophyServiceClient();
            }
            if (TrophyServiceClient.State != System.ServiceModel.CommunicationState.Opened && TrophyServiceClient.State != System.ServiceModel.CommunicationState.Opening)
            {
                TrophyServiceClient.OpenAsync();
            }

            if (UserLocationServiceClient == null)
            {
                UserLocationServiceClient = new UserLocationService.UserLocationServiceClient();
            }
            if (UserLocationServiceClient.State != System.ServiceModel.CommunicationState.Opened && UserLocationServiceClient.State != System.ServiceModel.CommunicationState.Opening)
            {
                UserLocationServiceClient.OpenAsync();
            }

            if (UserScoreServiceClient == null)
            {
                UserScoreServiceClient = new UserScoreService.UserScoreServiceClient();
            }

            if (UserScoreServiceClient.State != System.ServiceModel.CommunicationState.Opened && UserScoreServiceClient.State != System.ServiceModel.CommunicationState.Opening)
            {

                UserScoreServiceClient.OpenAsync();
            }

            if (FacebookServiceClient == null)
                FacebookServiceClient = new FacebookService.FacebookServiceClient();
            if (FacebookServiceClient.State != System.ServiceModel.CommunicationState.Opened && FacebookServiceClient.State != System.ServiceModel.CommunicationState.Opening)
            {
                FacebookServiceClient.OpenAsync();
            }


            if (FoursquareServiceClient == null)
            {
                FoursquareServiceClient = new FoursquareService.FoursquareServiceClient();
            }
            if (FoursquareServiceClient.State != System.ServiceModel.CommunicationState.Opened && FoursquareServiceClient.State != System.ServiceModel.CommunicationState.Opening)
            {

                FoursquareServiceClient.OpenAsync();
            }

            if (AppAuthenticationServiceClient == null)
            {
                AppAuthenticationServiceClient = new AppAuthenticationService.AppAuthenticationClient();

            }
            if (AppAuthenticationServiceClient.State != System.ServiceModel.CommunicationState.Opened && AppAuthenticationServiceClient.State != System.ServiceModel.CommunicationState.Opening)
            {

                AppAuthenticationServiceClient.OpenAsync();
            }

            if (CategoryServiceClient == null)
            {
                CategoryServiceClient = new CategoryService.CategoryServiceClient();

            }
            if (CategoryServiceClient.State != System.ServiceModel.CommunicationState.Opened && CategoryServiceClient.State != System.ServiceModel.CommunicationState.Opening)
            {
                CategoryServiceClient.OpenAsync();
            }

            if (VenueServiceClient == null)
            {
                VenueServiceClient = new VenueService.VenueServiceClient();

            }
            if (VenueServiceClient.State != System.ServiceModel.CommunicationState.Opened && VenueServiceClient.State != System.ServiceModel.CommunicationState.Opening)
            {
                VenueServiceClient.OpenAsync();
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

            if (TokenServiceClient == null)
            {
                TokenServiceClient = new TokenService.TokenServiceClient();

            }
            if (TokenServiceClient.State != System.ServiceModel.CommunicationState.Opened && TokenServiceClient.State != System.ServiceModel.CommunicationState.Opening)
            {
                TokenServiceClient.OpenAsync();
            }

            if (TwitterServiceClient == null)
            {
                TwitterServiceClient = new TwitterService.TwitterServiceClient();

            }
            if (TwitterServiceClient.State != System.ServiceModel.CommunicationState.Opened && TwitterServiceClient.State != System.ServiceModel.CommunicationState.Opening)
            {
                TwitterServiceClient.OpenAsync();
            }
        }
    }
}
