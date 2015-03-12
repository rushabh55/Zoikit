using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;

namespace Hangout.Client.Controls.Buzz
{
    public partial class BuzzList : UserControl
    {
        public BuzzList()
        {
            InitializeComponent();
        }

        private System.Collections.ObjectModel.ObservableCollection<BuzzService.Buzz> buzzList;

        public System.Collections.ObjectModel.ObservableCollection<BuzzService.Buzz> Buzzs
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

        public System.Windows.Visibility BuzzLoadingVisibility { get; set; }

        public delegate void PeopleHelper(string name, Guid id);

        public delegate void TagHelper(string name);

        public event PeopleHelper PeopleSelected;

        public event EventHelper NavigateToFacebook;

        public event EventHelper ScrollingTop;

        public event EventHelper ScrollingDown;


        public event EventHelper NavigateToTwitter;

        public event EventHelper PullToRefreshEvent;

        public event EventHelper MoreDataRequested;

        public event EventHelper ProgressBarShow;

        public event EventHelper ProgressBarDisappear;

        public delegate void IdHelper(Guid id);

        public event IdHelper UserSelected;

        public event IdHelper CommentOnBuzzSelected;

        public event TagHelper TagSelected;

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

        void x_TagSelected(string name)
        {
           if(TagSelected!=null)
           {
               TagSelected(name);
           }
        }

        public void DisableVerticalScrollBar()
        {
            
            ////disable vertical scrollbar of scrollviewer. 
            if (VisualTreeHelper.GetChildrenCount(BuzzLB) > 0)
            {
                DependencyObject obj = VisualTreeHelper.GetChild(BuzzLB, 0);
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
            

           if(MoreDataRequested!=null)
           {
               MoreDataRequested();
           }

           

           
        }

        public void BuzzLoadingProgressBarCollapse()
        {
            
        }

        public void BuzzLoadingProgressBarVisible()
        {
            
        }

        private void BuzzLB_RefreshRequested(object sender, System.EventArgs e)
        {
            if(PullToRefreshEvent!=null)
            {
                PullToRefreshEvent();
            }
        }

       

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this; 
        }

        public void StopPullToRefreshLoading()
        {
            BuzzLB.StopPullToRefreshLoading(true);
        }

        private void BuzzLB_ScrollStateChanged(object sender, Telerik.Windows.Controls.ScrollStateChangedEventArgs e)
        {
            if(e.NewState==Telerik.Windows.Controls.ScrollState.TopStretch)
            {
                if(ScrollingTop!=null)
                {
                    ScrollingTop();
                }
            }
            else
            {
                if (ScrollingDown != null)
                {
                    ScrollingDown();
                }
            }
            

            

            
        }
    }
}
