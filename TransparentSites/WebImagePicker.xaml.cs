using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Diagnostics;
using System.Device.Location;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace TransparentSites
{
    public partial class WebImagePicker : PhoneApplicationPage
    {
        string filter;
        List<String> files = new List<string>();
        IEnumerable<imageData> data;
        public WebImagePicker()
        {
            InitializeComponent();
            //Initiating the GeoCordinater and assigning it's handlers
        }

        private void image_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.imageUrl = ((Image)sender).Name.ToString().Replace("26","256");
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }

        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {    
            progress.Visibility = Visibility.Visible;
            progress.Height = 100;
            progress.Width = panel.Width;

            filter = boxCriteria.Text;
            try
            {
                beginRequest();
            }
            catch (Exception ಠ_ಠ)
            {
                MessageBox.Show("Error while trying to download the icons." + Environment.NewLine + "Check your internet connection and try again.");
                Debug.WriteLine(ಠ_ಠ.Message);
            }
            finally
            {
                progress.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        void beginRequest()
        {
            try
            {
                string uri = "http://api.icons8.com/api/iconsets/search?term=" + filter + "&amount=50&platform=win8";
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
                request.BeginGetResponse(new AsyncCallback(ReadWebRequestCallback), request);
            }
            catch (Exception)
            {
                throw new Exception("Error while trying to contact icons8.com, check your internet connection and try again");
            }
        }

        private void ReadWebRequestCallback(IAsyncResult ar)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)ar.AsyncState;
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(ar);
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string result = @reader.ReadToEnd();
                    XElement elements = XElement.Parse(result);
                    data = from var in elements.Descendants("icon") select new imageData
                           { imageUri = new Uri(var.Element("png").Element("png").Attribute("link").Value, UriKind.Absolute)};
                }
                this.Dispatcher.BeginInvoke((Action)(() => { LoadIMG(data); }));
            }
            catch (Exception ಠ_ಠ)
            {
                this.Dispatcher.BeginInvoke((Action)(() => { 
                    MessageBox.Show("Error while trying to download the icons." + Environment.NewLine + "Check your internet connection and try again."); 
                }));
                Debug.WriteLine(ಠ_ಠ.Message);
            }
        }

        private void LoadIMG(IEnumerable<imageData> images)
        {
            panel.Children.Clear();
            foreach (var image in images)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        ImageSource source = new BitmapImage(image.imageUri);
                        Image control = new Image();
                        control.DoubleTap += image_DoubleTap;
                        control.Name = image.imageUri.ToString();
                        control.Height = 60;
                        control.Width = 60;
                        control.Source = source;
                        control.Margin = new System.Windows.Thickness(10, 10, 10, 10);

                        WrapPanel wrap = new WrapPanel();
                        wrap.Children.Add(control);
                        wrap.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                        wrap.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                        panel.Children.Add(wrap);
                    });
            }
        }

        private void boxCriteria_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            boxCriteria.SelectAll();
        }
    }
}