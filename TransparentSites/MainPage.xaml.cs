using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TransparentSites.Resources;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Device.Location;
using System.Diagnostics;

namespace TransparentSites
{
    public partial class MainPage : PhoneApplicationPage, IDisposable
    {
        //Geographical position tracker for geoposition based ads
        private GeoCoordinateWatcher gcw = null;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            // Sample code to localize the ApplicationBar
            BuildLocalizedApplicationBar();

            //Initiating the GeoCordinater and assigning it's handlers
            this.gcw = new GeoCoordinateWatcher();
            this.gcw.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(gcw_PositionChanged);
            this.gcw.Start();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            tileDescription.Text = App.description;
            tileLink.Text = App.link;
            if (!String.IsNullOrEmpty(App.imageUrl))
            {
                tileIcon.Source = new BitmapImage(new Uri(App.imageUrl, UriKind.Relative));
            }

            base.OnNavigatedTo(e);
        }

        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            string uri;
            if (CurrentTheme == AppTheme.Dark)
            {
                uri = "/Assets/pins/whitePin.png";
            }
            else
            {
                uri = "/Assets/pins/darkPin.png";
            }
            
            ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri(uri, UriKind.Relative));
            appBarButton.Text = AppResources.AppBarButtonText;
            appBarButton.Click +=appBarButton_Click;
            ApplicationBar.Buttons.Add(appBarButton);

            // Create a new menu item with the localized string from AppResources.
            ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
            appBarMenuItem.Click += appBarMenuItem_Click;
            ApplicationBar.MenuItems.Add(appBarMenuItem);
        }

        private void appBarMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
        }

        private void appBarButton_Click(object sender, EventArgs e)
        {
            IconicTileData iconTile = new IconicTileData();
            iconTile.Title = tileDescription.Text;
            iconTile.WideContent1 = "TransparentSites";
            iconTile.WideContent2 = tileLink.Text;
            iconTile.IconImage = new Uri(App.imageUrl, UriKind.Relative);
            iconTile.SmallIconImage = new Uri(App.imageUrl, UriKind.Relative);
            iconTile.BackgroundColor = Colors.Transparent;

            ShellTile.Create(new Uri("/NavigationPage.xaml?url=" + tileLink.Text, UriKind.Relative), iconTile, false);
        }

        private void SiteTile_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.link = tileLink.Text;
            App.description = tileDescription.Text;
            NavigationService.Navigate(new Uri("/ImagePicker.xaml", UriKind.Relative));
        }

        private void gcw_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            // Stop the GeoCoordinateWatcher now that we have the device location.
            this.gcw.Stop();

            adControl1.Latitude = e.Position.Location.Latitude;
            adControl1.Longitude = e.Position.Location.Longitude;

            Debug.WriteLine("Device lat/long: " + e.Position.Location.Latitude + ", " + e.Position.Location.Longitude);
        }

        private void adControl1_ErrorOccurred(object sender, Microsoft.Advertising.AdErrorEventArgs e)
        {
            Debug.WriteLine("AdControl error: " + e.Error.Message);
        }


        private void AdControl_AdRefreshed(object sender, EventArgs e)
        {
            Debug.WriteLine("AdControl new ad received");
        }

        #region IDisposable Members

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.gcw != null)
                {
                    this.gcw.Dispose();
                    this.gcw = null;
                }
            }
        }

        #endregion


        //The following code handles how the app should work on either WP themes
        #region appTheme
        public enum AppTheme
        {
            Dark = 0,
            Light = 1
        }

        private static System.Windows.Media.Color lightThemeBackground = System.Windows.Media.Color.FromArgb(255, 255, 255, 255);
        private static System.Windows.Media.Color darkThemeBackgroud = System.Windows.Media.Color.FromArgb(255, 0, 0, 0);
        private static SolidColorBrush backgroundBrush;

        internal static AppTheme CurrentTheme
        {
            get
            {
                if (backgroundBrush == null)
                    backgroundBrush = Application.Current.Resources["PhoneBackgroundBrush"] as SolidColorBrush;

                if (backgroundBrush.Color == darkThemeBackgroud)
                    return AppTheme.Dark;
                if (backgroundBrush.Color == lightThemeBackground)
                    return AppTheme.Light;

                return AppTheme.Dark;
            }
        }
        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            App.link = tileLink.Text;
            App.description = tileDescription.Text;
            NavigationService.Navigate(new Uri("/ImagePicker.xaml", UriKind.Relative));

        }
    }
}