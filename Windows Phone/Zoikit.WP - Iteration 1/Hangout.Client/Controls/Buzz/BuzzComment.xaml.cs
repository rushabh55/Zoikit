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
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Hangout.Client.Controls.Buzz
{
    public partial class BuzzComment : UserControl
    {
        public BuzzComment()
        {
            InitializeComponent();
        }

        public BuzzService.BuzzComment Comment
        {
            get { return (BuzzService.BuzzComment)GetValue(BuzzProperty); }

            set
            {
                SetValue(BuzzProperty, value);
            }
        }

        public delegate void UserIdHelper(Guid id);

        public event UserIdHelper UserSelected;

        public static readonly DependencyProperty BuzzProperty =
        DependencyProperty.Register(
            "Comment",
            typeof(BuzzService.BuzzComment),
            typeof(BuzzComment),
            new PropertyMetadata(null, BuzzCommentValueChanged));


        private static void BuzzCommentValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BuzzComment obj = (BuzzComment)d;
            obj.DisplayData((BuzzService.BuzzComment)e.NewValue);
        }

        public void DisplayData(BuzzService.BuzzComment comment)
        {
            Core.Converters.PostedAdder obj = new Core.Converters.PostedAdder();

            AddBuzzCommentText("\"" + comment.Comment + "\"");
            PostedLBL.Text = obj.Convert(comment.DatePosted, null, null, System.Globalization.CultureInfo.CurrentCulture).ToString();
            NameLBL.Text = comment.User.Name;

            if (comment.User.ProfilePicURL != null)
            {
                ProfilePicImage.Source = new BitmapImage(new Uri(comment.User.ProfilePicURL, UriKind.RelativeOrAbsolute));
            }
        }

       

        private void AddBuzzCommentText(string text)
        {
            CommentLBL.Text = "";
            String[] t = text.Split(' ');
            String tmp = "";
            foreach (String s in t)
            {
                if (s.StartsWith("#"))
                {
                    //print tmp
                    CommentLBL.Inlines.Add(new Run() { Text = tmp });
                    //print s
                    CommentLBL.Inlines.Add(new Run() { Text = s + " ", Foreground = new SolidColorBrush(Color.FromArgb(255, 238, 123, 68)) });
                    //flush tmp
                    tmp = "";
                }
                else
                {
                    tmp += s + " ";
                }

            }

            //print tmp.
            CommentLBL.Inlines.Add(new Run() { Text = tmp + " " });
            //flish tmp
            tmp = "";

        }

       
      
        private void LayoutRoot_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (UserSelected != null)
            {
                UserSelected(Comment.User.UserID);
            }
        }

        private void NameLBL_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (UserSelected != null)
            {
                UserSelected(Comment.User.UserID);
            }
        }

        private void PostedLBL_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (UserSelected != null)
            {
                UserSelected(Comment.User.UserID);
            }
        }

        private void CommentLBL_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (UserSelected != null)
            {
                UserSelected(Comment.User.UserID);
            }
        }

       
    }
}
