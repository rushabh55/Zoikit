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
    public partial class DiscoverList : UserControl
    {
        public DiscoverList()
        {
            InitializeComponent();
        }

        private System.Collections.ObjectModel.ObservableCollection<DiscoverService.DiscoverObj> discoverList;

        public System.Collections.ObjectModel.ObservableCollection<DiscoverService.DiscoverObj> DiscoverListData
        {
            get
            {
                return discoverList;
            }
            set
            {
                discoverList = value;
                DiscoverLB.ItemsSource = null;
                DiscoverLB.ItemsSource = discoverList;
            }
        }


        public delegate void IdHelper(Guid id);
        public delegate void NameHelper(String name);
        public event IdHelper PeopleWhoFollowTagSelected;
        public event NameHelper TagSelected;
        public event IdHelper UserSelected;
        public event EventHandler MoreItemsRequested;

        private void DiscoverItem_Loaded(object sender, RoutedEventArgs e)
        {
            DiscoverItem item = sender as DiscoverItem;
            if (item != null)
            {
                item.UserSelected += item_UserSelected;
                item.TagSelected += item_TagSelected;
                item.PeopleWhoFollowTagSelected += item_PeopleWhoFollowTagSelected;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        void item_PeopleWhoFollowTagSelected(Guid id)
        {
            if(PeopleWhoFollowTagSelected!=null)
            {
                PeopleWhoFollowTagSelected(id);
            }
        }

        void item_TagSelected(String name)
        {
            if(TagSelected!=null)
            {
                TagSelected(name);
            }
        }

        void item_UserSelected(Guid id)
        {
            if(UserSelected!=null)
            {
                UserSelected(id);
            }
        }

        private void DiscoverLB_DataRequested(object sender, EventArgs e)
        {
            if(MoreItemsRequested!=null)
            {
                MoreItemsRequested(null, null);
            }
        }

        internal void BuzzLoadingProgressBarCollapse()
        {
           
        }
    }
}
