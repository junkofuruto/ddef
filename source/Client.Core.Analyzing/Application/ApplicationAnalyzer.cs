using Client.Core.Logging;
using Client.Core.Monitoring.Utilities;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Client.Core.Analyzing.Application;

public static class ApplicationAnalyzer
{
    private static HashSet<int> ingorePorts = new HashSet<int>() { 443, 80 };
    private static Logger logger = new Logger("Analyzing.Application.ApplicationAnalyzer");

    public static List<ApplicationInformation> ApplicationInformationList { get; private set; } = new List<ApplicationInformation>();
    public static event BadApplicationEventHandler? OnBadApplication;

    public async static void Handler(AnalyzerEventArgs e)
    {
        try
        {
            var processes = ApplicationInformationList.Where(x => x.ProcessPorts.Contains(e.Packet.SourceEndPoint.Port));
            if (processes.Count() == 0 && ingorePorts.Contains(e.Packet.SourceEndPoint.Port) == false) UpdateApplicationInformation();
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
        }
    }

    public async static void UpdateApplicationInformation()
    {
        await
        Task.Run(() =>
        {
            logger.Info("Updating processes...");

            Process process = new Process();
            process.StartInfo.FileName = "netstat";
            process.StartInfo.Arguments = "-ano";
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

            ApplicationInformationList = processPorts.GroupBy(p => p.Id)
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
        });
    }

    private struct ProcessPort
    {
        public int Id;
        public int Port;
    }
}