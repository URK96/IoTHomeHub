using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

using System;
using System.Threading.Tasks;


using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using Avalonia.Input.Raw;
using Avalonia.OpenGL;
using Avalonia.Platform;
using Avalonia.Rendering;
using Avalonia.Threading;
using Avalonia.X11.Glx;
using Avalonia.X11;

namespace IoTHubDevice.Views
{
    public class MainWindow : Window
    {
        private Window rootWindow;
        public MainWindow()
        {
            InitializeComponent();

            //this.HasSystemDecorations = false;
            //this.WindowState = WindowState.Maximized;
            //this.WindowState = WindowState.FullScreen; // this option only available Avalonia 0.10.0 or above
            //this.Topmost = true;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.CanResize = false;

            rootWindow.Content = new SplashView();

            _ = SplashProcess();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            AppEnvironment.mainWindow = rootWindow = this.FindControl<Window>("MainRootWindow");
        }

        private async Task SplashProcess()
        {
            await Task.Delay(5000);

            rootWindow.Content = new MainContainerView();
        }
    }
}