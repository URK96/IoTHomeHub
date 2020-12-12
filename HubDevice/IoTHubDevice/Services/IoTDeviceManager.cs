using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Timers;

using IoTHubDevice.Models;

using SmallDB;

using DBConstant = SmallDB.Constant.IoTDeviceDBConstant;

namespace IoTHubDevice.Services
{
    public class IoTDeviceManager
    {
        public ObservableCollection<IoTDevice> PairedList { get; }
        public ObservableCollection<IoTDevice> FindedList { get; }

        private Timer saveInfoTimer;
        public Timer refreshConnectionTimer;
        public Timer updateTimer;

        public IoTDeviceManager()
        {
            PairedList = new ObservableCollection<IoTDevice>();
            FindedList = new ObservableCollection<IoTDevice>();

            saveInfoTimer = new Timer(2000);
            saveInfoTimer.Elapsed += delegate { SaveInfo(); };
            saveInfoTimer.Stop();

            refreshConnectionTimer = new Timer(5000);
            refreshConnectionTimer.Elapsed += delegate 
            {
                try
                {
                    refreshConnectionTimer.Stop();
                    RefreshConnection(); 
                    refreshConnectionTimer.Start();
                }
                catch { }
            };
            refreshConnectionTimer.Stop();

            updateTimer = new Timer(3000);
            updateTimer.Elapsed += delegate 
            { 
                try
                {
                    updateTimer.Stop();
                    UpdateModuleData();
                    updateTimer.Start();
                }
                catch { }
            };
            updateTimer.Stop();
        }

        public async void ConnectPairedDevices()
        {
            foreach (var device in PairedList)
            {
                await device.ConnectDevice();
            }

            saveInfoTimer.Start();
            refreshConnectionTimer.Start();
            updateTimer.Start();
        }

        private async void RefreshConnection()
        {
            foreach (var device in PairedList)
            {
                try
                {
                    if (await device.CheckConnection())
                    {
                        await device.ConnectDevice();
                    }
                }
                catch
                {
                    device.Status = DeviceStatus.Disconnected;
                }
            }
        }

        private async void UpdateModuleData()
        {
            try
            {
                foreach (var device in PairedList)
                {
                    try
                    {
                        await device.Sensor.UpdateData();
                    }
                    catch (Exception ex)
                    {
                        //Debug.WriteLine(ex.ToString());
                    }
                }
            }
            catch {}
        }

        private void SaveInfo()
        {
            try
            {
                SmallDBService.SaveDB();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void AddDeviceToDB(IoTDevice device)
        {
            try
            {
                var dic = new Dictionary<string, object>();
                dic.Add(DBConstant.DEVICE_NAME, device.DeviceName);
                dic.Add(DBConstant.BLUETOOTH_NAME, device.BTName);
                dic.Add(DBConstant.BLUEZ_PATH, device.Path);
                dic.Add(DBConstant.MAC_ADDRESS, device.MACAddress);
                dic.Add(DBConstant.SENSOR_TYPE, (int)device.SensorType);
                dic.Add(DBConstant.STATUS_ARG, string.Empty);
                dic.Add(DBConstant.DEVICE_STATUS, (int)device.Status);
                dic.Add(DBConstant.BT_SERVICE_UUID, device.ServiceUUID);
                dic.Add(DBConstant.BT_GATT_RX_UUID, device.RXUUID);
                dic.Add(DBConstant.BT_GATT_TX_UUID, device.TXUUID);

                SmallDBService.AddRow(dic);

                //SmallDBService.SaveDB();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}