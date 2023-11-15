using Client.Core.Analyzing.Application;
using Client.Core.Analyzing.Address;
using Client.Core.Monitoring;

namespace Client.Application;

public static class Program
{
    public static CancellationTokenSource ProgramCancellationToken { get; private set; } = new CancellationTokenSource();

    public async static Task Main(string[] args)
    {
        MonitoringServer.OnPacketIncoming += ApplicationAnalyzer.Handler;
        MonitoringServer.OnPacketIncoming += AddressAnalyzer.Handler;
        await MonitoringServer.Start("Ethernet 2", ProgramCancellationToken.Token);
    }
}