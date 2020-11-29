using System;
using System.IO;
using System.Net;
using System.Threading;

using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IoTMobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeviceDetailPage : ContentPage
    {
        public IoTDevice Device { get; }

        private Timer refreshTimer;

        public DeviceDetailPage(IoTDevice device)
        {
            InitializeComponent();

            Device = device;

            BindingContext = this;

            IoTDeviceName.Text = Device.DeviceName;
            IoTDeviceMAC.Text = Device.MACAddress;
            IoTDeviceType.Text = Device.SensorType switch
            {
                DeviceType.Unknown => "알 수 없음",
                DeviceType.HTSensor => "온/습도",
                DeviceType.DustSensor => "미세먼지",
                DeviceType.LightSensor => "빛",
                _ => "판별 불가"
            };

            EnableTypeLayout();

            refreshTimer = new Timer(RefreshStatusData, new AutoResetEvent(false), TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10));
        }

        private void EnableTypeLayout()
        {
            switch (Device.SensorType)
            {
                case DeviceType.HTSensor:
                    HTSensorInfoLayout.IsVisible = true;
                    break;
                case DeviceType.DustSensor:
                    DustSensorInfoLayout.IsVisible = true;
                    break;
                case DeviceType.LightSensor:
                    LightSensorInfoLayout.IsVisible = true;
                    break;
                case DeviceType.Unknown:
                default:
                    break;
            }
        }

        private async void RefreshStatusData(object statusInfo)
        {
            try
            {
                string data = string.Empty;

                using (var wc = new WebClient())
                {
                    data = await wc.DownloadStringTaskAsync(Path.Combine(AppEnvironment.serverRoot, "api", "iothub", "devices", "status", "info", Device.MACAddress));
                }

                Device.StatusArgument = data;

                MainThread.BeginInvokeOnMainThread(() => { RefreshStatus(); });
            }
            catch { }
        }

        private void RefreshStatus()
        {
            try
            {
                string[] tmp = Device.StatusArgument.Split(';');

                switch (Device.SensorType)
                {
                    case DeviceType.HTSensor:
                        HTSensorTemp.Text = $"{tmp[0]}℃";
                        HTSensorHumidity.Text = $"{tmp[1]}%";
                        break;
                    case DeviceType.DustSensor:
                        DustSensorStatus.Text = tmp[0];
                        DustSensorValue.Text = $"{tmp[1]}㎍/㎥";
                        break;
                    case DeviceType.LightSensor:
                        LightSensorValue.Text = tmp[0];
                        break;
                    case DeviceType.Unknown:
                    default:
                        break;
                }
            }
            catch { }
        }
    }
}