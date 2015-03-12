using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangout.Client.Core.Location
{
    class Location
    {

        public static UserLocationService.City SelectedCity;

        public static System.Collections.ObjectModel.ObservableCollection<UserLocationService.City> GetLocationList()
        {
            return Settings.Settings.LocationList;
        }

        public static void AddLocation(UserLocationService.City city)
        {
            if (city != null)
            {


                System.Collections.ObjectModel.ObservableCollection<UserLocationService.City> list = GetLocationList();

                if(list.Where(o=>o.Id==city.Id).Count()==0)
                {
                    //add
                    list.Add(city);
                    Settings.Settings.LocationList = list;
                }
            }
            
        }
    }
}
