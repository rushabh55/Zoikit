using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Hangout.Cloud.Agent.Agents
{
    class CompatibilityAgent
    {
        public void Run()
        {
            //check compatibility everyday :)
            while (true)
            {
                //get all the users with outdated compatibility
               
                try
                {
                    Trace.WriteLine("CA Entered");

                    List<Guid> ids = Web.Core.BackgroundData.UserBackgroundData.GetOutDatedCompatibilityScoreuserIds(new TimeSpan(1, 0, 0, 0));

                    Trace.WriteLine("COUNT = " + ids.Count);

                    foreach (Guid id in ids)
                    {
                        Trace.WriteLine("Ids = " + id);
                        UpdateCompatibility(id);
                    }
                }
                
                catch(Exception ex)
                {
                    Web.Core.Exceptions.ExceptionReporting.AddAnException(Guid.Empty,Web.Core.ClientType.WindowsAzure,ex);
                }
               

                Thread.Sleep(new TimeSpan(1, 0, 0, 0)); //sleep for a day :)
            }
        }

        public static void UpdateCompatibility(Guid id)
        {
            Web.Core.Compatibility.Compatibility.UpdateCompatibility(id);
            Web.Core.BackgroundData.UserBackgroundData.UpdateCompatibilityDateTimeStamp(id);
        }
    }
}
