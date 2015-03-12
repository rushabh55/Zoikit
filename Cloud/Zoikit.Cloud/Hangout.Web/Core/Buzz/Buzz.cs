using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Hangout.Web.Core.Buzz
{
    public class Buzz
    {
        
        


        public static List<BuzzCommentData> GetBuzzComments(Guid buzzId)
        {
            List<Model.BuzzComment> list = Model.Table.BuzzComment.ExecuteQuery(new TableQuery<Model.BuzzComment>().Where(TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("BuzzID", QueryComparisons.Equal, buzzId.ToString()),
                TableOperators.And,
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.BuzzComment.GetPartitionKey(buzzId))))).ToList();


            List<BuzzCommentData> newList = new List<BuzzCommentData>();

            foreach (Model.BuzzComment comment in list)
            {
                newList.Add(new BuzzCommentData { comment = comment, profile = Core.Accounts.User.GetUserProfile(comment.UserID) });
            }


            return newList;
        }

        public static string SaveBuzzImage(byte[] image)
        {
            MemoryStream streams = new MemoryStream();

            // create storage account
            var account = CloudStorageAccount.Parse(Settings.CloudStorageString);

            // create blob client
            CloudBlobClient blobStorage = account.CreateCloudBlobClient();

            CloudBlobContainer container = blobStorage.GetContainerReference("buzzimage");
            container.CreateIfNotExists(); // adding this for safety

            string uniqueBlobName = "image_" + Guid.NewGuid() + ".jpg";

            CloudBlockBlob blob = container.GetBlockBlobReference(uniqueBlobName);

            blob.DeleteIfExists(); //delete if the file exists

            blob.Properties.ContentType = "image\\jpeg";


            BlobContainerPermissions containerPermissions = new BlobContainerPermissions();

            // The public access setting explicitly specifies that the container is private, 
            // so that it can't be accessed anonymously.
            containerPermissions.PublicAccess = BlobContainerPublicAccessType.Container;

            //Set the permission policy on the container.
            container.SetPermissions(containerPermissions);


            blob.UploadFromByteArray(image, 0, image.Count());


            streams.Close();

            return blob.Uri.ToString();
        }

        public static List<BuzzCommentData> GetBuzzCommentsAfter(Guid buzzId, DateTime after)
        {
            List<Model.BuzzComment> list =  Model.Table.BuzzComment.ExecuteQuery(new TableQuery<Model.BuzzComment>().Where(TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("BuzzID", QueryComparisons.Equal, buzzId.ToString()),
                TableOperators.And,
                TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.BuzzComment.GetPartitionKey(buzzId)), 
                TableOperators.And,
                TableQuery.GenerateFilterCondition("DateTimeStamp", QueryComparisons.GreaterThan, after.ToString()))))).ToList();


            List<BuzzCommentData> newList = new List<BuzzCommentData>();

            foreach (Model.BuzzComment comment in list)
            {
                newList.Add(new BuzzCommentData { comment = comment, profile = Core.Accounts.User.GetUserProfile(comment.UserID) });
            }


            return newList;

        }

      

       
      
       

       
        public static List<Model.BuzzByUser> GetUserLatestBuzzs(Guid userId, int take, List<Guid> skipList)
        {
            Core.Cloud.TableStorage.InitializeBuzzByUserTable();

            return Model.Table.BuzzByUser.ExecuteQuery(new TableQuery<Model.BuzzByUser>().Where(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.AmplifyBuzzByUser.GetPartitionKey(userId)))).ToList().Where(o=>!skipList.Contains(o.BuzzID)).Take(take).ToList();
        }


        public static List<Guid> GetUserBuzzInteract(Guid userId, int take, List<Guid> skipList)
        {
            Core.Cloud.TableStorage.InitializeUserBuzzInteract();

            return Model.Table.UserBuzzInteract.ExecuteQuery(new TableQuery<Model.UserBuzzInteract>().Where(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal,userId.ToString()))).ToList().Select(o=>o.BuzzID).Where(o => !skipList.Contains(o)).Take(take).ToList();
        }


        public static BuzzSaveStatus UpdateBuzz(Guid buzzId, string text, double? lat, double? lon, Guid? cityId,string imageURi)
        {
            if ((cityId == null || cityId == new Guid()) && lat == null && lon == null)
            {
                return BuzzSaveStatus.Error;
            }


            Model.BuzzByID obj = Core.Buzz.Buzz.GetBuzzByID(buzzId);

            if (obj == null)
            {
                return BuzzSaveStatus.Error;
            }

            Model.Location loc = null;
            if ((lat != null && lat != 0.0) && (lon != null && lon != 0.0))
            {
                Core.Cloud.TableStorage.InitializeLocationTable();
                loc = new Model.Location(lat.Value, lon.Value);
                Model.Table.Locations.Execute(TableOperation.InsertOrReplace(loc));
                obj.ScreenshotLocationID = loc.LocationID;

                
            }
            else
            {
                obj.CityID = cityId.Value;
                obj.ScreenshotLocationID = new Guid();
            }

            obj.DateTimeStamp = DateTime.Now;

            obj.Text = text;

            obj.ImageURL = imageURi;

            //remove all tokens in the current buzz. 

            Core.Tags.Tags.DeleteTagsInBuzz(obj.BuzzID);
            
            //parse and add tokens :)

            List<Model.Tag> tokens = Core.Tags.Tags.ParseTags(obj.Text);

            //link all these token to the current buzz. 

            Core.Cloud.TableStorage.InitializeTagInBuzzTable();

            foreach (Model.Tag t in tokens)
            {
                Model.TagInBuzz x = new Model.TagInBuzz(obj.BuzzID, t.TagID, obj.CityID);
                Model.BuzzByTag y = new Model.BuzzByTag(obj.BuzzID, t.TagID, obj.CityID);
                Model.Table.TagInBuzz.Execute(TableOperation.InsertOrReplace(x));
                Model.Table.BuzzByTag.Execute(TableOperation.InsertOrReplace(y));
            }


            Model.Table.Buzz.Execute(TableOperation.InsertOrReplace(obj));



            Core.Cloud.TableStorage.InitializeBuzzByIDTable();
            Model.BuzzByID obj1 = Core.Buzz.Buzz.GetBuzzByID(buzzId);
            if (obj1 != null)
            {
                obj1.UserID = obj.UserID;
                obj1.ImageURL = obj.ImageURL;
                obj1.DateTimeStamp = DateTime.Now;
                obj1.CityID = obj.CityID;
                obj1.ScreenshotLocationID = obj.ScreenshotLocationID;
                obj1.Text = obj.Text;
                Model.Table.BuzzByID.Execute(TableOperation.InsertOrReplace(obj1));
            }


            Core.Cloud.TableStorage.InitializeBuzzByUserTable();
            Model.BuzzByUser obj2 = Core.Buzz.Buzz.GetBuzzByUser(obj.BuzzID, obj.UserID);
            if (obj2 != null)
            {
                obj2.UserID = obj.UserID;
                obj2.ImageURL = obj.ImageURL;
                obj2.DateTimeStamp = DateTime.Now;
                obj2.CityID = obj.CityID;
                obj2.ScreenshotLocationID = obj.ScreenshotLocationID;
                obj2.Text = obj.Text;
                Model.Table.BuzzByUser.Execute(TableOperation.InsertOrReplace(obj2));
            }


            return BuzzSaveStatus.Saved;

        }

        public static BuzzTag InsertBuzz(Guid userId, string text, double? lat,double? lon, Guid? cityId,string imageUri)
        {

            if((cityId==null||cityId==new Guid())&&lat==null&&lon==null)
            {
                return new BuzzTag { SaveStatus = BuzzSaveStatus.Error, BuzzID = new Guid() };
            }

            Model.Buzz lastBuzz = GetLastBuzz(userId);

            if (lastBuzz != null)
            {
                if (lastBuzz.Text == text)
                {
                    return new BuzzTag { SaveStatus = BuzzSaveStatus.Spam, BuzzID = lastBuzz.BuzzID };
                }
            }

            Model.Buzz obj;

            Model.Location loc=null;
            if ((lat != null && lat != 0.0) && (lon != null && lon != 0.0))
            {

                Core.Cloud.TableStorage.InitializeLocationTable();

                loc = new Model.Location(lat.Value, lon.Value);
                obj = new Model.Buzz(userId, loc.CityID);
                Model.Table.Locations.Execute(TableOperation.InsertOrReplace(loc));

                
            }
            else
            {
                obj = new Model.Buzz(userId, cityId.Value);
                obj.ScreenshotLocationID = new Guid();
            }
            
            
            

            obj.DateTimeStamp = DateTime.Now;
            if (loc != null)
            {
                obj.ScreenshotLocationID = loc.LocationID;
            }


            obj.Text = text;

            //parse and add tokens :)

            List<Model.Tag> tokens = Core.Tags.Tags.ParseTags(obj.Text);

            //link all these token to the current buzz. 
            Core.Cloud.TableStorage.InitializeTagInBuzzTable();
            Core.Cloud.TableStorage.InitializeBuzzByTag();
            foreach (Model.Tag t in tokens)
            {
                Model.TagInBuzz x = new Model.TagInBuzz(obj.BuzzID, t.TagID,obj.CityID);
                Model.BuzzByTag y = new Model.BuzzByTag(obj.BuzzID, t.TagID, obj.CityID);

                
                Model.Table.TagInBuzz.Execute(TableOperation.InsertOrReplace(x));
                Model.Table.BuzzByTag.Execute(TableOperation.InsertOrReplace(y));
            }

            obj.UserID=userId;

            obj.ImageURL = imageUri;
           
            Model.Table.Buzz.Execute(TableOperation.InsertOrReplace(obj));

            Core.Cloud.TableStorage.InitializeBuzzByIDTable();
            Model.BuzzByID obj1= new Model.BuzzByID(obj.BuzzID, obj.UserID);
            obj1.UserID = userId;
            obj1.ImageURL = imageUri;
            obj1.DateTimeStamp = DateTime.Now;
            obj1.CityID = obj.CityID;
            obj1.ScreenshotLocationID = obj.ScreenshotLocationID;
            obj1.Text = obj.Text;
            Model.Table.BuzzByID.Execute(TableOperation.InsertOrReplace(obj1));


            Core.Cloud.TableStorage.InitializeBuzzByUserTable();
            Model.BuzzByUser obj2= new Model.BuzzByUser(obj.BuzzID,obj.UserID);
            obj2.UserID = userId;
            obj2.ImageURL = imageUri;
            obj2.DateTimeStamp = DateTime.Now;
            obj2.CityID = obj.CityID;
            obj2.ScreenshotLocationID = obj.ScreenshotLocationID;
            obj2.Text = obj.Text;
            Model.Table.BuzzByUser.Execute(TableOperation.InsertOrReplace(obj2));

            return new BuzzTag { BuzzID = obj.BuzzID, SaveStatus = BuzzSaveStatus.Saved };

        }

        public static Model.Buzz GetLastBuzz(Guid userId)
        {
            Core.Cloud.TableStorage.InitializeBuzzTable();

            return Model.Table.Buzz.ExecuteQuery(new TableQuery<Model.Buzz>().Where(
                TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId)).Take(1)).FirstOrDefault();
        }

        public static  List<Model.Buzz> GetBuzz(Guid userId, int take, List<Guid> skipList,double? lat,double? lon, Guid? cityId)
        {
            Core.Cloud.TableStorage.InitializeBuzzTable();

            if ((cityId==null||cityId==new Guid())&&lat==null&&lon==null)
            {
                throw new ArgumentNullException("Arguments null");
            }

            if (skipList == null)
            {
                skipList = new List<Guid>();
            }

            List<Model.Buzz> buzz = new List<Model.Buzz>();

            if (cityId == null)
            {
                Model.Location loc = new Model.Location(lat.Value, lon.Value);
                cityId = loc.CityID;
            }
           
            buzz.AddRange(Model.Table.Buzz.ExecuteQuery(
                new TableQuery<Model.Buzz>().Where(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.Buzz.GetPartitionKey(cityId.Value))).
                Take(Core.Smart.ListAlgo.TakeCount(take,skipList.Count))).Where(o=>!skipList.Contains(o.BuzzID))
                .Take(take).ToList());
                    
                    
            return buzz;

        }

        private static bool IsInBuzzCategory(Guid buzzid, Guid categoryId)
        {
            if (Model.Table.BuzzInCategory.ExecuteQuery(
                        new TableQuery<Model.BuzzInCategory>().Where(TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("BuzzID", QueryComparisons.Equal, buzzid.ToString()),
                        TableOperators.And,TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal,Model.BuzzInCategory.GetPartitionKey(categoryId)),
                        TableOperators.And, 
                        TableQuery.GenerateFilterCondition("CategoryID", QueryComparisons.Equal, categoryId.ToString())
                        )))).FirstOrDefault() == null)
            {
                return false;
            }


            return true;
        }

        public static List<Model.Buzz> GetBuzzBefore(Guid buzzId, Guid userId, double? lat,double? lon,Guid? cityId)
        {
            List<Model.Buzz> buzz = new List<Model.Buzz>();

            if(cityId==null)
            {
                Model.Location loc = new Model.Location(lat.Value, lon.Value);
                cityId = loc.CityID;
            }

            Model.BuzzByID b = Core.Buzz.Buzz.GetBuzzByID(buzzId);



            buzz.AddRange(Model.Table.Buzz.ExecuteQuery(
            new TableQuery<Model.Buzz>().Where(TableQuery.CombineFilters(
            TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.Buzz.GetPartitionKey(cityId.Value)),
            TableOperators.And,TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.LessThan, b.RowKey)))
            )
            .ToList());

                    
            return buzz;

        }

        internal static List<Model.Buzz> SearchBuzz(Guid userId, string searchtext, int take, List<Guid> skipList, double? lat, double? lon,  Guid? cityId)
        {

            Core.Cloud.TableStorage.InitializeBuzzTable();
           
            List<Model.Buzz> buzz = new List<Model.Buzz>();

            if (cityId == null)
            {
                Model.Location loc = new Model.Location(lat.Value, lon.Value);
                cityId = loc.CityID;
            }

            buzz.AddRange(Model.Table.Buzz.ExecuteQuery(
            new TableQuery<Model.Buzz>().Where(
            TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.Buzz.GetPartitionKey(cityId.Value))).
            Take(Core.Smart.ListAlgo.TakeCount(take,skipList.Count))).Where(o=>!skipList.Contains(o.BuzzID)&&o.Text.ToLower().Contains(searchtext.ToLower()))
            .Take(take).ToList());

                   
              

            return buzz;
        }


        public static List<Model.Buzz> GetBuzzAfter(Guid buzzId, Guid userId,  int take, Guid placeId, Guid cityId)
        {
            if (placeId == null && cityId == null)
            {
                throw new ArgumentNullException("Place and City ID both null");
            }
            List<Model.Buzz> buzz = new List<Model.Buzz>();
            if (placeId == null)
            {
               buzz.AddRange(Model.Table.Buzz.ExecuteQuery(
                        new TableQuery<Model.Buzz>().Where(TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.Buzz.GetPartitionKey(cityId)),
                        TableOperators.And,TableQuery.GenerateFilterCondition("BuzzID", QueryComparisons.LessThan, buzzId.ToString())))
                        )
                        .ToList());
            }
            else
            {
                buzz.AddRange(Model.Table.Buzz.ExecuteQuery(
                        new TableQuery<Model.Buzz>().Where(TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.Buzz.GetPartitionKey(placeId)),
                        TableOperators.And, TableQuery.GenerateFilterCondition("BuzzID", QueryComparisons.LessThan, buzzId.ToString())))
                        )
                        .ToList());
            }

            return buzz;
        }


    
      

        internal static int GetNoOfBuzzFollowed(Guid userId)
        {
            return Model.Table.BuzzFollow.ExecuteQuery(
                        new TableQuery<Model.BuzzFollow>().Where(
                        TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId))).Count();
        }

        internal static List<Model.Buzz> GetLocalBuzzByCategory(Guid userId, Guid categoryId, int take, List<Guid> skipList)
        {
            Model.City city=Core.Location.UserLocation.GetUserCity(userId);

            if(city==null)
            {
                return null;
            }

            return Model.Table.Buzz.ExecuteQuery(
                        new TableQuery<Model.Buzz>().Where(
                        TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.Buzz.GetPartitionKey(city.CityID))))
                        .Where(o=>IsInBuzzCategory(o.BuzzID,categoryId)).Where(o => !skipList.Contains(o.BuzzID)).Take(take).ToList();
                        
            
            
           
        }

        public static Model.BuzzByID GetBuzzByID(Guid buzzId)
        {
            Core.Cloud.TableStorage.InitializeBuzzByIDTable();

            return Model.Table.BuzzByID.ExecuteQuery(new TableQuery<Model.BuzzByID>().Where(TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.BuzzByID.GetPartitionKey(buzzId)),TableOperators.And,TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, Model.BuzzByID.GetRowKey(buzzId))))).FirstOrDefault();
        }


        public static Model.BuzzByUser GetBuzzByUserByID(Guid buzzId)
        {
            Core.Cloud.TableStorage.InitializeBuzzByUserTable();

            return Model.Table.BuzzByUser.ExecuteQuery(new TableQuery<Model.BuzzByUser>().Where(
                TableQuery.GenerateFilterConditionForGuid("BuzzID", QueryComparisons.Equal, buzzId))).FirstOrDefault();
        }


        public static Model.BuzzByTag GetBuzzByTagByID(Guid buzzId)
        {
            Core.Cloud.TableStorage.InitializeBuzzByTag();

            return Model.Table.BuzzByTag.ExecuteQuery(new TableQuery<Model.BuzzByTag>().Where(
                TableQuery.GenerateFilterConditionForGuid("BuzzID", QueryComparisons.Equal, buzzId))).FirstOrDefault();
        }

        public static Model.Buzz GetBuzzByID(Guid buzzId,Guid cityId)
        {
            Core.Cloud.TableStorage.InitializeBuzzTable();

            return Model.Table.Buzz.ExecuteQuery(new TableQuery<Model.Buzz>().Where(TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.Buzz.GetPartitionKey(cityId)),TableOperators.And,
                TableQuery.GenerateFilterConditionForGuid("BuzzID", QueryComparisons.Equal, buzzId)))).FirstOrDefault();
        }

        public static Model.Buzz GetBuzz(Guid buzzId)
        {
            Core.Cloud.TableStorage.InitializeBuzzTable();

            return Model.Table.Buzz.ExecuteQuery(new TableQuery<Model.Buzz>().Where(
                
                TableQuery.GenerateFilterConditionForGuid("BuzzID", QueryComparisons.Equal, buzzId))).FirstOrDefault();
        }

        public static Model.BuzzByUser GetBuzzByUser(Guid buzzId,Guid userId)
        {
            Core.Cloud.TableStorage.InitializeBuzzByUserTable();

            return Model.Table.BuzzByUser.ExecuteQuery(new TableQuery<Model.BuzzByUser>().Where(TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.BuzzByUser.GetPartitionKey(userId)),TableOperators.And,
                TableQuery.GenerateFilterConditionForGuid("BuzzID", QueryComparisons.Equal, buzzId)))).FirstOrDefault();
        }

        internal static List<Model.BuzzComment> GetBuzzComments(Guid userId, Guid buzzId, int take, List<Guid> skipList)
        {

            if(skipList==null)
            {
                skipList=new List<Guid>();
            }

            Core.Cloud.TableStorage.InitializeBuzzCommentTable();

            return Model.Table.BuzzComment.ExecuteQuery(
                        new TableQuery<Model.BuzzComment>().Where(
                        TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.Buzz.GetPartitionKey(buzzId)))).Where(o=>!skipList.Contains(o.BuzzCommentID)).Take(take).ToList();
            

        }

        internal static List<Model.BuzzComment> AddBuzzComment(Guid userId, Guid buzzId, string commentText,Guid lastcommentId)
        {

            Web.Core.Cloud.TableStorage.InitializeBuzzCommentTable();

                            

            Model.BuzzComment comment = new Model.BuzzComment(userId, buzzId, commentText);
            comment.UserID=userId;
            comment.CommentText = commentText;
            comment.BuzzID = buzzId;
            comment.DateTimeStamp = DateTime.Now;
            
            Model.Table.BuzzComment.Execute(TableOperation.InsertOrReplace(comment));

            //increase buzz comment count. 

            Core.Cloud.Queue.AddMessage("IncrementCommentCount:" + buzzId);
 
            return GetBuzzCommentsBefore(buzzId, lastcommentId);
        }

        public static void IncreaseBuzzComment(Guid buzzId)
        {
            Model.Buzz bz = Core.Buzz.Buzz.GetBuzz(buzzId);
            bz.CommentCount++;
            Model.Table.Buzz.Execute(TableOperation.Replace(bz));

            Model.BuzzByID bz2 = Core.Buzz.Buzz.GetBuzzByID(buzzId);
            bz2.CommentCount++;
            Model.Table.BuzzByID.Execute(TableOperation.Replace(bz2));

            Model.BuzzByUser bz3 = Core.Buzz.Buzz.GetBuzzByUserByID(buzzId);
            bz3.CommentCount++;
            Model.Table.BuzzByUser.Execute(TableOperation.Replace(bz3));

        }

        public static List<Model.BuzzComment> GetBuzzCommentsBefore(Guid buzzId, Guid lastcommentId)
        {
            Core.Cloud.TableStorage.InitializeBuzzCommentTable();

            List<Model.BuzzComment> comments = new List<Model.BuzzComment>();

           if(lastcommentId==null||lastcommentId==new Guid())
           {
               //get all comments. 

               comments.AddRange(Model.Table.BuzzComment.ExecuteQuery(
            new TableQuery<Model.BuzzComment>().Where(
            TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.BuzzComment.GetPartitionKey(buzzId)))).ToList());

               return comments;

           }
           else
           {
               Model.BuzzComment comment = Core.Buzz.Buzz.GetBuzzCommentByID(lastcommentId,buzzId);

               if (comment == null)
                   return comments;


                       comments.AddRange(Model.Table.BuzzComment.ExecuteQuery(
                  new TableQuery<Model.BuzzComment>().Where(TableQuery.CombineFilters(
                  TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.BuzzComment.GetPartitionKey(buzzId)),
                  TableOperators.And, TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.LessThan, comment.RowKey)))
                  )
                  .ToList());


                       return comments;
           }


          
        }

        private static Model.BuzzComment GetBuzzCommentByID(Guid lastcommentId,Guid buzzId)
        {
            Core.Cloud.TableStorage.InitializeBuzzCommentTable();

            return Model.Table.BuzzComment.ExecuteQuery(
            new TableQuery<Model.BuzzComment>().Where(TableQuery.CombineFilters(
            TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.BuzzComment.GetPartitionKey(buzzId)), TableOperators.And, TableQuery.GenerateFilterConditionForGuid("BuzzCommentID", QueryComparisons.Equal, lastcommentId)))).FirstOrDefault();


        }

        public static void NotifyBuzzComment(Guid userId, Guid buzzId, string comment)
        {

            Core.Cloud.TableStorage.InitializeNotificationsTable();
            Core.Cloud.TableStorage.InitializeNotificationsUserTable();

            List<Guid> userIds = Core.Buzz.Follow.GetUsersWhoLikesBuzz(buzzId);

            //now start notifting users. :)
            foreach (Guid id in userIds)
            {

                if (id == userId)
                {
                    continue; //Dont notify the current user who followed the buzz. 
                }
                string temp=buzzId.ToString();
                //remove all the user notifications. :)
                List<Model.Notifications> notifications = Model.Table.Notifications.ExecuteQuery(
                        new TableQuery<Model.Notifications>().Where(TableQuery.CombineFilters(
                        TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId)
                        ,TableOperators.And,
                        TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("Param1", QueryComparisons.Equal, temp.ToString())
                        ,TableOperators.And,
                        TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("Type1", QueryComparisons.Equal, "Buzz")
                        ,TableOperators.And,
                        TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("MarkAsRead", QueryComparisons.Equal, false.ToString())
                        ,TableOperators.And,
                        TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.GreaterThan, Model.Notifications.GetPartitionKey(userId)))))))).ToList();
                
                
                
                List<Guid> tempUsers = new List<Guid>(); //these guys commented on the buzz
                foreach (Model.Notifications n in notifications)
                {
                    tempUsers.AddRange(Core.Notifications.Notification.GetNotificationUsers(n.NotificationID).Select(o=>o.UserID).ToList());
                    Model.Table.Notifications.Execute(TableOperation.Delete(n));
                }

                if (tempUsers.Contains(userId))
                {
                    tempUsers.Remove(userId);
                }

               
                tempUsers.Add(userId);

                if (tempUsers.Contains(userId))
                {
                    tempUsers.Remove(userId);
                }

                Guid lastuserId = tempUsers.LastOrDefault();
                Model.UserProfile profile = Core.Accounts.User.GetUserProfile(lastuserId);
                Model.BuzzByID buzz = Core.Buzz.Buzz.GetBuzzByID(buzzId);
                string desc = "";
                if (tempUsers.Count - 1 > 0)
                {
                    desc = profile.Name + " and " + (tempUsers.Count - 1) + " other people commented \"" + buzz.Text + "\"";
                }
                else
                {
                    desc = profile.Name + " commented \""+comment+"\" on  \"" + buzz.Text + "\"";
                }

                Core.Notifications.Notification.AddNotification(userId, "","", desc, tempUsers, buzzId.ToString(), userId.ToString(), "Buzz", "User", "BuzzComment");

            }
        }

        public static void NotifyBuzzUpdate(Guid userId, Guid buzzId)
        {
            List<Guid> userIds = Core.Buzz.Follow.GetUsersWhoLikesBuzz(buzzId);

            if (userIds.Contains(userId))
            {
                userIds.Remove(userId);
            }

            //now start notifting users. :)
            foreach (Guid id in userIds)
            {

                Model.UserProfile profile = Core.Accounts.User.GetUserProfile(userId);
                Model.BuzzByID buzz = Core.Buzz.Buzz.GetBuzzByID(buzzId);
                string desc = "";
                desc = profile.Name + " updated the buzz \""+ buzz.Text + "\" ";
                List<Guid> userList=new List<Guid>();
                userList.Add(userId);

                Core.Notifications.Notification.AddNotification(id, "","", desc,userList, buzzId.ToString(), userId.ToString(), "Buzz", "User", "BuzzUpdated");

            }
        }

        public static void NotifyBuzzAdded(Guid userId, Guid buzzid)
        {
            List<Guid> following = Core.Users.Follow.GetFollowersList(userId, 9999999, new List<Guid>());

            List<Model.Tag> tokens = Core.Tags.Tags.GetTagsInBuzz(buzzid);
            Model.UserProfile profile = Core.Accounts.User.GetUserProfile(userId);
            Model.BuzzByID buzz = Core.Buzz.Buzz.GetBuzzByID(buzzid);

            if (buzz != null)
            {
                if (buzz.CityID != null)
                {
                    foreach (Model.Tag token in tokens)
                    {
                        following.AddRange(Core.Tags.Follow.GetLocalFollowersByCity(buzz.CityID, token.TagID, 9999999, new List<Guid>()).Select(o => o.UserID).ToList());
                    }
                }
            }

          

            string description="<bold> "+profile.Name + " </bold> shouted out the buzz \"" + buzz.Text + "\"";

           

           

            following = following.Distinct().ToList();


            if (following.Contains(userId))
            {
                following.Remove(userId);
            }

            foreach (Guid i in following) //now start notifying.
            {
                List<Guid> t = new List<Guid>();
                t.Add(userId);
                Core.Notifications.Notification.AddNotification(i, "","", description, t, buzzid.ToString(), userId.ToString(), "Buzz", "User", "BuzzAdded");
            }
        }

        //internal static bool DoesUserMakesOtherInteract(Guid userId)
        //{

        //    List<Guid> buzzIds= Core.Buzz.Buzz.GetUserLatestBuzzs(userId,0,).Select(o=>o.BuzzID).ToList();
            
        //    foreach(Guid id in buzzIds)
        //    {
        //        int count=  Model.Table.BuzzFollow.ExecuteQuery(new TableQuery<Model.BuzzFollow>().Where(
        //        TableQuery.GenerateFilterCondition("BuzzID", QueryComparisons.Equal, id.ToString()))).Count();


        //        if (count > 3)
        //        {
        //            return true;
        //        }

           
        //    }
            
        //   return false;

        //}


        internal static void PushBuzzCommentUpdate(Guid buzzId)
        {
            List<Guid> userIds = Core.Buzz.Follow.GetUsersWhoLikesBuzz(buzzId);

            Core.Notifications.Notification.SendBuzzCommentNotification(userIds, buzzId);
        }



        internal static List<Model.Buzz> GetBuzzByIDs(IEnumerable<Guid> buzzIDs)
        {
            List<Model.Buzz> buzz = new List<Model.Buzz>();

            foreach (Guid id in buzzIDs)
            {
                buzz.Add(Core.Buzz.Buzz.GetBuzz(id));
            }

            return buzz;
        }

        internal static List<Model.BuzzByID> GetBuzzsByID(IEnumerable<Guid> buzzIDs)
        {
            List<Model.BuzzByID> buzz = new List<Model.BuzzByID>();

            foreach (Guid id in buzzIDs)
            {
                buzz.Add(Core.Buzz.Buzz.GetBuzzByID(id));
            }

            return buzz;
        }



        internal static bool HasNewBuzz(Guid userId, Guid cityId, Guid lastBuzzId, string zat)
        {
            Core.Cloud.TableStorage.InitializeBuzzTable();

            TableQuery<Model.Buzz> q = new TableQuery<Model.Buzz>().Where(
                 TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.Buzz.GetPartitionKey(cityId)));
            q.TakeCount = 1;

            Core.Cloud.TableStorage.InitializeBuzzTable();

            Model.Buzz buzz= Model.Table.Buzz.ExecuteQuery(q).FirstOrDefault();

            if(buzz.BuzzID==lastBuzzId)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public static void IncreaseBuzzCount(Guid userId)
        {
            Model.UserProfile profile = Core.Accounts.User.GetUserProfile(userId);

            profile.BuzzCount++;

            Model.Table.UserProfile.Execute(TableOperation.Replace(profile));
        }
    }
}