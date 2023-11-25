using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Core.Analyzing.Application;

internal class ApplicationInformation
{
    public int Id { get; set; }
    public HashSet<int> ProcessPorts { get; set; } = null!;
    public string? ProcessName { get; set; }

    public override string ToString() => $"{Id}\t {ProcessName}:\t {string.Join(", ", ProcessPorts)}";
}
