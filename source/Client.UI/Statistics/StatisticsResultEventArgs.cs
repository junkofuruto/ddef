namespace Client.UI.Statistics;

public class StatisticsResultEventArgs
{
    public long PacketsRecieved { get; set; }
    public long BadAddressCauses { get; set; }
    public long BadApplicationsCauses { get; set; }
}