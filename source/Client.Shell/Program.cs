using Client.Core.Analyzing.Address;
using Client.Core.Analyzing.Application;
using Client.Core.Monitoring;

CancellationTokenSource MonitoringServerCancellationToken = new CancellationTokenSource();

AddressAnalyzer.Configure();
ApplicationAnalyzer.Configure();

MonitoringServer.PacketHandler += ApplicationAnalyzer.Handler;
MonitoringServer.PacketHandler += AddressAnalyzer.Handler;

await MonitoringServer.Start("Ethernet 2", MonitoringServerCancellationToken.Token);

//using System.Buffers.Binary;
//using System.Net;

//IPAddress address = IPAddress.Parse("149.154.167.51");
//Console.WriteLine(BinaryPrimitives.ReadUInt32BigEndian(address.GetAddressBytes()));