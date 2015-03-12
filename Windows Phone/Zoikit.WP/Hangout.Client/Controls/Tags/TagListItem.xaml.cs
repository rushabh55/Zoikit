using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Hangout.Client
{
  
    	
      public partial class TagListItem : UserControl
      {

          public delegate void TokenHelper(TokenService.UserToken token);

          public event TokenHelper FollowToken;

          public event TokenHelper UnFollowToken;

          public delegate void IdHelper(int id);


        public event IdHelper TokenNameTapped;



        public TagListItem()
        {
            InitializeComponent();
            TiltEffect.TiltableItems.Add(typeof(Canvas));
            TiltEffect.TiltableItems.Add(typeof(Grid));
        }

        

        public TokenService.UserToken Token
        {
            get { return (TokenService.UserToken)GetValue(TokenProperty); }

            set
            {
                SetValue(TokenProperty, value);
            }
        }

                public static readonly DependencyProperty TokenProperty =
        DependencyProperty.Register(
            "Token",
            typeof(TokenService.UserToken),
            typeof(TagListItem),
            new PropertyMetadata(null, TokenValueChanged));


        private static void TokenValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TagListItem obj = (TagListItem)d;
            obj.DisplayData((TokenService.UserToken)e.NewValue);
        }

        public void DisplayData(TokenService.UserToken userToken)
        {

            TokenID = userToken.Token.Id;

            if (!userToken.Token.Name.StartsWith("#"))
            {
                userToken.Token.Name = "#" + userToken.Token.Name;
            }
            
            TokenNameLBL.Text = userToken.Token.Name;

            if (userToken.Following)
            {
                ShowUnFollow.Begin();
            }
            else
            {
                ShowFollow.Begin();
            }

            UpdateDownPanel(userToken);

        }

        private void UpdateDownPanel(TokenService.UserToken userToken)
        {
            
            if (userToken.Following)
            {
                if (userToken.NoOfPeopleFollowing > 1)
                {
                    PeopleFollowingLBL.Text = userToken.NoOfPeopleFollowing + " people around you love "+userToken.Token.Name;
                }
                else
                {
                    PeopleFollowingLBL.Text = "You love "+userToken.Token.Name;
                }
            }
            else
            {
                if (userToken.NoOfPeopleFollowing > 0)
                {
                    PeopleFollowingLBL.Text = userToken.NoOfPeopleFollowing + " people around you love "+userToken.Token.Name;
                }
                else
                {
                    PeopleFollowingLBL.Text = "Love " + userToken.Token.Name + "?";
                }
            }
        }

        private void FollowBtn_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Follow();
        }

        private void Follow()
        {
            Services.TokenServiceClient.FollowTokenCompleted += TokenServiceClient_FollowTokenCompleted;
            Services.TokenServiceClient.FollowTokenAsync(Core.User.User.UserID, TokenID, Core.User.User.ZAT);
            Token.NoOfPeopleFollowing++;
            Token.Following = true;
            ShowUnFollow.Completed += ShowUnFollow_Completed;
            ShowUnFollow.Begin();
            UpdateDownPanel(Token);
        }

        void ShowUnFollow_Completed(object sender, EventArgs e)
        {
            if (FollowToken != null)
            {
                FollowToken(Token);
            }
        }

        void TokenServiceClient_FollowTokenCompleted(object sender, TokenService.FollowTokenCompletedEventArgs e)
        {
            Services.TokenServiceClient.FollowTokenCompleted -= TokenServiceClient_FollowTokenCompleted;
            if (e.Error == null)
            {
                //do nothing

            }
            else
            {
                Token.NoOfPeopleFollowing--;
                Token.Following = false;
               
                ShowFollow.Begin();
                UpdateDownPanel(Token);
            }
        }

        void ShowFollow_Completed(object sender, EventArgs e)
        {
            if (UnFollowToken != null)
            {
                UnFollowToken(Token);
            }
        }

        private void UnfollowBtn_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Unfollow();
        }

        private void Unfollow()
        {
            Services.TokenServiceClient.UnfollowTokenCompleted += TokenServiceClient_UnfollowTokenCompleted;
            Services.TokenServiceClient.UnfollowTokenAsync(Core.User.User.UserID, TokenID, Core.User.User.ZAT);
            Token.NoOfPeopleFollowing--;
            Token.Following = false;
            ShowFollow.Completed += ShowFollow_Completed;
            ShowFollow.Begin();
            UpdateDownPanel(Token);
        }

        void TokenServiceClient_UnfollowTokenCompleted(object sender, TokenService.UnfollowTokenCompletedEventArgs e)
        {
            Services.TokenServiceClient.FollowTokenCompleted -= TokenServiceClient_FollowTokenCompleted;
            if (e.Error == null)
            {
                //do nothing

            }
            else
            {
                Token.NoOfPeopleFollowing++;
                Token.Following = true;
                ShowUnFollow.Begin();
                UpdateDownPanel(Token);
            }
        }

        


        public int TokenID { get; set; }

        private void UnfollowCanvas_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Unfollow();
        }

        private void FollowCanvas_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Follow();
        }

        private void StackPanel_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (TokenNameTapped != null)
            {
                TokenNameTapped(Token.Token.Id);
            }
        }
      }
}
