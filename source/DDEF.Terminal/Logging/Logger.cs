namespace DDEF.Terminal.Logging;

public static class Logger
{
    private static object _lock = new object();

    public async static Task Log(string message, LogLevel level) => await Task.Run(() => Log(level.ToString().ToUpper().Substring(0, 3), message, (ConsoleColor)level));
    public async static Task Input(string message) => await Log(message, LogLevel.User);
    public async static Task Info(string message) => await Log(message, LogLevel.Info);
    public async static Task Warning(string message) => await Log(message, LogLevel.Warning);
    public async static Task Error(string message) => await Log(message, LogLevel.Error);
    public async static Task Debug(string message) => await Log(message, LogLevel.Debug);

    private static void Log(string level, string message, ConsoleColor outputColor)
    {
        lock (_lock)
        {
            var timestamp = DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]");

            Console.ForegroundColor = outputColor;
            Console.WriteLine($"\u001b[1m{timestamp} {level}: {message}\u001b[0m");
            Console.ResetColor();
        }
    }
}