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
using Microsoft.Phone.Notification;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System.Windows.Threading;

namespace Hangout.Client.Core.PushNotification
{
    public class PushNotification
    {
        #region PushNotificaiton

       

        

        private static HttpNotificationChannel httpChannel;
        static string channelName = "WassupChannel";
        static string fileName = "PushNotificationsSettings.dat";
        static int pushConnectTimeout = 30;


        #endregion

        public delegate void PushNotificationExceptionOccouredHandler(NotificationChannelErrorEventArgs e);

        public delegate void PushNotificationReceivedHandler(HttpNotificationEventArgs e);

        public delegate void NotificationPayloadReceivedHelper(int userId,int notificationId,string text,string meta,DateTime datetime);

        public static event EventHandler PushNotificationSubscribed;
        public static event EventHandler PushNotificationSubscribedFailed;
        public static event PushNotificationExceptionOccouredHandler PushNotificationExceptionOccoured;
        public static event PushNotificationReceivedHandler PushNotificationReceived;
        public static event NotificationPayloadReceivedHelper NotificationUpdateRecieved;


      

        public static void ConnectToPushNotificationService()
        {
            try
            {
                //First, try to pick up existing channel
                httpChannel = HttpNotificationChannel.Find(channelName);

                if (null != httpChannel)
                {
                    //Trace("Channel Exists - no need to create a new one");
                    SubscribeToChannelEvents();

                    //Trace("Register the URI with 3rd party web service");
                    SubscribeToService();   //TODO: Place Notification

                    //Dispatcher.BeginInvoke(() => UpdateStatus("Channel recovered"));
                }
                else
                {
                    //Trace("Trying to create a new channel...");
                    //Create the channel
                    httpChannel = new HttpNotificationChannel(channelName, "MeetupService");
                    //Trace("New Push Notification channel created successfully");

                    SubscribeToChannelEvents();

                    //Trace("Trying to open the channel");
                    httpChannel.Open();


                    //Dispatcher.BeginInvoke(() => UpdateStatus("Channel open requested"));
                }
                if (!httpChannel.IsShellTileBound)
                {

                    httpChannel.BindToShellTile(); //Bign to the Tile :)
                }

            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }


        }

        private static void SubscribeToService()
        {
            try
            {
                Services.PushNotificationServiceClient.SubscribeCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_SubscribeCompleted);
                Services.PushNotificationServiceClient.SubscribeAsync(Core.User.User.UserID, httpChannel.ChannelUri, Core.User.User.ZAT);
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        static void client_SubscribeCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (PushNotificationSubscribed != null)
                        PushNotificationSubscribed(null, new EventArgs());
                }
                else
                {
                    if (PushNotificationSubscribedFailed != null)
                        PushNotificationSubscribedFailed(null, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        private static void SubscribeToChannelEvents()
        {
            try
            {
                //Register to UriUpdated event - occurs when channel successfully opens
                httpChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(httpChannel_ChannelUriUpdated);
                //Subscribed to Raw Notification
                httpChannel.HttpNotificationReceived += new EventHandler<HttpNotificationEventArgs>(httpChannel_HttpNotificationReceived);

                //general error handling for push channel
                httpChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(httpChannel_ExceptionOccurred);

            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }

        }


        static void httpChannel_ChannelUriUpdated(object sender, NotificationChannelUriEventArgs e)
        {
            try
            {
                //Trace("Channel opened. Got Uri:\n" + httpChannel.ChannelUri.ToString());

                //Trace("Subscribing to channel events");

                SubscribeToService();

                //Dispatcher.BeginInvoke(() => UpdateStatus("Channel created successfully"));
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }



        static void httpChannel_ExceptionOccurred(object sender, NotificationChannelErrorEventArgs e)
        {
            try
            {

                if (PushNotificationExceptionOccoured != null)
                    PushNotificationExceptionOccoured(e);

            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }


        private static void ParseMeetupRequestRAWPayload(Stream e, out int FromUserID, out DateTime datetime)
        {
            try
            {
                XDocument document;

                using (var reader = new StreamReader(e))
                {
                    string payload = reader.ReadToEnd().Replace('\0', ' ');
                    document = XDocument.Parse(payload);
                }

                FromUserID = Convert.ToInt32(from c in document.Descendants("MeetupRequest")
                                             select c.Element("UserID").Value);



                datetime = DateTime.Parse((from c in document.Descendants("MeetupRequest")
                                           select c.Element("DateTime").Value).First());
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                FromUserID = -1;
                datetime = DateTime.Now;
            }
        }

       




        static void httpChannel_HttpNotificationReceived(object sender, HttpNotificationEventArgs e)
        {
            try
            {

                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() => 
                {
                

                    XDocument document;

                    if (IsNotificationUpdatePayload(e.Notification.Body, out document))
                    {
                        ParseNotificationRAWPayload(document);
                    }

                    if (IsBuzzCommentPayload(document))
                    {
                        ParseBuzzCommentRAWPayload(document);
                    }

                    if (IsMessageRealTimeNotification(document))
                    {
                        ParseMessageRealTimeNotification(document);
                    }

                    if (IsUnreadMessageCountNotification(document))
                    {
                        ParseUnreadMessagePayload(document);
                    }

                });


                
            }
            catch (Exception ex)
            {
                
                Core.Exceptions.ExceptionReporting.Report(ex);
                
            }
            
        }

        private static void ParseMessageRealTimeNotification(XDocument document)
        {
            TextService.Text obj = new TextService.Text();

            obj.DateTimeStamp = Convert.ToDateTime((from c in document.Descendants("MessageRealTime")
                                                select c.Element("DateTimeStamp").Value).FirstOrDefault().ToString());

            obj.MarkAsRead = Convert.ToBoolean((from c in document.Descendants("MessageRealTime")
                                                    select c.Element("MarkAsRead").Value).FirstOrDefault().ToString());

            obj.TextId= Convert.ToInt32((from c in document.Descendants("MessageRealTime")
                                                    select c.Element("TextID").Value).FirstOrDefault().ToString());

            obj.TextMessage = ((from c in document.Descendants("MessageRealTime")
                                          select c.Element("TextMessage").Value).FirstOrDefault().ToString());

            obj.User = new TextService.CompactUser();

            obj.User.UserID = Convert.ToInt32((from c in document.Descendants("MessageRealTime")
                                          select c.Element("UserID").Value).FirstOrDefault().ToString());

            obj.User.ProfilePicURL= ((from c in document.Descendants("MessageRealTime")
                                          select c.Element("ProfilePicURL").Value).FirstOrDefault().ToString());

            obj.User.Name = ((from c in document.Descendants("MessageRealTime")
                                          select c.Element("UserName").Value).FirstOrDefault().ToString());

            if (RealTimeText != null)
            {
                RealTimeText(obj);
            }



        }


        public  delegate void RealTimeTextHelper(TextService.Text text);

        public static event RealTimeTextHelper RealTimeText;
     
      

        private static bool IsUnreadMessageCountNotification(XDocument document)
        {
            try
            {
                if (document.Descendants("MessageUpdate").Count() > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                document = null;
                return false;
            }
        }

        private static void ParseUnreadMessagePayload(XDocument document)
        {
            int MessageCount = Convert.ToInt32((from c in document.Descendants("MessageUpdate")
                                                select c.Element("MessageCount").Value).FirstOrDefault().ToString());
            System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                if (MessageCount > 99)
                {
                    Settings.Settings.UnreadMesssages = 99;
                }
                else
                {
                    Settings.Settings.UnreadMesssages = MessageCount;
                }
            });
        }

        private static bool IsMessageRealTimeNotification(XDocument document)
        {
            try
            {
                if (document.Descendants("MessageRealTime").Count() > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                document = null;
                return false;
            }
        }

        public delegate void BuzzCommentNotificationReceivedHElper(int buzzId);

        public static event BuzzCommentNotificationReceivedHElper BuzzCommentNotificationReceived;

        private static void ParseBuzzCommentRAWPayload(XDocument document)
        {
            int BuzzId = Convert.ToInt32((from c in document.Descendants("BuzzCommentUpdate")
                                                     select c.Element("BuzzID").Value).FirstOrDefault().ToString());

            if (BuzzCommentNotificationReceived != null)
            {
                BuzzCommentNotificationReceived(BuzzId);
            }

        }

        private static bool IsBuzzCommentPayload(XDocument document)
        {
            try
            {
                if (document.Descendants("BuzzCommentUpdate").Count() > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                document = null;
                return false;
            }
        }

        private static void ParseNotificationRAWPayload(XDocument document)
        {
           int NotificationCount= Convert.ToInt32((from c in document.Descendants("NotificationUpdate")
                                      select c.Element("NotificationCount").Value).FirstOrDefault().ToString());
           System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
               {
                   if (NotificationCount > 99)
                   {
                       Settings.Settings.UnreadNotifications = 99;
                   }
                   else
                   {
                       Settings.Settings.UnreadNotifications = NotificationCount;
                   }
               });
           
        }

        static bool IsNotificationUpdatePayload(Stream e,out XDocument document)
        {
            try
            {

                using (var reader = new StreamReader(e))
                {
                    string payload = reader.ReadToEnd().Replace('\0', ' ');
                    document = XDocument.Parse(payload);
                }

                if (document.Descendants("NotificationUpdate").Count() > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                document = null;
                return false;
            }

        }

        




    }
}
