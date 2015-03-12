using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using WindowsPhone.Recipes.Push.Messasges;

namespace Hangout.Web.Core.PushNotifications
{
    public class PushNotifications
    {

        public static void Subscribe(Guid userId, Uri channelUri)
        {
            Core.Cloud.TableStorage.InitializePushNotificationsTable();

            Model.PushNotification notification = GetPushNotification(userId);
            bool flag = false;
            if (notification == null)
            {
                flag = true;
                notification = new Model.PushNotification(userId, channelUri.ToString());
            }

            notification.URI = channelUri.ToString();

            if (flag)
            {
                Model.Table.PushNotifications.Execute(TableOperation.Insert(notification));
            }
            else
            {
                Model.Table.PushNotifications.Execute(TableOperation.Replace(notification));
            }
            
        }

        public static void Unsubscribe(Guid userId)
        {
            Core.Cloud.TableStorage.InitializePushNotificationsTable();
            Model.PushNotification notification = GetPushNotification(userId);
            if (notification != null)
            {
                Model.Table.PushNotifications.Execute(TableOperation.Delete(notification));
            }
        }

        public static Model.PushNotification GetPushNotification(Guid userId)
        {
            Core.Cloud.TableStorage.InitializePushNotificationsTable();
            Model.Location loc = Core.Location.UserLocation.GetLastLocation(userId);
            if (loc != null)
            {
                TableQuery<Model.PushNotification> pn = new TableQuery<Model.PushNotification>().Where(TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.PushNotification.GetPartitionKey(loc.CityID)), TableOperators.And, TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, userId.ToString())));

                return Model.Table.PushNotifications.ExecuteQuery(pn).FirstOrDefault();
            }

            return null;
        }

        public static bool Exists(Guid userId)
        {
            Core.Cloud.TableStorage.InitializePushNotificationsTable();
            if (GetPushNotification(userId) == null)
            {
                return false;
            }
            return true;
        }

        private static byte[] PrepareMeetupRAWPayload(Guid userId, DateTime datetime)
        {
            MemoryStream stream = new MemoryStream();

            XmlWriterSettings settings = new XmlWriterSettings() { Indent = true, Encoding = Encoding.UTF8 };
            XmlWriter writer = XmlTextWriter.Create(stream, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("MeetupUpdate");

            writer.WriteStartElement("UserID");
            writer.WriteValue(userId);
            writer.WriteEndElement();

            writer.WriteStartElement("DateTime");
            writer.WriteValue(datetime);
            writer.WriteEndElement();


            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();

            byte[] payload = stream.ToArray();
            return payload;
        }

        public static void SendMeetupRequestNotificationRAW(Guid fromuserId, Guid touserId)
        {



            RawPushNotificationMessage rawPushNotificationMessage = new RawPushNotificationMessage(MessageSendPriority.High);

            //Prepare payload
            byte[] payload = PrepareMeetupRAWPayload(fromuserId, DateTime.Now);

            rawPushNotificationMessage.RawData = payload;

            if (Exists(touserId))
            {
                rawPushNotificationMessage.Send(new Uri(GetPushNotification(touserId).URI, UriKind.RelativeOrAbsolute));
            }

        }

        public static void SendNotification(Model.Notifications notification)
        {
            SendRawNotification(notification); //send RAW Notification :)

        }

        public static void SendWindows7TileNotification(Guid userId)
        {
            try
            {
                // Get the URI that the Microsoft Push Notification Service returns to the Push Client when creating a notification channel.
                // Normally, a web service would listen for URIs coming from the web client and maintain a list of URIs to send
                // notifications out to.
                Core.Cloud.TableStorage.InitializePushNotificationsTable();

                Model.PushNotification pn = GetPushNotification(userId);


                if (pn == null)
                {
                    return;

                }

                if (pn.URI == null)
                {
                    return;
                }

                string subscriptionUri = pn.URI.ToString();


                HttpWebRequest sendNotificationRequest = (HttpWebRequest)WebRequest.Create(subscriptionUri);

                // Create an HTTPWebRequest that posts the Tile notification to the Microsoft Push Notification Service.
                // HTTP POST is the only method allowed to send the notification.
                sendNotificationRequest.Method = "POST";

                // The optional custom header X-MessageID uniquely identifies a notification message. 
                // If it is present, the same value is returned in the notification response. It must be a string that contains a UUID.
                // sendNotificationRequest.Headers.Add("X-MessageID", "<UUID>");

                // Create the Tile message.
                string tileMessage = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<wp:Notification xmlns:wp=\"WPNotification\">" +
                    "<wp:Tile>" +
                      "<wp:BackTitle> Notifications </wp:BackTitle>" +
                      "<wp:BackContent>" + Notifications.Notification.GetUnreadNotificationCount(userId) + " new notifications.</wp:BackContent>" +
                   "</wp:Tile> " +
                "</wp:Notification>";

                // Set the notification payload to send.
                byte[] notificationMessage = Encoding.Default.GetBytes(tileMessage);

                // Set the web request content length.
                sendNotificationRequest.ContentLength = notificationMessage.Length;
                sendNotificationRequest.ContentType = "text/xml";
                sendNotificationRequest.Headers.Add("X-WindowsPhone-Target", "token");
                sendNotificationRequest.Headers.Add("X-NotificationClass", "1");


                using (Stream requestStream = sendNotificationRequest.GetRequestStream())
                {
                    requestStream.Write(notificationMessage, 0, notificationMessage.Length);
                }

                // Send the notification and get the response.
                HttpWebResponse response = (HttpWebResponse)sendNotificationRequest.GetResponse();
                string notificationStatus = response.Headers["X-NotificationStatus"];
                string notificationChannelStatus = response.Headers["X-SubscriptionStatus"];
                string deviceConnectionStatus = response.Headers["X-DeviceConnectionStatus"];

                // Display the response from the Microsoft Push Notification Service.  
                // Normally, error handling code would be here. In the real world, because data connections are not always available,
                // notifications may need to be throttled back if the device cannot be reached.


            }
            catch 
            {

            }

        }

        private static void SendRawNotification(Model.Notifications Notification)
        {
            //send a push notification to the user. :)
            try
            {
                // Get the URI that the Microsoft Push Notification Service returns to the push client when creating a notification channel.
                // Normally, a web service would listen for URIs coming from the web client and maintain a list of URIs to send
                // notifications out to.

                Core.Cloud.TableStorage.InitializePushNotificationsTable();

                RawPushNotificationMessage rawPushNotificationMessage = new RawPushNotificationMessage(MessageSendPriority.High);

                //Prepare payload
                byte[] payload = PrepareNotificationRAWPayload(Notification);

                rawPushNotificationMessage.RawData = payload;

                if (Exists(Notification.UserID))
                {
                    rawPushNotificationMessage.Send(new Uri(GetPushNotification(Notification.UserID).URI, UriKind.RelativeOrAbsolute));
                }
            }
            catch 
            {
                //report exception


            }
            
        }

        private static byte[] PrepareNotificationRAWPayload(Model.Notifications notification)
        {
            MemoryStream stream = new MemoryStream();

            XmlWriterSettings settings = new XmlWriterSettings() { Indent = true, Encoding = Encoding.UTF8 };
            XmlWriter writer = XmlTextWriter.Create(stream, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("NotificationUpdate");

            writer.WriteStartElement("UserID");
            writer.WriteValue(notification.UserID);
            writer.WriteEndElement();

            writer.WriteStartElement("NotificationID");
            writer.WriteValue(notification.NotificationID);
            writer.WriteEndElement();

            writer.WriteStartElement("Text");

            writer.WriteEndElement();

            writer.WriteStartElement("MetaData");

            writer.WriteEndElement();

            writer.WriteStartElement("DateTime");
            writer.WriteValue(notification.DateTimeStamp);
            writer.WriteEndElement();

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();

            byte[] payload = stream.ToArray();
            return payload;
        }

        internal static void SendNotification(Guid userId)
        {
            SendRawNotification(userId);
        }

        private static void SendRawNotification(Guid userId)
        {
            //send a push notification to the user. :)
            try
            {
                // Get the URI that the Microsoft Push Notification Service returns to the push client when creating a notification channel.
                // Normally, a web service would listen for URIs coming from the web client and maintain a list of URIs to send
                // notifications out to.

                Core.Cloud.TableStorage.InitializePushNotificationsTable();

                RawPushNotificationMessage rawPushNotificationMessage = new RawPushNotificationMessage(MessageSendPriority.High);

                //Prepare payload
                byte[] payload = PrepareNotificationRAWPayload(userId);

                rawPushNotificationMessage.RawData = payload;

                rawPushNotificationMessage.Send(new Uri(GetPushNotification(userId).URI, UriKind.RelativeOrAbsolute));

            }
            catch 
            {

            }

        }

        private static byte[] PrepareNotificationRAWPayload(Guid userId)
        {
            MemoryStream stream = new MemoryStream();

            XmlWriterSettings settings = new XmlWriterSettings() { Indent = true, Encoding = Encoding.UTF8 };
            XmlWriter writer = XmlTextWriter.Create(stream, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("NotificationUpdate");

            writer.WriteStartElement("NotificationCount");
            writer.WriteValue(Core.Notifications.Notification.GetUnreadNotificationCount(userId).ToString());
            writer.WriteEndElement();

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();

            byte[] payload = stream.ToArray();
            return payload;
        }


        private static byte[] PrepareBuzzCommentNotificationRAWPayload(Guid userId, Guid buzzId)
        {
            MemoryStream stream = new MemoryStream();

            XmlWriterSettings settings = new XmlWriterSettings() { Indent = true, Encoding = Encoding.UTF8 };
            XmlWriter writer = XmlTextWriter.Create(stream, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("BuzzCommentUpdate");

            writer.WriteStartElement("BuzzID");
            writer.WriteValue(buzzId);
            writer.WriteEndElement();

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();

            byte[] payload = stream.ToArray();
            return payload;
        }

        internal static void SendBuzzCommentNotification(Guid userId, Guid buzzId)
        {
            ////send a push notification to the user. :)
            //try
            //{
            //    // Get the URI that the Microsoft Push Notification Service returns to the push client when creating a notification channel.
            //    // Normally, a web service would listen for URIs coming from the web client and maintain a list of URIs to send
            //    // notifications out to.

            //    Core.Cloud.TableStorage.InitializePushNotificationsTable();

            //    RawPushNotificationMessage rawPushNotificationMessage = new RawPushNotificationMessage(MessageSendPriority.High);

            //    //Prepare payload
            //    byte[] payload = PrepareBuzzCommentNotificationRAWPayload(userId, buzzId);

            //    rawPushNotificationMessage.RawData = payload;

            //    rawPushNotificationMessage.Send(new Uri(GetPushNotification(userId).URI, UriKind.RelativeOrAbsolute));

            //}
            //catch 
            //{

            //}
        }

        internal static void SendTextRealTimeNotification(Guid toId, Model.Text txt)
        {
            //send a push notification to the user. :)
            try
            {
                // Get the URI that the Microsoft Push Notification Service returns to the push client when creating a notification channel.
                // Normally, a web service would listen for URIs coming from the web client and maintain a list of URIs to send
                // notifications out to.


                Core.Cloud.TableStorage.InitializePushNotificationsTable();

                RawPushNotificationMessage rawPushNotificationMessage = new RawPushNotificationMessage(MessageSendPriority.High);

                //Prepare payload
                byte[] payload = PrepareMessageRealTimeNotificationRAWPayload(toId, txt);

                rawPushNotificationMessage.RawData = payload;
                Model.PushNotification push=GetPushNotification(toId);
                MessageSendResult res= rawPushNotificationMessage.Send(new Uri(push.URI, UriKind.RelativeOrAbsolute));

            }
            catch 
            {

            }
        }

        private static byte[] PrepareMessageRealTimeNotificationRAWPayload(Guid toId, Model.Text txt)
        {
            MemoryStream stream = new MemoryStream();

            XmlWriterSettings settings = new XmlWriterSettings() { Indent = true, Encoding = Encoding.UTF8 };
            XmlWriter writer = XmlTextWriter.Create(stream, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("MessageRealTime");

            writer.WriteStartElement("TextID");
            writer.WriteValue(txt.TextID.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("MarkAsRead");
            writer.WriteValue(txt.MarkAsRead);
            writer.WriteEndElement();

            writer.WriteStartElement("DateTimeStamp");
            writer.WriteValue(txt.DateTimeStamp);
            writer.WriteEndElement();

            writer.WriteStartElement("TextMessage");
            writer.WriteValue(txt.TextMessage);
            writer.WriteEndElement();

            Core.Accounts.UserData data = Core.Accounts.User.GetUserData(txt.UserFrom);


            writer.WriteStartElement("UserID");
            writer.WriteValue(data.User.UserID.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("ProfilePicURL");
            writer.WriteValue(data.Profile.ProfilePicURL);
            writer.WriteEndElement();

            writer.WriteStartElement("UserName");
            writer.WriteValue(data.Profile.Name);
            writer.WriteEndElement();

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();

            byte[] payload = stream.ToArray();
            return payload;
        }

        internal static void SendNoOfUnreadNotification(Guid toId)
        {
            //send a push notification to the user. :)
            try
            {
                // Get the URI that the Microsoft Push Notification Service returns to the push client when creating a notification channel.
                // Normally, a web service would listen for URIs coming from the web client and maintain a list of URIs to send
                // notifications out to.

                Core.Cloud.TableStorage.InitializePushNotificationsTable();

                RawPushNotificationMessage rawPushNotificationMessage = new RawPushNotificationMessage(MessageSendPriority.High);

                //Prepare payload
                byte[] payload = PrepareNoOfUnreadNotificationRAWPayload(toId);

                rawPushNotificationMessage.RawData = payload;

                rawPushNotificationMessage.Send(new Uri(GetPushNotification(toId).URI, UriKind.RelativeOrAbsolute));

            }
            catch 
            {

            }
        }

        private static byte[] PrepareNoOfUnreadNotificationRAWPayload(Guid toId)
        {
            MemoryStream stream = new MemoryStream();

            XmlWriterSettings settings = new XmlWriterSettings() { Indent = true, Encoding = Encoding.UTF8 };
            XmlWriter writer = XmlTextWriter.Create(stream, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("MessageUpdate");

            writer.WriteStartElement("MessageCount");
            writer.WriteValue(Core.Text.Text.GetNumberOfUnreadMessages(toId).ToString());
            writer.WriteEndElement();

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();

            byte[] payload = stream.ToArray();
            return payload;
        }

        internal static void SendWindows8TileNotification(Guid userId)
        {
            //get last unread notification . :)

            try
            {
                // Get the URI that the Microsoft Push Notification Service returns to the Push Client when creating a notification channel.
                // Normally, a web service would listen for URIs coming from the web client and maintain a list of URIs to send
                // notifications out to.

                Core.Cloud.TableStorage.InitializePushNotificationsTable();

                Model.PushNotification pn = GetPushNotification(userId);


                if (pn == null)
                {
                    return;

                }

                if (pn.URI == null)
                {
                    return;
                }

                string subscriptionUri = pn.URI.ToString();


                Model.Notifications notification = GetLastNotification(userId);




                if (notification != null)
                {

                    HttpWebRequest sendNotificationRequest = (HttpWebRequest)WebRequest.Create(subscriptionUri);

                    // Create an HTTPWebRequest that posts the Tile notification to the Microsoft Push Notification Service.
                    // HTTP POST is the only method allowed to send the notification.
                    sendNotificationRequest.Method = "POST";

                    // The optional custom header X-MessageID uniquely identifies a notification message. 
                    // If it is present, the same value is returned in the notification response. It must be a string that contains a UUID.
                    // sendNotificationRequest.Headers.Add("X-MessageID", "<UUID>");
                    string tileMessage = "";
                    if (!(bool)notification.MarkAsRead)
                    {
                        //send a tile notification.

                        tileMessage = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                       "<wp:Notification xmlns:wp=\"WPNotification\" Version=\"2.0\">" +
                         "<wp:Tile Template=\"IconicTile\">" +
                           "<wp:SmallIconImage>Images\\SmallHat.png</wp:SmallIconImage>" +
                           "<wp:IconImage>Images\\MedHat.png</wp:IconImage>";

                            if (notification.Title == null || notification.Title == "")
                            {
                                tileMessage += "<wp:WideContent1  Action=\"Clear\"></wp:WideContent1>";
                            }
                            else
                            {
                                tileMessage += "<wp:WideContent1>" + notification.Title + "</wp:WideContent1>";
                            }

                            if (notification.Description == null || notification.Description == "")
                            {
                                tileMessage += "<wp:WideContent2  Action=\"Clear\"></wp:WideContent2>";
                            }
                            else
                            {
                                tileMessage += "<wp:WideContent2>" + notification.Description + "</wp:WideContent2>";
                            }
                           
                           tileMessage+="<wp:WideContent3></wp:WideContent3>" +
                           "<wp:Count>" + Core.Notifications.Notification.GetUnreadNotificationCount(userId) + "</wp:Count>" +
                           "<wp:Title>Zoik it!</wp:Title>" +
                           "<wp:BackgroundColor></wp:BackgroundColor>" +
                         "</wp:Tile>" +
                       "</wp:Notification>";


                        

                    }
                    else
                    {
                        //send empy tile. :)

                        tileMessage = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                      "<wp:Notification xmlns:wp=\"WPNotification\" Version=\"2.0\">" +
                        "<wp:Tile Template=\"IconicTile\">" +
                         "<wp:SmallIconImage>Images\\SmallHat.png</wp:SmallIconImage>" +
                          "<wp:IconImage>Images\\MedHat.png</wp:IconImage>" +
                          "<wp:WideContent1  Action=\"Clear\"></wp:WideContent1>" +
                          "<wp:WideContent2  Action=\"Clear\"></wp:WideContent2>" +
                          "<wp:WideContent3 Action=\"Clear\"></wp:WideContent3>" +
                          "<wp:Count  Action=\"Clear\"></wp:Count>" +
                          "<wp:Title>Zoik it!</wp:Title>" +
                          "<wp:BackgroundColor></wp:BackgroundColor>" +
                        "</wp:Tile>" +
                      "</wp:Notification>";

                    }




                    // Set the notification payload to send.
                    byte[] notificationMessage = Encoding.Default.GetBytes(tileMessage);

                    // Set the web request content length.
                    sendNotificationRequest.ContentLength = notificationMessage.Length;
                    sendNotificationRequest.ContentType = "text/xml";
                    sendNotificationRequest.Headers.Add("X-WindowsPhone-Target", "token");
                    sendNotificationRequest.Headers.Add("X-NotificationClass", "1");


                    using (Stream requestStream = sendNotificationRequest.GetRequestStream())
                    {
                        requestStream.Write(notificationMessage, 0, notificationMessage.Length);
                    }

                    // Send the notification and get the response.
                    HttpWebResponse response = (HttpWebResponse)sendNotificationRequest.GetResponse();
                    string notificationStatus = response.Headers["X-NotificationStatus"];
                    string notificationChannelStatus = response.Headers["X-SubscriptionStatus"];
                    string deviceConnectionStatus = response.Headers["X-DeviceConnectionStatus"];

                    // Display the response from the Microsoft Push Notification Service.  
                    // Normally, error handling code would be here. In the real world, because data connections are not always available,
                    // notifications may need to be throttled back if the device cannot be reached.


                }


            }
            catch 
            {

            }

            //push it in a tile and shoot. :)
        }

        private static Model.Notifications GetLastNotification(Guid userId)
        {

            Core.Cloud.TableStorage.InitializeNotificationsTable();

            TableQuery<Model.Notifications> n = new TableQuery<Model.Notifications>().Where(TableQuery.CombineFilters(
            TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.Notifications.GetPartitionKey(userId)), TableOperators.And, TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId))).Take(1);




            Model.Notifications notification = Model.Table.Notifications.ExecuteQuery(n).FirstOrDefault();
            return notification;
        }


        public static void SendToastNotification(Guid userId, string text1, string text2, string navigate)
        {
            try
            {

                //get last unread notification . :)


                Core.Cloud.TableStorage.InitializePushNotificationsTable();

                Model.PushNotification pn = GetPushNotification(userId);


                if (pn == null)
                {
                    return;

                }

                if (pn.URI == null)
                {
                    return;
                }

                string subscriptionUri = pn.URI.ToString();


                Model.Notifications notification = GetLastNotification(userId);
                // Get the URI that the Microsoft Push Notification Service returns to the push client when creating a notification channel.
                // Normally, a web service would listen for URIs coming from the web client and maintain a list of URIs to send
                // notifications out to.



                HttpWebRequest sendNotificationRequest = (HttpWebRequest)WebRequest.Create(subscriptionUri);

                // Create an HTTPWebRequest that posts the toast notification to the Microsoft Push Notification Service.
                // HTTP POST is the only method allowed to send the notification.
                sendNotificationRequest.Method = "POST";

                // The optional custom header X-MessageID uniquely identifies a notification message. 
                // If it is present, the same value is returned in the notification response. It must be a string that contains a UUID.
                // sendNotificationRequest.Headers.Add("X-MessageID", "<UUID>");

                // Create the toast message.
                string toastMessage = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<wp:Notification xmlns:wp=\"WPNotification\">" +
                   "<wp:Toast>" +
                        "<wp:Text1>" + text1 + "</wp:Text1>" +
                        "<wp:Text2>" + text2 + "</wp:Text2>" +
                        "<wp:Param>" + navigate + "</wp:Param>" +
                   "</wp:Toast> " +
                "</wp:Notification>";

                // Set the notification payload to send.
                byte[] notificationMessage = Encoding.Default.GetBytes(toastMessage);

                // Set the web request content length.
                sendNotificationRequest.ContentLength = notificationMessage.Length;
                sendNotificationRequest.ContentType = "text/xml";
                sendNotificationRequest.Headers.Add("X-WindowsPhone-Target", "toast");
                sendNotificationRequest.Headers.Add("X-NotificationClass", "2");


                using (Stream requestStream = sendNotificationRequest.GetRequestStream())
                {
                    requestStream.Write(notificationMessage, 0, notificationMessage.Length);
                }

                // Send the notification and get the response.
                HttpWebResponse response = (HttpWebResponse)sendNotificationRequest.GetResponse();
                string notificationStatus = response.Headers["X-NotificationStatus"];
                string notificationChannelStatus = response.Headers["X-SubscriptionStatus"];
                string deviceConnectionStatus = response.Headers["X-DeviceConnectionStatus"];

                // Display the response from the Microsoft Push Notification Service.  
                // Normally, error handling code would be here. In the real world, because data connections are not always available,
                // notifications may need to be throttled back if the device cannot be reached.

            }
            catch 
            {

            }

        }

        internal static void SendTextToastNotification(Guid toId, Model.Text txt)
        {
            if (txt != null && txt.UserFrom != null)
            {

                Model.UserProfile profile = Core.Accounts.User.GetUserProfile(txt.UserFrom);

                if (profile != null)
                {
                    SendToastNotification(toId, profile.Name, txt.TextMessage, "/Texts/UserText.xaml?id=" + profile.UserID);
                }
            }
        }

        internal static void ClearWindows8tile(Guid userId)
        {
            //get last unread notification . :)

            

            try
            {
                // Get the URI that the Microsoft Push Notification Service returns to the Push Client when creating a notification channel.
                // Normally, a web service would listen for URIs coming from the web client and maintain a list of URIs to send
                // notifications out to.

                Core.Cloud.TableStorage.InitializePushNotificationsTable();


                Model.PushNotification pn = GetPushNotification(userId);


                if (pn == null)
                {
                    return;

                }

                if (pn.URI == null)
                {
                    return;
                }

                string subscriptionUri = pn.URI.ToString();


                HttpWebRequest sendNotificationRequest = (HttpWebRequest)WebRequest.Create(subscriptionUri);

                // Create an HTTPWebRequest that posts the Tile notification to the Microsoft Push Notification Service.
                // HTTP POST is the only method allowed to send the notification.
                sendNotificationRequest.Method = "POST";

                // The optional custom header X-MessageID uniquely identifies a notification message. 
                // If it is present, the same value is returned in the notification response. It must be a string that contains a UUID.
                // sendNotificationRequest.Headers.Add("X-MessageID", "<UUID>");
                string tileMessage = "";


                //send empy tile. :)

                tileMessage = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
              "<wp:Notification xmlns:wp=\"WPNotification\" Version=\"2.0\">" +
                "<wp:Tile Template=\"IconicTile\">" +
                 "<wp:SmallIconImage>Images\\SmallHat.png</wp:SmallIconImage>" +
                  "<wp:IconImage>Images\\MedHat.png</wp:IconImage>" +
                  "<wp:WideContent1  Action=\"Clear\"></wp:WideContent1>" +
                  "<wp:WideContent2  Action=\"Clear\"></wp:WideContent2>" +
                  "<wp:WideContent3 Action=\"Clear\"></wp:WideContent3>" +
                  "<wp:Count  Action=\"Clear\"></wp:Count>" +
                  "<wp:Title>Zoik it!</wp:Title>" +
                  "<wp:BackgroundColor></wp:BackgroundColor>" +
                "</wp:Tile>" +
              "</wp:Notification>";


                // Set the notification payload to send.
                byte[] notificationMessage = Encoding.Default.GetBytes(tileMessage);

                // Set the web request content length.
                sendNotificationRequest.ContentLength = notificationMessage.Length;
                sendNotificationRequest.ContentType = "text/xml";
                sendNotificationRequest.Headers.Add("X-WindowsPhone-Target", "token");
                sendNotificationRequest.Headers.Add("X-NotificationClass", "1");


                using (Stream requestStream = sendNotificationRequest.GetRequestStream())
                {
                    requestStream.Write(notificationMessage, 0, notificationMessage.Length);
                }

                // Send the notification and get the response.
                HttpWebResponse response = (HttpWebResponse)sendNotificationRequest.GetResponse();
                string notificationStatus = response.Headers["X-NotificationStatus"];
                string notificationChannelStatus = response.Headers["X-SubscriptionStatus"];
                string deviceConnectionStatus = response.Headers["X-DeviceConnectionStatus"];

                // Display the response from the Microsoft Push Notification Service.  
                // Normally, error handling code would be here. In the real world, because data connections are not always available,
                // notifications may need to be throttled back if the device cannot be reached.

            }
            catch
            {

            }
        }
        
    }
}