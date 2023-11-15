using Client.Core.Logging;
using Client.Core.Monitoring.Protocol;
using Client.Core.Monitoring.Utilities;

namespace Client.Core.Analyzing.Address;

public static class AddressAnalyzer
{
    private static Logger logger = new Logger("Analyzing.Address.AddressAnalyzer");

    public static event BadAddressEventHandler? OnBadAddress;

    public async static void Handler(AnalyzerEventArgs e)
    {
        await CheckPacketAdresses(e.Packet);
    }

    private async static Task CheckPacketAdresses(Packet packet)
    {
        bool isBadDestinationAddress = AddressServerDataService.Find(packet.DestinationEndPoint!.Address);
        bool isBadSourceAddress = AddressServerDataService.Find(packet.SourceEndPoint!.Address);

        if (isBadDestinationAddress || isBadSourceAddress)
        {
            var result = await AddressServerDataService.GetAddressInformation(isBadDestinationAddress ? packet.DestinationEndPoint!.Address : packet.SourceEndPoint!.Address);
            OnBadAddress?.Invoke(new BadAddressEventArgs
            {
                Address = isBadDestinationAddress ? packet.DestinationEndPoint!.Address : packet.SourceEndPoint!.Address,
                Source = isBadDestinationAddress ? BadAddressSource.Into : BadAddressSource.From,
                Reason = BadAddressReason.Unknown
            });
        }
    }
}