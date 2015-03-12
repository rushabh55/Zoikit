using Coding4Fun.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace Hangout.Client
{
    public class ChatBubbleDirectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            TextService.Text t = (TextService.Text)value;

            if(parameter==null)
            {
                if (t.User.UserID == Core.User.User.UserID)
                {
                    return ChatBubbleDirection.LowerRight;
                }
                else
                {
                    return ChatBubbleDirection.UpperLeft;
                }
            }

            else if (parameter.ToString() == "Colour")
            {
                if (t.User.UserID == Core.User.User.UserID)
                {
                    return new SolidColorBrush(Color.FromArgb(255, 216, 148, 0));
                }
                else
                {
                    return new SolidColorBrush(Color.FromArgb(255, 255, 175, 4));
                }
            }

            return null;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
