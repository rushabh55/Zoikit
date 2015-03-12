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

        public event EventHandler MorebuttonTapped;

        private bool showMoreButton;

        public bool ShowMoreButtonVisibility
        {
            get
            {
                return showMoreButton;

            }

            set
            {
                showMoreButton = value;
                if (value)
                {
                    GetMoreButton.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    GetMoreButton.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        private List<BuzzService.Buzz> buzzList;

        public List<BuzzService.Buzz> Buzzes
        {
            get
            {
                return buzzList;
            }
            set
            {
                buzzList = value;
                BuzzLB.ItemsSource = null;
                BuzzLB.ItemsSource = Buzzes;
            }
        }

        void x_BuzzSelected(int id)
        {
            if (BuzzSelected != null)
            {
                BuzzSelected(id);
            }
           
        }

        void x_UserSelected(int id)
        {
            if (UserSelected != null)
            {
                UserSelected(id);
            }
        }

        

        public delegate void IdHelper(int id);

        public event IdHelper UserSelected;

        public event IdHelper BuzzSelected;

        private void BuzzItem_Loaded_1(object sender, RoutedEventArgs e)
        {
            BuzzListItem x = (BuzzListItem)sender;
            x.UserSelected += x_UserSelected;
            x.BuzzSelected += x_BuzzSelected;
        }

        private void GetMoreButton_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (MorebuttonTapped != null)
            {
                MorebuttonTapped(null, new EventArgs());
            }
        }

        public void RefreshList()
        {
            BuzzLB.ItemsSource = null;
            BuzzLB.DataContext = null;
            BuzzLB.ItemsSource = buzzList;
            BuzzLB.DataContext = buzzList;
        }

       
        public void ShowMoreButton()
        {
            MorePG.Visibility = System.Windows.Visibility.Collapsed;
            GetMoreButton.Visibility = System.Windows.Visibility.Visible;
        }

        public void ShowProgressBar()
        {
            MorePG.Visibility = System.Windows.Visibility.Visible;
            GetMoreButton.Visibility = System.Windows.Visibility.Collapsed;
        }

        public void HideDownPanel()
        {
            MorePG.Visibility = System.Windows.Visibility.Collapsed;
            GetMoreButton.Visibility = System.Windows.Visibility.Collapsed;
        }

       
   

    }
}
