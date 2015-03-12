using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace Hangout.Web.Core.Location
{
    public class City
    {


        public static List<Model.City> Search(string query)
        {

            Core.Cloud.TableStorage.InitializeCityTable();

            List<Model.City> city=Model.Table.City.ExecuteQuery(new TableQuery<Model.City>()).Where(o => o.Name.ToLower().StartsWith(query.ToLower())).ToList();

            return city;
        }

        public static Model.City InsertCityIfNotExists(string cityName, string countryName)
        {
            Core.Cloud.TableStorage.InitializeCountryTable();
            Core.Cloud.TableStorage.InitializeCityTable();

            Model.Country country=Model.Table.Country.ExecuteQuery(new TableQuery<Model.Country>().Where(TableQuery.GenerateFilterCondition("CountryName", QueryComparisons.Equal, countryName.ToString()))).FirstOrDefault();

            Guid countryId; 
            if(country==null)
            {
                countryId=Core.Location.Country.InsertCountryIfNotExists(countryName);
            }
            else
            {
                countryId = country.CountryID;
            }

            Model.City city = Model.Table.City.ExecuteQuery(new TableQuery<Model.City>().Where(TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("Name", QueryComparisons.Equal, cityName.ToString()),TableOperators.And, TableQuery.GenerateFilterConditionForGuid("CountryID", QueryComparisons.Equal, country.CountryID)
                ))).FirstOrDefault();
            if (city == null)
            {
                city = new Model.City(cityName, countryId);
                city.Name = cityName;


                Model.Table.City.Execute(TableOperation.Insert(city));

                return city;
            }
            else
            {
                return city;
            }
        }


        public static Model.City GetCityByLocation(double latitude, double longitude)
        {

            Core.Cloud.TableStorage.InitializeCityTable();


            Model.Location loc = new Model.Location(latitude, longitude);

            if(loc.CityID!=null)
            {
                return GetCityById(loc.CityID);
            }

            return null;

        }


        public static Model.City GetCityById(Guid id)
        {
            Core.Cloud.TableStorage.InitializeCityTable();

            return Model.Table.City.ExecuteQuery(new TableQuery<Model.City>().Where(TableQuery.CombineFilters( TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.City.GetPartitionKey(id)),TableOperators.And,
                TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, id.ToString())))).FirstOrDefault();
        }

        public static List<Guid> GetuserIdsFromUserCity(Guid userId)
        {
            Core.Cloud.TableStorage.InitializeCityTable();

            Model.City city = Core.Location.UserLocation.GetUserCity(userId);

            if (city != null)
            {
                List<Guid> userIds = Model.Table.UserLocations.ExecuteQuery(new TableQuery<Model.UserLocation>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.UserLocation.GetPartitonKeyByCityID(city.CityID)))).ToList().Select(o => o.UserID).Distinct().ToList();
                
               

                if (userIds.Contains(userId))
                {
                    userIds.Remove(userId);
                }

                return userIds;
            }

            return null;
        }







        public static void ChangeUserCity(Guid userId, Guid oldCityId, Guid newCityId)
        {
            Core.Cloud.TableStorage.InitializePushNotificationsTable();

            Model.PushNotification push = Core.PushNotifications.PushNotifications.GetPushNotification(userId);

            if (push != null)
            {

                Model.Table.PushNotifications.Execute(TableOperation.Delete(push));
                push.PartitionKey = newCityId.ToString();
                Model.Table.PushNotifications.Execute(TableOperation.Insert(push));
            }

            Core.Cloud.TableStorage.InitializeUserTagFollowTable();
            Core.Cloud.TableStorage.InitializeLocalTagFollowCount();


            List<Guid> follows = Core.Tags.Follow.GetTagIdFollowing(userId, 999999, new List<Guid>());

            if (follows != null)
            {

                foreach (Guid id in follows)
                {
                    Model.UserTagFollow follow = Core.Tags.Follow.GetUserTagFollow(userId, id, oldCityId);

                    if (follow != null)
                    {
                        Model.Table.UserTagFollow.Execute(TableOperation.Delete(follow));
                        follow.PartitionKey = Model.UserTagFollow.GetPartitionKey(userId, newCityId);
                        Model.Table.UserTagFollow.Execute(TableOperation.InsertOrReplace(follow));

                        Core.Tags.Follow.ChangeLocalTagFollowCount(id, oldCityId, -1);

                        Core.Tags.Follow.ChangeLocalTagFollowCount(id, newCityId, 1);
                    }
                }
            }
        }
    }
}