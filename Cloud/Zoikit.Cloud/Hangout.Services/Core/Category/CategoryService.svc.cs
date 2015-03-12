using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Hangout.Services.Core.Category
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CategoryService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CategoryService.svc or CategoryService.svc.cs at the Solution Explorer and start debugging.
    public class CategoryService : ICategoryService
    {
        Web.Services.Category.Category obj = new Web.Services.Category.Category();

        
        public Web.Core.Follow.FollowResult FollowCategory(Guid userId, Guid categoryId, string zat)
        {
            return obj.FollowCategory(userId, categoryId, zat);
        }

        public Web.Core.Follow.FollowResult UnfollowCategory(Guid userId, Guid categoryId, string zat)
        {
            return obj.UnfollowCategory(userId, categoryId, zat);
        }

        public List<Web.Services.Objects.Category.UserCategory> GetAllCategories(Guid userId, string zat)
        {
            return obj.GetAllCategories(userId, zat);
        }

        public List<Web.Services.Objects.Category.UserCategory> GetCategoriesFollowed(Guid userId, string zat)
        {
            return obj.GetCategoriesFollowed(userId, zat);
        }

        public List<Guid> GetCategoryIDFollowed(Guid userId, string zat)
        {
            return obj.GetCategoryIDFollowed(userId, zat);
        }


        public List<Web.Services.Objects.Category.UserCategory> GetCategoriesFollowedByUser(Guid meId, Guid userId, string zat)
        {
            return obj.GetCategoriesFollowed(meId, userId, zat);
        }


        public Web.Services.Objects.Category.UserCategory GetCategoyrByID(Guid userId, Guid categoryId, string zat)
        {
            return obj.GetCategoyrByID(userId, categoryId, zat);
        }
    }
}
