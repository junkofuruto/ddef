using Client.Core.Logging;
using System.Buffers.Binary;
using System.Net;

namespace Client.Core.Analyzing.Address;

internal static class AddressServerDataService
{
    private static Logger logger = new Logger("Analyzing.Address.IPDatabaseCollector");
    private static HashSet<uint> ips = new HashSet<uint>();

    static AddressServerDataService()
    {
        UpdateBadAddressesList();
    }

    public static bool Find(uint ip) => ips.Contains(ip);

    public static bool Find(IPAddress address)
    {
        var addressBytes = address.GetAddressBytes();
        Array.Reverse(addressBytes);
        return Find(BinaryPrimitives.ReadUInt32LittleEndian(addressBytes));
    }

    public static void UpdateBadAddressesList()
    {
        logger.Info("Collecting IPs database from server...");
        //ips.Add(3232271942);
    }

    public async static Task<AddressInformation> GetAddressInformation(IPAddress address)
    {
        return new AddressInformation();
    }
}