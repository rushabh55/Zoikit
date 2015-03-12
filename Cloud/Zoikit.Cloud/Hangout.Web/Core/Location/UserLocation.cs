using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Web;

namespace Hangout.Web.Core.Location
{
    public class UserLocation
    {

        public static Model.Location UpdateUserLocation(Guid userId,Model.Location location)
        {
            bool newflag = false;
            Guid oldCityId = new Guid();
            Model.Location l = GetLastLocation(userId);

            bool cityChange = false;

           

            Model.UserLocation userLocation = GetLastUserLocation(userId);
            if (l != null)
            {
                if (l.CityID != location.CityID)
                {
                    //citychanged
                    oldCityId = l.CityID;
                    cityChange = true;
                }
            }

            if (l == null)
            {
                
                newflag = true;
            }

            l = location;
           


            Model.Table.Locations.Execute(TableOperation.InsertOrReplace(l));

            if (newflag)
            {
                userLocation = new Model.UserLocation(userId, l.LocationID);
                userLocation.UserID = userId;
                userLocation.DateTimeStamp = DateTime.Now;
                Model.Table.UserLocations.Execute(TableOperation.InsertOrReplace(userLocation));
            }
           
            else
            {
                Model.Table.UserLocations.Execute(TableOperation.Delete(userLocation));

                Model.UserLocation uloc=new Model.UserLocation(userId, l.LocationID);

                
                uloc.UserID = userId;
                uloc.DateTimeStamp = DateTime.Now;
                Model.Table.UserLocations.Execute(TableOperation.InsertOrReplace(uloc));
            }



            if(cityChange&&oldCityId!=new Guid())
            {
                Core.Cloud.Queue.AddMessage("CityChange:" + userId + ":" + oldCityId.ToString()+":" + location.CityID);
            }

            return l;

            
        }

        private static Model.UserLocation GetLastUserLocation(Guid userId)
        {
            Core.Cloud.TableStorage.InitializeUserLocationsTable();

            return Model.Table.UserLocations.ExecuteQuery(new TableQuery<Model.UserLocation>().Where(TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId))).FirstOrDefault();
        }

        public static List<UserLocationData> GetNearestUsers(Guid userId,int radius)
        {
            //Model.Location lastLocation = GetLastLocation(userId);

            //if (lastLocation == null)
            //{
            //    return new List<UserLocationData>();
            //}

            //if(lastLocation.Locality==null)
            //{
            //    return new List<UserLocationData>();
            //}

            //List<int> userIds = .Locations.Where(o => o.Locality == lastLocation.Locality&&o.UserID!=null).Select(o => o.UserID.Value).ToList();

            //if (userIds.Contains(userId))
            //{
            //    userIds.Remove(userId);
            //}

            //List<UserLocationData> locationData = new List<UserLocationData>();

            //foreach (Guid userId Guid userIds)
            //{
            //    UserLocationData data = new UserLocationData();
            //    data.UserData = Core.Accounts.User.GetUserData(userId);
                

            //    if (data.UserData.Profile == null)
            //    {
            //        continue;
            //    }
            //    data.location = GetLastLocation(userId);
            //    if (data.location == null)
            //    {
            //        continue;
            //    }


            //    locationData.Add(data);
                
            //}

            //return locationData;
            return null;
        }

        public static Model.Location GetLastLocation(Guid userId)
        {
            Core.Cloud.TableStorage.InitializeLocationTable();
            Core.Cloud.TableStorage.InitializeUserLocationsTable();

            Model.UserLocation location = Model.Table.UserLocations.ExecuteQuery(new TableQuery<Model.UserLocation>().Where(TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId))).FirstOrDefault();
            
            if (location == null)
            {
                return null;
            }

            return Core.Location.Location.GetLocation(location.LocationID);
        }

        public static Model.Location GetCurrentLocation(Guid userId)
        {

           DateTime minus30=DateTime.Now-new TimeSpan(0,30,0);

            Model.UserLocation location = Model.Table.UserLocations.ExecuteQuery(new TableQuery<Model.UserLocation>().Where(TableQuery.CombineFilters(
                TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId), TableOperators.And, 
                TableQuery.GenerateFilterCondition("DateTimeStamp", QueryComparisons.GreaterThan, minus30.ToString())
                ))).FirstOrDefault();
            
           
            if (location == null)
            {
                return null;
            }

            return Core.Location.Location.GetLocation(location.LocationID);
            
        }

        public  static Model.City GetUserCity(Guid userId)
        {
            Model.Location userLocation = Core.Location.UserLocation.GetLastLocation(userId);
            if (userLocation == null)
            {
                return null;
            }

            return Core.Location.City.GetCityById(userLocation.CityID);
        }



       

       

        
    }
}