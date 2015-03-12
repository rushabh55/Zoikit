using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Hangout.Client
{
    public partial class TextListItem : UserControl
    {
        public TextListItem()
        {
            InitializeComponent();
            LayoutRoot.Tap += LayoutRoot_Tap;
        }

        public delegate void TextSelectedHelper(TextService.Text text);

        public event TextSelectedHelper TextSelected;



        void LayoutRoot_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (TextSelected != null)
            {
                TextSelected(TextMessage);
            }
        }



        public TextService.Text TextMessage
        {
            get { return (TextService.Text)GetValue(TextProperty); }

            set
            {
                SetValue(TextProperty, value);
            }
        }

        public static readonly DependencyProperty TextProperty =
       DependencyProperty.Register(
           "TextMessage",
           typeof(TextService.Text),
           typeof(Client.TextListItem),
           new PropertyMetadata(null, TextValueChanged));


        private static void TextValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Client.TextListItem obj = (Client.TextListItem)d;
            obj.DisplayData((TextService.Text)e.NewValue);
        }

        public void DisplayData(TextService.Text text)
        {
            if (text != null)
            {
                NAMELBL.Text = text.User.Name;
                MessageLBL.Text = text.TextMessage;
                PROFILE_PIC_LBL.Source = new BitmapImage(new Uri(text.User.ProfilePicURL, UriKind.RelativeOrAbsolute));
                if (text.MarkAsRead)
                {
                    MarkAsRedGuy.Opacity = 0;
                    MessageLBL.Foreground=new SolidColorBrush(Color.FromArgb(255,203,211,220));
                }
                else
                {
                    MarkAsRedGuy.Opacity = 1;
                    MessageLBL.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 175,4));
                }

                Core.Converters.DateTimeToStringConverter s=new Core.Converters.DateTimeToStringConverter();

                PostedLBL.Text = s.Convert(text.DateTimeStamp, null, null, null).ToString();
            }

        }


    }
}
