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
    public partial class TagShortList : UserControl
    {
        public TagShortList()
        {
            InitializeComponent();
        }


        public List<UserService.Token> Tags
        {

            get { return (List<UserService.Token>)GetValue(TagsProperty); }


            set
            {
                try
                {

                    if (value.Count > 0)
                    {
                        SetValue(TagsProperty, value);
                        TagLB.ItemsSource = null;
                        TagLB.ItemsSource = Tags;
                        TagLB.Visibility = System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        TagLB.Visibility = System.Windows.Visibility.Collapsed;
                    }
                }
                catch (Exception ex)
                {
                    Core.Exceptions.ExceptionReporting.Report(ex);
                }

            }
        }
        public static readonly DependencyProperty TagsProperty =
        DependencyProperty.Register(
            "Tags",
            typeof(List<UserService.Token>),
            typeof(TagShortList),
            new PropertyMetadata(null, TagsValueChanged));


        private static void TagsValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
           
        }

        public delegate void TagSelectedHelper(int id);

        public event TagSelectedHelper TagSelected;

        private void TagLB_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (TagLB.SelectedItem != null)
            {
                UserService.Token Tag = (UserService.Token)TagLB.SelectedItem;

                if (Tag != null && TagSelected != null)
                {
                    TagSelected(Tag.Id);
                }

            }
        }

       
    }
}
