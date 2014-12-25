using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace TransparentSites
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            //Code to localize the ApplicationBar
            BuildLocalizedApplicationBar();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Store the typed data so the userdon't need to re-type it when he comeback
            tileDescription.Text = App.description;
            tileLink.Text = App.link;
            if (!String.IsNullOrEmpty(App.imageUrl))
            {
                //Downloading the image
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    ImageSource source = new BitmapImage(new Uri(App.imageUrl, UriKind.Absolute));
                    tileIcon.Source = source;
                });
            }
            base.OnNavigatedTo(e);
        }

        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            string uriPin;
            if (CurrentTheme == AppTheme.Dark)
            {
                uriPin = "/Assets/pins/whitePin.png";
            }
            else
            {
                uriPin = "/Assets/pins/darkPin.png";
            }
            
            ApplicationBarIconButton appBarButtonPin = new ApplicationBarIconButton(new Uri(uriPin, UriKind.Relative));
            appBarButtonPin.Text = TransparentSites.Resources.AppResources.AppBarButtonText;
            appBarButtonPin.Click += appBarButton_Click;
            ApplicationBar.Buttons.Add(appBarButtonPin);

            ApplicationBarIconButton appBarButtonSearch = new ApplicationBarIconButton(new Uri("/Assets/pins/feature.search.png", UriKind.Relative));
            appBarButtonSearch.Text = "Search";
            appBarButtonSearch.Click += appBarButton_ClickSearch;
            ApplicationBar.Buttons.Add(appBarButtonSearch);

        }
        private void appBarButton_ClickSearch(object sender, EventArgs e)
        {
            App.link = tileLink.Text;
            App.description = tileDescription.Text;
            NavigationService.Navigate(new Uri("/WebImagePicker.xaml", UriKind.Relative));
        }

        private void appBarMenuItem_ClickAbout(object sender, EventArgs e)
        {
            App.link = tileLink.Text;
            App.description = tileDescription.Text;
            NavigationService.Navigate(new Uri("/WebImagePicker.xaml", UriKind.Relative));
        }

        private void appBarButton_Click(object sender, EventArgs e)
        {
            try
            {
                IconicTileData iconTile = new IconicTileData();
                iconTile.Title = tileDescription.Text;
                iconTile.WideContent1 = "TransparentSites";
                iconTile.WideContent2 = tileLink.Text;
                iconTile.SmallIconImage = iconTile.IconImage = new Uri(App.imageUrl, UriKind.Absolute);
                iconTile.BackgroundColor = Colors.Transparent;            
                ShellTile.Create(new Uri("/NavigationPage.xaml?url=" + tileLink.Text, UriKind.Relative), iconTile, false);

            }
            catch (Exception ಠ_ಠ)
            {
                MessageBox.Show("Error while creating your live tile", "Error", MessageBoxButton.OK);
                Debug.WriteLine(ಠ_ಠ.Message);
            }
            
        }

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

        private void SiteTile_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.link = tileLink.Text;
            App.description = tileDescription.Text;
            NavigationService.Navigate(new Uri("/WebImagePicker.xaml", UriKind.Relative));
        }
    }
}