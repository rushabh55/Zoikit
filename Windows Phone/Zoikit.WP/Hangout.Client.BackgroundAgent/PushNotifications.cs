﻿using System;
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

namespace Hangout.Client.BackgroundAgent
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

        public delegate void NotificationPayloadReceivedHelper(int userId, int notificationId, string text, string meta, DateTime datetime);

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
               
            }


        }

        private static void SubscribeToService()
        {
            try
            {
                Services.PushNotificationServiceClient.SubscribeCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_SubscribeCompleted);
                Services.PushNotificationServiceClient.SubscribeAsync(Settings.UserData.UserID, httpChannel.ChannelUri, Settings.UserData.ZAT);
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
               
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
                FromUserID = -1;
                datetime = DateTime.Now;
            }
        }






        static void httpChannel_HttpNotificationReceived(object sender, HttpNotificationEventArgs e)
        {
            try
            {


                XDocument document;
                if (IsNotificationUpdatePayload(e.Notification.Body, out document))
                {
                    ParseNotificationRAWPayload(document);
                }

                if (IsBuzzCommentPayload(e.Notification.Body, out document))
                {
                    ParseBuzzCommentRAWPayload(document);
                }

            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
            }

        }

        public delegate void BuzzCommentNotificationReceivedHElper(int buzzId);

        public static event BuzzCommentNotificationReceivedHElper BuzzCommentNotificationReceived;

        private static void ParseBuzzCommentRAWPayload(XDocument document)
        {
            int BuzzId = Convert.ToInt32((from c in document.Descendants("BuzzCommentUpdate")
                                          select c.Element("BuzzID").Value).Single().ToString());

            if (BuzzCommentNotificationReceived != null)
            {
                BuzzCommentNotificationReceived(BuzzId);
            }

        }

        private static bool IsBuzzCommentPayload(Stream stream, out XDocument document)
        {
            try
            {

                using (var reader = new StreamReader(stream))
                {
                    string payload = reader.ReadToEnd().Replace('\0', ' ');
                    document = XDocument.Parse(payload);
                }

                if (document.Descendants("BuzzCommentUpdate") != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                document = null;
                return false;
            }
        }

        private static void ParseNotificationRAWPayload(XDocument document)
        {
            int NotificationCount = Convert.ToInt32((from c in document.Descendants("NotificationUpdate")
                                                     select c.Element("NotificationCount").Value).Single().ToString());
            System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                Settings.UnreadNotifications = NotificationCount;
            });

        }

        static bool IsNotificationUpdatePayload(Stream e, out XDocument document)
        {
            try
            {

                using (var reader = new StreamReader(e))
                {
                    string payload = reader.ReadToEnd().Replace('\0', ' ');
                    document = XDocument.Parse(payload);
                }

                if (document.Descendants("NotificationUpdate") != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                document = null;
                return false;
            }

        }






    }
}