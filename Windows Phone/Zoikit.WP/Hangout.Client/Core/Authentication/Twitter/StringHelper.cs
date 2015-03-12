using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Hangout.Client.Core.Authentication.Twitter
{
        public static class StringHelper
        {
            public static string EncodeToUpper(string raw)
            {
                raw = HttpUtility.UrlEncode(raw);
                return Regex.Replace(raw, "(%[0-9a-f][0-9a-f])", c => c.Value.ToUpper());
            }

            public static string UNIXTimestamp
            {
                get
                {
                    return Convert.ToString((int)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds);
                }
            }
        }
}
