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

namespace TransparentSites
{
    public partial class WebImagePicker : PhoneApplicationPage, IDisposable
    {
        string filter;
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
            this.imageList.ItemsSource = imageSource;
        }

        private void image_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }

        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {
            filter = boxCriteria.Text;
            beginRequest();
        }

        void beginRequest()
        {
            string uri = "http://api.icons8.com/api/iconsets/search?term=" + filter + "&amount=20&platform=win8";
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.BeginGetResponse(new AsyncCallback(ReadWebRequestCallback), request);
        }

        private void ReadWebRequestCallback(IAsyncResult ar)
        {
            HttpWebRequest request = (HttpWebRequest)ar.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(ar);
            using(StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string result = @reader.ReadToEnd();
                //XDocument docResult = XDocument.Parse(result, LoadOptions.None);
                XElement elements = XElement.Parse(result);
                //FillList(docResult);
                FillList2(elements);

            }
        }

        private void FillList2(XElement elements)
        {
            var data = from var in elements.Descendants("icon")
                       //where (int)var.Element("width") == 256
                       select new imageData
                       {
                           imageUri = var.Element("png").Element("png").Attribute("link").Value
                       };
            this.Dispatcher.BeginInvoke((Action)(() => { imageList.ItemsSource = data; }));
        }

        private void FillList(XDocument docResult)
        {
            //var element = XElement.Load(docResult,);

            var data = from query in docResult.Descendants("icon")
                          select new imageData
                          {
                              imageUri = query.Element("png").Value
                          };

            this.Dispatcher.BeginInvoke((Action)(() => { imageList.ItemsSource = data; }));
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