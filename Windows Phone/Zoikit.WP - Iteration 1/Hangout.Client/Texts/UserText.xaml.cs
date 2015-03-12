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
using System.Windows.Media.Imaging;

namespace Hangout.Client.Texts
{
    public partial class UserText : PhoneApplicationPage
    {
      

        public System.Collections.ObjectModel.ObservableCollection<TextService.Text> Texts { get; set; }

        public UserText()
        {
            InitializeComponent();
            Loaded += UserText_Loaded;
            
           
        }

        void TextLB_RefreshRequested(object sender, EventArgs e)
        {
           
        }

        

        private Guid GetID()
        {
            try
            {

                if(NavigationContext.QueryString.ContainsKey("id"))
                {
                    Id = new Guid(NavigationContext.QueryString["id"].ToString());
                    return Id;
                }
                else
                {
                    NavigateToDashboard();
                }

                return new Guid();
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                return new Guid();
            }
        }


        void PushNotification_RealTimeText(TextService.Text text)
        {
            if (text.User.UserID == ToUserID)
            {
                Texts.Add(text);
                if (Texts != null && Texts.Count > 0)
                {
                    TextLB.BringIntoView(Texts.Last());
                }
                

            }
        }

        void UserText_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                try
                {

                    Core.PushNotification.PushNotification.RealTimeText += PushNotification_RealTimeText;
                    TextLB.ScrollStateChanged += TextLB_ScrollStateChanged;


                    Texts = new System.Collections.ObjectModel.ObservableCollection<TextService.Text>();

                  

                    if (!Core.Network.NetworkStatus.IsNetworkAvailable())
                    {
                        ShowTextError.Begin();
                        return;
                    }


                    RefreshList();

                    Guid id = GetID();
                    ToUserID = id;
                    if (ToUserID == new Guid())
                    {
                        NavigateToDashboard();
                        return;
                    }
                    LoadUserText();

                }
                catch (Exception ex)
                {
                    Core.Exceptions.ExceptionReporting.Report(ex);
                    MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                }

            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }

        void TextLB_ScrollStateChanged(object sender, Telerik.Windows.Controls.ScrollStateChangedEventArgs e)
        {
            if(e.NewState==Telerik.Windows.Controls.ScrollState.TopStretch)
            {
                //reached top. 
                LoadUserText();
            }
        }

      

        private void LoadUserText()
        {

            System.Collections.ObjectModel.ObservableCollection<Guid> skipList = new System.Collections.ObjectModel.ObservableCollection<Guid>();



            if (Texts!= null)
            {
                if (Texts.Count > 0)
                {
                    LoadPrevTextPB.Visibility = System.Windows.Visibility.Visible;
                    skipList = new System.Collections.ObjectModel.ObservableCollection<Guid>(Texts.Select(o => o.TextId));
                }
                else
                {
                    ShowTextLoading.Begin();
                }
            }
            else
            {
                ShowTextLoading.Begin();

            }
           
           
            Services.TextServiceClient.GetUserTextCompleted += TextServiceClient_GetUserTextCompleted;
            Services.TextServiceClient.GetUserTextAsync(Core.User.User.UserID, ToUserID, skipList, 10, Core.User.User.ZAT);
        }

        void TextServiceClient_GetUserTextCompleted(object sender, TextService.GetUserTextCompletedEventArgs e)
        {

            LoadPrevTextPB.Visibility = System.Windows.Visibility.Collapsed;

            Services.TextServiceClient.MarkAsReadAsync(ToUserID, Core.User.User.UserID, Core.User.User.ZAT);

           

            Services.TextServiceClient.GetUserTextCompleted -= TextServiceClient_GetUserTextCompleted;

            bool Empty;

            if (e.Error == null)
            {
                if (e.Result != null)
                {
                    ProfilePicImg.Source = new BitmapImage(new Uri(e.Result.User.ProfilePicURL));
                    NAMELBL.Text = e.Result.User.Name;

                    if (Texts == null||Texts.Count==0)
                    {
                        Empty = true;
                        Texts = new System.Collections.ObjectModel.ObservableCollection<TextService.Text>();
                    }
                    if (e.Result.Texts != null)
                    {
                        if (e.Result.Texts.Count != 0)
                        {
                            if(Texts==null)
                            {
                                Texts = new System.Collections.ObjectModel.ObservableCollection<TextService.Text>();
                            }

                            if (Texts.Count == 0)
                            {
                                Texts = e.Result.Texts;

                            }
                            else
                            {
                                int i = 0;
                                foreach (TextService.Text t in e.Result.Texts)
                                {
                                    Texts.Insert(i, t);
                                    i++;
                                }
                            }
                            
                        }

                       
                    }

                    if (Texts.Count > 0)
                    {
                        ShowText.Begin();
                    }
                    else
                    {
                        ShowNoText.Begin();
                    }
                }
                else
                {
                    ShowNoText.Begin();
                }
            }
            else
            {
                ShowTextError.Begin();
            }


            if (Texts!=null&&Texts.Count > 0)
            {
                RefreshList();
                //scroll to end. 
                TextLB.BringIntoView(Texts.Last());
            }
        }



        private void RefreshList()
        {
            TextLB.ItemsSource = null;
            TextLB.ItemsSource = Texts;
        }

        private void NavigateToDashboard()
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    
                        NavigationService.Navigate(new Uri(Navigation.Dashboard, UriKind.RelativeOrAbsolute));
                   

                });
            }
            catch (System.Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }
        }


        private void dashboardBtn_Click(object sender, EventArgs e)
        {
            NavigateToDashboard();
        }

      

    

        public Guid ToUserID { get; set; }

        

        void TextServiceClient_SendTextCompleted(object sender, TextService.SendTextCompletedEventArgs e)
        {
            Services.TextServiceClient.SendTextCompleted -= TextServiceClient_SendTextCompleted;
            if (e.Error == null)
            {
                if (e.Result == TextService.TextSentStatus.Error)
                {
                    MorePB.Visibility = System.Windows.Visibility.Collapsed;
                    MessageBox.Show("Can't send your text. Problem with your network", "Turbulance up there", MessageBoxButton.OK);
                }
                else
                {

                    Guid g = new Guid();
                    if(Texts.Count>0)
                    {
                        g = Texts.Last().TextId;
                    }
                   
                    Services.TextServiceClient.GetTextsAfterCompleted += TextServiceClient_GetTextsAfterCompleted;
                    Services.TextServiceClient.GetTextsAfterAsync(Core.User.User.UserID, ToUserID, g, Core.User.User.ZAT);
                }
            }
            else
            {
                MorePB.Visibility = System.Windows.Visibility.Collapsed;
                MessageBox.Show("Can't send your text. Problem with your network", "Turbulance up there", MessageBoxButton.OK);
            }
        }

        void TextServiceClient_GetTextsAfterCompleted(object sender, TextService.GetTextsAfterCompletedEventArgs e)
        {
            Services.TextServiceClient.MarkAsReadAsync(ToUserID, Core.User.User.UserID, Core.User.User.ZAT);
            MorePB.Visibility = System.Windows.Visibility.Collapsed;
            Services.TextServiceClient.GetTextsAfterCompleted -= TextServiceClient_GetTextsAfterCompleted;
            if (e.Error == null)
            {
                if (e.Result != null)
                {

                    if (Texts == null)
                    {
                        Texts = new System.Collections.ObjectModel.ObservableCollection<TextService.Text>();
                    }
                    if (e.Result.Count != 0)
                    {
                        foreach (TextService.Text t in e.Result)
                        {
                            Texts.Add(t);
                        }

                    }


                    if (Texts.Count > 0)
                    {
                        ShowText.Begin();
                    }
                    else
                    {
                        ShowNoText.Begin();
                    }
                }
                else
                {
                    if (Texts.Count == 0)
                    {
                        ShowNoText.Begin();
                    }
                    else
                    {
                        ShowText.Begin();
                    }
                }
            }
            else
            {
                ShowTextError.Begin();
            }


            if (Texts!=null&&Texts.Count > 0)
                    {
                //scroll to end. 
                        TextLB.BringIntoView(Texts.Last());
            }
        }

      

       


       
        public Guid Id { get; set; }

        private void CommentTB1_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
                if(e.Key==Key.Enter)
                {
                    if (!String.IsNullOrWhiteSpace(CommentTB1.Text))
                    {
                        MorePB.Visibility = System.Windows.Visibility.Visible;
                        Services.TextServiceClient.SendTextCompleted += TextServiceClient_SendTextCompleted;
                        Services.TextServiceClient.SendTextAsync(Core.User.User.UserID, ToUserID, CommentTB1.Text.Trim(), Core.User.User.ZAT);
                        CommentTB1.Text = "";
                        
                    }
                }
        }

        private void CommentTB_TextChanged(object sender, TextChangedEventArgs e)
        {
                if(CommentTB1.Text.Count()>0)
                {
                    typewEmailLBL.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    typewEmailLBL.Visibility = System.Windows.Visibility.Visible;
                }
        }

        private void typewEmailLBL_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            CommentTB1.Focus();
        }

        private void ProfilePicImg_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.UserProfile + "?id=" + Id, UriKind.RelativeOrAbsolute));
            });
        }

        private void NAMELBL_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri(Navigation.UserProfile + "?id=" + Id, UriKind.RelativeOrAbsolute));
            });
        }
    }
}