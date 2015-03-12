using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Threading;
using System.Windows.Media.Imaging;

namespace Hangout.Client
{
    public partial class You : UserControl
    {




        public event EventHandler AccountClicked;
        public event EventHandler ProfileClicked;
        public event EventHandler InterestClicked;
        public event EventHandler NotificationClicked;
        public event EventHandler UserClicked;
        public event EventHandler HelpClicked;
        public event EventHandler AboutUsClicked;
        public event EventHandler LoadingStarted;
        public event EventHandler LoadCompleted;
        public event EventHandler Error;
        public event EventHandler MessageClicked;

        public You()
        {
            InitializeComponent();
            TiltEffect.TiltableItems.Add(typeof(Grid));
        }

       

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            //Get Reps. 
            if (Core.User.User.UserID != -1)
            {
                if (LoadingStarted != null)
                {
                    LoadingStarted(null, new EventArgs());
                }
                Settings.Settings.NotificationCountChanged += Settings_NotificationCountChanged;
                Settings.Settings.MessageCountChanged += Settings_MessageCountChanged;
                ProfilePic.Source = new BitmapImage(new Uri(Settings.Settings.UserData.ProfilePicURL, UriKind.RelativeOrAbsolute));
                NameLBL.Text = Settings.Settings.UserData.Name;
                Services.UserServiceClient.GetUserRepCompleted += UserServiceClient_GetUserRepCompleted;
                Services.UserServiceClient.GetUserRepAsync(Core.User.User.UserID, Core.User.User.ZAT);
                NotificationcountLBL.Text = Settings.Settings.UnreadNotifications.ToString();
                MessageNoLBL.Text = Settings.Settings.UnreadMesssages.ToString();
            }

        }

        void Settings_MessageCountChanged(object sender, EventArgs e)
        {
            MessageNoLBL.Text = Settings.Settings.UnreadMesssages.ToString();
        }

        void Settings_NotificationCountChanged(object sender, EventArgs e)
        {
            NotificationcountLBL.Text = Settings.Settings.UnreadNotifications.ToString();
        }

        void UserServiceClient_GetUserRepCompleted(object sender, UserService.GetUserRepCompletedEventArgs e)
        {
            Services.UserServiceClient.GetUserRepCompleted -= UserServiceClient_GetUserRepCompleted;
            if (e.Result != null)
            {
                if (e.Result.Rank == 0)
                {
                    RANKLBL.Text = "XX";
                }
                else
                {
                    RANKLBL.Text = e.Result.Rank.ToString();
                }
                REPSLBL.Text = e.Result.Reps.ToString();

                StartStoryboard();
                if (LoadCompleted != null)
                {
                    LoadCompleted(null, new EventArgs());
                }
            }
            else
            {
                if (Error != null)
                {
                    Error(null, new EventArgs());
                }
            }
        }

        private int storyBoardCount = 0;
        DispatcherTimer showStoryboardsTimer = new DispatcherTimer();
        private void StartStoryboard()
        {

            showStoryboardsTimer.Interval = new TimeSpan(0, 0, 0); //tick immediately during the first thing
            showStoryboardsTimer.Tick += showStoryboardsTimer_Tick;
            showStoryboardsTimer.Start();

        }

        void showStoryboardsTimer_Tick(object sender, EventArgs e)
        {
            showStoryboardsTimer.Interval += new TimeSpan(0, 0, new Random().Next(4, 6));
            storyBoardCount++;

            if (storyBoardCount == 3)
            {
                storyBoardCount = 1;
            }


            if (storyBoardCount == 1)
            {
                ShowRank.Begin();
            }
            if (storyBoardCount == 2)
            {
                ShowReps.Begin();
            }

        }

      

        private void ProfileGrid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (ProfileClicked != null)
            {
                ProfileClicked(null, new EventArgs());
            }
        }

        private void InterestGrid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (InterestClicked != null)
            {
                InterestClicked(null, new EventArgs());
            }
        }

        private void NotificationsGrid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (NotificationClicked != null)
            {
                NotificationClicked(null, new EventArgs());
            }
        }

        private void AccountsGrid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (AccountClicked != null)
            {
                AccountClicked(null, new EventArgs());
            }
        }

        private void AboutUsGrid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (AboutUsClicked != null)
            {
                AboutUsClicked(null, new EventArgs());
            }
        }

        private void HelpGrid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (MessageClicked != null)
            {
                MessageClicked(null, new EventArgs());
            }
        }

        private void UserGrid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (UserClicked != null)
            {
                UserClicked(null, new EventArgs());
            }
        }

       
    }
}
