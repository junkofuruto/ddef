using Client.Core.Logging;

namespace Client.Core.Analyzing.Application;

internal static class BadApplicationDataSerivce
{
    private static Logger logger = new Logger("Analyzing.Address.BadApplicationDataSerivce");
    private static HashSet<string> badApplications  = new HashSet<string>();
    private static List<BadApplicationEventArgs> badApplicationsInfo  = new List<BadApplicationEventArgs>();

    public async static Task<BadApplicationEventArgs?> Find(string app)
    {
        return await Task.Run(() =>
        {
            if (badApplications.Contains(app))
                return badApplicationsInfo.Where(a => a.Application == app).First();
            else return null;
        });
    }

    public async static void StartUpdateSchedule()
    {
        logger.Info("Collecting bad applications database from server...");

        //badApplications.Add("Telegram");
        //badApplicationsInfo.Add(new BadApplicationEventArgs("Telegram", "sniffer", "bad reputation"));

        await Task.Delay(TimeSpan.FromMinutes(5));
    }

}