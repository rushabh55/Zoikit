using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Hangout.Client.Controls.Hangouts
{
    public partial class HangoutComments : UserControl
    {
        public HangoutComments()
        {
            InitializeComponent();
        }


        public event EventHandler PreviousBtnClicked;
 
        


        private List<BuzzService.BuzzComment> hangoutComments;

        public List<BuzzService.BuzzComment> HangoutComment
        {
            get
            {
               return hangoutComments;
                
            }
            set
            {
                    hangoutComments = value;
                    UpdateData();
            }
        }

        public void UpdateData()
        {

            try
            {
                if (hangoutComments.Count > 0)
                {
                    HangoutCommentsLB.DataContext = hangoutComments;
                    HangoutCommentsLB.ItemsSource = null;
                    HangoutCommentsLB.ItemsSource = hangoutComments;
                    HangoutCommentsLB.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    HangoutCommentsLB.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
            }

            
        }

        private void GetPreviousButton_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (PreviousBtnClicked != null)
            {
                PreviousBtnClicked(null, new EventArgs());
            }
        }

        public void RefreshList()
        {
            HangoutCommentsLB.DataContext = hangoutComments;
            HangoutCommentsLB.ItemsSource = null;
            HangoutCommentsLB.ItemsSource = hangoutComments;
        }

        public delegate void UserSelectedHelper(int id);

        public event UserSelectedHelper UserSelected;

        private void HangoutCommentsLB_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (HangoutCommentsLB.SelectedItem != null)
            {
                BuzzService.BuzzComment t = (BuzzService.BuzzComment)HangoutCommentsLB.SelectedItem;

                if (UserSelected != null)
                {

                    UserSelected(t.User.UserID);
                }

            }
        }
    }
}
