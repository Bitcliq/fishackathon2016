using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Platform.Services.Geolocation;

namespace PinFixed.BL
{
    public class GPSUtils
    {
        string PositionStatus;
        public string PositionLatitude;
        public string PositionLongitude;

        public DateTime LastUpdated;

        IGeolocator geolocator;
        CancellationTokenSource cancelSource;
        TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();

        private bool IsBusy = false;
        public GPSUtils()
        {
            GetPosition();
        }


        void Setup()
        {


            if (this.geolocator != null)
                return;


            this.geolocator = DependencyService.Get<IGeolocator>();

            if (geolocator != null)
            {
                this.geolocator.StartListening(10000, 1, true);

                this.geolocator.PositionError += OnListeningError;
                this.geolocator.PositionChanged += OnPositionChanged;


                //this.geolocator.StartListening +=  StartListening;     
            }
        }




        private void OnListeningError(object sender, PositionErrorEventArgs e)
        {
            PositionStatus = String.Empty;
            PositionLatitude = String.Empty;
            PositionLongitude = String.Empty;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Handles the <see cref="E:PositionChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PositionEventArgs"/> instance containing the event data.</param>
        private void OnPositionChanged(object sender, PositionEventArgs e)
        {
            //throw new NotImplementedException();

            PositionLatitude = e.Position.Latitude + "";
            PositionLongitude = e.Position.Longitude + "";
            LastUpdated = DateTime.Now;
            System.Diagnostics.Debug.WriteLine(" Position Changed  " + PositionLatitude + " " + PositionLongitude);
        }

        public async Task GetPosition()
        {
            if (!IsBusy)
            {
                Setup();

                this.cancelSource = new CancellationTokenSource();

                PositionStatus = String.Empty;
                PositionLatitude = String.Empty;
                PositionLongitude = String.Empty;
                IsBusy = true;
                await this.geolocator.GetPositionAsync(timeout: 5000, cancelToken: this.cancelSource.Token, includeHeading: true)
                    .ContinueWith(t =>
                    {
                        IsBusy = false;
                        if (t.IsFaulted)
                            PositionStatus = ((GeolocationException)t.Exception.InnerException).Error.ToString();
                        else if (t.IsCanceled)
                            PositionStatus = "Canceled";
                        else
                        {
                            PositionStatus = t.Result.Timestamp.ToString("G");
                            PositionLatitude = t.Result.Latitude.ToString("N4");
                            PositionLongitude = t.Result.Longitude.ToString("N4");
                            LastUpdated = t.Result.Timestamp.Date.AddHours(t.Result.Timestamp.Hour).AddMinutes(t.Result.Timestamp.Minute).AddSeconds(t.Result.Timestamp.Second).AddMilliseconds(t.Result.Timestamp.Millisecond);
                        }

                    }, scheduler);
            }
        }


    }
}
