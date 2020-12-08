using Avalonia.Threading;

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Linq;

using HashtagChris.DotNetBlueZ;
using HashtagChris.DotNetBlueZ.Extensions;

using IoTHubDevice.Models;

using SmallDB;

using DBConstant = SmallDB.Constant.IoTDeviceDBConstant;

namespace IoTHubDevice.Services
{
    public class BTService
    {
        public bool IsSearching { get; private set; }
        public IAdapter1 BTAdapter { get; }

        public event EventHandler BTFindStart;
        public event EventHandler BTFindEnd;

        private IAdapter1 adapter;

        public BTService()
        {
            LoadFirstAdapter();
            LoadPairedDevices();
        }

        public async void LoadFirstAdapter()
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

        public void LoadPairedDevices()
        {
            try
            {
                SmallDBService.LoadDB();
                
                foreach (DataRow dr in SmallDBService.DBTable.Rows)
                {
                    AppEnvironment.deviceManager.PairedList.Add(new IoTDevice(dr));
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

                var exceptMAC = new List<string>();

                foreach (var pDevice in AppEnvironment.deviceManager.PairedList)
                {
                    exceptMAC.Add(pDevice.MACAddress.Replace(':', '_'));
                }

                var devices = await BTAdapter.GetDevicesAsync();

                foreach (var device in devices)
                {
                    var paths = device.ObjectPath.ToString().Split('/');
                    var mac = paths[paths.Length - 1];

                    if (!exceptMAC.Contains(mac) &&
                        await CheckBTName(device))
                    {
                        AppEnvironment.deviceManager.FindedList.Add(new IoTDevice(device));
                    }
                }

                using (await adapter.WatchDevicesAddedAsync(async device =>
                {
                    await Dispatcher.UIThread.InvokeAsync(async () =>
                    {
                        if (await CheckBTName(device))
                        {
                            AppEnvironment.deviceManager.FindedList.Add(new IoTDevice(device));
                        }
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

        private async Task<bool> CheckBTName(Device device)
        {
            try
            {
                var btName = await device.GetNameAsync();

                if (!btName.Equals("ED-BT 52810"))
                {
                    throw new Exception("Not equal bt name");
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}