using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Hangout.Client.Core.Authentication.Twitter
{
    public class OAuthClient
    {
        public static void PerformRequest(Dictionary<string, string> parameters, string url, string consumerSecret, string token, RequestType type)
        {
            string OAuthHeader = OAuthClient.GetOAuthHeader(parameters, "POST", url, consumerSecret, token);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Headers["Authorization"] = OAuthHeader;
            request.BeginGetResponse(new AsyncCallback(GetResponse), new object[] { request, type });
        }

        public static void GetToken(IAsyncResult result)
        {

            HttpWebRequest request = (HttpWebRequest)result.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);

            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string[] data = reader.ReadToEnd().Split(new char[] { '&' });
                int index = data[0].IndexOf("=");
                string token = data[0].Substring(index + 1, data[0].Length - index - 1);
                Debug.WriteLine("TOKEN OBTAINED");

                WebBrowserTask task = new WebBrowserTask();
                task.Uri = new Uri("https://api.twitter.com/oauth/authorize?oauth_token=" + token);
                task.Show();
            }
        }

        static void GetResponse(IAsyncResult result)
        {
            HttpWebRequest request = (HttpWebRequest)(((object[])result.AsyncState)[0]);
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);

            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                RequestType currentType = (RequestType)(((object[])result.AsyncState)[1]);
                string completeString = reader.ReadToEnd();

                switch (currentType)
                {
                    case RequestType.InitialToken:
                        {
                            string[] data = completeString.Split(new char[] { '&' });
                            int index = data[0].IndexOf("=");
                            App.TwitterToken = data[0].Substring(index + 1, data[0].Length - index - 1);
                            index = data[1].IndexOf("=");
                            App.TwitterTokenSecret = data[1].Substring(index + 1, data[1].Length - index - 1);

                            WebBrowserTask task = new WebBrowserTask();
                            task.Uri = new Uri("https://api.twitter.com/oauth/authorize?oauth_token=" + App.TwitterToken);
                            task.Show();
                            break;
                        }

                    case RequestType.AccessToken:
                        {
                            string[] data = completeString.Split(new char[] { '&' });
                            int index = data[0].IndexOf("=");
                            App.TwitterToken = data[0].Substring(index + 1, data[0].Length - index - 1);
                            index = data[1].IndexOf("=");
                            App.TwitterTokenSecret = data[1].Substring(index + 1, data[1].Length - index - 1);
                            index = data[2].IndexOf("=");
                            App.TwitterUserID = data[2].Substring(index + 1, data[2].Length - index - 1);
                            index = data[3].IndexOf("=");
                            App.TwitterUsername = data[3].Substring(index + 1, data[3].Length - index - 1);
                            break;
                        }
                }
            }
        }

        public static string GetOAuthHeader(Dictionary<string, string> parameters, string httpMethod, string url, string consumerSecret, string tokenSecret)
        {
            parameters = parameters.OrderBy(x => x.Key).ToDictionary(v => v.Key, v => v.Value);

            string concat = string.Empty;

            string OAuthHeader = "OAuth ";
            foreach (string k in parameters.Keys)
            {
                concat += k + "=" + parameters[k] + "&";
                OAuthHeader += k + "=" + "\"" + parameters[k] + "\", ";
            }

            concat = concat.Remove(concat.Length - 1, 1);
            concat = StringHelper.EncodeToUpper(concat);

            concat = httpMethod + "&" + StringHelper.EncodeToUpper(url) + "&" + concat;

            byte[] content = Encoding.UTF8.GetBytes(concat);

            HMACSHA1 hmac = new HMACSHA1(Encoding.UTF8.GetBytes(consumerSecret + "&" + tokenSecret));
            hmac.ComputeHash(content);

            string hash = Convert.ToBase64String(hmac.Hash);
            hash = hash.Replace("-", "");

            OAuthHeader += "oauth_signature=\"" + StringHelper.EncodeToUpper(hash) + "\"";

            return OAuthHeader;
        }
    }

    public enum RequestType
    {
        InitialToken,AccessToken
    }
}


