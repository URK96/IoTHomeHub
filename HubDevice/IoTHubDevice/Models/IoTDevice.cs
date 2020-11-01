using System;
using System.Threading.Tasks;
using System.Diagnostics;

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
        public IoTDeviceType.IBaseType Sensor { get; set; }
        public DeviceStatus Status { get; set; }
        public string ServiceUUID { get; set; }
        public string TXUUID { get; set; }
        public string RXUUID { get; set; }
        public IGattService1 GattService { get; set; }
        public GattCharacteristic RXCharacteristic { get; set; }
        public GattCharacteristic TXCharacteristic { get; set; }

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

                await Task.Delay(500);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}