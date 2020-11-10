using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text;

using SmallDB;

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
            DeviceName = dr["DeviceName"] as string;
            BTName = dr["BTName"] as string;
            MACAddress = dr["MACAddress"] as string;
            Path = dr["Path"] as string;
            SensorType = (DeviceType)dr["SensorType"];
            Status = (DeviceStatus)dr["DeviceStatus"];
        }
    }
}