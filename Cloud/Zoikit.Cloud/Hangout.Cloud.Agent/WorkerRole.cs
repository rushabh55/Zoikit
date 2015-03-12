using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Threading.Tasks;

namespace Hangout.Cloud.Agent
{
    public class WorkerRole : RoleEntryPoint
    {
        public override void Run()
        {
            
                //// This is a sample worker implementation. Replace with your logic.
                Agents.QueueReader queueReader = new Agents.QueueReader();
                Agents.FacebookAgent fb = new Agents.FacebookAgent();
                Agents.TwitterAgent ta = new Agents.TwitterAgent();

                
                Task.Factory.StartNew(() => ta.Run(), TaskCreationOptions.LongRunning);
                Task.Factory.StartNew(() => queueReader.Run(), TaskCreationOptions.LongRunning);
                Task.Factory.StartNew(() => fb.Run(), TaskCreationOptions.LongRunning);
                //Task.Factory.StartNew(() => scoreUpdate.Run(), TaskCreationOptions.LongRunning);
                //Task.Factory.StartNew(() => compatibilityAgent.Run(), TaskCreationOptions.LongRunning);
                ////Task.Factory.StartNew(() => NotificationAgent.Run(), TaskCreationOptions.LongRunning);

                System.Threading.Thread.Sleep(Timeout.Infinite);

           
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }
    }
}
