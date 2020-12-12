using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Interactivity;  
using Avalonia.Input; 
using Avalonia.Threading;

using System;
using System.Timers;
using System.IO;

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
        private TextBlock deviceStatus;
        private TextBlock deviceInfo;
        private Button deviceCommand;

        private IoTDevice selectedDevice;

        private DispatcherTimer detailTimer;

        public DevicesView()
        {
            InitializeComponent();

            DataContext = new DevicesViewModel();

            detailTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2)
            };
            detailTimer.Tick += RefreshDetailInfo;
            detailTimer.Start();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            pairedDeviceListBox = this.FindControl<ListBox>("PairedDeviceListBox");

            deviceName = this.FindControl<TextBlock>("DeviceName");
            deviceType = this.FindControl<TextBlock>("DeviceType");
            deviceMAC = this.FindControl<TextBlock>("DeviceMAC");
            deviceStatus = this.FindControl<TextBlock>("DeviceStatus");
            deviceInfo = this.FindControl<TextBlock>("DeviceInfo");
            deviceCommand = this.FindControl<Button>("DeviceCommand");
        }

        private void RefreshDetailInfo(object sender, EventArgs e)
        {
            try
            {
                if (selectedDevice == null)
                {
                    return;
                }

                deviceName.Text = selectedDevice.DeviceName;
                deviceType.Text = selectedDevice.DeviceTypeString;
                deviceMAC.Text = selectedDevice.MACAddress;
                deviceInfo.Text = selectedDevice.Sensor.GetInfoString();

                var statusText = string.Empty;
                var statusColor = Brushes.White;

                switch (selectedDevice.Status)
                {
                    case DeviceStatus.Connected:
                        statusText = "Connected";
                        statusColor = Brushes.Green;
                        break;
                    case DeviceStatus.Disconnected:
                        statusText = "Disconnected";
                        statusColor = Brushes.OrangeRed;
                        break;
                    case DeviceStatus.Unknown:
                        statusText = "Unknown";
                        statusColor = Brushes.LightGray;
                        break;
                }

                deviceStatus.Text = statusText;
                deviceStatus.Foreground = Brushes.AliceBlue;

                switch (selectedDevice.SensorType)
                {
                    case IoTDeviceType.DeviceType.HTSensor:
                    case IoTDeviceType.DeviceType.DustSensor:
                        deviceCommand.IsVisible = true;
                        deviceCommand.IsEnabled = true;
                        deviceCommand.Content = "Fan Toggle";
                        break;
                    case IoTDeviceType.DeviceType.LightSensor:
                        deviceCommand.IsVisible = true;
                        deviceCommand.IsEnabled = true;
                        deviceCommand.Content = "LED Toggle";
                        break;
                    case IoTDeviceType.DeviceType.GasSensor:
                    default:
                        deviceCommand.IsVisible = false;
                        deviceCommand.IsEnabled = false;
                        break;
                }
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

        private async void CommandButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string command = (sender as Button).Content as string switch
                {
                    "Fan Toggle" => "Fan",
                    "LED Toggle" => "LED",
                    _ => ""
                };

                //AppEnvironment.deviceManager.updateTimer.Stop();
                await selectedDevice.SendCommand(command);
                //AppEnvironment.deviceManager.updateTimer.Start();
            }
            catch { }
        }

        private void DeviceListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (e.AddedItems.Count < 1)
                {
                    return;
                }

                detailTimer.Stop();

                var device = e.AddedItems[0] as IoTDevice;
                
                selectedDevice = device;

                pairedDeviceListBox.SelectedItems = null;

                RefreshDetailInfo(this, new EventArgs());

                detailTimer.Start();
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