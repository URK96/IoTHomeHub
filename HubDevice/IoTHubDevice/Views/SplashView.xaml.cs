using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace IoTHubDevice.Views
{
    public class SplashView : UserControl
    {
        public SplashView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}