using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Markup.Xaml;

using AvaloniaProgressRing;

using System;
using System.Linq;
using System.Threading.Tasks;

using IoTHubDevice.Models;
using IoTHubDevice.ViewModels;

namespace IoTHubDevice.Views
{
    public class BTFindDialog : Window
    {
        private ProgressRing searchStatusRing;
        private TextBlock statusLabel;
        private ListBox deviceListBox;  

        public BTFindDialog()
        {
            InitializeComponent();

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            DataContext = new BTFindDialogViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            searchStatusRing = this.FindControl<ProgressRing>("SearchStatusRing");
            statusLabel = this.FindControl<TextBlock>("StatusLabel");
            deviceListBox = this.FindControl<ListBox>("DeviceListBox");

            AppEnvironment.btService.BTFindStart += delegate 
            { 
                searchStatusRing.IsActive = true;
                deviceListBox.IsEnabled = false;
            };
            AppEnvironment.btService.BTFindEnd += delegate 
            { 
                searchStatusRing.IsActive = false; 
                deviceListBox.IsEnabled = true;
            };

            this.Closed += delegate
            {
                AppEnvironment.btService.BTFindStart -= delegate { searchStatusRing.IsActive = true; };
                AppEnvironment.btService.BTFindEnd -= delegate { searchStatusRing.IsActive = false; };
            };
        }

        private async void FindButtonClick(object sender, RoutedEventArgs e)
        {
            await AppEnvironment.btService.FindDevices();
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if ((e.AddedItems.Count < 1) ||
                    AppEnvironment.btService.IsSearching)
                {
                    return;
                }

                var device = e.AddedItems[0] as IoTDevice;

                deviceListBox.SelectedItems = null;
                deviceListBox.IsEnabled = false;

                statusLabel.Text = $"Connecting {device.MACAddress}...";

                await device.ConnectDevice();

                await Task.Delay(1000);

                // Add check type process
                statusLabel.Text = $"Check response {device.MACAddress}...";

                await device.SendCommand("Server");

                if (!(await device.ReceiveResponse()).Equals("Module"))
                {
                    throw new Exception("Not match check");
                }

                await Task.Delay(1000);

                statusLabel.Text = $"Check type {device.MACAddress}...";

                await device.SendCommand("Type");

                var type = await device.ReceiveResponse();

                var sep = device.MACAddress.Split(':')[0];

                if (type.Equals("HT"))
                {
                    device.SensorType = IoTDeviceType.DeviceType.HTSensor;
                    device.Sensor = new IoTDeviceType.HTSensor(device);

                    device.DeviceName = $"HT {sep}";
                }
                else if (type.Equals("Light"))
                {
                    device.SensorType = IoTDeviceType.DeviceType.LightSensor;
                    device.Sensor = new IoTDeviceType.LightSensor(device);

                    device.DeviceName = $"Light {sep}";
                }
                else if (type.Equals("Dust"))
                {
                    device.SensorType = IoTDeviceType.DeviceType.DustSensor;
                    device.Sensor = new IoTDeviceType.DustSensor(device);

                    device.DeviceName = $"Dust {sep}";
                }
                else if (type.Equals("Gas"))
                {
                    device.SensorType = IoTDeviceType.DeviceType.GasSensor;
                    device.Sensor = new IoTDeviceType.GasSensor(device);

                    device.DeviceName = $"Gas {sep}";
                }
                else
                {
                    throw new Exception("Cannot recognize type");
                }

                await Task.Delay(1000);

                device.Status = DeviceStatus.Connected;

                statusLabel.Text = $"Success";

                AppEnvironment.deviceManager.PairedList.Add(device);
                AppEnvironment.deviceManager.FindedList.Remove(device);
                AppEnvironment.deviceManager.AddDeviceToDB(device);

                await Task.Delay(2000);
            }
            catch (Exception ex)
            {
                statusLabel.Text = $"Check device status and try again";

                Console.WriteLine(ex.ToString());

                await Task.Delay(2000);
            }
            finally
            {
                statusLabel.Text = "";
                deviceListBox.IsEnabled = true;
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