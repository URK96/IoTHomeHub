using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Animation;

using System;
using System.Threading.Tasks;

namespace IoTHubDevice.Views
{
    public class MainContainerView : UserControl
    {
        private TabControl mainTab;

        public MainContainerView()
        {
            InitializeComponent();

            //_ = PageProcess();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            mainTab = this.FindControl<TabControl>("MainTabControl");
        }
    }
}