using Avalonia.Threading;

using System;
using System.Threading.Tasks;
using System.Linq;

using HashtagChris.DotNetBlueZ;
using HashtagChris.DotNetBlueZ.Extensions;

using IoTHubDevice.Models;

namespace IoTHubDevice.Services
{
    public class BTService
    {
        public bool IsSearching { get; private set; }

        public event EventHandler BTFindStart;
        public event EventHandler BTFindEnd;

        private IAdapter1 adapter;

        public BTService()
        {
            _ = LoadFirstAdapter();
            _ = LoadPairedDevices();
        }

        public async Task LoadFirstAdapter()
        {
            try
            {
                adapter = (await BlueZManager.GetAdaptersAsync()).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task LoadPairedDevices()
        {
            try
            {
                var devices = await adapter.GetDevicesAsync();

                foreach (var device in devices)
                {
                    var iotDevice = new IoTDevice(device)
                    {
                        BTName = await device.GetNameAsync(),
                        MACAddress = string.Empty,
                        Path = device.ObjectPath.ToString(),
                    };

                    AppEnvironment.deviceManager.PairedList.Add(iotDevice);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task FindDevices()
        {
            try
            {
                AppEnvironment.deviceManager.FindedList.Clear();

                IsSearching = true;
                BTFindStart?.Invoke(this, new EventArgs());

                using (await adapter.WatchDevicesAddedAsync(async device =>
                {
                    await Dispatcher.UIThread.InvokeAsync(async () =>
                    {
                        AppEnvironment.deviceManager.FindedList.Add(new IoTDevice(device)
                        {
                            BTName = await device.GetNameAsync(),
                            MACAddress = string.Empty,
                            Path = device.ObjectPath.ToString()
                        });
                    });
                }))
                {
                    await adapter.StartDiscoveryAsync();
                    await Task.Delay(TimeSpan.FromSeconds(5));
                }

                await adapter.StopDiscoveryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                IsSearching = false;
                BTFindEnd?.Invoke(this, new EventArgs());
            }
        }
    }
}