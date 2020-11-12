using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text;

using SmallDB;

using DBConstant = SmallDB.Constant.IoTDeviceDBConstant;

namespace IoTHubServer
{
    public enum DeviceType
    {
        Unknown = 0,
        HTSensor = 1,
        DustSensor,
        LightSensor
    }

    public enum DeviceStatus
    {
        Unknown = 0,
        Disconnected = 1,
        Connected
    }

    public class IoTDevice
    {
        public string DeviceName { get; set; }
        public string BTName { get; set; }
        public string MACAddress { get; set; }
        public string Path { get; set; }
        public DeviceType SensorType { get; set; }
        public DeviceStatus Status { get; set; }

        public IoTDevice(DataRow dr)
        {
            DeviceName = dr[DBConstant.DEVICE_NAME] as string;
            BTName = dr[DBConstant.BLUETOOTH_NAME] as string;
            MACAddress = dr[DBConstant.MAC_ADDRESS] as string;
            Path = dr[DBConstant.BLUEZ_PATH] as string;
            SensorType = (DeviceType)dr[DBConstant.SENSOR_TYPE];
            Status = (DeviceStatus)dr[DBConstant.DEVICE_STATUS];
        }
    }
}