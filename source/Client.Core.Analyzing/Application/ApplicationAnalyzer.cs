using Client.Core.Logging;
using Client.Core.Monitoring.Utilities;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Client.Core.Analyzing.Application;

public static class ApplicationAnalyzer
{
    private static Logger logger = new Logger("Analyzing.Application.ApplicationAnalyzer");
    private static HashSet<int> ingorePorts = new HashSet<int>();
    private static List<ApplicationInformation> applicationInformationList = new List<ApplicationInformation>();

    public static event BadApplicationEventHandler? OnBadApplication;

    public static void Configure()
    {
        logger.Info("Configuration...");
        BadApplicationDataSerivce.StartUpdateSchedule();
    }

    public async static void Handler(AnalyzerEventArgs e)
    {
        try
        {
            var sourcePort = e.Packet.SourceEndPoint.Port;
            var matchingProcesses = applicationInformationList.Where(x => x.ProcessPorts.Contains(sourcePort)).ToList();

            if (matchingProcesses.Any())
            {
                var matchingProcessNames = string.Join("; ", matchingProcesses.Select(x => $"{x.Id} {x.ProcessName}"));
                logger.Info(matchingProcessNames);

                foreach (var process in matchingProcesses.Where(x => x.ProcessName != null))
                {
                    var info = await BadApplicationDataSerivce.Find(process.ProcessName!);
                    if (info != null && OnBadApplication != null) OnBadApplication.Invoke(info);
                }
            }

            if (!matchingProcesses.Any() && !ingorePorts.Contains(sourcePort))
            {
                await UpdateApplicationInformation(sourcePort);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
        }
    }

    private async static Task UpdateApplicationInformation(int issuer)
    {
        await
        Task.Run(() =>
        {
            logger.Info($"{issuer} updating processes");
            ingorePorts.Add(issuer);

            Process process = new Process();
            process.StartInfo.FileName = "netstat";
            process.StartInfo.Arguments = "-ano";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            Regex regex = new Regex(@":(\d+)\s+.+\s+(\d+)", RegexOptions.Compiled);
            MatchCollection matches = regex.Matches(output);

            var processPorts = new List<ProcessPort>();
            var newApplicationInformationList = new List<ApplicationInformation>();

            foreach (Match match in matches)
            {
                var port = Convert.ToInt32(match.Groups[1].Value);
                var pid = Convert.ToInt32(match.Groups[2].Value);

                if (ingorePorts.Contains(port) == false) processPorts.Add(new ProcessPort { Id = pid, Port = port });
            }

            var startCount = applicationInformationList.Count;

            applicationInformationList = processPorts.GroupBy(p => p.Id)
                .Select(g =>
                {
                    try
                    {
                        return new ApplicationInformation
                        {
                            Id = g.Key,
                            ProcessPorts = new HashSet<int>(g.Select(p => p.Port)),
                            ProcessName = Process.GetProcessById(g.Key).ProcessName
                        };
                    }
                    catch 
                    { 
                        return null;
                    }
                }).Where(x => x != null).ToList()!;
            logger.Info($"Updating done, collected {applicationInformationList.Count - startCount} application");
        });
    }

    private struct ProcessPort
    {
        public int Id;
        public int Port;
    }
}