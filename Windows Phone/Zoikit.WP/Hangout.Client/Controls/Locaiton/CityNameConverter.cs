using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Hangout.Client.Controls.Locaiton
{
    class CityNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            UserLocationService.City city = (UserLocationService.City)value;
            if (city == null)
            {
                return "";
            }

            if (city.Name == null)
            {
                return "";
            }

            if (city.Name != null && city.Country == null)
            {
                return city.Name;
            }

            
            return city.Name + ", " + city.Country.Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
