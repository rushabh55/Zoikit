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
    public partial class BadgeList : UserControl
    {
        public BadgeList()
        {
            InitializeComponent();
            TiltEffect.TiltableItems.Add(typeof(Grid));
        }

        public delegate void TrophySelectedHelper(TrophyService.Trophy trophy);

        public event TrophySelectedHelper TrophySelected;

        private List<TrophyService.Trophy> trophies;

        public List<TrophyService.Trophy> Trophies
        {
            get
            {
                return trophies;
            }

            set
            {
                trophies = value;
                BadgeLB.ItemsSource = null;
                BadgeLB.ItemsSource = trophies;
            }
        }

        public void RefreshList()
        {
            BadgeLB.DataContext = null;
            BadgeLB.ItemsSource = null;
            BadgeLB.DataContext = trophies;
            BadgeLB.ItemsSource = trophies;
        }

        private void TrophyLB_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (BadgeLB.SelectedItem != null)
            {
                if (TrophySelected != null)
                {
                    TrophySelected((TrophyService.Trophy)BadgeLB.SelectedItem);
                }
            }
        }
    }
}
