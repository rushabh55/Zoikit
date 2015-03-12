using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.Discover
{
    public class Discover
    {
        public static List<Objects.Discover.DiscoverObj> GetItems(Guid userId, int take, List<Guid> skipList, Guid cityId, string zat)
        {
           


            Services.Users.User u=new Users.User();
            Services.Tag.Tag t = new Tag.Tag();
            //get users.


            int userRand = new Random().Next((int)(take*0.7));


            List<Guid> userIds= Core.Users.Users.GetLocalUsers(userId, cityId, userRand, skipList);

            List<Web.Services.Objects.Discover.DiscoverObj> disobj = new List<Web.Services.Objects.Discover.DiscoverObj>();

            foreach(Guid id in userIds)
            {

               Web.Services.Objects.Discover.DiscoverObj o = new Objects.Discover.DiscoverObj();
               o.User=u.ConvertUserByID(userId,id);

               disobj.Add(o);
            }

            //get tags.
            int tagRand = take - userRand;

            List<Guid> tagId = Core.Tags.Follow.DiscoverLocalTags(userId, tagRand, cityId, skipList);

            foreach(Guid i in tagId)
            {
                Web.Services.Objects.Discover.DiscoverObj o = new Objects.Discover.DiscoverObj();
                o.Tag=t.ConvertUserTag(userId, i, cityId);

                disobj.Add(o);
            }

           

            //shoot.

            return disobj.OrderBy(o => new Random().Next()).ToList();
        }
    }
}