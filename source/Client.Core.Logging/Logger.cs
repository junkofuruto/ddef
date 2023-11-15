namespace Client.Core.Logging;

public sealed class Logger
{
    private string name;

    public Logger(string name)
    {
        this.name = name;
    }

    private void Log(string message, string prefix)
    {
        Console.WriteLine($"{prefix} [{DateTime.Now.ToString("HH:mm:ss.fff")}] {name}: {message}");
    }

    public void Info(string message) => Log(message, "INF");
    public void Warning(string message) => Log(message, "WRN");
    public void Error(string message) => Log(message, "ERR");
    public void Packet(string message) => Log(message, "PCK");
}