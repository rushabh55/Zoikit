using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Hangout.Client.Core.Converters
{
    public class AccountTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //UserService.UserKnow val = (UserService.UserKnow)value;

            //UserService.SocialNetworkType type = (UserService.SocialNetworkType)val.Type;

            //if (type != null)
            //{
            //    if (type == UserService.SocialNetworkType.Facebook)
            //    {
            //        return "FACEBOOK";

            //    }


            //    if (type == UserService.SocialNetworkType.Twitter)
            //    {
            //        return "TWITTER";

            //    }


            //    if (type == UserService.SocialNetworkType.Foursquare)
            //    {
            //        return "FOURSQUARE";

            //    }

            //}

            return "ZOIK IT";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
