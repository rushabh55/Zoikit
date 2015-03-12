using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Hangout.Client.Controls.Tag
{
    public partial class TagItem : UserControl
    {
        public TagItem()
        {
            InitializeComponent();
        }

        public delegate void IdHelper(Guid id);
       
        public delegate void NameHelper(String name);
        public event NameHelper TagSelected;


        public DiscoverService.UserTag TagData
        {
            get { return (DiscoverService.UserTag)GetValue(TagProperty); }

            set
            {
                SetValue(TagProperty, value);
            }
        }

        public static readonly DependencyProperty TagProperty =
       DependencyProperty.Register(
           "TagData",
           typeof(DiscoverService.UserTag),
           typeof(TagItem),
           new PropertyMetadata(null, TagValueChanged));


        private static void TagValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TagItem obj = d as TagItem;
            obj.DisplayData((DiscoverService.UserTag)e.NewValue);
        }

        public void DisplayData(DiscoverService.UserTag tag)
        {
            NameLBL.Text = tag.Tag.Name;

            if(tag.Following)
            {
                tag.NoOfLocalPeopleFollowing--;
                ShowFollowingBtn.Begin();
            }
            else
            {
                ShowFollowBtn.Begin();    
            }

            if(tag.NoOfLocalPeopleFollowing>0)
            {
                AboutMeLBL.Text = tag.NoOfLocalPeopleFollowing + " people are following";
            }
            else
            {
                AboutMeLBL.Visibility = System.Windows.Visibility.Collapsed;
            }
            

        }


        private void Canvas_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if(TagSelected!=null)
            {
                TagSelected(TagData.Tag.Name);
            }
        }

        private void NameLBL_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (TagSelected != null)
            {
                TagSelected(TagData.Tag.Name);
            }
        }

        

        private void FollowBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (this.TagData != null)
            {

                if (!this.TagData.Following)
                {
                    this.TagData.Following = true;
                    ShowFollowingBtn.Begin();
                    Services.TagServiceClient.FollowTagCompleted += TagServiceClient_FollowTagCompleted;
                    Services.TagServiceClient.FollowTagAsync(Core.User.User.UserID, TagData.Tag.Id, Core.Location.CurrentLocation.Location.City.Id, Core.User.User.ZAT);
                }

            }
        }

        void TagServiceClient_FollowTagCompleted(object sender, TagService.FollowTagCompletedEventArgs e)
        {
            Services.TagServiceClient.FollowTagCompleted -= TagServiceClient_FollowTagCompleted;

            if (e.Error != null)
            {
                TagData.Following = false;
                ShowFollowBtn.Begin();
                MessageBox.Show(Messages.CantFollow, Messages.Sorry, MessageBoxButton.OK);
            }
        }

        private void UnFollowBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (this.TagData != null)
            {

                if (this.TagData.Following)
                {
                    this.TagData.Following = false;
                    ShowFollowBtn.Begin();
                    Services.TagServiceClient.UnfollowTagCompleted += TagServiceClient_UnfollowTagCompleted;
                    Services.TagServiceClient.UnfollowTagAsync(Core.User.User.UserID, TagData.Tag.Id, Core.Location.CurrentLocation.Location.City.Id, Core.User.User.ZAT);
                }

            }
        }

        void TagServiceClient_UnfollowTagCompleted(object sender, TagService.UnfollowTagCompletedEventArgs e)
        {
            Services.TagServiceClient.UnfollowTagCompleted -= TagServiceClient_UnfollowTagCompleted;

            if (e.Error != null)
            {
                TagData.Following = true;
                ShowFollowingBtn.Begin();
                MessageBox.Show(Messages.CantUnfollow, Messages.Sorry, MessageBoxButton.OK);
            }
        }
    }
}
