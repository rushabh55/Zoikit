using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Documents;
using System.Windows.Media;

namespace Hangout.Client.Controls.Buzz
{
    public partial class BuzzControl : UserControl
    {


        public BuzzControl()
        {
            InitializeComponent();
        }


        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            AttachEventHandlers();
        }

        public delegate void IdHelper(int id);

        public event IdHelper UserSelectedEvent;

        public event IdHelper BuzzSelectedEvent;

        private UserLocationService.City city;

        private VenueService.Venue venue;

        private int? categoryId;

        public int? CategoryId 
        {
            get
            {
                return categoryId;
            }
            set
            {
                categoryId = value;
                if (value != null)
                {
                    BuzzInput.CategoryId = value;
                }
            }
        }

        public void DisableLocationVenueSelector()
        {
            LocationSP.Visibility = System.Windows.Visibility.Collapsed;
            LocationDisabled = true;
        }

        
        public event EventHandler ChangeLocationTapped;

        public UserLocationService.City City
        {
            get
            {
                return city;
            }

            set
            {
                city = value;
                BuzzLB.Buzzes = null;
                BuzzLB.HideDownPanel();
                if (value != null)
                {
                    
                    Venue = null;
                    BuzzInput.SelectedCity = City;
                    GetBuzz();

                }

            }
        }

        public VenueService.Venue Venue
        {
            get
            {
                return venue;
            }

            set
            {
                venue = value;
                BuzzLB.Buzzes = null;
                BuzzLB.HideDownPanel();
                if (value != null)

                {
                    BuzzInput.SelectedVenue = Venue;
                    City = null;
                    GetBuzz();

                }
                
            }


        }

        public event EventHandler StartBuzzLoading;

        private void GetBuzz()
        {
            if (StartBuzzLoading != null)
            {
                StartBuzzLoading(null, new EventArgs());
            }
            
            if(Venue==null&&city==null)
            {
                return;
            }

            Services.BuzzServiceClient.GetBuzzCompleted+=BuzzServiceClient_GetBuzzCompleted;
            if (Venue != null)
            {
                FillVenueLBL();
                Services.BuzzServiceClient.GetBuzzAsync(Core.User.User.UserID, 10, new System.Collections.ObjectModel.ObservableCollection<int>(), venue.VenueID, null, CategoryId, Core.User.User.ZAT);

            }
            else
            {
                FillCityLBL();
                Services.BuzzServiceClient.GetBuzzAsync(Core.User.User.UserID,10,new System.Collections.ObjectModel.ObservableCollection<int>(),null, City.Id,CategoryId, Core.User.User.ZAT);
            }
            
        }

        private void FillCityLBL()
        {
            if (!LocationDisabled)
            {
                LocationLBL.Text = "";
                LocationLBL.Inlines.Add(new Run() { Text = City.Name + ", " + City.Country.Name, FontWeight = FontWeights.Bold, TextDecorations = TextDecorations.Underline, Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 175, 4)) });
                LocationLBL.Visibility = System.Windows.Visibility.Visible;
                LocationLBL.Opacity = 1;
                LocationLBL.Text = LocationLBL.Text.ToUpper();
                LocationSP.Visibility = System.Windows.Visibility.Visible;
            }

            
        }

        private void FillVenueLBL()
        {
            if (!LocationDisabled)
            {
                LocationLBL.Text = "";
                LocationLBL.Inlines.Add(new Run() { Text = Venue.Name, FontWeight = FontWeights.Bold, TextDecorations = TextDecorations.Underline, Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 175, 4)) });
                LocationLBL.Visibility = System.Windows.Visibility.Visible;
                LocationLBL.Opacity = 1;
                LocationLBL.Text = LocationLBL.Text.ToUpper();
                LocationSP.Visibility = System.Windows.Visibility.Visible;
            }
        }


        private void AttachEventHandlers()
        {
            BuzzLB.MorebuttonTapped += BuzzLB_MorebuttonTapped;
            BuzzInput.BuzzSaved += BuzzInput_BuzzSaved;
            BuzzInput.SavingBuzz += BuzzInput_SavingBuzz;
            BuzzLB.BuzzSelected += BuzzLB_BuzzSelected;
            BuzzLB.UserSelected += BuzzLB_UserSelected;
            BuzzInput.Canceled += BuzzInput_Canceled;
           
        }

        public event EventHandler Canceled;

        void BuzzInput_Canceled(object sender, EventArgs e)
        {
            if (Canceled != null)
            {
                Canceled(null, new EventArgs());
            }
        }

       

        void BuzzLB_UserSelected(int id)
        {
            if (UserSelectedEvent != null)
            {
                UserSelectedEvent(id);
            }
        }

        void BuzzLB_BuzzSelected(int id)
        {
            if (BuzzSelectedEvent != null)
            {
                BuzzSelectedEvent(id);
            }
        }

        void BuzzLB_MorebuttonTapped(object sender, EventArgs e)
        {
            BuzzLB.ShowProgressBar();
            if (BuzzLB.Buzzes != null)
            {
                if (!String.IsNullOrWhiteSpace(SearchText) && SearchTB.Text.Trim().Count() >= 3)
                {
                    if (BuzzLB.Buzzes.Count > 0)
                    {
                        if (Venue == null && city == null)
                        {
                            return;
                        }

                        Services.BuzzServiceClient.SearchBuzzCompleted+=BuzzServiceClient_SearchBuzzCompleted;

                        if (Venue != null)
                        {
                            Services.BuzzServiceClient.SearchBuzzAsync(Core.User.User.UserID, SearchText,10, new System.Collections.ObjectModel.ObservableCollection<int>(BuzzLB.Buzzes.Select(x => x.BuzzID).ToList()), Venue.VenueID, null, CategoryId, Core.User.User.ZAT);

                        }
                        else
                        {
                            Services.BuzzServiceClient.SearchBuzzAsync(Core.User.User.UserID,SearchText, 10, new System.Collections.ObjectModel.ObservableCollection<int>(BuzzLB.Buzzes.Select(x => x.BuzzID).ToList()), null, City.Id, CategoryId, Core.User.User.ZAT);
                        }



                    }
                }
                else
                {
                    if (BuzzLB.Buzzes.Count > 0)
                    {
                        if (Venue == null && city == null)
                        {
                            return;
                        }

                        Services.BuzzServiceClient.GetBuzzCompleted += BuzzServiceClient_GetBuzzCompleted;

                        if (Venue != null)
                        {

                            Services.BuzzServiceClient.GetBuzzAsync(Core.User.User.UserID, 10, new System.Collections.ObjectModel.ObservableCollection<int>(BuzzLB.Buzzes.Select(x => x.BuzzID).ToList()), Venue.VenueID, null, CategoryId, Core.User.User.ZAT);

                        }
                        else
                        {

                            Services.BuzzServiceClient.GetBuzzAsync(Core.User.User.UserID, 10, new System.Collections.ObjectModel.ObservableCollection<int>(BuzzLB.Buzzes.Select(x => x.BuzzID).ToList()), null, City.Id, CategoryId, Core.User.User.ZAT);
                        }



                    }
                }
               
            }
        }


        public event EventHandler SavingBuzz;

        void BuzzInput_SavingBuzz(object sender, EventArgs e)
        {
            SearchText = "";
            SearchSP.Visibility = System.Windows.Visibility.Collapsed;
            PG.Visibility = System.Windows.Visibility.Visible;
            if (SavingBuzz != null)
            {
                SavingBuzz(null, new EventArgs());
            }
        }

        void BuzzInput_BuzzSaved(BuzzService.HangoutSaveStatus status)
        {
            PG.Visibility = System.Windows.Visibility.Collapsed;

            if (status == BuzzService.HangoutSaveStatus.Saved)
            {
                RefreshBuzz();

            }

            if (status == BuzzService.HangoutSaveStatus.Error)
            {
                PG.Visibility = System.Windows.Visibility.Collapsed;
                MessageBox.Show("We cannot add your status at this point of time. Please try again later", "We're sorry", MessageBoxButton.OK);
            }

            if (status == BuzzService.HangoutSaveStatus.Spam)
            {
                PG.Visibility = System.Windows.Visibility.Collapsed;
                MessageBox.Show("Looks like you're spamming Zoik it!. We believe in more open and connected world. If you're spamming. Please stop.", "Stop spamming", MessageBoxButton.OK);
            }
        }

        public void RefreshBuzz()
        {
            PG.Visibility = System.Windows.Visibility.Visible;
            BuzzInput.Clear(Venue, City);
            BuzzInput.Visibility = System.Windows.Visibility.Collapsed;
            //get last buzz id
            int BuzzId = 0;
            if (BuzzLB.Buzzes != null)
            {
                if (BuzzLB.Buzzes.Count > 0)
                {
                    BuzzId = BuzzLB.Buzzes.OrderByDescending(o => o.Posted).FirstOrDefault().BuzzID;
                }
            }


            if (Venue == null && city == null)
            {
                PG.Visibility = System.Windows.Visibility.Collapsed;
                return;
            }

            Services.BuzzServiceClient.GetBuzzBeforeCompleted += BuzzServiceClient_GetBuzzBeforeCompleted;

            if (Venue != null)
            {

                Services.BuzzServiceClient.GetBuzzBeforeAsync(BuzzId, Core.User.User.UserID, Venue.VenueID, null, CategoryId, Core.User.User.ZAT);

            }
            else
            {

                Services.BuzzServiceClient.GetBuzzBeforeAsync(BuzzId, Core.User.User.UserID, null, City.Id, CategoryId, Core.User.User.ZAT);
            }
        }

        void BuzzServiceClient_GetBuzzBeforeCompleted(object sender, BuzzService.GetBuzzBeforeCompletedEventArgs e)
        {
            Services.BuzzServiceClient.GetBuzzBeforeCompleted -= BuzzServiceClient_GetBuzzBeforeCompleted;
            PG.Visibility = System.Windows.Visibility.Collapsed;
            if (e.Error == null)
            {
                if (e.Result != null)
                {
                    if (e.Result.Count > 0)
                    {
                        if (BuzzLB.Buzzes == null)
                        {
                            BuzzLB.Buzzes = new List<BuzzService.Buzz>();
                        }


                        BuzzLB.Buzzes.AddRange(e.Result.ToList());
                        BuzzLB.Buzzes = BuzzLB.Buzzes.OrderByDescending(o => o.Posted).ToList();
                        BuzzLB.Buzzes = BuzzLB.Buzzes.Distinct().ToList();
                        BuzzLB.RefreshList();

                        if (BuzzLB.Buzzes.Count > 0)
                        {
                            if (BuzzLoaded != null)
                            {
                                BuzzLoaded(null, new EventArgs());
                            }

                        }
                        else
                        {
                            if (NoBuzz != null)
                            {
                                NoBuzz(null, new EventArgs());
                            }
                        }


                    }
                }

            }
            else
            {
                if (BuzzLoadError != null)
                {
                    BuzzLoadError(null, new EventArgs());
                }
            }
        }

        void BuzzServiceClient_GetBuzzCompleted(object sender, BuzzService.GetBuzzCompletedEventArgs e)
        {
            PG.Visibility = System.Windows.Visibility.Collapsed;
            Services.BuzzServiceClient.GetBuzzCompleted -= BuzzServiceClient_GetBuzzCompleted;
            BuzzLB.Visibility = System.Windows.Visibility.Visible;
            if (e.Error == null)
            {
                if (e.Result == null)
                {
                    if (NoBuzz != null)
                    {
                        NoBuzz(null, new EventArgs());
                    }
                }
                else
                {
                    if (BuzzLB.Buzzes == null)
                    {
                        BuzzLB.Buzzes = new List<BuzzService.Buzz>();
                    }

                  
                    LocationLBL.Visibility = System.Windows.Visibility.Visible;
                    BuzzLB.Buzzes.AddRange(e.Result.ToList());
                    BuzzLB.Buzzes = BuzzLB.Buzzes.OrderByDescending(o => o.Posted).ToList();
                    BuzzLB.Buzzes = BuzzLB.Buzzes.Distinct().ToList();
                    BuzzLB.RefreshList();

                    if (BuzzLB.Buzzes.Count > 0)
                    {
                        if (BuzzLoaded != null)
                        {
                            BuzzLoaded(null, new EventArgs());
                        }

                    }
                    else
                    {
                        if (NoBuzz != null)
                        {
                            NoBuzz(null, new EventArgs());
                        }
                    }

                    if (e.Result.Count < 10)
                    {
                        BuzzLB.HideDownPanel();
                    }
                    else
                    {
                        BuzzLB.ShowMoreButton();
                    }

                    
                }
            }
            else
            {
                if (BuzzLoadError != null)
                {
                    BuzzLoadError(null, new EventArgs());
                }
            }
        }

        public event EventHandler BuzzLoaded;

        public event EventHandler BuzzLoadError;

        public event EventHandler NoBuzz;

        public void AddBuzz()
        {
            BuzzInput.Visibility = System.Windows.Visibility.Visible;
        }

       

        private void LocationLBL_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (ChangeLocationTapped != null)
            {
                ChangeLocationTapped(null, new EventArgs());
            }
        }




        public bool LocationDisabled { get; set; }

        private void SearchTB_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            
        }

        void BuzzServiceClient_SearchBuzzCompleted(object sender, BuzzService.SearchBuzzCompletedEventArgs e)
        {
            PG.Visibility = System.Windows.Visibility.Collapsed;
            Services.BuzzServiceClient.SearchBuzzCompleted-=BuzzServiceClient_SearchBuzzCompleted;
            BuzzLB.Visibility = System.Windows.Visibility.Visible;
            if (e.Error == null)
            {
                if (e.Result == null)
                {
                    if (NoBuzz != null)
                    {
                        NoBuzz(null, new EventArgs());
                    }
                }
                else
                {
                    if (BuzzLB.Buzzes == null)
                    {
                        BuzzLB.Buzzes = new List<BuzzService.Buzz>();
                    }


                    LocationLBL.Visibility = System.Windows.Visibility.Visible;
                    BuzzLB.Buzzes.AddRange(e.Result.ToList());
                    BuzzLB.Buzzes = BuzzLB.Buzzes.OrderByDescending(o => o.Posted).ToList();
                    BuzzLB.Buzzes = BuzzLB.Buzzes.Distinct().ToList();
                    BuzzLB.RefreshList();

                    if (BuzzLB.Buzzes.Count > 0)
                    {
                        if (BuzzLoaded != null)
                        {
                            BuzzLoaded(null, new EventArgs());
                        }

                    }
                    else
                    {
                        if (NoBuzz != null)
                        {
                            NoBuzz(null, new EventArgs());
                        }
                    }

                    if (e.Result.Count < 10)
                    {
                        BuzzLB.HideDownPanel();
                    }
                    else
                    {
                        BuzzLB.ShowMoreButton();
                    }


                }
            }
            else
            {
                if (BuzzLoadError != null)
                {
                    BuzzLoadError(null, new EventArgs());
                }
            }
        }

        private void SearchCancelBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            SearchSP.Visibility = System.Windows.Visibility.Collapsed;
            SearchTB.Text = "";
            SearchText = "";
            BuzzLB.Buzzes = new List<BuzzService.Buzz>();
            RefreshBuzz();
            
        }

        public string SearchText { get; set; }

        internal void HidesearchIfVisible()
        {
            if (SearchSP.Visibility == System.Windows.Visibility.Visible)
            {
                if (!String.IsNullOrWhiteSpace(SearchText))
                {
                    BuzzLB.Buzzes = new List<BuzzService.Buzz>();
                    BuzzLB.RefreshList();
                }
            }

            SearchText = "";
            SearchSP.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void SearchBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (SearchTB.Text.Trim().Count() >= 3)
            {
                //start searching...
                SearchText = SearchTB.Text.Trim();

                BuzzLB.Buzzes = new List<BuzzService.Buzz>();
                BuzzLB.RefreshList();

                if (StartBuzzLoading != null)
                {
                    StartBuzzLoading(null, new EventArgs());
                }

                if (Venue == null && city == null)
                {
                    return;
                }

                Services.BuzzServiceClient.SearchBuzzCompleted += BuzzServiceClient_SearchBuzzCompleted;
                if (Venue != null)
                {
                    FillVenueLBL();
                    Services.BuzzServiceClient.SearchBuzzAsync(Core.User.User.UserID, SearchText, 10, new System.Collections.ObjectModel.ObservableCollection<int>(), venue.VenueID, null, CategoryId, Core.User.User.ZAT);

                }
                else
                {
                    FillCityLBL();
                    Services.BuzzServiceClient.SearchBuzzAsync(Core.User.User.UserID, SearchText, 10, new System.Collections.ObjectModel.ObservableCollection<int>(), null, City.Id, CategoryId, Core.User.User.ZAT);
                }
            }
            else
            {
                MessageBox.Show("Add more characters to search.", "Search Text too short", MessageBoxButton.OK);
            }
        }
    }
}
