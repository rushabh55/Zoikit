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

namespace Hangout.Client.Core.Location
{
    public class DistanceConverter : IValueConverter
    {
        

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (Settings.Settings.UserData!=null)
                {
                    
                    if (!String.IsNullOrWhiteSpace(Settings.Settings.UserData.DefaultLengthUnits))
                    {
                        if (Settings.Settings.UserData.DefaultLengthUnits.ToLower() == "miles")
                        {
                            return (int)Distance.ConvertToMiles((double)value) + " Miles";
                        }
                        else
                        {
                            return System.Convert.ToInt32(value.ToString()) + " KM";
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
           
            throw new NotImplementedException();
        }
    }
}
