using Avalonia;
using Avalonia.Controls;

using System;
using System.IO;

using IoTHubDevice.Services;

namespace IoTHubDevice
{
    public static class AppEnvironment
    {
        public const string weatherAPIRootURL = "https://openweathermap.org/";
        public static string weatherIconBaseURL => Path.Combine(weatherAPIRootURL, "img", "wn");

        public static Window mainWindow;
        public static IoTDeviceManager deviceManager;
        public static BTService btService;
        public static WeatherService weather;
    }
}