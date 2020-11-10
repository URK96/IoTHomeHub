using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text;

using HashtagChris.DotNetBlueZ;
using HashtagChris.DotNetBlueZ.Extensions;

namespace IoTHubDevice.Models
{
    public class IoTDevice : Device
    {
        public Device BaseDevice { get; }
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


        private Dictionary<string, object> btOption;

        public IoTDevice(Device device)
        {
            BaseDevice = device;

            Status = DeviceStatus.Disconnected;

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

                    return;
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

                    return;
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

                    return;
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

                await Task.Delay(500);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        public async Task<bool> SendCommand(string command)
        {
            try
            {
                if (TXCharacteristic == null)
                {
                    throw new Exception("No TX Characteristic");
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
            string response = string.Empty;

            try
            {
                if (RXCharacteristic == null)
                {
                    throw new Exception("No RX Characteristic");
                }

                Debug.WriteLine($"Read response from {DeviceName}");

                var result = await RXCharacteristic.ReadValueAsync(btOption);

                for (int i = 0; (i < 5) && (result.Length == 0); ++i)
                {
                    Debug.WriteLine($"Retry read...{i + 1}");
                    
                    result = await RXCharacteristic.ReadValueAsync(btOption);

                    await Task.Delay(1000);
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