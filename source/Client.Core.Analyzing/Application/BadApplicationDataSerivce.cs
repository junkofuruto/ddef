using Client.Core.Analyzing.DataAccess.Entities;
using Client.Core.Logging;

namespace Client.Core.Analyzing.Application;

internal static class BadApplicationDataSerivce
{
    private static Logger logger = new Logger("Analyzing.Address.BadApplicationDataSerivce");
    private static HashSet<BadApplicationEventArgs> badApplicationsInfo  = new HashSet<BadApplicationEventArgs>();

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
        badApplicationsInfo = (await BadApplication.GetAllAsync(User.Current)).Select(x => new BadApplicationEventArgs
        {
            Application = x.Name,
            Reason = x.Reason,
            Message = x.Message
        }).ToHashSet();
        await Task.Delay(TimeSpan.FromMinutes(5));
    }
}