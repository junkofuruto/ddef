using Client.UI.Statistics;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Client.UI.Pages
{
    public partial class DashboardPage : Page
    {
        private static long totalPackets = 0;
        private static long totalBadApplicationCauses = 0;
        private static long totalBadAddressesCauses = 0;

        private static DashboardPage? instance;

        private DashboardPage()
        {
            InitializeComponent();

            StatisticsCollector.OnStatisticsTimeskipSchedule += StatisticsCollectorOnStatisticsTimeskipSchedule;

            StartStatisticsTotalStatisticsImbedding();
        }

        private void StatisticsCollectorOnStatisticsTimeskipSchedule(StatisticsResultEventArgs e)
        {
            TotalPacketSeries.Values.Add(Convert.ToDouble(e.PacketsRecieved));
            BadIpCausesSeries.Values.Add(Convert.ToDouble(e.BadAddressCauses));
            BadAppCausesSeries.Values.Add(Convert.ToDouble(e.BadApplicationsCauses));

            if (TotalPacketSeries.Values.Count > 20)
            {
                TotalPacketSeries.Values.RemoveAt(0);
                BadIpCausesSeries.Values.RemoveAt(0);
                BadAppCausesSeries.Values.RemoveAt(0);
            }
        }

        public static DashboardPage Instance
        {
            get
            {
                if (instance == null)
                    instance = new DashboardPage();
                return instance;
            }
        }

        private async void StartStatisticsTotalStatisticsImbedding()
        {
            while (true)
            {
                IncreaseTotalPacketsCountValueGradually();
                IncreaseTotalBadAppsCountValueGradually();
                IncreaseTotalBadAddressesCountValueGradually();

                await Task.Delay(TimeSpan.FromSeconds(5));
            }
            
        }
        private async void IncreaseTotalPacketsCountValueGradually()
        {
            for (; totalPackets <= StatisticsCollector.PacketsRecieved.Select(x => x.Value).Sum(); ++totalPackets)
            {
                TotalPacketsCountTextBlock.Text = totalPackets.ToString().PadLeft(10, '0');
                await Task.Delay(10);
            }
        }
        private async void IncreaseTotalBadAppsCountValueGradually()
        {
            for (; totalBadApplicationCauses <= StatisticsCollector.BadApplicationsCauses.Select(x => x.Value).Sum(); ++totalBadApplicationCauses)
            {
                TotalBadAppsCountTextBlock.Text = totalBadApplicationCauses.ToString().PadLeft(10, '0');
                await Task.Delay(5);
            }
        }
        private async void IncreaseTotalBadAddressesCountValueGradually()
        {
            for (; totalBadAddressesCauses <= StatisticsCollector.BadAddressCauses.Select(x => x.Value).Sum(); ++totalBadAddressesCauses)
            {
                TotalBadAddressesTextBlock.Text = totalBadAddressesCauses.ToString().PadLeft(10, '0');
                await Task.Delay(5);
            }
        }

        private void TotalPacketsCheckBoxChecked(object sender, System.Windows.RoutedEventArgs e) 
            => TotalPacketSeries.Visibility = ((CheckBox)sender).IsChecked!.Value ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
        private void AppCausesCheckBoxChecked(object sender, System.Windows.RoutedEventArgs e) 
            => BadAppCausesSeries.Visibility = ((CheckBox)sender).IsChecked!.Value ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
        private void AddressCausesCheckBoxChecked(object sender, System.Windows.RoutedEventArgs e) 
            => BadIpCausesSeries.Visibility = ((CheckBox)sender).IsChecked!.Value ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
    }
}
