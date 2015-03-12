using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Hangout.Client.Controls.Buzz
{
    public partial class BuzzCommentList : UserControl
    {
        public BuzzCommentList()
        {
            InitializeComponent();
        }

        private System.Collections.ObjectModel.ObservableCollection<BuzzService.BuzzComment> buzzCommentList;

        public System.Collections.ObjectModel.ObservableCollection<BuzzService.BuzzComment> BuzzComments
        {
            get
            {
                return buzzCommentList;
            }
            set
            {
                buzzCommentList = value;
                BuzzCommentsLB.ItemsSource = null;
                BuzzCommentsLB.ItemsSource = BuzzComments;
            }
        }


        public delegate void UserIdHelper(Guid id);

        public event UserIdHelper UserSelected;

        public delegate void EventHelper();

        public event EventHelper DataRequested;

        public event EventHelper RefreshRequested;


        void x_UserSelected(Guid id)
        {
            if(UserSelected!=null)
            {
                UserSelected(id);
            }
        }

        

        public void RefreshList()
        {
            BuzzCommentsLB.ItemsSource = null;
            BuzzCommentsLB.DataContext = null;
            BuzzCommentsLB.ItemsSource = BuzzComments;
            BuzzCommentsLB.DataContext = BuzzComments;
        }

        private void BuzzCommentItemLoaded(object sender, RoutedEventArgs e)
        {
            BuzzComment x = (BuzzComment)sender;
            x.UserSelected += x_UserSelected;
        }


        public void LoadingPBCollapse()
        {

        }

        private void BuzzLB_DataRequested(object sender, EventArgs e)
        {
            if(DataRequested!=null)
            {
                DataRequested();
            }
        }

        private void BuzzLB_RefreshRequested(object sender, EventArgs e)
        {
            if(RefreshRequested!=null)
            {
                RefreshRequested();
            }
        }

        public void StopRefreshLoading()
        {
            BuzzCommentsLB.StopPullToRefreshLoading(true);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BuzzCommentsLB.IsPullToRefreshEnabled = true;
            BuzzCommentsLB.DataVirtualizationMode = Telerik.Windows.Controls.DataVirtualizationMode.OnDemandAutomatic;

        }
    }
}
