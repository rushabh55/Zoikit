using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Threading;

namespace Hangout.Client
{
    public partial class TextLoader : UserControl
    {
        public TextLoader()
        {
            InitializeComponent();
        }

        public void DisplayText()
        {
            ShowTxt.Begin();
            AnimateText.Begin();
        }

        public void DisplayText(string text)
        {
            LoadingTextLBL.Text = text;
            ShowTxt.Begin();
            AnimateText.Begin();
        }

        public void DisplayText(List<string> text)
        {
            Dispatcher.BeginInvoke(() =>
                {
                    foreach (string t in text)
                    {
                        LoadingTextLBL.Text = t;
                        ShowTxt.Begin();
                        Thread.Sleep(new TimeSpan(0, 0, 4));
                        HideTxt.Begin();
                        Thread.Sleep(new TimeSpan(0, 0, 1));
                    }
                });
        }

        public void Collapse()
        {
            LoadingTextLBL.Visibility = System.Windows.Visibility.Collapsed;
            PGLoad.Visibility = System.Windows.Visibility.Collapsed;
        }

        public void Visible()
        {
            LoadingTextLBL.Visibility = System.Windows.Visibility.Visible;
            PGLoad.Visibility = System.Windows.Visibility.Visible;
        }
        
    }
}
