using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Core.Smart
{
    public class ListAlgo
    {

        public static int TakeCount(int takeCount, int skipCount)
        {
            return takeCount + skipCount;
        }

        public static int SkipCount(int takeCount, int SkipCount)
        {
            return 0;
        }
    }
}