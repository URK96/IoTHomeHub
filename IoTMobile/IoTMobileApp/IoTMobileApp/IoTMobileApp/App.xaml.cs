using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace IoTMobileApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            InitializePreferences();

            AppEnvironment.serverRoot = $"http://{Preferences.Get(SettingConstants.SERVER_IP, string.Empty)}:8080/";

            MainPage = new MainPage();
        }

        private void InitializePreferences()
        {
            if (!Preferences.ContainsKey(SettingConstants.SERVER_IP))
            {
                Preferences.Set(SettingConstants.SERVER_IP, "0.0.0.0");
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
