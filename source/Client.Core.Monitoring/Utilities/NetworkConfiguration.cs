using Client.Core.Logging;
using System.Net;
using System.Net.NetworkInformation;

namespace Client.Core.Monitoring.Utilities;

public class NetworkConfiguration
{
    private static Logger logger = new Logger("Utilities.NetworkConfiguration");

    public IPAddress? LocalAddress { get; private set; }
    public IPAddress? GatewayAddress { get; private set; }

    private NetworkConfiguration() { }

    public static NetworkConfiguration? Create(string networkName)
    {
        logger.Info($"Creating network configuration - {networkName}");
        NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

        foreach (NetworkInterface networkInterface in interfaces)
        {
            if (networkInterface.Name == networkName)
            {
                IPInterfaceProperties ipProperties = networkInterface.GetIPProperties();

                foreach (var gateway in ipProperties.GatewayAddresses)
                {
                    if (gateway.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        var ipAddress = ipProperties.UnicastAddresses.FirstOrDefault(ua => ua.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?.Address;
                        var defaultGateway = gateway.Address;

                        if (ipAddress != null)
                        {
                            return new NetworkConfiguration
                            {
                                LocalAddress = ipAddress,
                                GatewayAddress = defaultGateway
                            };
                        }
                    }
                }
            }
        }

        logger.Error($"Unable to create network configuration");
        return null;
    }

    public static NetworkInterface[] GetAllInterfaces() => NetworkInterface.GetAllNetworkInterfaces();
}