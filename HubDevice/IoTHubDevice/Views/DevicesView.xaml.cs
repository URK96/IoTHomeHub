using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Interactivity;  
using Avalonia.Input; 

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

        private void PairedBTItemLayoutPointerEnter(object sender, PointerEventArgs e)
        {
            ((StackPanel)sender).Background = Brushes.DarkGray;
        }

        private void PairedBTItemLayoutPointerLeave(object sender, PointerEventArgs e)
        {
            ((StackPanel)sender).Background = Brushes.Black;
        }
    }
}