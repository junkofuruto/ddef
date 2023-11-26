using Client.Core.Analyzing.DataAccess.Entities;
using Client.Core.Logging;
using Client.Core.Monitoring;
using Client.Core.Monitoring.Utilities;
using Client.UI.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client.UI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Task.Run(App.Configure);
            App.StartMonitoringServer();

            InitializeComponent();

            ProfileTextBlock.Text = $"{User.Current?.FirstName.ToCharArray().First()}{User.Current?.LastName.ToCharArray().First()}";
            UsernameTextBlock.Text = $"@{User.Current?.Username}";
            NameTextBlock.Text = $"{User.Current?.FirstName} {User.Current?.LastName}";

            App.Frame = MainWindowFrame;
            App.Frame.Navigate(ConsolePage.Instance);
        }

        private void NavigateDashboardClick(object sender, RoutedEventArgs e) => App.Frame!.Navigate(DashboardPage.Instance);
        private void NavigateConsoleClick(object sender, RoutedEventArgs e) => App.Frame!.Navigate(ConsolePage.Instance);
        private void NavigateAccountClick(object sender, RoutedEventArgs e) => App.Frame!.Navigate(AccountPage.Instance);
        private async void ExitClick(object sender, RoutedEventArgs e)
        {
            await User.Current?.DropTokenAsync();
            Close();
        }

        private void MainWindowsMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void MainWindowsMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point currentPosition = e.GetPosition(this);
                Left = currentPosition.X + Left - Mouse.GetPosition(this).X;
                Top = currentPosition.Y + Top - Mouse.GetPosition(this).Y;
            }
        }
    }
}
