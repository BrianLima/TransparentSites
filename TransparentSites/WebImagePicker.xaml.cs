using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Diagnostics;
using System.Device.Location;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using System.Net.Http;
using TransparentSitesDB;
using System.Windows.Media;

namespace TransparentSites
{
    public partial class WebImagePicker : PhoneApplicationPage, IDisposable
    {
        string filter;
        List<String> files = new List<string>();
        IEnumerable<imageData> data;
        List<imageData> imageSource;
        //Geographical position tracker for geoposition based ads
        private GeoCoordinateWatcher gcw = null;

        public WebImagePicker()
        {
            InitializeComponent();
            //Initiating the GeoCordinater and assigning it's handlers
            this.gcw = new GeoCoordinateWatcher();
            this.gcw.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(gcw_PositionChanged);
            this.gcw.Start();

            imageSource = new List<imageData>();
        }

        private void image_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string a = ((Image)sender).Name;
        }

        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {
            filter = boxCriteria.Text;
            beginRequest();
        }

        void beginRequest()
        {
            string uri = "http://api.icons8.com/api/iconsets/search?term=" + filter + "&amount=50&platform=win8";
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.BeginGetResponse(new AsyncCallback(ReadWebRequestCallback), request);
        }

        private void ReadWebRequestCallback(IAsyncResult ar)
        {
            HttpWebRequest request = (HttpWebRequest)ar.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(ar);
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string result = @reader.ReadToEnd();
                //XDocument docResult = XDocument.Parse(result, LoadOptions.None);
                XElement elements = XElement.Parse(result);
                //FillList(docResult);
                data = from var in elements.Descendants("icon")
                       select new imageData { 
                           imageUri = new Uri(var.Element("png").Element("png").Attribute("link").Value, UriKind.Absolute) 
                       };
            }
            this.Dispatcher.BeginInvoke((Action)(() => { LoadIMG(data); }));
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

        private void boxCriteria_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            boxCriteria.SelectAll();
        }
    }
}