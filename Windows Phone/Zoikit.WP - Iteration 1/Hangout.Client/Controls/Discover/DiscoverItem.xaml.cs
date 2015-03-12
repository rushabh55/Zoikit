using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Hangout.Client.Controls.Discover
{
    public partial class DiscoverItem : UserControl
    {
        public DiscoverItem()
        {
            InitializeComponent();
        }


        public delegate void IdHelper(Guid id);
        public delegate void NameHelper(String name);

        public event IdHelper PeopleWhoFollowTagSelected;
        public event NameHelper TagSelected;
        public event IdHelper UserSelected; 

        public DiscoverService.DiscoverObj DiscoverData
        {
            get { return (DiscoverService.DiscoverObj)GetValue(DiscoverProperty); }

            set
            {
                SetValue(DiscoverProperty, value);
            }
        }

        public static readonly DependencyProperty DiscoverProperty =
       DependencyProperty.Register(
           "DiscoverData",
           typeof(DiscoverService.DiscoverObj),
           typeof(DiscoverItem),
           new PropertyMetadata(null, DiscoverValueChanged));


        private static void DiscoverValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiscoverItem obj = (DiscoverItem)d;
            obj.DisplayData((DiscoverService.DiscoverObj)e.NewValue);
        }

        public void DisplayData(DiscoverService.DiscoverObj data)
        {
            if(data.User==null)
            {
                UserListItem.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                UserListItem.User = Convert(data.User);
            }

            if(data.Tag==null)
            {
                this.TagItem.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                this.TagItem.TagData = data.Tag;
            }
            
        }

        private UserService.User Convert(DiscoverService.User user)
        {
            UserService.User usr = new UserService.User();
            usr.AboutUs = user.AboutUs;
            usr.CommonItems = user.CommonItems;
            usr.IsFollowing = user.IsFollowing;
            usr.ProfilePicURL = user.ProfilePicURL;
            usr.Name = user.Name;
            usr.UserID = user.UserID;

            return usr;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.TagItem.TagSelected += TagItem_TagSelected;
            
            this.UserListItem.UserSelected += UserListItem_UserSelected;
        }

        void UserListItem_UserSelected(Guid id)
        {
            if(UserSelected!=null)
            {
                UserSelected(id);
            }
        }

        void TagItem_PeopleWhoFollowTagSelected(Guid id)
        {
            if(PeopleWhoFollowTagSelected!=null)
            {
                PeopleWhoFollowTagSelected(id);
            }
        }

        void TagItem_TagSelected(String name)
        {
            if (TagSelected != null)
            {
                TagSelected(name);

            }


        }
    }
}
