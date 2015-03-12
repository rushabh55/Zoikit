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
    public partial class CategoryShortList : UserControl
    {
        public CategoryShortList()
        {
            InitializeComponent();
        }

        public List<CategoryService.Category> Categories
        {
            get
            {
                return categories;
            }

            set
            {
                if (value == null)
                {
                    return;
                }

                categories = value;

                CategoryLB.DataContext = null;
                CategoryLB.ItemsSource = null;
                CategoryLB.DataContext = categories;
                CategoryLB.ItemsSource = categories;
            }
        }

        private List<CategoryService.Category> categories;

        private void UserShortDisplayLB_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
           
        }

        public delegate void CategorySelectedHelper(int id);

        public event CategorySelectedHelper CategorySelected;

        private void CategoryLB_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (CategoryLB.SelectedItem != null)
            {
                CategoryService.Category category = (CategoryService.Category)CategoryLB.SelectedItem;

                if (category != null && CategorySelected != null)
                {
                    CategorySelected(category.CategoryID);
                }

            }
        }

    }
}
