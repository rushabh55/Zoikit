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
using System.Threading;
using System.Windows.Threading;

namespace Hangout.Client
{
    public partial class UserShortDisplay : UserControl
    {
        public UserShortDisplay()
        {
            InitializeComponent();
        }


        private delegate void UserSelectedHelper(int id);

        private  event UserSelectedHelper UserSelected;


        public NotificationService.CompactUser User
        {
            get { return (NotificationService.CompactUser)GetValue(UserProperty); }

            set
            {
                SetValue(UserProperty, value);
            }
        }

        public void DisplayData(NotificationService.CompactUser user)
        {
           

            if (user.ProfilePicURL != null)
            {
                ProfilePicImg.Source = new BitmapImage(new Uri(user.ProfilePicURL, UriKind.Absolute));
            }

            this.LayoutRoot.Visibility = System.Windows.Visibility.Visible;

            //start storyboards 

            
        }

        public static readonly DependencyProperty UserProperty =
        DependencyProperty.Register(
            "User",
            typeof(NotificationService.CompactUser),
            typeof(Client.UserShortDisplay),
            new PropertyMetadata(null, UserValueChanged));


        private static void UserValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Client.UserShortDisplay obj = (Client.UserShortDisplay)d;
            obj.DisplayData((NotificationService.CompactUser)e.NewValue);
        }

        private void LayoutRoot_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (User != null && UserSelected != null)
            {
                UserSelected(User.UserID);
            }

        }


    }
}
