using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;

namespace Hangout.Client.Controls.User
{
    public partial class User : UserControl
    {
        public User()
        {
            InitializeComponent();
            this.Loaded += User_Loaded;
        }

        void User_Loaded()
        {
            Header.PeopleSelected += Header_PeopleSelected;
            Header.ProfileImageSelected += Header_ProfileImageSelected;
            Header.MessageSelected += Header_MessageSelected;
           
        }

        void Header_MessageSelected(Guid id)
        {
            if(MessageSelected!=null)
            {
                MessageSelected(id);
            }
        }

        void Header_ProfileImageSelected(Guid id)
        {
            if(ProfileImageSelected!=null)
            {
                ProfileImageSelected(id);
            }
        }

        void Header_PeopleSelected(string name, Guid id)
        {
            if(PeopleSelected!=null)
            {
                PeopleSelected(name, id);
            }
        }


        public delegate void EventHelper();
        public delegate void PeopleHelper(string name,Guid id);
        public delegate void IdHelper( Guid id);
        public event IdHelper ProfileImageSelected;
        public event EventHelper Error;
        public event IdHelper MessageSelected;
        public event PeopleHelper PeopleSelected; 
        public event EventHelper Loaded;
        public event EventHelper Loading;
        public event EventHelper BuzzLoaded;
        public event EventHelper BuzzLoading;
        public event EventHelper BuzzLoadEnd;
       

        public void LoadUser(Guid userId)
        {
            
            if(userId!=null&&userId!=new Guid())
            {
                if(Loading!=null)
                {
                    Loading();
                }
                UserID = userId;
                //load user
                Services.UserServiceClient.GetUserCompleteProfileCompleted += UserServiceClient_GetUserCompleteProfileCompleted;
                Services.UserServiceClient.GetUserCompleteProfileAsync(Core.User.User.UserID, userId, Core.User.User.ZAT);
                //load buzz
                LoadBuzz();
            }

            else
            {
                if (Error != null)
                {
                    Error();
                }
            }

            
            
        }

       public void LoadBuzz()
        {

            if(BuzzLoading!=null)
            {
                BuzzLoading();
            }

            

            MyBuzzLB.MoreDataRequested -= MyBuzzLB_MoreDataRequested;

            System.Collections.ObjectModel.ObservableCollection<Guid> skipList = new System.Collections.ObjectModel.ObservableCollection<Guid>();

            if (MyBuzzLB.Buzzs != null)
            {
                if (MyBuzzLB.Buzzs.Count > 0)
                {
                    skipList = new System.Collections.ObjectModel.ObservableCollection<Guid>(MyBuzzLB.Buzzs.Select(o => o.BuzzID));
                }
            }

            if (skipList.Count == 0)
            {
               
            }

           
            Services.BuzzServiceClient.GetUserBuzzCompleted += BuzzServiceClient_GetUserBuzzCompleted;
            Services.BuzzServiceClient.GetUserBuzzAsync(UserID, 10, skipList, Core.User.User.ZAT);
        }

        void MyBuzzLB_MoreDataRequested()
        {
            LoadBuzz();
        }

        void BuzzServiceClient_GetUserBuzzCompleted(object sender, BuzzService.GetUserBuzzCompletedEventArgs e)
        {



            try
            {


                Services.BuzzServiceClient.GetUserBuzzCompleted -= BuzzServiceClient_GetUserBuzzCompleted;


                if (e.Error == null)
                {
                    if (e.Result != null)
                    {


                        if (MyBuzzLB.Buzzs != null)
                        {
                            foreach (BuzzService.Buzz x in e.Result)
                            {
                                MyBuzzLB.Buzzs.Add(x);
                            }
                        }
                        else
                        {
                            MyBuzzLB.Buzzs = e.Result;
                        }

                        if (MyBuzzLB.Buzzs == null || MyBuzzLB.Buzzs.Count == 0)
                        {
                            ShowNoBuzz.Begin();
                        }
                        else
                        {
                            ShowBuzzPage.Begin();
                        }

                        if (e.Result.Count() == 10)
                        {
                            if (BuzzLoadEnd != null)
                            {
                                BuzzLoadEnd();
                            }
                        }

                    }
                    else
                    {
                        if (MyBuzzLB.Buzzs == null || MyBuzzLB.Buzzs.Count == 0)
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
                    if (e.Result.Count < 10)
                    {
                        if (BuzzLoadEnd != null)
                        {
                            BuzzLoadEnd();
                        }
                    }
                }

                if (BuzzLoaded != null)
                {
                    BuzzLoaded();
                }

                MyBuzzLB.BuzzLB.Height = double.NaN;
                MyBuzzLB.BuzzLB.UpdateLayout();
                MyBuzzLB.Height = double.NaN;
                MyBuzzLB.UpdateLayout();
            }
            catch { }
            

           
        }

        
        

        void UserServiceClient_GetUserCompleteProfileCompleted(object sender, UserService.GetUserCompleteProfileCompletedEventArgs e)
        {
            Services.UserServiceClient.GetUserCompleteProfileCompleted += UserServiceClient_GetUserCompleteProfileCompleted;

            if(e.Error==null)
            {
                if(e.Result!=null)
                {
                    user = e.Result;
                    UserID = e.Result.UserID;
                    Header.Profile = e.Result;
                   
                    if(Loaded!=null)
                    {
                        Loaded();
                    }

                }
                else
                {
                    if (Error != null)
                    {
                        Error();
                    }
                }
            }
            else
            {
                if(Error!=null)
                {
                    Error();
                }
            }
        }

       

        public Guid UserID { get; set; }



        public UserService.CompleteUserProfile user { get; set; }

      

        

      

        

       
    }
}
