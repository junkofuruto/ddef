using System.Net;

namespace Client.Core.Analyzing.Address;

public class BadAddressEventArgs
{
    public static readonly BadAddressEventArgs Default = new BadAddressEventArgs()
    {
        Address = null,
    };

    public IPAddress? Address { get; set; }
    public string Reason { get; set; }
    public string? Message { get; set; }
}