using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Hangout.Client.Tag
{
    public partial class Tag : PhoneApplicationPage
    {
        public Tag()
        {
            InitializeComponent();
            MainPivot.SelectionChanged += MainPivot_SelectionChanged;
        }

        void MainPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           if(MainPivot.SelectedIndex==1)
           {
               if(!PeopleLoaded)
               {
                   PeopleLoaded = true;
                   LoadPeople();
               }
           }
        }

        private void LoadPeople()
        {
            if (MainPivot.SelectedIndex == 1)
            {
                if (TagObject != null)
                {
                    TagUserList.DataRequested -= TagUserList_DataRequested;
                    System.Collections.ObjectModel.ObservableCollection<Guid> skipList = new System.Collections.ObjectModel.ObservableCollection<Guid>();

                    if (TagUserList.Users != null)
                    {
                        if (TagUserList.Users.Count > 0)
                        {
                            PeoplePB.Visibility = System.Windows.Visibility.Visible;
                            skipList = new System.Collections.ObjectModel.ObservableCollection<Guid>(TagUserList.Users.Select(o => o.UserID));
                        }
                        else
                        {
                            PeoplePB.Visibility = System.Windows.Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        PeoplePB.Visibility = System.Windows.Visibility.Collapsed;
                    }

                    if (skipList.Count == 0)
                    {
                        ShowPeopleLoading.Begin();
                    }

                    if (SelectedCity != null)
                    {
                        Services.UserServiceClient.GetLocalFollowersByTagCompleted += UserServiceClient_GetLocalFollowersByTagCompleted;
                        Services.UserServiceClient.GetLocalFollowersByTagAsync(Core.User.User.UserID, TagObject.Tag.Id, Core.Location.Location.SelectedCity.Id, 10, skipList, Core.User.User.ZAT);
                    }
                    else
                    {
                        ShowPeopleError.Begin();
                    }
                }
            }
        }

        void UserServiceClient_GetLocalFollowersByTagCompleted(object sender, UserService.GetLocalFollowersByTagCompletedEventArgs e)
        {

            PeoplePB.Visibility = System.Windows.Visibility.Collapsed;

            Services.UserServiceClient.GetLocalFollowersByTagCompleted -= UserServiceClient_GetLocalFollowersByTagCompleted;

            if (e.Error == null)
            {
                if (e.Result != null)
                {
                    if (TagUserList.Users != null)
                    {
                        foreach (UserService.User x in e.Result)
                        {
                            if (TagUserList.Users.Where(o=>o.UserID==x.UserID).Count()==0)
                            {
                                TagUserList.Users.Add(x);
                            }
                        }
                    }
                    else
                    {
                        TagUserList.Users = e.Result;
                    }

                    if (TagUserList.Users == null || TagUserList.Users.Count == 0)
                    {
                        ShowNoPeople.Begin();
                    }
                    else
                    {
                        ShowPeoplePage.Begin();
                    }

                }
                else
                {
                    if (TagUserList.Users == null || TagUserList.Users.Count == 0)
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
                    TagUserList.DataRequested += TagUserList_DataRequested;
                }
            }

            TagUserList.BuzzLoadingProgressBarCollapse();
        }

        void TagUserList_DataRequested()
        {
            LoadPeople();
        }

        private void Followers_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
           
        }

        private void FollowBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (this.TagObject != null)
            {

                if (!this.TagObject.Following)
                {
                    this.TagObject.Following = true;
                    ShowUnfollow.Begin();
                    Services.TagServiceClient.FollowTagCompleted += TagServiceClient_FollowTagCompleted;
                    Services.TagServiceClient.FollowTagAsync(Core.User.User.UserID, TagObject.Tag.Id, Core.Location.CurrentLocation.Location.City.Id, Core.User.User.ZAT);
                    this.TagObject.NoOfLocalPeopleFollowing++;
                    //update lbl
                    Followers.Text = this.TagObject.NoOfLocalPeopleFollowing + " Followers";

                    if(TagUserList.Users.Count==0)
                    {
                        LoadPeople();
                    }
                }

            }
        }

        void TagServiceClient_FollowTagCompleted(object sender, TagService.FollowTagCompletedEventArgs e)
        {
            Services.TagServiceClient.FollowTagCompleted -= TagServiceClient_FollowTagCompleted;

            if (e.Error != null)
            {
                TagObject.Following = false;
                ShowFollow.Begin();
                MessageBox.Show(Messages.CantFollow, Messages.Sorry, MessageBoxButton.OK);
            }
        }

        private void UnFollowBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (this.TagObject != null)
            {

                if (this.TagObject.Following)
                {
                    this.TagObject.Following = false;
                    ShowFollow.Begin();
                    Services.TagServiceClient.UnfollowTagCompleted += TagServiceClient_UnfollowTagCompleted;
                    Services.TagServiceClient.UnfollowTagAsync(Core.User.User.UserID, TagObject.Tag.Id, Core.Location.CurrentLocation.Location.City.Id, Core.User.User.ZAT);

                    if (this.TagObject.NoOfLocalPeopleFollowing > 0)
                    {
                        this.TagObject.NoOfLocalPeopleFollowing--;
                        //update lbl
                        Followers.Text = this.TagObject.NoOfLocalPeopleFollowing + " Followers";
                    }

                    if(this.TagUserList.Users.Where(o=>o.UserID==Core.User.User.UserID).Count()>0 )
                    {
                        //remove me and update. 
                        UserService.User user = TagUserList.Users.Where(o => o.UserID == Core.User.User.UserID).First();
                        //remove. 
                        TagUserList.Users.Remove(user);

                        if(TagUserList.Users.Count==0)
                        {
                            ShowNoPeople.Begin();
                        }
                    }
                }
                
            }
        }

        void TagServiceClient_UnfollowTagCompleted(object sender, TagService.UnfollowTagCompletedEventArgs e)
        {
            Services.TagServiceClient.UnfollowTagCompleted -= TagServiceClient_UnfollowTagCompleted;

            if (e.Error != null)
            {
                TagObject.Following= true;
                ShowUnfollow.Begin();
                MessageBox.Show(Messages.CantUnfollow, Messages.Sorry, MessageBoxButton.OK);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (SelectedCity != null)
            {
                if (SelectedCity.Id == Core.Location.Location.SelectedCity.Id)
                {
                    //do nothign.
                    return;
                }
            }

            BuzzLB.Buzzs = new System.Collections.ObjectModel.ObservableCollection<BuzzService.Buzz>();
            TagUserList.Users = new System.Collections.ObjectModel.ObservableCollection<UserService.User>();
            ShowTagLoading.Begin();
            ShowBuzzLoading.Begin();
            ShowPeopleLoading.Begin();

            

            if (Core.Location.Location.SelectedCity != null)
            {
                SelectedCity = Core.Location.Location.SelectedCity;
            }
            else
            {
                SelectedCity = Core.Location.CurrentLocation.Location.City;
            }

            var id = "";

            if (NavigationContext.QueryString.ContainsKey("name"))
            {
                id = NavigationContext.QueryString["name"];
                TagNameLBL.Text = "#" + id;
            }
            else
            {
                NavigateToDashboard();
            }



            Services.TagServiceClient.GetTagByNameCompleted += TagServiceClient_GetTagByNameCompleted;
            Services.TagServiceClient.GetTagByNameAsync(Core.User.User.UserID, id, SelectedCity.Id, Core.User.User.ZAT);

        }


        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {

            BuzzLB.BuzzLB.IsPullToRefreshEnabled = true;
            BuzzLB.BuzzLB.DataVirtualizationMode = Telerik.Windows.Controls.DataVirtualizationMode.OnDemandAutomatic;
            BuzzLB.Height = 481;
            BuzzLB.BuzzLB.Height = 481;
            TagUserList.Height = 490;
            TagUserList.UserLB.Height = 490;

            //get name
           
            //get people who like this buzz

            AttachEvents();

           
            
        }

        private void AttachEvents()
        {
            BuzzLB.CommentOnBuzzSelected += BuzzLB_CommentOnBuzzSelected;
            BuzzLB.UserSelected += BuzzLB_UserSelected;
            BuzzLB.NavigateToFacebook += BuzzLB_NavigateToFacebook;
            BuzzLB.NavigateToTwitter += BuzzLB_NavigateToTwitter;
            BuzzLB.TagSelected += BuzzLB_TagSelected;
            BuzzLB.PeopleSelected += BuzzLB_PeopleSelected;
            BuzzLB.PullToRefreshEvent += BuzzLB_PullToRefreshEvent;
            BuzzLB.MoreDataRequested += BuzzLB_MoreDataRequested;

            TagUserList.UserSelected += UserList_UserSelected;
            TagUserList.DataRequested += UserList_DataRequested;
        }


        void UserList_DataRequested()
        {
            
            LoadPeople();
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

        void BuzzLB_PullToRefreshEvent()
        {
            LoadTagBuzz();
        }

        void BuzzLB_PeopleSelected(string text, Guid id)
        {
            NavigateToPeopleList(text, id);
        }

        private void NavigateToPeopleList(string text, Guid id)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.PeopleList + "?text=" + text + "&id=" + id.ToString(), UriKind.RelativeOrAbsolute));
            });
        }

        void BuzzLB_TagSelected(string name)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Tag + "?name=" + name, UriKind.RelativeOrAbsolute));
            });
        }

        void BuzzLB_NavigateToTwitter()
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.TwitterConnect, UriKind.RelativeOrAbsolute));
            });
        }

        void BuzzLB_NavigateToFacebook()
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.FacebookConnect, UriKind.RelativeOrAbsolute));
            });
        }

        
        void BuzzLB_UserSelected(Guid id)
        {
            NavigateToUserProfile(id);
        }

        private void NavigateToUserProfile(Guid id)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.UserProfile + "?id=" + id.ToString(), UriKind.RelativeOrAbsolute));
            });
        }

        void BuzzLB_CommentOnBuzzSelected(Guid id)
        {
            NavigateToBuzzCommentPage(id);
        }

        private void NavigateToBuzzCommentPage(Guid id)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.BuzzComments + "?id=" + id.ToString(), UriKind.RelativeOrAbsolute));
            });
        }

        void TagServiceClient_GetTagByNameCompleted(object sender, TagService.GetTagByNameCompletedEventArgs e)
        {
            Services.TagServiceClient.GetTagByNameCompleted -= TagServiceClient_GetTagByNameCompleted;

            if (e.Error == null)
            {
                if (e.Result != null)
                {

                    

                    TagObject = e.Result;

                    if (MainPivot.SelectedIndex == 0)
                    {
                        LoadTagBuzz();
                    }
                    else
                    {
                        LoadPeople();
                    }
                    

                   
                        if (e.Result.Following)
                        {
                            ShowUnfollow.Begin();
                        }
                        else
                        {
                            ShowFollow.Begin();
                        }
                   

                   

                    if (SelectedCity == null)
                    {
                        LocationSP.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    else
                    {
                        LocationLBL.Text = SelectedCity.Name + ", " + SelectedCity.Country.Name;
                    }

                    
                    Followers.Text = e.Result.NoOfLocalPeopleFollowing+ " Followers";


                    ShowTagPage.Begin();

                }
                else
                {
                    ShowTagError.Begin();
                }
            }
            else
            {
                ShowTagError.Begin();
            }
        }

        private void LoadTagBuzz()
        {
            BuzzLB.MoreDataRequested -= BuzzLB_MoreDataRequested;
            System.Collections.ObjectModel.ObservableCollection<Guid> skipList = new System.Collections.ObjectModel.ObservableCollection<Guid>();

            if (BuzzLB.Buzzs != null)
            {
                if (BuzzLB.Buzzs.Count > 0)
                {
                    BuzzPB.Visibility = System.Windows.Visibility.Visible;
                    skipList = new System.Collections.ObjectModel.ObservableCollection<Guid>(BuzzLB.Buzzs.Select(o => o.BuzzID));
                }
            }

            if (skipList.Count == 0)
            {
                ShowBuzzLoading.Begin();
            }

            if(SelectedCity!=null)
            {
                Services.BuzzServiceClient.GetLocalBuzzByTagCompleted += BuzzServiceClient_GetLocalBuzzByTagCompleted;
                Services.BuzzServiceClient.GetLocalBuzzByTagAsync(Core.User.User.UserID, TagObject.Tag.Id, Core.Location.Location.SelectedCity.Id, 10, skipList, Core.User.User.ZAT);
            }
            else
            {
                ShowBuzzError.Begin();
            }
        }

        void BuzzLB_MoreDataRequested()
        {
           
            LoadTagBuzz();
        }

        void BuzzServiceClient_GetLocalBuzzByTagCompleted(object sender, BuzzService.GetLocalBuzzByTagCompletedEventArgs e)
        {
            BuzzPB.Visibility = System.Windows.Visibility.Collapsed;

            Services.BuzzServiceClient.GetLocalBuzzByTagCompleted -= BuzzServiceClient_GetLocalBuzzByTagCompleted;

            if (e.Error == null)
            {
                if (e.Result != null)
                {
                    if (BuzzLB.Buzzs != null)
                    {
                        foreach (BuzzService.Buzz x in e.Result)
                        {
                            if (BuzzLB.Buzzs.Count > 0)
                            {
                                if(BuzzLB.Buzzs.First().Posted<x.Posted)
                                {
                                    BuzzLB.Buzzs.Insert(0, x);
                                }
                                else
                                {
                                    //add it to last/ 
                                    BuzzLB.Buzzs.Add(x);

                                }
                            }
                            else
                            {
                                BuzzLB.Buzzs.Add(x);
                            }
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
                        ShowBuzzPage.Begin();
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
                    BuzzLB.MoreDataRequested +=BuzzLB_MoreDataRequested;
                }
            }

            BuzzLB.BuzzLoadingProgressBarCollapse();
            BuzzLB.StopPullToRefreshLoading();
        }

       

       
            
       

        private void NavigateToDashboard()
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.Dashboard, UriKind.RelativeOrAbsolute));
            });
        }

        public bool PeopleLoaded { get; set; }

        public UserLocationService.City SelectedCity { get; set; }

        public TagService.UserTag TagObject { get; set; }

        private void StackPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //navigate to people pane
            MainPivot.SelectedIndex = 1;
        }

        private void LocationSP_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.LocationPicker, UriKind.RelativeOrAbsolute));
            });
        }
    }
}