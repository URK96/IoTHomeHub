using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using IoTHubDevice.Models;

namespace IoTHubDevice.Services
{
    public class IoTDeviceManager
    {
        public ObservableCollection<IoTDevice> PairedList { get; }
        public ObservableCollection<IoTDevice> FindedList { get; }

        public IoTDeviceManager()
        {
            PairedList = new ObservableCollection<IoTDevice>();
            FindedList = new ObservableCollection<IoTDevice>();
        }
    }
}