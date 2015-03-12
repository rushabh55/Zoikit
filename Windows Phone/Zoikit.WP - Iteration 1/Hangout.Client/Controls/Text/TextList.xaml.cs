using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Hangout.Client.Controls.Text
{
    public partial class TextList : UserControl
    {
        public TextList()
        {
            InitializeComponent();
        }

        private System.Collections.ObjectModel.ObservableCollection<TextService.Text> texts;

        public System.Collections.ObjectModel.ObservableCollection<TextService.Text> Texts
        {
            get
            {
                return texts;
            }
            set
            {
                texts = value;
                TextLB.ItemsSource = null;
                TextLB.ItemsSource = Texts;
            }
        }


        public delegate void IdHelper(Guid id);

        public event IdHelper TextSelected;

        public event EventHandler DataRequested; 
      

        private void TextLoaded(object sender, RoutedEventArgs e)
        {
            TextListItem o = sender as TextListItem;

            o.TextSelected += o_TextSelected;
        }

        void o_TextSelected(Guid id)
        {
            if(TextSelected!=null)
            {
                TextSelected(id);
            }
        }

        private void BuzzLB_DataRequested(object sender, EventArgs e)
        {
            if(DataRequested!=null)
            {
                DataRequested(null, null);
            }
        }

        internal void CollapseLoader()
        {
            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TextLB.DataVirtualizationMode = Telerik.Windows.Controls.DataVirtualizationMode.OnDemandAutomatic;
        }
    }
}
