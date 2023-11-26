using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Core.Analyzing.Address;
using Client.Core.Analyzing.Application;
using Client.Core.Analyzing.DataAccess.Entities;
using Client.Core.Monitoring;
using Client.Core.Monitoring.Utilities;

namespace Client.UI.Statistics;

public static class StatisticsCollector
{
    public static TimeSpan TimeskipSchedulePause { get; private set; } = TimeSpan.FromSeconds(5);

    public static List<StatisticsTimestamp<long>> PacketsRecieved { get; private set; } = new List<StatisticsTimestamp<long>>();
    public static List<StatisticsTimestamp<long>> BadAddressCauses { get; private set; } = new List<StatisticsTimestamp<long>>();
    public static List<StatisticsTimestamp<long>> BadApplicationsCauses { get; private set; } = new List<StatisticsTimestamp<long>>();

    public static event StatisticsResultEventHandler? OnStatisticsTimeskipSchedule;

    public static void Configure()
    {
        ReportSendingSchedule();
        StatisticsTimeskipSchedule();

        MonitoringServer.PacketHandler += HandlePacket;
        AddressAnalyzer.OnBadAddress += HandleBadAddress;
        ApplicationAnalyzer.OnBadApplication += HandleBadApplication;
    }

    private async static void ReportSendingSchedule()
    {
        await Task.Delay(TimeSpan.FromMinutes(5));

        var packets = PacketsRecieved.Where(x => x.Timestamp > DateTime.Now.AddMinutes(-5)).Select(x => x.Value).Sum();
        var addresses = BadAddressCauses.Where(x => x.Timestamp > DateTime.Now.AddMinutes(-5)).Select(x => x.Value).Sum();
        var apps = BadApplicationsCauses.Where(x => x.Timestamp > DateTime.Now.AddMinutes(-5)).Select(x => x.Value).Sum();

        if (User.Current != null)
            await User.Current.ReportAsync(packets, addresses, apps);
    }

    private async static void StatisticsTimeskipSchedule()
    {
        while (true)
        {
            PacketsRecieved.Add(new StatisticsTimestamp<long> { Timestamp = DateTime.Now, Value = 0 });
            BadAddressCauses.Add(new StatisticsTimestamp<long> { Timestamp = DateTime.Now, Value = 0 });
            BadApplicationsCauses.Add(new StatisticsTimestamp<long> { Timestamp = DateTime.Now, Value = 0 });

            await Task.Delay(TimeskipSchedulePause);

            if (OnStatisticsTimeskipSchedule != null)
                OnStatisticsTimeskipSchedule.Invoke(new StatisticsResultEventArgs
                {
                    PacketsRecieved = PacketsRecieved.Last().Value,
                    BadAddressCauses = BadAddressCauses.Last().Value,
                    BadApplicationsCauses = BadApplicationsCauses.Last().Value,
                });
        }
    }
    private static void HandlePacket(AnalyzerEventArgs e) => PacketsRecieved.Last().Value += 1;
    private static void HandleBadAddress(BadAddressEventArgs e) => BadAddressCauses.Last().Value += 1;
    private static void HandleBadApplication(BadApplicationEventArgs e) => BadApplicationsCauses.Last().Value += 1;
}