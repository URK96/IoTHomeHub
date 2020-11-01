using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

using IoTHubDevice.Services;

namespace IoTHubDevice.Views
{
    public class SplashView : UserControl
    {
        public SplashView()
        {
            InitializeComponent();

            AppEnvironment.btService = new BTService();
            AppEnvironment.deviceManager = new IoTDeviceManager();
            AppEnvironment.weather = new WeatherService();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}