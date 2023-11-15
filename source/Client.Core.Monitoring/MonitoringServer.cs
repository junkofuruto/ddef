using Client.Core.Logging;
using Client.Core.Monitoring.Protocol;
using Client.Core.Monitoring.Utilities;
using System.Net.Sockets;
using System.Net;

namespace Client.Core.Monitoring;

public static class MonitoringServer
{
    private static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, System.Net.Sockets.ProtocolType.IP);
    private static Logger logger = new Logger("Monitoring.MonitoringServer");
    private static readonly byte[] optionInValue = { 1, 0, 0, 0 };

    public static event AnalyzerEventHandler? OnPacketIncoming;
    public static NetworkConfiguration? NetworkConfiguration { get; private set; }

    public async static Task Start(string networkName, CancellationToken cancellationToken)
    {
        await 
        Task.Run(() =>
        {
            NetworkConfiguration = NetworkConfiguration.Create(networkName);
            if (NetworkConfiguration == null) return;

            logger.Info($"Server IOControl binded to address {NetworkConfiguration.LocalAddress?.ToString()}");
            logger.Info($"Creating monitoring server with default gateway {NetworkConfiguration.GatewayAddress?.ToString()}...");

            socket.Bind(new IPEndPoint(NetworkConfiguration.LocalAddress!, 0));
            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, true);
            socket.IOControl(IOControlCode.ReceiveAll, optionInValue, null);

            CleanupSchedule(cancellationToken);

            var buffer = new byte[socket.ReceiveBufferSize];

            while (cancellationToken.IsCancellationRequested == false)
            {
                socket.Receive(buffer, buffer.Length, SocketFlags.None);
                var packet = Packet.Parse(buffer);
                packet.Log();
                if (OnPacketIncoming != null && packet != null) OnPacketIncoming.Invoke(new AnalyzerEventArgs
                {
                    Packet = Packet.Parse(buffer)
                });
            }

            socket.Close();
        });
    }

    private static void CleanupSchedule(CancellationToken cancellationToken)
    {
        Task.Run(() =>
        {
            while (cancellationToken.IsCancellationRequested == false)
            {
                logger.Info("Cleaning up socket leaked memory");
                GC.Collect();
                Task.Delay(2000).Wait();
            }
        }, cancellationToken);
    }
}