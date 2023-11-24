using System;

namespace Client.UI.Statistics;

public record StatisticsTimestamp<T>
{
    public DateTime Timestamp { get; set; }
    public T? Value { get; set; }
}