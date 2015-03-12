using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Facebook;
using Newtonsoft.Json.Linq;

namespace Hangout.Web.Core.Tags
{
    public class FacebookTag
    {
        public static void RefreshTags(Guid userId)
        {
            Model.FacebookData user = Core.Accounts.Facebook.GetFacebookData(userId);

            Model.Location loc = Core.Location.UserLocation.GetLastLocation(userId);

            if(loc==null)
            {
                return;
            }

            if (user != null)
            {
                if (user.AccessToken != null)
                {
                    FacebookClient client = new FacebookClient(user.AccessToken);

                    string result = client.Get(@"/me/likes/").ToString();

                    JObject obj = JObject.Parse(result);

                    List<JToken> token = obj.SelectToken("data").ToList();

                    foreach (JToken t in token)
                    {
                        JToken tk = t.SelectToken("name").ToString().ToLower();

                        if (tk != null)
                        {
                            Model.Tag x = Core.Tags.Tags.InsertTagIfNotExists(tk.ToString().ToLower());

                            if (x != null)
                            {
                                Core.Tags.User.AddFacebookUserTags(userId, x.TagID,loc.CityID);
                            }

                        }
                    }
                }
            }
        }
    }
}