using Client.Core.Analyzing.DataAccess.Entities;
using Client.Core.Logging;

namespace Client.Core.Analyzing.Application;

internal static class BadApplicationDataSerivce
{
    private static Logger logger = new Logger("Analyzing.Address.BadApplicationDataSerivce");
    private static List<BadApplicationEventArgs> badApplicationsInfo  = new List<BadApplicationEventArgs>();

    public async static Task<BadApplicationEventArgs?> Find(string app)
    {
        return await Task.Run(() =>
        {
            var apps = badApplicationsInfo.Where(a => a.Application == app);
            if (apps.Any()) return badApplicationsInfo.Where(a => a.Application == app).First();
            else return null;
        });
    }

    public async static void StartUpdateSchedule()
    {
        logger.Info("Collecting bad applications database from server...");
        var collectedData = await BadApplication.GetBadApplications(User.Current);
        badApplicationsInfo.AddRange(collectedData.Select(x => new BadApplicationEventArgs(x.Name, x.Reason, x.Message)));
        await Task.Delay(TimeSpan.FromMinutes(5));
    }
}