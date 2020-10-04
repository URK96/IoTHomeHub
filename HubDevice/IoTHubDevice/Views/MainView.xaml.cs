using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;

using System;

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

        private DispatcherTimer clockTimer;

        public MainView()
        {
            InitializeComponent();

            clockTimer.Start();
        }

        private void InitializeComponent()
        {
            clockTimer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromMilliseconds(1000)
            };
            clockTimer.Tick += UpdateClock;

            AvaloniaXamlLoader.Load(this);

            dateText = this.FindControl<TextBlock>("MainDateText");
            dateWeek = this.FindControl<TextBlock>("MainDateWeek");

            clockLayout = this.FindControl<StackPanel>("MainClockLayout");
            clockHour = this.FindControl<TextBlock>("MainClockHour");
            clockMin = this.FindControl<TextBlock>("MainClockMin");
            clockPoint = this.FindControl<Ellipse>("MainClockPoint");
        }

        private void UpdateClock(object sender, EventArgs e)
        {
            var dt = DateTime.Now;

            dateText.Text = $"{dt.Year}.{dt.Month}.{dt.Day}";
            dateWeek.Text = dt.DayOfWeek.ToString();

            clockHour.Text = dt.Hour.ToString();
            clockMin.Text = dt.Minute.ToString("D2");
            //clockSec.Text = dt.Second.ToString();

            clockLayout.Opacity = 1.0;
        }
    }
}