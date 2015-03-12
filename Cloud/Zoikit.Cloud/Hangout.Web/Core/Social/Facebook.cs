using Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Hangout.Web.Core.Social
{
    public class Facebook
    {
        public static  void ShareOnFacebook(string accessTag, string Status)
        {
            try
            {
                var client = new FacebookClient(accessTag);

                Dictionary<string, object> parameters=new Dictionary<string,object>();

                parameters["message"] = Status;
                client.Post("me/feed", parameters);
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.AddAnException(Guid.Empty, Web.Core.ClientType.WindowsAzure, ex);
            }


        }

        internal static void CheckFacebookFollow(Guid userId, Guid followuserId)
        {
            try
            {
                Model.FacebookData data1 = Core.Accounts.Facebook.GetFacebookData(userId);


                if (data1 != null)
                {
                    Model.FacebookData data2 = Core.Accounts.Facebook.GetFacebookData(followuserId);

                    if (data2 != null)
                    {
                        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(@"https://graph.facebook.com/me/og.follows?access_token=" + data1.AccessToken + "&method=POST&profile=" + @"http://www.facebook.com/" + data2.FacebookID);
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();


                    }
                }
            }

            catch 
            {

            }
        }
    }
}