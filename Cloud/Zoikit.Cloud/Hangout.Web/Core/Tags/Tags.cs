using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Core.Tags
{
    public class Tags
    {
        public static Model.Tag InsertTagIfNotExists(string name)
        {
            Core.Cloud.TableStorage.InitializeTagTable();

            name = NormalizeTagName(name);

            if (name != null)
            {
                Core.Cloud.TableStorage.InitializeTagTable();
                Model.Tag token = Model.Table.Tag.ExecuteQuery(new TableQuery<Model.Tag>().Where(TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("Name", QueryComparisons.Equal, name.ToString()), TableOperators.And, TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, name), TableOperators.And,TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.Tag.GetPartitionKey(name))
                    )))).FirstOrDefault();

                if (token == null)
                {
                    token = new Model.Tag(name);
                    token.Name = name;
                    token.Description = "";

                    Model.Table.Tag.Execute(TableOperation.InsertOrReplace(token));
                    
                }

                return token;
            }
            else
            {
                return null;
            }

        }

        public static string NormalizeTagName(string tokenname)
        {
            tokenname = tokenname.ToLower();

            string newTag = "";

            newTag=tokenname.ToLower().TrimEnd('.', ',', '?', '!', '$', '%', '^', '.', '#', '*', '(', ')', '{', '}', '\"');
            newTag = newTag.TrimStart('#');

            if (newTag.Contains(" "))
            {
                string[] words = newTag.Split(' ');
                newTag = "";
                foreach(string x in words)
                {
                    if(newTag=="")
                    {
                        newTag += x.ToLower();


                    }
                    else
                    {
                        //cap the first letter of the word
                        newTag+= char.ToUpper(x[0]) + x.Substring(1);
                    }
                }
            }

           

            if (newTag == "")
            {
                return null;
            }

            return newTag;
        }


        

       

        

       


        internal static Model.Tag GetTagByID(Guid id)
        {
            Core.Cloud.TableStorage.InitializeTagTable();

            return Model.Table.Tag.ExecuteQuery(new TableQuery<Model.Tag>().Where(TableQuery.GenerateFilterConditionForGuid("TagID", QueryComparisons.Equal, id))).FirstOrDefault();
        }

        internal static List<Model.Tag> ParseTags(string text)
        {

            


            List<Model.Tag> list = new List<Model.Tag>();

            List<string> tokens = text.Split(' ').Where(o => o.StartsWith("#")).ToList();

            foreach (string x in tokens)
            {
                list.Add(InsertTagIfNotExists(x));
            }

            return list;
        }

        public static List<Model.Category> GetCategoryOfTag(Guid tagId)
        {
            Core.Cloud.TableStorage.InitializeTagInCategoryTable();

             List<Model.TagInCategory> token = Model.Table.TagInCategory.ExecuteQuery(new TableQuery<Model.TagInCategory>().Where(
                    TableQuery.GenerateFilterConditionForGuid("TagID", QueryComparisons.Equal, tagId)
                    )).ToList();


            List<Model.Category> cat=new List<Model.Category>();
            foreach(Model.TagInCategory obj in token)
            {
                cat.Add(Core.Category.Category.GetCategoryByID(obj.CategoryID));
            }

            return cat;

           
        }

        internal static List<Model.Tag> GetTagsInBuzz(Guid buzzId)
        {
            Core.Cloud.TableStorage.InitializeTagInBuzzTable();

            List<Model.TagInBuzz> token = Model.Table.TagInBuzz.ExecuteQuery(new TableQuery<Model.TagInBuzz>().Where(
                     TableQuery.GenerateFilterConditionForGuid("BuzzID", QueryComparisons.Equal, buzzId)
                     )).ToList();


            List<Model.Tag> cat = new List<Model.Tag>();
            foreach (Model.TagInBuzz obj in token)
            {
                cat.Add(Core.Tags.Tags.GetTagByID(obj.TagID));
            }

            return cat;
        }

        public static bool Exists(string name)
        {
            Core.Cloud.TableStorage.InitializeTagTable();

            name = NormalizeTagName(name);

            if ( Model.Table.Tag.ExecuteQuery(new TableQuery<Model.Tag>().Where(TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("Name", QueryComparisons.Equal, name.ToString()), TableOperators.And, TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.Tag.GetPartitionKey(name))
                    ))).FirstOrDefault() !=null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static Guid GetTagIdByName(string name)
        {
            Core.Cloud.TableStorage.InitializeTagTable();

            name = NormalizeTagName(name);
            return Model.Table.Tag.ExecuteQuery(new TableQuery<Model.Tag>().Where(TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, name.ToString()), TableOperators.And, TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.Tag.GetPartitionKey(name))
                    ))).FirstOrDefault().TagID;
           
        }

        internal static void DeleteTagsInBuzz(Guid buzzId)
        {
            Core.Cloud.TableStorage.InitializeTagInBuzzTable();

            List<Model.TagInBuzz> token = Model.Table.TagInBuzz.ExecuteQuery(new TableQuery<Model.TagInBuzz>().Where(
                     TableQuery.GenerateFilterConditionForGuid("BuzzID", QueryComparisons.Equal, buzzId)
                     )).ToList();

            TableBatchOperation batch = new TableBatchOperation();
            
            
            foreach(Model.TagInBuzz t in token)
            {
                batch.Add(TableOperation.Delete(t));

            }

            Model.Table.TagInBuzz.ExecuteBatch(batch);

            

        }
    }
}