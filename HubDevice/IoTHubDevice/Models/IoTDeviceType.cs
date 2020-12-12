using System;
using System.Timers;
using System.Text;
using System.Threading.Tasks;

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
            LightSensor,
            GasSensor
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

            public abstract Task UpdateData();
            public abstract string GetInfoString();
        }

        public class HTSensor : BaseType
        {
            public double Humidity { get; private set; }
            public double Temperature { get; private set; }
            public bool FanStatus { get; private set; }

            public HTSensor(IoTDevice device) : base(device) 
            { 

            }

            public override async Task UpdateData()
            {
                try
                {
                    if (await device.SendCommand("Data"))
                    {
                        var data = await device.ReceiveResponse();
                        var datas = data.Split(';');

                        if (datas.Length == 3)
                        {
                            UpdateDB(data);

                            Temperature = double.Parse(datas[0]);
                            Humidity = double.Parse(datas[1]);
                            FanStatus = int.Parse(datas[2]) == 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            public override string GetInfoString()
            {
                var sb = new StringBuilder();

                sb.AppendLine($"{Temperature}℃ / {Humidity}%");
                sb.Append($"Fan : {(FanStatus ? "On" : "Off")}");

                return sb.ToString();
            }
        }

        public class DustSensor : BaseType
        {
            public int Level { get; private set; }
            public double Dust { get; private set; }
            public bool FanStatus { get; private set; }

            public DustSensor(IoTDevice device) : base(device)
            {

            }

            public override async Task UpdateData()
            {
                if (await device.SendCommand("Data"))
                {
                    var data = await device.ReceiveResponse();
                    var datas = data.Split(';');

                    if (datas.Length == 3)
                    {
                        UpdateDB(data);

                        Level = int.Parse(datas[0]);
                        Dust = double.Parse(datas[1]);
                        FanStatus = int.Parse(datas[2]) == 1;
                    }
                }
            }

            public override string GetInfoString()
            {
                var sb = new StringBuilder();

                sb.AppendLine($"{Dust}ug/m^3 / Level {Level}");
                sb.Append($"Fan : {(FanStatus ? "On" : "Off")}");

                return sb.ToString();
            }
        }

        public class LightSensor : BaseType
        {
            public double LightValue { get; private set; }
            public bool LEDStatus { get; private set; }

            public LightSensor(IoTDevice device) : base(device)
            {

            }

            public override async Task UpdateData()
            {
                if (await device.SendCommand("Data"))
                {
                    var data = await device.ReceiveResponse();
                    var datas = data.Split(';');

                    if (datas.Length == 2)
                    {
                        UpdateDB(data);

                        LightValue = double.Parse(datas[0]);
                        LEDStatus = int.Parse(datas[1]) == 1;
                    }
                }
            }

            public override string GetInfoString()
            {
                var sb = new StringBuilder();

                sb.AppendLine($"Light : {LightValue}");
                sb.Append($"LED : {(LEDStatus ? "On" : "Off")}");

                return sb.ToString();
            }
        }

        public class GasSensor : BaseType
        {
            public int Level { get; private set; }
            public int Value { get; private set; }

            public GasSensor(IoTDevice device) : base(device)
            {

            }

            public override async Task UpdateData()
            {
                if (await device.SendCommand("Data"))
                {
                    var data = await device.ReceiveResponse();
                    var datas = data.Split(';');

                    if (datas.Length == 2)
                    {
                        UpdateDB(data);

                        Level = int.Parse(datas[0]);
                        Value = int.Parse(datas[1]);
                    }
                }
            }

            public override string GetInfoString()
            {
                var sb = new StringBuilder();

                sb.AppendLine($"CO2 PPM : {Value}");
                sb.Append($"Level : {Level}");

                return sb.ToString();
            }
        }
    }
}