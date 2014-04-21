using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace TransparentSites
{
    public partial class NavigationPage : PhoneApplicationPage
    {
        public NavigationPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string url = NavigationContext.QueryString["url"];

            if (!url.Contains("http://"))
            {
                url = "http://" + url;
            }

            WebBrowserTask navigate = new WebBrowserTask();
            navigate.Uri = new Uri(url, UriKind.RelativeOrAbsolute);
            navigate.Show();
            Application.Current.Terminate();
        }

    }
}