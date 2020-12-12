using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace IoTMobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingPage : ContentPage
    {
        public SettingPage()
        {
            InitializeComponent();
        }

        private async void ServerIP_SettingCell_Tapped(object sender, EventArgs e)
        {
            var result = await DisplayPromptAsync("서버 IP 입력", "서버 IP를 입력하세요", "설정", "취소", null, -1, null, Preferences.Get(SettingConstants.SERVER_IP, string.Empty));

            AppEnvironment.serverRoot = $"http://{result}:8080/";

            Preferences.Set(SettingConstants.SERVER_IP, result);
        }
    }
}