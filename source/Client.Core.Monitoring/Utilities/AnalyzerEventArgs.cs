using Client.Core.Monitoring.Protocol;

namespace Client.Core.Monitoring.Utilities;

public class AnalyzerEventArgs
{
    public Packet Packet { get; set; } = null!;
}
