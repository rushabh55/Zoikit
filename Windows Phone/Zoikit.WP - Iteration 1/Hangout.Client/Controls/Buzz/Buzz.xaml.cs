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
using System.Windows.Documents;
using System.Windows.Media;
using Microsoft.Phone.Tasks;
using Hangout.Client.Core.Social;

namespace Hangout.Client.Controls.Buzz
{
    public partial class Buzz : UserControl
    {
        public Buzz()
        {
            InitializeComponent();
        }

        public BuzzService.Buzz BuzzData
        {
            get { return (BuzzService.Buzz)GetValue(BuzzProperty); }

            set
            {
                SetValue(BuzzProperty, value);
            }
        }

        public delegate void IdHelper(Guid id);

        public delegate void TagHelper(string name);

        public delegate void PeopleHelper(string name,Guid id);

        public delegate void EventHelper();

        public event EventHelper NavigateToFacebook;

        public event EventHelper NavigateToTwitter;

        public event EventHelper ProgressBarShow;

        public event EventHelper ProgressBarDisappear;

        

        public event TagHelper TagSelected;

        public event PeopleHelper PeopleSelected;

        public event IdHelper UserSelected;

        public event IdHelper CommentOnBuzzSelected;

        public static readonly DependencyProperty BuzzProperty =
        DependencyProperty.Register(
            "BuzzData",
            typeof(BuzzService.Buzz),
            typeof(Buzz),
            new PropertyMetadata(null, BuzzValueChanged));


        private static void BuzzValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Buzz obj = (Buzz)d;
            obj.DisplayData((BuzzService.Buzz)e.NewValue);
        }

        public void DisplayData(BuzzService.Buzz buzz)
        {
            Core.Converters.PostedAdder obj = new Core.Converters.PostedAdder();

            AddBuzzText(buzz.Text);
            PostedLBL.Text = obj.Convert(buzz.Posted, null, null, System.Globalization.CultureInfo.CurrentCulture).ToString();
            NameLBL.Text = buzz.User.Name;

            if (buzz.User.ProfilePicURL != null)
            {
                ProfileImage.Source = new BitmapImage(new Uri(buzz.User.ProfilePicURL, UriKind.RelativeOrAbsolute));
            }

            if(buzz.ImageURL==""||buzz.ImageURL==null)
            {
                BuzzImage.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                BuzzImage.Visibility = System.Windows.Visibility.Visible;
                BuzzImage.Source =  new BitmapImage(new Uri(buzz.ImageURL, UriKind.RelativeOrAbsolute));
            }

            string locationString = "";

            if(buzz.Location!=null)
            {
                if(buzz.Location.Address==""||buzz.Location.Address==null)
                {
                    //enter city
                    locationString = "in " + buzz.City.Name;
                }
                else
                {

                    if (buzz.Location.Address.Count() < 10)
                    {
                        locationString = "near " + buzz.Location.Address;
                    }
                    else
                    {
                        locationString = "in " + buzz.City.Name;
                    }

                    
                }
            }
            else
            {
                locationString = "in " + buzz.City.Name;
            }

           
            
            
            if(buzz.Distance!=-1)
            {
                if(locationString!="")
                {
                    locationString += " | ";
                }

                //miles OR KM
                if (Convert.ToInt32(buzz.Distance) > 0)
                {

                    if (Settings.Settings.UserData.DefaultLengthUnits == "Miles")

                    {
                        locationString += Convert.ToInt32(Core.Location.Distance.ConvertToMiles(buzz.Distance)) + " Mi away";
                    }
                    else
                    {
                        locationString += Convert.ToInt32(buzz.Distance) + " KM away";
                    }
                }
                else
                {
                    locationString += "near you";
                }
               
                

            }
            
            if(locationString=="")
            {
                LocationSP.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                LocationSP.Visibility = System.Windows.Visibility.Visible;
                LocationLBL.Text = locationString;
            }



            AmplifyLBLUpdate(buzz.AmplifyCount);


            LikeLBLUpdate(buzz.LikeCount);


            CommentLBLUpdate(buzz.CommentCount);
                

            if(buzz.Liked)
            {
                LikeLikedSB.Begin();
            }
            else
            {
                LikeNormalSB.Begin();
            }

            if(buzz.Amplified)
            {
                AmplifyAmplifiedSB.Begin();
            }
            else
            {
                AmplifyNormalSB.Begin();
            }

            if(buzz.Deamplified)
            {
                DeAmplifyDeAmplifiedSB.Begin();
            }
            else
            {
                DeAmplifyNormalSB.Begin();
            }
            

            if(buzz.BuzzComments.Count>0)
            {
                BuzzCommentLB.BuzzComments = buzz.BuzzComments;
            }
            else
            {
                BuzzCommentLB.Visibility = System.Windows.Visibility.Collapsed;
                
                if(buzz.BuzzComments.Count<3)
                {
                    ShowMoreCommentsLBL.Visibility = System.Windows.Visibility.Collapsed;
                }
            }

            if(BuzzData.User.UserID==Core.User.User.UserID)
            {
                //if User shouted this buzz

                DeAmplifyDisabledSB.Begin();
                LikeDiabledSB.Begin();
                AmplifyDisabledSB.Begin();
            }


        }

        private void LikeLBLUpdate(int count)
        {
            if (count == 0)
            {
                LikesLBL.Text = count + " Likes";
                LikesLBL.Foreground = new SolidColorBrush(Color.FromArgb(255, 113, 113, 113));
            }
            else if (count > 1)
            {
                LikesLBL.Text = count + " Likes";
                LikesLBL.Foreground = new SolidColorBrush(Color.FromArgb(255, 238, 123, 68));
            }
            else
            {
                LikesLBL.Text = count + " Like";
                LikesLBL.Foreground = new SolidColorBrush(Color.FromArgb(255, 238, 123, 68));
            }
        }

        private void AmplifyLBLUpdate(int count)
        {
            if (count == 0)
            {
                AmplifiedLBL.Text = count + " Amplified";
                AmplifiedLBL.Foreground = new SolidColorBrush(Color.FromArgb(255, 113, 113, 113));
            }
            else if (count > 1)
            {
                AmplifiedLBL.Text = count + " Amplified";
                AmplifiedLBL.Foreground = new SolidColorBrush(Color.FromArgb(255, 238, 123, 68));
            }
            else
            {
                AmplifiedLBL.Text = count + " Amplified";
                AmplifiedLBL.Foreground = new SolidColorBrush(Color.FromArgb(255, 238, 123, 68));
            }
        }

        private void CommentLBLUpdate(int count)
        {
            if (count == 0)
            {
                CommentsLBL.Text = count + " Comments";
                CommentsLBL.Foreground = new SolidColorBrush(Color.FromArgb(255, 113, 113, 113));
            }
            else if (count > 1)
            {
                CommentsLBL.Text = count + " Comments";
                CommentsLBL.Foreground = new SolidColorBrush(Color.FromArgb(255, 238, 123, 68));
            }
            else
            {
                CommentsLBL.Text = count + " Comment";
                CommentsLBL.Foreground = new SolidColorBrush(Color.FromArgb(255, 238, 123, 68));
            }
        }



        private void AddBuzzText(string text)
        {

            BuzzTextLBL.Blocks.Clear();
            
            String[] t = text.Split(' ');
            String tmp = "";

            Paragraph p = new Paragraph();

            foreach (String s in t)
            {
                if (s.StartsWith("#"))
                {
                    //print tmp
                    p.Inlines.Add(new Run() { Text = tmp });
                    //print s

                    Hyperlink link = new Hyperlink();
                    link.Inlines.Add(new Run() { Text = s});
                    link.Foreground = new SolidColorBrush(Color.FromArgb(255, 238, 123, 68));
                    link.TextDecorations = TextDecorations.Underline;
                    link.Click += (sender, e) =>
                    {
                        if (TagSelected != null)
                        {
                            //get tag id.
                            TagSelected(s.TrimStart('#'));
                        }
                    };

                    p.Inlines.Add(link);

                    Hyperlink link2 = new Hyperlink();
                    link2.Inlines.Add(new Run() { Text = " " });

                    p.Inlines.Add(link2);
                    //flush tmp
                    tmp = "";
                }
                else
                {
                    tmp += s + " ";
                }

            }

            //print tmp.
            p.Inlines.Add(new Run() { Text = tmp + " " });
            //flish tmp
            tmp = "";

            BuzzTextLBL.Blocks.Add(p);

        }



        private void LayoutRoot_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            
        }

        private void MoreBtn_Click(object sender, RoutedEventArgs e)
        {
            Grid button = sender as Grid;
            ContextMenu contextMenu = ContextMenuService.GetContextMenu(button);

            if (contextMenu.Parent == null)
            {
                contextMenu.IsOpen = true;
            } 
        }

        private void CopyBuzzBtn_Tap(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(BuzzData.User.Name + " : \n " + BuzzData.Text+ " \n " +BuzzData.Posted.ToShortDateString()+", "+BuzzData.Posted.ToShortTimeString());
        }

        private void ShareOnFacebookBtn_Tap(object sender, RoutedEventArgs e)
        {
            if(Core.Authentication.Accounts.LoggedInToFacebook())
            {
                Services.FacebookServiceClient.PostFacebookStatusCompleted += FacebookServiceClient_PostFacebookStatusCompleted;
                Services.FacebookServiceClient.PostFacebookStatusAsync(Settings.Settings.FacebookData.AccessToken, BuzzData.Text, AppID.ZoikitAppID, AppID.ZoikitAppToken);

                if(ProgressBarShow!=null)
                {
                    ProgressBarShow();
                }

                
            }
            else
            {
                //save on Social Post

                SocialPost.FacebookPost = true;
                SocialPost.Text = BuzzData.Text;

                if(NavigateToFacebook!=null)
                {
                    NavigateToFacebook();
                }
            }
        }

        void FacebookServiceClient_PostFacebookStatusCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Services.FacebookServiceClient.PostFacebookStatusCompleted -= FacebookServiceClient_PostFacebookStatusCompleted;

            if(e.Error!=null)
            {
                MessageBox.Show(Messages.FacebookStatusError, Messages.Sorry, MessageBoxButton.OK);
            }
            if(ProgressBarDisappear!=null)
            {
                ProgressBarDisappear();
            }
            
        }

        private void ShareOnTwitterBtn_Tap(object sender, RoutedEventArgs e)
        {
            if (Core.Authentication.Accounts.LoggedInToTwitter())
            {
                Services.TwitterServiceClient.PostTweetCompleted += TwitterServiceClient_PostTweetCompleted;
                Services.TwitterServiceClient.PostTweetAsync(Settings.Settings.TwitterData.AccessToken,Settings.Settings.TwitterData.AccessTokenSecret, BuzzData.Text, AppID.ZoikitAppID, AppID.ZoikitAppToken);

                if (ProgressBarShow != null)
                {
                    ProgressBarShow();
                }


            }
            else
            {
                //save on Social Post

                SocialPost.TwitterPost = true;
                SocialPost.Text = BuzzData.Text;

                if (NavigateToTwitter != null)
                {
                    NavigateToTwitter();
                }
            }
        }



        void TwitterServiceClient_PostTweetCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Services.TwitterServiceClient.PostTweetCompleted -= TwitterServiceClient_PostTweetCompleted;

            if (e.Error != null)
            {
                MessageBox.Show(Messages.TwitterStatusError, Messages.Sorry, MessageBoxButton.OK);
            }
            if (ProgressBarDisappear != null)
            {
                ProgressBarDisappear();
            }
        }

        private void TextBuzzBtn_Tap(object sender, RoutedEventArgs e)
        {
            SmsComposeTask smsComposeTask = new SmsComposeTask();
            smsComposeTask.Body = BuzzData.User.Name + " : \n " + BuzzData.Text + " \n " + BuzzData.Posted.ToShortDateString() + ", " + BuzzData.Posted.ToShortTimeString();
            smsComposeTask.Show();
        }

        private void EmailBuzzBtn_Tap(object sender, RoutedEventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();
            emailComposeTask.Body = BuzzData.User.Name + " : \n " + BuzzData.Text + " \n " + BuzzData.Posted.ToShortDateString() + ", " + BuzzData.Posted.ToShortTimeString();
            emailComposeTask.Show();
        }

       

        void BuzzServiceClient_FollowBuzzCompleted(object sender, BuzzService.FollowBuzzCompletedEventArgs e)
        {
            Services.BuzzServiceClient.FollowBuzzCompleted -= BuzzServiceClient_FollowBuzzCompleted;

            if(e.Error!=null)
            {
                LikeNormalSB.Begin();
                MessageBox.Show(Messages.CannotLike, Messages.Sorry, MessageBoxButton.OK);
            }
        }

        void BuzzServiceClient_UnfollowBuzzCompleted(object sender, BuzzService.UnfollowBuzzCompletedEventArgs e)
        {

            Services.BuzzServiceClient.UnfollowBuzzCompleted -= BuzzServiceClient_UnfollowBuzzCompleted;
            if (e.Error != null)
            {
                LikeLikedSB.Begin();
                MessageBox.Show(Messages.CannotUnLike, Messages.Sorry, MessageBoxButton.OK);
            }
        }

        private void CommentBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if(CommentOnBuzzSelected!=null)
            {
                CommentOnBuzzSelected(BuzzData.BuzzID);
            }
        }


        private void LikeBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (this.BuzzData.User.UserID != Core.User.User.UserID)
            {
                if (BuzzData.Liked)
                {
                    //unlike
                    LikeNormalSB.Begin();
                    Services.BuzzServiceClient.UnfollowBuzzCompleted += BuzzServiceClient_UnfollowBuzzCompleted;
                    Services.BuzzServiceClient.UnfollowBuzzAsync(Core.User.User.UserID, BuzzData.BuzzID, Core.User.User.ZAT);
                   
                    BuzzData.Liked = false;
                    this.BuzzData.LikeCount--;
                    LikeLBLUpdate(this.BuzzData.LikeCount);
                }
                else
                {
                    LikeLikedSB.Begin();
                    Services.BuzzServiceClient.FollowBuzzCompleted += BuzzServiceClient_FollowBuzzCompleted;
                    Services.BuzzServiceClient.FollowBuzzAsync(Core.User.User.UserID, BuzzData.BuzzID, Core.User.User.ZAT);

                    BuzzData.Liked = true;
                    this.BuzzData.LikeCount++;
                    LikeLBLUpdate(this.BuzzData.LikeCount);
                }

            }
        }

        private void AmplifyBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (this.BuzzData.User.UserID != Core.User.User.UserID)
            {

                if (!this.BuzzData.Amplified)
                {

                    AmplifyAmplifiedSB.Begin();
                    DeAmplifyNormalSB.Begin();
                    Services.BuzzServiceClient.AmplifyCompleted += BuzzServiceClient_AmplifyCompleted;
                    Services.BuzzServiceClient.AmplifyAsync(this.BuzzData.BuzzID, Core.User.User.UserID, true, Core.User.User.ZAT);
                    this.BuzzData.Amplified = true;
                    this.BuzzData.Deamplified = false;
                    this.BuzzData.AmplifyCount++;
                    AmplifyLBLUpdate(this.BuzzData.AmplifyCount);

                }
                else
                {
                    AmplifyNormalSB.Begin();
                    Services.BuzzServiceClient.UndoAmplificationCompleted += BuzzServiceClient_UndoAmplificationCompleted;
                    Services.BuzzServiceClient.UndoAmplificationAsync(this.BuzzData.BuzzID, Core.User.User.UserID, true, Core.User.User.ZAT);
                    this.BuzzData.Amplified = false;
                    this.BuzzData.AmplifyCount--;
                    AmplifyLBLUpdate(this.BuzzData.AmplifyCount);

                }
            }
        }

        void BuzzServiceClient_UndoAmplificationCompleted(object sender, BuzzService.UndoAmplificationCompletedEventArgs e)
        {
            Services.BuzzServiceClient.UndoAmplificationCompleted -= BuzzServiceClient_UndoAmplificationCompleted;

            if(e.Error==null)
            {
                if(e.Result==BuzzService.AmplifyStatus.DeamplificationUndoError)
                {
                    DeAmplifyDeAmplifiedSB.Begin();
                    MessageBox.Show(Messages.CannotUnDeAmplify, Messages.Sorry, MessageBoxButton.OK);
                }
                if (e.Result == BuzzService.AmplifyStatus.AmplificationUndoError)
                {
                    AmplifyAmplifiedSB.Begin();
                    MessageBox.Show(Messages.CannotUnAmplify, Messages.Sorry, MessageBoxButton.OK);
                    if (this.BuzzData.Amplified)
                    {
                        this.BuzzData.AmplifyCount++;
                        AmplifyLBLUpdate(this.BuzzData.AmplifyCount);
                    }
                }
            }
            else
            {
                MessageBox.Show(Messages.Windy, Messages.Sorry, MessageBoxButton.OK);
            }
        }

        void BuzzServiceClient_AmplifyCompleted(object sender, BuzzService.AmplifyCompletedEventArgs e)
        {

            Services.BuzzServiceClient.AmplifyCompleted -= BuzzServiceClient_AmplifyCompleted;

            if (e.Error == null)
            {
                if (e.Result == BuzzService.AmplifyStatus.DeamplificationError)
                {
                    DeAmplifyNormalSB.Begin();
                    MessageBox.Show(Messages.CannotDeamplify, Messages.Sorry, MessageBoxButton.OK);
                }
                if (e.Result == BuzzService.AmplifyStatus.AmplificationError)
                {
                    AmplifyNormalSB.Begin();
                    MessageBox.Show(Messages.CannotAmplify, Messages.Sorry, MessageBoxButton.OK);

                    if (this.BuzzData.Amplified)
                    {
                        this.BuzzData.AmplifyCount--;
                        AmplifyLBLUpdate(this.BuzzData.AmplifyCount);
                    }
                }
            }
            else
            {
                MessageBox.Show(Messages.Windy, Messages.Sorry, MessageBoxButton.OK);
            }
        }

        private void DeAmplify_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (this.BuzzData.User.UserID != Core.User.User.UserID)
            {
                if (!this.BuzzData.Deamplified)
                {
                    DeAmplifyDeAmplifiedSB.Begin();
                    AmplifyNormalSB.Begin();
                    Services.BuzzServiceClient.AmplifyCompleted += BuzzServiceClient_AmplifyCompleted;
                    Services.BuzzServiceClient.AmplifyAsync(this.BuzzData.BuzzID, Core.User.User.UserID, false, Core.User.User.ZAT);

                    if (this.BuzzData.Amplified)
                    {
                        this.BuzzData.AmplifyCount--;
                        AmplifyLBLUpdate(this.BuzzData.AmplifyCount);
                    }

                    this.BuzzData.Deamplified = true;
                    this.BuzzData.Amplified = false;



                }
                else
                {
                    DeAmplifyNormalSB.Begin();
                    Services.BuzzServiceClient.UndoAmplificationCompleted += BuzzServiceClient_UndoAmplificationCompleted;
                    Services.BuzzServiceClient.UndoAmplificationAsync(this.BuzzData.BuzzID, Core.User.User.UserID, false, Core.User.User.ZAT);
                    this.BuzzData.Deamplified = false;
                }
            }
        }

        private void ProfileImage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if(UserSelected!=null)
            {
                UserSelected(BuzzData.User.UserID);
            }
        }

        private void NameLBL_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (UserSelected != null)
            {
                UserSelected(BuzzData.User.UserID);
            }
        }

        private void LikesLBL_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (BuzzData.LikeCount > 0)
            {
                if (PeopleSelected != null)
                {
                    PeopleSelected("BuzzLiked", BuzzData.BuzzID);
                }
            }
        }

        private void AmplifiedLBL_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (BuzzData.AmplifyCount > 0)
            {
                if (PeopleSelected != null)
                {
                    PeopleSelected("BuzzAmplified", BuzzData.BuzzID);
                }
            }
        }

        private void CommentsLBL_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

            if(BuzzData.CommentCount>0)
            {
                //go to comments page. 
                if (CommentOnBuzzSelected != null)
                {
                    CommentOnBuzzSelected(BuzzData.BuzzID);
                }
            }
            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BuzzCommentLB.BuzzCommentsLB.IsPullToRefreshEnabled = false;
            BuzzCommentLB.BuzzCommentsLB.DataVirtualizationMode = Telerik.Windows.Controls.DataVirtualizationMode.None;
            BuzzCommentLB.UserSelected += BuzzCommentLB_UserSelected;

            //disable vertical scrollbar of scrollviewer. 
            if (VisualTreeHelper.GetChildrenCount(BuzzCommentLB.BuzzCommentsLB) > 0)
            {
                DependencyObject obj = VisualTreeHelper.GetChild(BuzzCommentLB.BuzzCommentsLB, 0);
                if (VisualTreeHelper.GetChildrenCount(obj) > 0)
                {
                    DependencyObject grid = VisualTreeHelper.GetChild(obj, 0);
                    if (VisualTreeHelper.GetChildrenCount(grid) > 0)
                    {
                        ScrollViewer sv = VisualTreeHelper.GetChild(grid, 1) as ScrollViewer;
                        sv.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
                    }
                }

                
            }
            
            
           
        }

        void BuzzCommentLB_UserSelected(Guid id)
        {
            if (UserSelected != null)
            {
                UserSelected(id);
            }
        }

        private void ShowMoreCommentsLBL_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if(CommentOnBuzzSelected!=null)
            {
                CommentOnBuzzSelected(BuzzData.BuzzID);
            }
        }


     

    }
}
