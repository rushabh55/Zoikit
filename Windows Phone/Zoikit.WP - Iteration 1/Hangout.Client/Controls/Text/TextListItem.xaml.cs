using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Hangout.Client.Controls.Text
{
    public partial class TextListItem : UserControl
    {
        public TextListItem()
        {
            InitializeComponent();
        }


        public TextService.Text Text
        {
            get { return (TextService.Text)GetValue(TextProperty); }

            set
            {
                SetValue(TextProperty, value);
            }
        }

        public delegate void IdHelper(Guid id);

        public event IdHelper TextSelected;

        public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(
            "Text",
            typeof(TextService.Text),
            typeof(TextListItem),
            new PropertyMetadata(null, TextValueChanged));


        private static void TextValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextListItem obj = (TextListItem)d;
            obj.DisplayData((TextService.Text)e.NewValue);
        }

        public void DisplayData(TextService.Text text)
        {
            Core.Converters.DateTimeToStringConverter convert = new Core.Converters.DateTimeToStringConverter();

            PROFILE_PIC_LBL.Source = new BitmapImage(new Uri(text.User.ProfilePicURL));

            NAMELBL.Text = text.User.Name;

            MessageLBL.Text = text.TextMessage;

            if(!text.MarkAsRead)
            {
                //turn on the effects! 
                MessageLBL.Foreground = new SolidColorBrush(Color.FromArgb(255, 238, 123, 68));
            }
            

            PostedLBL.Text = convert.Convert(text.DateTimeStamp, null, null, null).ToString();
        }

        private void StackPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
        	if(TextSelected!=null)
            {
                this.Text.MarkAsRead = true;
                MessageLBL.Foreground = new SolidColorBrush(Color.FromArgb(255, 75, 75, 75));
                TextSelected(Text.User.UserID);
            }
        }
    }
}
