using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows.Media;

namespace Hangout.Client.Controls.Buzz
{
    public partial class BuzzListItem : UserControl
    {
        public BuzzListItem()
        {
            InitializeComponent();
        }

        public BuzzService.Buzz Buzz
        {
            get { return (BuzzService.Buzz)GetValue(BuzzProperty); }

            set
            {
                SetValue(BuzzProperty, value);
            }
        }

        public delegate void IdHelper(int id);

        public event IdHelper UserSelected;

        public event IdHelper BuzzSelected;


        public static readonly DependencyProperty BuzzProperty =
        DependencyProperty.Register(
            "Buzz",
            typeof(BuzzService.Buzz),
            typeof(BuzzListItem),
            new PropertyMetadata(null, BuzzValueChanged));


        private static void BuzzValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BuzzListItem obj = (BuzzListItem)d;
            obj.DisplayData((BuzzService.Buzz)e.NewValue);
        }

        public void DisplayData(BuzzService.Buzz buzz)
        {
            Core.Converters.PostedAdder obj=new Core.Converters.PostedAdder();
            AddBuzzText("\"" + buzz.Text + "\"");
            postedLBL.Text = obj.Convert(buzz.Posted, null, null, System.Globalization.CultureInfo.CurrentCulture).ToString();
            Username.Text = buzz.User.Name.ToUpper();

            if (buzz.User.ProfilePicURL != null)
            {
                ProfilePic.Source = new BitmapImage(new Uri(buzz.User.ProfilePicURL, UriKind.RelativeOrAbsolute));
            }

            if (buzz.User.UserID != Core.User.User.UserID)
            {

                if (buzz.IsFollowing)
                {
                    ShowUnfollow.Begin();
                }
                else
                {
                    ShowFollow.Begin();
                }
            }
            UpdateFollowUI();

            if (buzz.HangoutDateTime != null)
            {
                TimeLBL.Text = buzz.HangoutDateTime.Value.ToShortDateString() + " at " + buzz.HangoutDateTime.Value.ToShortTimeString();
            }
            else
            {
                TimeSP.Visibility = System.Windows.Visibility.Collapsed;
            }

            if (buzz.Venue != null)
            {
                VenueNameLBL.Text = buzz.Venue.Name;
            }
            else
            {
                VenueSP.Visibility = System.Windows.Visibility.Collapsed;
            }

        }

        private void AddBuzzText(string text,string place)
        {
            BuzzLBL.Text = "";
            String[] t = text.Split(' ');
            String tmp = "";
            foreach (String s in t)
            {
                if(s.StartsWith("\""))
                {
                    BuzzLBL.Inlines.Add(new Run() { Text = "\"" });
                    s.TrimStart('\"');
                }

                if (s.StartsWith("#"))
                {
                    //print tmp
                    BuzzLBL.Inlines.Add(new Run() { Text = tmp});
                    //print s
                    BuzzLBL.Inlines.Add(new Run() { Text = s+" ",Foreground=new SolidColorBrush(Color.FromArgb(255,255,175,4))});
                    //flush tmp
                    tmp="";
                }
                else
                {
                    tmp += s + " ";
                }
            }

            //print tmp.
            BuzzLBL.Inlines.Add(new Run() { Text = tmp + " " });
            BuzzLBL.Inlines.Add(new Run() { Text =  "- @"+place, Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 175, 4)) });
           //flish tmp
            tmp = "";
            
        }

        private void AddBuzzText(string text)
        {
            BuzzLBL.Text = "";
            String[] t = text.Split(' ');
            String tmp = "";
            foreach (String s in t)
            {
                if (s.StartsWith("#"))
                {
                    //print tmp
                    BuzzLBL.Inlines.Add(new Run() { Text = tmp });
                    //print s
                    BuzzLBL.Inlines.Add(new Run() { Text = s + " ", Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 175, 4)) });
                    //flush tmp
                    tmp = "";
                }
                else
                {
                    tmp += s + " ";
                }

            }

            //print tmp.
            BuzzLBL.Inlines.Add(new Run() { Text = tmp + " " });
            //flish tmp
            tmp = "";

        }

        void BuzzServiceClient_FollowBuzzCompleted(object sender, BuzzService.FollowBuzzCompletedEventArgs e)
        {
            Services.BuzzServiceClient.FollowBuzzCompleted -= BuzzServiceClient_FollowBuzzCompleted;
            if (e.Error == null)
            {
                Buzz.IsFollowing = true;
                Buzz.NoOfFollowers++;
                UpdateFollowUI();
            }
            else
            {
                ShowFollow.Begin();
            }
        }

        private void UpdateFollowUI()
        {
            if (Buzz.IsFollowing)
            {
                if (Buzz.NoOfFollowers > 1)
                {
                    PeopleFollowingLBL.Text = Buzz.NoOfFollowers + " people around you follow this.";
                }
                else
                {
                    if (Core.User.User.UserID != Buzz.User.UserID)
                    {
                        PeopleFollowingLBL.Text = "You are following this.";
                    }
                    else
                    {
                        PeopleFollowingLBL.Visibility = System.Windows.Visibility.Collapsed;
                    }
                }
            }
            else
            {
                if (Buzz.NoOfFollowers > 1)
                {
                    PeopleFollowingLBL.Text = (Buzz.NoOfFollowers-1) + " people around you follow this.";
                }
                else
                {
                    PeopleFollowingLBL.Visibility = System.Windows.Visibility.Collapsed;
                }
            }

            PeopleFollowingLBL.Text = PeopleFollowingLBL.Text.ToUpper();
        }

        

        void BuzzServiceClient_UnfollowBuzzCompleted(object sender, BuzzService.UnfollowBuzzCompletedEventArgs e)
        {
            Services.BuzzServiceClient.UnfollowBuzzCompleted -= BuzzServiceClient_UnfollowBuzzCompleted;
            if (e.Error == null)
            {
                Buzz.IsFollowing = false;
                Buzz.NoOfFollowers--;
                UpdateFollowUI();
            }
            else
            {
                ShowUnfollow.Begin();
            }
        }


        

       

        private void FollowBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ShowUnfollow.Begin();
            Services.BuzzServiceClient.FollowBuzzCompleted += BuzzServiceClient_FollowBuzzCompleted;
            Services.BuzzServiceClient.FollowBuzzAsync(Core.User.User.UserID, Buzz.BuzzID, Core.User.User.ZAT);
        }

        private void UnfollowBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ShowFollow.Begin();
            Services.BuzzServiceClient.UnfollowBuzzCompleted += BuzzServiceClient_UnfollowBuzzCompleted;
            Services.BuzzServiceClient.UnfollowBuzzAsync(Core.User.User.UserID, Buzz.BuzzID, Core.User.User.ZAT);
        }

        private void LayoutRoot_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (BuzzSelected != null)
            {
                BuzzSelected(Buzz.BuzzID);
            }
        }

        private void ProfileSP_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //user selected.
            if (UserSelected != null)
            {
                if (Buzz.User != null)
                {
                    UserSelected(Buzz.User.UserID);

                }
            }
        }

    }
}
