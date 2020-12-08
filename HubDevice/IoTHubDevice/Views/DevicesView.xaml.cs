using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Interactivity;  
using Avalonia.Input; 

using System;

using IoTHubDevice.Models;
using IoTHubDevice.ViewModels;

namespace IoTHubDevice.Views
{
    public class DevicesView : UserControl
    {
        private ListBox pairedDeviceListBox;
        private TextBlock deviceName;
        private TextBlock deviceType;
        private TextBlock deviceMAC;

        public DevicesView()
        {
            InitializeComponent();

            DataContext = new DevicesViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            pairedDeviceListBox = this.FindControl<ListBox>("PairedDeviceListBox");

            deviceName = this.FindControl<TextBlock>("DeviceName");
            deviceType = this.FindControl<TextBlock>("DeviceType");
            deviceMAC = this.FindControl<TextBlock>("DeviceMAC");
        }

        private async void RefreshDetailInfo(IoTDevice device)
        {
            try
            {
                deviceName.Text = device.DeviceName;
                deviceType.Text = device.DeviceTypeString;
                deviceMAC.Text = device.MACAddress;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private async void FindButtonClick(object sender, RoutedEventArgs e)
        {
            var dialog = new BTFindDialog
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            
            await dialog.ShowDialog(AppEnvironment.mainWindow);
        }

        private void DeviceListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (e.AddedItems.Count < 1)
                {
                    return;
                }

                var device = e.AddedItems[0] as IoTDevice;

                pairedDeviceListBox.SelectedItems = null;

                RefreshDetailInfo(device);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

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