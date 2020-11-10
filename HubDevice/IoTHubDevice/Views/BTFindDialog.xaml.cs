using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Markup.Xaml;

using AvaloniaProgressRing;

using System;

using IoTHubDevice.ViewModels;

namespace IoTHubDevice.Views
{
    public class BTFindDialog : Window
    {
        private ProgressRing searchStatusRing;        

        public BTFindDialog()
        {
            InitializeComponent();

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            DataContext = new BTFindDialogViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            searchStatusRing = this.FindControl<ProgressRing>("SearchStatusRing");

            AppEnvironment.btService.BTFindStart += delegate { searchStatusRing.IsActive = true; };
            AppEnvironment.btService.BTFindEnd += delegate { searchStatusRing.IsActive = false; };

            this.Closed += delegate
            {
                AppEnvironment.btService.BTFindStart -= delegate { searchStatusRing.IsActive = true; };
                AppEnvironment.btService.BTFindEnd -= delegate { searchStatusRing.IsActive = false; };
            };
        }

        private async void FindButtonClick(object sender, RoutedEventArgs e)
        {
            await AppEnvironment.btService.FindDevices();
        }

        private void PairedBTItemLayoutPointerEnter(object sender, PointerEventArgs e)
        {
            ((StackPanel)sender).Background = Brushes.DarkGray;
        }

        private void PairedBTItemLayoutPointerLeave(object sender, PointerEventArgs e)
        {
            ((StackPanel)sender).Background = Brushes.Black;
        }
    }
}