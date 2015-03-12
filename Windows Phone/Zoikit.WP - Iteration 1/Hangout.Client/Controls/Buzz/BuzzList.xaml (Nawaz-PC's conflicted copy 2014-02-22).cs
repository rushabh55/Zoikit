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
    public partial class BuzzList : UserControl
    {
        public BuzzList()
        {
            InitializeComponent();
        }

        private List<BuzzService.Buzz> buzzList;

        public List<BuzzService.Buzz> Buzzs
        {
            get
            {
                return buzzList;
            }
            set
            {
                buzzList = value;
                BuzzLB.ItemsSource = null;
                BuzzLB.ItemsSource = Buzzs;
            }
        }


        public delegate void EventHelper();


        public delegate void PeopleHelper(string name, Guid id);

        public event PeopleHelper PeopleSelected;

        public event EventHelper PulldownRefreshRequested;

        public event EventHelper NavigateToFacebook;

        public event EventHelper NavigateToTwitter;

        public event EventHelper ProgressBarShow;

        public event EventHelper ProgressBarDisappear;

        public delegate void IdHelper(Guid id);

        public event IdHelper UserSelected;

        public event IdHelper CommentOnBuzzSelected;

        public event IdHelper TagSelected;

        void x_UserSelected(Guid id)
        {
            if (UserSelected != null)
            {
                UserSelected(id);
            }
        }



        public void RefreshList()
        {
            BuzzLB.ItemsSource = null;
            BuzzLB.DataContext = null;
            BuzzLB.ItemsSource = Buzzs;
            BuzzLB.DataContext = Buzzs;
        }

        

        private void BuzzItem_Loaded(object sender, RoutedEventArgs e)
        {
            Buzz x = (Buzz)sender;
            x.UserSelected += x_UserSelected;
            x.CommentOnBuzzSelected += x_CommentOnBuzzSelected;
            x.NavigateToFacebook += x_NavigateToFacebook;
            x.NavigateToTwitter += x_NavigateToTwitter;
            x.ProgressBarDisappear += x_ProgressBarDisappear;
            x.TagSelected += x_TagSelected;
            x.ProgressBarShow += x_ProgressBarShow;
            x.PeopleSelected += x_PeopleSelected;
        }

        void x_PeopleSelected(string name, Guid id)
        {
            if(PeopleSelected!=null)
            {
                PeopleSelected(name, id);
            }
        }

        void x_TagSelected(Guid id)
        {
           if(TagSelected!=null)
           {
               TagSelected(id);
           }
        }

        void x_ProgressBarShow()
        {
            if(ProgressBarShow!=null)
            {
                ProgressBarShow();
            }
        }

        void x_ProgressBarDisappear()
        {
            if(ProgressBarDisappear!=null)
            {
                ProgressBarDisappear();
            }
        }

        void x_NavigateToTwitter()
        {
            if(NavigateToTwitter!=null)
            {
                NavigateToTwitter();
            }
        }

        void x_NavigateToFacebook()
        {
            if(NavigateToFacebook!=null)
            {
                NavigateToFacebook();
            }
        }

        void x_CommentOnBuzzSelected(Guid id)
        {
           if(CommentOnBuzzSelected!=null)
           {
               CommentOnBuzzSelected(id);
           }
        }

        private void BuzzLB_DataRequested(object sender, System.EventArgs e)
        {
            MessageBox.Show("data requested");

            //sample
            DataTemplate cardLayout = new DataTemplate();
            BuzzLB.ItemLoadingTemplate = cardLayout;

        }

        private void BuzzLB_RefreshRequested(object sender, System.EventArgs e)
        {
            
            if(PulldownRefreshRequested!=null)
            {
                PulldownRefreshRequested();
            }
        }

        public void HidePullDownRefreshLoading()
        {
            this.BuzzLB.StopPullToRefreshLoading(true);
        }
    }
}
