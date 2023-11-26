using Client.Core.Analyzing.DataAccess.Entities;
using Client.Core.Monitoring.Utilities;
using Client.UI.Pages;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;
using Microsoft.PowerShell.Commands;

namespace Client.UI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Task.Run(() => Configure("Ethernet 2")).Wait();
            App.Frame = MainWindowFrame;
            App.NetworkInterfaces = NetworkConfiguration.GetAllInterfaces();
        }

        public static void UpdateData(MainWindow window)
        {
            window.ProfileTextBlock.Text = $"{User.Current?.FirstName.ToCharArray().First()}{User.Current?.LastName.ToCharArray().First()}";
            window.UsernameTextBlock.Text = $"@{User.Current?.Username}";
            window.NameTextBlock.Text = $"{User.Current?.FirstName} {User.Current?.LastName}";
        }

        private async void Configure(string adapterName)
        {
            await Dispatcher.InvokeAsync(async () =>
            {
                try
                {
                    await App.Configure();
                    App.StartMonitoringServer(adapterName);
                    UpdateData(this);
                    App.Frame.Navigate(AccountPage.Instance);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
        }

        private void NavigateDashboardClick(object sender, RoutedEventArgs e)
        {
            try
            {
                App.Frame!.Navigate(DashboardPage.Instance);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void NavigateConsoleClick(object sender, RoutedEventArgs e)
        {
            try
            {
                App.Frame!.Navigate(ConsolePage.Instance);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void NavigateAccountClick(object sender, RoutedEventArgs e) => App.Frame!.Navigate(AccountPage.Instance);
        private async void ExitClick(object sender, RoutedEventArgs e)
        {
            await User.Current?.DropTokenAsync();
            Close();
        }

        private void MainWindowsMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            } catch { }
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

        private void AdaptersComboBoxSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
