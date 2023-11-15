using Client.Core.Monitoring.Protocol;

namespace Client.Core.Analyzing.Application;

public class BadApplicationEventArgs
{
    public Packet Packet { get; set; }
    public string Application { get; set; }
    public string Reason { get; set; }
    public string? Message { get; set; }

    public BadApplicationEventArgs(Packet packet, string application, string reason)
    {
        Packet = packet;
        Application = application;
        Reason = reason;
    }
    public BadApplicationEventArgs(Packet packet, string application, string reason, string message)
    {
        Packet = packet;
        Application = application;
        Reason = reason;
        Message = message;
    }
}
