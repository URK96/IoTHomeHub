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

        public abstract class BaseType
        {
            public IoTDevice device;

            public BaseType(IoTDevice device)
            {
                this.device = device;
            }

            public abstract void UpdateData();
        }

        public class HTSensor : BaseType
        {
            public double Humidity { get; private set; }
            public double Temperature { get; private set; }

            public HTSensor(IoTDevice device) : base(device) 
            { 

            }

            public override void UpdateData()
            {

            }
        }

        public class DustSensor : BaseType
        {
            public double Dust { get; private set; }

            public DustSensor(IoTDevice device) : base(device)
            {

            }

            public override void UpdateData()
            {
                
            }
        }

        public class LightSensor : BaseType
        {
            public double Value { get; private set; }

            public LightSensor(IoTDevice device) : base(device)
            {

            }

            public override void UpdateData()
            {

            }
        }
    }
}