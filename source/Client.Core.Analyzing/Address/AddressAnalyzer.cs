using Client.Core.Logging;
using Client.Core.Monitoring.Protocol;
using Client.Core.Monitoring.Utilities;

namespace Client.Core.Analyzing.Address;

public static class AddressAnalyzer
{
    private static Logger logger = new Logger("Analyzing.Address.AddressAnalyzer");

    public static event BadAddressEventHandler? OnBadAddress;

    public static void Configure()
    {
        AddressServerDataService.StartUpdateSchedule();
    }

    public async static void Handler(AnalyzerEventArgs e)
    {
        await CheckPacketAdresses(e.Packet);
    }

    private async static Task CheckPacketAdresses(Packet packet)
    {
        await CheckPacketSourceAddress(packet);
        await CheckPacketDestinationAddress(packet);
    }

    private async static Task CheckPacketSourceAddress(Packet packet)
    {
        var result = await AddressServerDataService.Find(packet.SourceEndPoint!.Address);
        if (result != null && OnBadAddress != null)
        {
            logger.Warning("Found bad address");
            OnBadAddress.Invoke(result);
        }
    }
    private async static Task CheckPacketDestinationAddress(Packet packet)
    {
        var result = await AddressServerDataService.Find(packet.DestinationEndPoint!.Address);
        if (result != null && OnBadAddress != null)
        {
            logger.Warning("Found bad address");
            OnBadAddress.Invoke(result);
        }
    }
}