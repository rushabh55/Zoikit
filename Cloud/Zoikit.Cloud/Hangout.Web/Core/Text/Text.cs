using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Core.Text
{
    public class Text
    {
        public static  List<Model.Text> GetUserText(Guid fromId, Guid toId, List<Guid> skipList, int take)
        {

            Web.Core.Cloud.TableStorage.InitializeTextTable();
            TableQuery<Model.Text> query = new TableQuery<Model.Text>().Where(
                   TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.Text.GetPartitionKey(fromId, toId)));

            query.TakeCount=Core.Smart.ListAlgo.TakeCount(take,skipList.Count());


             return Model.Table.Text.ExecuteQuery(
             query).Where(o=>!skipList.Contains(o.TextID)).OrderBy(o=>o.DateTimeStamp).ToList();
        }

        public static List<Model.Text> GetTextsAfter(Guid fromId, Guid toId, Guid textId)
        {


            Core.Cloud.TableStorage.InitializeTextTable();

            Model.Text t = Model.Table.Text.ExecuteQuery(
                new TableQuery<Model.Text>().Where(
                TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.Text.GetPartitionKey(fromId, toId)),
                    TableOperators.And,
                      TableQuery.CombineFilters(TableQuery.CombineFilters(

                       TableQuery.CombineFilters(
                        TableQuery.GenerateFilterConditionForGuid("UserFrom", QueryComparisons.Equal, fromId),
                        TableOperators.And,
                            TableQuery.GenerateFilterConditionForGuid("UserTo", QueryComparisons.Equal, toId)),

                            TableOperators.Or,

                            TableQuery.CombineFilters(
                        TableQuery.GenerateFilterConditionForGuid("UserFrom", QueryComparisons.Equal, toId),
                        TableOperators.And,
                            TableQuery.GenerateFilterConditionForGuid("UserTo", QueryComparisons.Equal, fromId))),


                            TableOperators.And,
                             TableQuery.GenerateFilterConditionForGuid("TextID", QueryComparisons.Equal, textId))))).FirstOrDefault();


            if(t==null)
            {
                return new List<Model.Text>();
            }

            return Model.Table.Text.ExecuteQuery(
                new TableQuery<Model.Text>().Where(
                TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.Text.GetPartitionKey(fromId, toId)),
                    TableOperators.And,
                      TableQuery.CombineFilters(TableQuery.CombineFilters(

                       TableQuery.CombineFilters(
                        TableQuery.GenerateFilterConditionForGuid("UserFrom", QueryComparisons.Equal, fromId),
                        TableOperators.And,
                            TableQuery.GenerateFilterConditionForGuid("UserTo", QueryComparisons.Equal, toId)),
                            
                            TableOperators.Or,

                            TableQuery.CombineFilters(
                        TableQuery.GenerateFilterConditionForGuid("UserFrom", QueryComparisons.Equal, toId),
                        TableOperators.And,
                            TableQuery.GenerateFilterConditionForGuid("UserTo", QueryComparisons.Equal, fromId))) ,


                            TableOperators.And,
                             TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.LessThan, t.RowKey))))).OrderBy(o => o.DateTimeStamp).ToList();


            
        }

        public static List<Model.Text> GetText(Guid userId, List<Guid> skipList,int take)
        {
            UserDistint d = new UserDistint();

            Core.Cloud.TableStorage.InitializeTextTable();

            return  Model.Table.Text.ExecuteQuery(
                new TableQuery<Model.Text>().Where(
                TableQuery.CombineFilters(
                TableQuery.GenerateFilterConditionForGuid("UserTo", QueryComparisons.Equal, userId),
                TableOperators.Or,
                TableQuery.GenerateFilterConditionForGuid("UserFrom", QueryComparisons.Equal, userId)))).Where(o=>!skipList.Contains(o.UserFrom)||!skipList.Contains(o.UserTo)).Distinct(d).Take(take).ToList();

            
        }

        public static TextData SendText(Guid fromId, Guid toId, string text)
        {
            try
            {

                TextData data = new TextData();
                Core.Cloud.TableStorage.InitializeTextTable();

                Model.Text t = new Model.Text(fromId, toId, text);
                t.UserTo = toId;
                t.UserFrom = fromId;
                t.TextMessage = text;
                t.DateTimeStamp = DateTime.Now;
                t.MarkAsRead = false;

                Model.Table.Text.Execute(TableOperation.Insert(t));


                return new TextData { Status = TextSentStatus.Saved, Text = t };
            }
            catch(Exception e)
            {
                Core.Exceptions.ExceptionReporting.AddAnException(fromId, ClientType.WindowsAzure, e);
                return new TextData { Status = TextSentStatus.Error, Text = null };

            }

        }

        public static void MarkAsRead(Guid fromId,Guid toId)
        {

            Core.Cloud.TableStorage.InitializeTextTable();

            List<Model.Text> texts = Model.Table.Text.ExecuteQuery(
                new TableQuery<Model.Text>().Where(
                TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.Text.GetPartitionKey(fromId, toId)),
                TableOperators.And,
                TableQuery.CombineFilters(TableQuery.GenerateFilterConditionForGuid("UserFrom", QueryComparisons.Equal, fromId),
                TableOperators.And,
                TableQuery.CombineFilters(TableQuery.GenerateFilterConditionForBool("MarkAsRead", QueryComparisons.Equal, false),
                TableOperators.And,
                TableQuery.GenerateFilterConditionForGuid("UserTo", QueryComparisons.Equal, toId)))))).ToList();

            TableBatchOperation batch = new TableBatchOperation();

            foreach (Model.Text t in texts)
            {
                t.MarkAsRead = true;

                batch.Replace(t);
            }


            if(batch.Count>0)
            {
                Model.Table.Text.ExecuteBatch(batch);
            }
           

        }

        public static void MarkAllAsRead(Guid userId)
        {

            Core.Cloud.TableStorage.InitializeTextTable();

            List<Model.Text> texts = Model.Table.Text.ExecuteQuery(
                new TableQuery<Model.Text>().Where(
                TableQuery.CombineFilters(TableQuery.GenerateFilterConditionForGuid("UserTo", QueryComparisons.Equal, userId),
                TableOperators.And,
                TableQuery.GenerateFilterConditionForBool("MarkAsRead", QueryComparisons.Equal, false)))).ToList();

            TableBatchOperation batch = new TableBatchOperation();

            foreach (Model.Text t in texts)
            {
                t.MarkAsRead = true;

                batch.Replace(t);
            }

            if(batch.Count>0)
            {
                Model.Table.Text.ExecuteBatch(batch);
            }

            

            
        }

        internal static int GetNumberOfUnreadMessages(Guid userId)
        {
            Core.Cloud.TableStorage.InitializeTextTable();

            return Model.Table.Text.ExecuteQuery(
                new TableQuery<Model.Text>().Where(
                TableQuery.CombineFilters(TableQuery.GenerateFilterConditionForGuid("UserTo", QueryComparisons.Equal, userId),
                TableOperators.And,
                TableQuery.GenerateFilterConditionForBool("MarkAsRead", QueryComparisons.Equal, false)))).Count();
        }

        internal static void SendTextNotifications(Guid toId,Model.Text txt)
        {

            Core.Cloud.TableStorage.InitializeNotificationsTable();
            Core.Cloud.TableStorage.InitializeTextTable();

            SendTextMainNotification(toId);


            //then add a new message notification with all the users. :)

            //Send a payload message
            Core.PushNotifications.PushNotifications.SendTextRealTimeNotification(toId, txt);


            //send text toast notification. :)
            Core.PushNotifications.PushNotifications.SendTextToastNotification(toId, txt);


            //Now send no of unread message Notification :)

            Core.PushNotifications.PushNotifications.SendNoOfUnreadNotification(toId);

            


        }

        public static void SendTextMainNotification(Guid toId)
        {

            Core.Cloud.TableStorage.InitializeNotificationsTable();
            Core.Cloud.TableStorage.InitializeTextTable();
            Core.Cloud.TableStorage.InitializeNotificationsUserTable();

            //delete all message notification first.
            List<Model.Notifications> notification = Model.Table.Notifications.ExecuteQuery(
                new TableQuery<Model.Notifications>().Where(
                TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.Notifications.GetPartitionKey(toId)),
                TableOperators.And,
                TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("Type2", QueryComparisons.Equal, "Message"),
                TableOperators.And,
                TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, toId))))).ToList();

            


            TableBatchOperation notificationBatch=new TableBatchOperation();
            TableBatchOperation notificationUserBatch = new TableBatchOperation();

            

            foreach(Model.Notifications a in notification)
            {
                //get all UserNotifcations.

                List<Model.NotificationUser> userNot = Model.Table.NotificationsUser.ExecuteQuery(
                new TableQuery<Model.NotificationUser>().Where(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.NotificationUser.GetPartitionKey(a.NotificationID)))).ToList();

                //batch them to delete

                userNot.ForEach(x => notificationUserBatch.Delete(x));

                if(notificationUserBatch.Count>0)
                {
                    Model.Table.NotificationsUser.ExecuteBatch(notificationUserBatch);
                }
            }
            
            
            notification.ForEach(o => notificationBatch.Delete(o));

            if (notificationBatch.Count > 0)
            {

                Model.Table.Notifications.ExecuteBatch(notificationBatch);
            }


            //now send text main notification .:)

            int unreadCount = Core.Text.Text.GetNumberOfUnreadMessages(toId);

            List<Guid> userIds =Model.Table.Text.ExecuteQuery(
                new TableQuery<Model.Text>().Where(
                TableQuery.CombineFilters(TableQuery.GenerateFilterConditionForGuid("UserTo", QueryComparisons.Equal, toId),
                TableOperators.And,
                TableQuery.GenerateFilterConditionForBool("MarkAsRead", QueryComparisons.Equal, false)))).Select(o => o.UserFrom).Distinct().ToList();

            if (unreadCount > 0)
            {
                if (userIds.Count > 0)
                {
                    if (userIds.Count > 1)
                    {
                       Guid lastId=userIds.FirstOrDefault();
                        Model.UserProfile profile=Core.Accounts.User.GetUserProfile(lastId);
                        Core.Notifications.Notification.AddNotification(toId, "You have New Messages","", "<bold> " + profile.Name + " </bold> and <bold> " + (userIds.Count - 1) + " other people </bold> have sent you new messages", userIds, "", "", "", "Message", "");
                    }
                    else
                    {
                        userIds.Reverse();
                        userIds = userIds.Take(5).ToList();
                        Guid lastId = userIds.FirstOrDefault();
                        Model.UserProfile profile = Core.Accounts.User.GetUserProfile(lastId);
                        Core.Notifications.Notification.AddNotification(toId, "You have a New Message", "", "<bold> " + profile.Name + " </bold> has sent you a message", userIds, lastId.ToString(), "", "User", "Message", "");
                    }
                }
            }
            else
            {
                    //send no messages. Notification count. :)
                    Core.PushNotifications.PushNotifications.SendNotification(toId);
            }


        }
    }
}