using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Core.Category
{
    public class User
    {
        public static List<Model.Category> MutualCategoriesFollowed(Guid user1Id, Guid user2Id)
        {
            List<Model.Category> FollowedByUser1 = Core.Category.Follow.GetCategoryFollowing(user1Id);
            List<Model.Category> FollowedByUser2 = Core.Category.Follow.GetCategoryFollowing(user2Id);

            return FollowedByUser1.Where(o => FollowedByUser2.Contains(o)).ToList();
        }

        public static List<Model.Category> UnMutualCategoriesFollowed(Guid user1Id, Guid user2Id)
        {
            List<Model.Category> FollowedByUser1 = Core.Category.Follow.GetCategoryFollowing(user1Id);
            List<Model.Category> FollowedByUser2 = Core.Category.Follow.GetCategoryFollowing(user2Id);

            return FollowedByUser1.Where(o => !FollowedByUser2.Contains(o)).ToList();
        }

        public static int GetNoOfMutualCategoriesFollowed(Guid user1Id, Guid user2Id)
        {
            return MutualCategoriesFollowed(user1Id, user2Id).Count();
        }

        public static List<Model.Category> GetRelatedFollowingCategory(Guid userId, Guid relateduserId, int count)
        {

            Random r=new Random();

            //take 70% of mutual categories
            int m=(count*60)/100;
            //take 30% of unmutual categories
            int um=(count*40)/100;
            List<Model.Category> list = MutualCategoriesFollowed(userId, relateduserId).OrderBy(o=>r.Next()).Take(m).ToList();

            list.AddRange(UnMutualCategoriesFollowed(userId, relateduserId).OrderBy(o => r.Next()).Take(um).ToList());


            if (count > list.Count())
            {
                int itemsNeeded = count - list.Count();

                //get all categories followed.

                List<Model.Category> followed = Core.Category.Follow.GetCategoryFollowing(userId);

                //add items from the followed list to the "list" which are not ther ein the list

                List<Model.Category> extra = followed.Where(o => !list.Contains(o)).OrderBy(o => r.Next()).Take(itemsNeeded).ToList();

                list.AddRange(extra);
            }


            return list;
        }
    }
}