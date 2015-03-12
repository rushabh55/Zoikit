using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace Hangout.Services
{
    public class WebRole : RoleEntryPoint
    {
        public override bool OnStart()
        {

            

            // To enable the AzureLocalStorageTraceListner, uncomment relevent section in the web.config  
           

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }
    }
}
