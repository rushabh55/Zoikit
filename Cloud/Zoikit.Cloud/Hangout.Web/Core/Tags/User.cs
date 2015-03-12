using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Core.Tags
{
    public class User
    {
        public static List<Model.Tag> MutualTagsFollowed(Guid user1Id, Guid user2Id)
        {
           return  Core.Tags.Follow.GetTagsFollowing(user1Id).Where(o=>Core.Tags.Follow.GetTagsFollowing(user1Id).Contains(o)).ToList();
        }

        public static int GetNoOfMutualTagsFollowed(Guid user1Id, Guid user2Id)
        {
            return MutualTagsFollowed(user1Id, user2Id).Count();
        }

        public static List<Model.Tag> UnMutualTagsFollowed(Guid user1Id, Guid user2Id)
        {
            return Core.Tags.Follow.GetTagsFollowing(user1Id).Where(o => !Core.Tags.Follow.GetTagsFollowing(user1Id).Contains(o)).ToList();
        }


        public static List<Model.Tag> GetRelatedFollowingTags(Guid userId, Guid relateduserId, int count)
        {

            Random r = new Random();

            //take 70% of mutual tokens
            int m = (count * 70) / 100;
            //take 30% of unmutual tokens
            int um = (count * 30) / 100;
            List<Model.Tag> list = MutualTagsFollowed(userId, relateduserId).OrderBy(o => r.Next()).Take(m).ToList();

            list.AddRange(UnMutualTagsFollowed(userId, relateduserId).OrderBy(o => r.Next()).Take(um).ToList());


            if (count > list.Count())
            {
                int itemsNeeded = count - list.Count();

                //get all categories followed.

                List<Model.Tag> followed = Core.Tags.Follow.GetTagsFollowing(userId);

                //add items from the followed list to the "list" which are not ther ein the list

                List<Model.Tag> extra = followed.Where(o => !list.Contains(o)).OrderBy(o => r.Next()).Take(itemsNeeded).ToList();

                list.AddRange(extra);
            }


            return list;
        }


        internal static void AddFacebookUserTags(Guid userId, Guid tokenId,Guid cityId)
        {
            Core.Tags.Follow.FollowTag(userId, tokenId, cityId,false);
        }
    }
}