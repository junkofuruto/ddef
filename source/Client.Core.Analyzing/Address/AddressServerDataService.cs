using Client.Core.Logging;
using System.Buffers.Binary;
using System.Net;

namespace Client.Core.Analyzing.Address;

internal static class AddressServerDataService
{
    private static Logger logger = new Logger("Analyzing.Address.AddressServerDataService");
    private static HashSet<BadAddressEventArgs> badAddressesInfo = new HashSet<BadAddressEventArgs>();

    public async static Task<BadAddressEventArgs?> Find(IPAddress address)
    {
        return await Task.Run(() =>
        {
            if (badAddressesInfo.Any(x => x.Address!.GetAddressBytes().SequenceEqual(address.GetAddressBytes())))
            {
                var value = badAddressesInfo.Where(x => x.Address!.GetAddressBytes().SequenceEqual(address.GetAddressBytes())).FirstOrDefault(BadAddressEventArgs.Default);
                return value;
            }
            else return null;
        });
    }

    public async static void StartUpdateSchedule()
    {
        logger.Info("Collecting IPs database from server...");

        badAddressesInfo.Clear();

        badAddressesInfo.Add(new BadAddressEventArgs()
        {
            Address = IPAddress.Parse("149.154.167.223"),
            Reason = BadAddressReason.Spammer,
            Message = "can be ignored"
        });

        await Task.Delay(TimeSpan.FromMinutes(5));
    }
}