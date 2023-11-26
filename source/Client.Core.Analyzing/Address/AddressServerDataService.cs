using Client.Core.Analyzing.DataAccess.Entities;
using Client.Core.Logging;
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

        badAddressesInfo = (await BadAddress.GetAllAsync(User.Current)).Select(x => new BadAddressEventArgs
        {
            Address = IPAddress.Parse(x.Host),
            Reason = x.Reason,
            Message = x.Message
        }).ToHashSet();

        await Task.Delay(TimeSpan.FromMinutes(5));
    }
}