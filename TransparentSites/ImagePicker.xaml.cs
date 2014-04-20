using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TransparentSitesDB;

namespace TransparentSites
{
    public partial class ImagePicker : PhoneApplicationPage
    {
        public ImagePicker()
        {
            InitializeComponent();

            LoadAllImages();
        }

        private void LoadAllImages()
        {
            IconsModel IconsModel = new IconsModel();
            ImageList.ItemsSource = IconsModel.GetIcons();
        }

        private void image_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var t = sender as Image;
            if (t != null)
            {
                IconsModel iconModel = t.DataContext as IconsModel;
                App.imageUrl = iconModel.IconPath;
            }

            NavigationService.Navigate(new Uri("/MainPage.xaml",UriKind.Relative));
        }
    }
}