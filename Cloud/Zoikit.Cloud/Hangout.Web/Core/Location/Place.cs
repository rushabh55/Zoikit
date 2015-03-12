using Hangout.Web.Core;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace Hangout.Web.Core.Location
{
    public class Place
    {
        public static List<Model.Place> GetRecommendedPlaces(Guid userId, double latitude, double longitude)
        {


            //construct the URI

            string uri = @"https://api.foursquare.com/v2/venues/explore?ll="+latitude+","+longitude+"&limit=5&client_id="+AppSettings.FoursquareClientID+"&client_secret="+AppSettings.FoursquareClientSecret;

            if (Core.Accounts.Foursquare.HasFoursquareData(userId))
            {
                //get the access token, 
                string accesstoken = Core.Accounts.Foursquare.GetFoursquareData(userId).AccessToken;
                uri += @"&oauth_token=" + accesstoken;
               
            }

            //get the list of venues from foursquare. 
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            JObject obj = JObject.Parse(new StreamReader(response.GetResponseStream()).ReadToEnd());

            //save it to the database, if Its not there already. 


            //checks :) 
            if (obj.SelectToken("response") == null)
            {
                return null;
            }


            if (obj.SelectToken("response").SelectToken("groups") == null)
            {
                return null;
            }

            if (obj.SelectToken("response").SelectToken("groups")[0] == null)
            {
                return null;
            }


            if (obj.SelectToken("response").SelectToken("groups")[0].SelectToken("items") == null)
            {
                return null;
            }


            List<JToken> fqPlaces = obj.SelectToken("response").SelectToken("groups")[0].SelectToken("items").ToList();

            
            return ParseFoursquarePlaces(fqPlaces);

          
        }

        public static List<Model.Place> GetNearbyPlaces(Guid userId, double latitude, double longitude)
        {


            //construct the URI

            string uri = @"https://api.foursquare.com/v2/venues/explore?ll=" + latitude + "," + longitude + "&radius=500&limit=5&client_id=" + AppSettings.FoursquareClientID + "&client_secret=" + AppSettings.FoursquareClientSecret;

            if (Core.Accounts.Foursquare.HasFoursquareData(userId))
            {
                //get the access token, 
                string accesstoken = Core.Accounts.Foursquare.GetFoursquareData(userId).AccessToken;
                uri += @"&oauth_token=" + accesstoken;

            }

            //get the list of venues from foursquare. 
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            JObject obj = JObject.Parse(new StreamReader(response.GetResponseStream()).ReadToEnd());

            //save it to the database, if Its not there already. 


            //checks :) 
            if (obj.SelectToken("response") == null)
            {
                return null;
            }


            if (obj.SelectToken("response").SelectToken("groups") == null)
            {
                return null;
            }

            if (obj.SelectToken("response").SelectToken("groups")[0] == null)
            {
                return null;
            }


            if (obj.SelectToken("response").SelectToken("groups")[0].SelectToken("items") == null)
            {
                return null;
            }


            List<JToken> fqPlaces = obj.SelectToken("response").SelectToken("groups")[0].SelectToken("items").ToList();


            return ParseFoursquarePlaces(fqPlaces);


        }
 
        public static List<Model.Place> SearchPlaces(Guid userId, double latitude, double longitude, string query)
        {


            if (String.IsNullOrWhiteSpace(query))
            {
                return null;
            }

            //construct the URI

            string uri = @"https://api.foursquare.com/v2/venues/search?ll=" + latitude + "," + longitude + "&limit=10&client_id=" + AppSettings.FoursquareClientID + "&client_secret=" + AppSettings.FoursquareClientSecret + "&query=" + query+"&v=20130129";

            

            //get the list of venues from foursquare. 
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            JObject obj = JObject.Parse(new StreamReader(response.GetResponseStream()).ReadToEnd());

            //save it to the database, if Its not there already. 


            //checks :) 
            if (obj.SelectToken("response") == null)
            {
                return null;
            }


            if (obj.SelectToken("response").SelectToken("venues") == null)
            {
                return null;
            }

            if (obj.SelectToken("response").SelectToken("venues")   == null)
            {
                return null;
            }


            List<JToken> fqPlaces = obj.SelectToken("response").SelectToken("venues").ToList();


            return ParseFoursquareSearchPlaces(fqPlaces);


        }

        public static List<Model.Tag> GetPlaceTags(Guid placeId)
        {
            List<Model.PlaceTag> ven = Model.Table.PlaceTag.ExecuteQuery(new TableQuery<Model.PlaceTag>().Where(
                    TableQuery.GenerateFilterCondition("PlaceID", QueryComparisons.Equal, placeId.ToString())
                    )).ToList();

            List<Model.Tag> tags = new List<Model.Tag>();

            foreach (Model.PlaceTag tag in ven)
            {
                tags.Add(Core.Tags.Tags.GetTagByID(tag.TagID));
            }

            return tags;
            

        }

        public static Guid InsertPlaceTagIfNotExists(Guid placeId, string tokenName)
        {
            Model.Tag token = Core.Tags.Tags.InsertTagIfNotExists(tokenName);
            if (token != null)
            {
                Model.Place place =  Model.Table.Places.ExecuteQuery(new TableQuery<Model.Place>().Where(
                    TableQuery.GenerateFilterCondition("PlaceID", QueryComparisons.Equal, placeId.ToString())
                    )).FirstOrDefault();

                if (place != null)
                {
                    Model.PlaceTag placetoken = Model.Table.PlaceTag.ExecuteQuery(new TableQuery<Model.PlaceTag>().Where( TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PlaceID", QueryComparisons.Equal, placeId.ToString()), 
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("TagID",QueryComparisons.Equal,token.TagID.ToString())))).FirstOrDefault();
                    if (placetoken == null)
                    {
                        placetoken = new Model.PlaceTag(token.TagID,placeId);
                        placetoken.PlaceID = placeId;
                        placetoken.TagID = token.TagID;
                        placetoken.DateTimeStamp = DateTime.Now;
                        Model.Table.PlaceTag.Execute(TableOperation.Insert(placetoken));
                    }


                }

                return token.TagID;
            }

            return Guid.Empty;

            

        }

        public static bool DoesPlaceExists(string foursquareId)
        {
            if (Model.Table.Places.ExecuteQuery(new TableQuery<Model.Place>().Where(
                    TableQuery.GenerateFilterCondition("FoursquarePlaceID", QueryComparisons.Equal, foursquareId.ToString())
                    )).FirstOrDefault() == null)
            {
                return false;
            }

            return true;
        }

        #region Private Methods

        private static List<Model.Place> ParseFoursquarePlaces(List<JToken> fqPlaces)
        {

            List<Model.Place> places = new List<Model.Place>();

            foreach (JToken t in fqPlaces)
            {

                Model.Place place = new Model.Place();

                if (t.SelectToken("venue").SelectToken("id") != null)
                {
                    place.FoursquarePlaceID = t.SelectToken("venue").SelectToken("id").ToString();

                    if (DoesPlaceExists(place.FoursquarePlaceID))
                    {
                        places.Add(GetPlace(place.FoursquarePlaceID));
                        continue;
                    }
                }

                if (t.SelectToken("venue").SelectToken("name") != null)
                {
                    place.Name = t.SelectToken("venue").SelectToken("name").ToString();
                }
                if (t.SelectToken("venue").SelectToken("contact") != null)
                {
                    if (t.SelectToken("venue").SelectToken("contact").SelectToken("phone") != null)
                    {
                        place.Phone = t.SelectToken("venue").SelectToken("contact").SelectToken("phone").ToString();
                    }

                    if (t.SelectToken("venue").SelectToken("contact").SelectToken("twitter") != null)
                    {
                        place.Twitter = t.SelectToken("venue").SelectToken("contact").SelectToken("twitter").ToString();
                    }
                }

                Model.Location location = new Model.Location();

                if (t.SelectToken("venue").SelectToken("location") != null)
                {

                    if (t.SelectToken("venue").SelectToken("location").SelectToken("lat") != null)
                    {
                        location.Latitude = Double.Parse(t.SelectToken("venue").SelectToken("location").SelectToken("lat").ToString());
                    }

                    if (t.SelectToken("venue").SelectToken("location").SelectToken("lng") != null)
                    {
                        location.Longitude = Double.Parse(t.SelectToken("venue").SelectToken("location").SelectToken("lng").ToString());
                    }

                    
                    location = Core.Location.Location.InsertLocation(location);
                   

                    if (location.CityID == null)
                    {
                        continue;
                    }
                }

                place.LocationID = location.LocationID;
                if (t.SelectToken("venue").SelectToken("canonicalUrl") != null)
                {
                    place.FoursquareCannonicalURL = t.SelectToken("venue").SelectToken("canonicalUrl").ToString();
                }

                //add a venue

                Model.Table.Places.Execute(TableOperation.Insert(place));

                //insert tokens now. :)

                if (t.SelectToken("venue").SelectToken("categories") != null)
                {
                    List<JToken> x = t.SelectToken("venue").SelectToken("categories").ToList();

                    foreach (JToken q in x)
                    {
                        string name = q.SelectToken("name").ToString();
                        InsertPlaceTagIfNotExists(place.PlaceID, name);
                    }
                }

                places.Add(place);

            }

            //return the list. 
            return places;
        }

        private static Model.Place GetPlace(string foursquareId)
        {
            return Model.Table.Places.ExecuteQuery(new TableQuery<Model.Place>().Where(
                    TableQuery.GenerateFilterCondition("FoursquarePlaceID", QueryComparisons.Equal, foursquareId.ToString())
                    )).FirstOrDefault();
            
        }

        private static List<Model.Place> ParseFoursquareSearchPlaces(List<JToken> fqPlaces)
        {

            List<Model.Place> places = new List<Model.Place>();

            foreach (JToken t in fqPlaces)
            {

                Model.Place place = new Model.Place();

                if (t.SelectToken("id") != null)
                {
                    place.FoursquarePlaceID = t.SelectToken("id").ToString();

                    if (DoesPlaceExists(place.FoursquarePlaceID))
                    {
                        places.Add(GetPlace(place.FoursquarePlaceID));
                        continue;
                    }
                }

                if (t.SelectToken("name") != null)
                {
                    place.Name = t.SelectToken("name").ToString();
                }
                if (t.SelectToken("contact") != null)
                {
                    if (t.SelectToken("contact").SelectToken("phone") != null)
                    {
                        place.Phone = t.SelectToken("contact").SelectToken("phone").ToString();
                    }

                    if (t.SelectToken("contact").SelectToken("twitter") != null)
                    {
                        place.Twitter = t.SelectToken("contact").SelectToken("twitter").ToString();
                    }
                }

                Model.Location location = new Model.Location();

                if (t.SelectToken("location") != null)
                {

                    if (t.SelectToken("location").SelectToken("lat") != null)
                    {
                        location.Latitude = Double.Parse(t.SelectToken("location").SelectToken("lat").ToString());
                    }

                    if (t.SelectToken("location").SelectToken("lng") != null)
                    {
                        location.Longitude = Double.Parse(t.SelectToken("location").SelectToken("lng").ToString());
                    }

                   
                    location = Core.Location.Location.InsertLocation(location);
                   
                }

                place.LocationID = location.LocationID;
                if (t.SelectToken("canonicalUrl") != null)
                {
                    place.FoursquareCannonicalURL = t.SelectToken("canonicalUrl").ToString();
                }

                //add a venue

                Model.Table.Places.Execute(TableOperation.Insert(place));

                //insert tokens now. :)

                if (t.SelectToken("categories") != null)
                {
                    List<JToken> x = t.SelectToken("categories").ToList();

                    foreach (JToken q in x)
                    {
                        string name = q.SelectToken("name").ToString();
                        InsertPlaceTagIfNotExists(place.PlaceID, name);
                    }
                }

                places.Add(place);

            }

            //return the list. 
            return places;
        }

        #endregion


        internal static Model.Place GetPlace(Guid id)
        {
            Core.Cloud.TableStorage.InitializePlacesTable();


            return Model.Table.Places.ExecuteQuery(new TableQuery<Model.Place>().Where(
                    TableQuery.GenerateFilterCondition("PlaceID", QueryComparisons.Equal, id.ToString())
                    )).FirstOrDefault();
        }



        internal static int GetNoOfFollowers(Guid placeId)
        {
            return Model.Table.UserPlaceFollow.ExecuteQuery(new TableQuery<Model.UserPlaceFollow>().Where(
                    TableQuery.GenerateFilterCondition("PlaceID", QueryComparisons.Equal, placeId.ToString())
                    )).Count();
        }

        internal static int GetNoOfPlacesFollowing(Guid userId)
        {
            return Model.Table.UserPlaceFollow.ExecuteQuery(new TableQuery<Model.UserPlaceFollow>().Where(
                    TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId)
                    )).Count();
        }

        public static CheckInStatus CheckIn(Guid userId, Guid placeId, double lat,double lon)
        {
            if (!IsCheckedIn(userId, placeId, lat, lon))
            {
                if (CanCheckedIn(userId, placeId, lat, lon))
                {
                    try
                    {
                        Model.UserCheckins checkin = new Model.UserCheckins();
                        checkin.UserID=userId;
                        checkin.PlaceID = placeId;
                        checkin.DateTimeStamp = DateTime.Now;
                        checkin.CheckedIn = true;

                        Model.Table.UserCheckins.Execute(TableOperation.Insert(checkin));

                        return CheckInStatus.CheckedIn;

                    }
                    catch(Exception e)
                    {
                        Core.Exceptions.ExceptionReporting.AddAnException(userId, ClientType.WindowsAzure, e);
                        return CheckInStatus.Error;
                    }
                }
                else
                {
                    return CheckInStatus.FarAway;
                }

            }

            return CheckInStatus.AlreadyCheckedIn;
        }

        public static bool IsCheckedIn(Guid userId, Guid placeId,double lat,double lon)
        {

            Core.Cloud.TableStorage.InitializeUserCheckinsTable();

            DateTime minus8=DateTime.Now-new TimeSpan(8,0,0);
            if(Model.Table.UserCheckins.ExecuteQuery(new TableQuery<Model.UserCheckins>().Where(TableQuery.CombineFilters(
            TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("CheckedIn", QueryComparisons.Equal, true.ToString()), TableOperators.And,
                TableQuery.GenerateFilterCondition("PlaceID", QueryComparisons.Equal, placeId.ToString())), TableOperators.And, TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.UserCheckins.GetPartitionKey(placeId)), TableOperators.And, 
                TableQuery.GenerateFilterCondition("DateTimeStamp", QueryComparisons.GreaterThan, minus8.ToString())
                )))).FirstOrDefault() !=null)
            {

                

                Model.Location venueLocation = Core.Location.Location.GetLocation(Core.Location.Place.GetPlace(placeId).LocationID);

                if (venueLocation != null)
                {
                    double dis = Core.Location.Distance.CalculateDistance(venueLocation.Latitude, venueLocation.Longitude, lat, lon);

                    if (dis > 0.3) //is distance is greater than 0.5 then return false;
                    {
                        Checkout(userId, placeId, false, lat, lon);
                        return false;
                    }

                    return true;
                }
            }

            return false;

        }

        public static bool CanCheckedIn(Guid userId, Guid placeId, double lat, double lon)
        {
            if (IsCheckedIn(userId, placeId, lat, lon))
            {
                return false;
            }

            Model.Location venueLocation = Core.Location.Location.GetLocation(Core.Location.Place.GetPlace(placeId).LocationID);

            if (venueLocation != null)
            {
                double dis = Core.Location.Distance.CalculateDistance(venueLocation.Latitude, venueLocation.Longitude, lat, lon);

                if (dis > 0.5) //is distance is greater than 0.5 then return false;
                {
                    return false;
                }

                return true;
            }


            return false;
        }

        public static CheckInStatus Checkout(Guid userId, Guid placeId,bool intended, double lat, double lon)
        {
            Core.Cloud.TableStorage.InitializeUserCheckinsTable();

            if (IsCheckedOut(userId, placeId))
            {
                Model.UserCheckins checkin = new Model.UserCheckins();
                checkin.UserID=userId;
                checkin.PlaceID = placeId;
                checkin.DateTimeStamp = DateTime.Now;
                checkin.CheckedIn = false;

                Model.Table.UserCheckins.Execute(TableOperation.Insert(checkin));
                
            }
            return CheckInStatus.CheckedOut;
        }

        public static bool IsCheckedOut(Guid userId, Guid placeId)
        {
            Model.UserCheckins checkin = Model.Table.UserCheckins.ExecuteQuery(new TableQuery<Model.UserCheckins>().Where(TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("PlaceID", QueryComparisons.Equal, placeId.ToString()), TableOperators.And, TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.UserCheckins.GetPartitionKey(placeId)), TableOperators.And, 
                TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId)
                )))).FirstOrDefault();

            if (checkin == null)
            {
                return true;
            }

            if (checkin.CheckedIn)
            {
                return false;
            }

            return true;
        }



        public static void NotifyPlaceCheckIn(Guid placeId, Guid userId)
        {
            List<Guid> notifiers = new List<Guid>();

            notifiers.AddRange(Core.Location.UserPlaceFollow.GetFollowersList(placeId));

            notifiers.AddRange(Core.Users.Follow.GetFollowersList(userId, 99999999, new List<Guid>()));

            notifiers.AddRange(GetPeopleAtPlace(placeId));

            notifiers = notifiers.Distinct().ToList();

            Model.Place place=Core.Location.Place.GetPlace(placeId);

            if (notifiers.Contains(userId))
            {
                notifiers.Remove(userId);
            }

            List<Guid> peopleAtPlace = GetPeopleAtPlace(placeId);
            peopleAtPlace = peopleAtPlace.Distinct().ToList();
            foreach (Guid id in notifiers)
            {
                List<Guid> tmp = new List<Guid>(peopleAtPlace);

                int venueCount = tmp.Count;

                if (tmp.Contains(id))
                {
                    tmp.Remove(id);
                }

                tmp = tmp.Take(5).ToList();
                tmp = tmp.Distinct().ToList();

                if(tmp.Count>0)
                {

                    Model.UserProfile profile=Core.Accounts.User.GetUserProfile(userId);
                    tmp = tmp.Distinct().ToList();

                    

                    Core.Notifications.Notification.AddNotification(id, "PEOPLE AT <bold> " + place.Name + " </bold>", "", "<bold> " + profile.Name + " </bold> is at " + place.Name, tmp, placeId.ToString(), userId.ToString(), "Place", "User", "PlaceCheckIn");
                    

                }

                
            }

        }

        private static List<Guid> GetPeopleAtPlace(Guid placeId)
        {
             
            UserPlaceCheckInComparer comp=new UserPlaceCheckInComparer();

            DateTime minus8=DateTime.Now-new TimeSpan(8,0,0);

            List<Guid> userIds =  Model.Table.UserCheckins.ExecuteQuery(new TableQuery<Model.UserCheckins>().Where(TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("PlaceID", QueryComparisons.Equal, placeId.ToString()), TableOperators.And, TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.UserCheckins.GetPartitionKey(placeId)), TableOperators.And, 
                TableQuery.GenerateFilterCondition("DateTimeStamp", QueryComparisons.GreaterThan, minus8.ToString())
                )))).ToList().Distinct(comp).Where(o=>o.CheckedIn==true).Select(o=>o.UserID).ToList();
            
            
            
            return userIds;
        }

        internal static int NoOfPlacesCheckedIn(Guid userId)
        {

            return Model.Table.UserCheckins.ExecuteQuery(new TableQuery<Model.UserCheckins>().Where(TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("CheckedIn", QueryComparisons.Equal, true.ToString()), TableOperators.And, 
                TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId)
                ))).Select(o => o.PlaceID).Distinct().Count();
            
        }

        internal static void CheckPlaceCheckedIn(Guid userId,double lat,double lon)
        {
            List<Model.Place> checkIn = GetPlacesCheckedIn(userId);

            foreach (Model.Place place in checkIn)
            {
                IsCheckedIn(userId, place.PlaceID, lat, lon);
            }
        }

        private static List<Model.Place> GetPlacesCheckedIn(Guid userId)
        {

            Core.Cloud.TableStorage.InitializeUserCheckinsTable();


            PlaceDistinct obj=new PlaceDistinct();
            List<Guid> placeIds =  Model.Table.UserCheckins.ExecuteQuery(new TableQuery<Model.UserCheckins>().Where(
                TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId)
                )).ToList().Distinct(obj).Where(o => o.CheckedIn).Select(o=>o.PlaceID).ToList(); 


            List<Model.Place> places=new List<Model.Place>();

            foreach(Guid id in placeIds)
            {
                places.Add(Core.Location.Place.GetPlace(id));
            }

            return places;
            
        }

        

        internal static int GetNoOfPeopleCurrentlyCheckedIn(Guid placeId)
        {
            UserPlaceCheckInComparer obj = new UserPlaceCheckInComparer();


            return Model.Table.UserCheckins.ExecuteQuery(new TableQuery<Model.UserCheckins>().Where(TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.UserCheckins.GetPartitionKey(placeId).ToString()), TableOperators.And, 
                TableQuery.GenerateFilterCondition("PlaceID", QueryComparisons.Equal, placeId.ToString())
                ))).ToList().Distinct(obj).Where(o => o.CheckedIn).Count();
            
            
           
        }

        internal static List<Model.User> GetPeopleCheckedIn(Guid placeId, int take, List<Guid> skipList)
        {    
            UserPlaceCheckInComparer obj = new UserPlaceCheckInComparer();


            DateTime minus5=DateTime.Now-new TimeSpan(5,0,0);
             List<Guid> userid =  Model.Table.UserCheckins.ExecuteQuery(new TableQuery<Model.UserCheckins>().Where(TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("PlaceID", QueryComparisons.Equal, placeId.ToString()), TableOperators.And, TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.UserCheckins.GetPartitionKey(placeId)), TableOperators.And, 
                TableQuery.GenerateFilterCondition("DateTimeStamp", QueryComparisons.GreaterThan, minus5.ToString())
                )))).ToList().Where(o=>!skipList.Contains(o.UserID)).ToList().Distinct(obj).Where(o => o.CheckedIn).Select(o => o.UserID).ToList();



             List<Model.User> users = new List<Model.User>();

             foreach (Guid id in userid)
             {
                 users.Add(Core.Accounts.User.GetUser(id));
             }

             return users;
            
        }

        public static void SyncFoursquarePlaces(Guid userId)
        {

            Model.FoursquareData data = Core.Accounts.Foursquare.GetFoursquareData(userId);

            if (data == null)
            {
                return;
            }

            if(data.AccessToken==null)
            {
               return;
            }

            //construct the URI

            //get previous offset. :)

            int count= Model.Table.UserCheckins.ExecuteQuery(new TableQuery<Model.UserCheckins>().Where(TableQuery.CombineFilters(
                TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId), TableOperators.And, 
                TableQuery.GenerateFilterCondition("FoursquareCheckinID", QueryComparisons.NotEqual, "")
                ))).Count();
                

            string uri = @"https://api.foursquare.com/v2/users/self/checkins?oauth_token="+data.AccessToken+"&v=20130129&offset="+count+"&sort=newestfirst";



            //get the list of venues from foursquare. 
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            JObject obj = JObject.Parse(new StreamReader(response.GetResponseStream()).ReadToEnd());

            //save it to the database, if Its not there already. 

            

            //checks :) 
            if (obj.SelectToken("response") == null)
            {
                return;
            }

            if (obj.SelectToken("response").SelectToken("checkins") == null)
            {
                return;
            }

            List<JToken> checkins = obj.SelectToken("response").SelectToken("checkins").SelectToken("items").ToList();

          

            foreach (JToken checkin in checkins)
            {

                //get the place first and then link checkin data. 

                JToken place = checkin.SelectToken("venue");
                List<JToken> tmpList=new List<JToken>();
                tmpList.Add(place);
                List<Model.Place> places = ParseFoursquareSearchPlaces(tmpList);


                if (places.Count > 0)
                {


                    Model.Place venue = places.FirstOrDefault();

                    //perform checkin if doesnt exists
                    string checkInId=checkin.SelectToken("id").ToString();
                    Model.UserCheckins check_in = Model.Table.UserCheckins.ExecuteQuery(new TableQuery<Model.UserCheckins>().Where(
                TableQuery.GenerateFilterCondition("FoursquareCheckinID", QueryComparisons.NotEqual, checkInId.ToString())
                )).FirstOrDefault();

                    if (check_in == null)
                    {
                        //perform checkin
                      
                        check_in = new Model.UserCheckins();

                        check_in.UserID=userId;
                        check_in.CheckedIn = true;
                        check_in.FoursquareCheckinID = checkin.SelectToken("id").ToString();
                        check_in.PlaceID = venue.PlaceID;

                        //long epoch conversion. But ya, Great! 
                        var utcDate = DateTime.Now.ToUniversalTime();
                        long baseTicks = 621355968000000000;
                        long tickResolution = 10000000;
                        long epoch = Convert.ToInt64(checkin.SelectToken("createdAt").ToString());
                        long epochTicks = (epoch * tickResolution) + baseTicks;
                        check_in.DateTimeStamp = new DateTime(epochTicks, DateTimeKind.Utc);


                        Model.Table.UserCheckins.Execute(TableOperation.InsertOrReplace(check_in));

                        


                    }

                    
                }




            }

            
        }
    }


    class UserPlaceCheckInComparer : IEqualityComparer<Model.UserCheckins>
    {

        public bool Equals(Model.UserCheckins x, Model.UserCheckins y)
        {

            if (x.UserID != null && y.UserID != null)
            {

                if (x.UserID == y.UserID)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;

        }

        public int GetHashCode(Model.UserCheckins obj)
        {
            return obj.GetHashCode();
        }
    }

    class PlaceDistinct : IEqualityComparer<Model.UserCheckins>
    {

        public bool Equals(Model.UserCheckins x, Model.UserCheckins y)
        {

            if (x.PlaceID != null && y.PlaceID != null)
            {

                if (x.PlaceID == y.PlaceID)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;

        }

        public int GetHashCode(Model.UserCheckins obj)
        {
            return obj.GetHashCode();
        }
    }
}