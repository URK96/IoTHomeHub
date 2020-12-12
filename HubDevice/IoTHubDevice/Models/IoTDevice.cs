using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text;

using HashtagChris.DotNetBlueZ;
using HashtagChris.DotNetBlueZ.Extensions;

using DBConstant = SmallDB.Constant.IoTDeviceDBConstant;

namespace IoTHubDevice.Models
{
    public class IoTDevice : Device
    {
        public Device BaseDevice { get; set; }
        public string DeviceName { get; set; }
        public string BTName { get; set; }
        public string MACAddress { get; set; }
        public string Path { get; set; }
        public IoTDeviceType.DeviceType SensorType { get; set; }
        public IoTDeviceType.BaseType Sensor { get; set; }
        public DeviceStatus Status { get; set; }
        public string ServiceUUID { get; set; }
        public string TXUUID { get; set; }
        public string RXUUID { get; set; }
        public IGattService1 GattService { get; set; }
        public GattCharacteristic RXCharacteristic { get; set; }
        public GattCharacteristic TXCharacteristic { get; set; }

        public string DeviceTypeString => SensorType switch
        {
            IoTDeviceType.DeviceType.HTSensor => "HT Sensor",
            IoTDeviceType.DeviceType.DustSensor => "Dust Sensor",
            IoTDeviceType.DeviceType.LightSensor => "Light Sensor",
            IoTDeviceType.DeviceType.GasSensor => "Gas Sensor",
            _ => "Unknown"
        };

        private Dictionary<string, object> btOption;

        public IoTDevice(DataRow dr)
        {
            DeviceName = dr[DBConstant.DEVICE_NAME] as string;
            BTName = dr[DBConstant.BLUETOOTH_NAME] as string;
            MACAddress = dr[DBConstant.MAC_ADDRESS] as string;
            SensorType = (IoTDeviceType.DeviceType)dr[DBConstant.SENSOR_TYPE];
            Status = DeviceStatus.Disconnected;
            ServiceUUID = dr[DBConstant.BT_SERVICE_UUID] as string;
            RXUUID = dr[DBConstant.BT_GATT_RX_UUID] as string;
            TXUUID = dr[DBConstant.BT_GATT_TX_UUID] as string;

            FindDevice(MACAddress);
        }

        public IoTDevice(Device device)
        {
            BaseDevice = device;
            Status = DeviceStatus.Disconnected;
            BTName = "ED-BT 52810";
            Path = device.ObjectPath.ToString();
            
            var paths = Path.Split('/');
            var macTemp = paths[paths.Length - 1].Replace('_', ':');
            MACAddress = macTemp.Remove(0, 4);
            
            InitializeInfo();
            InitializeConnectInfo();
        }

        private void InitializeInfo()
        {
            ServiceUUID = "6e400001-b5a3-f393-e0a9-e50e24dcca9e"; //(await BaseDevice.GetUUIDsAsync())[2];
            RXUUID = "6e400003-b5a3-f393-e0a9-e50e24dcca9e";
            TXUUID = "6e400002-b5a3-f393-e0a9-e50e24dcca9e";
        }

        private void InitializeTypeClass()
        {
            switch (SensorType)
            {
                case IoTDeviceType.DeviceType.HTSensor:
                    Sensor = new IoTDeviceType.HTSensor(this);
                    break;
                case IoTDeviceType.DeviceType.DustSensor:
                    Sensor = new IoTDeviceType.DustSensor(this);
                    break;
                case IoTDeviceType.DeviceType.LightSensor:
                    Sensor = new IoTDeviceType.LightSensor(this);
                    break;
                case IoTDeviceType.DeviceType.GasSensor:
                    Sensor = new IoTDeviceType.GasSensor(this);
                    break;
                default:
                    Sensor = null;
                    break;
            }
        }

        private void InitializeConnectInfo()
        {
            Connected += async delegate
            {
                Status = DeviceStatus.Connected;

                Debug.WriteLine("Connect device success");
            };
            Disconnected += async delegate
            {
                Status = DeviceStatus.Disconnected;

                Debug.WriteLine("Disconnect device success");
            };

            btOption = new Dictionary<string, object>();
            btOption.Add("device", $"{BaseDevice.ObjectPath}");
        }

        private async void FindDevice(string mac)
        {
            mac = mac.Replace(':', '_');

            var devices = await AppEnvironment.btService.BTAdapter.GetDevicesAsync();
            var result = from device in devices
                        where device.ObjectPath.ToString().Contains(mac)
                        select device;

            BaseDevice = result.FirstOrDefault();

            InitializeTypeClass();
            InitializeConnectInfo();
        }

        public async Task<bool> CheckConnection()
        {
            try
            {
                await BaseDevice.ConnectAsync();

                Status = DeviceStatus.Connected;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                Status = DeviceStatus.Disconnected;

                return false;
            }

            return true;
        }

        public async Task ConnectDevice()
        {
            try
            {
                // Connect device
                await BaseDevice.ConnectAsync();

                Debug.WriteLine("Get Gatt service...");


                // Get gatt service
                GattService = await BaseDevice.GetServiceAsync(ServiceUUID);

                if (GattService == null)
                {
                    Debug.WriteLine("Get Gatt service fail");

                    throw new Exception("Get Gatt service fail");
                }
                else
                {
                    Debug.WriteLine("Get Gatt service success");
                }


                // Get gatt tx characteristic
                Debug.WriteLine("Get Gatt tx characteristic...");

                TXCharacteristic = await GattService.GetCharacteristicAsync(TXUUID);

                if (TXCharacteristic == null)
                {
                    Debug.WriteLine("Get Gatt TX characteristic fail");

                    throw new Exception("Get Gatt TX fail");
                }
                else
                {
                    Debug.WriteLine("Get Gatt TX characteristic success");
                }


                // Get gatt rx characteristic
                Debug.WriteLine("Get Gatt rx characteristic...");

                RXCharacteristic = await GattService.GetCharacteristicAsync(RXUUID);

                if (RXCharacteristic == null)
                {
                    Debug.WriteLine("Get Gatt RX Characteristic fail");

                    throw new Exception("Get Gatt RX fail");
                }
                else
                {
                    Debug.WriteLine("Get RX characteristic success");
                }

                RXCharacteristic.Value += async (RXCharacteristic, e) =>
                {
                    try
                    {

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }
                };

                Status = DeviceStatus.Connected;

                await Task.Delay(500);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());

                Status = DeviceStatus.Disconnected;
            }
        }

        public async Task<bool> SendCommand(string command)
        {
            try
            {
                if (TXCharacteristic == null)
                {
                    Console.WriteLine("No TX Characteristic");

                    return false;
                    // throw new Exception("No TX Characteristic");
                }

                Debug.WriteLine($"Write command to {DeviceName} : {command}");

                await TXCharacteristic.WriteValueAsync(Encoding.UTF8.GetBytes(command), btOption);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());

                return false;
            }

            return true;
        }

        public async Task<string> ReceiveResponse()
        {
            const int delay = 2000;

            string response = string.Empty;

            try
            {
                await Task.Delay(delay);

                if (RXCharacteristic == null)
                {
                    Console.WriteLine("No RX Characteristic");

                    return null;
                }

                Debug.WriteLine($"Read response from {DeviceName}");

                var result = await RXCharacteristic.ReadValueAsync(btOption);

                for (int i = 0; (i < 5) && (result.Length == 0); ++i)
                {
                    Debug.WriteLine($"Retry read...{i + 1}");
                    
                    result = await RXCharacteristic.ReadValueAsync(btOption);

                    await Task.Delay(delay);
                }

                if (result.Length == 0)
                {
                    throw new Exception("Cannot receive any response");
                }
                else
                {
                    Debug.Write($"Receive {result.Length} bytes : ");
                    response = Encoding.UTF8.GetString(result);
                    Debug.WriteLine(response);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
            }

            return response;
        }
    }
}