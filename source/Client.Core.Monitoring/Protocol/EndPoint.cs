using System.Net;

namespace Client.Core.Monitoring.Protocol;

public class EndPoint
{
    public IPAddress Address { get; set; }
    public int Port { get; set; }

    public EndPoint(IPAddress address, int port)
    {
        Address = address;
        Port = port;
    }

    public override string ToString()
    {
        return $"{Address}:{Port}";
    }
}