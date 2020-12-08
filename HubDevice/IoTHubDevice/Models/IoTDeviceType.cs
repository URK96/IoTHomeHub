using System;

using SmallDB;

using DBConstant = SmallDB.Constant.IoTDeviceDBConstant;

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

            public void UpdateDB(string arg)
            {
                var dr = SmallDBService.FindDataRow(DBConstant.MAC_ADDRESS, device.MACAddress);

                if (dr != null)
                {
                    SmallDBService.EditRowCell(dr, DBConstant.STATUS_ARG, arg);
                }
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

            public override async void UpdateData()
            {
                if (await device.SendCommand("Data"))
                {
                    var data = await device.ReceiveResponse();
                    var datas = data.Split(';');

                    if (datas.Length == 2)
                    {
                        UpdateDB(data);

                        Humidity = double.Parse(datas[0]);
                        Temperature = double.Parse(datas[1]);
                    }
                }
            }
        }

        public class DustSensor : BaseType
        {
            public int Level { get; private set; }
            public double Dust { get; private set; }

            public DustSensor(IoTDevice device) : base(device)
            {

            }

            public override async void UpdateData()
            {
                if (await device.SendCommand("Data"))
                {
                    var data = await device.ReceiveResponse();
                    var datas = data.Split(';');

                    if (datas.Length == 2)
                    {
                        UpdateDB(data);

                        Level = int.Parse(datas[0]);
                        Dust = double.Parse(datas[1]);
                    }
                }
            }
        }

        public class LightSensor : BaseType
        {
            public double LightValue { get; private set; }

            public LightSensor(IoTDevice device) : base(device)
            {

            }

            public override async void UpdateData()
            {
                if (await device.SendCommand("Data"))
                {
                    var data = await device.ReceiveResponse();
                    var datas = data.Split(';');

                    if (datas.Length == 1)
                    {
                        UpdateDB(data);

                        LightValue = double.Parse(datas[0]);
                    }
                }
            }
        }
    }
}