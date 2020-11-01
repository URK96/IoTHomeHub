using System;

namespace IoTHubDevice.Models
{
    public static class IoTDeviceType
    {
        public enum DeviceType
        {
            Unknown = 0,
            HTSensor = 1,
            DustSensor,
            LightSensor
        }

        public interface IBaseType
        {
            void UpdateData();
        }

        public class HTSensor : IBaseType
        {
            public double Humidity { get; private set; }
            public double Temperature { get; private set; }

            public void UpdateData()
            {

            }
        }

        public class DustSensor : IBaseType
        {
            public double Dust { get; private set; }

            public void UpdateData()
            {
                
            }
        }

        public class LightSensor : IBaseType
        {
            public double Value { get; private set; }

            public void UpdateData()
            {

            }
        }
    }
}