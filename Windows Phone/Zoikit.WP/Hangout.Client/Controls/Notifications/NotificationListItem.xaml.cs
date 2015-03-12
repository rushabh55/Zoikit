﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using System.Windows.Documents;
using System.Windows.Media;

namespace Hangout.Client.Controls.Notifications
{
    public partial class NotificationListItem : UserControl
    {


        public delegate void IdHelper(int id);

        public event IdHelper BuzzSelected;
        public event IdHelper TrophySelected;
        public event IdHelper TagSelected;
        public event IdHelper CategorySelected;
        public event IdHelper UserSelected;
        public event IdHelper VenueSelected;

        NotificationService.Notification x;

        public NotificationListItem()
        {
            InitializeComponent();
            Loaded += NotificationListItem_Loaded;
            TiltEffect.TiltableItems.Add(typeof(Grid));
        }

        void NotificationListItem_Loaded(object sender, RoutedEventArgs e)
        {
            if (Notifi != null)
            {
                double height = 0;

                if (TitleBlock.Visibility == System.Windows.Visibility.Visible)
                {
                    height += TitleBlock.ActualHeight;
                }

                if (SP1.Visibility == System.Windows.Visibility.Visible)
                {
                    height += SP1.ActualHeight;
                }

                if (SP2.Visibility == System.Windows.Visibility.Visible)
                {
                    height += SP2.ActualHeight;
                }


                MarkAsRedGuy.Height = height;
            }
        }



        public NotificationService.Notification Notifi
        {
            get { return (NotificationService.Notification)GetValue(NotificationProperty); }

            set
            {
                SetValue(NotificationProperty, value);
            }
        }
        
        public static readonly DependencyProperty NotificationProperty =
       DependencyProperty.Register(
           "Notifi",
           typeof(NotificationService.Notification),
           typeof(Hangout.Client.Controls.Notifications.NotificationListItem),
           new PropertyMetadata(null, NotificationValueChanged));


        private static void NotificationValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Hangout.Client.Controls.Notifications.NotificationListItem obj = (Hangout.Client.Controls.Notifications.NotificationListItem)d;
            obj.DisplayData((NotificationService.Notification)e.NewValue);
        }

        private void DisplayData(NotificationService.Notification notification)
        {
            if (String.IsNullOrEmpty(notification.Title))
            {
                TitleBlock.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {

                TitleBlock.Text = "";
                String[] words = notification.Title.Split(' ');

                bool sw = false;
                foreach (string word in words)
                {
                    if (word == "<bold>")
                    {
                        sw = true;
                        continue;
                    }
                    if (word == "</bold>")
                    {
                        sw = false;
                        continue;
                    }

                    if (sw)
                    {
                        TitleBlock.Inlines.Add(new Run() { Text = (word + " ").ToUpper(), Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 175, 4)) });
                    }
                    else
                    {
                        TitleBlock.Inlines.Add(new Run() { Text = (word + " ").ToUpper() });
                    }

                }
            }

            if (!String.IsNullOrEmpty(notification.ProfilePicURL))
            {
                ProfilePic.Source = new BitmapImage(new Uri(notification.ProfilePicURL, UriKind.RelativeOrAbsolute));
                FillDesc(notification.Description, Desc1);
                SP2.Visibility = System.Windows.Visibility.Collapsed;
                SP1.Visibility = System.Windows.Visibility.Visible;
                
            }
            else
            {

                if (notification.UserList.Count == 1)
                {
                    ProfilePic.Source = new BitmapImage(new Uri(notification.UserList.Last().ProfilePicURL, UriKind.RelativeOrAbsolute));
                    FillDesc(notification.Description, Desc1);
                    SP2.Visibility = System.Windows.Visibility.Collapsed;
                    SP1.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    UserList.Users = notification.UserList.ToList();
                    FillDesc(notification.Description, Desc2);
                    SP2.Visibility = System.Windows.Visibility.Visible;
                    SP1.Visibility = System.Windows.Visibility.Collapsed;
                }
            }

            Core.Converters.PostedAdder con=new Core.Converters.PostedAdder();

            PostedLBL.Text = con.Convert(notification.DatetimePosted, null, null, null).ToString();

            if (!notification.MarkAsRead)
            {
                MarkAsReadSB.Begin();
                Notifi.MarkAsRead = false;
            }
            else
            {
                MarkAsRedGuy.Opacity = 0;
            }

           
        }

        private void FillDesc(string p, TextBlock Desc2)
        {
            Desc2.Text = "";
            String[] words = p.Split(' ');

            bool sw=false;
            foreach (string word in words)
            {
                if (word == "<bold>")
                {
                    sw=true;
                    continue;
                }
                if (word == "</bold>")
                {
                    sw=false;
                    continue;
                }

                if (sw)
                {
                    Desc2.Inlines.Add(new Run() { Text = word + " ", Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 175, 4)) });
                }
                else
                {
                    Desc2.Inlines.Add(new Run() { Text = word + " "});
                }

            }
        }

        public event EventHandler GoToTextsPage;

        public event IdHelper GoToUserTextsPage;

        private void LayoutRoot_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (Notifi.Type2 == "Message" && Notifi.Type1 == "User")
            {
                if (GoToUserTextsPage != null)
                {
                    GoToUserTextsPage(Int32.Parse(Notifi.Param1));
                }
            }

            if (Notifi.Type2 == "Message")
            {
                if (GoToTextsPage != null)
                {
                    GoToTextsPage(null, new EventArgs());
                }
            }
            if (Notifi.Type2 == "Trophy")
            {
                if (TrophySelected != null)
                {
                    TrophySelected(Int32.Parse(Notifi.Param1));
                }

                return;
            }

            if (Notifi.Type1 == "Buzz")
            {
                if (BuzzSelected != null)
                {
                    BuzzSelected(Int32.Parse(Notifi.Param1));
                }
            }

            if (Notifi.Type1 == "Tag")
            {
                if (TagSelected != null)
                {
                    TagSelected(Int32.Parse(Notifi.Param1));
                }
            }

            if (Notifi.Type1 == "Venue")
            {
                if (VenueSelected != null)
                {
                    VenueSelected(Int32.Parse(Notifi.Param1));
                }
            }
            if (Notifi.Type1 == "User")
            {
                if (UserSelected != null)
                {
                    UserSelected(Int32.Parse(Notifi.Param1));
                }
            }

            if (Notifi.Type1 == "Category")
            {
                if (CategorySelected != null)
                {
                    CategorySelected(Int32.Parse(Notifi.Param1));
                }
            }

            



        }


    }
}
