using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Hangout.Web.Core.Cloud
{
    public class Queue
    {
        public static void AddMessage(string message)
        {
            
                   
            //adding message to the queue for the worker role to finish processing
            var storageAccount = CloudStorageAccount.Parse(Settings.CloudStorageString);
            var queueClient = storageAccount.CreateCloudQueueClient();
            var queue = queueClient.GetQueueReference("hangoutqueue");
            queue.CreateIfNotExists();
            var msg = new CloudQueueMessage(message); //asking the worker to refresh likes :)
            queue.AddMessage(msg);
        }

        public static void GetCompatibility(int p)
        {
            Core.Cloud.Queue.AddMessage("Compatibility:" + p);
        }

        public static void AddTrophyRefreshTextToCloudQueue(Guid userId)
        {
            Core.Cloud.Queue.AddMessage("Trophy:" + userId);
        }

        public static void AddCloudHangoutInsertedMessage(Guid userId, int buzzId)
        {
            Cloud.Queue.AddMessage("HangoutAdded:" + buzzId + ":User:" + userId);
        }

        public static void RefreshFacebookLikes(Guid userId)
        {
            Core.Cloud.Queue.AddMessage("RefreshLikes:" + userId);
        }
        
    }
}