using Client.Core.Analyzing.Address;
using Client.Core.Analyzing.Application;
using Client.Core.Analyzing.DataAccess.Entities;
using Client.Core.Monitoring;
using Client.UI.Statistics;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Client.UI
{
    public partial class App : Application
    {
        public static NetworkInterface[]? NetworkInterfaces { get; set; }
        public static Frame? Frame { get; set; }
        public static Frame? AuthentificationFrame { get; set; }
        public static CancellationTokenSource MonitoringServerCancellationToken { get; private set; } = new CancellationTokenSource();

        public static async Task Configure()
        {
            if (User.Current.Plan.Id != 0)
            {
                AddressAnalyzer.Configure();
                ApplicationAnalyzer.Configure();
                StatisticsCollector.Configure();

                MonitoringServer.PacketHandler += ApplicationAnalyzer.Handler;
                MonitoringServer.PacketHandler += AddressAnalyzer.Handler;
            }
        }

        public async static void StartMonitoringServer(string adapterName)
        {
            await MonitoringServer.Start(adapterName, MonitoringServerCancellationToken.Token);
        }
    }
}
