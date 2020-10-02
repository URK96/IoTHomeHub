using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

using System.Threading.Tasks;

namespace IoTHubDevice.Views
{
    public class MainWindow : Window
    {
        private Window rootWindow;
        public MainWindow()
        {
            InitializeComponent();

            rootWindow.Content = new SplashView();

            _ = SplashProcess();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            rootWindow = this.FindControl<Window>("MainRootWindow");
        }

        private async Task SplashProcess()
        {
            await Task.Delay(5000);

            rootWindow.Content = new MainView();
        }
    }
}