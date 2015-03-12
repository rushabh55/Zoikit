using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Core.Notifications
{
    public class Notification
    {
        public static void AddNotification(Guid userId, string title,string profilePic,string description,List<Guid> userIds,string param1,string param2,string type1,string type2,string metaData)
        {

            Core.Cloud.TableStorage.InitializeNotificationsTable();
            Core.Cloud.TableStorage.InitializeNotificationsUserTable();


            //add a notification.
            Model.Notifications n = new Model.Notifications(userId,title,description);
            n.DateTimeStamp = DateTime.Now;
            n.MarkAsRead = false;
            n.UserID=userId;
            n.Title = title;
            n.Description = description;
            n.Param1 = param1;
            n.Param2 = param2;
            n.Type1 = type1;
            n.Type2 = type2;
            n.MetaData = metaData;
            n.ProfilePicURL = profilePic;

            Model.Table.Notifications.Execute(TableOperation.Insert(n));

            if (userIds != null)
            {

                foreach (Guid id in userIds)
                {
                    Model.NotificationUser nu = new Model.NotificationUser(n.NotificationID,id);
                    nu.UserID = id;
                    nu.NotificationID=n.NotificationID;
                    Model.Table.NotificationsUser.Execute(TableOperation.Insert(nu));
                }
            }
           
           
            //send the notification down to devices. 
            PushNotifications.PushNotifications.SendWindows8TileNotification(userId);
            PushNotifications.PushNotifications.SendNotification(userId);
        }

        public static void AddNotification(List<Guid> userId, string title, string description, List<Guid> userIds, string param1, string param2, string type1, string type2,string metaData)
        {
            foreach (Guid id in userId)
            {
                AddNotification(id, title,"", description, userIds, param1, param2, type1, type2,metaData);
            }
        }

        public static void MarkAsRead(Guid notificationId)
        {

            Core.Cloud.TableStorage.InitializeNotificationsTable();

            Model.Notifications n = Model.Table.Notifications.ExecuteQuery(new TableQuery<Model.Notifications>().Where(TableQuery.GenerateFilterConditionForGuid("NotificationID", QueryComparisons.Equal,notificationId))).FirstOrDefault();
            if (n != null)
            {
                n.MarkAsRead = true;
            }

            Model.Table.Notifications.Execute(TableOperation.Replace(n));
        }

        public static List<Model.Notifications> GetNotifications(Guid userId, int take, List<Guid> skipList)
        {


            Core.Cloud.TableStorage.InitializeNotificationsTable();
           

            List<Model.Notifications> notifications= Model.Table.Notifications.ExecuteQuery(new TableQuery<Model.Notifications>().Where(TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal,Model.Notifications.GetPartitionKey(userId)),TableOperators.And,TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal,userId))).Take(Core.Smart.ListAlgo.TakeCount(take,skipList.Count))).Where(o=>!skipList.Contains(o.NotificationID)).ToList();

            foreach (Model.Notifications n in notifications)
            {
                if (!(bool)n.MarkAsRead)
                {
                    MarkAsRead(n.NotificationID);
                    n.MarkAsRead = false;
                }
               
            }

            return notifications;
        }

        public static void MarkAsRead(List<Guid> notiifcationIds)
        {
            foreach (Guid a in notiifcationIds)
            {
                MarkAsRead(a);
            }
        }

        public static bool? IsRead(Guid notificationId)
        {

            Core.Cloud.TableStorage.InitializeNotificationsTable();


            Model.Notifications n = GetNotification(notificationId);
            if (n == null)
            {
                return null;
            }

            return n.MarkAsRead;
        }

        public static Model.Notifications GetNotification(Guid notificationId)
        {
            Core.Cloud.TableStorage.InitializeNotificationsTable();

            return Model.Table.Notifications.ExecuteQuery(new TableQuery<Model.Notifications>().Where(TableQuery.GenerateFilterConditionForGuid("NotificationID", QueryComparisons.Equal, notificationId))).FirstOrDefault();
        }

        public static int GetUnreadNotificationCount(Guid userId)
        {

            Core.Cloud.TableStorage.InitializeNotificationsTable();


            return Model.Table.Notifications.ExecuteQuery(new TableQuery<Model.Notifications>().Where(TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.Notifications.GetPartitionKey(userId)), TableOperators.And, TableQuery.CombineFilters(TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId), TableOperators.And, TableQuery.GenerateFilterConditionForBool("MarkAsRead", QueryComparisons.Equal, false))))).Count();
        }

        public static void MarkAllNotificationsAsRead(Guid userId)
        {
            Core.Cloud.TableStorage.InitializeNotificationsTable();

            List<Model.Notifications> notification =Model.Table.Notifications.ExecuteQuery(new TableQuery<Model.Notifications>().Where(TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.Notifications.GetPartitionKey(userId)), TableOperators.And, TableQuery.CombineFilters(TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId), TableOperators.And, TableQuery.GenerateFilterConditionForBool("MarkAsRead", QueryComparisons.Equal, false))))).ToList();
            TableBatchOperation batchOperation = new TableBatchOperation();
            foreach (Model.Notifications n in notification)
            {
                n.MarkAsRead = true;
                batchOperation.Replace(n);
            }

            if(batchOperation.Count>0)
            {
                Model.Table.Notifications.ExecuteBatch(batchOperation);           
            }

            
        }

        internal static void SendBuzzCommentNotification(List<Guid> userIds, Guid buzzId)
        {
            foreach (Guid userId in userIds)
            {
                PushNotifications.PushNotifications.SendBuzzCommentNotification(userId,buzzId);
            }
        }

        internal static Model.Notifications GetLastUnreadNotification(Guid userId)
        {
            Core.Cloud.TableStorage.InitializeNotificationsTable();

            return Model.Table.Notifications.ExecuteQuery(new TableQuery<Model.Notifications>().Where(TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.Notifications.GetPartitionKey(userId)), TableOperators.And, TableQuery.CombineFilters(TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId), TableOperators.And, TableQuery.GenerateFilterConditionForBool("MarkAsRead", QueryComparisons.Equal, false)))).Take(1)).FirstOrDefault();
        }

        internal static IEnumerable<Model.User> GetNotificationUsers(Guid notificationId)
        {
            Core.Cloud.TableStorage.InitializeNotificationsUserTable();

            List<Guid> userIds = Model.Table.NotificationsUser.ExecuteQuery(new TableQuery<Model.NotificationUser>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, notificationId.ToString()))).Select(o => o.UserID).ToList();

            List<Model.User> users = new List<Model.User>();

            foreach (Guid id in userIds)
            {
                users.Add(Core.Accounts.User.GetUser(id));
            }

            return users;

        }

        internal static List<Model.Notifications> GetNewNotifications(Guid userId, List<Guid> skipList)
        {
            Core.Cloud.TableStorage.InitializeNotificationsTable();
           

            List<Model.Notifications> notifications= Model.Table.Notifications.ExecuteQuery(
                new TableQuery<Model.Notifications>().Where(
                TableQuery.CombineFilters(TableQuery.GenerateFilterConditionForBool("MarkAsRead", QueryComparisons.Equal,false),
                TableOperators.And,
                TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal,Model.Notifications.GetPartitionKey(userId)),
                TableOperators.And,TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal,userId))))).Where(o=>!skipList.Contains(o.NotificationID)).Distinct().ToList();

            foreach (Model.Notifications n in notifications)
            {
                if (!(bool)n.MarkAsRead)
                {
                    MarkAsRead(n.NotificationID);
                    n.MarkAsRead = false;
                }
               
            }

            return notifications;
        }
    }
}