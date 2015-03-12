using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Hangout.Client.Controls.Notifications
{
    public partial class NotificationControl : UserControl
    {

        public event EventHandler NotificationClicked;
        public event EventHandler NotificationDisplayed;
        public event EventHandler NotificationHidden;


        public NotificationControl()
        {
            InitializeComponent();
            TiltEffect.TiltableItems.Add(typeof(Grid));
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            Settings.Settings.NotificationCountChanged += Settings_NotificationCountChanged;

            if (Settings.Settings.UnreadNotifications > 0)
            {
                FillLBL();
            }
        }

        private void FillLBL()
        {
            if (Settings.Settings.UnreadNotifications > 0)
            {
                if (Settings.Settings.UnreadNotifications / 10 == 0)
                {
                    //if Its only one digit. then 

                    CountLBL.Text = "0" + Settings.Settings.UnreadNotifications;
                }
                else
                {
                    CountLBL.Text = "" + Settings.Settings.UnreadNotifications;
                }

                ShowPage.Begin();

                if (NotificationDisplayed != null)
                {
                    NotificationDisplayed(null, new EventArgs());
                }
            }
            else
            {
                HidePage.Begin();
                if (NotificationHidden != null)
                {
                    NotificationHidden(null, new EventArgs());
                }
            }
        }

        void Settings_NotificationCountChanged(object sender, EventArgs e)
        {
            FillLBL();
        }

        private void LayoutRoot_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (Settings.Settings.UnreadNotifications > 0)
            {
                if (NotificationClicked != null)
                {
                    NotificationClicked(null, new EventArgs());
                }
            }
        }


        public void DisplayBackground()
        {
            YellowBackRect.Opacity = 1;
        }
    }
}
