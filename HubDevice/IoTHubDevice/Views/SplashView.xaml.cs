using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

using System;
using System.IO;
using System.Data;
using System.Collections.Generic;

using IoTHubDevice.Services;

using SmallDB;

namespace IoTHubDevice.Views
{
    public class SplashView : UserControl
    {
        enum TestType
        {
            Type1 = 0,
            Type2 = 1,
            Type3,
            Type4
        }

        public SplashView()
        {
            InitializeComponent();

            AppEnvironment.btService = new BTService();
            AppEnvironment.deviceManager = new IoTDeviceManager();
            AppEnvironment.weather = new WeatherService();

            TestDB();

            // CheckDB();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void TestDB()
        {
            if (!File.Exists(SmallDBService.DeviceDBFile))
            {
                SmallDBService.CreateDB();
            }

            SmallDBService.AddColumn("TestString", string.Empty);
            SmallDBService.AddColumn("TestBoolean", false);
            SmallDBService.AddColumn("TestInt", 0);

            var dic = new Dictionary<string, object>();
            dic.Add("TestString", "String");
            dic.Add("TestBoolean", true);
            dic.Add("TestInt", TestType.Type4);

            SmallDBService.AddRow(dic);
        }

        private void CheckDB()
        {
            if (!File.Exists(SmallDBService.DeviceDBFile))
            {
                SmallDBService.CreateDB();
            }
            else
            {
                SmallDBService.LoadDB();
            }
        }

        private void CreateDBSchemas()
        {
            SmallDBService.AddColumn("DeviceName", string.Empty);
            SmallDBService.AddColumn("BTName", string.Empty);
            SmallDBService.AddColumn("MACAddress", string.Empty);
            SmallDBService.AddColumn("SensorType", 0);
            SmallDBService.AddColumn("DeviceStatus", 0);
        }
    }
}