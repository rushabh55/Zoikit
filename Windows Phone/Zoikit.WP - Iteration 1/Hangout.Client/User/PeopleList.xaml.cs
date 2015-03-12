using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Hangout.Client.User
{
    public partial class PeopleList : PhoneApplicationPage
    {
        public PeopleList()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            UserList.UserLB.Height = 735;
            UserList.UserSelected += UserList_UserSelected;
            
            UserList.UserLB.DataVirtualizationMode = Telerik.Windows.Controls.DataVirtualizationMode.None;
            UserList.UserLB.ScrollStateChanged += UserLB_ScrollStateChanged;
            
            LoadPeople();
        }

        void UserLB_ScrollStateChanged(object sender, Telerik.Windows.Controls.ScrollStateChangedEventArgs e)
        {
           if(e.NewState==Telerik.Windows.Controls.ScrollState.BottomStretch)
           {
               LoadPeople();
           }
        }

     

        void UserList_UserSelected(Guid id)
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

        private void LoadPeople()
        {
            var text = "";

            System.Collections.ObjectModel.ObservableCollection<Guid> skipList = new System.Collections.ObjectModel.ObservableCollection<Guid>();

            

            if(UserList.Users!=null)
            {
                if(UserList.Users.Count>0)
                {
                    MoreItemsPB.Visibility = System.Windows.Visibility.Visible;
                    skipList = new System.Collections.ObjectModel.ObservableCollection<Guid>(UserList.Users.Select(o => o.UserID));
                }
                else
                {
                    ShowLoading.Begin();
                }
            }
            else
            {
                ShowLoading.Begin();

            }

            if (NavigationContext.QueryString.ContainsKey("text"))
            {
                text = NavigationContext.QueryString["text"];
            }

            if (text == "BuzzLiked")
            {
                var id = "";

                if (NavigationContext.QueryString.ContainsKey("id"))
                {
                    id = NavigationContext.QueryString["id"];
                }

                //get people who like this buzz

                LikeIcon.Visibility = System.Windows.Visibility.Visible;
                PageNameLBL.Text = "PEOPLE WHO LIKE THIS BUZZ";

                Services.BuzzServiceClient.GetUsersWhoLikeBuzzCompleted += UserserviceClient_GetUsersWhoLikeBuzzCompleted;
                Services.BuzzServiceClient.GetUsersWhoLikeBuzzAsync(Core.User.User.UserID,10,skipList, new Guid(id), Core.User.User.ZAT);

                
            }
            else if (text == "BuzzAmplified")
            {
                var id = "";

                if (NavigationContext.QueryString.ContainsKey("id"))
                {
                    id = NavigationContext.QueryString["id"];
                }

                //get people who like this buzz

                AmplifyIcon.Visibility = System.Windows.Visibility.Visible;
                PageNameLBL.Text = "PEOPLE WHO AMPLIFIED THIS BUZZ";

                Services.BuzzServiceClient.GetUsersWhoAmplifyCompleted += UserserviceClient_GetUsersWhoAmplifyCompleted;
                Services.BuzzServiceClient.GetUsersWhoAmplifyAsync(new Guid(id), 10, skipList, true, Core.User.User.UserID, Core.User.User.ZAT);

            }
            else if (text == "Followers")
            {
                var id = "";

                if (NavigationContext.QueryString.ContainsKey("id"))
                {
                    id = NavigationContext.QueryString["id"];
                }

                //get people who like this buzz

                PeopleIcon.Visibility = System.Windows.Visibility.Visible;
                PageNameLBL.Text = "FOLLOWERS";

                Services.UserServiceClient.GetUsersFollowingCompleted += UserServiceClient_GetUsersFollowingCompleted;
                Services.UserServiceClient.GetUsersFollowingAsync(Core.User.User.UserID, 10, skipList, Core.User.User.ZAT);

              

            }
            else if (text == "Following")
            {
                var id = "";

                if (NavigationContext.QueryString.ContainsKey("id"))
                {
                    id = NavigationContext.QueryString["id"];
                }

                //get people who like this buzz

                PeopleIcon.Visibility = System.Windows.Visibility.Visible;
                PageNameLBL.Text = "FOLLOWING";

                Services.UserServiceClient.GetUsersFollowedCompleted += UserServiceClient_GetUsersFollowedCompleted;
                Services.UserServiceClient.GetUsersFollowedAsync(Core.User.User.UserID, 10, skipList, Core.User.User.ZAT);

            }
            else
            {
                NaviagteToDashboard();
                return;
            }
        }

        void UserServiceClient_GetUsersFollowingCompleted(object sender, UserService.GetUsersFollowingCompletedEventArgs e)
        {
            MoreItemsPB.Visibility = System.Windows.Visibility.Collapsed;

            Services.UserServiceClient.GetUsersFollowingCompleted -= UserServiceClient_GetUsersFollowingCompleted;

            if (e.Error == null)
            {
                if (e.Result != null)
                {
                    if (UserList.Users != null)
                    {
                        foreach (UserService.User x in e.Result)
                        {
                            UserList.Users.Add(x);
                        }
                    }
                    else
                    {
                        UserList.Users = e.Result;
                    }

                    if (UserList.Users == null || UserList.Users.Count == 0)
                    {
                        ShowNoPeople.Begin();
                    }
                    else
                    {
                        ShowPage.Begin();
                    }

                }
                else
                {
                    if (UserList.Users == null || UserList.Users.Count == 0)
                    {
                        ShowNoPeople.Begin();
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
                   
                }
            }

            UserList.BuzzLoadingProgressBarCollapse();
        }

        void UserServiceClient_GetUsersFollowedCompleted(object sender, UserService.GetUsersFollowedCompletedEventArgs e)
        {
            MoreItemsPB.Visibility = System.Windows.Visibility.Collapsed;

            Services.UserServiceClient.GetUsersFollowedCompleted -= UserServiceClient_GetUsersFollowedCompleted;

            if (e.Error == null)
            {
                if (e.Result != null)
                {
                    if (UserList.Users != null)
                    {
                        foreach (UserService.User x in e.Result)
                        {
                            UserList.Users.Add(x);
                        }
                    }
                    else
                    {
                        UserList.Users = e.Result;
                    }

                    if (UserList.Users == null || UserList.Users.Count == 0)
                    {
                        ShowNoPeople.Begin();
                    }
                    else
                    {
                        ShowPage.Begin();
                    }

                }
                else
                {
                    if (UserList.Users == null || UserList.Users.Count == 0)
                    {
                        ShowNoPeople.Begin();
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
                    
                }
            }

            UserList.BuzzLoadingProgressBarCollapse();
        }

      

        void UserserviceClient_GetUsersWhoAmplifyCompleted(object sender, BuzzService.GetUsersWhoAmplifyCompletedEventArgs e)
        {
            MoreItemsPB.Visibility = System.Windows.Visibility.Collapsed;

            Services.BuzzServiceClient.GetUsersWhoAmplifyCompleted -= UserserviceClient_GetUsersWhoAmplifyCompleted;

            if (e.Error == null)
            {
                if (e.Result != null)
                {
                    if (UserList.Users != null)
                    {
                        foreach (BuzzService.User x in e.Result)
                        {
                            UserList.Users.Add(Convert(x));
                        }
                    }
                    else
                    {
                        UserList.Users = Convert(e.Result);
                    }

                    if (UserList.Users == null || UserList.Users.Count == 0)
                    {
                        ShowNoPeople.Begin();
                    }
                    else
                    {
                        ShowPage.Begin();
                    }

                }
                else
                {
                    if (UserList.Users == null || UserList.Users.Count == 0)
                    {
                        ShowNoPeople.Begin();
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
                   
                }
            }

            UserList.BuzzLoadingProgressBarCollapse();
        }

        private System.Collections.ObjectModel.ObservableCollection<UserService.User> Convert(System.Collections.ObjectModel.ObservableCollection<BuzzService.User> observableCollection)
        {
            System.Collections.ObjectModel.ObservableCollection<UserService.User> userList = new System.Collections.ObjectModel.ObservableCollection<UserService.User>();
             foreach(BuzzService.User u in observableCollection)
             {
                 userList.Add(Convert(u));
             }

             return userList;
        }

        private UserService.User Convert(BuzzService.User x)
        {
            UserService.User user = new UserService.User();
            user.AboutUs = x.AboutUs;
            user.CommonItems = x.CommonItems;
            user.IsFollowing = x.IsFollowing;
            user.Name = x.Name;
            user.ProfilePicURL = x.ProfilePicURL;
            user.UserID = x.UserID;


            return user;
        }

        void UserserviceClient_GetUsersWhoLikeBuzzCompleted(object sender, BuzzService.GetUsersWhoLikeBuzzCompletedEventArgs e)
        {
            MoreItemsPB.Visibility = System.Windows.Visibility.Collapsed;

            Services.BuzzServiceClient.GetUsersWhoLikeBuzzCompleted -= UserserviceClient_GetUsersWhoLikeBuzzCompleted;

            if (e.Error == null)
            {
                if (e.Result != null)
                {
                    if (UserList.Users != null)
                    {
                        foreach (BuzzService.User x in e.Result)
                        {
                            UserList.Users.Add(Convert(x));
                        }
                    }
                    else
                    {
                        UserList.Users = Convert(e.Result);
                    }

                    if (UserList.Users == null || UserList.Users.Count == 0)
                    {
                        ShowNoPeople.Begin();
                    }
                    else
                    {
                        ShowPage.Begin();
                    }

                }
                else
                {
                    if (UserList.Users == null || UserList.Users.Count == 0)
                    {
                        ShowNoPeople.Begin();
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
                    
                }
            }

            UserList.BuzzLoadingProgressBarCollapse();
        }

        

        private void NaviagteToDashboard()
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Dashboard, UriKind.RelativeOrAbsolute));
            });
        }

        
    }
}