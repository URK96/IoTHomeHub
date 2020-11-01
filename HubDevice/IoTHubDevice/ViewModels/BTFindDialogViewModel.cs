using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

using ReactiveUI;

using IoTHubDevice.Models;
using IoTHubDevice.Services;

namespace IoTHubDevice.ViewModels
{
    public class BTFindDialogViewModel : ViewModelBase
    {
        public ObservableCollection<IoTDevice> SearchDevices => AppEnvironment.deviceManager.FindedList;

        public BTFindDialogViewModel()
        {
            
        }
    }
}
