using Avalonia;
using Avalonia.Controls;

using System;

using IoTHubDevice.Services;

namespace IoTHubDevice
{
    public static class AppEnvironment
    {
        public static Window mainWindow;
        public static IoTDeviceManager deviceManager;
        public static BTService btService;
        public static WeatherService weather;
    }
}