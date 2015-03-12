using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Hangout.Client
{
    public partial class FadingText : UserControl
    {
        public FadingText()
        {
            InitializeComponent();
        }

        List<string> Texts;

        int current = 0;

        public void StartTextAnimation(List<string> texts)
        {
            Texts = texts;
            FadeSB.Completed += FadeSB_Completed;
            Change();
            FadeSB.Begin();
        }

        void FadeSB_Completed(object sender, EventArgs e)
        {
            Change();
            FadeSB.Begin();
        }

        private void Change()
        {
            int count = Texts.Count();
            TextBlk.Text = Texts[current];
            current++;
            current = current % (count);
            
        }
    }
}
