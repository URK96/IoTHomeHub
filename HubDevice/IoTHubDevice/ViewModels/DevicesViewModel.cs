using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

using IoTHubDevice.Models;

namespace IoTHubDevice.ViewModels
{
    public class DevicesViewModel : ViewModelBase
    {
        public ObservableCollection<IoTDevice> DeviceList => AppEnvironment.deviceManager.PairedList;

        public DevicesViewModel()
        {
            
        }
    }
}
