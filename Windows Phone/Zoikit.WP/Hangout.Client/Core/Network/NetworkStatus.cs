using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

namespace Hangout.Client.Core.Network
{
    class NetworkStatus
    {
        public static bool IsNetworkAvailable()
        {
            var available = NetworkInterface.GetIsNetworkAvailable();

            return available;
        }
    }
}
