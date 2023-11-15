using Client.Core.Logging;
using System.Net;

namespace Client.Core.Monitoring.Protocol;

public class Packet
{
    private readonly Logger logger = new Logger("Protocol.Packet");

    public ProtocolVersion Version { get; private set; }
    public byte HeaderLength { get; private set; }
    public ushort TotalLength { get; private set; }
    public ushort Identification { get; private set; }
    public bool ReservedFlag { get; private set; }
    public bool DonNotFragmentFlag { get; private set; }
    public bool MoreFragmentsFlag { get; private set; }
    public byte FragmentOffset { get; private set; }
    public byte TimeToLive { get; private set; }
    public ProtocolType ProtocolType { get; private set; }
    public ushort HeaderChecksum { get; private set; }
    public EndPoint SourceEndPoint { get; private set; } = null!;
    public EndPoint DestinationEndPoint { get; private set; } = null!;
    public Memory<byte> Data { get; private set; }

    private Packet()
    {
    }

    public static Packet Parse(ReadOnlySpan<byte> buffer)
    {
        var packet = new Packet();

        packet.Version = (ProtocolVersion)(buffer[0] >> 4);
        packet.HeaderLength = (byte)(buffer[0] & 0x0F);
        packet.TotalLength = (ushort)(buffer[2] << 8 | buffer[3]);
        packet.Identification = (ushort)(buffer[4] << 8 | buffer[5]);
        packet.ReservedFlag = (buffer[6] & 0x80) != 0;
        packet.DonNotFragmentFlag = (buffer[6] & 0x40) != 0;
        packet.MoreFragmentsFlag = (buffer[6] & 0x20) != 0;
        packet.FragmentOffset = (byte)(buffer[6] & 0x1F);
        packet.TimeToLive = buffer[8];
        packet.ProtocolType = (ProtocolType)buffer[9];
        packet.HeaderChecksum = (ushort)(buffer[10] << 8 | buffer[11]);

        IPAddress sourceIpAddress = new IPAddress(buffer.Slice(12, 4));
        ushort sourcePort = (ushort)((buffer[20] << 8) | buffer[21]);
        packet.SourceEndPoint = new EndPoint(sourceIpAddress, sourcePort);

        IPAddress destinationIpAddress = new IPAddress(buffer.Slice(16, 4));
        ushort destinationPort = (ushort)((buffer[22] << 8) | buffer[23]);
        packet.DestinationEndPoint = new EndPoint(destinationIpAddress, destinationPort);

        int dataLength = packet.TotalLength - packet.HeaderLength * 4;
        packet.Data = new byte[dataLength];
        buffer.Slice(packet.HeaderLength * 4, dataLength).CopyTo(packet.Data.Span);

        return packet;
    }

    public static Packet ParseAndLog(ReadOnlySpan<byte> byteArray)
    {
        var packet = Parse(byteArray);
        packet.Log();
        return packet;
    }

    public void Log()
    {
        logger.Packet($"{TotalLength.ToString().PadLeft(4, '0')} B " +
                      $"| {ProtocolType} " +
                      $"{SourceEndPoint?.ToString().PadRight(21)} " +
                      $"> {DestinationEndPoint?.ToString().PadRight(21)} ");
    }
}