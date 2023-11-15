using System.Net;

namespace Client.Core.Analyzing.Address;

public class BadAddressEventArgs
{
    public IPAddress Address { get; set; } = null!;
    public BadAddressSource Source { get; set; }
    public BadAddressReason Reason { get; set; }
    public string? Message { get; set; }
}