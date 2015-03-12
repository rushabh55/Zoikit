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

namespace Hangout.Client
{
    public partial class Search : PhoneApplicationPage
    {
        public Search()
        {
            InitializeComponent();
        }

        public string BuzzSearchText { get; set; }
        public string PeopleSearchText { get; set; }

        private void LocationSP_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.LocationPicker, UriKind.RelativeOrAbsolute));
                });

            }
            catch { }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                if (SelectedCity != Core.Location.Location.SelectedCity)
                {
                    //city chage
                    SelectedCity = Core.Location.Location.SelectedCity;
                    LocationLBL.Text = SelectedCity.Name + ", " + SelectedCity.Country.Name;
                    BuzzLB.Buzzs = new System.Collections.ObjectModel.ObservableCollection<BuzzService.Buzz>();
                    UserList.Users = new System.Collections.ObjectModel.ObservableCollection<UserService.User>();
                    PeopleSearch.Visibility = System.Windows.Visibility.Visible;
                    BuzzSearch.Visibility = System.Windows.Visibility.Visible;
                    PeopleSearch.Opacity = 1;
                    BuzzSearch.Opacity = 1;

                }
            }
            catch { }

        }

        void BuzzLB_PeopleSelected(string text, Guid id)
        {
            try
            {

                NavigateToPeopleList(text, id);
            }
            catch { }
        }

        private void NavigateToPeopleList(string text, Guid id)
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.PeopleList + "?text=" + text + "&id=" + id.ToString(), UriKind.RelativeOrAbsolute));
                });
            }
            catch { }
        }

        void BuzzLB_TagSelected(string name)
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.Tag + "?name=" + name, UriKind.RelativeOrAbsolute));
                });
            }
            catch { }
        }

        void BuzzLB_NavigateToTwitter()
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.TwitterConnect, UriKind.RelativeOrAbsolute));
                });
            }
            catch { }
        }

        void BuzzLB_NavigateToFacebook()
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.FacebookConnect, UriKind.RelativeOrAbsolute));
                });
            }
            catch { }
        }


        void BuzzLB_UserSelected(Guid id)
        {
            try
            {
                NavigateToUserProfile(id);
            }
            catch { }
        }

        private void NavigateToUserProfile(Guid id)
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.UserProfile + "?id=" + id.ToString(), UriKind.RelativeOrAbsolute));
                });
            }
            catch { }
        }

        void BuzzLB_CommentOnBuzzSelected(Guid id)
        {
            try
            {
                NavigateToBuzzCommentPage(id);
            }
            catch { }
        }

        private void NavigateToBuzzCommentPage(Guid id)
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.BuzzComments + "?id=" + id.ToString(), UriKind.RelativeOrAbsolute));
                });
            }
            catch { }
        }

        void UserList_UserSelected(Guid id)
        {
            try
            {
                NavigateToUser(id);
            }
            catch { }
        }

        private void NavigateToUser(Guid id)
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.UserProfile + "?id=" + id.ToString(), UriKind.RelativeOrAbsolute));
                });
            }
            catch { }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                BuzzLB.CommentOnBuzzSelected += BuzzLB_CommentOnBuzzSelected;
                BuzzLB.UserSelected += BuzzLB_UserSelected;
                BuzzLB.NavigateToFacebook += BuzzLB_NavigateToFacebook;
                BuzzLB.NavigateToTwitter += BuzzLB_NavigateToTwitter;
                BuzzLB.TagSelected += BuzzLB_TagSelected;
                BuzzLB.PeopleSelected += BuzzLB_PeopleSelected;
                UserList.UserSelected += UserList_UserSelected;
                BuzzLB.Height = 453;
                BuzzLB.BuzzLB.Height = 453;
                UserList.Height = 463;
                UserList.UserLB.Height = 463;
                MainPivot.SelectionChanged += MainPivot_SelectionChanged;

                if (SelectedCity == null)
                {
                    SelectedCity = Core.Location.Location.SelectedCity;
                }

                LocationLBL.Text = SelectedCity.Name + ", " + SelectedCity.Country.Name;
                BuzzLB.BuzzLB.DataVirtualizationMode = Telerik.Windows.Controls.DataVirtualizationMode.OnDemandAutomatic;
                UserList.UserLB.DataVirtualizationMode = Telerik.Windows.Controls.DataVirtualizationMode.OnDemandAutomatic;
                BuzzLB.MoreDataRequested += BuzzLB_MoreDataRequested;
                UserList.DataRequested += UserList_DataRequested;
            }
            catch { }
        }

        void MainPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (MainPivot.SelectedIndex == 0)
                {
                    if (PeopleSearchText == SearchTB.Text.Trim() && BuzzSearchText != SearchTB.Text.Trim())
                    {
                        BuzzSearchText = SearchTB.Text.Trim();
                        SearchBuzz(BuzzSearchText);
                    }


                }


                if (MainPivot.SelectedIndex == 1)
                {
                    if (BuzzSearchText == SearchTB.Text.Trim() && PeopleSearchText != SearchTB.Text.Trim())
                    {
                        PeopleSearchText = SearchTB.Text.Trim();
                        SearchPeople(BuzzSearchText);
                    }


                }
            }
            catch { }
        }

        void UserList_DataRequested()
        {
            try
            {
                SearchPeople(SearchTB.Text.Trim());
            }
            catch { }
        }

        void BuzzLB_MoreDataRequested()
        {
            try
            {
                SearchBuzz(SearchTB.Text.Trim());
            }
            catch { }
        }


        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                if (SearchTB.Text.Count() > 0)
                {
                    TypeSearchLBL.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    TypeSearchLBL.Visibility = System.Windows.Visibility.Visible;
                }
            }
            catch { }


        }

        private void SearchTB_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    if (SearchTB.Text.Trim().Count() >= 3)
                    {
                        //update comment. 

                        if (IsHashtag())
                        {
                            NavigateToTagPage(SearchTB.Text.Trim());
                            return;
                        }

                        TopLoaderVisible();


                        if (MainPivot.SelectedIndex == 0)
                        {
                            SearchBuzz(SearchTB.Text.Trim());
                        }
                        if (MainPivot.SelectedIndex == 1)
                        {
                            SearchPeople(SearchTB.Text.Trim());
                        }

                    }
                    else
                    {
                        MessageBox.Show("Please enter three or more characters", "Too Short", MessageBoxButton.OK);
                    }
                }
            }
            catch { }
        }

        private void NavigateToTagPage(string p)
        {
            try
            {
                p = p.TrimStart('#');
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri(Navigation.Tag + "?name=" + p, UriKind.RelativeOrAbsolute));
                });
            }
            catch { }
        }

        private bool IsHashtag()
        {
            try
            {
                if (SearchTB.Text.Trim().Split(' ').Count() == 1 && SearchTB.Text.Trim().StartsWith("#"))
                {
                    return true;
                }

                return false;
            }
            catch { return false; }
        }

        private void SearchPeople(string p)
        {
            try
            {
                if (p != PeopleSearchQuery)
                {
                    UserList.Users = new System.Collections.ObjectModel.ObservableCollection<UserService.User>();
                    PeopleSearchQuery = p;
                    LoadPeople(p);
                }
                else
                {

                    PeopleSearchQuery = p;
                    LoadPeople(p);
                }
            }
            catch { }

        }

        private void LoadPeople(string p)
        {
            try
            {
                UserList.DataRequested -= UserList_DataRequested;
                System.Collections.ObjectModel.ObservableCollection<Guid> skipList = new System.Collections.ObjectModel.ObservableCollection<Guid>();

                if (UserList.Users != null)
                {
                    if (UserList.Users.Count > 0)
                    {
                        TopLoaderVisible();
                        skipList = new System.Collections.ObjectModel.ObservableCollection<Guid>(UserList.Users.Select(o => o.UserID));
                    }
                }

                if (skipList.Count == 0)
                {
                    ShowPeopleLoading.Begin();
                }

                if (Core.Location.Location.SelectedCity != null && Core.Location.Location.SelectedCity != Core.Location.CurrentLocation.Location.City)
                {
                    Services.UserServiceClient.SearchUsersCompleted += UserServiceClient_SearchUsersCompleted;
                    Services.UserServiceClient.SearchUsersAsync(Core.User.User.UserID, p, 10, skipList, 0.0, 0.0, Core.Location.Location.SelectedCity.Id, Core.User.User.ZAT);
                }
                else
                {
                    Services.UserServiceClient.SearchUsersCompleted += UserServiceClient_SearchUsersCompleted;
                    Services.UserServiceClient.SearchUsersAsync(Core.User.User.UserID, p, 10, skipList, Core.Location.CurrentLocation.Location.Latitude, Core.Location.CurrentLocation.Location.Longitude, Core.Location.Location.SelectedCity.Id, Core.User.User.ZAT);
                }
            }
            catch { }
        }

        void UserServiceClient_SearchUsersCompleted(object sender, UserService.SearchUsersCompletedEventArgs e)
        {
            try
            {
                TopLoaderCollapse();

                Services.UserServiceClient.SearchUsersCompleted -= UserServiceClient_SearchUsersCompleted;

                if (e.Error == null)
                {
                    if (e.Result != null)
                    {
                        if (UserList.Users != null)
                        {

                            foreach (UserService.User x in e.Result)
                            {
                                if (UserList.Users.Where(o => o.UserID == x.UserID).Count() == 0)
                                {
                                    UserList.Users.Add(x);
                                }


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
                            ShowPeople.Begin();
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
                    ShowPeopleError.Begin();
                }


                if (e.Result != null)
                {
                    if (e.Result.Count == 10)
                    {
                        UserList.DataRequested += UserList_DataRequested;
                    }
                }
            }
            catch { }


        }

        private void TopLoaderCollapse()
        {
            try
            {
                TopLoader.Visibility = System.Windows.Visibility.Collapsed;
                TopPB.Visibility = System.Windows.Visibility.Collapsed;
            }
            catch { }
        }

        private void SearchBuzz(string p)
        {
            try
            {
                if (p != BuzzSearchQuery)
                {
                    BuzzLB.Buzzs = new System.Collections.ObjectModel.ObservableCollection<BuzzService.Buzz>();
                    BuzzSearchQuery = p;
                    LoadBuzz(p);
                }
                else
                {

                    BuzzSearchQuery = p;
                    LoadBuzz(p);
                }
            }
            catch { }
        }

        private void LoadBuzz(string p)
        {
            try
            {
                BuzzLB.MoreDataRequested -= BuzzLB_MoreDataRequested;
                System.Collections.ObjectModel.ObservableCollection<Guid> skipList = new System.Collections.ObjectModel.ObservableCollection<Guid>();

                if (BuzzLB.Buzzs != null)
                {
                    if (BuzzLB.Buzzs.Count > 0)
                    {
                        TopLoaderVisible();
                        skipList = new System.Collections.ObjectModel.ObservableCollection<Guid>(BuzzLB.Buzzs.Select(o => o.BuzzID));
                    }
                }

                if (skipList.Count == 0)
                {
                    ShowBuzzLoading.Begin();
                }

                if (Core.Location.Location.SelectedCity != null && Core.Location.Location.SelectedCity != Core.Location.CurrentLocation.Location.City)
                {
                    Services.BuzzServiceClient.SearchBuzzCompleted += BuzzServiceClient_SearchBuzzCompleted;
                    Services.BuzzServiceClient.SearchBuzzAsync(Core.User.User.UserID, p, 10, skipList, 0.0, 0.0, Core.Location.Location.SelectedCity.Id, Core.User.User.ZAT);
                }
                else
                {
                    Services.BuzzServiceClient.SearchBuzzCompleted += BuzzServiceClient_SearchBuzzCompleted;
                    Services.BuzzServiceClient.SearchBuzzAsync(Core.User.User.UserID, p, 10, skipList, Core.Location.CurrentLocation.Location.Latitude, Core.Location.CurrentLocation.Location.Longitude, Core.Location.Location.SelectedCity.Id, Core.User.User.ZAT);
                }
            }
            catch { }
        }

        void BuzzServiceClient_SearchBuzzCompleted(object sender, BuzzService.SearchBuzzCompletedEventArgs e)
        {
            try
            {
                TopLoaderCollapse();

                Services.BuzzServiceClient.SearchBuzzCompleted -= BuzzServiceClient_SearchBuzzCompleted;

                if (e.Error == null)
                {
                    if (e.Result != null)
                    {
                        if (BuzzLB.Buzzs != null)
                        {
                            foreach (BuzzService.Buzz x in e.Result)
                            {
                                BuzzLB.Buzzs.Add(x);
                            }
                        }
                        else
                        {
                            BuzzLB.Buzzs = e.Result;
                        }

                        if (BuzzLB.Buzzs == null || BuzzLB.Buzzs.Count == 0)
                        {
                            ShowNoBuzz.Begin();
                        }
                        else
                        {
                            ShowBuzzLB.Begin();
                        }

                    }
                    else
                    {
                        if (BuzzLB.Buzzs == null || BuzzLB.Buzzs.Count == 0)
                        {
                            ShowNoBuzz.Begin();
                        }
                    }

                }
                else
                {
                    //show error LBL
                    ShowBuzzError.Begin();
                }


                if (e.Result != null)
                {
                    if (e.Result.Count == 10)
                    {
                        BuzzLB.MoreDataRequested += BuzzLB_MoreDataRequested;
                    }
                }

                BuzzLB.BuzzLoadingProgressBarCollapse();
            }
            catch { }
        }

        public string PeopleSearchQuery { get; set; }

        public string BuzzSearchQuery { get; set; }

        private void TopLoaderVisible()
        {
            try
            {
                TopPB.Visibility = System.Windows.Visibility.Visible;
            }
            catch { }
        }

        private void TypeSearchLBL_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                SearchTB.Focus();
            }
            catch { }
        }



        public UserLocationService.City SelectedCity { get; set; }
    }
}