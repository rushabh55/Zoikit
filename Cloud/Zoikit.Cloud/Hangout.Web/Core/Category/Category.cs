using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Core.Category
{
    public class Category
    {
        public static List<Model.Category> GetAllCategory()
        {
            Core.Cloud.TableStorage.InitializeCategoryTable();

            return Model.Table.Category.ExecuteQuery(new TableQuery<Model.Category>()).ToList();
        }


        public static Model.Category GetCategoryByID(Guid id)
        {
            Core.Cloud.TableStorage.InitializeCategoryTable();

            return Model.Table.Category.ExecuteQuery(new TableQuery<Model.Category>().Where(
            TableQuery.GenerateFilterConditionForGuid("CategoryID", QueryComparisons.Equal,id))).FirstOrDefault();
        }


        public static void AddCategory(string name, string description, string smallUri, string medUri,string largeUri)
        {
            Core.Cloud.TableStorage.InitializeCategoryTable();

            Model.Category cat = new Model.Category(name);
            cat.Description = description;
            cat.LargePicURL = largeUri;
            cat.MedPicURL = medUri;
            cat.Name = name;
            cat.SmallPicURL = smallUri;


            Model.Table.Category.Execute(TableOperation.Insert(cat));


        }
    }
}