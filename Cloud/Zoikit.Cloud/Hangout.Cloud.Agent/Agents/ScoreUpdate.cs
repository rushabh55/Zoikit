using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Hangout.Cloud.Agent.Agents
{
    class ScoreUpdate
    {
        public void Run()
        {

            while (true)
            {
               
                try
                {
                    Trace.WriteLine("SU ENTERED");
                    Core.Score.ReduceInactiveUserScores();
                    Thread.Sleep(new TimeSpan(1, 0, 0, 0)); //start the same process after a day.
                }
                catch (Exception ex)
                {
                    Web.Core.Exceptions.ExceptionReporting.AddAnException(Guid.Empty, Web.Core.ClientType.WindowsAzure, ex);
                }
                   
            }
        }
    }
}
