using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace IoTHubDevice.Views
{
    public class DevicesView : UserControl
    {
        public DevicesView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}