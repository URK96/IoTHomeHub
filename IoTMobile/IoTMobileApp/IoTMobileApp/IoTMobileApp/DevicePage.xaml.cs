using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

using Newtonsoft.Json.Linq;

namespace IoTMobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DevicePage : ContentPage
    {
        public List<IoTDevice> Devices { get; set; }

        private Timer refreshTimer;

        public DevicePage()
        {
            InitializeComponent();

            Devices = new List<IoTDevice>();

            BindingContext = this;

            refreshTimer = new Timer(RefreshData, new AutoResetEvent(false), TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                refreshTimer.Change(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10));
            }
            catch { }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            try
            {
                refreshTimer.Change(Timeout.Infinite, Timeout.Infinite);
            }
            catch { }
        }

        private async void RefreshData(object statusInfo)
        {
            try
            {
                string data = string.Empty;

                using (var wc = new WebClient())
                {
                    data = await wc.DownloadStringTaskAsync(Path.Combine(AppEnvironment.serverRoot, "api", "iothub", "devices"));
                }

                var array = JArray.Parse(data);

                Devices.Clear();

                foreach (var token in array)
                {
                    Devices.Add(new IoTDevice(token));
                }

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    DeviceCollectionView.ItemsSource = null;
                    DeviceCollectionView.ItemsSource = Devices;
                });
            }
            catch { }
        }

        private async void DeviceCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (e.CurrentSelection.Count < 1)
                {
                    return;
                }

                var item = e.CurrentSelection.FirstOrDefault() as IoTDevice;

                (sender as CollectionView).SelectedItem = null;

                await Navigation.PushAsync(new DeviceDetailPage(item), true);
            }
            catch { }
        }
    }
}