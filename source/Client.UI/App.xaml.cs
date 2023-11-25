using Client.Core.Analyzing.Address;
using Client.Core.Analyzing.Application;
using Client.Core.Logging;
using Client.Core.Monitoring;
using Client.Core.Monitoring.Utilities;
using Client.UI.Statistics;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Client.UI
{
    public partial class App : Application
    {
        public static NetworkInterface[]? NetworkInterfaces { get; private set; }
        public static Frame? Frame { get; set; }
        public static CancellationTokenSource MonitoringServerCancellationToken { get; private set; } = new CancellationTokenSource();

        public static void Configure()
        {
            NetworkInterfaces = NetworkConfiguration.GetAllInterfaces();
            AddressAnalyzer.Configure();
            ApplicationAnalyzer.Configure();
            StatisticsCollector.Configure();

            MonitoringServer.PacketHandler += ApplicationAnalyzer.Handler;
            MonitoringServer.PacketHandler += AddressAnalyzer.Handler;
        }

        public async static void StartMonitoringServer()
        {
            await MonitoringServer.Start("Ethernet 2", MonitoringServerCancellationToken.Token);
        }
    }
}
