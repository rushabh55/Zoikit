using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Hangout.Client.Controls.Trophy
{
    public partial class TrophyList : UserControl
    {
        public TrophyList()
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
                TrophyLB.ItemsSource = null;
                TrophyLB.ItemsSource = trophies;
            }
        }

        public void RefreshList()
        {
            TrophyLB.DataContext = null;
            TrophyLB.ItemsSource = null;
            TrophyLB.DataContext = trophies;
            TrophyLB.ItemsSource = trophies;
        }

        private void TrophyLB_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (TrophyLB.SelectedItem != null)
            {
                if (TrophySelected != null)
                {
                    TrophySelected((TrophyService.Trophy)TrophyLB.SelectedItem);
                }
            }
        }
    }
}
