using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.Globalization;
using System.Windows.Threading;

namespace Hangout.Client.Controls.User
{
    public partial class UserDetailsPanel : UserControl
    {

        public delegate void UserHelper(UserService.CompleteUserProfile User);

        public event UserHelper MessageBtnClicked;

        public event UserHelper TagCountClicked;

        public event UserHelper BuzzCountClicked;

        public event UserHelper FollowingCountClicked;

        public event UserHelper FollowersCountClicked;

        public event UserHelper CategorieCountClicked;

        public event UserHelper VenuesCountClicked;

        public event UserHelper TrophyCountClicked;

        public event UserHelper BadgeCountClicked;


        public UserDetailsPanel()
        {
            InitializeComponent();
        }

        private UserService.CompleteUserProfile profile;

        public UserService.CompleteUserProfile Profile
        {
            get
            {
                return profile;

            }
            set
            {
                profile = value;
                if (Profile != null)
                {
                    LoadData();
                }
            }
        }

        public static double round(double value, int places)
        {
            if (places < 0) throw new Exception();

            long factor = (long)Math.Pow(10, places);
            value = value * factor;
            long tmp = (long)Math.Round(value);
            return (double)tmp / factor;
        }

        private void LoadData()
        {
            try
            {

                if (Core.User.User.UserID == Profile.UserID)
                {
                    TextMe.Visibility = System.Windows.Visibility.Collapsed;
                }

                if (Profile.Age != null)
                {
                    LayoutRoot.Visibility = System.Windows.Visibility.Visible;
                    if (Profile.Age != null)
                    {
                        AgeLBL.Text = profile.Age.ToString();
                    }
                    else
                    {
                        AgeSP.Visibility = System.Windows.Visibility.Collapsed;
                    }

                    if (profile.Status != null)
                    {
                        AboutMeLBL.Text = profile.AboutUs;
                    }
                    else
                    {
                        AboutMeSP.Visibility = System.Windows.Visibility.Collapsed;
                    }

                    if (Core.User.User.UserID == profile.UserID)
                    {
                        CommonSP.Opacity = 0;
                    }

                    if (profile.Gender != null)
                    {
                        if ((bool)profile.Gender)
                        {
                            GenderText.Text = "Male";
                            GenderImage.Source = new BitmapImage(new Uri("Male Icon.png", UriKind.Relative));
                        }
                        else
                        {
                            GenderText.Text = "Female";
                            GenderImage.Source = new BitmapImage(new Uri("Female Icon.png", UriKind.Relative));
                        }

                    }
                    else
                    {
                        SexSP.Visibility = System.Windows.Visibility.Collapsed;
                    }

                    if (profile.RelationshipStatus != null)
                    {
                        if (profile.RelationshipStatus.ToLower() == "single" && (bool)profile.Gender)
                        {
                            RelationShipText.Text = "Single";
                            RelationShipImage.Source = new BitmapImage(new Uri("SingleMale.PNG", UriKind.Relative));
                        }

                        if (profile.RelationshipStatus.ToLower() == "single" && !(bool)profile.Gender)
                        {
                            RelationShipText.Text = "Single";
                            RelationShipImage.Source = new BitmapImage(new Uri("SingleFemale.PNG", UriKind.Relative));
                        }

                        if (profile.RelationshipStatus.ToLower() == "engaged")
                        {
                            RelationShipText.Text = "Engaged";
                            RelationShipImage.Source = new BitmapImage(new Uri("Engaged.PNG", UriKind.Relative));
                        }

                        if (profile.RelationshipStatus.ToLower() == "married")
                        {
                            RelationShipText.Text = "Married";
                            RelationShipImage.Source = new BitmapImage(new Uri("Married.PNG", UriKind.Relative));
                        }

                        if (profile.RelationshipStatus.ToLower() == "divorced")
                        {
                            RelationShipText.Text = "Divorced";
                            RelationShipImage.Source = new BitmapImage(new Uri("Divorced.PNG", UriKind.Relative));
                        }

                        if (profile.RelationshipStatus.ToLower() == "in a relationship")
                        {
                            RelationShipText.Text = "Relationship";
                            RelationShipImage.Source = new BitmapImage(new Uri("Relationship.PNG", UriKind.Relative));
                        }


                    }
                    else
                    {
                        RelationshipStatusSP.Visibility = System.Windows.Visibility.Collapsed;
                    }


                    FollowingCount.Text = profile.FollowingCount.ToString();
                    FollowersCount.Text = profile.FollowersCount.ToString();
                    InterestCount.Text = profile.InterestCount.ToString();
                    VenueCount.Text = profile.VenueCount.ToString();
                    CategoryCount.Text = profile.CategoryCount.ToString();
                    BuzzCount.Text = profile.BuzzCount.ToString();
                    TrophyCount.Text = profile.TrophyCount.ToString();
                    BadgeCount.Text = profile.BadgeCount.ToString();

                    if (!String.IsNullOrWhiteSpace(profile.Name))
                    {
                        NameLBL.Text = profile.Name;
                    }
                   
                   
                    AboutMeLBL.Text = profile.AboutUs;
                       
                  

                    if (profile.ProfilePicURL != null)
                    {
                        ProfilePic.Source = new BitmapImage(new Uri(profile.ProfilePicURL, UriKind.RelativeOrAbsolute));
                    }

                    if (Core.User.User.UserID != profile.UserID)
                    {

                        if (profile.IsFollowing)
                        {
                            if (profile.Gender)
                            {
                                ShowMaleFollow.Begin();
                            }
                            else
                            {
                                ShowFemaleFollow.Begin();
                            }
                        }
                        else
                        {
                            if (profile.Gender)
                            {
                                ShowMaleUnfollow.Begin();
                            }
                            else
                            {
                                ShowFemaleUnfollow.Begin();
                            }
                        }
                    }
                    if (profile.Rank > 0)
                    {
                        RankLBL.Text = profile.Rank.ToString();
                    }
                    else
                    {
                        RankLBL.Text = "XX";
                    }

                    ThingsInCommonLBL.Text = profile.CommonItems.ToString();
                    RepsLBL.Text = profile.Reps.ToString();

                    if (Core.User.User.UserID == Profile.UserID)
                    {
                        DisSP.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    else
                    {
                        if (Profile.Distance != null)
                        {
                            DisSP.Visibility = System.Windows.Visibility.Visible;
                            AwayLBL.Text = "AWAY";
                            PostPic.Visibility = System.Windows.Visibility.Collapsed;
                            StepsPic.Visibility = System.Windows.Visibility.Collapsed;
                            double dis = round((double)Profile.Distance, 1);

                            if (dis > 0.3)
                            {
                                if (Settings.Settings.UserData.DefaultLengthUnits == "Km")
                                {

                                    string s = dis.ToString("0.0", CultureInfo.InvariantCulture);
                                    string[] parts = s.Split('.');
                                    int digit = parts[0].Count();

                                    disUnitsLBL.Text = "KM";

                                    if (digit > 1)
                                    {
                                        DistanceLBL.Text = parts[0];
                                    }
                                    else
                                    {
                                        DistanceLBL.Text = s;
                                    }

                                }
                                else
                                {
                                    disUnitsLBL.Text = "MI";
                                    dis = Core.Location.Distance.ConvertToMiles((double)Profile.Distance);
                                    string s = dis.ToString("0.0", CultureInfo.InvariantCulture);
                                    string[] parts = s.Split('.');
                                    int digit = parts[0].Count();

                                    disUnitsLBL.Text = "MI";

                                    if (digit > 1)
                                    {
                                        DistanceLBL.Text = parts[0];
                                    }
                                    else
                                    {
                                        DistanceLBL.Text = s;
                                    }
                                }
                            }
                            else
                            {
                                DisSP.Visibility = System.Windows.Visibility.Collapsed;
                                AwayLBL.Text = "NEAR YOU";
                                PostPic.Visibility = System.Windows.Visibility.Collapsed;
                                StepsPic.Visibility = System.Windows.Visibility.Visible;
                                StepsPic.Opacity = 1;
                            }
                        }
                        else
                        {
                            DisSP.Visibility = System.Windows.Visibility.Collapsed;
                            AwayLBL.Text = "FAR AWAY";
                            PostPic.Visibility = System.Windows.Visibility.Visible;
                            StepsPic.Visibility = System.Windows.Visibility.Collapsed;

                        }
                    }


                    StartStoryboard();
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
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
                ShowPanel1.Begin();
            }
            if (storyBoardCount == 2)
            {
                ShowPanel2.Begin();
            }

        }


        private void MaleUnfollowUserCanvas_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Follow();
            ShowMaleFollow.Begin();
        }

        private void Follow()
        {
            Services.UserServiceClient.FollowUserAsync(Core.User.User.UserID, profile.UserID, Core.User.User.ZAT);
        }

        private void MaleFollowUSerCanvas_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            UnFollow();
            ShowMaleUnfollow.Begin();
        }

        private void FemaleUnfollowUserCanvas_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Follow();
            ShowFemaleFollow.Begin();
        }

        private void FemaleFollowUserCanvas_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            UnFollow();
            ShowFemaleUnfollow.Begin();
        }

        private void UnFollow()
        {
            Services.UserServiceClient.UnfollowUserAsync(Core.User.User.UserID, profile.UserID, Core.User.User.ZAT);
        }

      
        private void MeetMeBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (MessageBtnClicked != null)
            {
                MessageBtnClicked(profile);
            }
        }

        private void Interest_SP_Tapped(object sender, GestureEventArgs e)
        {
            if (TagCountClicked != null)
            {
                TagCountClicked(profile);
            }
        }

        private void Buzz_SP_Tapped(object sender, GestureEventArgs e)
        {
            if (BuzzCountClicked != null)
            {
                BuzzCountClicked(profile);
            }
        }

        private void Venue_SP_Tapped(object sender, GestureEventArgs e)
        {
            if (VenuesCountClicked != null)
            {
                VenuesCountClicked(profile);
            }
        }

        private void Category_SP_Tapped(object sender, GestureEventArgs e)
        {
            if (CategorieCountClicked != null)
            {
                CategorieCountClicked(profile);
            }
        }

        private void Trophy_SP_Tapped(object sender, GestureEventArgs e)
        {
            if (TrophyCountClicked != null)
            {
                TrophyCountClicked(profile);
            }
        }

        private void Badge_SP_Tapped(object sender, GestureEventArgs e)
        {
            if (BadgeCountClicked != null)
            {
                BadgeCountClicked(profile);
            }
        }

        private void FollowingTapped(object sender, GestureEventArgs e)
        {
            if (FollowingCountClicked != null)
            {
                FollowingCountClicked(profile);
            }

        }

        private void Followers_SP_Tapped(object sender, GestureEventArgs e)
        {
            if (FollowersCountClicked != null)
            {
                FollowersCountClicked(profile);
            }
        }
    }
}
