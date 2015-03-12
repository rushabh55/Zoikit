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

namespace Hangout.Client.BackgroundAgent
{
    public static class Services
    {   
        
        public static AccountService.AccountServiceClient AccountServiceClient { get; set; }
        public static ExceptionReportingService.ExceptionReportingServiceClient ExceptionReportingServiceClient { get; set; }
        public static NotificationService.NotificationServiceClient NotificationServiceClient { get; set; }
        public static PushNotificationService.PushNotificationServiceClient PushNotificationServiceClient { get; set; }
        public static UserLocationService.UserLocationServiceClient UserLocationServiceClient { get; set; }
        
        public static void Close()
        {
            AccountServiceClient.CloseAsync();
            ExceptionReportingServiceClient.CloseAsync();
            NotificationServiceClient.CloseAsync();
            PushNotificationServiceClient.CloseAsync();
            UserLocationServiceClient.CloseAsync();
            
        }

        public static void Open()
        {
            if (AccountServiceClient == null)
            {
                AccountServiceClient = new AccountService.AccountServiceClient();
            }

            if (AccountServiceClient.State != System.ServiceModel.CommunicationState.Opened && AccountServiceClient.State != System.ServiceModel.CommunicationState.Opening)
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

            

            if (UserLocationServiceClient == null)
            {
                UserLocationServiceClient = new UserLocationService.UserLocationServiceClient();
            }
            if (UserLocationServiceClient.State != System.ServiceModel.CommunicationState.Opened && UserLocationServiceClient.State != System.ServiceModel.CommunicationState.Opening)
            {
                UserLocationServiceClient.OpenAsync();
            }
        }
    }
}
