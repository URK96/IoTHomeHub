using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Timers;

using IoTHubDevice.Models;

using SmallDB;

namespace IoTHubDevice.Services
{
    public class IoTDeviceManager
    {
        public ObservableCollection<IoTDevice> PairedList { get; }
        public ObservableCollection<IoTDevice> FindedList { get; }

        private Timer saveInfoTimer;
        private Timer refreshConnectionTimer;

        public IoTDeviceManager()
        {
            PairedList = new ObservableCollection<IoTDevice>();
            FindedList = new ObservableCollection<IoTDevice>();

            saveInfoTimer = new Timer(2000);
            saveInfoTimer.Elapsed += delegate { SaveInfo(); };
            saveInfoTimer.Stop();

            refreshConnectionTimer = new Timer(5000);
            refreshConnectionTimer.Elapsed += delegate { RefreshConnection(); };
            refreshConnectionTimer.Stop();
        }

        public async void ConnectPairedDevices()
        {
            foreach (var device in PairedList)
            {
                await device.ConnectDevice();
            }

            saveInfoTimer.Start();
            refreshConnectionTimer.Start();
        }

        private async void RefreshConnection()
        {
            foreach (var device in PairedList)
            {
                await device.CheckConnection();
            }
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
    }
}