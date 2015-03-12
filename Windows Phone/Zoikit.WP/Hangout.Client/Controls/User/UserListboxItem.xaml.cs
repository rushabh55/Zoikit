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
using System.ComponentModel;
using System.Windows.Threading;
using System.Globalization;

namespace Hangout.Client.Controls.User
{
    public partial class UserListboxItem : UserControl
    {

        public delegate void IdHelper(int id);

        public event IdHelper TokenSelected;
        public event IdHelper VenueSelected;
        public event IdHelper BuzzSelected;


        public UserListboxItem()
        {
            InitializeComponent();
            TiltEffect.TiltableItems.Add(typeof(Grid));
            TiltEffect.TiltableItems.Add(typeof(StackPanel));
            Loaded += UserListboxItem_Loaded;
        }

        void UserListboxItem_Loaded(object sender, RoutedEventArgs e)
        {
            LayoutRoot.Tap += LayoutRoot_Tap;
            InterestsList.TokenSelected += InterestsList_TokenSelected;
        }

        void LayoutRoot_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (User != null)
            {
                NavigateToUserPage(User.UserID);
            }
        }

        void InterestsList_TokenSelected(int id)
        {
            if (TokenSelected != null)
            {
                TokenSelected(id);
            }
        }

        public UserService.User User
        {
            get { return (UserService.User)GetValue(UserProperty); }

            set
            {
                SetValue(UserProperty, value);

            }
        }

       
        private void FillData(UserService.User user)
        {

            
            if (user != null)
            {
                if (!String.IsNullOrWhiteSpace(user.Name))
                {
                    NameLBL.Text = user.Name;
                }
                if (user.Status != null)
                {
                    AboutMeTitle.Text = "WHAT I'M DOING NOW";
                    AboutLBL.Text = user.Status.Text;
                    AttachStatusEvents();
                    Core.Converters.DateTimeToStringConverter obj=new Core.Converters.DateTimeToStringConverter();
                    PostedLBL.Text = obj.Convert(user.Status.DateTime, null, null, null).ToString();
                    TiltEffect.SetIsTiltEnabled(Panel1, true);
                }
                else
                {
                    TiltEffect.SetSuppressTilt(Panel1, true);
                    AboutLBL.Text = user.AboutUs;
                    PostedLBL.Visibility = System.Windows.Visibility.Collapsed;
                }

                if (user.ProfilePicURL != null)
                {
                    ProfilePic.Source = new BitmapImage(new Uri(user.ProfilePicURL, UriKind.RelativeOrAbsolute));
                }

                if (user.IsFollowing)
                {
                    if (user.Gender)
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
                    if (user.Gender)
                    {
                        ShowMaleUnfollow.Begin();
                    }
                    else
                    {
                        ShowFemaleUnfollow.Begin();
                    }
                }
                if (user.Rank > 0)
                {
                    RankLBL.Text = user.Rank.ToString();
                }
                else
                {
                    RankLBL.Text = "XX";
                }

                CommonThingsLBL.Text = user.CommonItems.ToString();

                InterestsList.Tokens = Convert(user.TokenFollowing.ToList());
                InterestsList.RefreshList();

                if (Core.User.User.UserID == User.UserID)
                {
                    DisSP.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    if (user.Distance != null)
                    {
                        DisSP.Visibility = System.Windows.Visibility.Visible;
                        AwayLBL.Text = "AWAY";
                        PostPic.Visibility = System.Windows.Visibility.Collapsed;
                        StepsPic.Visibility = System.Windows.Visibility.Collapsed;
                        double dis = round((double)user.Distance, 1);

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
                                dis = Core.Location.Distance.ConvertToMiles((double)user.Distance);
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

                if (user.TokenFollowing.Count > 0)
                {
                    StartStoryboard();
                }
                
            }


        }

        private List<TokenService.UserToken> Convert(List<UserService.UserToken> list)
        {
            List<TokenService.UserToken> tokens = new List<TokenService.UserToken>();


            foreach (UserService.UserToken t in list)
            {
                TokenService.UserToken token = new TokenService.UserToken { Following = t.Following, NoOfPeopleFollowing = t.NoOfPeopleFollowing, Token = new TokenService.Token { Id = t.Token.Id, Name = t.Token.Name } };

                tokens.Add(token);
            }

            return tokens;
        }

        public static double round(double value, int places)
        {
            if (places < 0) throw new Exception();

            long factor = (long)Math.Pow(10, places);
            value = value * factor;
            long tmp = (long)Math.Round(value);
            return (double)tmp / factor;
        }

        

        private void AttachStatusEvents()
        {
            TiltEffect.SetIsTiltEnabled(Panel1,true);
            Panel1.Tap += Panel1_Tap;
        }

        void Panel1_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (User.Status.Type == "Category")
            {
                NavigateToCategoryPage(int.Parse(User.Status.Param));
            }

            if (User.Status.Type == "Venue")
            {
                NavigateToVenuePage(int.Parse(User.Status.Param));
            }


            if (User.Status.Type == "User")
            {
                NavigateToUserPage(int.Parse(User.Status.Param));
            }

            if (User.Status.Type == "Buzz")
            {
                NavigateToBuzzPage(int.Parse(User.Status.Param));
            }

            if (User.Status.Type == "Tag")
            {
                NavigateToTagPage(int.Parse(User.Status.Param));
            }


        }

        private void NavigateToBuzzPage(int p)
        {
            if (BuzzSelected != null)
            {
                BuzzSelected(p);
            }
        }

        private void NavigateToUserPage(int p)
        {
            if (UserSelected != null)
            {
                UserSelected(p);
            }
        }

        private void NavigateToVenuePage(int p)
        {
            if (VenueSelected != null)
            {
                VenueSelected(p);
            }
        }

        private void NavigateToTagPage(int p)
        {
            if (TokenSelected != null)
            {
                TokenSelected(p);
            }
        }

        private void NavigateToCategoryPage(int id)
        {
            if (CategorySelected != null)
            {
                CategorySelected(id);
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



       

       

        

        public static readonly DependencyProperty UserProperty =
        DependencyProperty.Register(
            "User",
            typeof(UserService.User),
            typeof(Controls.User.UserListboxItem),
            new PropertyMetadata(null, UserValueChanged));


        private static void UserValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Controls.User.UserListboxItem item = (Controls.User.UserListboxItem)d;
            item.FillData((UserService.User)e.NewValue);
        }

        public delegate void CategorySelectedHelper(int id);

        public event CategorySelectedHelper CategorySelected;

        public delegate void TagSelectedHelper(int id);

        public event TagSelectedHelper TagSelected;

        public delegate void UserSelectedHelper(int id);

        public event UserSelectedHelper UserSelected;

       

        private void MaleUnfollowUserCanvas_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Follow();
            ShowMaleFollow.Begin();
        }

        private void Follow()
        {
            Services.UserServiceClient.FollowUserAsync(Core.User.User.UserID, User.UserID, Core.User.User.ZAT);
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
            Services.UserServiceClient.UnfollowUserAsync(Core.User.User.UserID, User.UserID, Core.User.User.ZAT);
        }

        




    }
}
