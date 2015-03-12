using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Input;

namespace Hangout.Client.Buzz
{
    public partial class BuzzComments : PhoneApplicationPage
    {
        public BuzzComments()
        {
            InitializeComponent();
        }

        private void NaviagteToDashboard()
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Dashboard, UriKind.RelativeOrAbsolute));
            });
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            CommentLB.BuzzCommentsLB.Height = 650;

            CommentLB.UserSelected += CommentLB_UserSelected;
            CommentTB.TextChanged += CommentTB_TextChanged;
            CommentTB.KeyDown += CommentTB_KeyDown;
            CommentLB.RefreshRequested += CommentLB_RefreshRequested;
            LoadComments();
        }

        void CommentTB_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key==Key.Enter)
            {
                if(CommentTB.Text.Count()>0)
                {
                    //update comment. 

                    if (NavigationContext.QueryString.ContainsKey("id"))
                    {
                        var id = NavigationContext.QueryString["id"];

                        Guid lastcommentId = new Guid();

                        if (CommentLB.BuzzComments != null)
                        {
                            if (CommentLB.BuzzComments.Count > 0)
                            {
                                lastcommentId = CommentLB.BuzzComments.FirstOrDefault().CommentID;
                            }
                        }

                        TopLoaderVisible();

                        Services.BuzzServiceClient.AddBuzzCommentCompleted += BuzzServiceClient_AddBuzzCommentCompleted;
                        Services.BuzzServiceClient.AddBuzzCommentAsync(Core.User.User.UserID, new Guid(id),CommentTB.Text.Trim(),lastcommentId, Core.User.User.ZAT);

                        CommentTB.Text = "";
                    }
                    else
                    {
                        NaviagteToDashboard();
                    }

                }
            }
        }

        private void TopLoaderVisible()
        {
            TopPB.Visibility = System.Windows.Visibility.Visible;
        }

        void BuzzServiceClient_AddBuzzCommentCompleted(object sender, BuzzService.AddBuzzCommentCompletedEventArgs e)
        {
            Services.BuzzServiceClient.AddBuzzCommentCompleted -= BuzzServiceClient_AddBuzzCommentCompleted;

            if (e.Error == null)
            {
                if (e.Result != null)
                {
                    if (CommentLB.BuzzComments == null)
                    {
                        CommentLB.BuzzComments = new System.Collections.ObjectModel.ObservableCollection<BuzzService.BuzzComment>();
                    }



                    for (int i = e.Result.Count - 1; i >= 0; i--)
                    {
                        CommentLB.BuzzComments.Insert(0, e.Result[i]);
                    }


                    if (e.Result.Count() > 0)
                    {
                        ShowPage.Begin();
                    }

                }
            }
            else
            {
                //Show error LBL
                ShowError.Begin();
            }

            TopLoaderCollapsed();
        }

        private void TopLoaderCollapsed()
        {
            TopPB.Visibility = System.Windows.Visibility.Collapsed;
        }

        void CommentTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            CommentCharCountLBL.Text = (140 - (CommentTB.Text.Count())).ToString();

            if (CommentTB.Text.Count() > 0)
            {
                typewEmailLBL.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                typewEmailLBL.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void LoadComments()
        {
            var id = "";

            System.Collections.ObjectModel.ObservableCollection<Guid> skipList = new System.Collections.ObjectModel.ObservableCollection<Guid>();



            if (CommentLB.BuzzComments != null)
            {
                if (CommentLB.BuzzComments.Count > 0)
                {
                    skipList = new System.Collections.ObjectModel.ObservableCollection<Guid>(CommentLB.BuzzComments.Select(o => o.CommentID));
                }
                else
                {
                    ShowCommentLoading.Begin();
                }
            }
            else
            {
                ShowCommentLoading.Begin();

            }

            if (NavigationContext.QueryString.ContainsKey("id"))
            {
                id = NavigationContext.QueryString["id"];



                Services.BuzzServiceClient.GetBuzzCommentsCompleted += BuzzServiceClient_GetBuzzCommentsCompleted;
                Services.BuzzServiceClient.GetBuzzCommentsAsync(Core.User.User.UserID, new Guid(id), 10, skipList, Core.User.User.ZAT);
            }
            else
            {
                NaviagteToDashboard();
            }



            
        }

        void BuzzServiceClient_GetBuzzCommentsCompleted(object sender, BuzzService.GetBuzzCommentsCompletedEventArgs e)
        {
            Services.BuzzServiceClient.GetBuzzCommentsCompleted -= BuzzServiceClient_GetBuzzCommentsCompleted;

            

            if (e.Error == null)
            {
                if (e.Result != null)
                {
                    if (CommentLB.BuzzComments != null)
                    {
                        foreach (BuzzService.BuzzComment x in e.Result)
                        {
                            CommentLB.BuzzComments.Add(x);
                        }
                    }
                    else
                    {
                        CommentLB.BuzzComments = e.Result;
                    }

                    if (CommentLB.BuzzComments == null || CommentLB.BuzzComments.Count == 0)
                    {
                        ShowNoComments.Begin();
                    }
                    else
                    {
                        ShowPage.Begin();
                    }

                }
                else
                {
                    if (CommentLB.BuzzComments == null || CommentLB.BuzzComments.Count == 0)
                    {
                        ShowNoComments.Begin();
                    }
                }

            }
            else
            {
                //show error LBL
                ShowError.Begin();
            }


            if (e.Result != null)
            {
                if (e.Result.Count == 10)
                {
                    CommentLB.DataRequested +=CommentLB_DataRequested;
                }
            }

            CommentLB.LoadingPBCollapse();
        }

       

        void CommentLB_RefreshRequested()
        {
            //get buzz comment before method. 

            if (NavigationContext.QueryString.ContainsKey("id"))
            {
                var id = NavigationContext.QueryString["id"];

                Guid lastcommentId=new Guid();

                if(CommentLB.BuzzComments!=null)
                {
                    if(CommentLB.BuzzComments.Count>0)
                    {
                        lastcommentId = CommentLB.BuzzComments.FirstOrDefault().CommentID;
                    }
                }

                Services.BuzzServiceClient.GetBuzzCommentsBeforeCompleted += BuzzServiceClient_GetBuzzCommentsBeforeCompleted;
                Services.BuzzServiceClient.GetBuzzCommentsBeforeAsync(Core.User.User.UserID, new Guid(id),lastcommentId,Core.User.User.ZAT);
            }
            else
            {
                NaviagteToDashboard();
            }
            
        }

        void BuzzServiceClient_GetBuzzCommentsBeforeCompleted(object sender, BuzzService.GetBuzzCommentsBeforeCompletedEventArgs e)
        {
            Services.BuzzServiceClient.GetBuzzCommentsBeforeCompleted -= BuzzServiceClient_GetBuzzCommentsBeforeCompleted;

            if (e.Error == null)
            {
                if (e.Result != null)
                {
                    if (CommentLB.BuzzComments == null)
                    {
                        CommentLB.BuzzComments = new System.Collections.ObjectModel.ObservableCollection<BuzzService.BuzzComment>();
                    }

                   

                    for (int i = e.Result.Count - 1; i >= 0; i--)
                    {
                        CommentLB.BuzzComments.Insert(0, e.Result[i]);
                    }


                    if (e.Result.Count() > 0)
                    {
                        ShowPage.Begin();
                    }

                }
            }
            else
            {
                //Show error LBL
                ShowError.Begin();
            }

            CommentLB.StopRefreshLoading();
        }

        

        void CommentLB_DataRequested()
        {
            LoadComments();
        }

        void CommentLB_UserSelected(Guid id)
        {
            NavigateToUser(id);
        }


        private void NavigateToUser(Guid id)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.UserProfile + "?id=" + id.ToString(), UriKind.RelativeOrAbsolute));
            });
        }

        private void typewEmailLBL_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            CommentTB.Focus();
        }
    }
}