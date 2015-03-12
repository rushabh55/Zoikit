using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;

namespace Hangout.Client.Core.Converters
{
    public class DateTimeToStringConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                value = ((DateTime)value).ToLocalTime();
                var ts = DateTime.Now - ((DateTime)value);
                double delta = Math.Abs(ts.TotalSeconds);

                const int SECOND = 1;
                const int MINUTE = 60 * SECOND;
                const int HOUR = 60 * MINUTE;
                const int DAY = 24 * HOUR;
                const int MONTH = 30 * DAY;

                if (delta < 0)
                {
                    return "not yet";
                }
                if (delta < 1 * MINUTE)
                {
                    return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";
                }
                if (delta < 2 * MINUTE)
                {
                    return "a minute ago";
                }
                if (delta < 45 * MINUTE)
                {
                    return ts.Minutes + " minutes ago";
                }
                if (delta < 90 * MINUTE)
                {
                    return "an hour ago";
                }
                if (delta < 24 * HOUR)
                {
                    return ts.Hours + " hours ago";
                }
                if (delta < 48 * HOUR)
                {
                    return "yesterday";
                }
                if (delta < 30 * DAY)
                {
                    return ts.Days + " days ago";
                }
                if (delta < 12 * MONTH)
                {
                    int months = System.Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                    return months <= 1 ? "one month ago" : months + " months ago";
                }
                else
                {
                    int years = System.Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                    return years <= 1 ? "one year ago" : years + " years ago";
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                return "";
            }

        }

       

        

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
