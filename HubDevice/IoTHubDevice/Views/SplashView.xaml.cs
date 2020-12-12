using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Diagnostics;

using IoTHubDevice.Services;

using SmallDB;

using DBConstant = SmallDB.Constant.IoTDeviceDBConstant;

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
            AppEnvironment.commandManager = new CommandManager();

            // TestDB();

            CheckDB();
            AppEnvironment.btService.LoadPairedDevices();
            AppEnvironment.deviceManager.ConnectPairedDevices();

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
                CreateDBSchemas();
                SmallDBService.SaveDB();
            }
            else
            {
                SmallDBService.LoadDB();
            }
        }

        private void CreateDBSchemas()
        {
            SmallDBService.AddColumn(DBConstant.DEVICE_NAME, string.Empty);
            SmallDBService.AddColumn(DBConstant.BLUETOOTH_NAME, string.Empty);
            SmallDBService.AddColumn(DBConstant.BLUEZ_PATH, string.Empty);
            SmallDBService.AddColumn(DBConstant.MAC_ADDRESS, string.Empty);
            SmallDBService.AddColumn(DBConstant.SENSOR_TYPE, 0);
            SmallDBService.AddColumn(DBConstant.DEVICE_STATUS, 0);
            SmallDBService.AddColumn(DBConstant.STATUS_ARG, string.Empty);
            SmallDBService.AddColumn(DBConstant.BT_SERVICE_UUID, string.Empty);
            SmallDBService.AddColumn(DBConstant.BT_GATT_RX_UUID, string.Empty);
            SmallDBService.AddColumn(DBConstant.BT_GATT_TX_UUID, string.Empty);
        }
    }
}