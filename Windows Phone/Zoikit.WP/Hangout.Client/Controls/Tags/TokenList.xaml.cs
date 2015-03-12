using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Hangout.Client.Controls.Tags
{
    public partial class TokenList : UserControl
    {
        public TokenList()
        {
            InitializeComponent();
        }

        public delegate void TokenHelper(TokenService.UserToken token);

        public event TokenHelper FollowToken;

        public event TokenHelper UnFollowToken;

        public event EventHandler MoreBtnClicked;

        private List<TokenService.UserToken> tokens;

        public List<TokenService.UserToken> Tokens
        {
            get
            {
                return tokens;
            }

            set
            {
                tokens = value;
                TagLB.ItemsSource = null;
                TagLB.ItemsSource = Tokens;
            }
        }


        public delegate void IdHelper(int id);


        public event IdHelper TokenSelected;



        private void TagLI_Loaded_1(object sender, RoutedEventArgs e)
        {
            TagListItem li = (TagListItem)sender;
            
            li.TokenNameTapped += li_TokenNameTapped;
            li.FollowToken += li_FollowToken;
            li.UnFollowToken += li_UnFollowToken;
        }

        void li_UnFollowToken(TokenService.UserToken token)
        {
           if(UnFollowToken!=null)
           {
               UnFollowToken(token);
           }
        }

        void li_FollowToken(TokenService.UserToken token)
        {
            if (FollowToken != null)
            {
                FollowToken(token);
            }
        }

        void li_TokenNameTapped(int id)
        {
            if (TokenSelected != null)
            {
                TokenSelected(id);
            }
        }

       

        private void MoreBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (MoreBtnClicked != null)
            {
                MoreBtnClicked(null, new EventArgs());
            }
        }

        public void ShowMoreButton()
        {
            MorePG.Visibility = System.Windows.Visibility.Visible;
            MoreBtn.Visibility = System.Windows.Visibility.Collapsed;
        }

        public void ShowProgressBar()
        {
            MorePG.Visibility = System.Windows.Visibility.Collapsed;
            MoreBtn.Visibility = System.Windows.Visibility.Visible;
        }

        public void HideDownPanel()
        {
            MorePG.Visibility = System.Windows.Visibility.Collapsed;
            MoreBtn.Visibility = System.Windows.Visibility.Collapsed;
        }

        public void RefreshList()
        {
            TagLB.DataContext = null;
            TagLB.ItemsSource = null;
            TagLB.DataContext = tokens;
            TagLB.ItemsSource = tokens;
        }
    }
}
