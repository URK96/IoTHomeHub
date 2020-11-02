using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;

using System;
using System.Drawing;
using System.Net;
using System.IO;

namespace IoTHubDevice.Views
{
    public class MainView : UserControl
    {
        private TextBlock dateText;
        private TextBlock dateWeek;
        private StackPanel clockLayout;
        private TextBlock clockHour;
        private TextBlock clockMin;
        private Ellipse clockPoint;
        private StackPanel weatherLayout;
        private Avalonia.Controls.Image weatherIcon;
        private TextBlock weatherTemp;

        private DispatcherTimer clockTimer;
        private DispatcherTimer weatherTimer;

        public MainView()
        {
            InitializeComponent();

            clockTimer.Start();
            weatherTimer.Start();
        }

        private void InitializeComponent()
        {
            clockTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(1000)
            };
            clockTimer.Tick += UpdateClock;

            weatherTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };
            weatherTimer.Tick += UpdateWeather;

            AvaloniaXamlLoader.Load(this);

            dateText = this.FindControl<TextBlock>("MainDateText");
            dateWeek = this.FindControl<TextBlock>("MainDateWeek");

            clockLayout = this.FindControl<StackPanel>("MainClockLayout");
            clockHour = this.FindControl<TextBlock>("MainClockHour");
            clockMin = this.FindControl<TextBlock>("MainClockMin");
            clockPoint = this.FindControl<Ellipse>("MainClockPoint");

            weatherLayout = this.FindControl<StackPanel>("MainWeatherLayout");
            weatherIcon = this.FindControl<Avalonia.Controls.Image>("WeatherIcon");
            weatherTemp = this.FindControl<TextBlock>("WeatherTemperature");
        }

        private void UpdateClock(object sender, EventArgs e)
        {
            var dt = DateTime.Now;

            dateText.Text = $"{dt.Year}.{dt.Month}.{dt.Day}";
            dateWeek.Text = dt.DayOfWeek.ToString();

            clockHour.Text = dt.Hour.ToString("D2");
            clockMin.Text = dt.Minute.ToString("D2");
            //clockSec.Text = dt.Second.ToString();

            clockLayout.Opacity = 1.0;
        }

        private async void UpdateWeather(object sender, EventArgs e)
        {
            await AppEnvironment.weather.UpdateData("Incheon, KR");
            
            using (var wc = new WebClient())
            {
                string icon = AppEnvironment.weather.Data.Weathers[0].Icon;

                if (!File.Exists($"{icon}.png"))
                {
                    await wc.DownloadFileTaskAsync(System.IO.Path.Combine(AppEnvironment.weatherIconBaseURL, $"{icon}.png"), $"{icon}.png");
                }

                weatherIcon.Source = new Avalonia.Media.Imaging.Bitmap($"{icon}.png");
            }
            weatherTemp.Text = $"{AppEnvironment.weather.Data.Main.Temperature.CelsiusCurrent:F1}℃";

            weatherLayout.Opacity = 1.0;
        }
    }
}