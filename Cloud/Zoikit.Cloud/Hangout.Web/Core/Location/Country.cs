using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Core.Location
{
    public class Country
    {
        public static Guid InsertCountryIfNotExists(string countryName)
        {
            Core.Cloud.TableStorage.InitializeCountryTable();

            Model.Country country = Model.Table.Country.ExecuteQuery(new TableQuery<Model.Country>().Where(TableQuery.GenerateFilterCondition("CountryName", QueryComparisons.Equal, countryName.ToString()))).FirstOrDefault();
            if (country == null)
            {
                country = new Model.Country(countryName);
                country.CountryName = countryName;
                Model.Table.Country.Execute(TableOperation.Insert(country));
                return country.CountryID;
            }
            else
            {
                return country.CountryID;
            }

        }


        internal static Model.Country GetCountryByID(Guid countryId)
        {
            Core.Cloud.TableStorage.InitializeCountryTable();

            return Model.Table.Country.ExecuteQuery(new TableQuery<Model.Country>().Where(TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "1"),TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, countryId.ToString()))
                    )).FirstOrDefault();
        }
    }
}