using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;   

using System;

using IoTHubDevice.ViewModels;

namespace IoTHubDevice.Views
{
    public class DevicesView : UserControl
    {
        public DevicesView()
        {
            InitializeComponent();

            DataContext = new DevicesViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async void FindButtonClick(object sender, RoutedEventArgs e)
        {
            var dialog = new BTFindDialog
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            
            await dialog.ShowDialog(AppEnvironment.mainWindow);
        }
    }
}