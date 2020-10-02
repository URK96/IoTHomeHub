using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;

using System;

namespace IoTHubDevice.Views
{
    public class MainView : UserControl
    {
        private TextBlock clockHour;
        private TextBlock clockMin;
        private TextBlock clockSec;

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

            clockHour = this.FindControl<TextBlock>("MainClockHour");
            clockMin = this.FindControl<TextBlock>("MainClockMin");
            clockSec = this.FindControl<TextBlock>("MainClockSec");
        }

        private void UpdateClock(object sender, EventArgs e)
        {
            var dt = DateTime.Now;

            clockHour.Text = dt.Hour.ToString();
            clockMin.Text = dt.Minute.ToString();
            clockSec.Text = dt.Second.ToString();
        }
    }
}