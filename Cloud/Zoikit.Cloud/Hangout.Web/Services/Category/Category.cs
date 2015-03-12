using Hangout.Web.Core;
using Hangout.Web.Core.Follow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.Category
{
    public class Category
    {

       

        public List<Web.Services.Objects.Category.UserCategory> GetCategoriesFollowed(Guid userId,string zat)
        {
            List<Guid> ids = GetCategoryIDFollowed(userId, zat);
            List<Web.Services.Objects.Category.UserCategory> categories = new List<Web.Services.Objects.Category.UserCategory>();

            foreach (Guid id in ids)
            {
                Model.Category group = Core.Category.Category.GetCategoryByID(id);
                Web.Services.Objects.Category.UserCategory userCategory = new Objects.Category.UserCategory();
                Web.Services.Objects.Category.Category category = new Objects.Category.Category();
                category.CategoryID = group.CategoryID;
                category.PicURL = Settings.Root+group.MedPicURL;
                category.Name = group.Name;
                userCategory.Category=category;
                userCategory.Following=true;
                categories.Add(userCategory);
            }


            return categories;
        }

        public List<Guid> GetCategoryIDFollowed(Guid userId, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(zat))
                {
                    return Core.Category.Follow.GetCategoryFollowing(userId).Select(o=>o.CategoryID).ToList();
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }

            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return null;
            }
        }

        public Web.Core.Follow.FollowResult FollowCategory(Guid userId, Guid categoryId, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(zat))
                {


                   Web.Core.Follow.FollowResult res=  Core.Category.Follow.FollowCategory(userId, categoryId);



                   if (res == FollowResult.Following)
                   {
                       Core.Cloud.Queue.AddMessage("CategoryFollow:"+categoryId+":User:"+userId);
                   }

                   return res;
                }

                else
                {
                    throw new UnauthorizedAccessException();
                }
            }

            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return FollowResult.Error;
            }
        }

        public Web.Core.Follow.FollowResult UnfollowCategory(Guid userId, Guid categoryId, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(zat))
                {
                    Web.Core.Follow.FollowResult res= Core.Category.Follow.UnFollowCategory(userId, categoryId);

                    return res;
                }

                else
                {
                    throw new UnauthorizedAccessException();
                }
            }

            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return FollowResult.Error;
            }
        }

        public List<Web.Services.Objects.Category.UserCategory> GetAllCategories(Guid userId, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(zat))
                {

                    List<Objects.Category.UserCategory> list = new List<Objects.Category.UserCategory>();

                    List<Web.Model.Category> groups = Core.Category.Category.GetAllCategory();

                    

                    foreach (Model.Category ig in groups)
                    {
                        Objects.Category.UserCategory userCat = new Objects.Category.UserCategory();
                        Web.Services.Objects.Category.Category cat = new Objects.Category.Category();
                        cat.CategoryID = ig.CategoryID;
                        cat.Name = ig.Name;
                        cat.PicURL = Settings.Root + ig.MedPicURL;
                        userCat.Category = cat;
                        userCat.Following = Core.Category.Follow.IsFollowing(userId, ig.CategoryID);
                        list.Add(userCat);
                    }

                    return list;
                }

                else
                {
                    throw new UnauthorizedAccessException();
                }
            }

            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return null;
            }

        }

        internal static List<Objects.Category.Category> Convert(List<Model.Category> list)
        {
            List<Objects.Category.Category> newList = new List<Objects.Category.Category>();

            foreach (Model.Category cat in list)
            {
                newList.Add(Convert(cat));
            }

            return newList;
        }

        internal static Objects.Category.Category Convert(Model.Category category)
        {
            Objects.Category.Category obj = new Objects.Category.Category();
            obj.CategoryID = category.CategoryID;
            obj.Name = category.Name;
            obj.PicURL = Settings.Root + category.MedPicURL;

            return obj;
        }


        public List<Web.Services.Objects.Category.UserCategory> GetCategoriesFollowed(Guid meId, Guid userId, string zat)
        {
            List<Guid> ids = GetCategoryIDFollowed(userId, zat);
            List<Web.Services.Objects.Category.UserCategory> categories = new List<Web.Services.Objects.Category.UserCategory>();

            foreach (Guid id in ids)
            {
                Web.Services.Objects.Category.UserCategory userCategory = ConvertUserCategory(meId, id);
                categories.Add(userCategory);
            }


            return categories;
        }

        private Objects.Category.UserCategory ConvertUserCategory(Guid meId, Guid id)
        {
            Model.Category group = Core.Category.Category.GetCategoryByID(id);
            Web.Services.Objects.Category.UserCategory userCategory = new Objects.Category.UserCategory();
            Web.Services.Objects.Category.Category category = new Objects.Category.Category();
            category.CategoryID = group.CategoryID;
            category.PicURL = Settings.Root + group.MedPicURL;
            category.Name = group.Name;
            userCategory.Category = category;
            userCategory.Following = Core.Category.Follow.IsFollowing(meId, id);
            return userCategory;
        }

        public Objects.Category.UserCategory GetCategoyrByID(Guid userId, Guid categoryId, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(zat))
                {
                    Model.Category group = Core.Category.Category.GetCategoryByID(categoryId);
                    Web.Services.Objects.Category.UserCategory userCategory = new Objects.Category.UserCategory();
                    Web.Services.Objects.Category.Category category = new Objects.Category.Category();
                    category.CategoryID = group.CategoryID;
                    category.PicURL = Settings.Root + group.LargePicURL;
                    category.Name = group.Name;
                    userCategory.Category = category;
                    userCategory.Following = Core.Category.Follow.IsFollowing(userId, categoryId);
                    return userCategory;
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }

            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return null;
            }
        }

    }
}