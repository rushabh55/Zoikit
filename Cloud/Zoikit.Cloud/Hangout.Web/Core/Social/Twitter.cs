using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using TweetSharp;

namespace Hangout.Web.Core.Social
{
    public class Twitter
    {
        public static void PostTweet(string accesstoken, string accessTagSecret, string status)
        {
            try
            {
                if (status.Count() <= 130)
                {
                    TwitterService service = new TwitterService("1i9gU4XCDgwrLrFwIvtQ", "79COrlDe7Kp1jH35FNs8cz8CvTNK2ewVoNoiuuHR6uM");
                    service.AuthenticateWith(accesstoken, accessTagSecret);

                    //Posting a Tweet
                    service.SendTweet(new SendTweetOptions { Status = status }, (tweet, response) =>
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            //Tweet posted!
                        }
                        else
                        {
                            //Nah something went wrong! 
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.AddAnException(Guid.Empty, Web.Core.ClientType.WindowsAzure, ex);
            }
        }
    }
}