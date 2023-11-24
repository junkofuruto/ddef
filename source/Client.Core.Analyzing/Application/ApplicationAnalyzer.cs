using Client.Core.Logging;
using Client.Core.Monitoring.Utilities;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Client.Core.Analyzing.Application;

public static class ApplicationAnalyzer
{
    private static Logger logger = new Logger("Analyzing.Application.ApplicationAnalyzer");
    private static HashSet<int> ingorePorts = new HashSet<int>();
    private static HashSet<string> badApplicationList = new HashSet<string>();
    private static List<ApplicationInformation> applicationInformationList = new List<ApplicationInformation>();

    public static event BadApplicationEventHandler? OnBadApplication;

    static ApplicationAnalyzer()
    {
        logger.Info("Loading...");
    }

    public async static void Handler(AnalyzerEventArgs e)
    {
        try
        { 
            var processes = applicationInformationList.Where(x => x.ProcessPorts.Contains(e.Packet.SourceEndPoint.Port));
            if (processes.Count() > 0)
            {
                logger.Info(string.Join("; ", applicationInformationList.Where(x => x.ProcessPorts.Contains(e.Packet.SourceEndPoint.Port)).Select(x => $"{x.Id} {x.ProcessName}")));
                SearchBadApplications(processes);
            }
            if (processes.Count() == 0 && ingorePorts.Contains(e.Packet.SourceEndPoint.Port) == false) await UpdateApplicationInformation(e.Packet.SourceEndPoint.Port);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
        }
    }

    private async static void SearchBadApplications(IEnumerable<ApplicationInformation> issuers)
    {

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
            logger.Info($"Updating done, collected {applicationInformationList.Count} application");
        });
    }

    private struct ProcessPort
    {
        public int Id;
        public int Port;
    }
}