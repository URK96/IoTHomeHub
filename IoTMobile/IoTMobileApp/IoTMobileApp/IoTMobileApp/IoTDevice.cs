using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Newtonsoft.Json.Linq;

using DBConstant = SmallDB.Constant.IoTDeviceDBConstant;
using TokenIndexes = SmallDB.Constant.IoTDeviceTokenIndex;

namespace IoTMobileApp
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
        public string StatusArgument { get; set; }

        public IoTDevice(DataRow dr)
        {
            DeviceName = dr[DBConstant.DEVICE_NAME] as string;
            BTName = dr[DBConstant.BLUETOOTH_NAME] as string;
            MACAddress = dr[DBConstant.MAC_ADDRESS] as string;
            Path = dr[DBConstant.BLUEZ_PATH] as string;
            SensorType = (DeviceType)dr[DBConstant.SENSOR_TYPE];
            Status = (DeviceStatus)dr[DBConstant.DEVICE_STATUS];
            StatusArgument = dr[DBConstant.STATUS_ARG] as string;
        }

        public IoTDevice(JToken token)
        {
            DeviceName = token[TokenIndexes.DEVICE_NAME].ToString();
            BTName = token[TokenIndexes.BLUETOOTH_NAME].ToString();
            MACAddress = token[TokenIndexes.MAC_ADDRESS].ToString();
            Path = token[TokenIndexes.BLUEZ_PATH].ToString();
            SensorType = (DeviceType)token[TokenIndexes.SENSOR_TYPE].ToObject<int>();
            Status = (DeviceStatus)token[TokenIndexes.DEVICE_STATUS].ToObject<int>();
            StatusArgument = token[TokenIndexes.STATUS_ARG].ToString();
        }

        public IoTDevice() { }
    }
}
