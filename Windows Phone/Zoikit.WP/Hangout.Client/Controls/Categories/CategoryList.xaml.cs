using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Hangout.Client.Controls.Categories
{
    public partial class CategoryList : UserControl
    {

        public int selectedCategoryId { get; set; }

        private List<CategoryService.UserCategory> categories;


        public List<CategoryService.UserCategory> Categories
        {
            get
            {
                return categories;
            }

            set
            {
                categories = value;
                if (categories.Count > 0)
                {
                   CategoriesLB.DataContext = null;
                   CategoriesLB.ItemsSource = null;
                   CategoriesLB.DataContext = categories;
                   CategoriesLB.ItemsSource = categories;
                   CategoriesLB.Visibility = System.Windows.Visibility.Visible;
                   ShowPage.Begin();
                }
                else
                {
                   CategoriesLB.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        public delegate void CategorySelectedHelper(int categoryId);

        public event CategorySelectedHelper CategorySelected;

        public CategoryList()
        {
            InitializeComponent();
        }

        public void FollowCategory(int id)
        {
            Services.CategoryServiceClient.FollowCategoryCompleted += CategoryServiceClient_FollowCategoryCompleted;
            Services.CategoryServiceClient.FollowCategoryAsync(Core.User.User.UserID, id, Core.User.User.ZAT);
        }

        void CategoryServiceClient_FollowCategoryCompleted(object sender, CategoryService.FollowCategoryCompletedEventArgs e)
        {
            Services.CategoryServiceClient.FollowCategoryCompleted -= CategoryServiceClient_FollowCategoryCompleted;
            if (e.Error == null)
            {
                if (e.Result == CategoryService.FollowResult.Following)
                {
                    

                }
            }
        }

        public void UnfollowCategory(int id)
        {
            Services.CategoryServiceClient.UnfollowCategoryCompleted += CategoryServiceClient_UnfollowCategoryCompleted;
            Services.CategoryServiceClient.UnfollowCategoryAsync(Core.User.User.UserID, id, Core.User.User.ZAT);
        }

        void CategoryServiceClient_UnfollowCategoryCompleted(object sender, CategoryService.UnfollowCategoryCompletedEventArgs e)
        {
            Services.CategoryServiceClient.UnfollowCategoryCompleted -= CategoryServiceClient_UnfollowCategoryCompleted;
            if (e.Error == null)
            {
                if (e.Result == CategoryService.FollowResult.Unfollowed)
                {
                    
                }
            }
        }



        private void CategoriesLB_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (CategoriesLB.SelectedItem != null)
            {
                
                CategoryService.UserCategory cat = (CategoryService.UserCategory)CategoriesLB.SelectedItem;
                selectedCategoryId = cat.Category.CategoryID;
                

                if (CategorySelected != null)
                {
                    CategorySelected(cat.Category.CategoryID);
                }


            }
        }

        public void ShowLoading()
        {
            //set the selectedCategoryId as a DataContext
            this.DataContext = this;

            
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            ShowLoading();
        }

       

        private void FollowBtn_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            CategoryService.UserCategory cat = (sender as Button).DataContext as CategoryService.UserCategory;

            if (cat != null)
            {
                FollowCategory(cat.Category.CategoryID);
                Categories.Single(o => o.Category.CategoryID == cat.Category.CategoryID).Following = true;
            }

            
        }

        private void UnfollowBtn_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            CategoryService.UserCategory cat = (sender as Button).DataContext as CategoryService.UserCategory;

            if (cat != null)
            {
                UnfollowCategory(cat.Category.CategoryID);
                Categories.Single(o => o.Category.CategoryID == cat.Category.CategoryID).Following = false;
            }
        }

        



    }
}
