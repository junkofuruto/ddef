using DDEF.Terminal.Logging;

namespace DDEF.Terminal;

internal class Program
{
    public async static Task Main(string[] args)
    {
        await Logger.Info("Starting DDEF...");
    }
}