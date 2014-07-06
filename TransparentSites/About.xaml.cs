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
using Microsoft.Phone.Tasks;
using System.Diagnostics;
using Microsoft.Advertising.Mobile.UI;
using System.Device.Location;

namespace TransparentSites
{
    public partial class About : PhoneApplicationPage
    {
        public About()
        {
            InitializeComponent();
            avatar.Source = new BitmapImage(new Uri("/Assets/pins/briano.png", UriKind.Relative));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //WebBrowserTask used to open IE and send to a pre-determinated webpage
            WebBrowserTask follow = new WebBrowserTask();
            follow.Uri = new Uri("https://m.twitter.com/BrianoStorm", UriKind.Absolute);
            follow.Show();
        }

        private void RedditTPP_Click(object sender, RoutedEventArgs e)
        {
            WebBrowserTask follow = new WebBrowserTask();
            follow.Uri = new Uri("http://www.reddit.com/r/twitchplayspokemon/", UriKind.Absolute);
            follow.Show();
        }
    }
}