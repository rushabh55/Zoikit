using Hangout.Web.Core.Location;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Core.Users
{
    public class Users
    {
        public static List<Model.User> MutualPeopleFollowed(Guid user1Id, Guid user2Id)
        {

            List<string> selectColumns=new List<string>();
            selectColumns.Add("FollowuserId");

            TableQuery<Model.UserFollow> FollowedByUser1 = new TableQuery<Model.UserFollow>().Where(
            TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, user1Id)).Select(selectColumns);


            TableQuery<Model.UserFollow> FollowedByUser2 = new TableQuery<Model.UserFollow>().Where(
            TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, user2Id)).Select(selectColumns);


            IEnumerable<Guid> followByUser1Id = Model.Table.UserFollow.ExecuteQuery(FollowedByUser1).ToList().Select(o=>o.FollowuserId);

            IEnumerable<Guid> followByUser2Id = Model.Table.UserFollow.ExecuteQuery(FollowedByUser2).ToList().Select(o => o.FollowuserId);

            IEnumerable<Guid> comonuserIds = followByUser1Id.Where(o => followByUser2Id.Contains(o));

            return Core.Accounts.User.GetUsersById(comonuserIds.ToList()); //return list of common values. :)
        }

        public static List<Model.User> UnMutualPeopleFollowed(Guid user1Id, Guid user2Id)
        {
            List<Model.User> FollowedByUser1 = Core.Users.Follow.GetFollowingUserList(user1Id,999999,new List<Guid>());

            List<Model.User> FollowedByUser2 = Core.Users.Follow.GetFollowingUserList(user2Id,999999,new List<Guid>());

            return FollowedByUser1.Where(o => !FollowedByUser2.Contains(o)).ToList();
        }

        public static int GetNoOfMutualPeopleFollowed(Guid user1Id, Guid user2Id)
        {
            return MutualPeopleFollowed(user1Id, user2Id).Count();
        }

        public static List<Model.User> GetPeopleAroundYou(Guid userId,int count,List<Guid> skipList)
        {
            Random r=new Random();

            Model.Location loc = Core.Location.UserLocation.GetLastLocation(userId);

            if (loc == null)
            {
                return new List<Model.User>();
            }

            //List<Model.User> list = .Compatibilities.Where(o => o.User1ID == userId&&o.User.UserFollows.Where(q=>q.UserFollowID==o.User2ID).Count()==0).OrderBy(o => o.Score).Select(o => o.User1).Take(100).ToList().OrderBy(a => r.Next()).Take(unfollowCount).ToList();

            List<Model.UserLocation> list =  Model.Table.UserLocations.ExecuteQuery(new TableQuery<Model.UserLocation>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, loc.CityID.ToString()))).ToList();
            
            List<TemUserLocationData> newlist=new List<TemUserLocationData>();
             foreach (Model.UserLocation  u in list)
             {
                TemUserLocationData data=new TemUserLocationData();
                 data.user=Core.Accounts.User.GetUser(u.UserID);
                 data.location=Core.Location.Location.GetLocation(u.LocationID);  
             }
            
            return newlist.OrderBy(o => Core.Location.Distance.CalculateDistance(o.location.Latitude, o.location.Longitude,loc.Latitude,loc.Longitude)).Take(count * 2).OrderBy(x => r.Next(1, 50000)).Take(count).Select(o=>o.user).ToList();
                                  
            
        }

        public static List<Model.User> GetRelatedFollowedPeople(Guid userId, Guid relateduserId, int count)
        {

            Random r = new Random();

            //take 70% of mutual categories
            int m = (count * 70) / 100;
            //take 30% of unmutual categories
            int um = (count * 30) / 100;
            List<Model.User> list = MutualPeopleFollowed(userId, relateduserId).ToList().OrderBy(o => r.Next()).Take(m).ToList();

            list.AddRange(UnMutualPeopleFollowed(userId, relateduserId).ToList().OrderBy(o => r.Next()).Take(um).ToList());


            if (count > list.Count())
            {
                int itemsNeeded = count - list.Count();

                //get all categories followed.

                List<Model.User> followed = Core.Users.Follow.GetFollowingUserList(userId, 999999,new List<Guid>());

                //add items from the followed list to the "list" which are not ther ein the list

                List<Model.User> extra = followed.Where(o => !list.Contains(o)).ToList().OrderBy(o => r.Next()).Take(itemsNeeded).ToList();

                list.AddRange(extra);
            }


            return list;
        }

        public static List<Model.User> MutualPeopleFollowers(Guid user1Id, Guid user2Id)
        {
            List<Model.User> FollowersOfUser1 = Core.Users.Follow.GetFollowersUserList(user1Id,999999,new List<Guid>());

            List<Model.User> FollowersOfUser2 = Core.Users.Follow.GetFollowersUserList(user2Id,999999,new List<Guid>());

            return FollowersOfUser1.Where(o => FollowersOfUser2.Contains(o)).ToList(); //return list of common values. :)
        }

        public static List<Model.User> UnMutualPeopleFollowers(Guid user1Id, Guid user2Id)
        {
            List<Model.User> FollowedByUser1 = Core.Users.Follow.GetFollowersUserList(user1Id,999999,new List<Guid>());

            List<Model.User> FollowedByUser2 = Core.Users.Follow.GetFollowersUserList(user2Id,999999,new List<Guid>());

            return FollowedByUser1.Where(o => !FollowedByUser2.Contains(o)).ToList();
        }

        public static int GetNoOfMutualPeopleFollowers(Guid user1Id, Guid user2Id)
        {
            return MutualPeopleFollowers(user1Id, user2Id).Count();
        }

        public static List<Model.User> GetRelatedFollowersPeople(Guid userId, Guid relateduserId, int count)
        {

            Random r = new Random();

            //take 70% of mutual categories
            int m = (count * 70) / 100;
            //take 30% of unmutual categories
            int um = (count * 30) / 100;
            List<Model.User> list = MutualPeopleFollowers(userId, relateduserId).ToList().OrderBy(o => r.Next()).Take(m).ToList();

            list.AddRange(UnMutualPeopleFollowers(userId, relateduserId).ToList().OrderBy(o => r.Next()).Take(um).ToList());


            if (count > list.Count())
            {
                int itemsNeeded = count - list.Count();

                //get all categories followed.

                List<Model.User> followed = Core.Users.Follow.GetFollowersUserList(userId, 999999,new List<Guid>());

                //add items from the followed list to the "list" which are not ther ein the list

                List<Model.User> extra = followed.Where(o => !list.Contains(o)).ToList().OrderBy(o => r.Next()).Take(itemsNeeded).ToList();

                list.AddRange(extra);
            }


            return list;
        }


        internal static List<Model.User> GetLocalUsersByCategoryFollowing(Guid userId, Guid categoryId, int take, List<Guid> skipList)
        {
            Model.City city=Core.Location.UserLocation.GetUserCity(userId);

            List<Guid> localUserIDs=  Model.Table.UserLocations.ExecuteQuery(new TableQuery<Model.UserLocation>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, city.CityID.ToString()))).Select(o=>o.UserID).ToList();

            List<Guid> categoryFollowing = Model.Table.UserCategoryFollows.ExecuteQuery(new TableQuery<Model.UserCategoryFollow>().Where(TableQuery.GenerateFilterCondition("CategoryID", QueryComparisons.Equal, categoryId.ToString()))).Select(o=>o.UserID).ToList();

            return Core.Accounts.User.GetUsersById(localUserIDs.Where(o=>categoryFollowing.Contains(o)&&!skipList.Contains(o)).OrderBy(o => new Random().Next(1, 10000)).Take(take).ToList());

        }

        internal static List<Model.User> GetPeoplewhoFollowBuzz(Guid buzzId, int take, List<Guid> skipList)
        {
            return Core.Accounts.User.GetUsersById(Model.Table.BuzzFollow.ExecuteQuery(new TableQuery<Model.BuzzFollow>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.BuzzFollow.GetPartitionKey(buzzId)))).Select(o => o.UserID).ToList().Where(o => !skipList.Contains(o)).OrderByDescending(o => new Random().Next(1, 1000)).Take(take).ToList());
        }

        internal static List<Model.User> GetPeoplewhoFollowPlace(Guid placeId, int take, List<Guid> skipList)
        {
            return Core.Accounts.User.GetUsersById(Model.Table.UserPlaceFollow.ExecuteQuery(new TableQuery<Model.UserPlaceFollow>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.UserPlaceFollow.GetPartitionKey(placeId)))).Select(o=>o.UserID).ToList().Where(o=>!skipList.Contains(o)).OrderByDescending(o=> new Random().Next(1,1000)).Take(take).ToList());
        }

        internal static List<Model.User> GetPeopleNearPlace(Guid placeId, int take, List<Guid> skipList)
        {
            Model.Place v=Core.Location.Place.GetPlace(placeId);
            

            if (v != null)
            {
                Model.Location l=Core.Location.Location.GetLocation(v.LocationID);

                List<Model.UserLocation> list =  Model.Table.UserLocations.ExecuteQuery(new TableQuery<Model.UserLocation>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, l.CityID.ToString()))).Where(o=>!skipList.Contains(o.UserID)).ToList();
            
                List<TemUserLocationData> newlist=new List<TemUserLocationData>();

                 foreach (Model.UserLocation  u in list)
                 {
                    TemUserLocationData data=new TemUserLocationData();
                     data.user=Core.Accounts.User.GetUser(u.UserID);
                     data.location=Core.Location.Location.GetLocation(u.LocationID);  
                 }
            
                return newlist.OrderBy(o => Core.Location.Distance.CalculateDistance(o.location.Latitude, o.location.Longitude,l.Latitude,l.Longitude)).Take(take).Select(o=>o.user).ToList();

            }

            return null;
        }

        public static void NotifyInterestedPeopleNearUser(Guid userId)
        {
            Model.UserProfile up = Core.Accounts.User.GetUserProfile(userId);
            List<Model.User> users = GetUsersWithinRadius(userId, 1.0);    
            if(users.Count>0)
            {

                string desc="";

                Model.UserProfile profile=Core.Accounts.User.GetUserProfile(users.Last().UserID);



                if(users.Count>1)
                {
                    desc="<bold>"+profile.Name+"</bold> and <bold>"+(users.Count-1)+"other people </bold> are near you";
                }
                else
                {
                    desc="<bold>"+profile.Name+"</bold> is near you";
                }

                Core.Notifications.Notification.AddNotification(userId, "People around you","",desc,users.Select(o=>o.UserID).ToList(),"People","","Dashboard","","PeopleNearYou"); //notify this guy. :)

                foreach (Model.User user in users)
                {
                    desc = "<bold>" + up.Name + "</bold> is near you";
                    List<Guid> x=new List<Guid>();
                    x.Add(userId);
                    Core.Notifications.Notification.AddNotification(user.UserID, "People around you","", desc,x, "People", "", "Dashboard", "", "PeopleNearYou"); //notify this guy. :)
                }

                //and We're done! :)

            }

        }

        public static List<Model.User> GetUsersWithinRadius(Guid userId, double radius)
        {
            Model.Location location = Core.Location.UserLocation.GetLastLocation(userId);
            List<Model.User> finalList = new List<Model.User>();
            if (location == null)
            {
                List<Model.UserLocation> userIds = Model.Table.UserLocations.ExecuteQuery(new TableQuery<Model.UserLocation>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, location.CityID.ToString()))).ToList();

                foreach (Model.UserLocation  u in userIds)
                {
                    
                   
                        Model.Location loc = Core.Location.UserLocation.GetCurrentLocation(u.UserID);

                        if(Core.Location.Distance.CalculateDistance(loc.Latitude,loc.Longitude,location.Latitude,location.Longitude)<1.0)
                        {
                            finalList.Add(Core.Accounts.User.GetUser(u.UserID));
                        }

                          
                    }
                }

            

            return finalList;

        }

        internal static int GetNoOfCommonItems(Guid user1, Guid user2)
        {
            int items = 0;

            items += Core.Users.Users.GetNoOfMutualPeopleFollowed(user1, user2);
            items += Core.Category.User.GetNoOfMutualCategoriesFollowed(user1, user2);
            items += Core.Tags.User.GetNoOfMutualTagsFollowed(user1, user2);
            items += Core.Location.UserPlaceFollow.GetNoOfMutualPlacesFollowed(user1, user2);
            return items;
        }

        internal static List<Model.User> SearchPeopleAroundYou(Guid userId, string text, int count, List<Guid> skipList,double lat, double lon, Guid cityId)
        {

           

            List<Guid> userIds =
            Model.Table.UserLocations.ExecuteQuery(
                        new TableQuery<Model.UserLocation>().Where(
                        TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.UserLocation.GetPartitonKeyByCityID(cityId)))).Where(o=>!skipList.Contains(o.UserID)).Select(o=>o.UserID).ToList();
            

            List<Model.User> users=Core.Accounts.User.GetUsersById(Core.Accounts.User.GetUserProfilesById(userIds).Where(o=>o.Name.ToLower().Contains(text.ToLower())).Select(o=>o.UserID).Distinct().ToList());

            

            //search by interest
            string[] words = text.Split(' ');

            foreach (string word in words)
            {
                if(Core.Tags.Tags.Exists(word))
                {
                    Guid id=Core.Tags.Tags.GetTagIdByName(word);

                    List<Guid> localUserIDs=  Model.Table.UserLocations.ExecuteQuery(new TableQuery<Model.UserLocation>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, cityId.ToString()))).Select(o=>o.UserID).ToList();
                    List<Guid> tagFollow=  Model.Table.UserTagFollow.ExecuteQuery(new TableQuery<Model.UserTagFollow>().Where(TableQuery.GenerateFilterCondition("TagID", QueryComparisons.Equal,id.ToString()))).Select(o=>o.UserID).ToList();
                    users.AddRange(Core.Accounts.User.GetUsersById(localUserIDs.Where(o=>tagFollow.Contains(o)&&!skipList.Contains(o)).ToList()));
                     
                }
            }





            return users.OrderBy(o => new Random().Next()).Distinct().Take(count).ToList();
            //shoot!
        }

        public static List<Services.Objects.User.UserKnow> GetMutualUserKnows(Guid user1Id, Guid user2Id)
        {
            List<Services.Objects.User.UserKnow> list = new List<Services.Objects.User.UserKnow>();

            List<Model.User> USers1 = Core.Users.Follow.GetFollowingUserList(user1Id, 9999999, new List<Guid>());
            List<Model.User> USers2 = Core.Users.Follow.GetFollowersUserList(user2Id, 9999999, new List<Guid>());

            List<Model.User> users = USers1.Where(o => USers2.Contains(o)).ToList();
            foreach (Model.User user in users)
            {
                Services.Objects.User.UserKnow temp = new Services.Objects.User.UserKnow();
                Model.UserProfile profile=Core.Accounts.User.GetUserProfile(user.UserID);
                temp.Name = profile.Name.Split(' ')[0];
                temp.ProfilePicURL = new Uri(profile.ProfilePicURL);
                temp.UserID = user.UserID;
                temp.Type = Services.Objects.User.SocialNetworkType.Zoikit;

                list.Add(temp);
            }

            //now check if the user doesnot already exists with userId and both Name. :)

            //fb
            


            TableQuery<Model.FacebookData> FollowedByUser1 = new TableQuery<Model.FacebookData>().Where(
            TableQuery.GenerateFilterCondition("FacebookUser1ID", QueryComparisons.Equal, user1Id.ToString()));

          
            TableQuery<Model.FacebookData> FollowedByUser2 = new TableQuery<Model.FacebookData>().Where(
            TableQuery.GenerateFilterCondition("FacebookUser2ID", QueryComparisons.Equal, user2Id.ToString()));


            IEnumerable<Model.FacebookData> followByUser1Id = Model.Table.FacebookData.ExecuteQuery(FollowedByUser1);

            IEnumerable<Model.FacebookData> followByUser2Id = Model.Table.FacebookData.ExecuteQuery(FollowedByUser2);
            
            //get MutualPeopleFollowed users

            List<Model.FacebookData> fbUsers = followByUser1Id.Where(o => followByUser2Id.Contains(o)).ToList();

            foreach(Model.FacebookData id in fbUsers)
            {
                if (id.UserID != null)
                {
                    if (list.Where(o => o.UserID == id.UserID).Count() > 0)
                    {
                        continue;
                    }
                }
                    
              
                if(list.Where(o=>(id.FirstName+" "+id.LastName).Contains(o.Name)).Count()>0)
                {
                    continue;
                }

                //add user. :)

                Services.Objects.User.UserKnow temp = new Services.Objects.User.UserKnow();
                if (id.UserID != null)
                {
                    Model.UserProfile profile = Core.Accounts.User.GetUserProfile(id.UserID);
                    temp.Name = profile.Name.Split(' ')[0];
                    temp.ProfilePicURL = new Uri(profile.ProfilePicURL);
                    temp.UserID = id.UserID;
                    temp.Type = Services.Objects.User.SocialNetworkType.Zoikit;
                }
                else
                {

                    temp.Name = id.FirstName;
                    temp.ProfilePicURL = new Uri(id.ProfilePicURL);
                    temp.Type = Services.Objects.User.SocialNetworkType.Facebook;
                }

                list.Add(temp);
            }




           


            TableQuery<Model.TwitterData> twitterFolowedByUser1 = new TableQuery<Model.TwitterData>().Where(
            TableQuery.GenerateFilterCondition("TwitterUser1ID", QueryComparisons.Equal, user1Id.ToString()));

            

            TableQuery<Model.TwitterData> twitterFollowedByUser2 = new TableQuery<Model.TwitterData>().Where(
            TableQuery.GenerateFilterCondition("TwitterUser2ID", QueryComparisons.Equal, user2Id.ToString()));


            IEnumerable<Model.TwitterData> twitterfollowByUser1Id = Model.Table.TwitterData.ExecuteQuery(twitterFolowedByUser1);

            IEnumerable<Model.TwitterData> twitterfollowByUser2Id = Model.Table.TwitterData.ExecuteQuery(twitterFollowedByUser2);

            //get MutualPeopleFollowed users

            List<Model.TwitterData> twitterUsers = twitterfollowByUser1Id.Where(o => twitterfollowByUser2Id.Contains(o)).ToList();


            //twitter
            

            foreach (Model.TwitterData twitterUser in twitterUsers)
            {
                if (twitterUser.UserID != null)
                {
                    //check userId
                    if (list.Where(o => o.UserID == twitterUser.UserID).Count() > 0)
                    {
                        continue;
                    }

                }

                if (list.Where(o => (twitterUser.Name).Contains(o.Name)).Count() > 0)
                {
                    continue;
                }

                //add user. :)

                Services.Objects.User.UserKnow temp = new Services.Objects.User.UserKnow();
                if (twitterUser.UserID != null)
                {
                    Model.UserProfile profile = Core.Accounts.User.GetUserProfile(twitterUser.UserID);
                    temp.Name = profile.Name.Split(' ')[0];
                    temp.ProfilePicURL = new Uri(profile.ProfilePicURL);
                    temp.UserID = twitterUser.UserID;
                    temp.Type = Services.Objects.User.SocialNetworkType.Zoikit;
                }
                else
                {

                    temp.Name = twitterUser.Name.Split(' ')[0];
                    temp.ProfilePicURL = new Uri(twitterUser.ProfileImageURL);
                    temp.Type = Services.Objects.User.SocialNetworkType.Twitter;
                }

                list.Add(temp);
            }


            //fqs

           


            TableQuery<Model.FoursquareData> foursquareFolowedByUser1 = new TableQuery<Model.FoursquareData>().Where(
            TableQuery.GenerateFilterCondition("FoursquareUser1ID", QueryComparisons.Equal, user1Id.ToString()));

            

            TableQuery<Model.FoursquareData> foursquareFollowedByUser2 = new TableQuery<Model.FoursquareData>().Where(
            TableQuery.GenerateFilterCondition("FoursquareUser2ID", QueryComparisons.Equal, user2Id.ToString()));


            IEnumerable<Model.FoursquareData> foursquarefollowByUser1Id = Model.Table.FoursquareData.ExecuteQuery(foursquareFolowedByUser1);

            IEnumerable<Model.FoursquareData> foursquarefollowByUser2Id = Model.Table.FoursquareData.ExecuteQuery(foursquareFollowedByUser2);

            //get MutualPeopleFollowed users

            List<Model.FoursquareData> foursquareUsers = foursquarefollowByUser1Id.Where(o => foursquarefollowByUser2Id.Contains(o)).ToList();


           

            foreach (Model.FoursquareData fqUser in foursquareUsers)
            {
                if (fqUser.UserID != null)
                {
                    //check userId
                    if (list.Where(o => o.UserID == fqUser.UserID).Count() > 0)
                    {
                        continue;
                    }

                }

                if (list.Where(o => (fqUser.FirstName+" "+fqUser.LastName).Contains(o.Name)).Count() > 0)
                {
                    continue;
                }

                //add user. :)

                Services.Objects.User.UserKnow temp = new Services.Objects.User.UserKnow();
                if (fqUser.UserID != null)
                {
                    Model.UserProfile profile = Core.Accounts.User.GetUserProfile(fqUser.UserID);
                    temp.Name = profile.Name.Split(' ')[0];
                    temp.ProfilePicURL = new Uri(profile.ProfilePicURL);
                    temp.UserID = fqUser.UserID;
                    temp.Type = Services.Objects.User.SocialNetworkType.Zoikit;
                }
                else
                {

                    temp.Name = fqUser.FirstName;
                    temp.ProfilePicURL = new Uri(fqUser.PhotoPrefix+fqUser.PhotoSuffix);
                    temp.Type = Services.Objects.User.SocialNetworkType.Twitter;
                }

                list.Add(temp);
            }


            return list;
        }



        internal static List<Guid>  GetLocalUsers(Guid userId, Guid cityId, int take, List<Guid> skipList)
        {
            Core.Cloud.TableStorage.InitializeUserLocationsTable();


           if(skipList!=null)
           {
               skipList.Add(userId);
           }
           else
           {
               skipList = new List<Guid>();
           }

           TableQuery<Model.UserLocation> users = new TableQuery<Model.UserLocation>().Where(
           TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.UserLocation.GetPartitonKeyByCityID(cityId)));

           return Model.Table.UserLocations.ExecuteQuery(users).Select(o => o.UserID).Where(o => !skipList.Contains(o)).OrderBy(o=>new Random().Next()).Distinct().Take(take).ToList();
        }
    }
}